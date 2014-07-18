using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteMagic;
using System.Threading;
using System.Runtime;
namespace NWNDotNet.Hooks
{
    class MainLoop
    {
        //  0x0042CA10   = SrvMainLoop;
        public MainLoop()
        {
            Magic.Instance.Detours.CreateAndApply(Magic.Instance.RegisterDelegate<MainLoopDelegate>(0x0042CA10), MainLoopHandler, "MainLoop");

        }

        /*
         *  0x0066c050   - AppManager  - Read Pointer
         *  +0x04    - Read Pointer
         *  Voila - ExoAppInternal
         *  */


        private static readonly MainLoopDelegate MainLoopHandler = MainLoopHook;

        //Dont think we really need to do any work on the 'instance' as it will be the pThis for the CExoAppExternal : automatically provided?
        private static void MainLoopHook(uint instance)
        {
        
            //Call the original
            Magic.Instance.Detours["MainLoop"].CallOriginal(instance);
        }

        private delegate void MainLoopDelegate(uint pThis);


    }
}
