using System;
using Org.Utils;
using System.Data;
using System.Collections.Generic;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_InsertNotifyUser")]
    public class UserNotification:BaseEntity
    {

        [DataField(Size = 256, Type = DbType.String)]
        public string UserIdNotifyTo { get; set; }

        [DataField(Size = 256, Type = DbType.String)]
        public string UserIdNotifyFrom { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsActive { get; set; }

        [DataField(Type = DbType.Int32)]
        public int IsInsert { get; set; }

        [NonDBField(Type = DbType.String)]
        public string CustomUserIdFrom { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }

    }
}
