using Org.Utils;
using System.Collections.Generic;
using System.Data;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_DashboardInformationAndStatistics")]
    public class DashboardInfo
    {
        [NonDBField(Type = DbType.String)]
        public string UserId { get; set; }

        [DataField(Type = DbType.Int64)]
        public long TotalVendorCount { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal TotalTransVendorAmount { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal TotalVendorAmount { get; set; }

        [DataField(Type = DbType.Int64)]
        public long TotalAffiliateCount { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal TotalTransAffiliateAmount { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal TotalAffiliateAmount { get; set; }

        [DataField(Type = DbType.Int64)]
        public long VendorUserCount { get; set; }

        [DataField(Type = DbType.Int64)]
        public long VendorClickCount { get; set; }

        [DataField(Type = DbType.Int64)]
        public long AffiliateUserCount { get; set; }

        [DataField(Type = DbType.Int64)]
        public long AffiliateClickCount { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal VendorAffiliateAmount { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal VendorPayGenieAmount { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal AffiliateVendorAmount { get; set; }

        [DataField(Type = DbType.Decimal)]
        public decimal AffiliatePayGenieAmount { get; set; }
    }
}
