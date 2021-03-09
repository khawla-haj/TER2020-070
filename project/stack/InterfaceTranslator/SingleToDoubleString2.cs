using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using OpenSource.UPnP;

namespace SampleDevice.InterfaceTranslator.SingleToDoubleString2
{

    class SingleToDoubleString2
    {
        private string flow_name;
        private string flow_id;
        private NodeRedAPI.NodeRedAPI api;

        public SingleToDoubleString2(string flow_name, NodeRedAPI.NodeRedAPI api)
        {
            this.flow_name = flow_name;
            this.api = api;
            this.AddSingleToDoubleString2Flow();
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

        public bool AddSingleToDoubleString2Flow()
        {


            string flow_name = this.flow_name;
            string li_set_Value1_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string lo_StringsEvent_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string li_set_Value2SendEvent_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string set_Value1_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string set1_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string set_Value2SendEvent_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string set2_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string StringsEvent_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();


            this.flow_id = this.api.addFlowBlank(this.flow_name);

            JObject f = new JObject { { "id", flow_id }, { "label", flow_name }, { "disabled", false }, { "info", "" } };
            JArray nodes = new JArray();
            JObject li_set_Value1 = new JObject { { "id", li_set_Value1_id }, { "type", "link in" }, { "z", flow_id }, { "name", "link_in_Set_Value1" }, { "links", new JArray() }, { "x", 255 }, { "y", 240 }, { "wires", new JArray { new JArray(set_Value1_id) } } };
            JObject set_Value1 = new JObject { { "id", set_Value1_id }, { "type", "function" }, { "z", flow_id }, { "name", "set_Value1" }, { "func", "parameters = msg.payload;\nmsg.payload = parameters[0];\nreturn msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", 470 }, { "y", 240 }, { "wires", new JArray { new JArray(set1_id) } } };

            JArray rules1 = new JArray();
            JObject a = new JObject { { "t", "set" }, { "p", "v1" }, { "pt", "flow" }, { "to", "payload" }, { "tot", "msg" } };
            rules1.Add(a);
            JObject set1 = new JObject { { "id", set1_id }, { "type", "change" }, { "z", flow_id }, { "name", "setv1" }, { "rules", rules1 }, { "action", "" }, { "property", "" }, { "from", "" }, { "to", "" }, { "reg", false }, { "x", 730 }, { "y", 240 }, { "wires", new JArray { new JArray(StringsEvent_id) } } };
            
            JObject li_set_Value2SendEvent = new JObject { { "id", li_set_Value2SendEvent_id }, { "type", "link in" }, { "z", flow_id }, { "name", "link_in_set_Value2SendEvent" }, { "links", new JArray() }, { "x", 255 }, { "y", 340 }, { "wires", new JArray { new JArray(set_Value2SendEvent_id) } } };
            JObject set_Value2SendEvent = new JObject { { "id", set_Value2SendEvent_id }, { "type", "function" }, { "z", flow_id }, { "name", "set_Value2SendEvent" }, { "func", "parameters = msg.payload;\nmsg.payload = parameters[0];\nreturn msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", 500 }, { "y", 340 }, { "wires", new JArray { new JArray(set2_id) } } };

            JArray rules2 = new JArray();
            JObject b = new JObject { { "t", "set" }, { "p", "v2" }, { "pt", "flow" }, { "to", "payload" }, { "tot", "msg" } };
            rules2.Add(b); 

            JObject set2 = new JObject { { "id", set2_id }, { "type", "change" }, { "z", flow_id }, { "name", "setv2" }, { "rules", rules2 }, { "action", "" }, { "property", "" }, { "from", "" }, { "to", "" }, { "reg", false }, { "x", 730 }, { "y", 340 }, { "wires", new JArray { new JArray(StringsEvent_id) } } };
            JObject StringsEvent = new JObject { { "id", StringsEvent_id }, { "type", "function" }, { "z", flow_id }, { "name", "StringsEvent" }, { "func", "var parameters = new Array();\nparameters.push(flow.get(\"v1\"));\nparameters.push(flow.get(\"v2\"));\nmsg.payload = parameters;\nreturn msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", 1050 }, { "y", 240 }, { "wires",new JArray{new JArray(lo_StringsEvent_id)} }};
            JObject lo_StringsEvent = new JObject { { "id", lo_StringsEvent_id }, { "type", "link out" }, { "z", flow_id }, { "name", "link_out_StringsEvent" }, { "links", new JArray() }, { "x", 1275 }, { "y", 240 } };
            
    

            nodes.Add(li_set_Value1);
            nodes.Add(set_Value1);
            nodes.Add(set1);
            nodes.Add(li_set_Value2SendEvent);
            nodes.Add(set_Value2SendEvent);
            nodes.Add(set2);
            nodes.Add(StringsEvent);
            nodes.Add(lo_StringsEvent);
            f.Add("nodes", nodes);

            api.launchRequest("flow/" + flow_id, "PUT", f.ToString());

            //Console.WriteLine(serJson);
            return true;
        }
    }
}
