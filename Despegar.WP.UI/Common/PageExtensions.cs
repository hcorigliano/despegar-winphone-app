using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Developer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Common
{
    public static class PageExtensions
    {
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

    }
}
