using Microsoft.Maps.MapControl.WPF;
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
        double throttleLastValue = 0;
        double aileronLastValue = 0;

        public MainWindow()
        {
            InitializeComponent();
            IFlyModel model = new MyFlyModel(new MyTelnetClient());
            vm = new FlyViewModel(model);
            j_vm = new JoystickViewModel(model);
            joystick1.DataContext = j_vm;
            aileronSlider.DataContext = j_vm;
            ThrottleSlider.DataContext = j_vm;
            DataContext = vm;
           // vm.model.connect("127.0.0.1", 5401);
           // vm.model.start();
        }

        /*private void Slider_ValueChanged_Aileron(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newValue = Math.Round(e.NewValue, 1);
            if (Math.Abs(newValue - aileronLastValue) > 0.09)
            {
                Console.WriteLine("AILERON: new value: {0}\nlast value: {1}", newValue, aileronLastValue);
                aileronLastValue = newValue;
                this.j_vm.updateAileron(newValue);
            }
            
            
            
        }

        private void Slider_ValueChanged_Throttle(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newValue = Math.Round(e.NewValue, 1);
            if (Math.Abs(newValue - throttleLastValue) > 0.09)
            {
                Console.WriteLine("THROTTLE: new value: {0}\nlast value: {1}", newValue, throttleLastValue);
                throttleLastValue = newValue;
                this.j_vm.updateThrottle(newValue);
            }
            
        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logBox.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ConnectionWindow conWin = new ConnectionWindow();
            conWin.ShowDialog();
            //try
            //{
                int p = int.Parse(conWin.getPort());

                if (p>65535 || p<0)
                {
                    logBox.Text = "Please check your port number {1:65535}";
                }
                else
                {
                    vm.model.connect(conWin.getIpAddress(), p);
                    vm.model.start();
                }
                
            //}
            /*catch
            {
                if (conWin.conClicked)
                {
                    logBox.Text = "Please check your port number {1:65535}";
                }
                
            }*/
            
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            vm.model.disconnect();
        }
        public void setErrorMessage(string s)
        {
            Console.WriteLine("YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
            Console.WriteLine(s);
            logBox.Text = s;
        }

    }
}
