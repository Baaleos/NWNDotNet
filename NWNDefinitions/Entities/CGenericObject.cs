using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWNDotNet.NWNDefinitions.Entities
{
    class CGenericObject
    {
        public CGenericObject(uint Address)
        {
            FinalAddress = Address;
        }

        uint FinalAddress = 0x0;

        public uint ObjectType
        {
            get
            {
                return AccessMemory.ReadPointer(FinalAddress + 0x000C);
            }
        }
        public string LastName
        {
            get
            {
                uint add =  AccessMemory.ReadPointer(FinalAddress + 0x0010);
                return AccessMemory.ReadString(add);
            }

        }
    }
}
