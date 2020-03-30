using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
