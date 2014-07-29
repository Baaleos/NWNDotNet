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
                //System.Windows.Forms.MessageBox.Show("MainLoop Hooking!!");
                Magic.Instance.Detours.CreateAndApply(Magic.Instance.RegisterDelegate<MainLoopDelegate>(0x0042CA10), MainLoopHandler, "MainLoop");
                //System.Windows.Forms.MessageBox.Show("MainLoop Hooked!!");
            }
            catch (Exception ee)
            {
                //System.Windows.Forms.MessageBox.Show("MainLoop Exception + "+ee.ToString());
            }

        }
        public static AsmodeiServices.NWNListener listener;
        public static MainLoop main = null;
        public static string Setup()
        {
            try
            {
                listener = new AsmodeiServices.NWNListener();
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
        public static List<NetMessage.NetMessage> ListOfMessages = new List<NetMessage.NetMessage>();


        /// <summary>
        /// This list is used to receive the requests without delay, it is then fed to the main list when the main list is finished processing its work stack
        /// </summary>
        public static List<NetMessage.NetMessage> ExternalList = new List<NetMessage.NetMessage>();
        public static void AddToQueue(NetMessage.NetMessage MSG)
        {
            while (processingMessages)
            {
                //Thread.Sleep(0);
            }
            ExternalList.Add(MSG);
        }


        public static volatile Boolean processingMessages = false;
        
        //Dont think we really need to do any work on the 'instance' as it will be the pThis for the CExoAppExternal : automatically provided?
        private static void MainLoopHook(uint instance)
        {
            //System.Windows.Forms.MessageBox.Show("MainLoop Fired!!");
            //Call the original
            string ARG, ARG2;

            
            foreach (NetMessage.NetMessage msg in ListOfMessages)
            {
                //Act on the messages before returning
                switch (msg.GetCommandType())
                {
                    case "SHOUT":
                        ARG = msg.GetCommandArgumentList()[0].ToString();
                        NwnxAssembly.CBinding.RunAScript("nwnx_shout", ARG);
                        break;

                    default:
                        try
                        {
                            ARG = msg.GetCommandArgumentList()[0].ToString();
                            ARG2 = msg.GetCommandArgumentList()[1].ToString();
                            NwnxAssembly.CBinding.RunAScript(ARG, ARG2);
                        }
                        catch (Exception ee) { }
                        break;
                }

                //Finished acting
            }
            ListOfMessages.Clear();
            processingMessages = true;
            foreach (NetMessage.NetMessage newMessage in ExternalList)
            {
                ListOfMessages.Add(newMessage);
            }
            processingMessages = false;
            ExternalList.Clear();

            Magic.Instance.Detours["MainLoop"].CallOriginal(instance);
        }





        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void MainLoopDelegate(uint pThis);


    }
}
