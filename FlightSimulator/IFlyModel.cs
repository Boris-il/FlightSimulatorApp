using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulator
{

	//public delegate void propertyChangedEventHandler(Object sender, PropertyChangedEventArgs e);
	interface IFlyModel : INotifyPropertyChanged
	{
		// connection to the simulator
		void connect(string ip, int port);
		void disconnect();
		Task start();
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

		bool Stop { get; set; }
	}

	class MyFlyModel : IFlyModel
	{
		private static readonly Object obj = new Object();
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

		private double latitude = 32.0055;
		private double longitude = 34.8854;
		private Location location;

		// Controlling members
		private double throttle;
		private double aileron;
		private double elevator;
		private double rudder;
		private double lastElevator=0;
		private double lastRudder=0;

		private string messageString;

		public MyFlyModel(IClient client)
		{
			this.telnetClient = client;
			Stop = true;
		}
		public void connect(string ip, int port)
		{
			for (int i = 0; i < 5; i++)
			{
				try
				{
					telnetClient.connect(ip, port);
					Stop = false;
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
			//Console.WriteLine("disconnected by error");
			Stop = true;
			telnetClient.disconnect();
			telnetClient = new MyTelnetClient();
		}

		public async Task start()
		{
			new Thread(delegate ()
			{
				while (!Stop)
				{
					lock (obj)
					{
						try
						{
							telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
							HeadingDeg = Double.Parse(telnetClient.read());
						}


						catch (System.IO.IOException e)
						{
							MessageString = e.Message;
							disconnect();
							break;
						}
						catch
						{
							MessageString = "Could not get HeadingDeg value";
							telnetClient.read();
						}
					}
					lock (obj)
					{
						try
						{
							telnetClient.write("get /instrumentation/gps/indicated-vertical-speed\n");
							VerticalSpeed = Double.Parse(telnetClient.read());
						}
						catch (System.IO.IOException e)
						{
							MessageString = e.Message;
							disconnect();
							break;
						}
						catch
						{
							MessageString = "Could not get VerticalSpeed value";
							telnetClient.read();
						}
					}
					lock (obj)
					{
						try
						{
							telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
							GroundSpeed = Double.Parse(telnetClient.read());
						}
						catch (System.IO.IOException e)
						{
							MessageString = e.Message;
							disconnect();
							break;
						}
						catch
						{
							MessageString = "Could not get GroundSpeed value";
							telnetClient.read();
						}
					}
					lock (obj)
					{

						try
						{
							telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
							AirSpeed = Double.Parse(telnetClient.read());
						}
						catch (System.IO.IOException e)
						{
							MessageString = e.Message;
							disconnect();
							break;
						}
						catch
						{
							MessageString = "Could not get IndicatedSpeed value";
							telnetClient.read();
						}
					}
					lock (obj)
					{

						try
						{
							telnetClient.write("get /instrumentation/gps/indicated-altitude-ft\n");
							Altitude = Double.Parse(telnetClient.read());
						}
						catch (System.IO.IOException e)
						{
							MessageString = e.Message;
							disconnect();
							break;
						}
						catch
						{
							MessageString = "Could not get IndicatedAltitude value";
							telnetClient.read();
						}
					}
					lock (obj)
					{
						try
						{
							telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
							InternalRollDeg = Double.Parse(telnetClient.read());
						}
						catch (System.IO.IOException e)
						{
							MessageString = e.Message;
							disconnect();
							break;
						}
						catch
						{
							MessageString = "Could not get InternalRollDeg value";
							telnetClient.read();
						}
					}
					lock (obj)
					{
						try
						{
							telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
							InternalPitchDeg = Double.Parse(telnetClient.read());
						}
						catch (System.IO.IOException e)
						{
							MessageString = e.Message;
							disconnect();
							break;
						}
						catch
						{
							MessageString = "Could not get InternalPitchDeg value";
							telnetClient.read();
						}
					}
					lock (obj)
					{
						try
						{
							telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft\n");
							GpsAltitude = Double.Parse(telnetClient.read());
						}
						catch (System.IO.IOException e)
						{
							MessageString = e.Message;
							disconnect();
							break;
						}
						catch
						{
							MessageString = "Could not get IdicatedAltitude value";
							telnetClient.read();
						}
					}
					lock (obj)
					{
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
						}
						catch (System.IO.IOException e)
						{
							messageString += e;
							disconnect();
							break;
						}
						catch
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
						}
						catch (System.IO.IOException e)
						{
							messageString += e;
							disconnect();
							break;
						}
						catch
						{
							messageString = "Could not get LatitudeDeg value";
							telnetClient.read();
						}
					}
						Thread.Sleep(250);
						
				}
			}).Start();
			await Task.Delay(2000);
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
				headingDeg = Math.Round(value, 3);
				NotifyPropertyChanged("HeadingDeg");
			}
		}
		public double VerticalSpeed
		{
			get { return verticalSpeed; }
			set
			{
				verticalSpeed = Math.Round(value, 3);
				NotifyPropertyChanged("VerticalSpeed");
			}
		}
		public double GroundSpeed
		{
			get { return groundSpeed; }
			set
			{
				groundSpeed = Math.Round(value, 3);
				NotifyPropertyChanged("GroundSpeed");
			}
		}
		public double AirSpeed
		{
			get { return airSpeed; }
			set
			{
				airSpeed = Math.Round(value, 3);
				NotifyPropertyChanged("AirSpeed");
			}
		}
		public double Altitude
		{
			get { return altitude; }
			set
			{
				altitude = Math.Round(value, 3);
				NotifyPropertyChanged("Altitude");
			}
		}
		public double InternalRollDeg
		{
			get { return internalRollDeg; }
			set
			{
				internalRollDeg = Math.Round(value, 3);
				NotifyPropertyChanged("InternalRollDeg");
			}
		}
		public double InternalPitchDeg
		{
			get { return internalPitchDeg; }
			set
			{
				internalPitchDeg = Math.Round(value, 3);
				NotifyPropertyChanged("InternalPitchDeg");
			}
		}
		public double GpsAltitude
		{
			get { return gpsAltitude; }
			set
			{
				gpsAltitude = Math.Round(value, 3);
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
				try
				{
					throttle = value;
					updateThrottle();
					
				}
				catch
				{
					MessageString = "Throttle update issue";
				}
				

			}

		}

		public async void updateThrottle()
		{
			await Task.Run(() =>
			{
				try
				{
					lock (obj)
					{
						this.telnetClient.write("set /controls/engines/current-engine/throttle " + throttle + "\n");
						this.telnetClient.read();
					}
						
				}
				catch
				{
					if (Stop)
					{
						MessageString = "you are disconnected!";
					}
					else
					{
						MessageString = "Throttle update issue";
					}
					
				}
				
			});
			
		}
		public double Aileron
		{
			get
			{
				return aileron;
			}
			set
			{
				try
				{
					
					aileron = value;
					updateAileron();
					
				}
				catch
				{
					if (Stop)
					{
						MessageString = "you are disconnected!";
					}
					else
					{

						MessageString = "Aileron update issue";
					}
					
				}
				
			}
		}
		public async void updateAileron()
		{
			await Task.Run(() =>
			{
				try
				{
					lock (obj)
					{
						this.telnetClient.write("set /controls/flight/aileron " + aileron + "\n");
						this.telnetClient.read();
					}
						
				}
				catch
				{
					if (Stop)
					{
						MessageString = "you are disconnected!";
					}
					else
					{
						MessageString = "Aileron update issue";
					}
					
				}
				
			});
			
		}

		public double Elevator
		{
			get
			{
				return elevator;
			}
			set
			{
				try
				{
					elevator = value;
					updateElevator();	
				}
				catch
				{
					if (Stop)
					{
						MessageString = "you are disconnected!";
					}
					else
					{
						MessageString = "Elevator update issue";
					}
					
				}
				

			}
		}
		public async void updateElevator()
		{
			await Task.Run(() =>
			{
				lock (obj)
				{
					if (Elevator != lastElevator)
					{
						
						this.telnetClient.write("set /controls/flight/elevator " + Elevator + "\n");
						this.telnetClient.read();
						//Task.Delay(250);
						Console.WriteLine("sent elevator: {0}", Elevator);
						lastElevator = Elevator;

					}
					//else
					//{
					//Console.WriteLine(lastElevator);
					//}

				}
				
			});
			
		}
		public double Rudder
		{
			get
			{
				return rudder;
			}
			set
			{
				try
				{
					rudder = value;
					updateRudder();
				}
				catch
				{
					if (Stop)
					{
						MessageString = "you are disconnected!";
					}
					else
					{
						MessageString = "Rudder update issue";
					}
				}
				
			}
		}

		public async void updateRudder()
		{
			await Task.Run(() =>
			{
				lock (obj)
				{
					if (Rudder != lastRudder)
					{
					this.telnetClient.write("set /controls/flight/rudder " + Rudder + "\n");
					this.telnetClient.read();
					//Task.Delay(250);
					lastRudder = Rudder;
					}
					
					//Console.WriteLine("rudder = {0}", rudder);
				}
					
			});
			
		}

		public bool Stop
		{
			get { return stop; }
			set
			{
				stop = value;
				NotifyPropertyChanged("Stop");
			}
		}
	}
}
