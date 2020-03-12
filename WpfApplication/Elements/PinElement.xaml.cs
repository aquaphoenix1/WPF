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

namespace WpfApplication.Elements
{
    /// <summary>
    /// Interaction logic for PinElement.xaml
    /// </summary>
    public partial class PinElement : UserControl
    {
        public PinElement(string name)
        {
            InitializeComponent();

            PinEnterCheckBox.Content = name;
        }

        private void PinDrop(object sender, DragEventArgs e)
        {
            var elem = e.Data.GetData(typeof(CheckBox));

            if (elem is CheckBox)
            {
                var source = e.Source;

                if (!sender.Equals(elem))
                {
                    var sourcePoint = (source as CheckBox).PointToScreen(new Point(0d, 0d));
                    var destinationPoint = (elem as CheckBox).PointToScreen(new Point(0d, 0d));
                    UIController.DrawLine(sourcePoint, destinationPoint);
                }
            }
        }

        private void PinEnterCheckBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(sender as CheckBox, sender, DragDropEffects.Move);
        }
    }
}