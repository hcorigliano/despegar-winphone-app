using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Despegar.WP.UI.Developer.Controls
{
    public sealed partial class DesingUnit : UserControl
    {
        public TextBlock InnerText { get { return InText; } }
        public Rectangle InnerRect{ get { return InRect; } }

        public DesingUnit()
        {
            this.InitializeComponent();
        }
    }
}
