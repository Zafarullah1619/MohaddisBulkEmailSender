using System;
using Org.Utils;
using System.Data;
using System.Collections.Generic;

namespace Org.Business.Objects
{
    [DataTable("tblNavigations")]
    [StoreProcedure("sp_GetNavigationList")]
    public class Navigation : BaseEntity
    {
        [DataField(IsDBNull = false, Size = 1024, Type = DbType.String)]
        public string MenuTitle { get; set; }

        [DataField(IsDBNull = false, Size = 256, Type = DbType.String)]
        public string MenuIcon { get; set; }

        [DataField(IsDBNull = false, Type = DbType.Int64)]
        public long DisplayOrder { get; set; }

        [DataField(Type = DbType.Int64)]
        public Nullable<long> ParentId { get; set; }

        [DataField(Size = 256, Type = DbType.String)]
        public string ApplicationArea { get; set; }

        [DataField(Size = 256, Type = DbType.String)]
        public string Area { get; set; }

        [DataField(Size = 256, Type = DbType.String)]
        public string Controller { get; set; }

        [DataField(Size = 256, Type = DbType.String)]
        public string Action { get; set; }

        [NonDBField(Type = DbType.String)]
        public string UserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string MainUserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string RoleId { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsParentMenu { get; set; }

        [NonDBField]
        public List<Navigation> SubMenu { get; set; }

        public static Predicate<Navigation> ById(long Id)
        {
            return delegate (Navigation navigation)
            {
                return navigation.Id == Id;
            };
        }
    }
}
