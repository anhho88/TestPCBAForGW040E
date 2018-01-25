using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using TestPCBAForGW040E.Functions;

namespace TestPCBAForGW040E {
    /// <summary>
    /// Interaction logic for POPUP.xaml
    /// </summary>
    public partial class waitDUT : Window {

        Countdown t = new Countdown();

        public waitDUT(string timeout) {
            InitializeComponent();
            this.DataContext = t;
            Thread s = new Thread(new ThreadStart(() => {
                int z = 0;
                int tot = int.Parse(timeout) / 1000;
                while (tot - z > 0) {
                    t.time = (tot - z).ToString();
                    Thread.Sleep(1000);
                    z++;
                }
            }));
            s.IsBackground = true;
            s.Start();
        }


    }

    public class Countdown : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _time;

        public string time {
            get { return _time; }
            set { _time = value;
                OnPropertyChanged(nameof(time));
            }
        }
    }
}
