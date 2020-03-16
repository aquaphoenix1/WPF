using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication.Elements.MenuElements
{
    class MenuComputerElement : BaseUIElement
    {
        public MenuComputerElement(string name, int width, int height, DrawingImage picture) : base(name, width, height, picture, null, null)
        {
        }
    }
}
