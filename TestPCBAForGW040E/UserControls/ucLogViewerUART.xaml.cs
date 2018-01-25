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
    /// Interaction logic for ucLogViewerUART.xaml
    /// </summary>
    public partial class ucLogViewerUART : UserControl
    {
        public ucLogViewerUART()
        {
            InitializeComponent();
            this.DataContext = GlobalData.logContent;
        }

        private void TextBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            TextBox t = sender as TextBox;
            uartScrollViewer.ScrollToEnd();
        }
    }
}
