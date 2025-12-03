using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    
    public class BaseEntity
    {
        
        [KeyField]
        public Int64 Id { get; set; }

        [NonDBField(Size = 18, Type = DbType.Int64)]
        public Int64 PageNumber { get; set; }

        [NonDBField(Size = 18, Type = DbType.Int64)]
        public Int64 PageSize { get; set; }

        [NonDBField(Size = 10, Type = DbType.String)]
        public string SortOrder{ get; set; }

        [NonDBField(Size = 50, Type = DbType.String)]
        public string SortColumn { get; set; }

        [NonDBField(Size = 18, Type = DbType.Int64)]
        public Int64 OffSet { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }

    }
}
