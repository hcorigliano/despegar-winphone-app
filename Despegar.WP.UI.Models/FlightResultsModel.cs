using Despegar.Core.Business.Flight.Itineraries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;


namespace Despegar.WP.UI.Model
{
    public class FlightResultsModel : AppModelBase, Interfaces.IInitializeModelInterface, Interfaces.IValidateInterface
    {
        public FlightsItineraries Itineraries
        {
            
            set
            {
                //TODO initialize all variables needed for this page.
                
                //this.cheapest_price = value.cheapest_price;
                this.Currencies = value.currencies;
                
                this.Items = value.items;
                this.Facets = value.facets;
                
                this.Sorting = value.sorting;
                this.Paging = value.paging;
             
            }
        }

        //TODO: create the correct class for cheapest_price
        //public IObservable<object> cheapest_price { get; set; }

        private Currencies _currencies;
        public Currencies Currencies {
            get { return _currencies; }
            set { 
                _currencies = value;
                base.NotifyPropertyChanged("Currencies");
            }
        }
        
        //This items represents flights inbounds and outbounds
        private List<Item> _items;
        public List<Item> Items {
            get { return _items; }
            set {
                _items = value;
                base.NotifyPropertyChanged("Items");
            }
        }

        private List<Facet> _facets { get; set; }
        public List<Facet> Facets {
            get {
                return _facets;
                }
            set {
                _facets = value;
                base.NotifyPropertyChanged("Facets");
                }
        }

        private Sorting _sorting;
        public Sorting Sorting {
            get { return _sorting; } 
            set{
                _sorting = value;
                base.NotifyPropertyChanged("Sorting");
                } 
        }

        private Paging _paging;
        public Paging Paging{
            get { return _paging; }
            set {   _paging = value;
                    base.NotifyPropertyChanged("Paging");     
            }
        }


        public FlightResultsModel()
        {
            this.InitializeModel();
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

    }
}
