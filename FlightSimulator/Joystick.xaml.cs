using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        bool mousePressed;


        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.mousePressed = false;
            centerKnob_Completed(sender, e);
        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!mousePressed)
            {
                // set initial mouse coordinates
                this.start.X = e.GetPosition(this).X;
                this.start.Y = e.GetPosition(this).Y;
                Console.WriteLine("start X: {0}, start Y: {1}", this.start.X, this.start.Y);
                this.mousePressed = true;
            }
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            double d;
            if (this.mousePressed)
            {
                //Console.WriteLine("current X: {0}, current Y: {1}", knobPosition.X, knobPosition.Y);

                this.current.X = e.GetPosition(this).X - this.start.X;
                this.current.Y = e.GetPosition(this).Y - this.start.Y;

                // limit the movement of the knob
                if ((d = Math.Sqrt(this.current.X * this.current.X + this.current.Y * this.current.Y)) < (BlackBase.Width / 2))
                {
                    knobPosition.X = this.current.X;
                    knobPosition.Y = this.current.Y;
                }
                else
                {
                    if (d > (BlackBase.Width / 2) + 20)
                    {
                        this.mousePressed = false;
                        centerKnob_Completed(sender, e);
                    }
                    
                }
            }
        }

        private void KnobBase_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //todo
        }

        
    }
}
