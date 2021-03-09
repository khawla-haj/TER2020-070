// UPnP .NET Framework Device Stack, Core Module
// Device Builder Build#1.0.5329.22110

using System;
using OpenSource.UPnP;
using OpenSource.DeviceBuilder;
using SampleDevice;
using SampleDevice.NodeRedAPI;
using System.Threading;
using Newtonsoft.Json.Linq;
using SampleDevice.BeanToFlow.UPnP;
using System.Windows.Forms;

namespace OpenSource.DeviceBuilder
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	class SampleDeviceMain
	{
		public static DiscoverDevice dd { get; private set; }
		
		public static UPnPFlow u { get; private set; }
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//Application.Run(new Form1());

			dd = new DiscoverDevice();
			dd.StartScan();


			Thread thread2 = new Thread(new ThreadStart(dd.test));
			thread2.Start();






		}
		
	}
}

