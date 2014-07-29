using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace NWNDotNet.NWNDefinitions
{
    public class AppManager
    {
        public AppManager()
        {
            //  0x0066c050
            //FinalAddress = (uint)AccessMemory.ProcessMemory.ReadPointer(0x0066c050);
            //FinalAddress = (uint)AccessMemory.ProcessMemory.ReadPointer(FinalAddress);
        }
        public static uint FinalAddress = setupAddress();

        private static uint setupAddress()
        {
            if (FinalAddress == 0x0)
            {
                //System.Windows.Forms.MessageBox.Show("Setting up");
                uint uPointVal = (uint)AccessMemory.ReadPointer(0x0066c050);
                //System.Windows.Forms.MessageBox.Show("Got " + uPointVal.ToString());

                FinalAddress = uPointVal;
                return uPointVal;
            }
            else
            {
                return FinalAddress;
            }
        }
        public ServerExoApp ServerExoApp = new ServerExoApp();


    }
}
