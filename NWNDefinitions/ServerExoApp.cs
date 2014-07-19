using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace NWNDotNet.NWNDefinitions
{
    class ServerExoApp
    {
        static ServerExoApp()
        {
            //AppManager + 4 = ServerExoApp
            FinalAddress = AppManager.FinalAddress += 0x4;
            FinalAddress = (uint)AccessMemory.ProcessMemory.ReadPointer(FinalAddress);
        }
        public static uint FinalAddress = 0x0;

        #region Delegates

            [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
            public delegate int GetModuleLanguageDelegate(uint pThis);

        #endregion

        #region Method Setup
            public static GetModuleLanguageDelegate Internal_GetModuleLanguage = WhiteMagic.Magic.Instance.RegisterDelegate<GetModuleLanguageDelegate>(0x0042C900);
        #endregion

        //0x0042C900
        public static int GetModuleLanguage()
        {
            return Internal_GetModuleLanguage(FinalAddress);
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
