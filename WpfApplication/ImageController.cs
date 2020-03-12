using Svg2Xaml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication
{
    static class ImageController
    {
        private static string imagesPath = Directory.GetCurrentDirectory();

        public static void Init()
        {
#if DEBUG || RELEASE
            imagesPath = imagesPath.Substring(0, imagesPath.LastIndexOf("\\"));
            imagesPath = imagesPath.Substring(0, imagesPath.LastIndexOf("\\"));
#endif

            imagesPath += "\\Images\\";
        }

        public static DrawingImage Open(string fileName)
        {
            using (FileStream stream = new FileStream(imagesPath + fileName, FileMode.Open, FileAccess.Read))
            {
                return SvgReader.Load(stream);
            }
        }
    }
}
