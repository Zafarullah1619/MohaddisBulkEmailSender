using Org.Business.Methods;
using Org.Business.Objects;
using S22.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace ASE.Notifications
{
    public class NotificationsEmailHandler
    {
        public static void AfterSaleEmailNotification(string UserId, string PushNotificationSettingsJson, long? ProductId, string FromEmail, string FromName, string Subject, string EmailBody, string ToEmail, string Rule)
        {
            PushNotificationItem pItem = new PushNotificationItem();
            MailMessage message = new MailMessage();
            message.From = new MailAddress(FromEmail, FromName);

            message.Subject = Subject;
            message.Body = EmailBody;

            var Content = GetEmail(message);

            pItem.MessageContent = Content;
            pItem.ProductId = ProductId;
            pItem.Rule = Rule;
            pItem.ToEmail = ToEmail;
            pItem.PushNotificationSettingsJson = PushNotificationSettingsJson;
            pItem.UserId = UserId;
            pItem.SessionUserId = UserId;
            pItem.ContentSourceType = "email";
            PushNotificationsBAL.AddNotification(pItem);
        }
        public static string GetEmail(MailMessage mail, bool IsBodyHtml = true)
        {
            try
            {
                mail.IsBodyHtml = IsBodyHtml;

                SerializableMailMessage message = mail;

                using (var ms = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(ms, message);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
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
    }
}