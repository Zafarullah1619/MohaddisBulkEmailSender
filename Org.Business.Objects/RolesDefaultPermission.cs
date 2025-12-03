using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetRolesDefaultPermissions")]
    public class RolesDefaultPermission :BaseEntity
    {
        [DataField(Type = DbType.String)]
        public string RoleId { get; set; }

        [DataField(Type = DbType.String)]
        public string RoleName { get; set; }

        [DataField(Type = DbType.String)]
        public string ModulesAssigned { get; set; }

        [DataField(Type = DbType.String)]
        public string RightsAssigned { get; set; }

        [NonDBField(Size = 512, Type = DbType.String)]
        public string CurrentUserId { get; set; }

        [NonDBField(Size = 512, Type = DbType.String)]

        public string SessionAdminUserId { get; set; }

    }

}
