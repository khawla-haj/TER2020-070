using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleDevice.BeanToFlow.BasicUI
{
    class TextBox2 : Basic_UI
    {
        private string flow_name;
        private string flow_id;
        private NodeRedAPI.NodeRedAPI api;

        public TextBox2(string flow_name, NodeRedAPI.NodeRedAPI api) : base(flow_name, api)
        {
            this.flow_name = flow_name;
            this.api = api;
            this.AddCheckBoxFlow();
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

        public bool AddCheckBoxFlow()
        {
            AddDashboard();
            this.flow_id = this.api.addFlowBlank(this.flow_name);
            string ui_text_input_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string store_text_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string Clear_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string set_Text_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string TextChanged_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string Event_get_Text_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string link_in_Clear_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string link_in_set_Text_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string get_Text_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string link_in_get_Text_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string link_out_TextChanged_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string link_out_Event_get_Text_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string group_id = base.dashboard_id[1];
            dynamic order = base.dashboard_id[2];



            string serJson = string.Format("{{\"id\":\"{0}\",\"label\":\"{1}\",\"disabled\":false,\"info\":\"\",\"nodes\":[" +
                "{{\"id\":\"{2}\",\"type\":\"ui_text_input\",\"z\":\"{0}\",\"name\":\"{1}\",\"label\":\"{1}\",\"tooltip\":\"\",\"group\":\"{3}\",\"order\":{4},\"width\":0,\"height\":0,\"passthru\":true,\"mode\":\"text\",\"delay\":\"10\",\"topic\":\"TextChanged\",\"x\":580,\"y\":160,\"wires\":[[\"{5}\"]]}}," +
                "{{\"id\":\"{6}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"Clear\",\"func\":\"msg.payload = null;\\nreturn msg; \",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":330,\"y\":240,\"wires\":[[\"{5}\"]]}}," +
                "{{\"id\":\"{7}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"set_Text\",\"func\":\"return msg; \",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":340,\"y\":360,\"wires\":[[\"{5}\"]]}}," +
                "{{\"id\":\"{8}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"TextChanged\",\"func\":\"if (msg.topic ==\\\"TextChanged\\\")\\n    return msg;\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":850,\"y\":240,\"wires\":[[\"{9}\"]]}}," +
                "{{\"id\":\"{10}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"Event_get_Text\",\"func\":\"if(msg.topic == \\\"GetProp\\\"){{\\n    msg.payload = flow.get(\\\"text\\\");\\n    return msg;\\n}}\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":870,\"y\":360,\"wires\":[[\"{11}\"]]}}," +
                "{{\"id\":\"{12}\",\"type\":\"link in\",\"z\":\"{0}\",\"name\":\"link_in_Clear\",\"links\":[],\"x\":155,\"y\":240,\"wires\":[[\"{6}\"]]}}," +
                "{{\"id\":\"{13}\",\"type\":\"link in\",\"z\":\"{0}\",\"name\":\"link_in_set_Text\",\"links\":[],\"x\":155,\"y\":360,\"wires\":[[\"{7}\"]]}}," +
                "{{\"id\":\"{14}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"get_Text\",\"func\":\"msg.topic = \\\"GetProp\\\";\\nreturn msg;\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":340,\"y\":480,\"wires\":[[\"{5}\"]]}}," +
                "{{\"id\":\"{15}\",\"type\":\"link in\",\"z\":\"{0}\",\"name\":\"link_in_get_Text\",\"links\":[],\"x\":155,\"y\":480,\"wires\":[[\"{14}\"]]}}," +
                "{{\"id\":\"{9}\",\"type\":\"link out\",\"z\":\"{0}\",\"name\":\"link_out_TextChanged\",\"links\":[],\"x\":1075,\"y\":240}}," +
                "{{\"id\":\"{11}\",\"type\":\"link out\",\"z\":\"{0}\",\"name\":\"link_out_Event_get_Text\",\"links\":[],\"x\":1075,\"y\":360}}," +
                "{{\"id\":\"{5}\",\"type\":\"change\",\"z\":\"{0}\",\"name\":\"store_text\",\"rules\":[{{\"t\":\"set\",\"p\":\"text\",\"pt\":\"flow\",\"to\":\"payload\",\"tot\":\"msg\"}}],\"action\":\"\",\"property\":\"\",\"from\":\"\",\"to\":\"\",\"reg\":false,\"x\":580,\"y\":360,\"wires\":[[\"{8}\",\"{10}\",\"{2}\"]]}}]}}",flow_id, flow_name, ui_text_input_id, group_id, order, store_text_id, Clear_id, set_Text_id,TextChanged_id, link_out_TextChanged_id, Event_get_Text_id, link_out_Event_get_Text_id, link_in_Clear_id, link_in_set_Text_id, get_Text_id, link_in_get_Text_id);

            api.launchRequest("flow/" + flow_id, "PUT", serJson.ToString());
            Console.WriteLine(serJson);
            return true;
        }
    }
}
