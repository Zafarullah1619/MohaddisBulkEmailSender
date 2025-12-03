using Org.Utils;
using System.Collections.Generic;
using System.Data;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetConfiguration")]
    public class Configuration : BaseEntity
    {

        [DataField(Type = DbType.Decimal)]
        public decimal PayGenieFee { get; set; }


        [DataField(Type = DbType.Decimal)]
        public decimal ActivationFee { get; set; }


        [DataField(Type = DbType.Decimal)]
        public decimal ThresholdAmount { get; set; }


        [DataField(Type = DbType.Decimal)]
        public decimal PayGenieFixedFee { get; set; }


        [DataField(Type = DbType.Decimal)]
        public decimal HeatPopularity { get; set; }


        [DataField(Type = DbType.Decimal)]
        public decimal HeatDaysFactor { get; set; }


        [DataField(Type = DbType.Decimal)]
        public decimal ChargeBackAmount { get; set; }


        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool IsPrebillNotification { get; set; }


        [DataField(Type = DbType.Decimal)]
        public decimal PerbillNotificationSentBefore { get; set; }


        [DataField(Type = DbType.String)]
        public string PayPalPrimaryEmail { get; set; }


        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool IsOPRedirctToSuccess { get; set; }


        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool IsAllowRefundToSeller { get; set; }


        [DataField(Type = DbType.Int32)]
        public int? ProcessPaymentsGapInMinutes { get; set; }


        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }

        
    }
}
