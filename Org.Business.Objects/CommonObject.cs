using System;
using System.Collections.Generic;
using Org.Utils;
using System.Data;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Newtonsoft.Json;

namespace Org.Business.Objects
{
    
    public static class PaymentMethods
    {
        public const string PAYPAL = "PAYPAL";
        public const string STRIPE = "STRIPE";
        public const string AUTHORIZENET = "AUTHORIZENET";
        public const string BRAINTREE = "BRAINTREE";

        public const long PAYPALID = 1;
        public const long STRIPEID = 2;
        public const long AUTHORIZENETID = 3;
        public const long BRAINTREEID = 4;
    }

    public static class AutoResponders
    {
        public const long GetResponse = 1;
        public const long SendLane = 2;
        public const long MailChimp = 3;
        public const long AWeber = 4;        
    }

    public static class FunnelTemplateTypes
    {
        public const int Autowebinar = 1;
        public const int Membership = 3;
        public const int Misc = 4;
        public const int Optin = 5;
        public const int Sales = 6;
        public const int Webinar = 7;
        public const int AutowebinarRegistration = 8;
        public const int Replayroom = 9;
        public const int AutowebinarThankyou = 10;
        public const int Clickpop = 11;
        public const int Memberaccess = 12;
        public const int Membershiparea = 13;
        public const int Miscellaneous = 14;
        public const int Emailoptin = 15;
        public const int OptinThankyou = 16;
        public const int Oneclickdownsell = 17;
        public const int Oneclickupsell = 18;
        public const int Orderconfirmation = 19;
        public const int Orderform = 20;
        public const int Productlaunch = 21;
        public const int Salespage = 22;
        public const int Broadcastroom = 23;
        public const int WebinarRegistration = 24;
        public const int WebinarThankyou = 25;
        public const int Membershiparealessons = 26;

    }
    public static class PushNotificationItemStatus
    {
        public const int Scheduled = 0;
        public const int Sent = 1;
        public const int Retrying = 2;
        public const int Failed = 3;
        public const int Canceled = 4;
        public const int Disabled = 5;
        public const int NotAvailable = 6;
    }

    public static class NotificationEventTypes
    {
        public const int AddedInBlackList = 1;
        public const int RemovedFromBlackList = 2;
        public const int MemberStripeUpdated = 3;
        public const int MemberPayPalUpdated = 4;
        public const int MemberPasswordChange = 5;
        public const int MemberPhoneUpdated = 6;
        public const int MemberIPNKeyUpdated = 7;
        public const int MemberFlashFunnelAdded = 8;
        public const int MemberHelpdeskAdded = 9;
        public const int MemberSubUserAdded = 10;
        public const int MemberAddedAsSubUser = 11;
        public const int VendorProductApproved = 12;
        public const int VendorProductFeatured = 13;
        public const int AffiliateChargeback = 14;
        public const int VendorChargeback = 15;
        public const int AffiliateRefundOrder = 16;
        public const int CustomerRefundOrder = 17;
        public const int VendorRefundOrder = 18;
        public const int AffiliateNewSale = 19;
        public const int CustomerOrderReceipt = 20;
        public const int VendorNewSale = 21;
        public const int MemberSignupConfirmation = 22;
        public const int VendorNewAffiliateRequest = 23;
        public const int AffiliateRequestApproved = 24;
        public const int AffiliateRequestDeclined = 25;
        public const int MemberWelcomeEmail = 26;
        public const int AffiliateTransactionFailure = 27;
        public const int CustomerPrebillNotification = 28;
        public const int CustomerPaymentFailure = 29;
        public const int VendorTransactionFailure = 30;
        public const int VendorDelayedTransactionPaid = 31;
        public const int AffiliateDelayedTransactionPaid = 32;
        public const int VendorAffiliateBlocked = 33;
        public const int VendorAffiliateUnblocked = 34;
        public const int VendorAffiliateAutoapprovedAdded = 35;
        public const int VendorAffiliateAutoapprovedRemoved = 36;
        public const int MemberContactRemoved = 37;
        public const int MemberContactsDownloaded = 38;
        public const int MemberAppointmentAdded = 39;
        public const int MemberAppointmentUpdated = 40;
        public const int MemberAppointmentRemoved = 41;
        public const int MemberTaskAdded = 42;
        public const int MemberTaskUpdated = 43;
        public const int MemberTaskRemoved = 44;
        public const int MemberSurveyFilled = 45;
        public const int MemberSurveyUpdated = 46;
        public const int MemberBookingPageUpdated = 47;
    }

    public static class NotificationEventTemplateCodes
    {
        public const string AffiliateNewSale = "AffiliateNewSale";
        public const string AffiliateTransactionFailure = "AffiliateTransactionFailure";
        public const string AffiliateRefundOrder = "AffiliateRefundOrder";


        public const string VendorNewSale = "VendorNewSale";
        public const string VendorRefundOrder = "VendorRefundOrder";
        public const string VendorTransactionFailure = "VendorTransactionFailure";

        public const string CustomerRefundOrder = "CustomerRefundOrder";
        public const string CustomerOrderReceipt = "CustomerOrderReceipt";
        public const string CustomerPaymentFailure = "CustomerPaymentFailure";
        public const string AffiliateBonus = "AffiliateBonus";
        public const string CustomerTrailExpiry = "CustomerTrailExpiry";
        public const string CustomerUpdateCard = "CustomerUpdateCard";
        public const string CustomerCardExpiring = "CustomerCardExpiring";
        public const string Customer2ndNotice = "Customer2ndNotice";
        public const string Customer3rdNotice = "Customer3rdNotice";

        public const string MemberAccountApproved = "MemberAccountApproved";
        public const string MemberAccountUpdateConfirmation = "MemberAccountUpdateConfirmation";
        public const string MemberActivationFeePaid = "MemberActivationFeePaid";
        public const string VendorProductApproved = "VendorProductApproved";
        public const string AffiliateChargeback = "AffiliateChargeback";
        public const string VendorChargeback = "VendorChargeback";
        public const string MemberPasswordChange = "MemberPasswordChange";
        public const string MemberPaymentInfoUpdateConfirmation = "MemberPaymentInfoUpdateConfirmation";
        public const string MemberSignupConfirmation = "MemberSignupConfirmation";
        public const string SupportTicketAcknowledgement = "SupportTicketAcknowledgement";
        public const string SupportTicketReply = "SupportTicketReply";
        public const string VendorProductApprove = "VendorProductApprove";
        public const string MemberWelcomeEmail = "MemberWelcomeEmail";
        public const string CustomerPrebillNotification = "CustomerPrebillNotification";
        public const string AddedInBlackList = "AddedInBlackList";
        public const string RemovedFromBlackList = "RemovedFromBlackList";
        public const string CustomerPayPayPaymentFailure = "CustomerPayPayPaymentFailure";
        
    }
    public static class SurveyCountDownTypes
    {
        public const string DateCountDown = "DateCountDown";
        public const string TimeEverGreen = "TimeEverGreen";
        public const string DailyEverGreen = "DailyEverGreen";
    }

    public class GoogleReCaptchaRequest
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("remoteip")]
        public string RemoteIP { get; set; }
    }

    public class GoogleReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTimeOffset ChallengeTs { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public string[] ErrorCodes {get;set;}
    }

    public class IgnoreErrorPropertiesResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            //string[] Properties = {/* "AcceptTypes", "AnonymousID", "AppRelativeCurrentExecutionFilePath", "ApplicationPath", "Browser", "ClientCertificate", "ContentEncoding", "ContentLength", "ContentType", "Cookies", "CurrentExecutionFilePath", "CurrentExecutionFilePathExtension", "FilePath", "Files", */"Filter",/* "Form", "Headers", "HttpChannelBinding", "HttpMethod",*/ "InputStream", "IsAuthenticated", "IsLocal", "IsSecureConnection", "LogonUserIdentity", "Params", "Path", "PathInfo", "PhysicalApplicationPath", "PhysicalPath", "QueryString", "RawUrl", "ReadEntityBodyMode", "RequestContext", "RequestType", "ServerVariables", "TimedOutToken", "TlsTokenBindingInfo", "TotalBytes", "Unvalidated", "Url", "UrlReferrer", "UserAgent", "UserHostAddress", "UserHostName", "UserLanguages"};
            string[] IgnoredProperties ={
                                            "Filter",
                                            "InputStream",
                                            "RequestContext"
                                        };
            if (new List<string>(IgnoredProperties).Contains(property.PropertyName)) {
                property.Ignored = true;
            }
            return property;
        }
    }

    public static class CommonStatus
    {
        public static class InsertStatus
        {
            public const int AllreadyEsist = -1;
            public const int Insert = 1;
            public const int NotInsert = 0;

        }

        public static class BroadCastReceiverTypes
        {
            public const int AllContacts = 1;
            public const int ContactsInCampaign = 2;
            public const int ContactsWithTag = 3;

        }

        public static class ReceiverTypesSelectionCriteria
        {
            public const string All = "all";
            public const string Any = "any";
            public const string None = "none";

        }

        public static class SocialProfileTypes
        {
            
            public const int PrimaryWebsiteId = 1;
            public const int PrimaryFacebookLinkId = 2;
            public const int PrimaryTwiterLinkId = 3;
            public const int PrimaryLinkedinLinkId = 4;
        }


        public static class RefundPayPalStatus
        {
            private static Dictionary<string, string> _Status = new Dictionary<string, string>();
            public static Dictionary<string, string> Status
            {
                get
                {
                    if(_Status.ContainsKey("REFUNDED") == false)
                    {
                        _Status.Add("REFUNDED", "Refund successfully completed.");
                    }

                    if (_Status.ContainsKey("REFUNDED_PENDING") == false)
                    {
                        _Status.Add("REFUNDED_PENDING", "Refund awaiting transfer of funds; for example, a refund paid by eCheck.");
                    }

                    if (_Status.ContainsKey("NOT_PAID") == false)
                    {
                        _Status.Add("NOT_PAID", "Payment was never made; therefore, it cannot be refunded.");
                    }

                    if (_Status.ContainsKey("ALREADY_REVERSED_OR_REFUNDED") == false)
                    {
                        _Status.Add("ALREADY_REVERSED_OR_REFUNDED", "Request rejected because the refund was already made, or the payment was reversed prior to this request.");
                    }

                    if (_Status.ContainsKey("NO_API_ACCESS_TO_RECEIVER") == false)
                    {
                        _Status.Add("NO_API_ACCESS_TO_RECEIVER", "Request cannot be completed because you do not have third-party access from the receiver to make the refund.");
                    }

                    if (_Status.ContainsKey("REFUND_NOT_ALLOWED") == false)
                    {
                        _Status.Add("REFUND_NOT_ALLOWED", "Refund is not allowed.");
                    }

                    if (_Status.ContainsKey("INSUFFICIENT_BALANCE") == false)
                    {
                        _Status.Add("INSUFFICIENT_BALANCE", "Request rejected because the receiver from which the refund is to be paid does not have sufficient funds or the funding source cannot be used to make a refund.");
                    }

                    if (_Status.ContainsKey("AMOUNT_EXCEEDS_REFUNDABLE") == false)
                    {
                        _Status.Add("AMOUNT_EXCEEDS_REFUNDABLE", "Request rejected because you attempted to refund more than the remaining amount of the payment; call the PaymentDetails API operation to determine the amount already refunded.");
                    }

                    if (_Status.ContainsKey("PREVIOUS_REFUND_PENDING") == false)
                    {
                        _Status.Add("PREVIOUS_REFUND_PENDING", "Request rejected because a refund is currently pending for this part of the payment.");
                    }

                    if (_Status.ContainsKey("NOT_PROCESSED") == false)
                    {
                        _Status.Add("NOT_PROCESSED", "Request rejected because it cannot be processed at this time.");
                    }

                    if (_Status.ContainsKey("REFUND_ERROR") == false)
                    {
                        _Status.Add("REFUND_ERROR", "Request rejected because of an internal error.");
                    }

                    if (_Status.ContainsKey("PREVIOUS_REFUND_ERROR") == false)
                    {
                        _Status.Add("PREVIOUS_REFUND_ERROR", "Request rejected because another part of this refund caused an internal error.");
                    }

                    return _Status;
                }
            }

        }
      
        public static class ProductStatus
        {
            public const int PendingApproval = 1;
            public const int Denied = 2;
            public const int Approved = 3;
            public const int InActive = 4;
        }

        public static class AffiliateStatus
        {
            public const int All = 1;
            public const int Instant = 2;
            public const int Delayed = 3;
            public const int Denied = 4;
            public const int Block = 5;
        }

        public static class GirdPageSettings
        {
            public const long PageNumber = 1;
            public const long PageSize = 10;
            public const long OffSet = 0;
            public const int SortColumn = 1;
            public const string SortOrder = "asc";

            public static readonly int[] PageNumberList = { 5, 10, 15, 25, 50, 100, 250 };
        }
        public static class RemdomStringLength
        {
            public const int LengthSmall = 16;
            public const int LengthMedium = 32;
            public const int LengthLarge = 64;
        }
            public static class ContractsStatus
        {
            public const bool DeActive = false;
            public const bool Active = true;
        }


        public static class AffiliateRequestStatus
        {
            public const int Pending = 1;
            public const int Approved = 2;
            public const int Declined = 3;
        }

        public static class AffiliateApprovalType
        {
            public const int Manual = 1;
            public const int AutoApproved = 2;
            public const int NoAffiliate = 3;
        }

        public static class DateTimeFormat
        {
            public const string SimpleDateTimeOffSetFormat = "dd-MMM-yyyy HH:mm:ss zzz";
            public const string SimpleDateTimeFormat = "dd-MMM-yyyy hh:mm:ss tt";
            public const string SimpleDateTimeNoSecFormat = "dd-MMM-yyyy hh:mm tt";
            public const string JqueryDateTimeFormat = "dd-MM-yyyy h:mm:ss tt";

            public const string SimpleDateFormat = "dd-MMM-yyyy";
            public const string JqueryDateFormat = "dd-MM-yyyy";
            public const string MomentDateFormat = "DD-MMM-YYYY Z";
            public const string MomentDateFormatWithOutTimeZone = "DD-MMM-YYYY";
            public const string MomentDateTimeFormatWithOutTimeZone = "DD-MMM-YYYY hh:mm:ss A";
            public const string RangeDateFormat = "dd-M-yyyy";

            public const string MomentDateTimeFormat = "DD-MMM-YYYY hh:mm:ss A Z";
            public const string MomentDateTimeNoSecFormat = "DD-MMM-YYYY hh:mm A Z";
            public const string MomentDateTime24HourFormat = "DD-MMM-YYYY HH:mm:ss Z";
            public const string SetFieldDateTimeFormat = "yyyy-MM-dd hh:mm tt";
        }

        public static class ContractPageType
        {
            public const string Index = "Index";
            public const string HideContractIndex = "HideContractIndex";
            public const string HideContractPartner = "HideContractPartner";



        }


        public static class QuickDates
        {
            public const int Today = 1;
            public const int Yesterday = 2;
            public const int LastSevenDays = 7;
            public const int LastThirtyDays = 30;
            public const int LastSixtyDays = 60;
            public const int LastNintyDays = 90;
            public const int ThisMonth = 31;
            public const int LastMonth = 62;
            public const int AllTime = 9999;
        }

    }


    [StoreProcedure("sp_GetCountries")]
    public class Country
    {
        [DataField(Type = DbType.Int64)]
        public long Id { get; set; }

        [DataField(Type = DbType.String)]
        public string Name { get; set; }

        [DataField(Type = DbType.String)]
        public string Abbreviation { get; set; }

        [DataField(Type = DbType.String)]
        public string ShortName { get; set; }

        [DataField( IsDBNull = true,  Size = 512, Type = System.Data.DbType.Boolean)]
        public Boolean IsActive { get; set; }

        [DataField(Type = DbType.String)]
        public string PhoneCode { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool IsSelected { get; set; }
    }

    [StoreProcedure("sp_GetMax")]
    public class MaxValue
    {
        [DataField(Type = System.Data.DbType.Int64)]
        public long MaxVal { get; set; }
        [NonDBField(Type = System.Data.DbType.String)]
        public string ColumnName { get; set; }
        [NonDBField(Size = 512, Type = System.Data.DbType.String)]
        public string TableName { get; set; }
    }

    public class PageOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class SearchBox
    {
        public bool regex { get; set; }

        public string value { get; set; }
    }

    public class DataTablesParam
    {
        public long start { get; set; }
        public long length { get; set; }
        public int iColumns { get; set; }
        public SearchBox search { get; set; }
        public bool bEscapeRegex { get; set; }
        public int iSortingCols { get; set; }
        public int draw { get; set; }
        public List<string> sColumnNames { get; set; }
        public List<bool> bSortable { get; set; }
        public List<bool> bSearchable { get; set; }
        public List<string> sSearchValues { get; set; }
        public List<PageOrder> order { get; set; }
        public List<bool> bEscapeRegexColumns { get; set; }

        public DataTablesParam()
        {
            sColumnNames = new List<string>();
            bSortable = new List<bool>();
            bSearchable = new List<bool>();
            sSearchValues = new List<string>();
            bEscapeRegexColumns = new List<bool>();
        }

        public DataTablesParam(int iColumns)
        {
            this.iColumns = iColumns;
            sColumnNames = new List<string>(iColumns);
            bSortable = new List<bool>(iColumns);
            bSearchable = new List<bool>(iColumns);
            sSearchValues = new List<string>(iColumns);
            bEscapeRegexColumns = new List<bool>(iColumns);
        }
    }

    public class DataTablesResponseData
    {
        public long recordsTotal { get; set; }
        public long recordsFiltered { get; set; }
        public int draw { get; set; }
        public object[] aaData { get; set; }
    }

}
