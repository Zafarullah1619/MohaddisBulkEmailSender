using Org.Utils;
using System;
using System.Data;

namespace Org.Business.Objects
{


     [DataTable("AspNetUsers")]
     [StoreProcedure("sp_GetUserInfoByID")]

    public class UserInformation : BaseEntity
    {
        public UserInformation()
        {
            IsPasswordUpdate = false;
            IsGenerateIPNSecreatKey = false;
        }

        [DataField(IsDBNull = false, Size = 256, Type = DbType.String)]
        public string AspNetUserId { get; set; }

        [DataField(IsDBNull = false, Size = 256, Type = DbType.String)]
        public string UserId { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string FirstName { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Int32)]
        public long CountryId { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string CountryName { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string LastName { get; set; }

        [DataField(IsDBNull = true,  Type = DbType.String)]
        public string IPNSecretKey { get; set; }

        [DataField(IsDBNull = true , Size =256, Type = DbType.String)]
        public string Email { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool EmailConfirmed { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string PhoneNumber { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool PhoneNumberConfirmed { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool TwoFactorEnabled { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string UserName { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string PasswordHash { get; set; }

        [NonDBField( Type = DbType.String)]
        public string NewPasswordHash { get; set; }

        [NonDBField(Type = DbType.String)]
        public string ConfirmPasswordHash { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool IsPasswordUpdate { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Boolean)]
        public bool IsGenerateIPNSecreatKey { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string CustomUserId { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool ShowProductSaleStats { get; set; }

    }


    [DataTable("AspNetUsers")]
    [StoreProcedure("sp_UpdateInsertUserInfo")]
    public class UserInformationInsert : UserInformation {

    }

    [StoreProcedure("sp_AddUserInformation")]
    public class AddUserInformation : BaseEntity
    {
        [NonDBField(Type = DbType.String)]
        public string UserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string FirstName { get; set; }

        [NonDBField(Type = DbType.String)]
        public string LastName { get; set; }

        [NonDBField(Type = DbType.Int64)]
        public long CountryId { get; set; }

        [NonDBField(Type = DbType.Decimal)]
        public decimal PayGenieFee { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool ShowProductSaleStats { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }

        [NonDBField(Type = DbType.String)]
        public string FlashFunnelSubDomain { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool IsPayGenieUser { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool IsFunnelActive { get; set; }

        [NonDBField(Type = DbType.String)]
        public string UserType { get; set; }
    }

    [StoreProcedure("sp_RegisterPGUser")]
    public class RegisterPGUser 
    {

        [NonDBField(Type = DbType.String)]
        public string Id { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Email { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool EmailConfirmed { get; set; }

        [NonDBField(Type = DbType.String)]
        public string PasswordHash { get; set; }

        [NonDBField(Type = DbType.String)]
        public string SecurityStamp { get; set; }

        [NonDBField(Type = DbType.String)]
        public string PhoneNumber { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool PhoneNumberConfirmed { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool TwoFactorEnabled { get; set; }

        [NonDBField( Type = DbType.DateTime)]
        public DateTime? LockoutEndDateUtc { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool LockoutEnabled { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public int AccessFailedCount { get; set; }

        [NonDBField(Type = DbType.String)]
        public string UserName { get; set; }

        [NonDBField(Type = DbType.String)]
        public string FirstName { get; set; }

        [NonDBField(Type = DbType.String)]
        public string LastName { get; set; }

        [NonDBField(Type = DbType.Int64)]
        public long CountryId { get; set; }
        
        [NonDBField(Type = DbType.Boolean)]
        public bool ShowProductSaleStats { get; set; }
        
        [NonDBField(Type = DbType.String)]
        public string FlashFunnelSubDomain { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool IsPayGenieUser { get; set; }
        
        [DataField(Type = DbType.String)]
        public string Response { get; set; }

    }

    [StoreProcedure("sp_GetAspNetUsers")]
    public class GetAspNetUsers
    {

        [DataField(Type = DbType.String)]
        public string Id { get; set; }

        [DataField(Type = DbType.String)]
        public string Email { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool EmailConfirmed { get; set; }

        [DataField(Type = DbType.String)]
        public string PasswordHash { get; set; }

        [DataField(Type = DbType.String)]
        public string SecurityStamp { get; set; }

        [DataField(Type = DbType.String)]
        public string PhoneNumber { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool PhoneNumberConfirmed { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool TwoFactorEnabled { get; set; }

        [DataField(IsDBNull =true, Type = DbType.DateTime)]
        public DateTime? LockoutEndDateUtc { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool LockoutEnabled { get; set; }

        [DataField(Type = DbType.Int32)]
        public int AccessFailedCount { get; set; }

        [DataField(Type = DbType.String)]
        public string UserName { get; set; }

        [DataField(Type = DbType.String)]
        public string FirstName { get; set; }

        [DataField(Type = DbType.String)]
        public string LastName { get; set; }

        [DataField(Type = DbType.Int64)]
        public long CountryId { get; set; }


    }

    [StoreProcedure("sp_UpdateProfileImage")]
    public class UpdateProfileImage
    {
        [NonDBField(Type = DbType.String)]
        public string CustomUserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string ProfileImage { get; set; }
    }


    [StoreProcedure("sp_UpdateHelpDeskName")]
    public class UpdateHelpDeskName
    {
        [NonDBField(Type = DbType.String)]
        public string UserId { get; set; }

        [NonDBField(Type = DbType.String)]
        public string HelpDeskName { get; set; }
    }

    [StoreProcedure("sp_GetHelpDeskNameCount")]
    public class HelpDeskNameCount
    {
        [DataField(Type = DbType.Int16)]
        public int Count { get; set; }

        [NonDBField(Type = DbType.String)]
        public string HelpDeskName { get; set; }
    }

    [StoreProcedure("sp_DeleteNewlyCreatedUser")]
    public class DeleteNewlyCreatedUser
    {
        [DataField(IsDBNull = true, Type = DbType.String)]
        public string UserId { get; set; }
    }


    [StoreProcedure("sp_GetUserProfileInfoByUserId")]
    public class UserProfileInfo : BaseEntity
    {
        [DataField(IsDBNull =true, Type = DbType.String)]
        public string FullName { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Int32)]
        public int AffiliatesCount { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Int32)]
        public int ProductCount { get; set; }

        [DataField(IsDBNull = true, Type = DbType.Int32)]
        public int ApporvedProductCount { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string AboutMe { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string UserId { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string CustomUserId { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string ProfileImage { get; set; }
        [DataField(Type = DbType.Boolean)]
        public bool IsPayGenieUser { get; set; }

        [DataField(Type = DbType.Int32)]
        public long UserTypeId { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool HasUpsellOne { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool HasUpsellTwo { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool HasUpsellThree { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool HasUpsellFour { get; set; }

        [DataField(Type = DbType.String)]
        public string LicCode { get; set; }

        [DataField(Type = DbType.Int16)]
        public long NoOfDomains { get; set; }

        [DataField(Type = DbType.Int16)]
        public long NoOfFunnels { get; set; }

        [DataField(Type = DbType.Int16)]
        public long NoOfPages { get; set; }

        [DataField(Type = DbType.Int16)]
        public long NoOfTemplates { get; set; }

        [DataField(Type = DbType.Int16)]
        public long NoOfVisitors { get; set; }

    }

    [StoreProcedure("sp_GetEmailByUserID")]
    public class UserEmailByUserId
    {
        [DataField(IsDBNull = true, Type = DbType.String)]
        public string Email { get; set; }

        [DataField(IsDBNull = true, Type = DbType.String)]
        public string UserId { get; set; }
    }


}
