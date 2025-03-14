using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Enums
{
    public enum Appreciation
    {
        [EnumMember(Value = "A+")]
        Aplus,
        [EnumMember(Value = "A")]
        A,
        [EnumMember(Value = "B+")]
        Bplus,
        [EnumMember(Value = "B")]
        B,
        [EnumMember(Value = "C+")]
        Cplus,
        [EnumMember(Value = "C")]
        C,
        [EnumMember(Value = "D")]
        D,
        [EnumMember(Value = "F")]
        F
    }
}
