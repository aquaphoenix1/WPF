using System;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication.Elements.MenuElements;

namespace WpfApplication.Elements
{
    class ComputerElement : BaseElement, IElement
    {
        public ComputerElement(int width, int height, DrawingImage picture, MouseButtonEventHandler onMouseDown, double x, double y, int pinCount, string name = "Безымянный") : base(name, width, height, picture, onMouseDown)
        {
            SetLocation(x, y);

            for(var i = 0; i < pinCount; i++)
            {
                AddPin(string.Format("Pin {0}", i));
            }
        }
    }
}
