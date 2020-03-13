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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfApplication.Elements;
using WpfApplication.Elements.MenuElements;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MENU_ELEMENT_WIDTH = 50;
        private const int MENU_ELEMENT_HEIGHT = 50;

        public MainWindow()
        {
            InitializeComponent();

            ImageController.Init();
            UIController.Init(this);

            var elem = new MenuComputerElement("", MENU_ELEMENT_WIDTH, MENU_ELEMENT_HEIGHT, ImageController.Open("computer.svg"));
            elem.MouseDown += MenuItemMouseDown;

            Menu.Children.Add(elem);

            var elem1 = new MenuRouterElement("", MENU_ELEMENT_WIDTH, MENU_ELEMENT_HEIGHT, ImageController.Open("router.svg"));
            elem1.MouseDown += MenuItemMouseDown;

            Menu.Children.Add(elem1);
        }

        public Canvas GetMainPanel()
        {
            return Scheme;
        }

        private void MenuItemMouseDown(object sender, MouseButtonEventArgs e)
        {
            var elem = (BaseElement)sender;

            DragDrop.DoDragDrop(elem, elem, DragDropEffects.Copy);
        }

        private void NewItemDropped(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(MenuComputerElement)) is MenuComputerElement)
            {
                var point = e.GetPosition(GetMainPanel());
                Scheme.Children.Add(new ComputerElement(50, 50, ImageController.Open("computer.svg"), SchemeElementMouseDown, point.X - 25, point.Y - 25, 1));
            }
            else if (e.Data.GetData(typeof(MenuRouterElement)) is MenuRouterElement)
            {
                var point = e.GetPosition(GetMainPanel());
                Scheme.Children.Add(new RouterElement(50, 50, ImageController.Open("router.svg"), SchemeElementMouseDown, point.X - 25, point.Y - 25, 8));
            }
            else if (e.Data.GetData(typeof(ComputerElement)) is ComputerElement)
            {
                var elem = ((ComputerElement)(e.Data.GetData(typeof(ComputerElement))));
                var point = e.GetPosition(GetMainPanel());
                MoveElement(elem, point);
            }
            else if (e.Data.GetData(typeof(RouterElement)) is RouterElement)
            {
                var elem = ((RouterElement)(e.Data.GetData(typeof(RouterElement))));
                var point = e.GetPosition(GetMainPanel());
                MoveElement(elem, point);
            }
        }

        private void MoveElement(BaseElement elem, Point point)
        {
            elem.SetLocation(point.X, point.Y);

            Scheme.Dispatcher.Invoke(delegate () { return; }, DispatcherPriority.Render);

            foreach (var child in Scheme.Children)
            {
                if (child is GraphEdge)
                {
                    var c = child as GraphEdge;
                    if (c.SourceElement == elem)
                    {
                        var sourceOffset = c.SourceCB.TransformToAncestor(c.SourceCB.Parent as Canvas).Transform(new Point(0, 0));

                        var sourcePoint = c.SourceCB.TransformToAncestor(GetMainPanel()).Transform(new Point(0, 0));

                        sourcePoint.X += sourceOffset.X;
                        sourcePoint.Y += sourceOffset.Y;

                        c.Source = sourcePoint;
                    }
                    else if (c.DestinationElement == elem)
                    {
                        var sourceOffset = c.DestinationCB.TransformToAncestor(c.DestinationCB.Parent as Canvas).Transform(new Point(0, 0));

                        var sourcePoint = c.DestinationCB.TransformToAncestor(GetMainPanel()).Transform(new Point(0, 0));

                        sourcePoint.X += sourceOffset.X;
                        sourcePoint.Y += sourceOffset.Y;

                        c.Destination = sourcePoint;
                    }
                }
            }
        }

        private void SchemeElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is IElement)
                {
                    if (sender is ComputerElement)
                    {
                        DragDrop.DoDragDrop(sender as ComputerElement, sender as ComputerElement, DragDropEffects.Move);
                    }
                    else if (sender is RouterElement)
                    {
                        DragDrop.DoDragDrop(sender as RouterElement, sender as RouterElement, DragDropEffects.Move);
                    }
                }
            }
            else
            {

            }
        }

        internal void DrawLine(Point p1, Point p2, IElement source, IElement destination, CheckBox SCB, CheckBox DCB)
        {
            var edge = new GraphEdge();
            edge.SourceElement = source;
            edge.DestinationElement = destination;
            edge.Source = p1;
            edge.Destination = p2;
            edge.SourceCB = SCB;
            edge.DestinationCB = DCB;
            Scheme.Children.Add(edge);
        }

        private void Scheme_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(MenuComputerElement)) as MenuComputerElement != null)
            {
                e.Effects = DragDropEffects.Copy;
            }
            else if (e.Data.GetData(typeof(MenuRouterElement)) as MenuRouterElement != null)
            {
                e.Effects = DragDropEffects.Copy;
            }
            else if (e.Data.GetData(typeof(ComputerElement)) as ComputerElement != null)
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
    }
}
