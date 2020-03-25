using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
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

        internal void RemoveUIElement(BaseUIElement element)
        {
            var removedEdges = new List<GraphEdge>();

            foreach (var child in GetMainPanel().Children)
            {
                if (child is GraphEdge)
                {
                    var c = child as GraphEdge;
                    if (c.SourceElement.Equals(element) || c.DestinationElement.Equals(element))
                    {
                        removedEdges.Add(c);
                    }
                }
            }

            removedEdges.ForEach(RemoveLink);

            GetMainPanel().Children.Remove(element);
        }

        internal void RemoveLink(GraphEdge edge)
        {
            GetMainPanel().Children.Remove(edge);
        }

        internal bool CheckLines(CheckBox checkBox)
        {
            foreach (var child in GetMainPanel().Children)
            {
                if (child is GraphEdge)
                {
                    var c = child as GraphEdge;

                    if (c.SourceCB.Equals(checkBox) || c.DestinationCB.Equals(checkBox))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Canvas GetMainPanel()
        {
            return Scheme;
        }

        private void MenuItemMouseDown(object sender, MouseButtonEventArgs e)
        {
            var elem = (BaseUIElement)sender;

            DragDrop.DoDragDrop(elem, elem, DragDropEffects.Copy);
        }

        private void ItemDropped(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(MenuComputerElement)) is MenuComputerElement)
            {
                var point = e.GetPosition(GetMainPanel());

                var computer = new Computer(point, ElementsController.GetNextId());
                ElementsController.AddElement(computer);

                GetMainPanel().Children.Add(new ComputerElement(50, 50, ImageController.Open("computer.svg"), SchemeElementMouseDown, point.X, point.Y, 1, computer));
            }
            else if (e.Data.GetData(typeof(MenuRouterElement)) is MenuRouterElement)
            {
                var point = e.GetPosition(GetMainPanel());

                var router = new Router(point, ElementsController.GetNextId());
                ElementsController.AddElement(router);

                GetMainPanel().Children.Add(new RouterElement(50, 50, ImageController.Open("router.svg"), SchemeElementMouseDown, point.X, point.Y, 8, router));
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

        private void MoveElement(BaseUIElement elem, Point point)
        {
            elem.SetLocation(point.X, point.Y);

            ElementsController.ChangeLocation(elem, point);

            GetMainPanel().Dispatcher.Invoke(delegate () { return; }, DispatcherPriority.Render);

            foreach (var child in GetMainPanel().Children)
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
                    if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                    {
                        (sender as BaseUIElement).TogglePins();
                    }
                    else
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
            }
            else
            {
                if (sender is IElement)
                {
                    new Settings(sender as IElement).ShowDialog();
                }
                else
                {
                    new Settings(sender as GraphEdge).ShowDialog();
                }
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
            GetMainPanel().Children.Add(edge);

            edge.MouseDown += SchemeElementMouseDown;
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

        private void SaveMenuClick(object sender, RoutedEventArgs e)
        {
            var elements = ElementsController.Elements;
            var links = ElementsController.Links;

            var xml = new XElement("DataStore", elements.Select(x => new XElement(x.GetType().ToString(),
                                                                       new XAttribute("Id", x.Id),
                                                                       new XAttribute("Name", x.Name),
                                                                       new XAttribute("Point", x.Point))),
                                                                       links.Select(x => new XElement("Link",
                                                                       new XAttribute("Element1", x.Element1.Id),
                                                                       new XAttribute("Element2", x.Element2.Id),
                                                                       new XAttribute("FirstPosition", x.FirstPosition),
                                                                       new XAttribute("SecondPosition", x.SecondPosition),
                                                                       new XAttribute("Length", x.Length)))
                                                                       );
            xml.Save(Directory.GetCurrentDirectory() + "save.txt");
        }

        private void NewMenuClick(object sender, RoutedEventArgs e)
        {
            GetMainPanel().Children.Clear();

            ElementsController.Clear();
        }

        private void OpenMenuClick(object sender, RoutedEventArgs e)
        {
            NewMenuClick(null, null);

            var text = File.ReadAllText(Directory.GetCurrentDirectory() + "save.txt");

            var str = XElement.Parse(text);

            try
            {
                str.Elements().ToList().ForEach(x =>
                {
                    if (x.Name.ToString().Contains("Computer"))
                    {
                        var point = x.Attribute("Point");
                        var val = point.Value.Split(new char[] { ';' });
                        var id = x.Attribute("Id").Value;
                        ElementsController.SetLastId(int.Parse(id));
                        var computer = new Computer(new Point(double.Parse(val[0]), double.Parse(val[1])), int.Parse(id))
                        {
                            Name = x.Attribute("Name").Value
                        };

                        ElementsController.AddElement(computer);
                        GetMainPanel().Children.Add(new ComputerElement(50, 50, ImageController.Open("computer.svg"), SchemeElementMouseDown, computer.Point.X, computer.Point.Y, 1, computer, computer.Name));
                    }
                    else if (x.Name.ToString().Contains("Router"))
                    {
                        var point = x.Attribute("Point");
                        var val = point.Value.Split(new char[] { ';' });
                        var id = x.Attribute("Id").Value;
                        ElementsController.SetLastId(int.Parse(id));
                        var router = new Router(new Point(double.Parse(val[0]), double.Parse(val[1])), int.Parse(id))
                        {
                            Name = x.Attribute("Name").Value
                        };

                        ElementsController.AddElement(router);
                        GetMainPanel().Children.Add(new RouterElement(50, 50, ImageController.Open("router.svg"), SchemeElementMouseDown, router.Point.X, router.Point.Y, 8, router, router.Name));
                    }
                    else
                    {
                        var length = int.Parse(x.Attribute("Length").Value);

                        var id1 = int.Parse(x.Attribute("Element1").Value);
                        var id2 = int.Parse(x.Attribute("Element2").Value);

                        var fp = int.Parse(x.Attribute("FirstPosition").Value);
                        var sp = int.Parse(x.Attribute("SecondPosition").Value);

                        var s = ElementsController.Elements.First(y => y.Id == id1);
                        var d = ElementsController.Elements.First(y => y.Id == id2);

                        ElementsController.AddLink(s, d, fp, sp, length);

                        BaseUIElement firstBaseElement = null;
                        BaseUIElement secondBaseElement = null;

                        for (var i = 0; i < GetMainPanel().Children.Count; i++)
                        {
                            if (GetMainPanel().Children[i] is BaseUIElement)
                            {
                                var el = GetMainPanel().Children[i] as BaseUIElement;
                                if (el.Element.Equals(s))
                                {
                                    firstBaseElement = el;
                                }
                                else if (el.Element.Equals(d))
                                {
                                    secondBaseElement = el;
                                }
                            }
                        }

                        var firstPin = firstBaseElement.GetPinByNumber(fp);
                        var secondPin = secondBaseElement.GetPinByNumber(sp);

                        var source = firstPin.GetCheckBox();
                        var destination = secondPin.GetCheckBox();

                        var sourceOffset = destination.TransformToAncestor(destination.Parent as Canvas).Transform(new Point(0, 0));
                        var destinationOffset = source.TransformToAncestor(source.Parent as Canvas).Transform(new Point(0, 0));

                        var sourcePoint = destination.TransformToAncestor(GetMainPanel()).Transform(new Point(0, 0));
                        sourcePoint.X += sourceOffset.X;
                        sourcePoint.Y += sourceOffset.Y;

                        var destinationPoint = source.TransformToAncestor(GetMainPanel()).Transform(new Point(0, 0));
                        destinationPoint.X += destinationOffset.X;
                        destinationPoint.Y += destinationOffset.Y;

                        DrawLine(destinationPoint, sourcePoint, firstPin.ParentElement, secondPin.ParentElement, source, destination);
                    }

                    GetMainPanel().Dispatcher.Invoke(delegate () { return; }, DispatcherPriority.Render);
                });
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки файла");
            }
        }
    }
}
