using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteMagic;
using System.Threading;
using System.Runtime;
using System.Reflection;
using System.Runtime.InteropServices;
namespace NWNDotNet.Hooks
{
    class MainLoop
    {
        //  0x0042CA10   = SrvMainLoop;
        public MainLoop()
        {
            try
            {
                System.Windows.Forms.MessageBox.Show("MainLoop Hooking!!");
                Magic.Instance.Detours.CreateAndApply(Magic.Instance.RegisterDelegate<MainLoopDelegate>(0x0042CA10), MainLoopHandler, "MainLoop");
                System.Windows.Forms.MessageBox.Show("MainLoop Hooked!!");
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("MainLoop Exception + "+ee.ToString());
            }

        }
        public static MainLoop main = null;
        public static string Setup()
        {
            try
            {
                main = new MainLoop();
            }
            catch (Exception ee)
            {
                return ee.ToString();
            }

            return "Done";
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
            System.Windows.Forms.MessageBox.Show("MainLoop Fired!!");
            //Call the original
            Magic.Instance.Detours["MainLoop"].CallOriginal(instance);
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void MainLoopDelegate(uint pThis);


    }
}
