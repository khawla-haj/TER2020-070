// UPnP .NET Framework Device Stack, Device Module
// Device Builder Build#1.0.5329.22110

using System;
using OpenSource.UPnP;
using OpenSource.DeviceBuilder;

namespace OpenSource.DeviceBuilder
{
	/// <summary>
	/// Summary description for SampleDevice.
	/// </summary>
	class SampleDevice
	{
		private UPnPDevice device;
		private UPnPDevice[] devices;

		public SampleDevice()
		{
			//devices = UPnPDevice.GetDevices("urn:wcomp-fr:device:StructuralDevice:1");
			//Console.WriteLine(devices);

			device = UPnPDevice.CreateRootDevice(1800,1.0,"\\");
			
			device.FriendlyName = "Container1_Structural_2";
			device.Manufacturer = "I3S Rainbow";
			device.ManufacturerURL = "https://www.wcomp.fr/";
			device.ModelName = "SharpWComp-3.2 Container";
			device.ModelDescription = "UPnP Device for compositional adaptation of WComp containers";
			device.ModelNumber = "3.2.1.1 on CLR 2.0.50727.9151";
			device.HasPresentation = false;
			device.DeviceURN = "urn:wcomp-fr:device:StructuralDevice:1";
			OpenSource.DeviceBuilder.DvStructuralService StructuralService = new OpenSource.DeviceBuilder.DvStructuralService();
			StructuralService.External_CreateBean = new DvStructuralService.Delegate_CreateBean(StructuralService_CreateBean);
			StructuralService.External_CreateBeanAtPos = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_CreateBeanAtPos(StructuralService_CreateBeanAtPos);
			StructuralService.External_CreateLink = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_CreateLink(StructuralService_CreateLink);
			StructuralService.External_GetADL = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetADL(StructuralService_GetADL);
			StructuralService.External_GetBeanNames = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetBeanNames(StructuralService_GetBeanNames);
			StructuralService.External_GetBeans = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetBeans(StructuralService_GetBeans);
			StructuralService.External_GetBeansLinkedFrom = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetBeansLinkedFrom(StructuralService_GetBeansLinkedFrom);
			StructuralService.External_GetBeansLinkedTo = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetBeansLinkedTo(StructuralService_GetBeansLinkedTo);
			StructuralService.External_GetBeanTypes = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetBeanTypes(StructuralService_GetBeanTypes);
			StructuralService.External_GetLinks = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetLinks(StructuralService_GetLinks);
			StructuralService.External_GetLinksFrom = new DvStructuralService.Delegate_GetLinksFrom(StructuralService_GetLinksFrom);
			StructuralService.External_GetLinksTo = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetLinksTo(StructuralService_GetLinksTo);
			StructuralService.External_GetLoadedTypes = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetLoadedTypes(StructuralService_GetLoadedTypes);
			StructuralService.External_GetPropertyValue = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetPropertyValue(StructuralService_GetPropertyValue);
			StructuralService.External_GetPropertyValues = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetPropertyValues(StructuralService_GetPropertyValues);
			StructuralService.External_GetTypeEvents = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetTypeEvents(StructuralService_GetTypeEvents);
			StructuralService.External_GetTypeMethods = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetTypeMethods(StructuralService_GetTypeMethods);
			StructuralService.External_GetTypeOfBean = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetTypeOfBean(StructuralService_GetTypeOfBean);
			StructuralService.External_GetTypePorts = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetTypePorts(StructuralService_GetTypePorts);
			StructuralService.External_GetTypeProperties = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_GetTypeProperties(StructuralService_GetTypeProperties);
			StructuralService.External_LoadType = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_LoadType(StructuralService_LoadType);
			StructuralService.External_RemoveBean = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_RemoveBean(StructuralService_RemoveBean);
			StructuralService.External_RemoveLink = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_RemoveLink(StructuralService_RemoveLink);
			StructuralService.External_SetAdaptationSchema = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_SetAdaptationSchema(StructuralService_SetAdaptationSchema);
			StructuralService.External_SetPropertyValue = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_SetPropertyValue(StructuralService_SetPropertyValue);
			StructuralService.External_UnloadType = new OpenSource.DeviceBuilder.DvStructuralService.Delegate_UnloadType(StructuralService_UnloadType);
			device.AddService(StructuralService);
			
			// Setting the initial value of evented variables
			StructuralService.Evented_output = "Sample String";
		}
		
		public void Start()
		{
			device.StartDevice();
		}
		
		public void Stop()
		{
			device.StopDevice();
		}
		
		public System.String StructuralService_CreateBean(System.String beanType, System.String beanName )
		{
			Console.WriteLine("StructuralService_CreateBean(" + beanType.ToString() + beanName.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_CreateBeanAtPos(System.String beanType, System.String beanName, System.Int32 x, System.Int32 y )
		{
			Console.WriteLine("StructuralService_CreateBeanAtPos(" + beanType.ToString() + beanName.ToString() + x.ToString() + y.ToString() + ")");
			
			return "Sample String";
		}
		
		public void StructuralService_CreateLink(System.String source, System.String srcEvent, System.String destination, System.String dstAction, System.String dstParams)
		{
			Console.WriteLine("StructuralService_CreateLink(" + source.ToString() + srcEvent.ToString() + destination.ToString() + dstAction.ToString() + dstParams.ToString() + ")");
		}
		
		public System.String StructuralService_GetADL()
		{
			Console.WriteLine("StructuralService_GetADL(" + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetBeanNames()
		{
			Console.WriteLine("StructuralService_GetBeanNames(" + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetBeans()
		{
			Console.WriteLine("StructuralService_GetBeans(" + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetBeansLinkedFrom(System.String source )
		{
			Console.WriteLine("StructuralService_GetBeansLinkedFrom(" + source.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetBeansLinkedTo(System.String destination )
		{
			Console.WriteLine("StructuralService_GetBeansLinkedTo(" + destination.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetBeanTypes()
		{
			Console.WriteLine("StructuralService_GetBeanTypes(" + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetLinks()
		{
			Console.WriteLine("StructuralService_GetLinks(" + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetLinksFrom(System.String source )
		{
			Console.WriteLine("StructuralService_GetLinksFrom(" + source.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetLinksTo(System.String destination )
		{
			Console.WriteLine("StructuralService_GetLinksTo(" + destination.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetLoadedTypes()
		{
			Console.WriteLine("StructuralService_GetLoadedTypes(" + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetPropertyValue(System.String instName, System.String propName )
		{
			Console.WriteLine("StructuralService_GetPropertyValue(" + instName.ToString() + propName.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetPropertyValues(System.String instName )
		{
			Console.WriteLine("StructuralService_GetPropertyValues(" + instName.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetTypeEvents(System.String type )
		{
			Console.WriteLine("StructuralService_GetTypeEvents(" + type.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetTypeMethods(System.String type )
		{
			Console.WriteLine("StructuralService_GetTypeMethods(" + type.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetTypeOfBean(System.String instName )
		{
			Console.WriteLine("StructuralService_GetTypeOfBean(" + instName.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetTypePorts(System.String type )
		{
			Console.WriteLine("StructuralService_GetTypePorts(" + type.ToString() + ")");
			
			return "Sample String";
		}
		
		public System.String StructuralService_GetTypeProperties(System.String type )
		{
			Console.WriteLine("StructuralService_GetTypeProperties(" + type.ToString() + ")");
			
			return "Sample String";
		}
		
		public void StructuralService_LoadType(System.String fileName)
		{
			Console.WriteLine("StructuralService_LoadType(" + fileName.ToString() + ")");
		}
		
		public void StructuralService_RemoveBean(System.String instName)
		{
			Console.WriteLine("StructuralService_RemoveBean(" + instName.ToString() + ")");
		}
		
		public void StructuralService_RemoveLink(System.String linkName)
		{
			Console.WriteLine("StructuralService_RemoveLink(" + linkName.ToString() + ")");
		}
		
		public void StructuralService_SetAdaptationSchema(System.String adaptDescr)
		{
			Console.WriteLine("StructuralService_SetAdaptationSchema(" + adaptDescr.ToString() + ")");
		}
		
		public void StructuralService_SetPropertyValue(System.String instName, System.String propName, System.String propValue)
		{
			Console.WriteLine("StructuralService_SetPropertyValue(" + instName.ToString() + propName.ToString() + propValue.ToString() + ")");
		}
		
		public void StructuralService_UnloadType(System.String type)
		{
			Console.WriteLine("StructuralService_UnloadType(" + type.ToString() + ")");
		}
		
	}
}

