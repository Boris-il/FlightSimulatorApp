using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FlightSimulator;

namespace FlightSimulator
{

	//public delegate void propertyChangedEventHandler(Object sender, PropertyChangedEventArgs e);
	interface IFlyModel : INotifyPropertyChanged
	{
		// connection to the simulator
		void connect(string ip, int port);
		void disconnect();
		void start();
		//void update(string prop, double e);

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

		double Throttle { set; get; }
		double Ailrone { set; get; }
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
					try
					{
						telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
						HeadingDeg = Double.Parse(telnetClient.read());

						telnetClient.write("get /instrumentation/gps/indicated-vertical-speed\n");
						VerticalSpeed = Double.Parse(telnetClient.read());

						telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
						GroundSpeed = Double.Parse(telnetClient.read());

						telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
						AirSpeed = Double.Parse(telnetClient.read());

						telnetClient.write("get /instrumentation/gps/indicated-altitude-ft\n");
						Altitude = Double.Parse(telnetClient.read());

						telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
						InternalRollDeg = Double.Parse(telnetClient.read());

						telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
						InternalPitchDeg = Double.Parse(telnetClient.read());

						telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft\n");
						GpsAltitude = Double.Parse(telnetClient.read());
					} catch
					{
						continue;
					}
					
					Thread.Sleep(250);
				}
			}).Start();
		}

		public void NotifyPropertyChanged(string propName)
		{
			if (this.PropertyChanged != null)
			{
				switch (propName)
				{
					case "Ailrone":
						this.telnetClient.write("set /controls/flight/aileron " + Ailrone + "\n");
						break;
					case "Throttle":
						this.telnetClient.write("set /controls/engines/current-engine/throttle " + Throttle + "\n");
						break;
					default:
						this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
						break;
				}
			}
				
		}

		/*public void update(string prop, double e)
		{
			new Thread(delegate ()
			{
				switch (prop)
				{
					case "Ailrone":
						this.telnetClient.write("set /controls/flight/aileron " + e + "\n");
						break;
					case "Throttle":
						this.telnetClient.write("set /controls/engines/current-engine/throttle " + e + "\n");
						break;
				}
			}).Start();
			
		}*/

		private double headingDeg;
		private double verticalSpeed;
		private double groundSpeed;
		private double airSpeed;
		private double altitude;
		private double internalRollDeg;
		private double internalPitchDeg;
		private double gpsAltitude;

		private double throttle;
		private double ailrone;

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

		public double Throttle
		{
			get
			{
				return throttle;
			}
			set
			{
				throttle = value;
				NotifyPropertyChanged("Throttle");
			}

		}
		public double Ailrone
		{
			get
			{
				return ailrone;
			}
			set
			{
				ailrone = value;
				NotifyPropertyChanged("Ailrone");
			}

		}
	}
}
