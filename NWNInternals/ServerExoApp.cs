using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteMagic;
namespace NWNDotNet.NWNInternals
{
    class ServerExoApp
    {
        public ServerExoApp()
        {

        }

        private uint ExoAppAddress = 0x0;
        public uint GetInstance()
        {
            if (ExoAppAddress == 0x0)
            {
                Memory m = new Memory(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                uint uPointVal = (uint)m.ReadPointer(Internals.AppMananger);
                uPointVal = (uint)m.ReadPointer(uPointVal);
                uPointVal += 0x4;
                uPointVal = (uint)m.ReadPointer(uPointVal);
                ExoAppAddress = uPointVal;
            }

            return ExoAppAddress;
        }
    }
}
