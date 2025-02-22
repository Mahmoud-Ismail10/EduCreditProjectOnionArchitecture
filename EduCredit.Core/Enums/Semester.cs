using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Enums
{
    public enum Semester
    {
        [EnumMember(Value = "First")]
        First,
        [EnumMember(Value = "Second")]
        Second
    }
}
