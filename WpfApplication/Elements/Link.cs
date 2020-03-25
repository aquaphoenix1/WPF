﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication.Elements
{
    public class Link
    {
        public BaseElement Element1 { get; private set; }
        public BaseElement Element2 { get; private set; }

        public int FirstPosition { get; private set; }
        public int SecondPosition { get; private set; }

        public int Length { get; set; }

        public Link(BaseElement element1, BaseElement element2, int firstPosition, int secondPosition, int length)
        {
            Element1 = element1;
            Element2 = element2;

            FirstPosition = firstPosition;
            SecondPosition = secondPosition;

            Length = length;
        }
    }
}
