
using Org.Utils;
using System.Data;

namespace Org.Business.Objects
{
    public interface IDeliveryProviderConfiguration
    {
        //marker interface
    }
    public class SMTPDeliveryProviderConfiguration : IDeliveryProviderConfiguration
    {
        public SMTPDeliveryProviderConfiguration()
        {
            SmtpServer = "localhost";
            SmtpPort = 25;
            EnableSsl = false;
            SmtpFromDisplayName = "ASE";
        }
        [DataField(Type = DbType.String)]
        public string SmtpServer { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Int32)]
        public int? SmtpPort { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool? EnableSsl { get; set; }

        [DataField(Type = DbType.String)]
        public string SmtpUserName { get; set; }

        [DataField(Type = DbType.String)]
        public string SmtpPassword { get; set; }

        [DataField(Type = DbType.String)]
        public string SmtpFromAddress { get; set; }

        [DataField(Type = DbType.String)]
        public string SmtpFromDisplayName { get; set; }

    }
}
