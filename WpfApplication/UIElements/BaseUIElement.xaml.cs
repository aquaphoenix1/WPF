using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication.Elements
{
    /// <summary>
    /// Interaction logic for BaseElement.xaml
    /// </summary>
    public partial class BaseUIElement : UserControl
    {
        public BaseElement Element { get; private set; }

        public BaseUIElement(string name, int width, int height, DrawingImage picture, MouseButtonEventHandler onMouseDown, BaseElement element)
        {
            InitializeComponent();

            Element = element;

            Width = width;
            Height = height;

            ElementPicture.Source = picture;

            ElementPicture.Width = width;
            ElementPicture.Height = height;

            if (name != "")
            {
                ElementName.Visibility = System.Windows.Visibility.Visible;
                ElementName.Content = name;
                Element.Name = name;
            }

            if (onMouseDown != null)
            {
                MouseDown += onMouseDown;
            }
        }

        internal void ChangeName(string text)
        {
            Element.Name = text;
            ElementName.Content = text;
        }

        public void SetLocation(double x, double y)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
        }

        protected void AddPin(string name, int number)
        {
            PinsPanel.Children.Add(new PinElement(name, this, number));
        }

        public WrapPanel GetPinsPanel()
        {
            return PinsPanel;
        }

        internal void TogglePins()
        {
            if (PinsPanel.Visibility == System.Windows.Visibility.Visible)
            {
                PinsPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                PinsPanel.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
