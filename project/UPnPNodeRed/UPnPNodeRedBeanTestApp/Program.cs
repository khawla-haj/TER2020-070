using System;
using WComp.UPnPDevice;

namespace UPnPNodeRedBeanTestApp {
    class Program {
        static void Main(string[] args) {
            NodeREDUPnPBean bean = new NodeREDUPnPBean();
            bean._DeviceOK += Bean__DeviceOK;
            bean._DeviceFailed += Bean__DeviceFailed;

            bean.Uri = "http://[FE80:0000:0000:0000:5C80:D4F2:29E8:8F26]:1579/";

            Console.ReadLine();
        }

        private static void Bean__DeviceFailed() {
            Console.WriteLine("DeviceFailed");
        }

        private static void Bean__DeviceOK() {
            Console.WriteLine("DeviceOK");
        }
    }
}
