using System;
using Org.Utils;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetMembersList")]
    public class Member : BaseEntity
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
        public string ProfileImage { get; set; }

        [DataField(Type = DbType.Int64)]
        public long ApprovedProducts { get; set; }

        [DataField(Type = DbType.Int64)]
        public long TotalProducts { get; set; }

        [DataField(Type = DbType.Int64)]
        public long ApprovedAffiliates { get; set; }

        [DataField(Type = DbType.Int64)]
        public long TotalAffiliates { get; set; }

        [DataField(Type = DbType.Int64)]
        public long TotalCount { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal Earnings { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal PayGenieFee { get; set; }

        [NonDBField(Type = DbType.String)]
        public string AdminUserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset RecCreatedDt { get; set; }


        public static DateTimeOffset MinRegDate = DateTimeOffset.Now.AddYears(-7);
        public static DateTimeOffset MaxRegDate = DateTimeOffset.Now;

        public Member()
        {

            StartRegDate = MinRegDate;
            EndRegDate = MaxRegDate;
            CurrencyCode = "USD";
            CurrencySymbol = "$";
        }

        [DataField(Size = 5, Type = DbType.String)]
        public string CurrencySymbol { get; set; }


        [DataField(Size = 5, Type = DbType.String)]
        public string CurrencyCode { get; set; }

        [NonDBField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset EndRegDate { get; set; }

        [NonDBField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset StartRegDate { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsActive { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsLockedOut { get; set; }

        [NonDBField(Size = 5, Type = DbType.String)]
        public int IsBlackListed { get; set; }


        [DataField(Type = DbType.Int64)]
        public long BlackListId { get; set; }
    }
}
