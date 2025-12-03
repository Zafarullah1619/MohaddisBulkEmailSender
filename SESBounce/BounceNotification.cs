using Amazon.SQS.Model;
using Org.Business.Methods;
using Org.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESBounce
{
    public class BounceNotification
    {
        private static void ProcessQueuedBounce(ReceiveMessageResponse response)
        {
            int messages = response.Messages.Count;

            if (messages > 0)
            {
                foreach (var m in response.Messages)
                {
                    // First, convert the Amazon SNS message into a JSON object.
                    var notification = Newtonsoft.Json.JsonConvert.DeserializeObject<AmazonSqsNotification>(m.Body);

                    // Now access the Amazon SES bounce notification.
                    var bounce = Newtonsoft.Json.JsonConvert.DeserializeObject<AmazonSesBounceNotification>(notification.Message);

                    if(bounce!=null)
                    {
                        foreach (var recipient in bounce.Bounce.BouncedRecipients)
                        {
                            Logger.LogRelativeMessage("ProcessQueuedBounce recipient.EmailAddress:: " + recipient.EmailAddress, true);
                            PushNotificationsBAL.MarkAsBounced(recipient.EmailAddress);
                        }
                    }
                }
            }
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
