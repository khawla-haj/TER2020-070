// UPnP .NET Framework Device Stack, Core Module
// Device Builder Build#1.0.5329.22110

using System;
using OpenSource.UPnP;
using OpenSource.DeviceBuilder;
using SampleDevice;
using SampleDevice.NodeRedAPI;
using System.Threading;

namespace OpenSource.DeviceBuilder
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	class SampleDeviceMain
	{
		public static DiscoverDevice dd { get; private set; }
		

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{


			dd = new DiscoverDevice();
			dd.StartScan();
	
			

			Thread thread2 = new Thread(new ThreadStart(dd.test));
			thread2.Start();




			// Starting UPnP Device
			//System.Console.WriteLine("UPnP .NET Framework Stack");
			//System.Console.WriteLine("Device Builder Build#1.0.5329.22110");
			//SampleDevice device = new SampleDevice();

			//device.Start();
			System.Console.WriteLine("Press return to stop device.");
			System.Console.ReadLine();
			//device.Stop();
			
		}
		
	}
}

