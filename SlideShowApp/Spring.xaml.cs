using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SlideShowApp
{
    public partial class Spring : UserControl
    {
        private const double UNIT = Math.PI / 2.0d;
        private const int COUNT = 18;
        private const int SEGMENTS = 2;

        private double _strokeThickness;
        public double StrokeThickness
        {
            get
            {
                return _strokeThickness;
            }
            set
            {
                _strokeThickness = value;
                foregounrd.StrokeThickness = _strokeThickness;
                background.StrokeThickness = _strokeThickness;
            }
        }

        public Spring()
        {
            InitializeComponent();
            for (int i = 0; i < COUNT; i++)
            {
                PathFigure pf = new PathFigure();
                for (int j = 0; j < SEGMENTS; j++)
                {
                    pf.Segments.Add(new BezierSegment());
                }
                PathGeometry pg = IsForeground(i) ? foreGeometry : backGeometry;
                pg.Figures.Add(pf);
            }
        }

        private void Resize()
        {
            int foregounrdIndex = 0;
            int backgroundIndex = 0;
            for (int i = 0; i < COUNT; i++)
            {
                PathFigure pf = null;
                if (IsForeground(i))
                {
                    pf = foreGeometry.Figures[foregounrdIndex];
                    foregounrdIndex++;
                }
                else
                {
                    pf = backGeometry.Figures[backgroundIndex];
                    backgroundIndex++;
                }
                double sx = UNIT * (double)i;
                double sy = Math.Sin(sx);
                pf.StartPoint = ToScreenPoint(sx, sy);

                double dx = UNIT / (double)(SEGMENTS * 3);
                for (int j = 0; j < SEGMENTS; j++)
                {
                    double x1 = UNIT * ((double)i + ((double)j / (double)SEGMENTS));
                    double y1 = Math.Sin(x1);
                    double yPrime1 = Math.Cos(x1);

                    double x2 = UNIT * ((double)i + ((double)(j + 1) / (double)SEGMENTS));
                    double y2 = Math.Sin(x2);
                    double yPrime2 = Math.Cos(x2);

                    double cx1 = x1 + dx;
                    double cy1 = y1 + (yPrime1 * dx);
                    double cx2 = x2 - dx;
                    double cy2 = y2 - (yPrime2 * dx);

                    BezierSegment bs = pf.Segments[j] as BezierSegment;
                    if (bs != null)
                    {
                        bs.Point1 = ToScreenPoint(cx1, cy1);
                        bs.Point2 = ToScreenPoint(cx2, cy2);
                        bs.Point3 = ToScreenPoint(x2, y2);
                    }
                }
            }
        }

        private Point ToScreenPoint(double x, double y)
        {
            double sx = (this.Width - _strokeThickness) * ((x * 2) / ((double)COUNT * Math.PI)) + (_strokeThickness / 2.0d);
            double sy = (this.Height - _strokeThickness) * ((1 - y) / 2.0d) + (_strokeThickness / 2.0d);
            return new Point(sx, sy);
        }

        private bool IsForeground(int i)
        {
            int f = i % 4;
            return (f == 0 || f == 3);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resize();
        }
    }
}
