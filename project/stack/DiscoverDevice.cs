using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenSource.UPnP;
using SampleDevice.NodeRedAPI;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Collections;

namespace SampleDevice
{
	class DiscoverDevice
	{
		public NodeRedAPI.NodeRedAPI api = new NodeRedAPI.NodeRedAPI();
		private UPnPSmartControlPoint wcompControlPoint { get; set; }
		private UPnPSmartControlPoint noderedControlPoint { get; set; }

		public UPnPDevice deviceBindedFromWcomp;
		public UPnPDevice noderedControler;

		public UPnPService wcompService;
		public UPnPService noderedService;
		public int i = 0;


		public ArrayList output = new ArrayList();



		public virtual void StartScan()
		{
			wcompControlPoint = new UPnPSmartControlPoint(OnDeviceAdded1, OnServiceAdded, "urn:wcomp-fr:device:StructuralDevice:1");
			noderedControlPoint = new UPnPSmartControlPoint(OnDeviceAdded2, OnServiceAdded, "urn:schemas-upnp-org:device:NodeRED:1");

			
		}

		public void test()
		{
		
			while (true)
			{
				if (output.Count != 0)
				{
					Console.WriteLine("thread running");
					CheckStateVariable csv = new CheckStateVariable();
					Operation op = new Operation(api, csv.CheckValue(output[0].ToString()));
					if (output[0] != null)
						op.CheckAction();
					output.RemoveAt(0);
				}
			}
		}

	private void OnServiceAdded(UPnPSmartControlPoint sender, UPnPService service)
		{
		}

		private void OnDeviceAdded1(UPnPSmartControlPoint cp, UPnPDevice device)
		{
			

			this.deviceBindedFromWcomp = device;
			Console.WriteLine("found wcomp device " + deviceBindedFromWcomp.DeviceURN);
					

			wcompService = deviceBindedFromWcomp.GetService("urn:upnp-org:serviceId:StructuralService");
			//noderedService = this.noderedControler.GetService("urn:upnp-org:serviceId:NodeREDAPIService");

			//service.GetStateVariableObject("output");

			wcompService.Subscribe(500, (service, subscribeok) =>
			{
				if (!subscribeok) return;
				var stateVariable = service.GetStateVariableObject("output");
				stateVariable.OnModified += StateValueChanged;

				Console.WriteLine("state variable " + stateVariable);
			});

			


		}
		private void OnDeviceAdded2(UPnPSmartControlPoint cp, UPnPDevice device)
		{
			this.noderedControler = device;
			Console.WriteLine("found nodered device " + this.noderedControler.UniqueDeviceName);
			noderedService = this.noderedControler.GetService("urn:upnp-org:serviceId:NodeREDAPIService");

		}

		private void StateValueChanged(UPnPStateVariable sender, object newvalue)
		{
			Console.WriteLine("output: "+sender.Value);
			
			//csv.CheckValue(sender.Value.ToString());
			if (i != 0)
			{
				output.Add(sender.Value);
			}
			i++;
			
		}


		
	}

}
