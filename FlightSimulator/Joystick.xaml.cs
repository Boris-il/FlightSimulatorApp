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

        Point start = new Point();
        Point current = new Point();
        private double x1, y1;
        bool mousePressed;


        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.mousePressed = false;
            centerKnob_Completed(sender, e); //check
        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            Canvas.SetLeft(Knob, 125);
            Canvas.SetTop(Knob, 125);
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!mousePressed)
            {
                // set initial mouse coordinates
                this.start.X = e.GetPosition(sender as Canvas).X;
                this.start.Y = e.GetPosition(sender as Canvas).Y;
                Console.WriteLine("start X: {0}, start Y: {1}", this.start.X, this.start.Y);
                this.mousePressed = true;
            }
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mousePressed)
            {
                this.current.X = e.GetPosition(sender as Canvas).X;
                this.current.Y = e.GetPosition(sender as Canvas).Y;
                Console.WriteLine("current X: {0}, current Y: {1}", this.current.X, this.current.Y);

                Canvas.SetLeft(Knob, this.current.X);
                Canvas.SetTop(Knob, this.current.Y);
                //double radius = getRadius(x, y, x1, y1);
            }
        }

        private double getRadius(double x, double y, double x1, double y1)
        {
            return Math.Sqrt((x1 - x) * (x1 - x) + (y1 - y) * (y1 - y));
        }
    }
}
