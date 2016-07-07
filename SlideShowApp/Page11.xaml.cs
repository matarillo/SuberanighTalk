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
    public partial class Page11 : UserControl
    {
        public HSql HSql
        {
            get
            {
                return ((App)Application.Current).hsql;
            }
        }

        public Page11()
        {
            InitializeComponent();
        }

        private void execButton_Click(object sender, RoutedEventArgs e)
        {
            string sql = inputTextBox.Text;
            outputTextBox.Text = HSql.ExecuteQuery(sql);
        }
    }
}
