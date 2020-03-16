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
using WpfApplication.Elements.MenuElements;

namespace WpfApplication.Elements
{
    /// <summary>
    /// Interaction logic for PinElement.xaml
    /// </summary>
    public partial class PinElement : UserControl
    {
        private IElement parentElement;
        public int Number { get; private set; }

        public PinElement(string name, BaseUIElement elem, int number)
        {
            InitializeComponent();

            PinEnterCheckBox.Content = name;

            parentElement = elem as IElement;

            Number = number;
        }

        private void PinDrop(object sender, DragEventArgs e)
        {
            var elem = e.Data.GetData(typeof(CheckBox));

            if (elem is CheckBox)
            {
                var source = elem as CheckBox;
                var destination = e.Source as CheckBox;

                var p1 = (((destination.Parent as Canvas).Parent) as PinElement).parentElement;
                var p2 = (((source.Parent as Canvas).Parent) as PinElement).parentElement;

                if (!sender.Equals(elem) && UIController.CheckCheckBox(destination as CheckBox) && !p1.Equals(p2))
                {
                    var sourceOffset = destination.TransformToAncestor(destination.Parent as Canvas).Transform(new Point(0, 0));
                    var destinationOffset = source.TransformToAncestor(source.Parent as Canvas).Transform(new Point(0, 0));

                    var sourcePoint = destination.TransformToAncestor(UIController.GetMainPanel()).Transform(new Point(0, 0));
                    sourcePoint.X += sourceOffset.X;
                    sourcePoint.Y += sourceOffset.Y;

                    var destinationPoint = source.TransformToAncestor(UIController.GetMainPanel()).Transform(new Point(0, 0));
                    destinationPoint.X += destinationOffset.X;
                    destinationPoint.Y += destinationOffset.Y;

                    UIController.DrawLine(destinationPoint, sourcePoint, ((source.Parent as Canvas).Parent as PinElement).parentElement, parentElement, source, destination);
                    
                    ElementsController.AddLink((((source.Parent as Canvas).Parent as PinElement).parentElement as BaseUIElement).Element, (((destination.Parent as Canvas).Parent as PinElement).parentElement as BaseUIElement).Element, ((source.Parent as Canvas).Parent as PinElement).Number, ((destination.Parent as Canvas).Parent as PinElement).Number);
                }
            }
        }

        private void PinEnterCheckBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool isFree = UIController.CheckCheckBox(sender as CheckBox);
            if (isFree)
            {
                DragDrop.DoDragDrop(sender as CheckBox, sender, DragDropEffects.Move);
            }
        }
    }
}