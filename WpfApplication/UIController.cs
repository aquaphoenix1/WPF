using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApplication.Elements;
using WpfApplication.Elements.MenuElements;

namespace WpfApplication
{
    static class UIController
    {
        private static MainWindow mainWindow { get; set; }

        public static void DrawLine(Point p1, Point p2, IElement source, IElement destination, CheckBox SCB, CheckBox DCB)
        {
            mainWindow.DrawLine(p1, p2, source, destination, SCB, DCB);
        }

        internal static void Init(MainWindow mainWindow)
        {
            UIController.mainWindow = mainWindow;
        }

        internal static bool CheckCheckBox(CheckBox checkBox)
        {
            return mainWindow.CheckLines(checkBox);
        }

        internal static Visual GetMainPanel()
        {
            return mainWindow.GetMainPanel();
        }

        internal static void RemoveElement(BaseUIElement element)
        {
            mainWindow.RemoveUIElement(element);
        }

        internal static void RemoveLink(GraphEdge elem)
        {
            mainWindow.RemoveLink(elem);
        }
    }
}
