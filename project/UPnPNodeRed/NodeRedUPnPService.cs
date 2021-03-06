using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSource.UPnP;
using SampleDevice.NodeRedAPI;
using System;
using System.Configuration;

namespace OpenSource.DeviceBuilder {
    /// <summary>
    /// Transparent DeviceSide UPnP Service
    /// </summary>
    public class NodeRedUPnPService : IUPnPService {
        private NodeRedAPI api = new NodeRedAPI();

        // Place your declarations above this line

        #region AutoGenerated Code Section [Do NOT Modify, unless you know what you're doing]
        //{{{{{ Begin Code Block

        private _DvImportedService _S;
        public static string URN = "urn:schemas-upnp-org:service:NodeREDAPI:1";
        public double VERSION {
            get {
                return (double.Parse(_S.GetUPnPService().Version));
            }
        }

        public delegate void OnStateVariableModifiedHandler(NodeRedUPnPService sender);
        public event OnStateVariableModifiedHandler OnStateVariableModified_Hostname;
        public event OnStateVariableModifiedHandler OnStateVariableModified_Port;
        public System.String Hostname {
            get {
                return ((System.String)_S.GetStateVariable("Hostname"));
            }
            set {
                _S.SetStateVariable("Hostname", value);
            }
        }
        public System.String Port {
            get {
                return ((System.String)_S.GetStateVariable("Port"));
            }
            set {
                _S.SetStateVariable("Port", value);
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_Hostname {
            get {
                return (((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("Hostname")).Accumulator);
            }
            set {
                ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("Hostname")).Accumulator = value;
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_Port {
            get {
                return (((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("Port")).Accumulator);
            }
            set {
                ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("Port")).Accumulator = value;
            }
        }
        public double ModerationDuration_Hostname {
            get {
                return (((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("Hostname")).ModerationPeriod);
            }
            set {
                ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("Hostname")).ModerationPeriod = value;
            }
        }
        public double ModerationDuration_Port {
            get {
                return (((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("Port")).ModerationPeriod);
            }
            set {
                ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("Port")).ModerationPeriod = value;
            }
        }
        public delegate void Delegate_AddLink(System.String nodeSourceId, System.String nodeDestId, System.String flowId);
        public delegate void Delegate_AddNode(System.String node, System.String flowId);
        public delegate System.String Delegate_GetHostname();
        public delegate System.String Delegate_GetPort();
        public delegate void Delegate_RemoveLink(System.String nodeSourceId, System.String nodeDestId, System.String flowId);
        public delegate void Delegate_RemoveNode(System.String nodeId, System.String flowId);
        public delegate void Delegate_SetHostname(System.String hostname);
        public delegate void Delegate_SetPort(System.String port);
        public delegate System.String Delegate_GetNodes(System.String flowId);
        public delegate System.String Delegate_GetFlows();
        public delegate String Delegate_AddFlow(System.String flow);
        public delegate void Delegate_DeleteFlow(System.String flowId);
        public delegate void Delegate_UpdateNode(System.String node, System.String flowId);
        public delegate void Delegate_TestMethod();
        public delegate void Delegate_SetOutputEventState(bool state);


        public Delegate_AddLink External_AddLink = null;
        public Delegate_AddNode External_AddNode = null;
        public Delegate_GetHostname External_GetHostname = null;
        public Delegate_GetPort External_GetPort = null;
        public Delegate_RemoveLink External_RemoveLink = null;
        public Delegate_RemoveNode External_RemoveNode = null;
        public Delegate_SetHostname External_SetHostname = null;
        public Delegate_SetPort External_SetPort = null;
        public Delegate_GetNodes External_GetNodes = null;
        public Delegate_GetFlows External_GetFlows = null;
        public Delegate_AddFlow External_AddFlow = null;
        public Delegate_DeleteFlow External_DeleteFlow = null;
        public Delegate_UpdateNode External_UpdateNode = null;
        public Delegate_TestMethod External_TestMethod = null;
        public Delegate_SetOutputEventState External_SetOutputEventState = null;

        public void RemoveStateVariable_Hostname() {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("Hostname"));
        }
        public void RemoveStateVariable_Port() {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("Port"));
        }
        public void RemoveAction_AddLink() {
            _S.GetUPnPService().RemoveMethod("AddLink");
        }
        public void RemoveAction_AddNode() {
            _S.GetUPnPService().RemoveMethod("AddNode");
        }
        public void RemoveAction_GetHostname() {
            _S.GetUPnPService().RemoveMethod("GetHostname");
        }
        public void RemoveAction_GetPort() {
            _S.GetUPnPService().RemoveMethod("GetPort");
        }
        public void RemoveAction_RemoveLink() {
            _S.GetUPnPService().RemoveMethod("RemoveLink");
        }
        public void RemoveAction_RemoveNode() {
            _S.GetUPnPService().RemoveMethod("RemoveNode");
        }
        public void RemoveAction_SetHostname() {
            _S.GetUPnPService().RemoveMethod("SetHostname");
        }
        public void RemoveAction_SetPort() {
            _S.GetUPnPService().RemoveMethod("SetPort");
        }
        public void RemoveAction_GetNodes() {
            _S.GetUPnPService().RemoveMethod("GetNodes");
        }
        public void RemoveAction_GetFlows() {
            _S.GetUPnPService().RemoveMethod("GetFlows");
        }
        public void RemoveAction_AddFlow() {
            _S.GetUPnPService().RemoveMethod("AddFlow");
        }
        public void RemoveAction_UpdateNode() {
            _S.GetUPnPService().RemoveMethod("UpdateNode");
        }
        public System.Net.IPEndPoint GetCaller() {
            return (_S.GetUPnPService().GetCaller());
        }
        public System.Net.IPEndPoint GetReceiver() {
            return (_S.GetUPnPService().GetReceiver());
        }

        private class _DvImportedService {
            private NodeRedUPnPService Outer = null;
            private UPnPService S;
            internal _DvImportedService(NodeRedUPnPService n) {
                Outer = n;
                S = BuildUPnPService();
            }
            public UPnPService GetUPnPService() {
                return (S);
            }
            public void SetStateVariable(string VarName, object VarValue) {
                S.SetStateVariable(VarName, VarValue);
            }
            public object GetStateVariable(string VarName) {
                return (S.GetStateVariable(VarName));
            }
            protected UPnPService BuildUPnPService() {
                UPnPStateVariable[] RetVal = new UPnPStateVariable[3];
                RetVal[0] = new UPnPModeratedStateVariable("Hostname", typeof(System.String), false);
                RetVal[0].AddAssociation("GetHostname", "hostname");
                RetVal[0].AddAssociation("SetHostname", "hostname");
                RetVal[1] = new UPnPModeratedStateVariable("Port", typeof(System.String), false);
                RetVal[1].AddAssociation("GetPort", "port");
                RetVal[1].AddAssociation("SetPort", "port");
                RetVal[2] = new UPnPStateVariable("events", typeof(System.String), true);

                UPnPService S = new UPnPService(1, "urn:upnp-org:serviceId:NodeREDAPIService", URN, true, this);
                for (int i = 0; i < RetVal.Length; ++i) {
                    S.AddStateVariable(RetVal[i]);
                }
                S.AddMethod("AddLink");
                S.AddMethod("AddNode");
                S.AddMethod("UpdateNode");
                S.AddMethod("GetHostname");
                S.AddMethod("GetPort");
                S.AddMethod("RemoveLink");
                S.AddMethod("RemoveNode");
                S.AddMethod("SetHostname");
                S.AddMethod("SetPort");
                S.AddMethod("GetNodes");
                S.AddMethod("GetFlows");
                S.AddMethod("DeleteFlow");
                S.AddMethod("AddFlow");
                S.AddMethod("TestMethod");
                S.AddMethod("SetOutputEventState");
                return (S);
            }

            public void AddLink(System.String nodeSourceId, System.String nodeDestId, System.String flowId) {
                if (Outer.External_AddLink != null) {
                    Outer.External_AddLink(nodeSourceId, nodeDestId, flowId);
                } else {
                    Sink_AddLink(nodeSourceId, nodeDestId, flowId);
                }
            }
            public void AddNode(System.String node, System.String flowId) {
                if (Outer.External_AddNode != null) {
                    Outer.External_AddNode(node, flowId);
                } else {
                    Sink_AddNode(node, flowId);
                }
            }
            public void UpdateNode(System.String node, System.String flowId) {
                if (Outer.External_UpdateNode != null) {
                    Outer.External_UpdateNode(node, flowId);
                } else {
                    Sink_UpdateNode(node, flowId);
                }
            }
            [OpenSource.UPnP.ReturnArgument("hostname")]
            public System.String GetHostname() {
                object RetObj = null;
                if (Outer.External_GetHostname != null) {
                    RetObj = Outer.External_GetHostname();
                } else {
                    RetObj = Sink_GetHostname();
                }
                return ((System.String)RetObj);
            }
            [OpenSource.UPnP.ReturnArgument("port")]
            public System.String GetPort() {
                object RetObj = null;
                if (Outer.External_GetPort != null) {
                    RetObj = Outer.External_GetPort();
                } else {
                    RetObj = Sink_GetPort();
                }
                return ((System.String)RetObj);
            }
            public void RemoveLink(System.String nodeSourceId, System.String nodeDestId, System.String flowId) {
                if (Outer.External_RemoveLink != null) {
                    Outer.External_RemoveLink(nodeSourceId, nodeDestId, flowId);
                } else {
                    Sink_RemoveLink(nodeSourceId, nodeDestId, flowId);
                }
            }
            public void RemoveNode(System.String nodeId, System.String flowId) {
                if (Outer.External_RemoveNode != null) {
                    Outer.External_RemoveNode(nodeId, flowId);
                } else {
                    Sink_RemoveNode(nodeId, flowId);
                }
            }
            public void SetHostname(System.String hostname) {
                if (Outer.External_SetHostname != null) {
                    Outer.External_SetHostname(hostname);
                } else {
                    Sink_SetHostname(hostname);
                }
            }
            public void SetPort(System.String port) {
                if (Outer.External_SetPort != null) {
                    Outer.External_SetPort(port);
                } else {
                    Sink_SetPort(port);
                }
            }

            public System.String GetNodes(System.String flowId) {
                object RetObj = null;
                if (Outer.External_GetNodes != null) {
                    RetObj = Outer.External_GetNodes(flowId);
                } else {
                    RetObj = Sink_GetNodes(flowId);
                }
                return ((System.String)RetObj);
            }

            [OpenSource.UPnP.ReturnArgument("flows")]
            public System.String GetFlows() {
                object RetObj = null;
                if (Outer.External_GetFlows != null) {
                    RetObj = Outer.External_GetFlows();
                } else {
                    RetObj = Sink_GetFlows();
                }
                return ((System.String)RetObj);
            }

            public void DeleteFlow(System.String flowId) {
                if (Outer.External_DeleteFlow != null) {
                    Outer.External_DeleteFlow(flowId);
                } else {
                    Sink_DeleteFlow(flowId);
                }
            }

            [OpenSource.UPnP.ReturnArgument("newflowid")]
            public String AddFlow(System.String flow) {
                object RetObj = null;
                if (Outer.External_AddFlow != null) {
                    RetObj = Outer.External_AddFlow(flow);
                } else {
                    RetObj = Sink_AddFlow(flow);
                }
                return ((System.String)RetObj);
            }

            public void TestMethod() {
                if (Outer.External_TestMethod != null) {
                    Outer.External_TestMethod();
                } else {
                    Sink_TestMethod();
                }
            }

            public void SetOutputEventState(bool state) {
                if (Outer.External_SetOutputEventState != null) {
                    Outer.External_SetOutputEventState(state);
                } else {
                    Sink_SetOutputEventState(state);
                }
            }

            public Delegate_AddLink Sink_AddLink;
            public Delegate_AddNode Sink_AddNode;
            public Delegate_GetHostname Sink_GetHostname;
            public Delegate_GetPort Sink_GetPort;
            public Delegate_RemoveLink Sink_RemoveLink;
            public Delegate_RemoveNode Sink_RemoveNode;
            public Delegate_SetHostname Sink_SetHostname;
            public Delegate_SetPort Sink_SetPort;
            public Delegate_GetNodes Sink_GetNodes;
            public Delegate_GetFlows Sink_GetFlows;
            public Delegate_AddFlow Sink_AddFlow;
            public Delegate_UpdateNode Sink_UpdateNode;
            public Delegate_DeleteFlow Sink_DeleteFlow;
            public Delegate_TestMethod Sink_TestMethod;
            public Delegate_SetOutputEventState Sink_SetOutputEventState;
        }
        public NodeRedUPnPService() {
            _S = new _DvImportedService(this);
            _S.GetUPnPService().GetStateVariableObject("Hostname").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_Hostname);
            _S.GetUPnPService().GetStateVariableObject("Port").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_Port);
            _S.Sink_AddLink = new Delegate_AddLink(AddLink);
            _S.Sink_AddNode = new Delegate_AddNode(AddNode);
            _S.Sink_GetHostname = new Delegate_GetHostname(GetHostname);
            _S.Sink_GetPort = new Delegate_GetPort(GetPort);
            _S.Sink_RemoveLink = new Delegate_RemoveLink(RemoveLink);
            _S.Sink_RemoveNode = new Delegate_RemoveNode(RemoveNode);
            _S.Sink_SetHostname = new Delegate_SetHostname(SetHostname);
            _S.Sink_SetPort = new Delegate_SetPort(SetPort);
            _S.Sink_GetNodes = new Delegate_GetNodes(GetNodes);
            _S.Sink_GetFlows = new Delegate_GetFlows(GetFlows);
            _S.Sink_AddFlow = new Delegate_AddFlow(AddFlow);
            _S.Sink_DeleteFlow = new Delegate_DeleteFlow(DeleteFlow);
            _S.Sink_UpdateNode = new Delegate_UpdateNode(UpdateNode);
            _S.Sink_TestMethod = new Delegate_TestMethod(TestMethod);
            _S.Sink_SetOutputEventState = new Delegate_SetOutputEventState(SetOutputEventState);

            api.output_Event += Api_output_Event;
        }

        private void Api_output_Event(string NewValue) {
            _S.GetUPnPService().GetStateVariableObject("events").Value = NewValue;
        }

        public NodeRedUPnPService(string ID) : this() {
            _S.GetUPnPService().ServiceID = ID;
        }
        public UPnPService GetUPnPService() {
            return (_S.GetUPnPService());
        }
        private void OnModifiedSink_Hostname(UPnPStateVariable sender, object NewValue) {
            if (OnStateVariableModified_Hostname != null) OnStateVariableModified_Hostname(this);
        }
        private void OnModifiedSink_Port(UPnPStateVariable sender, object NewValue) {
            if (OnStateVariableModified_Port != null) OnStateVariableModified_Port(this);
        }
        //}}}}} End of Code Block

        #endregion

        /// <summary>
        /// Action: AddLink
        /// </summary>
        public void AddLink(System.String nodeSourceId, System.String nodeDestId, System.String flowId) {
            if (flowId == null) {
                throw (new UPnPCustomException(800, "No FlowID found"));
            }

            bool done = api.addLink(nodeSourceId, nodeDestId, flowId);

            if (!done) {
                throw (new UPnPCustomException(800, "Node source or destination id not found"));
            }
        }
        /// <summary>
        /// Action: AddNode
        /// </summary>
        public void AddNode(System.String node, System.String flowId) {
            if (flowId == null) {
                throw (new UPnPCustomException(800, "No FlowID found"));
            }

            bool done = api.addNode(JObject.Parse(node), flowId);
            if (!done) {
                throw (new UPnPCustomException(800, "Node id already exists"));
            }
        }

        public void UpdateNode(System.String node, System.String flowId) {
            if (flowId == null) {
                throw (new UPnPCustomException(800, "No FlowID found"));
            }

            dynamic realNode = JObject.Parse(node);

            if (node == null || realNode["id"] == null) {
                throw (new UPnPCustomException(800, "Node id not found"));
            }
            bool done = api.updateNode(realNode, flowId);
            if (!done) {
                throw (new UPnPCustomException(800, "Node id does not exist"));
            }
        }
        /// <summary>
        /// Action: GetHostname
        /// </summary>
        /// <returns>Associated StateVariable: Hostname</returns>
        public System.String GetHostname() {
            if (Hostname == null) {
                return ConfigurationManager.AppSettings["hostname"];
            }
            return Hostname;
        }
        /// <summary>
        /// Action: GetPort
        /// </summary>
        /// <returns>Associated StateVariable: Port</returns>
        public System.String GetPort() {
            if (Port == null) {
                return ConfigurationManager.AppSettings["port"];
            }
            return Port;
        }
        /// <summary>
        /// Action: RemoveLink
        /// </summary>
        public void RemoveLink(System.String nodeSourceId, System.String nodeDestId, System.String flowId) {
            if (flowId == null) {
                throw (new UPnPCustomException(800, "No FlowID found"));
            }

            bool done = api.removeLink(nodeSourceId, nodeDestId, flowId);

            if (!done) {
                throw (new UPnPCustomException(800, "Node source or destination id not found"));
            }
        }
        /// <summary>
        /// Action: RemoveNode
        /// </summary>
        public void RemoveNode(System.String nodeId, System.String flowId) {
            if (flowId == null) {
                throw (new UPnPCustomException(800, "No FlowID found"));
            }

            api.removeNode(nodeId, flowId);
        }
        /// <summary>
        /// Action: SetHostname
        /// </summary>
        /// <param name="hostname">Associated State Variable: Hostname</param>
        public void SetHostname(System.String hostname) {
            Hostname = hostname;
            api.hostname = hostname;
        }
        /// <summary>
        /// Action: SetPort
        /// </summary>
        /// <param name="port">Associated State Variable: Port</param>
        public void SetPort(System.String port) {
            Port = port;
            api.port = port;
        }

        public System.String GetNodes(System.String flowId) {
            if (flowId == null) {
                throw (new UPnPCustomException(800, "No FlowID found"));
            }
            JArray nodes = api.getNodes(flowId);

            System.String response = JsonConvert.SerializeObject(nodes);

            return response;
        }

        public System.String GetFlows() {
            JArray flows = api.getFlows();

            System.String response = flows.ToString();

            return response;
        }

        public String AddFlow(System.String flow) {
            return api.addFlow(JObject.Parse(flow));
        }

        public void DeleteFlow(System.String flowId) {
            api.DeleteFlow(flowId);
        }

        private int val = 0;
        public void TestMethod() {
            api.ResetState();
        }

        public void SetOutputEventState(bool state) {
            Console.WriteLine("SetOutputEventState " + state);
            api.OutputEventState = state;
            if (!state) {
                api.RefreshState();
            }
        }
    }
}