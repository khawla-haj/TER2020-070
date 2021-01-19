using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenSource.UPnP;

namespace SampleDevice
{
	class DiscoverDevice
	{
		private UPnPSmartControlPoint wcompControlPoint { get; set; }
		private UPnPSmartControlPoint noderedControlPoint { get; set; }

		public UPnPDevice deviceBindedFromWcomp;
		public UPnPDevice noderedControler;

		public UPnPService wcompService;
		public UPnPService noderedService;

		public virtual void StartScan()
		{
			wcompControlPoint = new UPnPSmartControlPoint(OnDeviceAdded1, OnServiceAdded, "urn:wcomp-fr:device:StructuralDevice:1");
			wcompControlPoint = new UPnPSmartControlPoint(OnDeviceAdded2, OnServiceAdded, "urn:schemas-upnp-org:device:NodeRED:1");
		}


		private void OnServiceAdded(UPnPSmartControlPoint sender, UPnPService service)
		{
		}

		private void OnDeviceAdded1(UPnPSmartControlPoint cp, UPnPDevice device)
		{
			Console.WriteLine("found device " + device.UniqueDeviceName);
			switch (device.UniqueDeviceName)
			{
				case "8347dca1-19c5-494f-9eab-7999512fcaa0":
					this.deviceBindedFromWcomp = device;
					Console.WriteLine("found wcomp device " + this.deviceBindedFromWcomp.UniqueDeviceName);
					break;
				case "dc7a530e-b3f2-431f-8691-279a6fd2dd3a":
					this.noderedControler = device;
					Console.WriteLine("found nodered device " + this.noderedControler.UniqueDeviceName);
					break;
			}

			wcompService = this.deviceBindedFromWcomp.GetService("urn:upnp-org:serviceId:StructuralService");
			//noderedService = this.noderedControler.GetService("urn:upnp-org:serviceId:NodeREDAPIService");

			

			wcompService.Subscribe(600, (service, subscribeok) =>
			{
				if (!subscribeok) return;

				var stateVariable = service.GetStateVariableObject("output");
				stateVariable.OnModified += StateValueChanged;

				//Console.WriteLine("state variable " + stateVariable);
			});

		}
		private void OnDeviceAdded2(UPnPSmartControlPoint cp, UPnPDevice device)
		{
			Console.WriteLine("found device " + device.UniqueDeviceName);
			switch (device.UniqueDeviceName)
			{
				case "8347dca1-19c5-494f-9eab-7999512fcaa0":
					this.deviceBindedFromWcomp = device;
					Console.WriteLine("found wcomp device " + this.deviceBindedFromWcomp.UniqueDeviceName);
					break;
				case "dc7a530e-b3f2-431f-8691-279a6fd2dd3a":
					this.noderedControler = device;
					Console.WriteLine("found nodered device " + this.noderedControler.UniqueDeviceName);
					break;
			}

			//wcompService = this.deviceBindedFromWcomp.GetService("urn:upnp-org:serviceId:StructuralService");
			noderedService = this.noderedControler.GetService("urn:upnp-org:serviceId:NodeREDAPIService");
			//getFlows();
		}

		private void StateValueChanged(UPnPStateVariable sender, object newvalue)
		{
			Console.WriteLine(sender.Value);
			//Console.WriteLine(sender.Value.GetType());
			CheckValue(sender.Value.ToString());

		}

		private Dictionary<string,string[]> CheckValue(string value)
		{
			string com = null;
			string[] parameter = null;

			string[] arr = value.Split('|');

			Dictionary<string, string[]> command = new Dictionary<string, string[]>();
			switch (arr.GetValue(1))
			{
				case "NEW_BEAN" :
					com = "ADD";
					parameter = new string[2];
					parameter[0] = arr[2];
					parameter[1] = arr[3];
					Console.WriteLine("add bean");
					AddNode();
					break;
				case "DEL_BEAN":
					com = "REMOVE";
					parameter = new string[1];
					parameter[0] = arr[2];
					Console.WriteLine("remove bean");
					break;
			}

			command.Add(com, parameter);

			Console.WriteLine(command);
			return command;
		}

		public void getFlows()
		{
			noderedService.InvokeAsync("GetFlows",null);
		}

		private void AddNode()
		{
			UPnPAction[] noderedActions= noderedService.GetActions();
			Console.WriteLine(noderedActions[0].ArgumentList);
			var arguments = new UPnPArgument[2];
			arguments[0] = new UPnPArgument("node", "1");
			arguments[1] = new UPnPArgument("flowId", "f32eab8b.153d28");
			noderedService.InvokeAsync("AddNode", arguments);
		}
	}

}
