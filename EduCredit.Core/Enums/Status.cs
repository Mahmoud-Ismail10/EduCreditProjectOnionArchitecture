using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Enums
{
    public enum Status
    {
        [EnumMember(Value = "Pending")]
        Pending,    // Waiting for approval or action
        [EnumMember(Value = "Approved")]
        Approved,   // Successfully approved
        [EnumMember(Value = "Rejected")]
        Rejected    // Declined or not accepted
    }
}
