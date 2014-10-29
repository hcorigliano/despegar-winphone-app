using Despegar.Core.Business;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Classes;
using System;
using System.Linq;
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

        #region ** Service Mocks **
        public List<IGrouping<ServiceKey, MockOption>> MockGroups { get; set; }
        #endregion

        #region ** Other Tools **
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
        
        public double Opacity
        {
            get { return MetroGridHelper.Opacity; }

            set {
                MetroGridHelper.Opacity = value;
                OnPropertyChanged("Opacity");
            }
        }

        public List<double> OpacityOptions
        {
            get { return new List<double>() { .1, .2, .3, .4, .5, .6, .7, .8, .9, 1 }; }
        }
        #endregion

        public DeveloperViewModel()
        {
            // Load Mocks list
            var mocks = Mock.AllMocks
                .Select(x => new MockOption() { MockKey = x.MockID, ServiceKey = x.ServiceID, Name = x.MockID.ToString(), Enabled = GlobalConfiguration.CoreContext.IsMockEnabled(x.MockID) })
                .ToList();

            // Add "None" Option
            foreach (ServiceKey key in Enum.GetValues(typeof(ServiceKey)).Cast<ServiceKey>())
            {
                mocks.Add(new MockNoneOption() { ServiceKey = key });
            }

            MockGroups = mocks
                .GroupBy(x => x.ServiceKey)
                .OrderBy(g => g.Key)
                .ToList();                        
        }
    }
}
