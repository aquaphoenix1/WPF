using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication.Elements
{
    /// <summary>
    /// Interaction logic for BaseElement.xaml
    /// </summary>
    public partial class BaseElement : UserControl
    {
        public BaseElement(string name, int width, int height, DrawingImage picture, MouseButtonEventHandler onMouseDown)
        {
            InitializeComponent();

            Width = width;
            Height = height;

            ElementPicture.Source = picture;

            ElementPicture.Width = width;
            ElementPicture.Height = height;

            if (name != "")
            {
                ElementName.Visibility = System.Windows.Visibility.Visible;
                ElementName.Content = name;
            }

            if (onMouseDown != null)
            {
                MouseDown += onMouseDown;
            }
        }

        public void SetLocation(double x, double y)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
        }

        protected void AddPin(string name)
        {
            PinsPanel.Children.Add(new PinElement(name));
        }
    }
}
