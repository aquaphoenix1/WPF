﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfApplication.Elements.MenuElements;

namespace WpfApplication.Elements
{
    sealed class GraphEdge : UserControl
    {
        public IElement SourceElement { get; set; }
        public IElement DestinationElement { get; set; }

        public CheckBox SourceCB { get; set; }
        public CheckBox DestinationCB { get; set; }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Point), typeof(GraphEdge), new FrameworkPropertyMetadata(default(Point)));
        public Point Source { get { return (Point)this.GetValue(SourceProperty); } set { this.SetValue(SourceProperty, value); } }

        public static readonly DependencyProperty DestinationProperty = DependencyProperty.Register("Destination", typeof(Point), typeof(GraphEdge), new FrameworkPropertyMetadata(default(Point)));
        public Point Destination { get { return (Point)this.GetValue(DestinationProperty); } set { this.SetValue(DestinationProperty, value); } }

        public GraphEdge()
        {
            LineSegment segment = new LineSegment(default(Point), true);
            PathFigure figure = new PathFigure(default(Point), new[] { segment }, false);
            PathGeometry geometry = new PathGeometry(new[] { figure });
            BindingBase sourceBinding = new Binding { Source = this, Path = new PropertyPath(SourceProperty) };
            BindingBase destinationBinding = new Binding { Source = this, Path = new PropertyPath(DestinationProperty) };
            BindingOperations.SetBinding(figure, PathFigure.StartPointProperty, sourceBinding);
            BindingOperations.SetBinding(segment, LineSegment.PointProperty, destinationBinding);
            Content = new Path
            {
                Data = geometry,
                StrokeThickness = 5,
                Stroke = Brushes.White,
                MinWidth = 1,
                MinHeight = 1
            };
        }
    }
}
