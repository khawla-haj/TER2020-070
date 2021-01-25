using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SampleDevice.NodeRedAPI;

namespace SampleDevice.BeanToFlow.BasicUI
{
    class Basic_UI
    {
        protected Node node;
        protected string ui_type;
        protected string bean_name;
        protected string flow_id;
        protected dynamic[] dashboard_id;
        protected NodeRedAPI.NodeRedAPI api;

        public Basic_UI( string bean_name, NodeRedAPI.NodeRedAPI api)
        {
            this.bean_name = bean_name;
            this.api = api;
        }

        public void AddFlow()
        {
            this.flow_id = this.api.addFlowBlank(this.bean_name);
        }

        public void AddDashboard()
        {
            dashboard_id = this.api.addDashboardTab("BaiscUI_Wcomp");
        }

        public bool Generate_Flow(dynamic flow)
        {
            this.api.addFlowFromJason(this.flow_id,flow);
            Console.WriteLine(flow_id);
            return true;
        }
    }
}
