using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApplication.Elements.MenuElements;

namespace WpfApplication
{
    static class UIController
    {
        public static MainWindow mainWindow { get; private set; }

        public static void DrawLine(Point p1, Point p2, IElement source, IElement destination, CheckBox SCB, CheckBox DCB)
        {
            mainWindow.DrawLine(p1, p2, source, destination, SCB, DCB);
        }

        internal static void Init(MainWindow mainWindow)
        {
            UIController.mainWindow = mainWindow;
        }
    }
}
