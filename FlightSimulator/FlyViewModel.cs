using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;

namespace FlightSimulator
{
	class FlyViewModel : INotifyPropertyChanged
	{
		public IFlyModel model;

		// constructor
		public FlyViewModel(IFlyModel a_model)
		{
			this.model = a_model;
			model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
			{
				NotifyPropertyChanged("VM_" + e.PropertyName);
			};

		}


		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged(string propName)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		// Properties
		public double VM_HeadingDeg
		{
			get { return this.model.HeadingDeg; }
			/*set
            {
                VM_HeadingDeg = value;
                NotifyPropertyChanged("HeadingDeg");
            }*/
		}
		public double VM_VerticalSpeed
		{
			get { return this.model.VerticalSpeed; }
			/*set
			{
				verticalSpeed = value;
				NotifyPropertyChanged("VerticalSpeed");
			}*/
		}
		public double VM_GroundSpeed
		{
			get { return this.model.GroundSpeed; }
			/*set
			{
				groundSpeed = value;
				NotifyPropertyChanged("GroundSpeed");
			}*/
		}
		public double VM_AirSpeed
		{
			get { return this.model.AirSpeed; }
			/*set
			{
				airSpeed = value;
				NotifyPropertyChanged("AirSpeed");
			}*/
		}
		public double VM_Altitude
		{
			get { return this.model.Altitude; }
			/*set
			{
				altitude = value;
				NotifyPropertyChanged("Altitude");
			}*/
		}
		public double VM_InternalRollDeg
		{
			get { return this.model.InternalRollDeg; }
			/*set
			{
				internalRollDeg = value;
				NotifyPropertyChanged("InternalRollDeg");
			}*/
		}
		public double VM_InternalPitchDeg
		{
			get { return this.model.InternalPitchDeg; }
			/*set
			{
				internalPitchDeg = value;
				NotifyPropertyChanged("InternalPitchDeg");
			}*/
		}
		public double VM_GpsAltitude
		{
			get { return this.model.GpsAltitude; }
			/*set
			{
				gpsAltitude = value;
				NotifyPropertyChanged("GpsAltitude");
			}*/
		}

		public double VM_Latitude
		{
			get { return this.model.Latitude; }
		}
		public double VM_Longitude
		{
			get { return this.model.Longitude; }
		}

		public Location VM_Location
		{
			get
			{ return this.model.Location; }
		}


		public double VM_Throttle
		{
			set
			{
				model.Throttle = value;
			}
		}
		public double VM_Ailrone
		{
			set
			{
				model.Ailrone = value;
			}

		}

	}
}
