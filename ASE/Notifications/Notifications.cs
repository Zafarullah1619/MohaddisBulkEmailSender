using Org.Business.Methods;
using Org.Business.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace ASE.Notifications
{
    public class InProcessPushNotificationScheduler
    {

        private static readonly Timer Timer = new Timer(OnTimerElapsed);
        private static ApplicationPushNotificationSetting AppSetting { get; set; }

        public static void Start(ApplicationPushNotificationSetting appSetting = null)
        {
            if (appSetting == null)
            {
                AppSetting = NotificationsHandler.GetApplicationNotificationSetting("ASE");
                AppSetting.Serialized = AppSetting.PushNotificationSettingsJson;
            }
            else
            {
                AppSetting = appSetting;
            }

            int interval = AppSetting.DeliveryIntervalMinutes;
            Timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(interval * 60 * 1000));
        }

        private static void OnTimerElapsed(object sender)
        {
            //HostingEnvironment.QueueBackgroundWorkItem(
            //    async ct =>
            //    {
            //        await NotificationsHandler.SendNotificationsAsync("email", ct, AppSetting);
            //    });

        }
    }
    public class NotificationsHandler
    {
        public static ApplicationPushNotificationSetting GetApplicationNotificationSetting(string ApplicationName)
        {
            List<ApplicationPushNotificationSetting> appSettings = PushNotificationsBAL.GetApplicationNotificationSettings(ApplicationName);

            if (appSettings == null || appSettings.Count <= 0)
            {
                return null;
            }

            return appSettings[0];
        }


        public static void SendEmail(string SessionUserId, string UserId, string ToEmail, string CcEmail, string BccEmail, string FromEmail, string FromName, string Subject, string Body)
        {
            PushNotificationItem pItem = new PushNotificationItem();

            try
            {
                //MailMessage message = new MailMessage(new MailAddress(FromEmail), new MailAddress(ToEmail));
                MailMessage message = new MailMessage();
                message.From = new MailAddress(FromEmail, FromName);
                message.Subject = Subject;
                message.Body = Body;
                try
                {
                    message.CC.Add(new MailAddress(CcEmail));
                }
                catch
                {

                }
                try
                {
                    message.Bcc.Add(new MailAddress(BccEmail));
                }
                catch
                {

                }
                message.IsBodyHtml = true;
                pItem.MessageContent = NotificationsEmailHandler.GetEmail(message);
                pItem.ToEmail = ToEmail;

                pItem.UserId = UserId;
                pItem.SessionUserId = SessionUserId;
                pItem.ContentSourceType = "email";

                PushNotificationsBAL.AddNotification(pItem);
            }
            catch (Exception ex)
            {

            }
        }


        public static async Task<int> SendNotificationsAsync(string ContentSourceType, CancellationToken ct, ApplicationPushNotificationSetting appSetting = null, string ApplicationName = "PayGenieNetwork")
        {
            try
            {
                var PerDayLimit = Convert.ToInt32(ConfigurationManager.AppSettings["PerDayLimit"]);
                var PerHourLimit = Convert.ToInt32(ConfigurationManager.AppSettings["PerHourLimit"]);
                var BadgeInterval = Convert.ToInt32(ConfigurationManager.AppSettings["BadgeInterval"]);
                var BadgeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["BadgeLimit"]);

                var readyNote = PushNotificationsBAL.GetNotifications(ContentSourceType, PerDayLimit, PerHourLimit, BadgeLimit, BadgeInterval, ct, appSetting, ApplicationName);

                if (readyNote == null || readyNote.Count <= 0) { return -1; }
                //return -1;
                await SendNotificationMessageAsync(readyNote, ct, appSetting, ApplicationName);
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        private static async Task SendNotificationMessageAsync(List<PushNotificationItem> ReadyNote, CancellationToken ct, ApplicationPushNotificationSetting appSetting = null, string ApplicationName = "PayGenieNetwork")
        {
            try
            {
                string DestinationType = "email";

                foreach (var note in ReadyNote)
                {
                    if (note.Destination == null)
                    {
                        DestinationType = "email";
                    }
                    else
                    {
                        DestinationType = note.Destination.DestinationType;
                    }



                    try
                    {
                        appSetting.Serialized = note.DestinationSettings;

                        var retryMax = appSetting.RetryAttempts;
                        var retryIntv = appSetting.RetryIntervalMinutes;

                        var provider = appSetting.DeliveryProviders.FirstOrDefault(p => p.DestinationType == DestinationType);
                        if (provider == null)
                        {
                            //no provider
                            note.DeliveryStatus = PushNotificationItemStatus.NotAvailable;
                            note.ScheduledSendDate = null;
                        }
                        else
                        {
                            await provider.SendReadyMessageAsync(note, retryMax, retryIntv, ct);
                        }
                    }
                    catch (Exception)
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
