using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleDevice.NodeRedAPI
{
    public class Node
    {
        public string id { get; set; }
        public string type { get; set; }
        public string z { get; set; }
        public string name { get; set; }
        public string bean { get; set; }
        public string assembly { get; set; }
        public string dll { get; set; }
        public string dllValueType { get; set; }
        public bool configured { get; set; }
        public object[] properties { get; set; }
        public Propertyvalues propertyValues { get; set; }
        public string func { get; set; }
        public int outputs { get; set; }
        public int noerr { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string[][] wires { get; set; }
    }

    public class Propertyvalues
    {
    }
}
