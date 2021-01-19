// UPnP .NET Framework Device Stack, Core Module
// Device Builder Build#1.0.5329.22110

using System;
using System.Runtime.InteropServices;

namespace OpenSource.DeviceBuilder {
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	class SampleDeviceMain {
		static NodeREDUPnPDevice device;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			// fine approach to handling exiting the app
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
			HandlerRoutine hr = new HandlerRoutine(Handler);
			SetConsoleCtrlHandler(hr, true);
			GC.KeepAlive(hr);

			// Starting UPnP Device
			Console.WriteLine("NodeRED UPnP Device");
			device = new NodeREDUPnPDevice();
			device.Start();
			Console.WriteLine("Device started!");
			while (true) { Console.ReadLine(); }
		}

        #region exit handlers
        static void CurrentDomain_ProcessExit(object sender, EventArgs e) {
			device.Stop();
		}
		static Boolean Handler(CtrlTypes CtrlType) {
			// A switch to handle the event type.
			switch (CtrlType) {
				case CtrlTypes.CTRL_C_EVENT:
				case CtrlTypes.CTRL_BREAK_EVENT:
				case CtrlTypes.CTRL_CLOSE_EVENT:
				case CtrlTypes.CTRL_LOGOFF_EVENT:
				case CtrlTypes.CTRL_SHUTDOWN_EVENT:
					device.Stop();
					Console.WriteLine("exit");
					Environment.Exit(0);
					break;
			}
			return true;
		}
		// Declare the SetConsoleCtrlHandler function
		// as external and receiving a delegate.  
		[DllImport("Kernel32")]
		public static extern Boolean SetConsoleCtrlHandler(HandlerRoutine Handler, Boolean Add);

		// A delegate type to be used as the handler routine
		// for SetConsoleCtrlHandler.
		public delegate Boolean HandlerRoutine(CtrlTypes CtrlType);

		// An enumerated type for the control messages
		// sent to the handler routine.
		public enum CtrlTypes {
			CTRL_C_EVENT = 0,
			CTRL_BREAK_EVENT,
			CTRL_CLOSE_EVENT,
			CTRL_LOGOFF_EVENT = 5,
			CTRL_SHUTDOWN_EVENT
		}
        #endregion
    }
}