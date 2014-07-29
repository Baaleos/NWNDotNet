using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NWNDotNet.NWNDefinitions.Entities
{
    class Creature
    {
        public Creature(uint add)
        {
            FinalAddress = add;
        }
        uint FinalAddress = 0x0;


        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate int GetBlindDelegate(uint pThis);
        #endregion


        #region Method Setup
            private static GetBlindDelegate Internal_GetIsBlind = WhiteMagic.Magic.Instance.RegisterDelegate<GetBlindDelegate>(0x004AEB40);
        #endregion

        public static int GetIsBlind(string ID)
        {
            //uint NewValue = (uint)System.Int32.Parse(ID, System.Globalization.NumberStyles.HexNumber);
            uint uNew = ServerExoApp.GetGameObject(ID);
            return Internal_GetIsBlind(uNew);
        }
    }
}
