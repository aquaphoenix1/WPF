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

        public PinElement(string name, BaseElement elem)
        {
            InitializeComponent();

            PinEnterCheckBox.Content = name;

            parentElement = elem as IElement;
        }

        private void PinDrop(object sender, DragEventArgs e)
        {
            var elem = e.Data.GetData(typeof(CheckBox));

            if (elem is CheckBox)
            {
                var element = elem as CheckBox;
                var source = e.Source as CheckBox;

                if (!sender.Equals(elem))
                {
                    var sourceOffset = source.TransformToAncestor(source.Parent as Canvas).Transform(new Point(0, 0));
                    var destinationOffset = element.TransformToAncestor(element.Parent as Canvas).Transform(new Point(0, 0));

                    var sourcePoint = source.TransformToAncestor(UIController.mainWindow.GetMainPanel()).Transform(new Point(0, 0));
                    sourcePoint.X += sourceOffset.X;
                    sourcePoint.Y += sourceOffset.Y;

                    var destinationPoint = element.TransformToAncestor(UIController.mainWindow.GetMainPanel()).Transform(new Point(0, 0));
                    destinationPoint.X += destinationOffset.X;
                    destinationPoint.Y += destinationOffset.Y;

                    UIController.DrawLine(destinationPoint, sourcePoint, ((element.Parent as Canvas).Parent as PinElement).parentElement, parentElement, element, source);
                }
            }
        }

        private void PinEnterCheckBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(sender as CheckBox, sender, DragDropEffects.Move);
        }
    }
}