﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator
{
    class JoystickViewModel : INotifyPropertyChanged
	{
        public IFlyModel model;

		// constructor
		public JoystickViewModel(IFlyModel a_model)
		{
			this.model = a_model;
			model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
			{
				//NotifyPropertyChanged("VM_" + e.PropertyName);
			};

		}
		public void NotifyPropertyChanged(string propName)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		public double VM_Throttle
		{
			set
			{
				model.Throttle = value;
			}
		}
		public double VM_Aileron
		{
			set
			{
				model.Aileron = value;
			}

		}

		public void updateAileron(double d)
		{
			VM_Aileron = d;
		}

		public void updateThrottle(double d)
		{
			VM_Throttle = d;
		}

		public double VM_PointX
		{
			set
			{
				//Console.WriteLine("last X was: {0}\nnew X is: {1}", model.Rudder, value) ;
				model.Rudder = Math.Round((value / 85), 2);
			}
		}
		public double VM_PointY
		{
			set
			{
				//Console.WriteLine("Y updated");	
				model.Elevator = Math.Round((value / (-85)), 2);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
