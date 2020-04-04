using System;
using System.ComponentModel;
using System.Threading;
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

		// Controlling preperties
		double Elevator { get; set; }
		double Rudder { get; set; }
		double Throttle { set; get; }
		double Aileron { set; get; }

		string MessageString { get; set; }




	}

	class MyFlyModel : IFlyModel
	{
		public event PropertyChangedEventHandler PropertyChanged;

		IClient telnetClient;
		volatile Boolean stop;

		// Dashboard members
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

		// Controlling members
		private double throttle;
		private double aileron;
		private double elevator;
		private double rudder;

		private string messageString;

		public MyFlyModel(IClient client)
		{
			this.telnetClient = client;
			this.stop = false;
		}
		public void connect(string ip, int port)
		{
			for (int i = 0; i < 5; i++)
			{
				try
				{
					this.stop = false;
					telnetClient.connect(ip, port);
					break;
				}
				catch
				{
					MessageString = "Connection problem. Please try again!";
				}
			}
			
			
		}
		public void disconnect()
		{
			this.stop = true;
			telnetClient.disconnect();
			telnetClient = new MyTelnetClient();
		}

		public void start()
		{
			new Thread(delegate ()
			{
				while (!stop)
				{
					try
					{
						telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
						HeadingDeg = Double.Parse(telnetClient.read());
					} catch
					{
						messageString = "Could not get HeadingDeg value";
						// read for "ERR"
						telnetClient.read();
					}

					try
					{
						telnetClient.write("get /instrumentation/gps/indicated-vertical-speed\n");
						VerticalSpeed = Double.Parse(telnetClient.read());
					} catch
					{
						messageString = "Could not get VerticalSpeed value";
						telnetClient.read();
					}

					try
					{
						telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
						GroundSpeed = Double.Parse(telnetClient.read());
					} catch
					{
						messageString = "Could not get GroundSpeed value";
						telnetClient.read();
					}

					try
					{
						telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
						AirSpeed = Double.Parse(telnetClient.read());
					} catch
					{
						messageString = "Could not get IndicatedSpeed value";
						telnetClient.read();
					}

					try
					{
						telnetClient.write("get /instrumentation/gps/indicated-altitude-ft\n");
						Altitude = Double.Parse(telnetClient.read());
					} catch
					{
						messageString = "Could not get IndicatedAltitude value";
						telnetClient.read();
					}

					try
					{
						telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
						InternalRollDeg = Double.Parse(telnetClient.read());
					} catch
					{
						messageString = "Could not get InternalRollDeg value";
						telnetClient.read();
					}

					try
					{
						telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
						InternalPitchDeg = Double.Parse(telnetClient.read());
					} catch
					{
						messageString = "Could not get InternalPitchDeg value";
						telnetClient.read();
					}

					try
					{
						telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft\n");
						GpsAltitude = Double.Parse(telnetClient.read());
					} catch
					{
						messageString = "Could not get IdicatedAltitude value";
						telnetClient.read();
					}

					try
					{
						telnetClient.write("get /position/longitude-deg\n");
						double returnVal = Double.Parse(telnetClient.read());
						// check correctness of returned value
						//returnVal = 190;
						if (returnVal > -180 && returnVal < 180)
						{
							Longitude = returnVal;
						}
						else
						{
							messageString = "LONGITUDE not in range";
						}
					} catch
					{
						messageString = "Could not get LongitudeDeg value";
						telnetClient.read();
					}

					try
					{
						telnetClient.write("get /position/latitude-deg\n");
						double returnVal = Double.Parse(telnetClient.read());
						// check correctness of returned value
						if (returnVal > -90 && returnVal < 90)
						{
							Latitude = returnVal;
						}
						else
						{
							messageString = "LATITUDE not in range";
						}
					} catch
					{
						messageString = "Could not get LatitudeDeg value";
						telnetClient.read();
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

		public string MessageString
		{
			get { return messageString; }
			set
			{
				messageString = value;
				NotifyPropertyChanged("MessageString");
			}
		}

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
				this.telnetClient.write("set /controls/engines/current-engine/throttle " + Throttle + "\n");
				this.telnetClient.read();

			}

		}
		public double Aileron
		{
			get
			{
				return aileron;
			}
			set
			{
				aileron = value;
				this.telnetClient.write("set /controls/flight/aileron " + Aileron + "\n");
				this.telnetClient.read();
			}
		}

		public double Elevator
		{
			get
			{
				return elevator;
			}
			set
			{
				elevator = value;
				this.telnetClient.write("set /controls/flight/elevator " + Elevator + "\n");
				this.telnetClient.read();

			}
		}
		public double Rudder
		{
			get
			{
				return rudder;
			}
			set
			{
				rudder = value;
				this.telnetClient.write("set /controls/flight/rudder " + Rudder + "\n");
				this.telnetClient.read();
			}
		}
	}
}
