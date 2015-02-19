using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Despegar.WP.UI.Model.ViewModel
{
    /// <summary>
    /// Provides the Base ViewModel for the Application
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
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
        public IBugTracker BugTracker { get; set; }
        public INavigator Navigator { get; set; }

        public ViewModelBase(INavigator navigator, IBugTracker tracker) 
        {
            this.Navigator = navigator;
            this.BugTracker = tracker;
        }

        public abstract void OnNavigated(object navigationParams);

        protected void OnViewModelError(string errorCode)
        {
            if (ViewModelError != null)
                ViewModelError(this, new ViewModelErrorArgs(errorCode));
        }

        protected void OnViewModelError(string errorCode, object parameter)
        {
            BugTracker.LeaveBreadcrumb("ViewModel Error Raised: " + errorCode);

            if (ViewModelError != null)
                ViewModelError(this, new ViewModelErrorArgs(errorCode, parameter));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}