using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	/// 

	public partial class App : Application
	{
		MainWindow wnd;

		public App() : base()
		{
			SetupUnhandledExceptionHandling();
		}
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			
			// Create the startup window
			wnd = new MainWindow();
			// Do stuff here, e.g. to the window
			//wnd.Title = "name fron app";
			// Show the window
			wnd.Show();


		}
		private void SetupUnhandledExceptionHandling()
		{
			AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
				ShowUnhandledException(args.ExceptionObject as Exception);
			//Application.Current.DispatcherUnhandledException
			Dispatcher.UnhandledException += (sender, args) =>
			{
				// If we are debugging, let Visual Studio handle the exception and take us to the code that threw it
				ShowUnhandledException(args.Exception);
				args.Handled = true;
			};
		}

		private void ShowUnhandledException(Exception e)
		{
			Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
			Console.WriteLine(e.Message);
			wnd.setErrorMessage(e.Message);
		}
	}
}
