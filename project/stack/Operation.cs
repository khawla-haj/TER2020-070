using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSource.UPnP;
using SampleDevice.BeanToFlow.BasicUI;
using SampleDevice.NodeRedAPI;

namespace SampleDevice
{
    class Operation
    {
        UPnPService service;
		string[][] res = new string[2][];
		NodeRedAPI.NodeRedAPI api;

        public Operation(UPnPService service, string[][] res)
        {
            this.service = service;
            this.res = res;
			CheckAction();
			this.getFlows();
        }

		public Operation(NodeRedAPI.NodeRedAPI api, string[][] res)
		{
			this.api = api;
			this.res = res;
			CheckAction();
		}

		public void CheckAction()
		{
			if (this.res[0][0].Equals("ADDb"))
				this.AddFlow_ByBean(this.res[1][0], this.res[1][1]);
			if (this.res[0][0].Equals("REMOVEb"))
				this.DelFlow_ByBean(this.res[1][0]);
			if (this.res[0][0].Equals("ADDsimplel"))
			{
				//string[] par = this.res["ADDsimplel"];
				this.AddSimpleLink(this.res[1][0], this.res[1][1], this.res[1][2], this.res[1][3]);
			}
			if (this.res[0][0].Equals("ADDimconptablel"))
			{
				//string[] par = this.res["ADDImcomptablel"];
				this.AddImcomptableLink(this.res[1][0], this.res[1][1], this.res[1][2], this.res[1][3], this.res[1][4]);
			}
			if (this.res[0][0].Equals("REMOVEl"))
				this.DelLink(this.res[1][0]);
		}

		public void AddFlow_ByBean(string name, string type)
		{
			Console.WriteLine("call nodered api: AddFlowByBean");
			switch (type)
			{
				case("System.Windows.Forms.TextBox"):
					TextBox btf = new TextBox(name, this.api);
					break;
			}

		}

		public void DelFlow_ByBean(string name)
		{
			Console.WriteLine("call nodered api: DelFlow");
		}

		public void AddSimpleLink(string from, string to, string operation1, string operation2)
		{
			Console.WriteLine(from);
			Console.WriteLine(to);
			Console.WriteLine(operation1);
			Console.WriteLine(operation2);
			Console.WriteLine("call nodered api: AddSimplelink");
			string jsonfile = "../../LinksTemplateJson/Simple_Link.json";//JSON文件路径
			JToken flow_tk;
			string flow_id = api.addFlowBlank("SimpleLink");

			string linkout_name = "link_out_" + operation1;
			string linkin_name = "link_in_" + operation2;
			string linkin_simplelink = "linkin_simplelink";
			string linkout_simplelink = "linkout_simplelink";
			string flow_from_id="";
			string flow_to_id="";
			string linkout_id = "";
			string linkin_id = "";
			string linkin_simplelink_id = "";
			string linkout_simplelink_id = "";

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

			using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
			{
				using (JsonReader reader = new JsonTextReader(file))
				{
					JArray flow = JArray.Load(reader);
					flow_tk = flow.ElementAt(0);
					//string flow_id = (GetRandomString(8) + "." + GetRandomString(5)).ToLower();

					flow_tk["label"] = "SimpleLink";
					flow_tk["id"] = flow_id;

					JToken nodes = flow_tk["nodes"];
					foreach(var node in nodes)
					{
						string type = (string)node["type"];
						string name = (string)node["name"];
						node["id"] = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
						node["z"] = flow_id;

						if (name == linkin_simplelink)
							linkin_simplelink_id = (string)node["id"];
						if (name == linkout_simplelink)
							linkout_simplelink_id = (string)node["id"];
						if (type == "comment")
							node["name"] = "This is an simple link from Flow: " + from + ": method: " + operation1 + " to Flow: " + to + " method: " + operation2;
						
						
					}
					foreach (var node in nodes)
					{
						string name = (string)node["name"];
						if (name == linkin_simplelink)
						{
							node["wires"][0][0] = linkout_simplelink_id;
							node["links"][0] = linkout_id;
						}
						if (name == linkout_simplelink)
							node["links"][0] = linkin_id;
					}
				}
			}


			for (int k = 0; k < flows.Count; k++)
			{
				if ((string)flows.ElementAt(k)["type"] == "link out" && (string)flows.ElementAt(k)["z"] == flow_from_id && (string)flows.ElementAt(k)["name"] == linkout_name) 
				{
					//Console.WriteLine(flows.ElementAt(k)["links"].ToList().Count == 0);
					if (flows.ElementAt(k)["links"].ToList().Count == 0)
					{
						string[] a = new string[1] { linkin_simplelink_id };
						flows.ElementAt(k)["links"] = JArray.FromObject(a);
					}
					if (flows.ElementAt(k)["links"].ToList().Count > 0)
						flows.ElementAt(k)["links"][0].AddAfterSelf(linkin_simplelink_id);
					//JArray.FromObject(flows.ElementAt(k)["links"]).Add(linkin_id);
					//Console.WriteLine(flows.ElementAt(k));
				}
				if ((string)flows.ElementAt(k)["type"] == "link in" && (string)flows.ElementAt(k)["z"] == flow_to_id && (string)flows.ElementAt(k)["name"] == linkin_name)
				{
					if (flows.ElementAt(k)["links"].ToList().Count == 0)
					{
						string[] a = new string[1] { linkout_simplelink_id };
						flows.ElementAt(k)["links"] = JArray.FromObject(a);
					}
					if (flows.ElementAt(k)["links"].ToList().Count > 0)
						flows.ElementAt(k)["links"][0].AddAfterSelf(linkout_simplelink_id);
					//JArray.FromObject(flows.ElementAt(k)["links"]).Add(linkout_id);
					//a.Add(linkout_id);
					//Console.WriteLine(flows.ElementAt(k));
				}
				
				//flows.ElementAt(k)["links"][0][0] = linkout_id;
			}
			//Console.WriteLine(flows);

			
			api.launchRequest("flows", "POST", flows.ToString());
			api.addFlowFromJason(flow_id, flow_tk);


		}

		public void AddImcomptableLink(string from, string to, string operation1, string operation2, string operation3)
		{
			Console.WriteLine("call nodered api: AddImcomptableLink");
			string flow_id = api.addFlowBlank("ImcompableLink");

			string jsonfile = "../../LinksTemplateJson/Imcompable_Link.js";//JSON文件路径
			string linkin_addtoken_id = "";
			string linkin_checktoken_id = "";
			string linkout_addtoken_id = "";
			string linkout_checktoken_id = "";
			string addtoken_id = "";
			string checktoken_id = "";
			string token = GetRandomString(10);
			string flow_from_id = "";
			string flow_to_id = "";
			string linkout_name = "link_out_" + operation1;
			string linkin_name = "link_in_" + operation2;
			string linkin_imcompable_name = "link_in_" + operation3;
			string linkout_imcompable_name = "link_out_Event_" + operation3;
			string linkout_id = "";
			string linkin_id = "";
			string linkout_imcompable_id = "";
			string linkin_imcompable_id = "";
			JToken flow_tk;


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
				if ((string)flows.ElementAt(k)["type"] == "link out" && (string)flows.ElementAt(k)["z"] == flow_from_id && (string)flows.ElementAt(k)["name"] == linkout_imcompable_name)
					linkout_imcompable_id = (string)flows.ElementAt(k)["id"];
				if ((string)flows.ElementAt(k)["type"] == "link in" && (string)flows.ElementAt(k)["z"] == flow_from_id && (string)flows.ElementAt(k)["name"] == linkin_imcompable_name)
					linkin_imcompable_id = (string)flows.ElementAt(k)["id"];
			}


			using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
			{
				using (JsonReader reader = new JsonTextReader(file))
				{
					JArray flow = JArray.Load(reader);
					flow_tk = flow.ElementAt(0);
					//string flow_id = (GetRandomString(8) + "." + GetRandomString(5)).ToLower();

					flow_tk["label"] = "ImcompableLink";
					flow_tk["id"] = flow_id;

					JToken nodes = flow_tk["nodes"];
					foreach (var node in nodes)
					{
						string type = (string)node["type"];

						if (type != "function")
							node["id"] = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
						if (type == "function")
							node["id"] = (GetRandomString(8) + "." + GetRandomString(5)).ToLower();
						node["z"] = flow_id;

						if (type == "link in")
						{
							if ((string)node["name"] == "linkin_addtoken")
								linkin_addtoken_id = (string)node["id"];
							if ((string)node["name"] == "linkin_checktoken")
								linkin_checktoken_id = (string)node["id"];
						}
						if (type == "link out")
						{
							if ((string)node["name"] == "linkout_addtoken")
								linkout_addtoken_id = (string)node["id"];
							if ((string)node["name"] == "linkout_checktoken")
								linkout_checktoken_id = (string)node["id"];
						}
						if (type == "comment")
						{
							node["name"] = "This is an imcompable link from Flow: " + from + ": method: " + operation1 + " to Flow: " + to + " method: " + operation2 + " and using imcompable method: " + operation3;
						}
						if (type == "function")
						{
							if ((string)node["name"] == "AddToken")
								addtoken_id = (string)node["id"];
						}
						if (type == "function")
						{
							if ((string)node["name"] == "CheckToken")
								checktoken_id = (string)node["id"];
						}

					}
					foreach (var node in nodes)
					{
						string name = (string)node["name"];

						if (name == "linkin_addtoken")
						{
							node["wires"][0][0] = addtoken_id;
							node["links"][0] = linkout_id;
						}
						if (name == "linkin_checktoken")
						{
							node["wires"][0][0] = checktoken_id;
							node["links"][0] = linkout_imcompable_id;
						}
						if (name == "linkout_addtoken")
						{
							node["links"][0] = linkin_imcompable_id;
						}
						if (name == "linkout_checktoken")
						{
							node["links"][0] = linkin_id;
						}
						if (name == "AddToken")
						{
							node["wires"][0][0] = linkout_addtoken_id;
							node["func"] = "console.log(\"add token\");\nmsg.token = \""+token+"\";\nreturn msg;";
						}
						if (name == "CheckToken")
						{
							node["wires"][0][0] = linkout_checktoken_id;
							node["func"] = "console.log(msg);\nif(msg.token==\""+token+"\")\n    return msg;\nelse\n    node.error(\"wrong token\");\n";
						}
					}
				}
			}
			//Console.WriteLine(flow_tk);
			
			//api.launchRequest("flow/" + flow_id, "POST", flow_tk.ToString());

			for (int l = 0; l < flows.Count; l++)
			{
				if ((string)flows.ElementAt(l)["type"] == "link out" && (string)flows.ElementAt(l)["z"] == flow_from_id && (string)flows.ElementAt(l)["name"] == linkout_name)
				{
					if (flows.ElementAt(l)["links"].ToList().Count == 0)
					{
						string[] a = new string[1] { linkin_addtoken_id };
						flows.ElementAt(l)["links"] = JArray.FromObject(a);
					}
					if (flows.ElementAt(l)["links"].ToList().Count > 0)
						flows.ElementAt(l)["links"][0].AddAfterSelf(linkin_addtoken_id);
				}
				if ((string)flows.ElementAt(l)["type"] == "link in" && (string)flows.ElementAt(l)["z"] == flow_to_id && (string)flows.ElementAt(l)["name"] == linkin_name)
				{
					if (flows.ElementAt(l)["links"].ToList().Count == 0)
					{
						string[] a = new string[1] { linkout_checktoken_id };
						flows.ElementAt(l)["links"] = JArray.FromObject(a);
					}
					if (flows.ElementAt(l)["links"].ToList().Count > 0)
						flows.ElementAt(l)["links"][0].AddAfterSelf(linkout_checktoken_id);

				}
				if ((string)flows.ElementAt(l)["type"] == "link out" && (string)flows.ElementAt(l)["z"] == flow_from_id && (string)flows.ElementAt(l)["name"] == linkout_imcompable_name)
				{
					if (flows.ElementAt(l)["links"].ToList().Count == 0)
					{
						string[] a = new string[1] { linkin_checktoken_id };
						flows.ElementAt(l)["links"] = JArray.FromObject(a);
					}
					if (flows.ElementAt(l)["links"].ToList().Count > 0)
						flows.ElementAt(l)["links"][0].AddAfterSelf(linkin_checktoken_id);
				}
				if ((string)flows.ElementAt(l)["type"] == "link in" && (string)flows.ElementAt(l)["z"] == flow_from_id && (string)flows.ElementAt(l)["name"] == linkin_imcompable_name)
				{
					if (flows.ElementAt(l)["links"].ToList().Count == 0)
					{
						string[] a = new string[1] { linkout_addtoken_id };
						flows.ElementAt(l)["links"] = JArray.FromObject(a);
					}
					if (flows.ElementAt(l)["links"].ToList().Count > 0)
						flows.ElementAt(l)["links"][0].AddAfterSelf(linkout_addtoken_id);
				}
			}
			api.launchRequest("flows", "POST", flows.ToString());
			api.addFlowFromJason(flow_id, flow_tk);
		}

		public void DelLink(string name)
		{
			Console.WriteLine("call nodered api: DelLink");
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

		public void getFlows()
		{
			//UPnPArgument uPnPArgument = new UPnPArgument("flowname", "590db922.6d19c8");
			UPnPArgument[] uPnPArguments = new UPnPArgument[1];
			//uPnPArguments[0] = uPnPArgument;
			UPnPAction action = this.service.GetAction("GetFlows");
			//Console.WriteLine(action.ArgumentList.Length);
			this.service.InvokeSync("GetFlows", null);
			this.service.GetActions();

		}



		private void AddNode()
		{
			UPnPAction[] noderedActions = this.service.GetActions();
			Console.WriteLine(noderedActions[0].ArgumentList);
			var arguments = new UPnPArgument[2];
			arguments[0] = new UPnPArgument("node", "1");
			arguments[1] = new UPnPArgument("flowId", "f32eab8b.153d28");
			this.service.InvokeAsync("AddNode", arguments);
		
		}
	}
}
