//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WComp.UPnPDevice {
    using System;
    using OpenSource.UPnP;
    using WComp.Beans;


    [Bean("UPnP Device")]
    public class NodeREDUPnPBean : IProxyBean {

        private string _uri = "http://192.168.56.1:36603/";

        private bool _isConnected = false;

        private OpenSource.UPnP.UPnPDevice _device = null;

        private OpenSource.UPnP.UPnPDeviceFactory _factory = null;

        private OpenSource.UPnP.UPnPService _upnporgId = null;

        private OpenSource.UPnP.UPnPStateVariable _events_Subscription = null;

        private OpenSource.UPnP.UPnPArgument _flows_from_GetFlows = null;

        private OpenSource.UPnP.UPnPArgument _hostname_from_GetHostname = null;

        private OpenSource.UPnP.UPnPArgument @__ReturnValue_from_GetNodes = null;

        private OpenSource.UPnP.UPnPArgument _port_from_GetPort = null;

        public NodeREDUPnPBean() {
            this._TryToConnect();
        }

        public bool IsConnected {
            get {
                return this._isConnected;
            }
        }

        public string Uri {
            get {
                return this._uri;
            }
            set {
                this._Disconnect();
                this._uri = value;
                if ((String.IsNullOrEmpty(this._uri) == false)) {
                    this._TryToConnect();
                }
            }
        }

        public event VoidEventHandler _DeviceOK;

        public event VoidEventHandler _DeviceFailed;

        public event StringEventHandler _UDN;

        public event _upnporgId_events_Handler events_Event;

        public event _upnporgId_GetFlows_ReturnHandler GetFlows_Return;

        public event _upnporgId_GetHostname_ReturnHandler GetHostname_Return;

        public event _upnporgId_GetNodes_ReturnHandler GetNodes_Return;

        public event _upnporgId_GetPort_ReturnHandler GetPort_Return;

        public virtual void _GetUDN() {
            if (((this._UDN != null)
                        && (this._device != null))) {
                this._UDN(this._device.UniqueDeviceName);
            }
        }

        private void _FireDeviceOK(OpenSource.UPnP.UPnPDeviceFactory factory, OpenSource.UPnP.UPnPDevice device, System.Uri uri) {
            this._device = device;
            this._isConnected = true;
            if ((this._DeviceOK != null)) {
                this._DeviceOK();
            }
            this._upnporgId = this._device.GetService("urn:upnp-org:serviceId:NodeREDAPIService");
            this._upnporgId.Subscribe(60, new OpenSource.UPnP.UPnPService.UPnPEventSubscribeHandler(this._upnporgId_SubscribeServiceOK));
        }

        private void _FireDeviceFailed(OpenSource.UPnP.UPnPDeviceFactory factory, System.Uri uri, System.Exception e) {
            if (((e != null)
                        && (e.Message == "Timeout occured trying to fetch description documents"))) {
                return;
            }
            this._isConnected = false;
            if ((this._DeviceFailed != null)) {
                this._DeviceFailed();
            }
        }

        public void _TryToConnect() {
            this._Disconnect();
            this._factory = new OpenSource.UPnP.UPnPDeviceFactory(new System.Uri(this._uri), 900, new OpenSource.UPnP.UPnPDeviceFactory.UPnPDeviceHandler(this._FireDeviceOK), new OpenSource.UPnP.UPnPDeviceFactory.UPnPDeviceFailedHandler(this._FireDeviceFailed));
        }

        public void _Disconnect() {
            if ((this._factory != null)) {
                this._factory.Shutdown();
            }
            this._FireDeviceFailed(null, null, null/*, null*/);
        }

        private void _upnporgId_SubscribeServiceOK(OpenSource.UPnP.UPnPService sender, bool OK) {
            this._events_Subscription = this._upnporgId.GetStateVariableObject("events");
            this._events_Subscription.OnModified += new OpenSource.UPnP.UPnPStateVariable.ModifiedHandler(this.Fire_events_Evented);
        }

        private void Fire_events_Evented(OpenSource.UPnP.UPnPStateVariable sender, object NewValue) {
            if ((this.events_Event != null)) {
                this.events_Event(((string)(NewValue)));
            }
        }

        public void AddFlow(string flow) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("AddFlow", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("flow", flow)});
            }
        }

        public void AddLink(string nodeSourceId, string nodeDestId, string flowId) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("AddLink", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("nodeSourceId", nodeSourceId),
                            new OpenSource.UPnP.UPnPArgument("nodeDestId", nodeDestId),
                            new OpenSource.UPnP.UPnPArgument("flowId", flowId)});
            }
        }

        public void AddNode(string node, string flowId) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("AddNode", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("node", node),
                            new OpenSource.UPnP.UPnPArgument("flowId", flowId)});
            }
        }

        public void DeleteFlow(string flowId) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("DeleteFlow", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("flowId", flowId)});
            }
        }

        private void GetFlowsInvokeSuccess(OpenSource.UPnP.UPnPService sender, string MethodName, OpenSource.UPnP.UPnPArgument[] Args, object ReturnValue, object Tag) {
            if ((this.GetFlows_Return != null)) {
                this.GetFlows_Return(((string)(ReturnValue)));
            }
        }

        private void GetFlowsInvokeError(OpenSource.UPnP.UPnPService sender, string MethodName, OpenSource.UPnP.UPnPArgument[] Args, OpenSource.UPnP.UPnPInvokeException e, object Tag) {
        }

        public void GetFlows() {
            if ((this._upnporgId != null)) {
                this._flows_from_GetFlows = new OpenSource.UPnP.UPnPArgument("flows", null);
                this._upnporgId.InvokeAsync("GetFlows", new OpenSource.UPnP.UPnPArgument[0], null, new OpenSource.UPnP.UPnPService.UPnPServiceInvokeHandler(this.GetFlowsInvokeSuccess), new OpenSource.UPnP.UPnPService.UPnPServiceInvokeErrorHandler(this.GetFlowsInvokeError));
            }
        }

        private void GetHostnameInvokeSuccess(OpenSource.UPnP.UPnPService sender, string MethodName, OpenSource.UPnP.UPnPArgument[] Args, object ReturnValue, object Tag) {
            if ((this.GetHostname_Return != null)) {
                this.GetHostname_Return(((string)(ReturnValue)));
            }
        }

        private void GetHostnameInvokeError(OpenSource.UPnP.UPnPService sender, string MethodName, OpenSource.UPnP.UPnPArgument[] Args, OpenSource.UPnP.UPnPInvokeException e, object Tag) {
        }

        public void GetHostname() {
            if ((this._upnporgId != null)) {
                this._hostname_from_GetHostname = new OpenSource.UPnP.UPnPArgument("hostname", null);
                this._upnporgId.InvokeAsync("GetHostname", new OpenSource.UPnP.UPnPArgument[0], null, new OpenSource.UPnP.UPnPService.UPnPServiceInvokeHandler(this.GetHostnameInvokeSuccess), new OpenSource.UPnP.UPnPService.UPnPServiceInvokeErrorHandler(this.GetHostnameInvokeError));
            }
        }

        private void GetNodesInvokeSuccess(OpenSource.UPnP.UPnPService sender, string MethodName, OpenSource.UPnP.UPnPArgument[] Args, object ReturnValue, object Tag) {
            if ((this.GetNodes_Return != null)) {
                this.GetNodes_Return(((string)(ReturnValue)));
            }
        }

        private void GetNodesInvokeError(OpenSource.UPnP.UPnPService sender, string MethodName, OpenSource.UPnP.UPnPArgument[] Args, OpenSource.UPnP.UPnPInvokeException e, object Tag) {
        }

        public void GetNodes(string flowId) {
            if ((this._upnporgId != null)) {
                this.@__ReturnValue_from_GetNodes = new OpenSource.UPnP.UPnPArgument("_ReturnValue", null);
                this._upnporgId.InvokeAsync("GetNodes", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("flowId", flowId)}, null, new OpenSource.UPnP.UPnPService.UPnPServiceInvokeHandler(this.GetNodesInvokeSuccess), new OpenSource.UPnP.UPnPService.UPnPServiceInvokeErrorHandler(this.GetNodesInvokeError));
            }
        }

        private void GetPortInvokeSuccess(OpenSource.UPnP.UPnPService sender, string MethodName, OpenSource.UPnP.UPnPArgument[] Args, object ReturnValue, object Tag) {
            if ((this.GetPort_Return != null)) {
                this.GetPort_Return(((string)(ReturnValue)));
            }
        }

        private void GetPortInvokeError(OpenSource.UPnP.UPnPService sender, string MethodName, OpenSource.UPnP.UPnPArgument[] Args, OpenSource.UPnP.UPnPInvokeException e, object Tag) {
        }

        public void GetPort() {
            if ((this._upnporgId != null)) {
                this._port_from_GetPort = new OpenSource.UPnP.UPnPArgument("port", null);
                this._upnporgId.InvokeAsync("GetPort", new OpenSource.UPnP.UPnPArgument[0], null, new OpenSource.UPnP.UPnPService.UPnPServiceInvokeHandler(this.GetPortInvokeSuccess), new OpenSource.UPnP.UPnPService.UPnPServiceInvokeErrorHandler(this.GetPortInvokeError));
            }
        }

        public void RemoveLink(string nodeSourceId, string nodeDestId, string flowId) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("RemoveLink", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("nodeSourceId", nodeSourceId),
                            new OpenSource.UPnP.UPnPArgument("nodeDestId", nodeDestId),
                            new OpenSource.UPnP.UPnPArgument("flowId", flowId)});
            }
        }

        public void RemoveNode(string nodeId, string flowId) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("RemoveNode", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("nodeId", nodeId),
                            new OpenSource.UPnP.UPnPArgument("flowId", flowId)});
            }
        }

        public void SetHostname(string hostname) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("SetHostname", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("hostname", hostname)});
            }
        }

        public void SetPort(string port) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("SetPort", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("port", port)});
            }
        }

        public void TestMethod() {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("TestMethod", new OpenSource.UPnP.UPnPArgument[0]);
            }
        }

        public void SetOutputEventState(bool state) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("SetOutputEventState", new OpenSource.UPnP.UPnPArgument[]{new OpenSource.UPnP.UPnPArgument("state", state)});
            }
        }

        public void UpdateNode(string node, string flowId) {
            if ((this._upnporgId != null)) {
                this._upnporgId.InvokeAsync("UpdateNode", new OpenSource.UPnP.UPnPArgument[] {
                            new OpenSource.UPnP.UPnPArgument("node", node),
                            new OpenSource.UPnP.UPnPArgument("flowId", flowId)});
            }
        }

        public delegate void _upnporgId_events_Handler(string NewValue);

        public delegate void _upnporgId_GetFlows_ReturnHandler(string flows);

        public delegate void _upnporgId_GetHostname_ReturnHandler(string hostname);

        public delegate void _upnporgId_GetNodes_ReturnHandler(string _ReturnValue);

        public delegate void _upnporgId_GetPort_ReturnHandler(string port);
    }
}
