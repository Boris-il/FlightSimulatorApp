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
using FlightSimulator;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        public Joystick()
        {
          InitializeComponent();
        }

        private double x, y, x1, y1;
        bool mousePressed;
        

        private void KnobBase_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.mousePressed = false;
            centerKnob_Completed(sender, e); //check
        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {
           // to do

        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!mousePressed)
            {
                this.x = e.GetPosition(sender as Ellipse).X;
                this.y = e.GetPosition(sender as Ellipse).Y;
                this.mousePressed = true;
            }
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mousePressed)
            {
                Ellipse eli = (sender as Ellipse);
                x1 = e.GetPosition(eli).X;
                y1 = e.GetPosition(eli).Y;
                double radius = getRadius(x, y, x1, y1);
                
            }
        }

        private double getRadius(double x, double y, double x1, double y1)
        {
            return Math.Sqrt((x1 - x) * (x1 - x) + (y1 - y) * (y1 - y));
        }

        private void KnobBase_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //todo
        }
    }
}
