using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace NWNDotNet.NWNDefinitions
{
    public class ServerExoApp
    {
        static ServerExoApp()
        {
            //AppManager + 4 = ServerExoApp
            FinalAddress = NWNDefinitions.AppManager.FinalAddress += 0x4;
            FinalAddress = (uint)AccessMemory.ReadPointer(FinalAddress);
           

        }
        public static uint FinalAddress = 0x0;

        #region Delegates

            [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
            delegate int GetModuleLanguageDelegate(uint pThis);

            [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
            delegate uint GetGameObjectByIdDelegate(uint pThis, uint GameObjectID);

        #endregion

        #region Method Setup
            private static GetModuleLanguageDelegate Internal_GetModuleLanguage = WhiteMagic.Magic.Instance.RegisterDelegate<GetModuleLanguageDelegate>(0x0042C900);
            private static GetGameObjectByIdDelegate Internal_GetGameObject = WhiteMagic.Magic.Instance.RegisterDelegate<GetGameObjectByIdDelegate>(0x0042C810);
        #endregion

        //0x0042C900
        public static int GetModuleLanguage()
        {
            return Internal_GetModuleLanguage(FinalAddress);
        }


        //0x0042C810
        public static uint GetGameObject(string uInt)
        {
            uint NewValue = (uint)System.Int32.Parse(uInt, System.Globalization.NumberStyles.HexNumber);

            uint retVal = Internal_GetGameObject(FinalAddress,NewValue);
            //Entities.CGenericObject obj = new Entities.CGenericObject(retVal);
            //System.Windows.Forms.MessageBox.Show(obj.LastName.ToString());
            return (uint)retVal;
        }




        /*
         * uint32_t                   *vftable;

        CServerExoAppInternal      *srv_internal;

        CWorldTimer*		GetActiveTimer(uint32_t a1);
        CNWSPlayer* 		GetClientObjectByObjectId(nwn_objid_t oID);
        CNWSClient * 		GetClientObjectByPlayerId(uint32_t player_id, uint8_t a3);
        CGenericObject*		GetGameObject(nwn_objid_t oID);
        int					GetIsControlledByPlayer(nwn_objid_t oID);
        int					GetModuleLanguage();
        CNWSMessage*		GetNWSMessage();
        CGameObjectArray*	GetObjectArray();
        void* 				GetPlayerList();
        CServerAIMaster*	GetServerAIMaster();
         * /*/


    }
}
