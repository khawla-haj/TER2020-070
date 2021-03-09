using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using OpenSource.UPnP;

namespace SampleDevice.Basic.ToString
{

    class ToString
    {
        private string flow_name;
        private string flow_id;
        private NodeRedAPI.NodeRedAPI api;

        public ToString(string flow_name, NodeRedAPI.NodeRedAPI api)
        {
            this.flow_name = flow_name;
            this.api = api;
            this.AddToStringFlow();
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

        public bool AddToStringFlow()
        {


            string flow_name = this.flow_name;
            string li_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string lo_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string input_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string output_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string ts_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();


            this.flow_id = this.api.addFlowBlank(this.flow_name);

            JObject f = new JObject { { "id", flow_id }, { "label", flow_name }, { "disabled", false }, { "info", "" } };
            JArray nodes = new JArray();
            JObject li = new JObject { { "id", li_id }, { "type", "link in" }, { "z", flow_id }, { "name", "link_in_Input" }, { "links", new JArray() }, { "x", 315 }, { "y", 300 }, { "wires", new JArray { new JArray(input_id) } } };
            JObject lo = new JObject { { "id", lo_id }, { "type", "link out" }, { "z", flow_id }, { "name", "link_out_Output" }, { "links", new JArray() }, { "x", 1135 }, { "y", 300 } };
            JObject input = new JObject { { "id", input_id }, { "type", "function" }, { "z", flow_id }, { "name", "Input" }, { "func", "return msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", 470 }, { "y", 300 }, { "wires", new JArray { new JArray(ts_id) } } };
            JObject output = new JObject { { "id", output_id }, { "type", "function" }, { "z", flow_id }, { "name", "Output" }, { "func", "var parameters = new Array();\nOutput = msg.payload;\nparameters.push(Output);\nmsg.payload = parameters;\nreturn msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", 950 }, { "y", 300 }, { "wires", new JArray { new JArray(lo_id) } } };
            JObject ts = new JObject { { "id", ts_id }, { "type", "function" }, { "z", flow_id }, { "name", "toString" }, { "func", "parameters = msg.payload;\nmsg.payload = parameters[0].toString();\nreturn msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", 710 }, { "y", 300 }, { "wires", new JArray { new JArray(output_id) } } };

            nodes.Add(li);
            nodes.Add(lo);
            nodes.Add(input);
            nodes.Add(output);
            nodes.Add(ts);
            f.Add("nodes", nodes);

            api.launchRequest("flow/" + flow_id, "PUT", f.ToString());
            
            //Console.WriteLine(serJson);
            return true;
        }
    }
}
