using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleDevice.BeanToFlow.BasicUI
{
    class CheckBox : Basic_UI
    {
        private string flow_name;
        private string flow_id;
        private NodeRedAPI.NodeRedAPI api;

        public CheckBox(string flow_name, NodeRedAPI.NodeRedAPI api) : base(flow_name, api)
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
            string Click_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string Event_get_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string get_Checked_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower(); 
            string switch_Function_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string link_out_Click = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string link_out_Event_get_Checked_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string ui_switch_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string group_id = base.dashboard_id[1];
            dynamic order = base.dashboard_id[2];
            string link_in_get_Checked = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();



            string serJson = string.Format("{{\"id\":\"{0}\",\"label\":\"{1}\",\"disabled\":false,\"info\":\"\",\"nodes\":[" +
                "{{\"id\":\"{2}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"Click\",\"func\":\"if (msg.event == \\\"Click\\\"){{\\n    msg.payload = \\\"Click\\\";\\n    return msg;\\n}}\\n\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":830,\"y\":260,\"wires\":[[\"{6}\"]]}}," +
                "{{\"id\":\"{3}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"Event_get_Checked\",\"func\":\"if(msg.event == \\\"GetProp\\\")\\n    return msg;\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":880,\"y\":420,\"wires\":[[\"{7}\"]]}}," +
                "{{\"id\":\"{4}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"get_Checked\",\"func\":\"msg.topic = \\\"GetProp\\\";\\nreturn msg;\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":330,\"y\":340,\"wires\":[[\"{5}\"]]}}," +
                "{{\"id\":\"{5}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"switch_Function\",\"func\":\"if(msg.topic==\\\"Click\\\"){{\\n   msg.event = \\\"Click\\\";\\n   flow.set(\\\"bool\\\", msg.payload);\\n}}\\nif(msg.topic==\\\"GetProp\\\"){{\\n   msg.event = \\\"GetProp\\\";\\n   msg.payload = flow.get(\\\"bool\\\");\\n}}\\nreturn msg;\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":600,\"y\":340,\"wires\":[[\"{2}\",\"{3}\"]]}}," +
                "{{\"id\":\"{6}\",\"type\":\"link out\",\"z\":\"{0}\",\"name\":\"link_out_Click\",\"links\":[],\"x\":1035,\"y\":260}}," +
                "{{\"id\":\"{7}\",\"type\":\"link out\",\"z\":\"{0}\",\"name\":\"link_out_Event_get_Checked\",\"links\":[],\"x\":1035,\"y\":420}}," +
                "{{\"id\":\"{8}\",\"type\":\"ui_switch\",\"z\":\"{0}\",\"name\":\"\",\"label\":\"switch\",\"tooltip\":\"\",\"group\":\"{9}\",\"order\":{10},\"width\":0,\"height\":0,\"passthru\":true,\"decouple\":\"false\",\"topic\":\"Click\",\"style\":\"\",\"onvalue\":\"true\",\"onvalueType\":\"bool\",\"onicon\":\"\",\"oncolor\":\"\",\"offvalue\":\"false\",\"offvalueType\":\"bool\",\"officon\":\"\",\"offcolor\":\"\",\"x\":570,\"y\":220,\"wires\":[[\"{5}\"]]}}," +
                "{{\"id\":\"{11}\",\"type\":\"link in\",\"z\":\"{0}\",\"name\":\"link_in_get_Checked\",\"links\":[],\"x\":155,\"y\":340,\"wires\":[[\"{4}\"]]}}]}}", flow_id, flow_name, Click_id, Event_get_id, get_Checked_id, switch_Function_id, link_out_Click, link_out_Event_get_Checked_id, ui_switch_id, group_id, order, link_in_get_Checked);

            api.launchRequest("flow/" + flow_id, "PUT", serJson.ToString());
            Console.WriteLine(serJson);
            return true;
        }

    }
}
