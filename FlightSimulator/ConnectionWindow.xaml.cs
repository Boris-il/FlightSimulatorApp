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
using System.Windows.Shapes;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        private string ipAddress, port;
        public bool conClicked = false;
        public ConnectionWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            conClicked = true;
            this.ipAddress = ipText.Text;
            this.port = portText.Text;
            this.Close();
        }

        public string getIpAddress()
        {
            return this.ipAddress;
        }

        public string getPort()
        {
            return this.port;
        }
    }
}
