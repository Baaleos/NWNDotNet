using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace NwnxAssembly
{
    public class CBinding
    {

       [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
       private delegate int RunScriptDelegate(string sScript);

       [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
       private delegate string GetData();

       [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
       private delegate void SetData(byte[] sData);


        [DllImport("nwnx_cool.dll", EntryPoint = "RunScriptPublic", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetClassObject(string ScriptName);




        private static SetData setTheData = null;
        private static GetData getTheData = null;
        private static RunScriptDelegate runTheScript = null;
        private static ProcessModule module = null;
       public static string RunAScript(string sScript, string sArgument)
       {
           string sModules = "";
           try
           {
               //IntPtr pDll = NativeMethods.LoadLibrary(@"PathToYourDll.DLL");
               //oh dear, error handling here
               //if (pDll == IntPtr.Zero)if()
               if (module == null)
               {
                   foreach (ProcessModule pm in Process.GetCurrentProcess().Modules)
                   {

                       if (pm.ModuleName.Contains("cool"))
                       {
                           module = pm;
                           IntPtr pAddressOfRunScript = NativeMethods.GetProcAddress(pm.BaseAddress, "RunScriptPublic");
                           //return pAddressOfFunctionToCall.ToString();
                           runTheScript = (RunScriptDelegate)Marshal.GetDelegateForFunctionPointer(
                                                                                           pAddressOfRunScript,
                                                                                           typeof(RunScriptDelegate));

                           IntPtr pAddressOfGetData = NativeMethods.GetProcAddress(pm.BaseAddress, "GetData");
                           //return pAddressOfFunctionToCall.ToString();
                           getTheData = (GetData)Marshal.GetDelegateForFunctionPointer(
                                                                                           pAddressOfGetData,
                                                                                           typeof(GetData));

                           IntPtr pAddressOfSetData = NativeMethods.GetProcAddress(pm.BaseAddress, "SetData");
                           //return pAddressOfFunctionToCall.ToString();
                           setTheData = (SetData)Marshal.GetDelegateForFunctionPointer(
                                                                                           pAddressOfSetData,
                                                                                           typeof(SetData));
                           byte[] buffer = Encoding.ASCII.GetBytes(sArgument);
                           setTheData(buffer);
                           return runTheScript(sScript).ToString();
                       }
                   }
                   //oh dear, error handling here
                   //if(pAddressOfFunctionToCall == IntPtr.Zero)
               }
               else
               {
                   byte[] buffer = Encoding.ASCII.GetBytes(sArgument);
                   setTheData(buffer);
                   return runTheScript(sScript).ToString();
               }
              // MultiplyByTen multiplyByTen = (MultiplyByTen)Marshal.GetDelegateForFunctionPointer(
              //                                                                         pAddressOfFunctionToCall,
              //                                                                         typeof(MultiplyByTen));
               return "";
           }
           catch (Exception ee)
           {
               return ee.ToString();
           }
       }


    }

    static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);


        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
}
