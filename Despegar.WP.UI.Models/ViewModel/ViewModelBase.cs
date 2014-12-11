using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel
{
    /// <summary>
    /// Provides the Base ViewModel for the Application
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Indicates whether the ViewModel is awaiting an operation to finish, so the View should display a Loading and block the user input.
        /// </summary>
        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }

        public delegate void ViewModelErrorHandler(object sender, ViewModelErrorArgs e);

        public event ViewModelErrorHandler ViewModelError;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnViewModelError(string errorCode)
        {
            if (ViewModelError != null)
                ViewModelError(this, new ViewModelErrorArgs(errorCode));
        }

        //protected void OnViewModelError(string errorCode, object parameters)
        //{
        //    if (ViewModelError != null)
        //        ViewModelError(this, new ViewModelErrorArgs(errorCode));
        //}

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}