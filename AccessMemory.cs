using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWNDotNet
{
    class AccessMemory
    {
        private static WhiteMagic.Memory ProcessMemory = null;// = new WhiteMagic.Memory("nwserver");
        public static uint ReadPointer(uint address)
        {
            if (ProcessMemory == null)
            {
                ProcessMemory = new WhiteMagic.Memory("nwserver");
            }
            return (uint)ProcessMemory.ReadPointer(address);
        }

        public static string ReadString(uint address)
        {
            if (ProcessMemory == null)
            {
                ProcessMemory = new WhiteMagic.Memory("nwserver");
            }
            return ProcessMemory.ReadString(address);

        }
    }
}
