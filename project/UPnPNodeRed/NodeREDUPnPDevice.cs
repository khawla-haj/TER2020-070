// UPnP .NET Framework Device Stack, Device Module
// Device Builder Build#1.0.5329.22110

using System;
using OpenSource.UPnP;

namespace OpenSource.DeviceBuilder
{
    /// <summary>
    /// Summary description for SampleDevice.
    /// </summary>
    internal class NodeREDUPnPDevice {
		private readonly UPnPDevice device;
		
		public NodeREDUPnPDevice()
		{
			device = UPnPDevice.CreateRootDevice(1800,1.0,"\\");
			
			device.FriendlyName = "UpnpNodeRed";
			device.Manufacturer = "OpenSource";
			device.ManufacturerURL = "http://opentools.homeip.net";
			device.ModelName = "UpnpNodeRed";
			device.ModelDescription = "Node Red UPnP Device Using Auto-Generated UPnP Stack";
			device.ModelNumber = "X1";
			device.HasPresentation = false;
			device.DeviceURN = "urn:schemas-upnp-org:device:NodeRED:1";
            NodeRedUPnPService ImportedService = new NodeRedUPnPService();
			/*ImportedService.External_AddLink = new OpenSource.DeviceBuilder.DvImportedService.Delegate_AddLink(ImportedService_AddLink);
			ImportedService.External_AddNode = new OpenSource.DeviceBuilder.DvImportedService.Delegate_AddNode(ImportedService_AddNode);
			ImportedService.External_GetFlowId = new OpenSource.DeviceBuilder.DvImportedService.Delegate_GetFlowId(ImportedService_GetFlowId);
			ImportedService.External_RemoveLink = new OpenSource.DeviceBuilder.DvImportedService.Delegate_RemoveLink(ImportedService_RemoveLink);
			ImportedService.External_RemoveNode = new OpenSource.DeviceBuilder.DvImportedService.Delegate_RemoveNode(ImportedService_RemoveNode);
			ImportedService.External_SetFlowId = new OpenSource.DeviceBuilder.DvImportedService.Delegate_SetFlowId(ImportedService_SetFlowId);*/
			device.AddService(ImportedService);
		}
		
		public void Start()
		{
			device.StartDevice();
		}
		
		public void Stop()
		{
			device.StopDevice();
		}
		
		public void ImportedService_AddLink()
		{
			Console.WriteLine("ImportedService_AddLink(" + ")");
		}
		
		public void ImportedService_AddNode()
		{
			Console.WriteLine("ImportedService_AddNode(" + ")");
		}
		
		public System.String ImportedService_GetFlowId()
		{
			Console.WriteLine("ImportedService_GetFlowId(" + ")");
			
			return "Sample String";
		}
		
		public void ImportedService_RemoveLink()
		{
			Console.WriteLine("ImportedService_RemoveLink(" + ")");
		}
		
		public void ImportedService_RemoveNode()
		{
			Console.WriteLine("ImportedService_RemoveNode(" + ")");
		}
		
		public void ImportedService_SetFlowId(System.String flowId)
		{
			Console.WriteLine("ImportedService_SetFlowId(" + flowId.ToString() + ")");
		}
		
	}
}

