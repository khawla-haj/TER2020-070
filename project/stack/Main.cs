// UPnP .NET Framework Device Stack, Core Module
// Device Builder Build#1.0.5329.22110

using System;
using OpenSource.UPnP;
using OpenSource.DeviceBuilder;
using SampleDevice;
using SampleDevice.NodeRedAPI;

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

			string[][] s = new string[2][];
			s[0] = new string[1];
			s[1] = new string[2] { "Shirdrn", "Hamtty" };
			

			

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

