using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApplication.Elements;
using WpfApplication.Elements.MenuElements;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private object element;
        private GraphEdge graphEdge;

        public Settings()
        {
            InitializeComponent();
        }

        public Settings(IElement element) : this()
        {
            graphEdge = null;

            this.element = element;

            TextBox.Text = (this.element as BaseUIElement).Element.Name;
        }

        public Settings(GraphEdge element) : this()
        {
            this.graphEdge = element;

            this.element = ElementsController.Links.Find(x => IsValidEdge(x, element));

            TextBox.Text = (this.element as Link).Length.ToString();
        }

        private bool IsValidEdge(Link link, GraphEdge edge)
        {
            var source = (edge.SourceElement as BaseUIElement).Element;
            var destination = (edge.DestinationElement as BaseUIElement).Element;

            return ((link.Element1.Equals(source) && link.Element2.Equals(destination)) || (link.Element1.Equals(destination) && link.Element2.Equals(source)));
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            if (element is BaseUIElement)
            {
                (element as BaseUIElement).ChangeName(TextBox.Text);
            }
            else if (element is Link)
            {
                (element as Link).Length = int.Parse(TextBox.Text);
            }

            Close();
        }

        private void buttonDeleteElement_Click(object sender, RoutedEventArgs e)
        {
            if (element is BaseUIElement)
            {
                var elem = element as BaseUIElement;
                ElementsController.RemoveElement(elem);
                UIController.RemoveElement(elem);
            }
            else
            {
                var elem = element as Link;

                ElementsController.RemoveLink(elem);

                UIController.RemoveLink(graphEdge);
            }

            Close();
        }
    }
}
