using Org.Utils;
using System;
using System.Data;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetNotifications")]
    public class PushNotificationItem : BaseEntity
    {
        [DataField(Type = DbType.String)]
        public string ContentSourceType { get; set; }

        [DataField(Type = DbType.String)]
        public string UserId { get; set; }

        [DataField(Type = DbType.Int64)]
        public long DestinationId { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset CreatedDate { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? ScheduledSendDate { get; set; }

        [DataField(Type = DbType.Int32)]
        public int DeliveryStatus { get; set; }

        [DataField(Type = DbType.Int32)]
        public int RetryCount { get; set; }

        [DataField(Type = DbType.String)]
        public string MessageContent { get; set; }

        [DataField(Type = DbType.String)]
        public string FromName { get; set; }

        [DataField(Type = DbType.String)]
        public string FromEmail { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string DestinationAddress
        {
            get
            {
                if (Destination == null)
                {
                    return null;
                }
                return Destination.DestinationAddress;
            }
            set
            {
                if (Destination == null)
                {
                    Destination = new PushNotificationDestination();
                }
                Destination.DestinationAddress = value;
            }
        }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string DestinationType
        {
            get
            {
                if (Destination == null)
                {
                    return null;
                }
                return Destination.DestinationType;
            }
            set
            {
                if (Destination == null)
                {
                    Destination = new PushNotificationDestination();
                }
                Destination.DestinationType = value;
            }
        }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string DestinationSettings
        {
            get
            {
                if (Destination == null)
                {
                    return null;
                }
                return Destination.DestinationSettings;
            }
            set
            {
                if (Destination == null)
                {
                    Destination = new PushNotificationDestination();
                }
                Destination.DestinationSettings = value;
            }
        }

        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool IsDestinationEnabled
        {
            get
            {
                if (Destination == null)
                {
                    return false;
                }
                return Destination.IsEnabled;
            }
            set
            {
                if (Destination == null)
                {
                    Destination = new PushNotificationDestination();
                }
                Destination.IsEnabled = value;
            }
        }

        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool IsDestinationDefault
        {
            get
            {
                if (Destination == null)
                {
                    return false;
                }
                return Destination.IsDefault;
            }
            set
            {
                if (Destination == null)
                {
                    Destination = new PushNotificationDestination();
                }
                Destination.IsDefault = value;
            }
        }

        [NonDBField(Type = DbType.String)]
        public string Rule { get; set; }

        [NonDBField]
        public PushNotificationDestination Destination { get; set; }

        [NonDBField(Type = DbType.String)]
        public string SessionUserId { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public long? ProductId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string ToEmail { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public long? StepId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string PushNotificationSettingsJson { get; set; }
        
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
        public Nullable<System.DateTimeOffset> RecCreatedDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecUpdatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public Nullable<System.DateTimeOffset> RecUpdatedDt { get; set; }

        [DataField(Type = DbType.Int32)]
        public long? BadgeId { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public long? BadgeLimit { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public long? BadgeInterval { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public long? PerHourLimit { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public long? PerDayLimit { get; set; }

        internal DateTimeOffset? GetSendDate(ApplicationPushNotificationSetting appSettings, PushNotificationDestination userSettings)
        {
            var send = ScheduledSendDate;//we'll leave this alone if consolidation isn't used

            if (userSettings.IsEnabled && ScheduledSendDate.HasValue)
            {
                var now = DateTime.Now;

                var currentDelayMinutes = (now - CreatedDate).Minutes;

                var maxDelayMinutes = appSettings.AntiNoiseSettings.MaxConsolidationDelayMinutes;

                var delay = appSettings.AntiNoiseSettings.InitialConsolidationDelayMinutes;

                //ensure we don't bump thing over the max wait
                if (delay + currentDelayMinutes > maxDelayMinutes)
                {
                    delay = maxDelayMinutes - currentDelayMinutes;
                }

                send = now.AddMinutes(delay);
            }
            return send;
        }
    }

    [StoreProcedure("sp_GetNotificationBadges")]
    public class NotificationsBadge : BaseEntity
    {
        [DataField(Type = DbType.Int32)]
        public long Limit { get; set; }

        [DataField(Type = DbType.Int32)]
        public long Interval { get; set; }

        [DataField(Type = DbType.String)]
        public string RecCreatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public Nullable<System.DateTimeOffset> RecCreatedDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecUpdatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public Nullable<System.DateTimeOffset> RecUpdatedDt { get; set; }
    }
}
