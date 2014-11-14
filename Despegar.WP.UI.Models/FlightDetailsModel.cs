using Despegar.Core.Business.Flight.Itineraries;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public class FlightDetails : AppModelBase, IInitializeModelInterface, IValidateInterface
    {
        private Windows.ApplicationModel.Resources.ResourceLoader loader = new Windows.ApplicationModel.Resources.ResourceLoader();

        public Route outbound
        {
            set
            {
                this.Choice = value.choice;
                this.Duration   = value.duration;
                this.From   = value.from ;
                this.To   = value.to ;
                this.Departure   = value.departure ;
                this.Arrival   = value.arrival;
                this.Layovers   = value.layovers ;
                this.Segments = ToBindableSegmentList(value.segments, value.duration);
                
            }
        }

        public Route inbound
        { 
            set
            {
                this.Choice = value.choice;
                if (value.choice != -1)
                {
                    this.Choice = value.choice;
                    this.Duration = value.duration;
                    this.From = value.from;
                    this.To = value.to;
                    this.Departure = value.departure;
                    this.Arrival = value.arrival;
                    this.Layovers = value.layovers;
                    this.Segments = ToBindableSegmentList(value.segments, value.duration);
                }
            }
        }

        private List<BindableSegment> ToBindableSegmentList(List<Segment> listSegment , string duration)
        {
            List<BindableSegment> listBindable = new List<BindableSegment>();

            for(int i = 0 ; i < listSegment.Count() ; i++)
            {
                
                BindableSegment bindable = new BindableSegment();
                bindable.from = listSegment[i].from;
                bindable.to = listSegment[i].to;
                bindable.duration = listSegment[i].duration;
                bindable.airline = listSegment[i].airline;
                bindable.flight_id = listSegment[i].flight_id;
                bindable.departure_date = listSegment[i].departure_date;
                bindable.departure_time = listSegment[i].departure_time;
                bindable.arrival_date = listSegment[i].arrival_date;
                bindable.arrival_time = listSegment[i].arrival_time;
                bindable.cabin_type = listSegment[i].cabin_type;
                bindable.AirportCodeFromto = listSegment[i].from.code + " - " + listSegment[i].to.code;
                bindable.flightIdAndCabinType = loader.GetString("Flights_Flight") + listSegment[i].flight_id + " - " + listSegment[i].cabin_type;
                bindable.toInformation = loader.GetString("Flights_Arrives_In") + bindable.to.city;
                bindable.fromInformation = loader.GetString("Flights_Leaves") + bindable.from.city;

                if (listSegment[i].operated_by != null)
                {
                    bindable.operateBy = loader.GetString("Flights_Operated_By") + listSegment[i].operated_by.name;
                }
                if (i < listSegment.Count() - 1)
                {
                    //not is the last element
                    bindable.timeInformation = timeDiference(listSegment[i].arrival_time, listSegment[i].arrival_date, listSegment[i + 1].departure_time, listSegment[i + 1].departure_date);
                }
                else
                {
                    string[] durationTime = duration.Split(new Char[] { ':' });
                    bindable.timeInformation = loader.GetString("Flights_Duration") + durationTime[0] + "hs " + durationTime[1] + "m";
                }

                listBindable.Add(bindable);
            }
            return listBindable;
        }

        private string timeDiference(string arrival_time, string arrival_date, string departure_time, string departure_date)
        {
            string[] arrivalSplitTime = arrival_time.Split(new Char[] { ':' });
            string[] departureSplitTime = departure_time.Split(new Char[] { ':' });
            string[] arrivalSplitDate = arrival_date.Split(new Char[] { '-' });
            string[] departureSplitDate = departure_date.Split(new Char[] { '-' });
            DateTime arrival = new DateTime(Convert.ToInt32(arrivalSplitDate[0]), Convert.ToInt32(arrivalSplitDate[1]), Convert.ToInt32(arrivalSplitDate[2]), Convert.ToInt32(arrivalSplitTime[0]), Convert.ToInt32(arrivalSplitTime[1]), 0);
            DateTime departure = new DateTime(Convert.ToInt32(departureSplitDate[0]), Convert.ToInt32(departureSplitDate[1]), Convert.ToInt32(departureSplitDate[2]), Convert.ToInt32(departureSplitTime[0]), Convert.ToInt32(departureSplitTime[1]), 0);
            TimeSpan diference = departure.Subtract(arrival);
            return loader.GetString("Flights_Connect_with_pending") + diference.Hours + "hs " + diference.Minutes + "m";
        }


        private string _duration;
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                base.NotifyPropertyChanged("Duration");
            }
        }
        private Airport _from;
        public Airport From
            {
                get { return _from; }
            set
            {
                _from = value;
                base.NotifyPropertyChanged("From");
            }
        }
        private Airport _to;
        public Airport To
            {
                get { return _to; }
            set
            {
                _to = value;
                base.NotifyPropertyChanged("To");
            }
        }
        private string _departure;
        public string Departure
            {
                get { return _departure; }
            set
            {
                _departure = value;
                base.NotifyPropertyChanged("Departure");
            }
        }
        private string _arrival;
        public string Arrival
            {
                get { return _arrival; }
            set
            {
                _arrival = value;
                base.NotifyPropertyChanged("Arrival");
            }
        }
        private int _layovers; 
        public int Layovers
            {
                get { return _layovers; }
            set
            {
                _layovers = value;
                base.NotifyPropertyChanged("Layovers");
            }
        }
        
        private List<BindableSegment> _segments;
        public List<BindableSegment> Segments
        {
            get { return _segments; }
            set
            {
                _segments = value;
                base.NotifyPropertyChanged("Segments");
            }
        }

        private int _choice;
        public int Choice 
        {
            get { return _choice; }
            set {
                _choice = value;
                base.NotifyPropertyChanged("Choice");
            }
        }

        public bool HasSegments
        {
            get {
                    return (Segments != null);
                }
        }

        public new void InitializeModel()
        {
            base.InitializeModel();

        }

        public new void Validate()
        {
            base.Validate();

            //Validate each variable for this model
        }

        public bool isValid()
        {
            throw new NotImplementedException();
        }

    }
}
