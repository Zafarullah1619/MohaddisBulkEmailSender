using Org.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetSubscribers")]
    public class ProductSubscribers : BaseEntity
    {
        [DataField(Type = DbType.Int64)]
        public long Id { get; set; }

        [DataField(Type = DbType.String)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DataField(Type = DbType.String)]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ProductId is required")]
        [DataField(Type = DbType.Int32)]
        public long ProductId { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool IsActive { get; set; }

    }
}
