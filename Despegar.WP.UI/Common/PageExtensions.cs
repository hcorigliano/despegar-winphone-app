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
        #region ** DEV TOOLS EXTENSIONS **
        public static void CheckDeveloperTools(this Page page)
        {
           #if DEBUG
            CommandBar bar = page.BottomAppBar as CommandBar;

            if (bar != null) 
            {
                var btn = new AppBarButton() { Label = "Developer", Icon = new SymbolIcon(Symbol.Important) };
                btn.Click += DevToolsButton_Click;
                bar.PrimaryCommands.Add(btn);
            }
          #endif
        }

        private static void DevToolsButton_Click(object sender, RoutedEventArgs e)
        {
            ModalPopup popup = new ModalPopup(new DevTools());
            popup.Show();
        }
        #endregion

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