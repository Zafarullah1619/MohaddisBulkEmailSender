using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Org.Business.Objects;
using Org.DataAccess;
using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace Org.Business.Methods
{
    public class PushNotificationsBAL
    {
        public static List<ApplicationPushNotificationSetting> GetApplicationNotificationSettings(string ApplicationName = "PayGenieNetwork", string ConnectionStringName = "DefaultConnection")
        {
            using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
            {
                ApplicationPushNotificationSetting p = new ApplicationPushNotificationSetting();
                Filters Filter = new Filters();
                Filter.AddSqlParameters(() => p.ApplicationName, ApplicationName);
                IRepository<ApplicationPushNotificationSetting> oRepository = new Repository<ApplicationPushNotificationSetting>(uow.DataContext);

                return oRepository.LoadSP(Filter);
            }
        }

        public static void AddNotification(PushNotificationItem email, string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    Filters Filter = new Filters();

                    Filter.AddSqlParameters(() => email.SessionUserId, email.SessionUserId);
                    Filter.AddSqlParameters(() => email.ContentSourceType, email.ContentSourceType);
                    Filter.AddSqlParameters(() => email.MessageContent, email.MessageContent);
                    Filter.AddSqlParameters(() => email.UserId, email.UserId);
                    Filter.AddSqlParameters(() => email.ToEmail, email.ToEmail);
                    Filter.AddSqlParameters(() => email.ProductId, email.ProductId);
                    Filter.AddSqlParameters(() => email.Rule, email.Rule);
                    Filter.AddSqlParameters(() => email.PushNotificationSettingsJson, email.PushNotificationSettingsJson);
                    Filter.AddSqlParameters(() => email.Command, "ADD");

                    IRepository<PushNotificationItem> oRepository = new Repository<PushNotificationItem>(uow.DataContext);
                    oRepository.ExecuteSP(Filter);
                    //Org.Utils.Logger.LogRelativeMessage(("Add Notification Procedure executed: "));
                }
            }
            catch (Exception ex)
            {
                Org.Utils.Logger.LogRelativeMessage(("AddNotification Exception:: " + ex.Message), true);
                throw ex;
            }
        }


        public static List<PushNotificationItem> GetNotifications(string ContentSourceType, long? PerDayLimit, long? PerHourLimit, long? BadgeLimit, long? BadgeInterval, CancellationToken ct, ApplicationPushNotificationSetting appSetting = null, string ApplicationName = "ASE", string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    PushNotificationItem p = new PushNotificationItem();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.ContentSourceType, ContentSourceType);
                    Filter.AddSqlParameters(() => p.PerDayLimit, PerDayLimit);
                    Filter.AddSqlParameters(() => p.PerHourLimit, PerHourLimit);
                    Filter.AddSqlParameters(() => p.BadgeLimit, BadgeLimit);
                    Filter.AddSqlParameters(() => p.BadgeInterval, BadgeInterval);
                    IRepository<PushNotificationItem> oRepository = new Repository<PushNotificationItem>(uow.DataContext);

                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<PushNotificationItem> GetContactNotifications(string ContactEmail, string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    PushNotificationItem p = new PushNotificationItem();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.DestinationAddress, ContactEmail);
                    Filter.AddSqlParameters(() => p.Command, "Select");
                    IRepository<PushNotificationItem> oRepository = new Repository<PushNotificationItem>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetContactNotifications", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PushNotificationItem ContactNotificationDetail(long? Id, string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    PushNotificationItem p = new PushNotificationItem();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.Id, Id);
                    Filter.AddSqlParameters(() => p.Command, "EmailDetail");
                    IRepository<PushNotificationItem> oRepository = new Repository<PushNotificationItem>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetContactNotifications", Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool ResendContactNotification(long? Id, string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    PushNotificationItem p = new PushNotificationItem();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.Id, Id);
                    Filter.AddSqlParameters(() => p.Command, "SendEmail");
                    IRepository<PushNotificationItem> oRepository = new Repository<PushNotificationItem>(uow.DataContext);

                    return oRepository.ExecuteSP("sp_GetContactNotifications", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static BaseEntity UpdateNotificationStatus(PushNotificationItem notificationItem, string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => notificationItem.Id, notificationItem.Id);
                    Filter.AddSqlParameters(() => notificationItem.DeliveryStatus, notificationItem.DeliveryStatus);
                    Filter.AddSqlParameters(() => notificationItem.ScheduledSendDate, notificationItem.ScheduledSendDate);
                    IRepository<BaseEntity> oRepository = new Repository<BaseEntity>(uow.DataContext);

                    return oRepository.LoadSP("sp_UpdatePushNotificationStatus", Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool MarkAsUnsubscribed(string Email, string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    Filters Filter = new Filters();
                    PushNotificationItem notificationItem = new PushNotificationItem();
                    Filter.AddSqlParameters(() => notificationItem.Destination, Email);
                    IRepository<BaseEntity> oRepository = new Repository<BaseEntity>(uow.DataContext);

                    return oRepository.ExecuteSP("sp_MarkAsUnsubscribed", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool MarkAsBounced(string Email, string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    Filters Filter = new Filters();
                    PushNotificationItem notificationItem = new PushNotificationItem();
                    Filter.AddSqlParameters(() => notificationItem.Destination, Email);
                    IRepository<BaseEntity> oRepository = new Repository<BaseEntity>(uow.DataContext);

                    return oRepository.ExecuteSP("sp_MarkAsBounced", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool AdduserNotification(string toEmail, string MessageContent, string UserId, string ConnectionStringName = "DefaultConnection")
        //{
        //    try
        //    {
        //        using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
        //        {
        //            PushNotificationItem p = new PushNotificationItem();
        //            Filters Filter = new Filters();
        //            Filter.AddSqlParameters(() => p.ContentSourceType, "email");
        //            Filter.AddSqlParameters(() => p.MessageContent, MessageContent);
        //            Filter.AddSqlParameters(() => p.ToEmail, toEmail);
        //            Filter.AddSqlParameters(() => p.UserId, UserId);
        //            Filter.AddSqlParameters(() => p.Command, "ADD");
        //            IRepository<PushNotificationItem> oRepository = new Repository<PushNotificationItem>(uow.DataContext);

        //            return oRepository.ExecuteSP("sp_GetNotifications", Filter);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private static void AddNotification(string toEmail, string emailMessage, string emailSubject, string UserId)
        //{
        //    string fileName = null;
        //    try
        //    {
        //        MailMessage message = new MailMessage();
        //        string FromEmail = ConfigurationManager.AppSettings["SMTPFromEmail"];
        //        string FromName = ConfigurationManager.AppSettings["SMTPFromName"];
        //        message.From = new MailAddress(FromEmail, FromName);
        //        message.Subject = emailSubject;
        //        message.Body = emailMessage;
        //        message.IsBodyHtml = true;

        //        DatabaseContext dbContext = new DatabaseContext(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //        SqlParameter[] parameters = new SqlParameter[5];

        //        parameters[0] = new SqlParameter("ContentSourceType", "email");
        //        parameters[1] = new SqlParameter("MessageContent", GetEmail(message));
        //        parameters[2] = new SqlParameter("ToEmail", toEmail);
        //        parameters[3] = new SqlParameter("UserId", UserId);
        //        parameters[4] = new SqlParameter("Command", "ADD");
        //        SqlDataReader sqlReader = dbContext.ExecuteProcedure("sp_GetNotifications", parameters);


        //    }
        //    catch (Exception ex)
        //    {
        //        string message = "Exception Occured AddNotification Function: Message: " + ex.Message;
        //        if (ex.InnerException != null)
        //        {
        //            message += "_____Inner Exception: " + ex.InnerException.Message;
        //        }

        //    }
        //}


        //private static string GetEmail(MailMessage mail, bool IsBodyHtml = true)
        //{
        //    string fileName = null;
        //    try
        //    {
        //        mail.IsBodyHtml = IsBodyHtml;

        //        SerializableMailMessage message = mail;

        //        if (message.AlternateViews != null && message.AlternateViews.Count > 0)
        //        {
        //            foreach (var view in message.AlternateViews)
        //            {
        //                if (view.ContentStream.Position > 0)
        //                {
        //                    view.ContentStream.Position = 0;
        //                }
        //            }
        //        }

        //        using (var ms = new MemoryStream())
        //        {
        //            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(ms, message);
        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        public static string ProviderSettings(string Title, string FromName, string FromEmail, string SmtpServer, long SmtpPort, string SmtpUser, string SmtpPassword, string SmtpDomain, string SmtpFooter, bool IsDefault = false, bool EnableSSL = false, string SSLPort = "")
        {
            ApplicationPushNotificationSetting settings = new ApplicationPushNotificationSetting();
            PushNotificationDeliveryProviderSetting DPSettings = new PushNotificationDeliveryProviderSetting();
            SMTPDeliveryProviderConfiguration DPConfig = new SMTPDeliveryProviderConfiguration();
            AntiNoiseSetting ANS = new AntiNoiseSetting();

            settings.RetryAttempts = 2;
            settings.RetryIntervalMinutes = 2;
            settings.IsEnabled = true;

            DPConfig.SmtpFromAddress = FromEmail;
            DPConfig.SmtpFromDisplayName = FromName;
            DPConfig.SmtpPassword = SmtpPassword;
            DPConfig.SmtpServer = SmtpServer;
            DPConfig.SmtpUserName = SmtpUser;
            DPConfig.EnableSsl = EnableSSL;
            DPConfig.SmtpPort = Convert.ToInt32(SmtpPort);

            DPSettings.IsEnabled = true;
            DPSettings.ProviderAssemblyQualifiedName = "ASE.Notifications.SMTPDeliveryProvider, ASE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            settings.AntiNoiseSettings.ExcludeSubscriberEvents = true;
            settings.AntiNoiseSettings.InitialConsolidationDelayMinutes = 2;
            settings.AntiNoiseSettings.IsConsolidationEnabled = true;
            settings.AntiNoiseSettings.MaxConsolidationDelayMinutes = 10;

            DPSettings.ProviderConfigurationData = JObject.FromObject(DPConfig);
            settings.DeliveryProviderSettings = new List<PushNotificationDeliveryProviderSetting>();
            settings.DeliveryProviderSettings.Add(DPSettings);
            return settings.Serialized;
        }

        public static bool AddUserSmtpConfiguration(long? ProductId, string Title, string FromName, string FromEmail, string SmtpServer, long SmtpPort, string SmtpUser, string SmtpPassword, string SmtpDomain, string SmtpFooter, bool IsDefault, string PushNotificationSettingsJson, bool? IsPostmarkVerified, string PostMarkSignatureId, long? Id = 0, bool EnableSSL = false, string UserEmail = "", bool IsAWSBulkEmail = false, string AwsServiceURL = "", long? AwsBulkEmailCount = 0, string AwsAccessKeyId = "", string AwsSecretKey = "")
        {
            //
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    SmtpConfiguration SC = new SmtpConfiguration();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => SC.PushNotificationSettingsJson, PushNotificationSettingsJson);
                    Filter.AddSqlParameters(() => SC.ProductId, ProductId);
                    Filter.AddSqlParameters(() => SC.Title, Title);
                    Filter.AddSqlParameters(() => SC.FromName, FromName);
                    Filter.AddSqlParameters(() => SC.FromEmail, FromEmail);
                    Filter.AddSqlParameters(() => SC.SmtpServer, SmtpServer);
                    Filter.AddSqlParameters(() => SC.SmtpPort, SmtpPort);
                    Filter.AddSqlParameters(() => SC.SmtpUser, SmtpUser);
                    Filter.AddSqlParameters(() => SC.SmtpPassword, SmtpPassword);
                    Filter.AddSqlParameters(() => SC.SmtpDomain, SmtpDomain);
                    Filter.AddSqlParameters(() => SC.SmtpFooter, SmtpFooter);
                    Filter.AddSqlParameters(() => SC.IsDefault, IsDefault);
                    Filter.AddSqlParameters(() => SC.EnableSSL, EnableSSL);

                    Filter.AddSqlParameters(() => SC.IsAWSBulkEmail, IsAWSBulkEmail);
                    Filter.AddSqlParameters(() => SC.AwsServiceURL, AwsServiceURL);
                    Filter.AddSqlParameters(() => SC.AwsBulkEmailCount, AwsBulkEmailCount);
                    Filter.AddSqlParameters(() => SC.AwsAccessKeyId, AwsAccessKeyId);
                    Filter.AddSqlParameters(() => SC.AwsSecretKey, AwsSecretKey);

                    if (Id > 0)
                    {
                        Filter.AddSqlParameters(() => SC.Command, "Update");
                        Filter.AddSqlParameters(() => SC.RecUpdatedBy, UserEmail);
                        Filter.AddSqlParameters(() => SC.RecUpdatedDt, DateTimeOffset.Now);
                        Filter.AddSqlParameters(() => SC.Id, Id);
                    }
                    else
                    {
                        Filter.AddSqlParameters(() => SC.Command, "Insert");
                        Filter.AddSqlParameters(() => SC.RecCreatedBy, UserEmail);
                        Filter.AddSqlParameters(() => SC.RecCreatedDt, DateTimeOffset.Now);
                    }
                    IRepository<SmtpConfiguration> oRepository = new Repository<SmtpConfiguration>(uow.DataContext);

                    return oRepository.ExecuteSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ProductCustomer> lstProductCustomers(string Rule, string DBServer, string DBName, string DBUserName, string DBUserPass)
        {
            try
            {
                string ConnString = "Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";Integrated Security=False; User ID=" + DBUserName + "; password=" + DBUserPass + ";";
                using (IUnitOfWork uow = new UnitOfWork(ConnString, "Yes"))
                {
                    ProductCustomer PU = new ProductCustomer();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => PU.Rule, Rule);
                    IRepository<ProductCustomer> oRepository = new Repository<ProductCustomer>(uow.DataContext);
                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                Logger.LogRelativeMessage("lstProductCustomers Exception:: " + ex.Message, true);
                List<ProductCustomer> lstCustomers = new List<ProductCustomer>();
                return lstCustomers;
            }
        }

        public static List<ProductCustomer> lstMySQLProductCustomers(string Rule, string DBServer, string DBName, string DBUserName, string DBUserPass)
        {
            try
            {
                string ConnString = "Server=" + DBServer + ";" + " Database=" + DBName + ";" + " Uid=" + DBUserName + ";" + " Pwd=" + DBUserPass + ";";
                using (IUnitOfWork uow = new UnitOfWork(ConnString, "Yes"))
                {
                    ProductCustomer PU = new ProductCustomer();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => PU.Rule, Rule);
                    IRepository<ProductCustomer> oRepository = new Repository<ProductCustomer>(uow.DataContext);
                    return oRepository.LoadMySqlSP("sp_GetCustomersListForASE", Filter);
                }
            }
            catch (Exception ex)
            {
                Logger.LogRelativeMessage("lstMySQLProductCustomers Exception:: " + ex.Message, true);
                List<ProductCustomer> lstCustomers = new List<ProductCustomer>();
                return lstCustomers;
            }
        }

        public static List<ProductCustomer> GetDataFromMySQL(string Rule, string DBServer, string DBName, string DBUserName, string DBUserPass)
        {
            //string ConnString = "SERVER=" + DBServer + ";database=" + DBName + ";User ID=" + DBUserName + "; password=" + DBUserPass + ";";
            string ConnString = "Server=" + DBServer + ";port=3306;" + " Database=" + DBName + ";" + " Uid=" + DBUserName + ";" + " Pwd=" + DBUserPass + ";";
            Org.Utils.Logger.LogRelativeMessage(("MY SQL SERVER : " + ConnString), true);
            MySqlConnection connect = new MySqlConnection(ConnString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "sp_GetCustomersListForECB";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("@Rule", Rule));
            List<ProductCustomer> lstProductCustomer = new List<ProductCustomer>();
            try
            {
                cmd.Connection = connect;
                connect.Open();
                MySqlDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProductCustomer PC = new ProductCustomer();
                    PC.FirstName = dr["FirstName"].ToString();
                    PC.LastName = dr["LastName"].ToString();
                    PC.Email = dr["Email"].ToString();
                    //PC.IsActive =Convert.ToBoolean(dr["Email"]);
                    PC.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    lstProductCustomer.Add(PC);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Org.Utils.Logger.LogRelativeMessage(("GetDataFromMySQL Exception Message : " + ex.Message.ToString()), true);
                Org.Utils.Logger.LogRelativeMessage(("GetDataFromMySQL Exception : " + ex.ToString()), true);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
            return lstProductCustomer;
        }

    }
}