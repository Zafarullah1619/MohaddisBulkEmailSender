using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetCustomersListForECB")]
    public class ProductCustomer
    {
        [DataField(Type = DbType.String)]
        public string FirstName { get; set; }

        [DataField(Type = DbType.String)]
        public string LastName { get; set; }

        [DataField(Type = DbType.String)]
        public string Email { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool IsActive { get; set; }

        [DataField(Type = DbType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Rule { get; set; }

    }
}
