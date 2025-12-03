using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail.Model;
using Org.Business.Methods;
using Org.Business.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using S22.Mail;
using System.Security.Cryptography;
using System.Text;

namespace EmailService
{
    public static class EmailProcessor
    {
        private static long _TotalEmails = 0;

        private static long _ProcessedEmails = 0;

        private static long _EventId = 0;

        public static string Events = "";

        public static long TotalEmails { get { return _TotalEmails; } }

        public static long ProcessedEmails { get { return _ProcessedEmails; } }

        public static long EventId { get { return _EventId; } }

        private static bool IsStopped = false;

        private static bool HasStopped = true;

        private static ApplicationPushNotificationSetting AppSetting { get; set; }

        private static CancellationToken ct { get; set; }

        public async static void ProcessEmail(object eventLogger)
        {
            //Debugger.Launch();
            try
            {
                _EventId = 1;
                if ((ProcessedEmails > 0 || (TotalEmails - ProcessedEmails) > 0))
                {
                    _EventId = 2;
                    WriteLogs("Processed emails are less than total emails");
                    return;
                }
                else
                {
                    HasStopped = true;
                    _EventId = 3;
                    Dispose();
                }
                var ApplicationName = "ASE";


                Events = "" + _EventId;

                AppSetting = GetApplicationNotificationSetting("ASE");
                if (AppSetting != null)
                {
                    AppSetting.Serialized = AppSetting.PushNotificationSettingsJson;
                }
                HasStopped = false;

                // _EventId = 2;

                Events += "," + _EventId;

                //List<UserFunnelEmail> lUserFunnelEmails = FunnelEmailBAL.GetFunnelStepEmails();
                var PerDayLimit = Convert.ToInt32(ConfigurationManager.AppSettings["PerDayLimit"]);
                var PerHourLimit = Convert.ToInt32(ConfigurationManager.AppSettings["PerHourLimit"]);
                var BadgeInterval = Convert.ToInt32(ConfigurationManager.AppSettings["BadgeInterval"]);
                var BadgeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["BadgeLimit"]);

                var readyNote = PushNotificationsBAL.GetNotifications("email", PerDayLimit, PerHourLimit, BadgeLimit, BadgeInterval, ct, AppSetting, ApplicationName);
                WriteLogs("Totla emails in Queue " + readyNote.Count);
                //Debugger.Launch()
                //_EventId = 3;

                Events += "," + _EventId;

                if (readyNote == null || readyNote.Count <= 0) { return; }


                _EventId = 4;

                Events += "," + _EventId;

                _TotalEmails = 0;
                _ProcessedEmails = 0;

                if (readyNote.Count > 0)
                {
                    _TotalEmails = readyNote.Count;
                }
                _EventId = 5;
                Events += "," + _EventId;

                if (readyNote != null && readyNote.Count > 0)
                {
                    foreach (var item in readyNote)
                    {
                        if (item != null && item.Id > 0 && Convert.ToBoolean(item.IsAWSBulkEmail))
                        {
                            await SendEmailUsingAWS(item, AppSetting, ApplicationName);
                        }
                        else
                        {
                            await SendEmailNotificationMessageAsync(item, ct, AppSetting, ApplicationName);
                        }
                    }
                }
                //if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsAWSBulkEmail"]))
                //{
                //    await SendUsingAWS(readyNote, AppSetting, ApplicationName);
                //}
                //else
                //{
                //    await SendNotificationMessageAsync(readyNote, ct, AppSetting, ApplicationName);
                //}
                HasStopped = true;
                _EventId = 6;
                Events += "," + _EventId;
                Dispose();
            }
            catch (Exception ex)
            {
                //Debugger.Launch()
                _EventId = 7;
                Events += "," + _EventId;

                HasStopped = true;

                ((EventLog)eventLogger).WriteEntry("Exception Occured: (" + Convert.ToInt64(EmailProcessor.EventId) + ") Message: " + ex.Message);
                WriteLogs("Exception Occured: (" + Convert.ToInt64(EmailProcessor.EventId) + ") Message: " + ex.Message);
                if (ex.InnerException != null)
                {
                    ((EventLog)eventLogger).WriteEntry("Inner Exception: " + ex.InnerException.Message);
                }
            }
        }

        public static async Task SendEmailUsingAWS(PushNotificationItem ReadyNote, ApplicationPushNotificationSetting appSetting = null, string ApplicationName = "ASE")
        {
            try
            {
                //Debugger.Launch()
                _EventId = 8;
                Events += "," + _EventId;
                string AccessKey = ReadyNote.AwsAccessKeyId;//ConfigurationManager.AppSettings["AwsAccessKeyId"];
                string SecretKey = ReadyNote.AwsSecretKey;// ConfigurationManager.AppSettings["AwsSecretKey"];

                AWSCredentials credentials = new BasicAWSCredentials(AccessKey, SecretKey);
                _EventId = 9;
                Events += "," + _EventId;

                if (appSetting == null)
                {
                    appSetting = GetApplicationNotificationSetting(ApplicationName);
                }

                using (var client = AWSClientFactory.CreateAmazonSimpleEmailServiceClient(credentials, RegionEndpoint.USEast1))
                {
                    if (ReadyNote != null)
                    {
                        _EventId = 10;
                        Events += "," + _EventId;
                        appSetting.Serialized = ReadyNote.DestinationSettings;

                        var retryMax = appSetting.RetryAttempts;
                        var retryIntv = appSetting.RetryIntervalMinutes;

                        _EventId = 11;
                        Events += "," + _EventId;
                        var msg = GenerateMessageAsync(ReadyNote, ct);
                        var smsg = msg as SerializableMailMessage;

                        var emailContent = smsg.Body;

                        _EventId = 12;
                        Events += "," + _EventId;

                        string ToAddress = ReadyNote.DestinationAddress.ToString();
                        ToAddress = EncryptURL(ToAddress);
                        emailContent = emailContent.Replace("[[EmailAddress]]", ToAddress);

                        var SendRequest = new SendEmailRequest
                        {

                            Source = ReadyNote.FromEmail,//ConfigurationManager.AppSettings["FromEmail"], //smsg.From.Address.ToString(),
                            Destination = new Destination { ToAddresses = new List<string> { ReadyNote.DestinationAddress } },
                            Message = new Message
                            {
                                Subject = new Content(smsg.Subject),
                                Body = new Body { Html = new Content(emailContent) }
                            }
                        };
                        _EventId = 13;
                        Events += "," + _EventId;
                        try
                        {
                            //Debugger.Launch()
                            var response = await client.SendEmailAsync(SendRequest);

                            //Amazon.SQS.Model.ReceiveMessageResponse RMR = new Amazon.SQS.Model.ReceiveMessageResponse();
                            //var count = RMR.ReceiveMessageResult.Messages.Count;

                            _ProcessedEmails += 1;

                            _EventId = 14;
                            Events += "," + _EventId;
                            WriteLogs("SendUsingAWS Processed Emails " + _ProcessedEmails);
                            Events += " SendUsingAWS Processed Emails " + _ProcessedEmails + "At " + DateTimeOffset.Now + " :: ";
                        }
                        catch (Exception ex)
                        {
                            WriteLogs("SendUsingAWS Send Email Exception " + ex.Message);
                            _EventId = 16;
                            Events += "," + _EventId;
                            UpdateFailedNotificationStatus(ReadyNote, false, retryMax, retryIntv);
                        }
                        _EventId = 15;
                        Events += "," + _EventId;

                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogs("SendUsingAWS Main Send Email Exception:: " + ex.Message);
                _EventId = 16;
                Events += "," + _EventId;
            }
        }

        private static async Task SendEmailNotificationMessageAsync(PushNotificationItem ReadyNote, CancellationToken ct, ApplicationPushNotificationSetting appSetting = null, string ApplicationName = "ASE")
        {
            try
            {
                //////Debugger.Launch()
                _EventId = 8;
                Events += "," + _EventId;
                string DestinationType = "email";
                if (appSetting == null)
                {
                    appSetting = GetApplicationNotificationSetting(ApplicationName);
                }
                _EventId = 9;
                Events += "," + _EventId;

                _EventId = 10;
                Events += "," + _EventId;

                _EventId = 11;
                Events += "," + _EventId;
                if (ReadyNote.Destination == null)
                {
                    DestinationType = "email";
                }
                else
                {
                    DestinationType = ReadyNote.Destination.DestinationType;
                }
                _EventId = 12;
                Events += "," + _EventId;

                appSetting.Serialized = ReadyNote.DestinationSettings;

                var retryMax = appSetting.RetryAttempts;
                var retryIntv = appSetting.RetryIntervalMinutes;

                try
                {
                    //find a provider for the notification destination type
                    var provider = appSetting.DeliveryProviders.FirstOrDefault(p => p.DestinationType == DestinationType);
                    _EventId = 13;
                    Events += "," + _EventId;
                    if (provider == null)
                    {
                        //no provider
                        ReadyNote.DeliveryStatus = PushNotificationItemStatus.NotAvailable;
                        ReadyNote.ScheduledSendDate = null;
                    }
                    else
                    {
                        await provider.SendReadyMessageAsync(ReadyNote, retryMax, retryIntv, ct);
                        _ProcessedEmails += 1;
                        _EventId = 14;
                        Events += "," + _EventId;
                        WriteLogs("Processed Emails " + _ProcessedEmails);
                        Events += " Processed Emails " + _ProcessedEmails + "At " + DateTimeOffset.Now + " :: ";

                    }
                }
                catch (Exception ex)
                {
                    WriteLogs("Send Email Exception " + ex.Message);
                    _EventId = 16;
                    Events += "," + _EventId;
                }
                _EventId = 15;
                Events += "," + _EventId;

            }
            catch (Exception ex)
            {
                WriteLogs("Send Email Exception " + ex.Message);
                _EventId = 16;
                Events += "," + _EventId;
                throw ex;
            }
        }

        public static ApplicationPushNotificationSetting GetApplicationNotificationSetting(string ApplicationName)
        {
            List<ApplicationPushNotificationSetting> appSettings = PushNotificationsBAL.GetApplicationNotificationSettings(ApplicationName);

            if (appSettings == null || appSettings.Count <= 0)
            {
                return null;
            }

            return appSettings[0];
        }

        public static void WriteLogs(string message, string fileName = null)
        {
            try
            {
                string mint = DateTime.Now.Minute.ToString();
                if (mint.Length == 1)
                {
                    mint = "0" + mint;
                }
                mint = "00";    //Same File for whole Hour
                string hour = DateTime.Now.Hour.ToString();
                if (hour.Length == 1)
                {
                    hour = "0" + hour;
                }
                string day = DateTime.Now.Day.ToString();
                if (day.Length == 1)
                {
                    day = "0" + day;
                }
                string month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                string year = DateTime.Now.Year.ToString();

                string logPath = ConfigurationManager.AppSettings["LogPath"];
                if (string.IsNullOrEmpty(fileName))
                    fileName = ConfigurationManager.AppSettings["LogFileName"];
                string newPath = logPath + fileName + "/";
                if (!Directory.Exists(@newPath))
                {
                    Directory.CreateDirectory(@newPath);
                    logPath = newPath;
                }

                string file = newPath + fileName + year + month + day + ".txt";

                using (TextWriter output = File.AppendText(file))
                {
                    output.WriteLine(string.Format("{0} {1}: {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), message));
                }
            }
            catch (Exception ex)
            {
                string messageException = ex.Message;
            }
        }

        private static async Task SendNotificationMessageAsync(List<PushNotificationItem> ReadyNote, CancellationToken ct, ApplicationPushNotificationSetting appSetting = null, string ApplicationName = "ASE")
        {
            try
            {
                //////Debugger.Launch()
                _EventId = 8;
                Events += "," + _EventId;
                string DestinationType = "email";
                if (appSetting == null)
                {
                    appSetting = GetApplicationNotificationSetting(ApplicationName);
                }
                _EventId = 9;
                Events += "," + _EventId;

                _EventId = 10;
                Events += "," + _EventId;
                foreach (var note in ReadyNote)
                {
                    _EventId = 11;
                    Events += "," + _EventId;
                    if (note.Destination == null)
                    {
                        DestinationType = "email";
                    }
                    else
                    {
                        DestinationType = note.Destination.DestinationType;
                    }
                    _EventId = 12;
                    Events += "," + _EventId;

                    appSetting.Serialized = note.DestinationSettings;

                    var retryMax = appSetting.RetryAttempts;
                    var retryIntv = appSetting.RetryIntervalMinutes;

                    try
                    {
                        //find a provider for the notification destination type
                        var provider = appSetting.DeliveryProviders.FirstOrDefault(p => p.DestinationType == DestinationType);
                        _EventId = 13;
                        Events += "," + _EventId;
                        if (provider == null)
                        {
                            //no provider
                            note.DeliveryStatus = PushNotificationItemStatus.NotAvailable;
                            note.ScheduledSendDate = null;
                        }
                        else
                        {
                            await provider.SendReadyMessageAsync(note, retryMax, retryIntv, ct);
                            _ProcessedEmails += 1;
                            _EventId = 14;
                            Events += "," + _EventId;
                            WriteLogs("Processed Emails " + _ProcessedEmails);
                            Events += " Processed Emails " + _ProcessedEmails + "At " + DateTimeOffset.Now + " :: ";

                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs("Send Email Exception " + ex.Message);
                        _EventId = 16;
                        Events += "," + _EventId;
                    }
                    _EventId = 15;
                    Events += "," + _EventId;
                }
            }
            catch (Exception ex)
            {
                WriteLogs("Send Email Exception " + ex.Message);
                _EventId = 16;
                Events += "," + _EventId;
                throw ex;
            }
        }
        
        public static async Task SendUsingAWS(List<PushNotificationItem> ReadyNote, ApplicationPushNotificationSetting appSetting = null, string ApplicationName = "ASE")
        {
            try
            {
                //Debugger.Launch()
                _EventId = 8;
                Events += "," + _EventId;
                string AccessKey = ConfigurationManager.AppSettings["AwsAccessKeyId"];
                string SecretKey = ConfigurationManager.AppSettings["AwsSecretKey"];

                AWSCredentials credentials = new BasicAWSCredentials(AccessKey, SecretKey);
                _EventId = 9;
                Events += "," + _EventId;

                if (appSetting == null)
                {
                    appSetting = GetApplicationNotificationSetting(ApplicationName);
                }

                using (var client = AWSClientFactory.CreateAmazonSimpleEmailServiceClient(credentials, RegionEndpoint.USEast1))
                {
                    if (ReadyNote != null && ReadyNote.Count > 0)
                    {
                        _EventId = 10;
                        Events += "," + _EventId;
                        foreach (var note in ReadyNote)
                        {
                            appSetting.Serialized = note.DestinationSettings;

                            var retryMax = appSetting.RetryAttempts;
                            var retryIntv = appSetting.RetryIntervalMinutes;

                            _EventId = 11;
                            Events += "," + _EventId;
                            var msg = GenerateMessageAsync(note, ct);
                            var smsg = msg as SerializableMailMessage;

                            var emailContent = smsg.Body;

                            _EventId = 12;
                            Events += "," + _EventId;

                            string ToAddress = note.DestinationAddress.ToString();
                            ToAddress = EncryptURL(ToAddress);
                            emailContent = emailContent.Replace("[[EmailAddress]]", ToAddress);

                            var SendRequest = new SendEmailRequest
                            {

                                Source = ConfigurationManager.AppSettings["FromEmail"], //smsg.From.Address.ToString(),
                                Destination = new Destination { ToAddresses = new List<string> { note.DestinationAddress } },
                                Message = new Message
                                {
                                    Subject = new Content(smsg.Subject),
                                    Body = new Body { Html = new Content(emailContent) }
                                }
                            };
                            _EventId = 13;
                            Events += "," + _EventId;
                            try
                            {
                                //Debugger.Launch()
                                var response = await client.SendEmailAsync(SendRequest);

                                //Amazon.SQS.Model.ReceiveMessageResponse RMR = new Amazon.SQS.Model.ReceiveMessageResponse();
                                //var count = RMR.ReceiveMessageResult.Messages.Count;

                                _ProcessedEmails += 1;

                                _EventId = 14;
                                Events += "," + _EventId;
                                WriteLogs("SendUsingAWS Processed Emails " + _ProcessedEmails);
                                Events += " SendUsingAWS Processed Emails " + _ProcessedEmails + "At " + DateTimeOffset.Now + " :: ";
                            }
                            catch (Exception ex)
                            {
                                WriteLogs("SendUsingAWS Send Email Exception " + ex.Message);
                                _EventId = 16;
                                Events += "," + _EventId;
                                UpdateFailedNotificationStatus(note, false, retryMax, retryIntv);
                            }
                            _EventId = 15;
                            Events += "," + _EventId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogs("SendUsingAWS Main Send Email Exception:: " + ex.Message);
                _EventId = 16;
                Events += "," + _EventId;
            }
        }

        public static void UpdateFailedNotificationStatus(PushNotificationItem notificationItem, bool result, long retryMax, long retryIntv)
        {
            if (notificationItem.DeliveryStatus == PushNotificationItemStatus.Retrying)
            {
                notificationItem.RetryCount++;
            }
            if (result)
            {
                //if sent, mark sent and remove schedule
                notificationItem.DeliveryStatus = PushNotificationItemStatus.Sent;
                notificationItem.ScheduledSendDate = null;
            }
            else
            {
                if (notificationItem.RetryCount <= retryMax)
                {
                    //mark for retry, update schedule
                    notificationItem.DeliveryStatus = PushNotificationItemStatus.Retrying;
                    if (notificationItem.ScheduledSendDate != null)
                    {
                        notificationItem.ScheduledSendDate =
                            notificationItem.ScheduledSendDate.Value.AddMinutes(retryIntv ^ notificationItem.RetryCount);
                    }
                }
                else
                {
                    //too many retry attempts, mark fail and clear schedule
                    notificationItem.DeliveryStatus = PushNotificationItemStatus.Failed;
                    notificationItem.ScheduledSendDate = null;
                }
                //Org.TDS.Utils.Logger.LogRelativeMessage(("notificationItem.DeliveryStatus :: " + notificationItem.DeliveryStatus));
                PushNotificationsBAL.UpdateNotificationStatus(notificationItem);
            }

        }

        public static object GenerateMessageAsync(PushNotificationItem notificationItem, CancellationToken ct)
        {
            var memorydata = Convert.FromBase64String(notificationItem.MessageContent);
            using (var rs = new MemoryStream(memorydata))
            {
                var sf = new BinaryFormatter();
                return sf.Deserialize(rs);
            }
        }

        public static void Dispose(string BatchStatus = "COMPLETED")
        {
            IsStopped = true;

            _EventId = 17;

            while (!HasStopped)
            {
                _EventId = 18;
                IsStopped = true;
            }

            _EventId = 19;


            _EventId = 20;

            _EventId = 21;

            _TotalEmails = 0;
            _ProcessedEmails = 0;
            HasStopped = true;
            IsStopped = false;
        }

        public static string EncryptURL(string clearText)
        {
            if (string.IsNullOrWhiteSpace(clearText))
            {
                return clearText;
            }
            string EncryptionKey = "e9c33914-0ad4-4926-ab90-bac61713f931";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    clearText = Convert.ToBase64String(ms.ToArray());
                    clearText = clearText.Replace('/', '_');
                    clearText = clearText.Replace('+', '-');
                    int mm = clearText.Replace(" ", "").Length % 4;
                    if (mm > 0)
                    {
                        clearText += new string('=', 4 - mm);
                    }
                }
            }
            return clearText;
        }
    }

    /// <summary>Represents the bounce or complaint notification stored in Amazon SQS.</summary>
    class AmazonSqsNotification
    {
        public string Type { get; set; }
        public string Message { get; set; }
    }

    /// <summary>Represents an Amazon SES bounce notification.</summary>
    class AmazonSesBounceNotification
    {
        public string NotificationType { get; set; }
        public AmazonSesBounce Bounce { get; set; }
    }
    /// <summary>Represents meta data for the bounce notification from Amazon SES.</summary>
    class AmazonSesBounce
    {
        public string BounceType { get; set; }
        public string BounceSubType { get; set; }
        public DateTime Timestamp { get; set; }
        public List<AmazonSesBouncedRecipient> BouncedRecipients { get; set; }
    }
    /// <summary>Represents the email address of recipients that bounced
    /// when sending from Amazon SES.</summary>
    class AmazonSesBouncedRecipient
    {
        public string EmailAddress { get; set; }
    }
}
