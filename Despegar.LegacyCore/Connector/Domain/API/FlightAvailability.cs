using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{

    public class FlightAvailability : BaseResponse
    {
        public FlightAvailabilityItem flights { get; set; }
        public new FlightCheckoutMeta meta { get; set; }

        public FlightAvailabilityItem Item
        {
            get
            {
                return flights;
            }
        }
    }

    public class FlightCheckoutMeta : Meta
    {
        public string currencyCode { get; set; }
    }

    public class FlightAvailabilityItem
    {
        public FlightPaymentInfo paymentInfo { get; set; }
        public FlightPriceInfo priceInfo { get; set; }
        public List<FlightRoute> routes { get; set; }

        public void SetRoutesTypesAndSegmentsIndex ()
        {
            for (int i = 0; i < routes.Count; i++) {
                routes[i].Type = "TRAMO";
                routes[i].SetSegmentIndex();
            }
            if (routes.Count == 1 || routes.Count == 2) routes[0].Type = "IDA";
            if (routes.Count == 2) routes[1].Type = "VUELTA";
        }

        public List<FlightPaymentGroup> PayInOnePayment
        {
            get
            {
                if (paymentInfo.payInOnePayment != null)
                    return paymentInfo.payInOnePayment;

                List<FlightPaymentGroup> _payments = new List<FlightPaymentGroup>();

                foreach (var it in  paymentInfo.payments)
                {
                    if (it.installments.quantity == 1)
                    {
                        bool found = false;

                        foreach (var pay in _payments)
                        {
                            if (it.installments.quantity == pay.installments)
                            {
                                pay.payments.Add(it);
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            FlightPaymentGroup tmpGroup = new FlightPaymentGroup() { payments = new List<FlightPayment>() };
                            tmpGroup.installments = it.installments.quantity;
                            tmpGroup.payments.Add(it);
                            _payments.Add(tmpGroup);
                        }
                    }
                }

                if (_payments.Count > 0) _payments[0].Selected = true;
                paymentInfo.payInOnePayment = _payments;
                return _payments;
            }
        }

        public List<FlightPaymentGroup> PayWithoutInterest
        {
            get
            {
                if (paymentInfo.payWithoutInterest != null)
                    return paymentInfo.payWithoutInterest;

                List<FlightPaymentGroup> _payments = new List<FlightPaymentGroup>();

                foreach (var it in paymentInfo.payments)
                {
                    if (it.installments.quantity != 1 && it.cft == 0)
                    {
                        bool found = false;

                        foreach (var pay in _payments)
                        {
                            if (it.installments.quantity == pay.installments)
                            {
                                pay.payments.Add(it);
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            FlightPaymentGroup tmpGroup = new FlightPaymentGroup() { payments = new List<FlightPayment>() };
                            tmpGroup.installments = it.installments.quantity;
                            tmpGroup.payments.Add(it);
                            _payments.Add(tmpGroup);
                        }
                    }
                }

                paymentInfo.payWithoutInterest = _payments;
                return _payments;
            }
        }

        public List<FlightPaymentGroup> PayWithInterest
        {
            get
            {
                if (paymentInfo.payWithInterest != null)
                    return paymentInfo.payWithInterest;

                List<FlightPaymentGroup> _payments = new List<FlightPaymentGroup>();

                foreach (var it in paymentInfo.payments)
                {
                    if (it.cft > 0)
                    {
                        bool found = false;

                        foreach (var pay in _payments)
                        {
                            if (it.installments.quantity == pay.installments)
                            {
                                pay.payments.Add(it);
                                found = true;
                            }
                        }
             
                        if (!found)
                        {
                            FlightPaymentGroup tmpGroup = new FlightPaymentGroup() { payments = new List<FlightPayment>() };
                            tmpGroup.installments = it.installments.quantity;
                            tmpGroup.payments.Add(it);
                            _payments.Add(tmpGroup);
                        }
                    }
                }

                paymentInfo.payWithInterest = _payments;
                return _payments;
            }
        }

        public FlightPaymentGroup PayWithInterestProps
        {
            get
            {
                FlightPaymentGroup payment = new FlightPaymentGroup();
                List<FlightPaymentGroup> payments = PayWithInterest;
                List<string> installments = new List<string>();
                List<FlightPayment> creditCards = new List<FlightPayment>();
                string installm;

                if (payments.Count == 0)
                {
                    payment.Visible = "Collapsed";
                    return payment;
                }

                foreach (var pay in payments)
                {
                    installments.Add(pay.installments.ToString());

                    foreach (var cc in pay.payments)
                    {
                        creditCards.Add(cc);
                    }
                }

                List<string> distincts = installments.Distinct().ToList<string>();
                List<FlightPayment> credits = creditCards.GroupBy(cc => cc.cardCode).Select(group => group.First()).ToList<FlightPayment>();

                if (distincts.Count > 0)
                    installm = String.Join(", ", distincts.Take(distincts.Count - 1)) + " o " + distincts[distincts.Count - 1];
                else
                    installm = distincts[0];

                payment.Installments = installm;
                payment.payments = credits;
                payment.Visible = "Visible";

                return payment;
            }
        }
    }

    public class FlightPaymentInfo
    {
        public List<FlightPayment> payments { get; set; }

        public List<FlightPaymentGroup> payInOnePayment;
        public List<FlightPaymentGroup> payWithoutInterest;
        public List<FlightPaymentGroup> payWithInterest;
    }

    public class FlightPaymentGroup : AbstractDefinition
    {
        public string Installments { get; set; }
        public int installments { get; set; }
        public List<FlightPayment> payments { get; set; }


        public string Visible { get; set; }
        public bool Selected { get { return selected; } set { selected = value; NotifyPropertyChanged("Selected"); } }
        public bool selected;
    }


    public class FlightPayment : HotelCreditCard
    {
        public FlightPaymentInstallment installments { get; set; }
        public float interest { get; set; }
        public float cft { get; set; }
    }

    public class FlightPaymentInstallment
    {
        public float first { get; set; }
        public float others { get; set; }
        public int quantity { get; set; }
        public int QuantityMinusOne { get { return quantity - 1; } }
    }

    public class FlightPriceInfo
    {
        public FlightPriceInfoTotal total { get; set; }
        public FlightPriceInfoItem adults { get; set; }
        public FlightPriceInfoItem infants { get; set; }
        public FlightPriceInfoItem children { get; set; }

        public string Currency { get; set; }
    }

    public class FlightPriceInfoItem
    {
        public float baseFare { get; set; }
        public int quantity { get; set; }


        public float Total { get { return baseFare * quantity; } }
    }

    public class FlightPriceInfoTotal
    {
        public float charges { get; set; }
        public float fare { get; set; }
        public float taxes { get; set; }
        public float taxFee { get; set; }
    }

    public class FlightRoute
    {
        public string duration { get; set; }
        public List<FlightRouteSegment> segments { get; set; }


        public string Type { get; set; }

        public string From 
        {
            get 
            {
                if (segments.Count > 0)
                    return segments[0].departure.location;
                else return "";
            }
        }

        public string To
        {
            get
            {
                if (segments.Count > 0)
                    return segments[segments.Count -1].arrival.location;
                else return "";
            }
        }


        public void SetSegmentIndex()
        {
            for (int i = 0; i < segments.Count; i++)
                segments[i].Index = i + 1;
        }
    }

    public class FlightRouteSegment
    {
        public FlightRouteSegmentPart arrival { get; set; }
        public FlightRouteSegmentPart departure { get; set; }
        public int flightNumber { get; set; }
        public string  marketingCabinTypeDescription { get; set; }
        public string operatingCarrierCode { get; set; }
        public string operatingCarrierDescription { get; set; }
        public string duration { get; set; }

        // VM props
        public int Index { get; set; }
        public string FlightNumber { get { return operatingCarrierCode + flightNumber.ToString(); } }
    }

    public class FlightRouteSegmentPart
    {
        public string date { get; set; }
        public string timezone { get; set; }
        public string location { get; set; }
        public string locationDescription { get; set; }


        public DateTime DateTime { get { return DateTime.Parse(date); } }

        public string Date { get { return DateTime.ToString("dd MMM yyyy"); } }
        public string Hour { get { return DateTime.ToString("H:mm"); } }
    }
}