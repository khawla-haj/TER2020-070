using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleDevice.NodeRedAPI;

namespace SampleDevice.BeanToFlow.BasicUI
{
    class TextBox : Basic_UI
    {
        private JArray flow;
        private JToken flow_tk;
        private dynamic flow_name;
        private dynamic flow_id;
        private string Clear = "Clear";
        private string set_Text = "set_Text";
        private string get_Text = "get_Text";
        private string TextChanged = "TextChanged";
        private string Event_get_Text = "Event_get_Text";
        private string link_in_Clear = "link_in_Clear";
        private string link_in_set_Text = "link_in_set_Text";
        private string link_in_get_Text = "link_in_get_Text";
        private string link_out_TextChanged = "link_out_TextChanged";
        private string link_out_Event_get_Text = "link_out_Event_get_Text";





        public TextBox(string flow_name, NodeRedAPI.NodeRedAPI api) : base(flow_name, api)
        {
            this.flow_name = flow_name;
            this.AddTextBoxFlow();
        }

        public dynamic genarateJason()
        {
            string jsonfile = "../../BeanToFlow/BasicUI/test2.js";//JSON文件路径
            string Clear_id = "";
            string Set_Text_id = "";
            string TextChanged_id = "";
            string EventGetPropRet_id = "";
            string GetProp_id = "";
            string link_in_Clear_id = "";
            string link_in_Set_Text_id = "";
            string link_in_GetProp_id = "";
            string link_out_TextChanged_id = "";
            string link_out_EventGetPropRet_id = "";
            string ui_text_input_id = "";


            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                using (JsonReader reader = new JsonTextReader(file))
                {
                    int count = 0;
                    



                    flow = JArray.Load(reader);

                    flow_tk = flow.ElementAt(0);
                    flow_tk["label"] = flow_name;
                    flow_tk["id"] = base.flow_id;
                    this.flow_id = base.flow_id;

                    JToken nodes = flow_tk["nodes"];
                    foreach (var node in nodes)
                    {
                        string type = (string)node["type"];

                        node["id"] = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
                        node["z"] = flow_id;

                        if (type == "function")
                        {
                            if ((string)node["name"] == Clear)
                                Clear_id = (string)node["id"];
                            if ((string)node["name"] == set_Text)
                                Set_Text_id = (string)node["id"];
                            if ((string)node["name"] == TextChanged)
                                TextChanged_id = (string)node["id"];
                            if ((string)node["name"] == Event_get_Text)
                                EventGetPropRet_id = (string)node["id"];
                            if ((string)node["name"] == get_Text)
                                GetProp_id = (string)node["id"];
                        }
                        if (type == "link in")
                        {
                            if ((string)node["name"] == link_in_Clear)
                                link_in_Clear_id = (string)node["id"];
                            if ((string)node["name"] == link_in_set_Text)
                                link_in_Set_Text_id = (string)node["id"];
                            if ((string)node["name"] == link_in_get_Text)
                                link_in_GetProp_id = (string)node["id"];
                        }
                        if (type == "link out") { 
                            if ((string)node["name"] == link_out_TextChanged)
                                link_out_TextChanged_id = (string)node["id"];
                            if ((string)node["name"] == link_out_Event_get_Text)
                                link_out_EventGetPropRet_id = (string)node["id"];
                        }
                        if (type == "ui_text_input")
                        {
                            ui_text_input_id = (string)node["id"];
                        }
                        
                    }
                    foreach (var node in nodes)
                    {
                        string type = (string)node["type"];

                        if (type == "function")
                        {
                            if ((string)node["name"] == Clear)
                                node["wires"][0][0] = ui_text_input_id;
                            if ((string)node["name"] == set_Text)
                                node["wires"][0][0] = ui_text_input_id;
                            if ((string)node["name"] == TextChanged)
                                node["wires"][0][0] = link_out_TextChanged_id;
                            if ((string)node["name"] == Event_get_Text)
                                node["wires"][0][0] = link_out_EventGetPropRet_id;
                            if ((string)node["name"] == get_Text)
                                node["wires"][0][0] = EventGetPropRet_id;
                        }
                        if (type == "link in")
                        {
                            if ((string)node["name"] == link_in_Clear)
                                node["wires"][0][0] = Clear_id;
                            if ((string)node["name"] == link_in_set_Text)
                                node["wires"][0][0] = Set_Text_id;
                            if ((string)node["name"] == link_in_get_Text)
                                node["wires"][0][0] = GetProp_id;
                            //if ((string)node["name"] == "link_out_TextChanged")
                                //link_out_TextChanged_id = (string)node["id"];
                            //if ((string)node["name"] == "link_out_EventGetPropRet")
                                //link_out_EventGetPropRet_id = (string)node["id"];
                        }
                        if (type == "ui_text_input") {
                            count++;
                            node["group"] = base.dashboard_id[1];
                            node["order"] = count;
                            node["name"] = flow_name;
                            node["label"] = flow_name;
                            node["wires"][0][0]=TextChanged_id;
                            node["wires"][0][1] = EventGetPropRet_id;
                            
                            
                            //var tokenProp = wires.ElementAt(0).ElementAt(0) as JProperty;

                            //wire 要改
                        }
                    }
                    //Console.WriteLine(nodes);

                }
            }
            Console.WriteLine(flow_tk);
            return flow_tk;
        }
        public bool AddTextBoxFlow()
        {
            Console.WriteLine("call TextBox");
            AddDashboard();
            AddFlow();
            return Generate_Flow(this.genarateJason());   
        }


        public static string GetRandomHexNumber(int digits)
        {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
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
    }
}









