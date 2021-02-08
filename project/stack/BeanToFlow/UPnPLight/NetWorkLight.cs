using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleDevice.BeanToFlow.UPnPLight
{
    class NetWorkLight
    {
        private string flow_name;
        private string flow_id;
        private NodeRedAPI.NodeRedAPI api;

        public NetWorkLight(string flow_name, NodeRedAPI.NodeRedAPI api)
        {
            this.flow_name = flow_name;
            this.api = api;
            this.AddNetWorkLightFlow();
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


        public bool AddNetWorkLightFlow()
        {
            this.flow_id = this.api.addFlowBlank(this.flow_name);

            string flow_name = this.flow_name;
            string simple_soap_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string off_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower(); 
            string on_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string store_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string ssdp_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower(); 
            string link_in_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();
            string switch_id = (GetRandomString(8) + "." + GetRandomString(6)).ToLower();

            string serJson = string.Format("{{\"id\":\"{0}\",\"label\":\"{1}\",\"disabled\":false,\"info\":\"\",\"nodes\":" +
                "[{{\"id\":\"{2}\",\"type\":\"simple-soap\",\"z\":\"{0}\",\"host\":\"device\",\"hostType\":\"flow\",\"path\":\"_urn-upnp-org-serviceId-SwitchPower.0001_control\",\"pathType\":\"str\",\"action\":\"urn: schemas-upnp-org:service:SwitchPower:v#SetTarget\",\"actionType\":\"str\",\"body\":\"payload\",\"bodyType\":\"msg\",\"mustache\":false,\"attrkey\":\"$\",\"charkey\":\"_\",\"stripPrefix\":false,\"simplify\":false,\"normalizeTags\":false,\"normalize\":false,\"topic\":\"\",\"name\":\"\",\"useAuth\":false,\"x\":770,\"y\":360,\"wires\":[[]]}}," +
                "{{\"id\":\"{3}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"Off\",\"func\":\"msg.payload =  '<?xml version=\\\"1.0\\\" encoding=\\\"utf-8\\\"?>\\\\r\\\\n' +\\n      '<s:Envelope s:encodingStyle=\\\"http://schemas.xmlsoap.org/soap/encoding/\\\" xmlns:s=\\\"http://schemas.xmlsoap.org/soap/envelope/\\\">\\\\r\\\\n' +\\n        '<s:Body>\\\\r\\\\n' +\\n            '<u:SetTarget xmlns:u=\\\"urn:schemas-upnp-org:service:SwitchPower:1\\\" />\\\\r\\\\n' +\\n                '<newTargetValue>false</newTargetValue>\\\\r\\\\n'+\\n        '</s:Body>\\\\r\\\\n' +\\n      '</s:Envelope>';\\nreturn msg;\\n\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":530,\"y\":460,\"wires\":[[\"{2}\"]]}}," +
                "{{\"id\":\"{4}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"On\",\"func\":\"msg.payload =  '<?xml version=\\\"1.0\\\" encoding=\\\"utf-8\\\"?>\\\\r\\\\n' +\\n      '<s:Envelope s:encodingStyle=\\\"http://schemas.xmlsoap.org/soap/encoding/\\\" xmlns:s=\\\"http://schemas.xmlsoap.org/soap/envelope/\\\">\\\\r\\\\n' +\\n        '<s:Body>\\\\r\\\\n' +\\n            '<u:SetTarget xmlns:u=\\\"urn:schemas-upnp-org:service:SwitchPower:1\\\" />\\\\r\\\\n' +\\n                '<newTargetValue>true</newTargetValue>\\\\r\\\\n'+\\n        '</s:Body>\\\\r\\\\n' +\\n      '</s:Envelope>';\\nreturn msg;\\n\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":530,\"y\":320,\"wires\":[[\"{2}\"]]}}," +
                "{{\"id\":\"{5}\",\"type\":\"function\",\"z\":\"{0}\",\"name\":\"Store device\",\"func\":\"var device = flow.get(\\\"device\\\")||{{}};\\nif(msg.payload.nts==\\\"ssdp:alive\\\"){{\\n    device=msg.payload.location;\\n}}\\nif(msg.payload.nts==\\\"ssdp:byebye\\\"){{\\n    device = {{}};\\n}}\\nflow.set(\\\"device\\\", device);\\n\\n\",\"outputs\":1,\"noerr\":0,\"initialize\":\"\",\"finalize\":\"\",\"x\":790,\"y\":180,\"wires\":[[]]}}," +
                "{{\"id\":\"{6}\",\"type\":\"ssdp-discover\",\"z\":\"{0}\",\"ssdp\":\"1e6eba90.597335\",\"name\":\"\",\"atstart\":true,\"repeat\":\"1\",\"st\":\"urn:schemas-upnp-org:service:SwitchPower:1\",\"x\":430,\"y\":180,\"wires\":[[\"{5}\"]]}}," +
                "{{\"id\":\"{7}\",\"type\":\"link in\",\"z\":\"{0}\",\"name\":\"link_in_SetTarget\",\"links\":[],\"x\":195,\"y\":400,\"wires\":[[\"{8}\"]]}}," +
                "{{\"id\":\"{8}\",\"type\":\"switch\",\"z\":\"{0}\",\"name\":\"SetTarget\",\"property\":\"payload\",\"propertyType\":\"msg\",\"rules\":[{{\"t\":\"true\"}},{{\"t\":\"false\"}}],\"checkall\":\"true\",\"repair\":false,\"outputs\":2,\"x\":340,\"y\":400,\"wires\":[[\"{4}\"],[\"{3}\"]]}}]}}", flow_id, flow_name, simple_soap_id, off_id, on_id, store_id, ssdp_id, link_in_id, switch_id);


            api.launchRequest("flow/" + flow_id, "PUT", serJson.ToString());
            //Console.WriteLine(serJson);
            return true;
        }
    }
}
