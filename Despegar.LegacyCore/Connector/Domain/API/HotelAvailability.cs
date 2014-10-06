using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{

    public class HotelAvailability : BaseResponse
    {
        public List<HotelAvailabilityItem> availability { get; set; }
        public new HotelCheckoutMeta meta { get; set; }

        public HotelAvailabilityItem Item
        {
            get
            {
                if (availability[0] == null)
                    throw new NotImplementedException();

                return availability[0];
            }
        }
    }
    
    public class HotelCheckoutMeta : Meta
    {
        public string currencyCode { get; set; }
    }

    public class HotelAvailabilityItem
    {
        public int id { get; set; }
        public string suggestedRoom { get; set; }
        public string sessionTicket { get; set; }
        public string ticket { get; set; }
        
        public List<HotelRoomPackCluster> roomPackClusters { get; set; }
        public object suggestedPaymentId { get; set; }
        public HotelPaymentMethod paymentMethod { get; set; }

        public Hotel hotel { get; set; }
        public List<HotelRoom> rooms { get; set; }


        public string Currency { get; set; }

        public string SessionTicket { get { return sessionTicket; } }
        public Hotel Hotel { get { return hotel; } }
        public string SelectedRoom
        {
            get { return suggestedRoom; }
            set { suggestedRoom = value; }
        }

        public HotelRoom Room 
        { 
            get
            {
                HotelRoom room = new HotelRoom();
                for (int i = 0; i < rooms.Count; i++)
                    if (rooms[i].id == SelectedRoom)
                        room = rooms[i];
                return room;
            }
        }

        public List<HotelPayment> PayAtDestination
        { 
            get
            {
                List<HotelPayment> _payments = new List<HotelPayment>();
                for (int i = 0; i < paymentMethod.payments.Count; i++)
                    if (paymentMethod.payments[i].paymentType == "PayAtDestination")
                        for (int e = 0; e < paymentMethod.payments[i].rooms.Count; e++)
                            if (paymentMethod.payments[i].rooms[e].id == SelectedRoom)
                            {
                                if (paymentMethod.payments[i].id == int.Parse(suggestedPaymentId.ToString()))
                                    paymentMethod.payments[i].Selected = true;
                                _payments.Add(paymentMethod.payments[i]);
                            }
                return _payments;
            }
        }

        public List<HotelPayment> PayInOnePayment
        {
            get
            {
                List<HotelPayment> _payments = new List<HotelPayment>();
                for (int i = 0; i < paymentMethod.payments.Count; i++)
                    if (paymentMethod.payments[i].paymentType == "PayInOnePayment")
                        for (int e = 0; e < paymentMethod.payments[i].rooms.Count; e++)
                            if (paymentMethod.payments[i].rooms[e].id == SelectedRoom)
                            {
                                if (paymentMethod.payments[i].id == int.Parse(suggestedPaymentId.ToString()))
                                    paymentMethod.payments[i].Selected = true;
                                _payments.Add(paymentMethod.payments[i]);
                            }
                return _payments;
            }
        }

        public List<HotelPayment> PayWithoutInterest 
        { 
            get
            {
                List<HotelPayment> _payments = new List<HotelPayment>();
                for (int i = 0; i < paymentMethod.payments.Count; i++)
                    if (paymentMethod.payments[i].interest == 1 && paymentMethod.payments[i].paymentType == "PayInAdvance")
                        for (int e = 0; e < paymentMethod.payments[i].rooms.Count; e++)
                            if (paymentMethod.payments[i].rooms[e].id == SelectedRoom)
                            {
                                if (paymentMethod.payments[i].id == int.Parse(suggestedPaymentId.ToString()))
                                    paymentMethod.payments[i].Selected = true;
                                _payments.Add(paymentMethod.payments[i]);
                            }

                return _payments;
            }
        }

        public List<HotelPayment> PayWithInterest
        {
            get 
            {
                List<HotelPayment> _payments = new List<HotelPayment>();
                for (int i = 0; i < paymentMethod.payments.Count; i++)
                    if (paymentMethod.payments[i].interest > 1)
                        for (int e = 0; e < paymentMethod.payments[i].rooms.Count; e++)
                            if (paymentMethod.payments[i].rooms[e].id == SelectedRoom)
                            {
                                if (paymentMethod.payments[i].id == int.Parse(suggestedPaymentId.ToString()))
                                    paymentMethod.payments[i].Selected = true;
                                _payments.Add(paymentMethod.payments[i]);
                            }
                return _payments;
            }
        }

        public HotelPayment PayWithInterestProps
        {
            get
            {
                HotelPayment payment = new HotelPayment();
                List<HotelPayment> payments = PayWithInterest;
                List<string> installments = new List<string>();
                List<HotelCreditCard> creditCards = new List<HotelCreditCard>();
                string installm;

                if (payments.Count == 0)
                {
                    payment.Visible = "Collapsed";
                    return payment;
                }

                foreach (var pay in payments)
                {
                    installments.Add(pay.installmentQuantity.ToString());

                    foreach (var cc in pay.creditCards)
                     creditCards.Add(cc);
                }

                List<string> distincts = installments.Distinct().ToList<string>();
                List<HotelCreditCard> credits = creditCards.GroupBy(cc => cc.cardCode).Select(group => group.First()).ToList<HotelCreditCard>();

                if (distincts.Count > 0)
                    installm = String.Join(", ", distincts.Take(distincts.Count - 1)) + " o " + distincts[distincts.Count - 1];
                else
                    installm = distincts[0];

                payment.Installments = installm;
                payment.creditCards = credits;
                payment.Visible = "Visible";

                return payment;
            }
        }
    }

    public class HotelPaymentMethod {
        public List<HotelPayment> payments { get; set; }
    }

    public class HotelPayment : AbstractDefinition
    {
        public int id { get; set; }
        public float interest { get; set; }
        public string suggestedRoom { get; set; }
        public string paymentType { get; set; }
        public int installmentQuantity { get; set; }
        public List<HotelRoomPayment> rooms { get; set; }
        public List<HotelCreditCard> creditCards { get; set; }

        public string Visible { get; set; }
        public bool Selected { get { return selected; } set { selected = value; NotifyPropertyChanged("Selected"); } }
        public bool selected;

        public string Installments { get; set; }
    }

    public class HotelRoomPayment {
        public string id { get; set; }
        public float cft { get; set; }
        public HotelInstallments installments { get; set; }
    }

    public class HotelInstallments {
        public float first { get; set; }
        public float others { get; set; }
    }

    public class HotelCreditCard {
        public string cardCode { get; set; }
        public string cardDescription { get; set; }
    }

    public class HotelRoomPackCluster {
        public int clusterId { get; set; }
        public List<int> roomPackIds { get; set; }
    }


    public class Hotel {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string cityId { get; set; }
        public string address { get; set; }
        public int starRating { get; set; }
        public string pictureName { get; set; }


        public string Name { get { return name; } }
        public string Stars { get { return starRating.ToString(); } }
    }

    public class HotelRoom
    {
        public string id { get; set; }
        public string description { get; set; }
        public string regimeCode { get; set; }
        public string regimeDescription { get; set; }

        public List<HotelRoomDetail> roomsDetail { get; set; }
        public HotelPenalty penalty { get; set; }
        public HotelPrices prices { get; set; }


        public string Description { get { return description; } }
        public HotelPrices Prices { get { return prices; } }
        public HotelPenalty Penalty { get { return penalty; } }
        public string MealPlan { get { return regimeDescription; } }
    }

    public class HotelRoomDetail
    {
        public string description { get; set; }
        public int availableRooms { get; set; }
    }

    public class HotelPenalty
    {
        public int hours { get; set; }
        public string description { get; set; }
        public bool refundable { get; set; }
        public string refundType { get; set; }


        public string Cancelation
        {
            get
            {
                if (refundable && hours > 0) 
                    return String.Format("Cancelable hasta con {0}hs de anticipación", hours);
                
                else 
                    return "No reembolsable";
            }
        }
    }

    public class HotelPrices
    {
        public HotelTotalPrice totalPrice { get; set; }
        public HotelOriginalPrice originalPrice { get; set; }
        public HotelAveragePricePerNight averagePricesPerNight { get; set; }
        public float taxAtDestination { get; set; }
        public bool pricesWithoutTaxes { get; set; }


        public float Total { get { return totalPrice.totalPrice + totalPrice.extraTax; } }
        public float PricePerNight { get { return (totalPrice.priceWithoutTax / Nights / Rooms); } }
        public float PriceCalculated { get { return totalPrice.priceWithoutTax; } }
        public float Taxes { get { return totalPrice.taxes + totalPrice.serviceCharge;  } }
        public float TaxFee { get { return totalPrice.extraTax; } }
        public float FinancialCost { get { return totalPrice.financialCost; } }
        
        public int Nights { get; set; }
        public int Rooms { get; set; }
        public string Currency { get; set; }
    }

    public class HotelTotalPrice
    {
        public float extraTax { get; set; }
        public float totalPrice { get; set; }
        public float priceWithoutTax { get; set; }
        public float taxes { get; set; }
        public float serviceCharge { get; set; }
        public float financialCost { get; set; }
    }

    public class HotelOriginalPrice
    {
        //"extraTax": 605.96928,
        //"totalPrice": 2213.0364772174,
        //"priceWithoutTax": 1881.8337391304,
        //"taxes": 282.2750608696,
        //"serviceCharge": 48.9276772174,
        //"currency": "ARS"
    }

    public class HotelAveragePricePerNight
    {
        /*"avgPrice": {
            "totalPrice": 1247.6557690435002,
            "priceWithoutTax": 940.9168695652,
            "taxes": 141.1375304348
        },
        "avgDiscountPrice": {
            "totalPrice": 1247.6557690435002,
            "priceWithoutTax": 940.9168695652,
            "taxes": 141.1375304348
        }*/
    }
}