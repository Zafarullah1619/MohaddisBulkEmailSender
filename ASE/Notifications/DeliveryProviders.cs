using Org.Business.Methods;
using Org.Business.Objects;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace ASE.Notifications
{
    public abstract class PushNotificationDeliveryProviderBase : IPushNotificationDeliveryProvider
    {

        public abstract string DestinationType { get; }

        public abstract Task<object> GenerateMessageAsync(PushNotificationItem notificationItem, CancellationToken ct);

        public abstract Task<bool> SendNotificationAsync(PushNotificationItem notificationItem, object message, CancellationToken ct);

        public abstract IDeliveryProviderConfiguration Configuration { get; set; }

        public async Task SendReadyMessageAsync(PushNotificationItem notificationItem, int retryMax, int retryIntv, CancellationToken ct)
        {
            try
            {
                //do the meat
                var message = await GenerateMessageAsync(notificationItem, ct);
                var result = await SendNotificationAsync(notificationItem, message, ct);
                //if we're in a retry case, increment retry count
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
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
    public abstract class EmailDeliveryProviderBase : PushNotificationDeliveryProviderBase
    {
        public override IDeliveryProviderConfiguration Configuration { get; set; }

        public override string DestinationType
        {
            get { return "email"; }
        }

        public override Task<object> GenerateMessageAsync(PushNotificationItem notificationItem, CancellationToken ct)
        {
            var memorydata = Convert.FromBase64String(notificationItem.MessageContent);
            using (var rs = new MemoryStream(memorydata))
            {
                var sf = new BinaryFormatter();
                return Task.FromResult(sf.Deserialize(rs));
            }
        }
    }
}
