using Newtonsoft.Json.Linq;
using Org.Business.Objects;
using S22.Mail;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using ASE.Utility;

namespace ASE.Notifications
{
    public sealed class SMTPDeliveryProvider : EmailDeliveryProviderBase
    {
        public SMTPDeliveryProvider(JToken configuration)
        {
            Configuration = configuration == null ?
                new SMTPDeliveryProviderConfiguration() :
                configuration.ToObject<SMTPDeliveryProviderConfiguration>();
        }

        public override Task<bool> SendNotificationAsync(PushNotificationItem notificationItem, object message, CancellationToken ct)
        {
            var cfg = (SMTPDeliveryProviderConfiguration)Configuration;

            try
            {
                var DestinationSettings = notificationItem.DestinationSettings;

                if (DestinationSettings != null)
                {
                    var DeliveryProviderSettings = (Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationPushNotificationSetting>(DestinationSettings).DeliveryProviderSettings).FirstOrDefault();
                    if (DeliveryProviderSettings != null && DeliveryProviderSettings.ProviderConfigurationData != null)
                    {
                        cfg = DeliveryProviderSettings.ProviderConfigurationData.ToObject<SMTPDeliveryProviderConfiguration>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            string FromAddress, FromDisplayName;

            var sent = false;

            var smsg = message as SerializableMailMessage;
            if (smsg != null)
            {
                try
                {
                    var client = new SmtpClient()
                    {
                        Host = cfg.SmtpServer,
                        Port = cfg.SmtpPort ?? 25,
                        EnableSsl = cfg.EnableSsl ?? false
                    };

                    if (!string.IsNullOrEmpty(cfg.SmtpUserName))
                    {
                        client.Credentials = new NetworkCredential(cfg.SmtpUserName, cfg.SmtpPassword);

                    }

                    //Guid id = Guid.NewGuid(); //Save the id in your database
                    string[] AddressParts = cfg.SmtpFromAddress.Split('@');
                    string ReplyToAddress = "";

                    if (AddressParts.Length > 1)
                    {
                        ReplyToAddress = "<" + AddressParts[0] + "+notification" + notificationItem.Id + "@" + AddressParts[1] + ">"; //String.Format("<{0}@{1}>", id.ToString(), "onlinehelpdesk.site");
                    }
                    else
                    {
                        ReplyToAddress = "<" + cfg.SmtpFromAddress + ">";
                    }
                    //smsg.Headers.Add("Message-Id", messageId);

                    //using (TextWriter output = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath("~/log.txt")))
                    //{

                    //}

                    smsg.To.Add(new MailAddress(notificationItem.Destination.DestinationAddress, notificationItem.Destination.FromName));

                    FromAddress = cfg.SmtpFromAddress;
                    FromDisplayName = cfg.SmtpFromDisplayName;

                    if (smsg.From != null && smsg.From.Address != null && smsg.From.Address.Length > 1)
                    {
                        FromAddress = smsg.From.Address;
                    }

                    if (smsg.From != null && smsg.From.DisplayName != null && smsg.From.DisplayName.Length > 1)
                    {
                        FromDisplayName = smsg.From.DisplayName;
                    }

                    //smsg.From = new MailAddress(cfg.SmtpFromAddress, cfg.SmtpFromDisplayName);
                    //smsg.ReplyTo = new MailAddress(ReplyToAddress, cfg.SmtpFromDisplayName);

                    smsg.From = new MailAddress(FromAddress, FromDisplayName);

                    smsg.Headers.Add("In-Reply-To", ReplyToAddress);
                    smsg.Headers.Add("References", ReplyToAddress);
                    //client.Timeout = 10000;
                    MailMessage m = smsg;
                    string ToAddress = smsg.To[0].Address.ToString();
                    ToAddress = StringHelper.EncryptURL(ToAddress);
                    m.Body = m.Body.Replace("[[EmailAddress]]", ToAddress);//.Replace("http://emailcampaignbuilder.com", "http://localhost:52679");
                    try
                    {
                        client.Send(m);
                    }
                    catch (Exception ex)
                    {

                    }

                    sent = true;

                }
                catch (Exception ex)
                {
                    //using (TextWriter output = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath("~/log.txt")))
                    {
                    }
                    sent = false;
                    //TODO: log this somewhere
                }

            }
            return Task.FromResult(sent);
        }
    }
}
