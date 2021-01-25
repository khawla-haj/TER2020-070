using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

			string linkout_name = "link_out_" + operation1;
			string linkin_name = "link_in_" + operation2;
			string flow_from_id="";
			string flow_to_id="";
			string linkout_id = "";
			string linkin_id = "";

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
				if ((string)flows.ElementAt(k)["type"] == "link out" && (string)flows.ElementAt(k)["z"] == flow_from_id && (string)flows.ElementAt(k)["name"] == linkout_name) 
				{
					string[] a = new string[1] { linkin_id };
					flows.ElementAt(k)["links"] = JArray.FromObject(a);
					//JArray.FromObject(flows.ElementAt(k)["links"]).Add(linkin_id);
					//Console.WriteLine(flows.ElementAt(k));
				}
				if ((string)flows.ElementAt(k)["type"] == "link in" && (string)flows.ElementAt(k)["z"] == flow_to_id && (string)flows.ElementAt(k)["name"] == linkin_name)
				{
					string[] a = new string[1] { linkout_id };
					flows.ElementAt(k)["links"] = JArray.FromObject(a);
					//JArray.FromObject(flows.ElementAt(k)["links"]).Add(linkout_id);
					//a.Add(linkout_id);
					//Console.WriteLine(flows.ElementAt(k));
				}
				
				//flows.ElementAt(k)["links"][0][0] = linkout_id;
			}
			//Console.WriteLine(flows);

			api.launchRequest("flows", "POST", flows.ToString());


		}

		public void AddImcomptableLink(string from, string to, string operation1, string operation2, string operation3)
		{
			Console.WriteLine("call nodered api: AddImcomptableLink");
		}

		public void DelLink(string name)
		{
			Console.WriteLine("call nodered api: DelLink");
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
