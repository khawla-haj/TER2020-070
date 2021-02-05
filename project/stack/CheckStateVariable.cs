using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleDevice
{
    class CheckStateVariable
    {
		Dictionary<string, string[]> actions = new Dictionary<string, string[]>();

		public CheckStateVariable()
		{
		}

		public string[][] CheckValue(string value)
		{
			string com = null;
			string[] parameter = null;

			string[] arr = value.Split('|');

			//Dictionary<string, string[]> command = new Dictionary<string, string[]>();
			string[][] command = new string[2][];
			switch (arr.GetValue(1))
			{
				case "NEW_BEAN":
					com = "ADDb";
					parameter = new string[2];
					parameter[0] = arr[2];		//name
					parameter[1] = arr[3];		//type
					Console.WriteLine("   - add bean >> name: " + parameter[0]+", type: "+parameter[1] );
					Console.WriteLine("");
					//AddNode();
					break;
				case "DEL_BEAN":
					com = "REMOVEb";
					parameter = new string[1];
					parameter[0] = arr[2];		//name
					Console.WriteLine("   - remove bean>> name: " + parameter[0]);
					Console.WriteLine("");
					break;
				case "NEW_LINK":
					string[] link = arr[2].Split('-');
					if (link.GetValue(5).Equals(""))
					{
						com = "ADDsimplel";
						parameter = new string[4];
						string linkfrom = link.GetValue(1).ToString();
						string linkto = link.GetValue(2).ToString();
						string operation1 = link.GetValue(3).ToString();
						string operation2 = link.GetValue(4).ToString();
						parameter[0] = linkfrom;parameter[1] = linkto;parameter[2] = operation1;parameter[3] = operation2;
						Console.WriteLine("   - add simple link from " + linkfrom+" to "+linkto+" and operation1 is "+operation1+" and operation2 is "+operation2);
						Console.WriteLine("");
					}
					else
					{
						com = "ADDimconptablel";
						parameter = new string[5];
						string linkfrom = link.GetValue(1).ToString();
						string linkto = link.GetValue(2).ToString();
						string operation1 = link.GetValue(3).ToString();
						string operation2 = link.GetValue(4).ToString();
						string operation3 = link.GetValue(5).ToString();
						parameter[0] = linkfrom; parameter[1] = linkto; parameter[2] = operation1; parameter[3] = operation2; parameter[4] = operation3;
						Console.WriteLine("   - add imcompatible link from " + linkfrom + " to " + linkto + " and operation1 is " + operation1 + " and operation2 is " + operation2 + " and operation3 is " + operation3);
						Console.WriteLine("");
					}
					break;
				case "DEL_LINK":
					string[] link_del = arr[2].Split('-');
					if (link_del.GetValue(5).Equals(""))
					{
						com = "DELsimplel";
						parameter = new string[4];
						string linkfrom = link_del.GetValue(1).ToString();
						string linkto = link_del.GetValue(2).ToString();
						string operation1 = link_del.GetValue(3).ToString();
						string operation2 = link_del.GetValue(4).ToString();
						parameter[0] = linkfrom; parameter[1] = linkto; parameter[2] = operation1; parameter[3] = operation2;
						Console.WriteLine("   - delete simple link from " + linkfrom + " to " + linkto + " and operation1 is " + operation1 + " and operation2 is " + operation2);
						Console.WriteLine("");
					}
					else
					{
						com = "DELimconptablel";
						parameter = new string[5];
						string linkfrom = link_del.GetValue(1).ToString();
						string linkto = link_del.GetValue(2).ToString();
						string operation1 = link_del.GetValue(3).ToString();
						string operation2 = link_del.GetValue(4).ToString();
						string operation3 = link_del.GetValue(5).ToString();
						parameter[0] = linkfrom; parameter[1] = linkto; parameter[2] = operation1; parameter[3] = operation2; parameter[4] = operation3;
						Console.WriteLine("   - delete imcompatible link from " + linkfrom + " to " + linkto + " and operation1 is " + operation1 + " and operation2 is " + operation2 + " and operation3 is " + operation3);
						Console.WriteLine("");
					}
					break;
			}

			//this.actions.Add(com, parameter);
			//command.Add(com, parameter);
			command[0] = new string[1] {com};
			command[1] = parameter;
			//Console.WriteLine(command.Keys);

			return command;
		}
	}
}
