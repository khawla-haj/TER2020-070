using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using OpenSource.UPnP;
using SampleDevice.NodeRedAPI;

namespace SampleDevice.BeanToFlow.UPnP
{

    class UPnPFlow
    {
        private string flow_name;
        private string device_name;
        private string flow_id;
        private NodeRedAPI.NodeRedAPI api;
        private int link_in_x = 155;
        private int invoke_action_x = 1055;
        private int link_in_function_x = 550;
        private int event_x = 300;
        private int link_out_function_x = 800;
        private int link_out_x = 1055;
        private int y = 100;
        public ArrayList baseurl_ips = new ArrayList();
        private string upnp_configuration_id = "";
        public ArrayList UPnP_events = new ArrayList();
        public int num = 0;


        public UPnPFlow(string flow_name,string device_name, NodeRedAPI.NodeRedAPI api)
        {
            this.flow_name = flow_name;
            this.device_name = device_name;
            this.api = api;
            FindDevice();
            //this.AddUPnPFlow();
        }

        public static string GetRandomString(int length)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null,
            str = "0123456789";
            str += "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        public void OnDeviceAdded(UPnPSmartControlPoint cp, UPnPDevice device)
        {
            //Console.WriteLine(device.FriendlyName);
            if (device.FriendlyName.Equals(this.device_name))
            {
                //Console.WriteLine(device.FriendlyName);
                this.flow_id = this.api.addFlowBlank(this.flow_name);
                this.AddUPnPConfiguration(device.UniqueDeviceName);

                JObject flow = new JObject { { "id", flow_id }, { "label", flow_name }, { "disabled", false }, { "info", "" } };
                JArray nodes = new JArray();


                //nodes.Add(event_node);


                foreach (UPnPService s in device.Services)
                {
                    AddUPnPEventNodes(nodes, s);
                    //y += 80;
                    foreach (UPnPAction a in s.Actions)
                    {

                        AddUPnPActionNodes(nodes, a);
                        y += 80;
                    }
                }
                string li_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
                string lo_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
                string st_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
                JObject li = new JObject { { "id", li_id }, { "type", "link in" }, { "z", flow_id }, { "name", "link_in_ToString" }, { "links", new JArray("06abjfe7.3c2ib2") }, { "x", 185 }, { "y", 40 }, { "wires", new JArray { new JArray(st_id) } } };
                JObject lo = new JObject { { "id", lo_id }, { "type", "link out" }, { "z", flow_id }, { "name", "link_out_ToString" }, { "links", new JArray("ku7k2kv1.r1ylfh") }, { "x", 865 }, { "y", 40 } };
                JObject ts = new JObject { { "id", st_id }, { "type", "function" }, { "z", flow_id }, { "name", "ToString" }, { "func", "var parameters = new Array();\ns = \""+device.FriendlyName+"\";\nparameters.push(s);\nmsg.payload = parameters;\nreturn msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", 550 }, { "y", 40 }, { "wires", new JArray { new JArray(lo_id) } } };
                nodes.Add(li);
                nodes.Add(lo);
                nodes.Add(ts);
                //Console.WriteLine(nodes.ToString());
                flow.Add("nodes", nodes);
                api.launchRequest("flow/" + flow_id, "PUT", flow.ToString());
            }

            //baseurl_ips.Add(device.BaseURL.ToString());
        }

        public void FindDevice()
        {
            UPnPSmartControlPoint ControlPoint = new UPnPSmartControlPoint(OnDeviceAdded);

        }
        

        public void AddUPnPConfiguration(string uuid)
        {
            //Console.WriteLine(uuid);
            upnp_configuration_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string response = api.launchRequest("flows", "GET", null);
            JArray flows = JArray.Parse(response);

            JObject upnpconfig_jo = new JObject { { "id", upnp_configuration_id }, { "type", "upnp-configuration" }, { "uuid", uuid }, { "useHardcodedDeviceDescriptionURL", false }, { "deviceDescriptionURL", "" }, { "name", uuid+"_config" } };
            flows.Add(JToken.FromObject(upnpconfig_jo));
            api.launchRequest("flows", "POST", flows.ToString());


        }

        public void AddUPnPEventNodes(JArray nodes, UPnPService s)
        {
            

            foreach (UPnPStateVariable a in s.GetStateVariables())
            {
                if (a.SendEvent)
                {
                    string upnp_event_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
                    string function_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
                    string link_out_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
                    string urn = s.ServiceURN;

                    JObject event_node = new JObject { { "id", upnp_event_id }, { "type", "upnp-receiveEvent" }, { "z", flow_id }, { "upnpConfiguration", upnp_configuration_id }, { "serviceType", urn }, { "name", s.ServiceID }, { "x", event_x }, { "y", y }, { "wires", new JArray { new JArray(function_id) } } };
                    JObject function_node = new JObject { { "id", function_id }, { "type", "function" }, { "z", flow_id }, { "name", a.Name+"_Event" }, { "func", "var parameters = new Array();\n" + a.Name + " = msg.payload.events." + a.Name + ";\nparameters.push(" + a.Name + ");\nmsg.payload = parameters;\nreturn msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", link_out_function_x }, { "y", y }, { "wires", new JArray { new JArray(link_out_id) } } };
                    JObject link_out_node = new JObject { { "id", link_out_id }, { "type", "link out" }, { "z", flow_id }, { "name", "link_out_" + a.Name + "_Event" }, { "links", new JArray() }, { "x", link_out_x }, { "y", y } };
                    nodes.Add(event_node);
                    nodes.Add(function_node);
                    nodes.Add(link_out_node);
                    y += 80;
                    Console.WriteLine(a.Name);
                }
                
            }
        }

        public void AddUPnPActionNodes(JArray nodes, UPnPAction a)
        {
            string invoke_action_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string function_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string link_in_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();

            string parameters = "";
            for(int i = 0; i< a.ArgumentList.Length; i++)
            {
                if (i == 0)
                    parameters = a.ArgumentList[i].Name+": parameters["+i+"]";
                else
                    parameters = parameters+", "+a.ArgumentList[i].Name + ": parameters[" + i + "]";
            }
            //Console.WriteLine(parameters);
            JObject invoke_action_node = new JObject { { "id", invoke_action_id }, { "type", "upnp-invokeAction" }, { "z", flow_id }, { "upnpConfiguration", upnp_configuration_id }, { "name", "invoke action "+a.Name }, { "x", invoke_action_x }, { "y", y }, { "wires", new JArray { new JArray() } } };
            JObject function_node = new JObject { { "id", function_id }, { "type", "function" }, { "z", flow_id }, { "name", a.Name }, { "func", "parameters = msg.payload;\nmsg.payload = { };\nmsg.payload.action = '"+a.Name+"';\nmsg.payload.serviceType = '"+a.ParentService.ServiceURN+"';\nmsg.payload.params = {"+parameters+"};\nreturn msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", link_in_function_x }, { "y", y }, { "wires", new JArray { new JArray(invoke_action_id) } } };
            JObject link_in_node = new JObject { { "id", link_in_id }, { "type", "link in" }, { "z", flow_id }, { "name", "link_in_"+a.Name }, { "links",new JArray() }, { "x", link_in_x }, { "y", y }, { "wires", new JArray { new JArray(function_id) } } };
            nodes.Add(invoke_action_node);
            nodes.Add(function_node);
            nodes.Add(link_in_node);
        }

    }
}
