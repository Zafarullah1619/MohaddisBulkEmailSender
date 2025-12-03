using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    public class ProductContent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [DataField(Type = DbType.String)]
        public string RecCreatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecCreatedDt { get; set; }
    }
}
