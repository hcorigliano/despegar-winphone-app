using Despegar.Core.Business.Flight.Itineraries;
using Despegar.WP.UI.Model.Classes.Flights;
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
                if (value == null) return;

                //this.cheapest_price = value.cheapest_price;
                this.Currencies = value.currencies;
                
                FillItems(value.items);

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
        private List<BindableItem> _items;
        public List<BindableItem> Items {
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

        private void FillItems(List<Item> itemList)
        {
            if (Items == null)
            {
                Items = new List<BindableItem>();
            }
            Items.AddRange( (itemList.Select(il=> new BindableItem(il))).ToList() );
        }

        public void FillRoutedTemplate(Item item)
        {
            var _item = Items.FirstOrDefault(i=> item.id.Equals(i.id));
            if (_item != null)
            {
                _item.LinkFlightRoutes();
            }
        }

        public bool isValid()
        {
            throw new NotImplementedException();
        }
    }
}
