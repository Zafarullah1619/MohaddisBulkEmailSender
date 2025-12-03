using System;
using System.Collections.Generic;
using System.Linq;
using Org.Utils;
using Org.Business.Objects;
using Org.DataAccess;

namespace Org.Business.Methods
{
    public class UsersProfileBAL
    {
        public UsersProfileBAL()
        {
        }


        

        public static long AddUserInformation(string UserId, string FirstName, string LastName, long CountryId, bool IsFlashFunnelUser = false, string FlashFunnelSubDomain = null, string ConnectionStringName = "DefaultConnection", string UserType = "BASIC")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    List<AddUserInformation> lUserInformation;
                    AddUserInformation p = new AddUserInformation();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.UserId, UserId);
                    Filter.AddSqlParameters(() => p.FirstName, FirstName);
                    Filter.AddSqlParameters(() => p.LastName, LastName);
                    Filter.AddSqlParameters(() => p.CountryId, CountryId);
                    Filter.AddSqlParameters(() => p.ShowProductSaleStats, 1);
                    if (IsFlashFunnelUser)
                    {
                        Filter.AddSqlParameters(() => p.FlashFunnelSubDomain, FlashFunnelSubDomain);
                        Filter.AddSqlParameters(() => p.IsPayGenieUser, false);
                        Filter.AddSqlParameters(() => p.IsFunnelActive, true);
                        Filter.AddSqlParameters(() => p.UserType, UserType);
                    }

                    IRepository<AddUserInformation> oRepository = new Repository<AddUserInformation>(uow.DataContext);
                    lUserInformation = oRepository.LoadSP(Filter);
                    if (lUserInformation.Count > 0)
                    {
                        return lUserInformation[0].Id;
                    }
                    return -2;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RegisterPaygenieUserInFF(string UserId, string Email, bool EmailConfirmed, string PasswordHash, string SecurityStamp, string PhoneNumber, bool PhoneNumberConfirmed, bool TwoFactorEnabled, DateTime? LockoutEndDateUtc, bool LockoutEnabled, int AccessFailedCount, string UserName, string FirstName, string LastName, long CountryId, string FlashFunnelSubDomain, string ConnectionStringName = "FunnelConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    RegisterPGUser p = new RegisterPGUser();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.Id, UserId);
                    Filter.AddSqlParameters(() => p.Email, Email);
                    Filter.AddSqlParameters(() => p.EmailConfirmed, EmailConfirmed);
                    Filter.AddSqlParameters(() => p.PasswordHash, PasswordHash);
                    Filter.AddSqlParameters(() => p.SecurityStamp, SecurityStamp);
                    Filter.AddSqlParameters(() => p.PhoneNumber, PhoneNumber);
                    Filter.AddSqlParameters(() => p.PhoneNumberConfirmed, PhoneNumberConfirmed);
                    Filter.AddSqlParameters(() => p.TwoFactorEnabled, TwoFactorEnabled);
                    Filter.AddSqlParameters(() => p.LockoutEnabled, LockoutEnabled);
                    if (LockoutEndDateUtc != null)
                    {
                        Filter.AddSqlParameters(() => p.LockoutEndDateUtc, LockoutEndDateUtc);
                    }
                    Filter.AddSqlParameters(() => p.AccessFailedCount, AccessFailedCount);
                    Filter.AddSqlParameters(() => p.UserName, UserName);
                    Filter.AddSqlParameters(() => p.FirstName, FirstName);
                    Filter.AddSqlParameters(() => p.LastName, LastName);
                    Filter.AddSqlParameters(() => p.CountryId, CountryId);
                    Filter.AddSqlParameters(() => p.ShowProductSaleStats, 1);
                    Filter.AddSqlParameters(() => p.FlashFunnelSubDomain, FlashFunnelSubDomain);


                    IRepository<RegisterPGUser> oRepository = new Repository<RegisterPGUser>(uow.DataContext);
                    var result = oRepository.LoadSP(Filter).FirstOrDefault();
                    if (result != null)
                    {
                        return result.Response;
                    }

                    return "FAILED";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<GetAspNetUsers> GetAspNetUserById(string UserId, string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    List<GetAspNetUsers> lUserInformation;
                    GetAspNetUsers p = new GetAspNetUsers();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.Id, UserId);

                    IRepository<GetAspNetUsers> oRepository = new Repository<GetAspNetUsers>(uow.DataContext);
                    lUserInformation = oRepository.LoadSP(Filter);
                    return lUserInformation;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        public static UserInformation GetUserInformation(string UserID)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    List<UserInformation> lUserInformation = new List<UserInformation>();
                    UserInformation oUserInformation = new UserInformation();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUserInformation.UserId, UserID);
                    IRepository<UserInformation> oRepository = new Repository<UserInformation>(uow.DataContext);
                    lUserInformation = oRepository.LoadSP(Filter);
                    if (lUserInformation.Count > 0)
                    {
                        oUserInformation = lUserInformation[0];
                    }
                    return oUserInformation;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static UserEmailByUserId GetUserEmail(string UserID)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    UserEmailByUserId oUserInformation = new UserEmailByUserId();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUserInformation.UserId, UserID);
                    IRepository<UserEmailByUserId> oRepository = new Repository<UserEmailByUserId>(uow.DataContext);
                    return oUserInformation = oRepository.LoadSP(Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //UserEmailByUserId
        public static bool UpdateUserInformantion(UserInformationInsert oUserInformationInsert)
        {
            Boolean Result = false;
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    List<UserInformationInsert> lUserInformation = new List<UserInformationInsert>();
                    UserInformationInsert oUserInformation = new UserInformationInsert();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUserInformation.AspNetUserId, oUserInformationInsert.AspNetUserId);
                    Filter.AddSqlParameters(() => oUserInformation.Id, oUserInformationInsert.Id);
                    Filter.AddSqlParameters(() => oUserInformation.UserId, oUserInformationInsert.UserId);
                    Filter.AddSqlParameters(() => oUserInformation.Email, oUserInformationInsert.Email);
                    Filter.AddSqlParameters(() => oUserInformation.FirstName, oUserInformationInsert.FirstName);
                    Filter.AddSqlParameters(() => oUserInformation.LastName, oUserInformationInsert.LastName);
                    //Filter.AddSqlParameters(() => oUserInformation.PasswordHash, oUserInformationInsert.PasswordHash);
                    //Filter.AddSqlParameters(() => oUserInformation.UserName, oUserInformationInsert.UserName);
                    Filter.AddSqlParameters(() => oUserInformation.PhoneNumber, oUserInformationInsert.PhoneNumber);
                    Filter.AddSqlParameters(() => oUserInformation.IPNSecretKey, oUserInformationInsert.IPNSecretKey);
                    Filter.AddSqlParameters(() => oUserInformation.ShowProductSaleStats, oUserInformationInsert.ShowProductSaleStats);
                    IRepository<UserInformationInsert> oRepository = new Repository<UserInformationInsert>(uow.DataContext);
                    return oRepository.ExecuteSP(Filter);
                    //if (lUserInformation.Count > 0)
                    //{
                    //    oUserInformation = lUserInformation[0];
                    //}
                    //return oUserInformation;
                    //Result = true;
                    //return Result;
                }
            }
            catch (Exception ex)
            {
                Org.Utils.Logger.LogRelativeMessage(("UpdateUserInformantion EXCEPTION OCCURED: " + ex.ToString()), true);
                Org.Utils.Logger.LogRelativeMessage(("UpdateUserInformantion EXCEPTION MESSAGE: " + ex.Message), true);
                Result = false;
                return Result;
                throw ex;
            }
        }


        public static bool SetPayGeneiFeeAgainstVendor(string UserId, decimal Fee)
        {
            Boolean Result = false;
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    AddUserInformation UI = new AddUserInformation();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => UI.UserId, UserId);
                    Filter.AddSqlParameters(() => UI.PayGenieFee, Fee);
                    Filter.AddSqlParameters(() => UI.Command, "UpdatePayGenieFee");
                    IRepository<AddUserInformation> oRepository = new Repository<AddUserInformation>(uow.DataContext);
                    return oRepository.ExecuteSP(Filter);
                }
            }
            catch (Exception ex)
            {
                return Result;
                throw ex;
            }
        }

        public static bool UpdateProfileImage(string CustomUserId, string ProfileImage)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {

                    UpdateProfileImage oUpdateProfileImage = new UpdateProfileImage();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUpdateProfileImage.CustomUserId, CustomUserId);
                    Filter.AddSqlParameters(() => oUpdateProfileImage.ProfileImage, ProfileImage);
                    IRepository<UpdateProfileImage> oRepository = new Repository<UpdateProfileImage>(uow.DataContext);
                    oRepository.LoadSP(Filter);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public static bool UpdateHelpDeskName(string UserId, string HelpDeskName)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {

                    UpdateHelpDeskName oUpdateHelpDeskName = new UpdateHelpDeskName();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUpdateHelpDeskName.UserId, UserId);
                    Filter.AddSqlParameters(() => oUpdateHelpDeskName.HelpDeskName, HelpDeskName);
                    IRepository<UpdateHelpDeskName> oRepository = new Repository<UpdateHelpDeskName>(uow.DataContext);
                    oRepository.LoadSP(Filter);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public static bool IsHelpDeskTaken(string HelpDeskName)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {

                    List<HelpDeskNameCount> lHelpDeskNameCount = new List<HelpDeskNameCount>();
                    HelpDeskNameCount oHelpDeskNameCount = new HelpDeskNameCount();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oHelpDeskNameCount.HelpDeskName, HelpDeskName);
                    IRepository<HelpDeskNameCount> oRepository = new Repository<HelpDeskNameCount>(uow.DataContext);
                    lHelpDeskNameCount = oRepository.LoadSP(Filter);
                    if (lHelpDeskNameCount.FirstOrDefault().Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }


        public static UserProfileInfo GetUserProfileInfoByUserId(string UserID, string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    List<UserProfileInfo> lUserProfileInfo = new List<UserProfileInfo>();
                    UserProfileInfo oUserProfileInfo = new UserProfileInfo();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUserProfileInfo.UserId, UserID);
                    IRepository<UserProfileInfo> oRepository = new Repository<UserProfileInfo>(uow.DataContext);
                    lUserProfileInfo = oRepository.LoadSP(Filter);
                    if (lUserProfileInfo.Count > 0)
                    {
                        oUserProfileInfo = lUserProfileInfo[0];
                    }
                    return oUserProfileInfo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CustomerProfileInfo GetCustomerProfileInfo(string UserId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    List<CustomerProfileInfo> lUserProfileInfo = new List<CustomerProfileInfo>();
                    CustomerProfileInfo oUserProfileInfo = new CustomerProfileInfo();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUserProfileInfo.UserId, UserId);
                    IRepository<CustomerProfileInfo> oRepository = new Repository<CustomerProfileInfo>(uow.DataContext);
                    lUserProfileInfo = oRepository.LoadSP(Filter);
                    if (lUserProfileInfo.Count > 0)
                    {
                        oUserProfileInfo = lUserProfileInfo[0];
                    }
                    return oUserProfileInfo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Email GetRegistrationEmail(string UserId, string CallbackUrl)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Email p = new Email();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.VendorUserId, UserId);
                    Filter.AddSqlParameters(() => p.CallbackUrl, CallbackUrl);
                    IRepository<Email> oRepository = new Repository<Email>(uow.DataContext);
                    return oRepository.LoadSP("sp_GetRegistrationEmail", Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Email GetForgotPasswordEmail(string UserId, string CallbackUrl)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Email p = new Email();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.VendorUserId, UserId);
                    Filter.AddSqlParameters(() => p.CallbackUrl, CallbackUrl);
                    IRepository<Email> oRepository = new Repository<Email>(uow.DataContext);
                    return oRepository.LoadSP("sp_GetForgotPasswordEmail", Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        //Currently to set Specific rights for new user for initial launch
        public static bool SetRightsForNewUser(string Email)
        {
            try
            {
                CustomerProfileInfo U = new CustomerProfileInfo();
                Filters filter = new Filters();
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    filter.AddSqlParameters(() => U.Email, Email);
                    IRepository<CustomerProfileInfo> oRepository = new Repository<CustomerProfileInfo>(uow.DataContext);
                    return oRepository.ExecuteSP("sp_UpdateRightsAgainstSpecificMember", filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        



        public static bool DeleteNewlyCreatedUser(string UserId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    DeleteNewlyCreatedUser oDeleteNewlyCreatedUser = new DeleteNewlyCreatedUser();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oDeleteNewlyCreatedUser.UserId, UserId);
                    IRepository<DeleteNewlyCreatedUser> oRepository = new Repository<DeleteNewlyCreatedUser>(uow.DataContext);
                    oRepository.LoadSP(Filter);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
          
        }
    }
}
