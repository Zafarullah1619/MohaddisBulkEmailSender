using System;
using Org.Utils;
using System.Data;
using System.Collections.Generic;

namespace Org.Business.Objects
{
    [DataTable("tblMainModules")]
    [StoreProcedure("sp_GetUserModulesList")]
    public class MainModule : BaseEntity
    {
        [DataField(IsDBNull = false, Size = 512, Type = DbType.String)]
        public string ModuleName { get; set; }

        [DataField(IsDBNull = false, Size = 1024, Type = DbType.String)]
        public string ModuleCode { get; set; }

        [NonDBField(Type = DbType.String)]
        public string UserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string MainUserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string RoleId { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public int CheckAreaPermissions { get; set; }

        [NonDBField]
        public List<ApplicationArea> ApplicationAreas { get; set; }
    }

    [DataTable("tblApplicationAreas")]
    [StoreProcedure("sp_GetUserApplicationAreasList")]
    public class ApplicationArea : BaseEntity
    {

        [DataField(IsDBNull = false, Size = 512, Type = DbType.String)]
        public string ApplicationAreaName { get; set; }

        [DataField(IsDBNull = false, Size = 1024, Type = DbType.String)]
        public string ApplicationAreaCode { get; set; }

        [DataField(Size = 20, Type = DbType.Int32)]
        public Nullable<int> ModuleId { get; set; }

        [DataField(Size = 20, Type = DbType.Int32)]
        public Nullable<int> ParentId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string UserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string MainUserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string RoleId { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public int CheckAreaPermissions { get; set; }

        [NonDBField]
        public List<ApplicationArea> SubAreas { get; set; }

        [NonDBField]
        public List<AreaRule> AreaRules { get; set; }
    }

    [DataTable("tblAreaRules")]
    [StoreProcedure("sp_GetUserAreaRulesList")]
    public class AreaRule : BaseEntity
    {
        [DataField(IsDBNull = false, Size = 20, Type = DbType.Int32)]
        public int ApplicationAreaId { get; set; }

        [DataField(IsDBNull = false, Size = 512, Type = DbType.String)]
        public string AreaRuleName { get; set; }

        [DataField(IsDBNull = false, Size = 1536, Type = DbType.String)]
        public string AreaRuleCode { get; set; }

        [DataField(IsDBNull = false, Size = 1024, Type = DbType.String)]
        public string AreaDetails { get; set; }

        [DataField(Size = 20, Type = DbType.Int32)]
        public Nullable<int> NavigationId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string UserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string MainUserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string RoleId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }

        [NonDBField(Size = 20, Type = DbType.Int32)]
        public Nullable<int> ModuleId { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public int CheckAreaPermissions { get; set; }

        public static Predicate<AreaRule> ByRuleCode(string ruleCode)
        {
            return delegate (AreaRule areaRule)
            {
                return areaRule.AreaRuleCode == ruleCode;
            };
        }
    }
}
