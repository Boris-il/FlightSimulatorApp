using System.Windows;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;

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

           // vm.model.connect("127.0.0.1", 5401);
           // vm.model.start();
        }

        private void Slider_ValueChanged_Aileron(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.j_vm.updateAileron(e.NewValue);
        }

        private void Slider_ValueChanged_Throttle(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.j_vm.updateThrottle(e.NewValue);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logBox.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ConnectionWindow conWin = new ConnectionWindow();
            conWin.ShowDialog();
            try
            {
                int p = int.Parse(conWin.getPort());

                if (p > 65535 || p < 0)
                {
                    logBox.Text = "Please check your port number {1:65535}";
                }
                else
                {
                    vm.model.connect(conWin.getIpAddress(), p);
                    vm.model.start();
                }

            }
            catch
            {
                if (conWin.conClicked)
                {
                    logBox.Text = "Please check your port number {1:65535}";
                }

            }


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            vm.model.disconnect();
        }
    }
}
