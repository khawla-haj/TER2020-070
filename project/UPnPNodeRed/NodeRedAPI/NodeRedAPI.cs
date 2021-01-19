using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using WebSocketSharp;

namespace SampleDevice.NodeRedAPI {
#pragma warning disable S101 // Types should be named in PascalCase
    public class NodeRedAPI
#pragma warning restore S101 // Types should be named in PascalCase
    {
        private readonly string urlDefault = ConfigurationManager.AppSettings["hostname"] + ":" + ConfigurationManager.AppSettings["port"] + "/";
        public string UrlBase {
            get {
                if (hostname == null) {
                    return "http://" + urlDefault;
                } else {
                    string url = "http://" + hostname;
                    if (port != null) {
                        url += ":" + port;
                    } else {
                        url += ":" + ConfigurationManager.AppSettings["port"];
                    }
                    url += "/";
                    return url;
                }
            }
        }
        public string ServerBase {
            get {
                if (hostname == null) {
                    return urlDefault;
                } else {
                    string url = hostname;
                    if (port != null) {
                        url += ":" + port;
                    } else {
                        url += ":" + ConfigurationManager.AppSettings["port"];
                    }
                    url += "/";
                    return url;
                }
            }
        }

        private string m_hostname;
        private string m_port;
        public string hostname { get { return m_hostname; } set { m_hostname = value; WSConnect(); } }
        public string port { get { return m_port; } set { m_port = value; WSConnect();  } }

        public delegate void OutputEventHandler(string NewValue);
        public event OutputEventHandler output_Event;
        public NodeRedAPI() {
            output_Event += (val) => Console.WriteLine("output::" + val);
            WSConnect();
            StartEventEmissionTimer();
        }

        private readonly object WSMessageLock = new object();
        public void WSConnect() {
            lastFlowSet = new FlowSet();
            Console.WriteLine("Connecting to WS server ws://" + ServerBase + "comms");
            WebSocket WS;
            WS = new WebSocket("ws://" + ServerBase + "comms");
            WS.OnMessage += WS_OnMessage;
            WS.OnOpen += WS_OnOpen;
            WS.Connect();
        }

        private class Bean {
            public string Name;
            public string Id;
            public List<Port> Inputs;
            public List<Port> Outputs;
            public JObject Flow;
            public string Type;

            public override bool Equals(object obj) {
                return ((Bean)obj).Id == this.Id;
            }
        }
        private class Link {
            public Port From;
            public Port To;

            public string Name {
                get {
                    return "link-" + From?.Bean.Name + "-" + To?.Bean.Name + "-" + From?.LinkName + "-" + To?.LinkName + "-";
                }
            }

            public override bool Equals(object obj) {
                return ((Link)obj)?.From?.Id == this?.From?.Id && ((Link)obj)?.To?.Id == this?.To?.Id;
            }
        }
        private class Port {
            public string Id;
            public Bean Bean;
            public string Name;

            public string LinkName {
                get {
                    return Name.Replace("linkOut_", "").Replace("linkIn_", "");
                }
            }
        }
        private class FlowSet {
            public List<Bean> Beans;
            public List<Link> Links;
            public FlowSet() {
                Beans = new List<Bean>();
                Links = new List<Link>();
            }
            public bool BeanExists(Bean bean) {
                return Beans.FindIndex(b => b.Equals(bean)) != -1;
            }
            public bool LinkExists(Link link) {
                return Links.FindIndex(l => l.Equals(link)) != -1;
            }
            public static FlowSet fromJSON(JArray json, NodeRedAPI api) {
                FlowSet ret = new FlowSet();

                // get beans and their flows
                foreach (JToken flow in json) {
                    // get the full flow
                    JObject beanFlow = api.getFlowRaw(flow["id"].Value<string>());
                    // skip non UWD flows that dont have the ID in their info field
                    if (!beanFlow.ContainsKey("info") || !Guid.TryParse(beanFlow["info"].Value<string>(), out _)) { Console.WriteLine($"Skipping flow {beanFlow["label"].Value<string>()}"); continue; }

                    // extract inputs and outputs
                    List<Port> inputs = beanFlow["nodes"].ToList().FindAll(n => n["type"].Value<string>() == "link in").Select(n => new Port {Id = n["id"].Value<string>(), Bean = null, Name = n["name"].Value<string>() }).ToList(),
                        outputs = beanFlow["nodes"].ToList().FindAll(n => n["type"].Value<string>() == "link out").Select(n => new Port { Id = n["id"].Value<string>(), Bean = null, Name = n["name"].Value<string>() }).ToList();

                    // add the bean
                    JToken wcwrapper = beanFlow["nodes"].ToList().Find(n => n["type"].Value<string>() == "wcomp-wrapper");
                    string type;
                    if (wcwrapper == null) {
                        type = beanFlow["nodes"].ToList().Find(n => n["name"].Value<string>() == "beantype")["info"].Value<string>();
                    } else {
                        type = wcwrapper["bean"].Value<string>();
                    }
                    Bean b = new Bean() {
                        Name = flow["label"].Value<string>(),
                        Id = flow["id"].Value<string>(),
                        Flow = beanFlow,
                        Inputs = inputs,
                        Outputs = outputs,
                        Type = type
                    };
                    foreach(Port p in b.Inputs) { p.Bean = b; }
                    foreach (Port p in b.Outputs) { p.Bean = b; }
                    ret.Beans.Add(b);
                }

                
                foreach(Bean bean in ret.Beans) {
                    JObject currentFlow = api.getFlowRaw(bean.Id);
                    foreach(JToken node in currentFlow["nodes"]){
                        if(node["type"].Value<string>() == "link out") {
                            foreach(JToken link in node["links"]) {
                                Link newLink = new Link {
                                    From = ret.Beans.Select((bn) => bn.Outputs.Find(b => b.Id == node["id"].Value<string>())).FirstOrDefault(t => t!= null),
                                    To = ret.Beans.Select((bn) => bn.Inputs.Find(b => b.Id == link.Value<string>())).FirstOrDefault(t => t != null)
                                };
                                // skip links going outside of the beans for now
                                if (newLink.From == null || newLink.To == null) continue;

                                ret.Links.Add(newLink);
                            }
                        }
                    }
                }

                return ret;
            }

            public override string ToString() {
                string ret = "FlowSet::\n";
                foreach(Bean bean in Beans) {
                    ret += $"\t Bean: {bean.Id} ({bean.Name})\n";
                }
                foreach(Link link in Links) {
                    ret += $"\t Link: {link.From?.Id}({link.From?.Name}) - {link.To?.Id}({link.To?.Name})\n";
                }
                return ret;
            }
        }
        private FlowSet lastFlowSet = new FlowSet();
        private int EventSeqNumber = 0;
        public bool OutputEventState { get; set; } = true;
        private void WS_OnMessage(object sender, MessageEventArgs e) {
            if (e.IsText) {
                //Console.Write("WS_OnMessage:");
                string messageStr = Encoding.UTF8.GetString(e.RawData);
                JArray messages = JsonConvert.DeserializeObject<JArray>(messageStr);
               // Console.WriteLine(messageStr);

                foreach (var message in messages) {
                    switch (message["topic"].ToString()) {
                        case "notification/runtime-deploy":
                            lock (WSMessageLock) {
                                RefreshState();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void WS_OnOpen(object sender, EventArgs e) {
            Console.WriteLine("WS_OnOpen");
            RefreshState();
        }
        public void ResetState() {
            lastFlowSet = new FlowSet();
            RefreshState();
        }
        public void RefreshState() {
            Console.WriteLine("RefreshState");
            // skip if suppressed
            if (!OutputEventState) return;
            // Get the new flow state
            JArray flows = getFlows();

            FlowSet currentFlowSet = FlowSet.fromJSON(flows, this);
            Console.WriteLine("CFS::");
            Console.WriteLine(currentFlowSet);

            // Firing events based on WCompNetControlDevice#AppliMonitor (L. 365+)
            // Only reimplementing NEW_BEAN, DEL_BEAN, NEW_LINK and DEL_LINK

            lock (lastFlowSet) {
                // process new things
                foreach(Bean bean in currentFlowSet.Beans) {
                    if (!lastFlowSet.BeanExists(bean)) {
                        // new bean
                        if (bufferEvents) {
                            lock (eventBufferLock) {
                                eventBuffer.Add(String.Format("{0}|NEW_BEAN|{1}|{2}", EventSeqNumber++, bean.Name, bean.Type));
                            }
                        } else {
                            output_Event?.Invoke(String.Format("{0}|NEW_BEAN|{1}|{2}", EventSeqNumber++, bean.Name, bean.Type));
                        }
                    }
                }
                foreach(Link link in currentFlowSet.Links) {
                    if (!lastFlowSet.LinkExists(link)) {
                        // new link
                        if (bufferEvents) {
                            lock (eventBufferLock) {
                                eventBuffer.Add(String.Format("{0}|NEW_LINK|{1}", EventSeqNumber++, link.Name));
                            }
                        } else {
                            output_Event?.Invoke(String.Format("{0}|NEW_LINK|{1}", EventSeqNumber++, link.Name));
                        }
                    }
                }

                // process deleted things
                foreach (Bean bean in lastFlowSet.Beans) {
                    if (!currentFlowSet.BeanExists(bean)) {
                        // new bean
                        if (bufferEvents) {
                            lock (eventBufferLock) {
                                eventBuffer.Add(String.Format("{0}|DEL_BEAN|{1}", EventSeqNumber++, bean.Name));
                            }
                        } else {
                            output_Event?.Invoke(String.Format("{0}|DEL_BEAN|{1}", EventSeqNumber++, bean.Name));
                        }
                    }
                }
                foreach (Link link in lastFlowSet.Links) {
                    if (!currentFlowSet.LinkExists(link)) {
                        // new link
                        if (bufferEvents) {
                            lock (eventBufferLock) {
                                eventBuffer.Add(String.Format("{0}|DEL_LINK|{1}", EventSeqNumber++, link.Name));
                            }
                        } else {
                            output_Event?.Invoke(String.Format("{0}|DEL_LINK|{1}", EventSeqNumber++, link.Name));
                        }
                    }
                }

                // store current model
                lastFlowSet = currentFlowSet;
            }
        }

        private bool bufferEvents = true;
        private object eventBufferLock = new object();
        private List<string> eventBuffer = new List<string>();
        private Timer eventTimer;
        private void StartEventEmissionTimer() {
            if (bufferEvents) {
                // 500ms timer 
                eventTimer = new Timer((_) => {
                    lock (eventBufferLock) {
                        // if there are events send the first
                        if(eventBuffer.Count > 0) {
                            output_Event?.Invoke(eventBuffer[0]);
                            eventBuffer.RemoveAt(0);
                        }
                    }
                }, null, 0, 500);
            }
        }

        public bool addNode(dynamic node, string flowId) {
            if (flowId != null) {
                dynamic flow = getDynamicFlow(flowId);
                if (node.id == null) {
                    node.id = generateRandomNodeId();
                } else if (!(node.id is string)) {
                    node.id = node.id.ToString();
                } else {
                    if (isFlowContainsNode(flow, node.id)) {
                        return false;
                    }
                }
                flow["nodes"].Add(JToken.FromObject(node));
                //string body = JsonConvert.SerializeObject(flow);

                string response = launchRequest("flow/" + flowId, "PUT", flow.ToString());
                return true;
            }
            return false;
        }

        public bool updateNode(dynamic node, string flowId) {
            if (flowId != null) {
                dynamic flow = getDynamicFlow(flowId);
                string nodeId = node["id"];
                if (!isFlowContainsNode(flow, nodeId)) {
                    return false;
                }

                dynamic n = getNodeFromFlow(flow, nodeId);
                if (n != null) {
                    flow = removeNodeFromFlow(nodeId, flow);
                    flow["nodes"].Add(JToken.FromObject(node));
                } else {
                    return false;
                }


                string response = launchRequest("flow/" + flowId, "PUT", flow.ToString());
                return true;
            }
            return false;
        }

        public void removeNode(string id, string flowId) {
            if (flowId != null) {
                dynamic flow = getDynamicFlow(flowId);
                dynamic node = getNodeFromFlow(flow, id);
                Console.WriteLine(node);

                if (node != null) {
                    //flow["nodes"].Remove(JToken.FromObject(node));
                    /*JArray nodes = flow["nodes"];
                    List<JToken> newArray = nodes.ToObject<List<JToken>>();
                    Console.WriteLine(nodes);
                    newArray.Remove(JToken.FromObject(node));
                    flow["nodes"] = JArray.FromObject(newArray);
                    Console.WriteLine(flow["nodes"]);*/
                    flow = removeNodeFromFlow(id, flow);
                }

                string response = launchRequest("flow/" + flowId, "PUT", flow.ToString());
            }
        }

        public bool addLink(string nodeIdSource, string nodeIdDest, string flowId) {
            if (flowId != null) {
                dynamic flow = getDynamicFlow(flowId);
                bool done = addLinkToFlow(nodeIdSource, nodeIdDest, flow);
                if (done) {
                    string response = launchRequest("flow/" + flowId, "PUT", flow.ToString());
                }

                return done;
            }
            return false;
        }

        public bool removeLink(string nodeIdSource, string nodeIdDest, string flowId) {
            Console.WriteLine("removelink : " + nodeIdSource + "/" + nodeIdDest + " ::" + flowId);
            if (flowId != null) {
                dynamic flow = getDynamicFlow(flowId);
                dynamic source = getNodeFromFlow(flow, nodeIdSource);

                if (source != null) {
                    List<string> newArray = source.wires[0].ToObject<List<string>>();
                    newArray.Remove(nodeIdDest);
                    source.wires[0] = JArray.FromObject(newArray);
                    Console.WriteLine(source.ToString());
                    string response = launchRequest("flow/" + flowId, "PUT", flow.ToString());
                    return true;

                }
                return false;
            }
            return false;
        }

        public JArray getNodes(string flowId) {
            Console.WriteLine("getNodes::" + flowId);
            if (flowId != null) {
                dynamic flow = getDynamicFlow(flowId);

                return flow["nodes"];
            }
            return null;
        }

        public Flow getFlow(string id) {
            if (id != null) {
                string response = launchRequest("flow/" + id, "GET", null);
                Flow flow = JsonConvert.DeserializeObject<Flow>(response);
                return flow;
            }
            return null;
        }

        public JObject getFlowRaw(string id) {
            if (id != null) {
                string response = launchRequest("flow/" + id, "GET", null);
                JObject flow = JObject.Parse(response);
                return flow;
            }
            return null;
        }

        public dynamic getDynamicFlow(string id) {
            if (id != null) {
                string response = launchRequest("flow/" + id, "GET", null);
                dynamic flow = JObject.Parse(response);
                return flow;
            }
            return null;
        }

        public JArray getFlows() {
            Console.WriteLine("getFlows");
            string response = launchRequest("flows", "GET", null);
            JArray flows = JArray.Parse(response);
            for (int i = 0; i < flows.Count; i++) {
                string type = (string)flows.ElementAt(i)["type"];
                if (type != "tab") {
                    List<JToken> flowsList = flows.ToObject<List<JToken>>();
                    flowsList.RemoveAt(i);
                    flows = JArray.FromObject(flowsList);
                    i--;
                }
            }

            return flows;
        }

        public String addFlow(dynamic flow) {
            //flow["id"] = null;
            if (flow.nodes != null && flow.nodes.Count > 0) {
                foreach (dynamic node in flow.nodes) {
                    if (node.id == null) {
                        node.id = generateRandomNodeId();
                    }
                }
            }
            //Console.WriteLine(flow.ToString());
            return launchRequest("flow", "POST", flow.ToString());
        }

        public void DeleteFlow(string flowId) {
            string response = launchRequest("flows", "GET", null);
            JArray flows = JArray.Parse(response);
            JArray newFlows = new JArray();
            Console.WriteLine("DeleteFlow::" + flowId);
            foreach(dynamic node in flows) {
                if(node.id != flowId && node.z != flowId) {
                    newFlows.Add(node);
                }
            }
            launchRequest("flows", "POST", newFlows.ToString());
        }

        private string launchRequest(string endRequest, string methodRequest, string body) {
            string url = UrlBase + endRequest;

            WebRequest request = WebRequest.Create(url);
            request.Method = methodRequest;
            request.ContentType = "application/json";

            if (body != null) {
                byte[] data = Encoding.Default.GetBytes(body);
                request.ContentLength = data.Length;

                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
            }

            WebResponse response = null;
            try {
                response = request.GetResponse();
            } catch (Exception _) {
                Console.WriteLine("No response");
            }

            if (response != null) {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();

                return responseFromServer;
            }

            return "{}";
        }

        public static string generateRandomNodeId() {
            return (GetRandomHexNumber(8) + "." + GetRandomHexNumber(6)).ToLower();
        }

        public static string GetRandomHexNumber(int digits) {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        public static bool isFlowContainsNode(dynamic flow, string nodeId) {
            if (nodeId == null) {
                return false;
            }

            foreach (dynamic n in flow["nodes"]) {
                if (n.id == nodeId) {
                    return true;
                }
            }
            return false;
        }
        public static dynamic getNodeFromFlow(dynamic flow, string nodeId) {
            if (flow == null || flow["nodes"] == null || flow["nodes"].Count == 0) {
                return null;
            }

            for (int i = 0; i < flow["nodes"].Count; i++) {
                dynamic n = flow["nodes"][i];
                if (n.id == nodeId) {
                    return n;
                }
            }
            return null;
        }
        public static bool addLinkToFlow(string sourceId, string destId, dynamic flow) {
            if (flow == null || sourceId == null || destId == null) {
                return false;
            }

            if (!isFlowContainsNode(flow, sourceId) || !isFlowContainsNode(flow, destId)) {
                return false;
            }

            dynamic source = getNodeFromFlow(flow, sourceId);

            if (source == null) {
                return false;
            }

            source.wires[0].Add(destId);
            return true;
        }
        public static dynamic removeNodeFromFlow(string nodeId, dynamic flow) {
            if (flow != null && flow["nodes"] != null && nodeId != null) {
                for (int i = 0; i < flow["nodes"].Count; i++) {
                    dynamic n = flow["nodes"][i];
                    if (n.id == nodeId) {
                        JArray nodes = flow["nodes"];
                        List<JToken> newArray = nodes.ToObject<List<JToken>>();
                        newArray.RemoveAt(i);
                        /*foreach (JToken t in newArray) {
                            Console.WriteLine(t["id"]);
                        }*/
                        flow["nodes"] = JArray.FromObject(newArray);
                        //Console.WriteLine(flow["nodes"]);
                        return flow;
                    }
                }
            }
            return flow;
        }
    }
}