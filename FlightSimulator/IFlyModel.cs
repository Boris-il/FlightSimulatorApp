using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlightSimulator;

namespace FlightSimulator
{
	
	/*interface INotifyPropertyChanged
	{
		event PropertyChangedEventHandler propertyChanged;
	}*/
	//public delegate void propertyChangedEventHandler(Object sender, PropertyChangedEventArgs e);
	interface IFlyModel : INotifyPropertyChanged
    {
		// connection to the simulator
		void connect(string ip, int port);
		void disconnect();
		void start();

		// Dashboard properties
		double HeadingDeg { set; get; }
		double VerticalSpeed { set; get; }
		double GroundSpeed { set; get; }
		double AirSpeed { set; get; }
		double Altitude { set; get; }
		double InternalRollDeg { set; get; }
		double InternalPitchDeg { set; get; }
		double GpsAltitude { set; get; }

		// movement
		/*void move(double speed, int angle);
		void moveArm(int az, int e1, int e2, bool grip);*/
	}

    class MyFlyModel : IFlyModel
    {
		public event PropertyChangedEventHandler PropertyChanged;

		IClient telnetClient;
        volatile Boolean stop;

		public MyFlyModel(IClient client)
		{
			this.telnetClient = client;
			this.stop = false;
		}
        public void connect(string ip, int port)
        {
			telnetClient.connect(ip, port);
        }
		public void disconnect()
		{
			this.stop = true;
			telnetClient.disconnect();
		}

		public void start()
		{
			new Thread(delegate ()
			{
				while (!stop)
				{
					//todo path
					telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg");
					headingDeg = Double.Parse(telnetClient.read());

					telnetClient.write("get /instrumentation/gps/indicated-vertical-speed");
					verticalSpeed = Double.Parse(telnetClient.read());

					telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt");
					groundSpeed = Double.Parse(telnetClient.read());

					telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt");
					airSpeed = Double.Parse(telnetClient.read());

					telnetClient.write("get /instrumentation/gps/indicated-altitude-ft");
					altitude = Double.Parse(telnetClient.read());

					telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg");
					internalRollDeg = Double.Parse(telnetClient.read());

					telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg");
					internalPitchDeg = Double.Parse(telnetClient.read());

					telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft");
					gpsAltitude = Double.Parse(telnetClient.read());

					Thread.Sleep(250);
				}
			}).Start();
		}

		public void NotifyPropertyChanged(string propName)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		private double headingDeg;
		private double verticalSpeed;
		private double groundSpeed;
		private double airSpeed;
		private double altitude;
		private double internalRollDeg;
		private double internalPitchDeg;
		private double gpsAltitude;

		public double HeadingDeg
		{
			get { return headingDeg; }
			set
			{
				headingDeg = value;
				NotifyPropertyChanged("HeadingDeg");
			}
		}
		public double VerticalSpeed
		{
			get { return verticalSpeed; }
			set
			{
				verticalSpeed = value;
				NotifyPropertyChanged("VerticalSpeed");
			}
		}
		public double GroundSpeed
		{
			get { return groundSpeed; }
			set
			{
				groundSpeed = value;
				NotifyPropertyChanged("GroundSpeed");
			}
		}
		public double AirSpeed
		{
			get { return airSpeed; }
			set
			{
				airSpeed = value;
				NotifyPropertyChanged("AirSpeed");
			}
		}
		public double Altitude
		{
			get { return altitude; }
			set
			{
				altitude = value;
				NotifyPropertyChanged("Altitude");
			}
		}
		public double InternalRollDeg
		{
			get { return internalRollDeg; }
			set
			{
				internalRollDeg = value;
				NotifyPropertyChanged("InternalRollDeg");
			}
		}
		public double InternalPitchDeg
		{
			get { return internalPitchDeg; }
			set
			{
				internalPitchDeg = value;
				NotifyPropertyChanged("InternalPitchDeg");
			}
		}
		public double GpsAltitude
		{
			get { return gpsAltitude; }
			set
			{
				gpsAltitude = value;
				NotifyPropertyChanged("GpsAltitude");
			}
		}



	}
}
