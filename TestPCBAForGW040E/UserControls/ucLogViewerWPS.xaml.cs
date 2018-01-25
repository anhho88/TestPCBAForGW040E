using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestPCBAForGW040E.Functions;

namespace TestPCBAForGW040E.UserControls
{
    /// <summary>
    /// Interaction logic for ucLogViewerWPS.xaml
    /// </summary>
    public partial class ucLogViewerWPS : UserControl
    {
        public ucLogViewerWPS()
        {
            InitializeComponent();
            this.DataContext = GlobalData.logContent;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            TextBox t = sender as TextBox;
            uartScrollViewer.ScrollToEnd();
            GlobalData.mainwindowInformation.WPSDataSign = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            switch(b.Content) {
                case "Copy": {
                        break;
                    }
                case "Clear": {
                        break;
                    }
            }
        }
    }
}
