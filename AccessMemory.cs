using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWNDotNet
{
    class AccessMemory
    {
        public static WhiteMagic.Memory ProcessMemory = new WhiteMagic.Memory(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

    }
}
