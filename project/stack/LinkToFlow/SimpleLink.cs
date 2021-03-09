using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SampleDevice.LinkToFlow
{
    class SimpleLink
    {
        private string flow_name;
        private string flow_id;
        private NodeRedAPI.NodeRedAPI api;
        private string from;
        private string to;
        private string operation1;
        private string operation2;
        string linkin_simplelink_id;
        string linkout_simplelink_id;

        public SimpleLink(string from, string to, string operation1, string operation2, NodeRedAPI.NodeRedAPI api)
        {
            this.flow_name = "SimpleLink";
            this.api = api;
            this.from = from;
            this.to = to;
            this.operation1 = operation1;
            this.operation2 = operation2;
            if(!this.checkSimpleLinkExist())
                this.AddSimpleLinkFlow();
            this.AddLinks();
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
        public bool AddSimpleLinkFlow()
        {
            this.flow_id = this.api.addFlowBlank(this.flow_name);
            this.linkin_simplelink_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            this.linkout_simplelink_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string comment_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();

            string serJson = string.Format("{{ \"id\":\"{0}\",\"label\":\"SimpleLink\",\"disabled\":false,\"info\":\"\",\"nodes\":[" +
                "{{\"id\":\"{1}\",\"type\":\"link in\",\"z\":\"{0}\",\"name\":\"linkin_simplelink\",\"links\":[],\"x\":355,\"y\":320,\"wires\":[[\"{2}\"]]}}," +
                "{{\"id\":\"{2}\",\"type\":\"link out\",\"z\":\"{0}\",\"name\":\"linkout_simplelink\",\"links\":[],\"x\":755,\"y\":320}}," +
                "{{\"id\":\"{3}\",\"type\":\"comment\",\"z\":\"{0}\",\"name\":\"This is an simple link from Flow: {4}: method: {5} to Flow: {6} method: {7}\",\"info\":\"\",\"x\":500,\"y\":200,\"wires\":[]}}]}}", flow_id, linkin_simplelink_id, linkout_simplelink_id, comment_id, from, operation1, to, operation2);

            api.launchRequest("flow/" + flow_id, "PUT", serJson.ToString());
            Console.WriteLine(serJson);
            return true;
        }

        public bool checkSimpleLinkExist()
        {
            string response = this.api.launchRequest("flows", "GET", null);
            JArray flows = JArray.Parse(response);
            for (int i = 0; i < flows.Count; i++)
            {
                if ((string)flows.ElementAt(i)["type"] == "tab" && (string)flows.ElementAt(i)["label"] == "SimpleLink")
                {
                    this.flow_id = (string)flows.ElementAt(i)["id"];
                    for (int j = 0; j < flows.Count; j++)
                    {
                        if ((string)flows.ElementAt(j)["z"] == this.flow_id && (string)flows.ElementAt(i)["name"] == "linkin_simplelink")
                            linkin_simplelink_id = (string)flows.ElementAt(j)["id"];
                        if ((string)flows.ElementAt(j)["z"] == this.flow_id && (string)flows.ElementAt(i)["name"] == "linkout_simplelink")
                            linkout_simplelink_id = (string)flows.ElementAt(j)["id"];
                    }
                    return true;
                }
            }
            return false;
        }

        public void AddLinks()
        {
            string flow_from_id = "";
            string flow_to_id = "";
            string linkout_id = "";
            string linkin_id = "";
            string linkout_name = "link_out_" + operation1;
            string linkin_name = "link_in_" + operation2;

            string response = this.api.launchRequest("flows", "GET", null);
            JArray flows = JArray.Parse(response);
            for (int i = 0; i < flows.Count; i++)
            {
                if ((string)flows.ElementAt(i)["type"] == "tab" && (string)flows.ElementAt(i)["label"] == from)
                    flow_from_id = (string)flows.ElementAt(i)["id"];
                if ((string)flows.ElementAt(i)["type"] == "tab" && (string)flows.ElementAt(i)["label"] == to)
                    flow_to_id = (string)flows.ElementAt(i)["id"];
            }
            for (int j = 0; j < flows.Count; j++)
            {
                if ((string)flows.ElementAt(j)["type"] == "link out" && (string)flows.ElementAt(j)["z"] == flow_from_id && (string)flows.ElementAt(j)["name"] == linkout_name)
                    linkout_id = (string)flows.ElementAt(j)["id"];
                if ((string)flows.ElementAt(j)["type"] == "link in" && (string)flows.ElementAt(j)["z"] == flow_to_id && (string)flows.ElementAt(j)["name"] == linkin_name)
                    linkin_id = (string)flows.ElementAt(j)["id"];
            }

            for (int k = 0; k < flows.Count; k++)
            {
                if ((string)flows.ElementAt(k)["z"] == flow_id && (string)flows.ElementAt(k)["name"] == "linkin_simplelink")
                {
                    if (flows.ElementAt(k)["links"].ToList().Count > 0)
                        flows.ElementAt(k)["links"][0].AddAfterSelf(linkout_id);
                    if (flows.ElementAt(k)["links"].ToList().Count == 0)
                    {
                        string[] a = new string[1] { linkout_id };
                        flows.ElementAt(k)["links"] = JArray.FromObject(a);
                    }
                }
                if ((string)flows.ElementAt(k)["z"] == flow_id && (string)flows.ElementAt(k)["name"] == "linkout_simplelink")
                {
                    if (flows.ElementAt(k)["links"].ToList().Count > 0)
                        flows.ElementAt(k)["links"][0].AddAfterSelf(linkin_id);
                    if (flows.ElementAt(k)["links"].ToList().Count == 0)
                    {
                        string[] a = new string[1] { linkin_id };
                        flows.ElementAt(k)["links"] = JArray.FromObject(a);
                    }
                }

                if ((string)flows.ElementAt(k)["type"] == "link out" && (string)flows.ElementAt(k)["z"] == flow_from_id && (string)flows.ElementAt(k)["name"] == linkout_name)
                {
                    if (flows.ElementAt(k)["links"].ToList().Count > 0)
                        flows.ElementAt(k)["links"][0].AddAfterSelf(linkin_simplelink_id);
                    if (flows.ElementAt(k)["links"].ToList().Count == 0)
                    {
                        string[] a = new string[1] { linkin_simplelink_id };
                        flows.ElementAt(k)["links"] = JArray.FromObject(a);
                    }
                }
                if ((string)flows.ElementAt(k)["type"] == "link in" && (string)flows.ElementAt(k)["z"] == flow_to_id && (string)flows.ElementAt(k)["name"] == linkin_name)
                {
                    if (flows.ElementAt(k)["links"].ToList().Count > 0)
                        flows.ElementAt(k)["links"][0].AddAfterSelf(linkout_simplelink_id);
                    if (flows.ElementAt(k)["links"].ToList().Count == 0)
                    {
                        string[] a = new string[1] { linkout_simplelink_id };
                        flows.ElementAt(k)["links"] = JArray.FromObject(a);
                    }
                }

            }
            api.launchRequest("flows", "POST", flows.ToString());
        }
    }
}
