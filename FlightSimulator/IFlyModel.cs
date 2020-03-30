using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FlightSimulator;
using Microsoft.Maps.MapControl.WPF;

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
		double Latitude { set; get; }
		double Longitude { set; get; }
		Location Location { get; set; }

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

						telnetClient.write("get /position/longitude-deg\n");
						Longitude = Double.Parse(telnetClient.read());

						telnetClient.write("get /position/latitude-deg\n");
						Latitude = Double.Parse(telnetClient.read());
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
				this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
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

		private double latitude;
		private double longitude;
		private Location location;

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

		public double Latitude
		{
			get { return latitude; }
			set
			{
				latitude = value;
				NotifyPropertyChanged("Location");
				//Location.Latitude = value;
			}
		}
		public double Longitude
		{
			get { return longitude; }
			set
			{
				longitude = value;
				NotifyPropertyChanged("Location");
				//Location.Longitude = value;
			}
		}

		public Location Location
		{
			get { return new Location(latitude, longitude); }
			set
			{
				location = value;
				NotifyPropertyChanged("Location");
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
				//NotifyPropertyChanged("Throttle");
				this.telnetClient.write("set /controls/engines/current-engine/throttle " + Throttle + "\n");
				this.telnetClient.read();

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
				//NotifyPropertyChanged("Ailrone");
				this.telnetClient.write("set /controls/flight/aileron " + Ailrone + "\n");
				this.telnetClient.read();

			}

		}
	}
}
