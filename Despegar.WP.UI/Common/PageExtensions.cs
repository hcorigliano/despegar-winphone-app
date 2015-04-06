using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Developer;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Despegar.WP.UI.Common
{
    public static class PageExtensions
    {
       
        // Dependecy Object Extension
        public static IEnumerable<T> FindVisualChildren<T>(this Page page, DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(page, child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}