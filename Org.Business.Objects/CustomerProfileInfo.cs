using System;
using Org.Utils;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetCustomerProfileInfo")]
    public class CustomerProfileInfo : BaseEntity
    {
        [DataField(IsDBNull = true, Type = DbType.String)]
        public string FullName { get; set; }

        [DataField(Type = DbType.String)]
        public string FirstName { get; set; }

        [DataField(Type = DbType.String)]
        public string LastName { get; set; }

        [DataField(Type = DbType.String)]
        public string Email { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string AboutMe { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string UserId { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string CustomUserId { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string PhoneNumber { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string FacebookLink { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string TwitterLink { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string LinkedInLink { get; set; }

        [NonDBField(Type = DbType.String)]
        public string OldPassword { get; set; }

        [NonDBField(Type = DbType.String)]
        public string NewPassword { get; set; }

        [NonDBField(Type = DbType.String)]
        public string ConfirmPassword { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string ProfileImage { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }
    }
}
