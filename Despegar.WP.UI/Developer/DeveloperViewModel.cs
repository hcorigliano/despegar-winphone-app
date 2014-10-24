using Despegar.Core.Business;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;

namespace Despegar.WP.UI.Developer
{
    public class DeveloperViewModel : BindableBase
    {
        public Rect Viewport { get { return Window.Current.Bounds; } }

        // Service Mocks

        // Form automation

        // Other Tools
        public bool DesignGridEnabled { 
            get { return MetroGridHelper.IsVisible; }
            set
            {
                OnPropertyChanged("DesignGridEnabled");
                MetroGridHelper.IsVisible = value;
            }
        }
      
        public Color DesignGridColor
        {
            get { return MetroGridHelper.Color; }
            set
            {
                OnPropertyChanged("DesignGridColor");
                MetroGridHelper.Color = value;
            }
        }

        public List<double> OpacityOptions
        {
            get { return new List<double>() { .1, .2, .3, .4, .5, .6 , .7, .8, .9, 1 }; }
        }

        public double Opacity
        {
            get { return MetroGridHelper.Opacity; }

            set {
                MetroGridHelper.Opacity = value;
                OnPropertyChanged("Opacity");
            }
        }

        public ObservableCollection<MockOption> MockItems { get; set; }

        public DeveloperViewModel()
        {
            //this.MockItems = GlobalConfiguration.CoreContext;
            //Enum.GetValues(typeof(MockKey)).Cast<MockKey>().Select(x => x.ToString())
        }

    }
}
