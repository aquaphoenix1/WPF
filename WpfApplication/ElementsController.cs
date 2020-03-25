using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication.Elements;

namespace WpfApplication
{
    class ElementsController
    {
        public static List<BaseElement> Elements { get; private set; } = new List<BaseElement>();
        public static List<Link> Links { get; set; } = new List<Link>();

        private static int currentId = 0;

        public static void AddElement(BaseElement element)
        {
            Elements.Add(element);
        }

        internal static void ChangeLocation(BaseUIElement elem, Point point)
        {
            Elements.First(x => x.Equals(elem.Element)).Point = point;
        }

        internal static void AddLink(BaseElement element1, BaseElement element2, int firstPosition, int secondPosition, int length = 0)
        {
            Links.Add(new Link(element1, element2, firstPosition, secondPosition, length));
        }

        internal static void RemoveElement(BaseUIElement element)
        {
            var elem = Elements.Find(x => x.Equals(element.Element));
            var removedLinks = Links.Where(x => x.Element1.Equals(elem) || x.Element2.Equals(elem)).ToList();
            removedLinks.ForEach(RemoveLink);

            Elements.Remove(elem);
        }

        internal static void RemoveLink(Link link) {
            Links.Remove(link);
        }

        internal static void Clear()
        {
            Elements = new List<BaseElement>();
            Links = new List<Link>();
            currentId = 0;
        }

        internal static int GetNextId()
        {
            return ++currentId;
        }

        internal static void SetLastId(int id)
        {
            if (currentId < id)
            {
                currentId = id;
            }
        }
    }
}
