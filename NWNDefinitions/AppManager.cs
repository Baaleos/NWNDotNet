using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWNDotNet.NWNDefinitions
{
    class AppManager
    {
        static AppManager()
        {
            //  0x0066c050
            FinalAddress = (uint)AccessMemory.ProcessMemory.ReadPointer(0x0066c050);
            FinalAddress = (uint)AccessMemory.ProcessMemory.ReadPointer(FinalAddress);
        }
        public static uint FinalAddress = 0x0;


        public static ServerExoApp ServerExoApp = new ServerExoApp();


    }
}
