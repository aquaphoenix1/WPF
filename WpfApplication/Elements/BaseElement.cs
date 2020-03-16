using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication.Elements
{
    public abstract class BaseElement
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public Point Point { get; set; }

        public BaseElement(Point point, int id)
        {
            Point = point;
            Id = id;
        }
    }
}
