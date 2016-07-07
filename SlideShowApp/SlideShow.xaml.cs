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
    public partial class SlideShow : UserControl
    {
        private int position = 0;
        private const int COUNT = 15;

        public SlideShow()
        {
            InitializeComponent();
            currentPage.Children.Add(GetPage(position));
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                case Key.Enter:
                case Key.Right:
                    // HACK: テキストボックスあり
                    if (position == 11)
                    {
                        break;
                    }
                    if (position < COUNT - 1)
                    {
                        position++;
                        nextPage.Children.Clear();
                        nextPage.Children.Add(GetPage(position));
                        forward.Begin();
                    }
                    break;
                case Key.Back:
                case Key.Left:
                    // HACK: テキストボックスあり
                    if (position == 11)
                    {
                        break;
                    }
                    if (position > 0)
                    {
                        position--;
                        nextPage.Children.Clear();
                        nextPage.Children.Add(GetPage(position));
                        back.Begin();
                    }
                    break;
                case Key.F8:
                    if (position < COUNT - 1)
                    {
                        position++;
                        nextPage.Children.Clear();
                        nextPage.Children.Add(GetPage(position));
                        forward.Begin();
                    }
                    break;
                case Key.F7:
                    if (position > 0)
                    {
                        position--;
                        nextPage.Children.Clear();
                        nextPage.Children.Add(GetPage(position));
                        back.Begin();
                    }
                    break;
                default:
                    break;
            }
        }

        private UIElement GetPage(int index)
        {
            switch (index)
            {
                case 0:
                    return new Page0();
                case 1:
                    return new Page1();
                case 2:
                    return new Page2();
                case 3:
                    return new Page3();
                case 4:
                    return new Page4();
                case 5:
                    return new Page5();
                case 6:
                    return new Page6();
                case 7:
                    return new Page7();
                case 8:
                    return new Page8();
                case 9:
                    return new Page9();
                case 10:
                    return new Page10();
                case 11:
                    return new Page11();
                case 12:
                    return new Page12();
                case 13:
                    return new Page13();
                case 14:
                    return new Page14();
                default:
                    break;
            }
            throw new ArgumentOutOfRangeException();
        }

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            System.Windows.Interop.Content c = Application.Current.Host.Content;
            double w = c.ActualWidth;
            double h = c.ActualHeight;
            double sx = w / 1024.0d;
            double sy = h / 768.0d;
            scale.ScaleX = sx;
            scale.ScaleY = sy;
        }

        private void change_Completed(object sender, EventArgs e)
        {
            Canvas.SetLeft(currentPage, 0.0d);
            Canvas.SetLeft(spring, 1024.0d);
            Canvas.SetLeft(nextPage, 1024.0d);
            UIElement p = nextPage.Children[0];
            nextPage.Children.Clear();
            currentPage.Children.Clear();
            currentPage.Children.Add(p);
        }
    }
}
