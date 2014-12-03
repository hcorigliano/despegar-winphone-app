﻿using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.IService;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.Interfaces;
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
        
        public Core.IService.IFlightService flightService;

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

        public FlightResultsModel(INavigator navigator, IFlightService flightService)
        {
            // TODO: Complete member initialization

            this.Navigator = navigator;
            this.flightService = flightService;
           
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

            if (itemList != null)
            {
                //Items.AddRange((itemList.Select(il => new BindableItem(il))).ToList());
                var list = (itemList.Select(il => new BindableItem(il))).ToList();
                this.Items = list;
                //base.NotifyPropertyChanged("Items");
            }
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

        public List<Facet> SelectedFacets
        {
            get
            {
                var facetList = this._facets.Where(f => f.values.Any(fv => fv.selected == true));
                return facetList.ToList();
            }
        }

        public Value3 SelectedSorting
        {
            get
            {
                var selectedSortingList = this._sorting.values.FirstOrDefault(sr => sr.selected == true);
                return selectedSortingList;
            }
        }


        public string SelectedCurrency
        {
            get
            {
                if (this.Currencies == null) return String.Empty;

                var _cur = this.Currencies.values.Where(x => x.selected == true).FirstOrDefault();

                if (_cur == null)
                    return String.Empty;


                return ((Value)_cur).label;
            }
        }


        public void Clear()
        {
           
            //this.cheapest_price = value.cheapest_price;
            this.Currencies = null;

            if (this.Items!=null) 
                this.Items.Clear();

            if (this.Items!=null)
                this.Facets.Clear();

            this.Sorting = null;
            this.Paging = null;
        }

        public INavigator Navigator { get; set; }
    }
}
