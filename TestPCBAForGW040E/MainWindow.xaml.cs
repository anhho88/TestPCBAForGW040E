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

namespace TestPCBAForGW040E {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {


        List<TextBlock> listTextBlock = null;

        private void configStartUpLocation() {
            this.Top = 0;
            this.Left = 0;
            this.Width = System.Windows.SystemParameters.WorkArea.Width * 1.0;
            this.Height = System.Windows.SystemParameters.WorkArea.Height;
        }

        public MainWindow() {
            InitializeComponent();
            this.configStartUpLocation();
            listTextBlock = new List<TextBlock>() { tbTesting, tbLogViewerUART, tbLogViewerWPS, tbLogViewerSystem, tbSettingsDUT, tbHelpUserGuide, tbHelpHistory, tbHelpAbout };
            this.tbTesting.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC6FF00"));
            this.DataContext = GlobalData.mainwindowInformation;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            //this.DragMove();
        }

        private void btnClose_MouseDown(object sender, MouseButtonEventArgs e) {
            this.Close();
        }

        private void TextBlock_MouseMove(object sender, MouseEventArgs e) {
            TextBlock t = sender as TextBlock;
            t.FontSize = 18;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e) {
            TextBlock t = sender as TextBlock;
            t.FontSize = 16;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            TextBlock t = sender as TextBlock;
            foreach(var item in listTextBlock) {
                item.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF5F5F5"));
            }
            t.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC6FF00"));
            int i = listTextBlock.IndexOf(t);
            bringUCtoFront(i);
        }

        private void bringUCtoFront(int index) {
            List<Control> list = new List<Control>() { ucTesting, ucLogViewerUART, ucLogViewerWPS, ucLogViewerSystem, ucConfigDUT, ucHelpUserGuide, ucHelpHistory, ucHelpAbout};
            for (int i = 0; i < list.Count; i++) {
                if (i == index) Canvas.SetZIndex(list[i], 1);
                else Canvas.SetZIndex(list[i], 0);
            }
        }
    }


}
