using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SampleDevice
{
    class TestJson
    {
        public string GetJsonData()
        {
            string a = "[[2010, 1000], [2012, 500], [2013, 900], [2015, 100], [2017, 3000],]";
            string b = "[[2010, 1000], [2012, 500], [2013, 900], [2015, 100], [2017, 3000],]";
            string c = "[[2010, 1000], [2012, 500], [2013, 900], [2015, 100], [2017, 3000],]";
            string d = "[[2010, 1000], [2012, 500], [2013, 900], [2015, 100], [2017, 3000],]";
            JObject HS = new JObject { { "name", "炉石旅店" }, { "data", a } };
            JObject SD = new JObject { { "name", "谢尔顿" }, { "data", b } };
            JObject SV = new JObject { { "name", "7天" }, { "data", c } };
            JObject RJ = new JObject { { "name", "如家" }, { "data", d } };

            var jarray = new JArray();
            jarray.Add(HS);
            jarray.Add(SD);
            jarray.Add(SV);
            jarray.Add(RJ);

            JObject val = new JObject { { "val", jarray.ToString() } };
            //string jResult = JsonConvert.SerializeObject(val);
            string jResult = val.ToString();
            return jResult;
        }
    }
}
