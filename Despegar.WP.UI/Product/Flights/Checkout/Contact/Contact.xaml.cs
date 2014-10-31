using Despegar.Core.Business.Flight.BookingFields;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Flights.Checkout.Contact
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactData : Page
    {
       
        
        public ContactData()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void InStackPannel(StackPanel stackPannel)
        {

            foreach (var stackPannelChild in stackPannel.Children)
            {
                System.Type type = stackPannelChild.GetType();
                                
                try
                {                    
                    InStackPannel((StackPanel)stackPannelChild);
                }
                catch { }

                try
                {
                    InTextBox((TextBox)stackPannelChild);
                }
                catch { }

                try
                {
                    //dynamic result = InComboBox((ComboBox)stackPannelChild);
                    InComboBox((ComboBox)stackPannelChild);
                }
                catch { }

            }
        }

        //private dynamic InComboBox(ComboBox comboBox)
        //{
        //    throw new NotImplementedException();
        //}

        private void InComboBox(ComboBox comboBox)
        {
            var test = ((Option)comboBox.SelectedItem).value;
            int i = 1;
        }

        private void InTextBox(TextBox textBox)
        {            
            string texto = ((TextBox)textBox).Text;
            int i = 1;
        }

        private void Button_Click_Test(object sender, RoutedEventArgs e)
        {
            //dynamic result = new ExpandoObject();
            //InStackPannel(contact);

            int i = 1;

            
        }
    }
}
