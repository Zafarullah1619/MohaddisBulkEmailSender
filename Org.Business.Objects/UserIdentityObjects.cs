using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Utils;
using System.Data;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetUserInfo")]
    public class UserInfo
    {
        [DataField(Type = DbType.String)]
        public string SessionUserId { get; set; }
        [DataField(Type = DbType.String)]
        public string FullName { get; set; }

        [NonDBField(Type = DbType.String)]
        public string SessionAdminUserId { get; set; }
        [DataField(Type = DbType.String)]
        public string UserId { get; set; }
        [DataField(Type = DbType.String)]
        public string RoleId { get; set; }
        [DataField(Type = DbType.String)]
        public string RoleName { get; set; }
        [DataField(Type = DbType.String)]
        public string MainUserId { get; set; }
        [DataField(Type = DbType.String)]
        public string UserEmail { get; set; }
        [DataField(Type = DbType.String)]
        public string CustomUserId { get; set; }
        [DataField(Type = DbType.String)]
        public string CustomSessionUserId { get; set; }
        [DataField(Type = DbType.String)]
        public string ModulesAssigned { get; set; }
        [DataField(Type = DbType.String)]
        public string RightsAssigned { get; set; }

        [NonDBField(Type = DbType.String)]
        public string SessionID { get; set; }

        [NonDBField(Type = DbType.String)]
        public string WebsiteID { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool AddSession { get; set; }

        [NonDBField(Type = DbType.String)]
        public string LoginAttemptId { get; set; }
        

        
    }

    [StoreProcedure("sp_AddSubUser")]
    public class AddSubUser
    {
        [DataField(Type = DbType.Int32)]
        public int RETURNVAL { get; set; }

        [NonDBField(Size = 512, Type = DbType.String)]
        public string NewSubUserId { get; set; }

        [NonDBField(Size = 512, Type = DbType.String)]
        public string CurrentUserId { get; set; }

        [NonDBField(Size = 512, Type = DbType.String)]
        public string MainUserId { get; set; }

        [NonDBField(Size = 512, Type = DbType.String)]
        public string CurrentRoleId { get; set; }
        [NonDBField(Size = 512, Type = DbType.String)]
        public string SessionAdminUserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string AreaRights { get; set; }
    }

    [DataTable("AspNetRoles")]
    [StoreProcedure("sp_GetRolesList")]
    public class UserRoles
    {
        [KeyField(Size = 512, Type = DbType.String)]
        public string Id { get; set; }
        [DataField(IsDBNull = false, Size = 512, Type = DbType.String)]
        public string Name { get; set; }
    }

    [DataTable("AspNetUsers")]
    [StoreProcedure("sp_GetUserList")]
    public class Users
    {
        [KeyField(Size = 512, Type = DbType.String)]
        public string Id { get; set; }

        [DataField(Size = 512, Type = DbType.String)]
        public string Email { get; set; }

        [DataField(IsDBNull = false, Type = DbType.Boolean)]
        public bool EmailConfirmed { get; set; }

        [DataField(Type = DbType.String)]
        public string PasswordHash { get; set; }

        [DataField(Type = DbType.String)]
        public string SecurityStamp { get; set; }

        [DataField(Type = DbType.String)]
        public string PhoneNumber { get; set; }

        [DataField(IsDBNull = false, Type = DbType.Boolean)]
        public bool PhoneNumberConfirmed { get; set; }

        [DataField(IsDBNull = false, Type = DbType.Boolean)]
        public bool TwoFactorEnabled { get; set; }

        [DataField(Type = DbType.DateTime)]
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }

        [DataField(IsDBNull = false, Type = DbType.Boolean)]
        public bool LockoutEnabled { get; set; }

        [DataField(IsDBNull = false, Type = DbType.Int32)]
        public int AccessFailedCount { get; set; }

        [DataField(IsDBNull = false, Size = 512, Type = DbType.String)]
        public string UserName { get; set; }
    }

    [StoreProcedure("sp_GetLoginUser")]
    public class LoginUser : Users
    {
        [DataField(Type = DbType.String)]
        public string LoginAttemptId { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool RememberMe { get; set; }

        [DataField(Type = DbType.String)]
        public string UserIPAddress { get; set; }

        [DataField(Type = DbType.String)]
        public string ReturnUrl { get; set; }

        [DataField(Type = DbType.String)]
        public string RequestObject { get; set; }

        [DataField(Type = DbType.String)]
        public string AcceptTypes { get; set; }
        [DataField(Type = DbType.String)]
        public string AnonymousID { get; set; }
        [DataField(Type = DbType.String)]
        public string AppRelativeCurrentExecutionFilePath { get; set; }
        [DataField(Type = DbType.String)]
        public string ApplicationPath { get; set; }
        [DataField(Type = DbType.String)]
        public string Browser { get; set; }
        [DataField(Type = DbType.String)]
        public string ClientCertificate { get; set; }
        [DataField(Type = DbType.String)]
        public string ContentEncoding { get; set; }
        [DataField(Type = DbType.Int32)]
        public int ContentLength { get; set; }
        [DataField(Type = DbType.String)]
        public string ContentType { get; set; }
        [DataField(Type = DbType.String)]
        public string Cookies { get; set; }
        [DataField(Type = DbType.String)]
        public string CurrentExecutionFilePath { get; set; }
        [DataField(Type = DbType.String)]
        public string CurrentExecutionFilePathExtension { get; set; }
        [DataField(Type = DbType.String)]
        public string FilePath { get; set; }
        [DataField(Type = DbType.String)]
        public string Files { get; set; }
        [DataField(Type = DbType.String)]
        public string Form { get; set; }
        [DataField(Type = DbType.String)]
        public string Headers { get; set; }
        [DataField(Type = DbType.String)]
        public string HttpChannelBinding { get; set; }
        [DataField(Type = DbType.String)]
        public string HttpMethod { get; set; }
        [DataField(Type = DbType.Boolean)]
        public bool IsAuthenticated { get; set; }
        [DataField(Type = DbType.Boolean)]
        public bool IsLocal { get; set; }
        [DataField(Type = DbType.Boolean)]
        public bool IsSecureConnection { get; set; }
        [DataField(Type = DbType.String)]
        public string LogonUserIdentity { get; set; }
        [DataField(Type = DbType.String)]
        public string Params { get; set; }
        [DataField(Type = DbType.String)]
        public string Path { get; set; }
        [DataField(Type = DbType.String)]
        public string PathInfo { get; set; }
        [DataField(Type = DbType.String)]
        public string PhysicalApplicationPath { get; set; }
        [DataField(Type = DbType.String)]
        public string PhysicalPath { get; set; }
        [DataField(Type = DbType.String)]
        public string QueryString { get; set; }
        [DataField(Type = DbType.String)]
        public string RawUrl { get; set; }
        [DataField(Type = DbType.String)]
        public string ReadEntityBodyMode { get; set; }
        [DataField(Type = DbType.String)]
        public string RequestType { get; set; }
        [DataField(Type = DbType.String)]
        public string ServerVariables { get; set; }
        [DataField(Type = DbType.String)]
        public string TimedOutToken { get; set; }
        [DataField(Type = DbType.String)]
        public string TlsTokenBindingInfo { get; set; }
        [DataField(Type = DbType.Int32)]
        public int TotalBytes { get; set; }
        [DataField(Type = DbType.String)]
        public string Unvalidated { get; set; }
        [DataField(Type = DbType.String)]
        public string Url { get; set; }
        [DataField(Type = DbType.String)]
        public string UrlReferrer { get; set; }
        [DataField(Type = DbType.String)]
        public string UserAgent { get; set; }
        [DataField(Type = DbType.String)]
        public string UserHostAddress { get; set; }
        [DataField(Type = DbType.String)]
        public string UserHostName { get; set; }
        [DataField(Type = DbType.String)]
        public string UserLanguages { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsLoginSucceed { get; set; }

        [DataField(Type = DbType.String)]
        public string FirstName { get; set; }

        [DataField(Type = DbType.String)]
        public string LastName { get; set; }

        [DataField(Type = DbType.String)]
        public string AboutMe { get; set; }

        [DataField(Type = DbType.String)]
        public string IPNSecretKey { get; set; }

        [DataField(Type = DbType.String)]
        public string CustomUserId { get; set; }

        [DataField(Type = DbType.String)]
        public string CountryId { get; set; }

        [DataField(Type = DbType.String)]
        public string FullName { get; set; }

        [DataField(Type = DbType.String)]
        public string ProfileImage { get; set; }

        [DataField(Type = DbType.String)]
        public string HelpDeskName { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsFunnelActive { get; set; }

        [DataField(Type = DbType.String)]
        public string FlashFunnelSubDomain { get; set; }

        [DataField(Type = DbType.String)]
        public string UserRoleName { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }
    }

    [StoreProcedure("sp_HasPermissions")]
    public class HasPermissions
    {
        [DataField(Type = DbType.Int32)]
        public int HasAreaPermissions { get; set; }

        [DataField(Type = DbType.String)]
        public string ModulesAssigned { get; set; }

        [DataField(Type = DbType.String)]
        public string RightsAssigned { get; set; }

        [NonDBField(Size = 512, Type = DbType.String)]
        public string UserId { get; set; }

        [NonDBField(Size = 512, Type = DbType.String)]
        public string MainUserId { get; set; }

        [NonDBField(Size = 512, Type = DbType.String)]
        public string RoleId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string AreaRuleCode { get; set; }
    }

    [StoreProcedure("sp_GetCRMSenderEmail")]
    public class CRMSenderEmail
    {
        [DataField(Type = DbType.Boolean)]
        public bool IsPostmarkVerified { get; set; }

        [DataField(Type = DbType.String)]
        public string FromEmail { get; set; }

        [DataField(Type = DbType.String)]
        public string UserId { get; set; }

        [DataField(Type = DbType.String)]
        public string FromName { get; set; }

        [DataField(Type = DbType.String)]
        public string PostMarkSignatureId { get; set; }

        [DataField(Type = DbType.String)]
        public string PushNotificationSettingsJson { get; set; }
        
        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public long? SurveyId { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public long? BookingPageId { get; set; }
        
        [NonDBField(Type = DbType.String)]
        public string CustomUserId { get; set; }
    }

    [StoreProcedure("sp_GetVendorList")]
    public class Vendors
    {
        [DataField(Type = DbType.String)]
        public string UserId { get; set; }

        [DataField(Size = 512, Type = DbType.String)]
        public string Email { get; set; }

        [DataField(Type = DbType.String)]
        public string FullName { get; set; }

        [DataField(Type = DbType.String)]
        public string CustomUserId { get; set; }
    }

    [StoreProcedure("sp_CheckPublicDomain")]
    public class PublDomain:BaseEntity
    {
        [DataField(Type = DbType.String)]
        public string DomainName { get; set; }

        [DataField(Type = DbType.String)]
        public string FromEmail { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsPublicEmail { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsPublicHost { get; set; }

    }
}
