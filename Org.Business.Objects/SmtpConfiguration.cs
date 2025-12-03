using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetUserSmtpConfigurations")]
    public class SmtpConfiguration : BaseEntity
    {
        [DataField(Type = DbType.Int32)]
        public long ProductId { get; set; }

        [DataField(Type = DbType.String)]
        public string Title { get; set; }

        [DataField(Type = DbType.String)]
        public string FromName { get; set; }

        [DataField(Type = DbType.String)]
        public string FromEmail { get; set; }

        [DataField(Type = DbType.String)]
        public string SmtpServer { get; set; }

        [DataField(Type = DbType.Int32)]
        public long? SmtpPort { get; set; }

        [DataField(Type = DbType.String)]
        public string SmtpUser { get; set; }

        [DataField(Type = DbType.String)]
        public string SmtpPassword { get; set; }

        [DataField(Type = DbType.String)]
        public string SmtpDomain { get; set; }

        [DataField(Type = DbType.String)]
        public string SmtpFooter { get; set; }

        [DataField(Type = DbType.String)]
        public bool? IsDefault { get; set; }

        [DataField(Type = DbType.String)]
        public string PushNotificationSettingsJson { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool? EnableSSL { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool? IsAWSBulkEmail { get; set; }

        [DataField(Type = DbType.String)]
        public string AwsServiceURL { get; set; }

        [DataField(Type = DbType.Int32)]
        public long AwsBulkEmailCount { get; set; }

        [DataField(Type = DbType.String)]
        public string AwsAccessKeyId { get; set; }

        [DataField(Type = DbType.String)]
        public string AwsSecretKey { get; set; }

        [DataField(Type = DbType.String)]
        public string RecCreatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecCreatedDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecUpdatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecUpdatedDt { get; set; }
    }

    
}
