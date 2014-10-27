using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Despegar.WP.UI.Developer
{
    internal class GestureInputProcessor
    {
        GestureRecognizer gestureRecognizer;
        UIElement element;
        string FillColor = "fill1";
        string StrokeColor = "stroke1";

        public GestureInputProcessor(GestureRecognizer gr, UIElement target)
        {
            this.gestureRecognizer = gr;
            this.element = target;

            this.gestureRecognizer.GestureSettings =
                Windows.UI.Input.GestureSettings.Tap |
                Windows.UI.Input.GestureSettings.Hold | //hold must be set in order to recognize the press & hold gesture 
                Windows.UI.Input.GestureSettings.RightTap;

            // Set up pointer event handlers. These receive input events that are used by the gesture recognizer. 
            this.element.PointerCanceled += OnPointerCanceled;
            this.element.PointerPressed += OnPointerPressed;
            this.element.PointerReleased += OnPointerReleased;
            this.element.PointerMoved += OnPointerMoved;

            // Set up event handlers to respond to gesture recognizer output 
            this.gestureRecognizer.Tapped += OnTapped;
            this.gestureRecognizer.RightTapped += OnRightTapped;
        }

        void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            // Route teh events to the gesture recognizer 
            this.gestureRecognizer.ProcessDownEvent(args.GetCurrentPoint(this.element));
            // Set the pointer capture to the element being interacted with 
            this.element.CapturePointer(args.Pointer);
            // Mark the event handled to prevent execution of default handlers 
            args.Handled = true;
        }

        void OnPointerCanceled(object sender, PointerRoutedEventArgs args)
        {
            this.gestureRecognizer.CompleteGesture();
            args.Handled = true;
        }

        void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            this.gestureRecognizer.ProcessUpEvent(args.GetCurrentPoint(this.element));
            args.Handled = true;
        }

        void OnPointerMoved(object sender, PointerRoutedEventArgs args)
        {
            this.gestureRecognizer.ProcessMoveEvents(args.GetIntermediatePoints(this.element));
            args.Handled = true;
        }

        void OnTapped(object sender, TappedEventArgs e)
        {
            if (this.element is Shape) { UpdateFillColor(this.element as Shape); }
        }

        void OnRightTapped(object sender, RightTappedEventArgs e)
        {
            if (this.element is Shape) { UpdateStrokeColor(this.element as Shape); }
        }

        void UpdateFillColor(Shape shape)
        {
            if (this.FillColor == "fill1")
            {
                shape.Fill = new SolidColorBrush(Colors.Yellow);
                this.FillColor = "fill2";
            }
            else
            {
                shape.Fill = new SolidColorBrush(Colors.Aqua);
                this.FillColor = "fill1";
            }
        }

        void UpdateStrokeColor(Shape shape)
        {
            if (StrokeColor == "stroke1")
            {
                shape.Stroke = new SolidColorBrush(Colors.Red);
                StrokeColor = "stroke2";
            }
            else
            {
                shape.Stroke = new SolidColorBrush(Colors.Purple);
                StrokeColor = "stroke1";
            }
        }
    } 
}