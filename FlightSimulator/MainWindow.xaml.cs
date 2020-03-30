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
        JoystickViewModel j_vm;

        public MainWindow()
        {
            InitializeComponent();

            IFlyModel model = new MyFlyModel(new MyTelnetClient());

            vm = new FlyViewModel(model);
            j_vm = new JoystickViewModel(model);
            joystick1.DataContext = j_vm;

            DataContext = vm;
            /*vm.model.connect("127.0.0.1", 5402);
            vm.model.start();*/
        }

        private void Slider_ValueChanged_Aileron(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.j_vm.updateAileron(e.NewValue);
        }

        private void Slider_ValueChanged_Throttle(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.j_vm.updateThrottle(e.NewValue);
        }
    }
}
