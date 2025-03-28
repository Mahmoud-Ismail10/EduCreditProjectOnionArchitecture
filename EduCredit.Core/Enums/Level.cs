using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Enums
{
    public enum Level
    {
        [EnumMember(Value = "First")]
        First=1,
        [EnumMember(Value = "Second")]
        Second=2,
        [EnumMember(Value = "Third")]
        Third=3,
        [EnumMember(Value = "Fourth")]
        Fourth=4
    }
}
