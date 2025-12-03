using Org.Utils;
using System.Data;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetAspNetRoles")]
    public class AspNetRole
    {
        [DataField(Type = DbType.String)]
        public string Id { get; set; }

        [DataField(Type = DbType.String)]
        public string Name { get; set; }

    }
}
