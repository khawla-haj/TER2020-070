using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SampleDevice.BeanToFlow.BasicUI
{
    class Button : Basic_UI
    {
        private string flow_name;
        private string flow_id;
        private NodeRedAPI.NodeRedAPI api;

        public Button(string flow_name, NodeRedAPI.NodeRedAPI api) : base(flow_name, api)
        {
            this.flow_name = flow_name;
            this.api = api;
            this.AddButtonFlow();
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

        public bool AddButtonFlow()
        {
            AddDashboard();
            this.flow_id = this.api.addFlowBlank(this.flow_name);
            string ui_button_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string Click_function_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string Button_function_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string link_out_Click_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string group_id = base.dashboard_id[1];
            dynamic order = base.dashboard_id[2];

            
            JObject jo = new JObject { { "id", flow_id }, { "label" , flow_name }, { "disabled", false }, { "info", "" } };
            JArray nodes = new JArray();
            JObject ui_button = new JObject { { "id", ui_button_id }, { "type", "ui_button" }, { "z", flow_id }, { "name", "" }, { "group", group_id }, { "order", order }, { "width", 0 }, { "height", 0 }, { "passthru", false }, { "label", "button" }, { "tooltip", "" }, { "color", "" }, { "bgcolor", "" }, { "icon", "" }, { "payload", "" }, { "payloadType", "str" }, { "topic", "Click" }, { "x", 450 }, { "y", 260 }, { "wires", new JArray { new JArray(Button_function_id) } } };
            JObject Click_function = new JObject { { "id", Click_function_id }, { "type", "function" }, { "z", flow_id }, { "name", "Click" }, { "func", "if (msg.event == \"Click\"){\n    msg.payload = \"Click\";\n    return msg;\n}\n" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", 730 }, { "y", 360 }, { "wires", new JArray { new JArray(link_out_Click_id) } } };
            JObject link_out_Click = new JObject { { "id", link_out_Click_id }, { "type", "link out" }, { "z", flow_id }, { "name", "link_out_Click" }, { "links", new JArray() }, { "x", 935 }, { "y", 360 } };
            JObject Button_function = new JObject { { "id", Button_function_id }, { "type", "function" }, { "z", flow_id }, { "name", "button_Function" }, { "func", "if(msg.topic==\"Click\"){\n   msg.event = \"Click\";\n}\nreturn msg;" }, { "outputs", 1 }, { "noerr", 0 }, { "initialize", "" }, { "finalize", "" }, { "x", 480 }, { "y", 360 }, { "wires", new JArray { new JArray(Click_function_id) } } };


            nodes.Add(ui_button);
            nodes.Add(Button_function);
            nodes.Add(Click_function);
            nodes.Add(link_out_Click); 

            jo.Add("nodes", nodes);
            
            api.launchRequest("flow/" + flow_id, "PUT", jo.ToString());
            //Console.WriteLine(serJson);
            return true;
        }

    }
}
