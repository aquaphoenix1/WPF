using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication
{
    static class UIController
    {
        private static MainWindow mainWindow;

        public static void DrawLine(Point p1, Point p2)
        {
            mainWindow.DrawLine(p1, p2);
        }

        internal static void Init(MainWindow mainWindow)
        {
            UIController.mainWindow = mainWindow;
        }
    }
}
