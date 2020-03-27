using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FlyViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new FlyViewModel(new MyFlyModel(new MyTelnetClient()));
            DataContext = vm;
            vm.model.connect("172.0.0.1", 5401);
            vm.model.start();
            
            
            
            
            while (true)
            {
                Console.WriteLine("{0}", vm.model.HeadingDeg);
                Console.WriteLine("{0}", vm.model.GroundSpeed);
                Console.WriteLine("{0}", vm.model.GpsAltitude);
                Console.WriteLine("{0}", vm.model.InternalPitchDeg);
                //Thread.Sleep(3000);
            }
        }
    }
}
