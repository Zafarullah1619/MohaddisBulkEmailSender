using System;
using System.Collections.Generic;
using Org.Utils;
using Org.Business.Objects;
using System.Linq;
using Org.DataAccess;
using System.Web;
using Newtonsoft.Json;

namespace Org.Business.Methods
{
    public class UserPermissionsBAL
    {

        public UserPermissionsBAL()
        {
        }

        public static List<HasPermissions> GetPermissions(string UserId, string MainUserId, string RoleId, string AreaRuleCode)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    HasPermissions oUserPermission = new HasPermissions();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUserPermission.UserId, UserId);
                    Filter.AddSqlParameters(() => oUserPermission.MainUserId, MainUserId);
                    Filter.AddSqlParameters(() => oUserPermission.RoleId, RoleId);
                    Filter.AddSqlParameters(() => oUserPermission.AreaRuleCode, AreaRuleCode);
                    IRepository<HasPermissions> oRepository = new Repository<HasPermissions>(uow.DataContext);

                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsValidUser(string UserId, string SessionID, string WebsiteID, string LoginAttemptId = null)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    UserInfo p = new UserInfo();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.UserId, UserId);
                    Filter.AddSqlParameters(() => p.SessionID, SessionID);
                    Filter.AddSqlParameters(() => p.WebsiteID, WebsiteID);
                    Filter.AddSqlParameters(() => p.LoginAttemptId, LoginAttemptId);
                    //Filter.AddSqlParameters(() => p.Command, "VERIFY");
                    IRepository<BaseEntity> oRepository = new Repository<BaseEntity>(uow.DataContext);

                    BaseEntity b = oRepository.LoadSP("sp_ValidateUser", Filter).FirstOrDefault();

                    if (b != null && b.Id > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
        public static int HasPermission(string UserId, string MainUserId, string RoleId, string AreaRuleCode)
        {
            try
            {
                List<HasPermissions> p = GetPermissions(UserId, MainUserId, RoleId, AreaRuleCode);

                if (p.Count <= 0)
                {
                    return 0;
                }

                return p[0].HasAreaPermissions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UserInfo GetUserInfo(string pUserId, string pUserEmail, string pMainUserId, string pUserRoleName, string SessionID = null, string WebsiteID = null, bool AddSession = false, string LoginAttemptId = null)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    UserInfo oUserPermission = new UserInfo();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUserPermission.UserId, pUserId);
                    Filter.AddSqlParameters(() => oUserPermission.MainUserId, pMainUserId);
                    Filter.AddSqlParameters(() => oUserPermission.RoleName, pUserRoleName);
                    Filter.AddSqlParameters(() => oUserPermission.UserEmail, pUserEmail);
                    Filter.AddSqlParameters(() => oUserPermission.SessionID, SessionID);
                    Filter.AddSqlParameters(() => oUserPermission.WebsiteID, WebsiteID);
                    Filter.AddSqlParameters(() => oUserPermission.AddSession, AddSession);
                    Filter.AddSqlParameters(() => oUserPermission.LoginAttemptId, LoginAttemptId);
                    IRepository<UserInfo> oRepository = new Repository<UserInfo>(uow.DataContext);

                    List<UserInfo> result = oRepository.LoadSP(Filter);

                    if (result.Count <= 0)
                    {
                        return new UserInfo();
                    }

                    return result[0];
                }
            }
            catch (Exception ex)
            {
                Logger.LogRelativeMessage("GetUserInfo Exception " + ex.Message, true);
                throw ex;
            }
        }

        public static UserInfo GetCustomerUserInfo(string pUserId, string pUserEmail, string pMainUserId, string pUserRoleName, string SessionID = null, string WebsiteID = null, bool AddSession = false, string LoginAttemptId = null, string ConnectionString = "CRMConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork("DefaultConnection"))
                {
                    UserInfo oUserPermission = new UserInfo();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUserPermission.UserId, pUserId);
                    Filter.AddSqlParameters(() => oUserPermission.MainUserId, pMainUserId);
                    Filter.AddSqlParameters(() => oUserPermission.RoleName, pUserRoleName);
                    Filter.AddSqlParameters(() => oUserPermission.UserEmail, pUserEmail);
                    Filter.AddSqlParameters(() => oUserPermission.SessionID, SessionID);
                    Filter.AddSqlParameters(() => oUserPermission.WebsiteID, WebsiteID);
                    Filter.AddSqlParameters(() => oUserPermission.AddSession, AddSession);
                    Filter.AddSqlParameters(() => oUserPermission.LoginAttemptId, LoginAttemptId);
                    IRepository<UserInfo> oRepository = new Repository<UserInfo>(uow.DataContext);

                    List<UserInfo> result = oRepository.LoadSP("sp_GetUserInfo", Filter);

                    if (result.Count <= 0)
                    {
                        return new UserInfo();
                    }

                    return result[0];
                }
            }
            catch (Exception ex)
            {
                Logger.LogRelativeMessage("GetCustomerUserInfo Exception " + ex.Message, true);
                throw ex;
            }
        }
        public static long LoadMainModulesMax()
        {
            //MainModule oMainModule = new MainModule();
            //Filters filter = new Filters();
            try
            {
                //return (Int32)DataAccessHelper.LoadObjectMax(oMainModule, () => oMainModule.Id, filter);
                return CommonBAL.GetMaxValue("Id", "tblMainModules");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public static LoginUser SaveLoginAttempt(string Email, string Password, bool RememberMe, string UserIPAddress, string ReturnUrl, HttpRequestBase request)
        {
            try
            {
                string RequestObject = JsonConvert.SerializeObject(request,
                                                                Formatting.Indented, new JsonSerializerSettings
                                                                {
                                                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                                                    ContractResolver = new IgnoreErrorPropertiesResolver()
                                                                });


                string AcceptTypes = JsonConvert.SerializeObject(request.AcceptTypes);
                string AnonymousID = request.AnonymousID;
                string AppRelativeCurrentExecutionFilePath = request.AppRelativeCurrentExecutionFilePath;
                string ApplicationPath = request.ApplicationPath;
                string Browser = JsonConvert.SerializeObject(request.Browser);
                string ClientCertificate = JsonConvert.SerializeObject(request.ClientCertificate);
                string ContentEncoding = JsonConvert.SerializeObject(request.ContentEncoding);
               
                int ContentLength = request.ContentLength;
                string ContentType = request.ContentType;
                string Cookies = JsonConvert.SerializeObject(request.Cookies);
                string CurrentExecutionFilePath = request.CurrentExecutionFilePath;
                string CurrentExecutionFilePathExtension = request.CurrentExecutionFilePathExtension;
                string FilePath = request.FilePath;
                string Files = JsonConvert.SerializeObject(request.Files);
                

                string Form = JsonConvert.SerializeObject(request.Form);
                string Headers = JsonConvert.SerializeObject(request.Headers);
                string HttpChannelBinding = JsonConvert.SerializeObject(request.HttpChannelBinding);
                string HttpMethod = request.HttpMethod;
                bool IsAuthenticated = request.IsAuthenticated;
                bool IsLocal = request.IsLocal;
                bool IsSecureConnection = request.IsSecureConnection;
                string LogonUserIdentity = JsonConvert.SerializeObject(request.LogonUserIdentity);
                string Params = JsonConvert.SerializeObject(request.Params);
                string Path = request.Path;
                string PathInfo = request.PathInfo;
                

                string PhysicalApplicationPath = request.PhysicalApplicationPath;
                string PhysicalPath = request.PhysicalPath;
                string QueryString = JsonConvert.SerializeObject(request.QueryString);
                string RawUrl = request.RawUrl;
                string ReadEntityBodyMode = JsonConvert.SerializeObject(request.ReadEntityBodyMode);
                string RequestType = request.RequestType;
                string ServerVariables = JsonConvert.SerializeObject(request.ServerVariables);
                string TimedOutToken = JsonConvert.SerializeObject(request.TimedOutToken);
                string TlsTokenBindingInfo = null;

                


                int TotalBytes = request.TotalBytes;
                string Unvalidated = JsonConvert.SerializeObject(request.Unvalidated);
                string Url = JsonConvert.SerializeObject(request.Url);
                string UrlReferrer = JsonConvert.SerializeObject(request.UrlReferrer);
                string UserAgent = request.UserAgent;
                string UserHostAddress = request.UserHostAddress;
                string UserHostName = request.UserHostName;
                string UserLanguages = JsonConvert.SerializeObject(request.UserLanguages);
                


                using (IUnitOfWork uow = new UnitOfWork())
                {
                    


                    LoginUser p = new LoginUser();
                    Filters Filter = new Filters();

                    Filter.AddSqlParameters(() => p.Email, Email);
                    Filter.AddSqlParameters(() => p.PasswordHash, Password);
                    Filter.AddSqlParameters(() => p.RememberMe, RememberMe);
                    Filter.AddSqlParameters(() => p.ReturnUrl, ReturnUrl);
                    Filter.AddSqlParameters(() => p.UserIPAddress, UserIPAddress);
                    Filter.AddSqlParameters(() => p.RequestObject, RequestObject);
                    Filter.AddSqlParameters(() => p.AcceptTypes, AcceptTypes);
                    Filter.AddSqlParameters(() => p.AnonymousID, AnonymousID);
                    Filter.AddSqlParameters(() => p.AppRelativeCurrentExecutionFilePath, AppRelativeCurrentExecutionFilePath);
                    Filter.AddSqlParameters(() => p.ApplicationPath, ApplicationPath);
                    Filter.AddSqlParameters(() => p.Browser, Browser);
                    Filter.AddSqlParameters(() => p.ClientCertificate, ClientCertificate);
                    Filter.AddSqlParameters(() => p.ContentEncoding, ContentEncoding);
                    Filter.AddSqlParameters(() => p.ContentLength, ContentLength);
                    Filter.AddSqlParameters(() => p.ContentType, ContentType);
                    Filter.AddSqlParameters(() => p.Cookies, Cookies);
                    Filter.AddSqlParameters(() => p.CurrentExecutionFilePath, CurrentExecutionFilePath);
                    Filter.AddSqlParameters(() => p.CurrentExecutionFilePathExtension, CurrentExecutionFilePathExtension);
                    Filter.AddSqlParameters(() => p.FilePath, FilePath);
                    Filter.AddSqlParameters(() => p.Files, Files);
                    Filter.AddSqlParameters(() => p.Form, Form);
                    Filter.AddSqlParameters(() => p.Headers, Headers);
                    Filter.AddSqlParameters(() => p.HttpChannelBinding, HttpChannelBinding);
                    Filter.AddSqlParameters(() => p.HttpMethod, HttpMethod);
                    Filter.AddSqlParameters(() => p.IsAuthenticated, IsAuthenticated);
                    Filter.AddSqlParameters(() => p.IsLocal, IsLocal);
                    Filter.AddSqlParameters(() => p.IsSecureConnection, IsSecureConnection);
                    Filter.AddSqlParameters(() => p.LogonUserIdentity, LogonUserIdentity);
                    Filter.AddSqlParameters(() => p.Params, Params);
                    Filter.AddSqlParameters(() => p.Path, Path);
                    Filter.AddSqlParameters(() => p.PathInfo, PathInfo);
                    Filter.AddSqlParameters(() => p.PhysicalApplicationPath, PhysicalApplicationPath);
                    Filter.AddSqlParameters(() => p.PhysicalPath, PhysicalPath);
                    Filter.AddSqlParameters(() => p.QueryString, QueryString);
                    Filter.AddSqlParameters(() => p.RawUrl, RawUrl);
                    Filter.AddSqlParameters(() => p.ReadEntityBodyMode, ReadEntityBodyMode);
                    Filter.AddSqlParameters(() => p.RequestType, RequestType);
                    Filter.AddSqlParameters(() => p.ServerVariables, ServerVariables);
                    Filter.AddSqlParameters(() => p.TimedOutToken, TimedOutToken);
                    Filter.AddSqlParameters(() => p.TlsTokenBindingInfo, TlsTokenBindingInfo);
                    Filter.AddSqlParameters(() => p.TotalBytes, TotalBytes);
                    Filter.AddSqlParameters(() => p.Unvalidated, Unvalidated);
                    Filter.AddSqlParameters(() => p.Url, Url);
                    Filter.AddSqlParameters(() => p.UrlReferrer, UrlReferrer);
                    Filter.AddSqlParameters(() => p.UserAgent, UserAgent);
                    Filter.AddSqlParameters(() => p.UserHostAddress, UserHostAddress);
                    Filter.AddSqlParameters(() => p.UserHostName, UserHostName);
                    Filter.AddSqlParameters(() => p.UserLanguages, UserLanguages);
                    Filter.AddSqlParameters(() => p.Command, "INSERT");

                    IRepository<LoginUser> oRepository = new Repository<LoginUser>(uow.DataContext);
                    


                    return oRepository.LoadSP(Filter).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                Logger.LogRelativeMessage("SaveLoginAttempt Exception:: " + ex.ToString(), true);
                throw ex;
            }
        }

        public static int UpdateMemberRights(string pNewSubUserId, string SessionAdminUserId, string pCurrentUserId, string pAreaRights)
        {
            try
            {
                /*
                DbCommand oCmd = DataAccessHelper.CreateStoredProcCommand("sp_AddSubUser");
                DataAccessHelper.CreateInParameter(oCmd, "pNewSubUserId", System.Data.DbType.String, pNewSubUserId);
                DataAccessHelper.CreateInParameter(oCmd, "pCurrentUserId", System.Data.DbType.String, pCurrentUserId);
                DataAccessHelper.CreateInParameter(oCmd, "pMainUserId", System.Data.DbType.String, pMainUserId);
                DataAccessHelper.CreateInParameter(oCmd, "pCurrentRoleName", System.Data.DbType.String, pCurrentRoleName);
                DataAccessHelper.CreateInParameter(oCmd, "pAreaRights", System.Data.DbType.String, pAreaRights);
                oAddSubUser = Convert.ToInt32(DataAccessHelper.LoadObject(oAddSubUser, oCmd));
                */

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    AddSubUser oUser = new AddSubUser();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUser.NewSubUserId, pNewSubUserId);
                    Filter.AddSqlParameters(() => oUser.SessionAdminUserId, SessionAdminUserId);
                    Filter.AddSqlParameters(() => oUser.CurrentUserId, pCurrentUserId);
                    Filter.AddSqlParameters(() => oUser.AreaRights, pAreaRights);
                    IRepository<AddSubUser> oRepository = new Repository<AddSubUser>(uow.DataContext);

                    oUser = oRepository.LoadSP("sp_UpdateMemberRights", Filter)[0];

                    return oUser.RETURNVAL;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static void AddFunnelInfo(string SessionUserId, string UserId, string CustomSessionUserId, string FlashFunnelSubDomain, string ConnectionStringName = "FunnelConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionStringName))
                {
                    UserInfo p = new UserInfo();
                    Filters Filter = new Filters();

                    Filter.AddSqlParameters(() => p.SessionUserId, SessionUserId);
                    Filter.AddSqlParameters(() => p.UserId, UserId);
                    Filter.AddSqlParameters(() => p.CustomSessionUserId, CustomSessionUserId);
                    
                    IRepository<BaseEntity> oRepository = new Repository<BaseEntity>(uow.DataContext);

                    oRepository.ExecuteSP("sp_AddFunnelInfo", Filter);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public static string GetUserIdByEmail(string pUserEmail)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Users oUser = new Users();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oUser.Email, pUserEmail);
                    IRepository<Users> oRepository = new Repository<Users>(uow.DataContext);

                    List<Users> lUsers = oRepository.LoadSP(Filter);
                    if (lUsers.Count > 0)
                    {
                        return lUsers[0].Id;
                    }
                    else
                    {
                        return "Invalid";
                    }
                }
                //filter.AddParameters(() => oUser.Email, OperatorsList.Equal, pUserEmail);
                //return ((Users)DataAccessHelper.LoadObject(oUser, "SELECT [Id] FROM [AspNetUsers]", filter).ToList()[0]).Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetRoleId(string pRoleName)
        {
            try
            {
                //UserRoles oRole = new UserRoles();
                //Filters filter = new Filters();
                //filter.AddParameters(() => oRole.Name, OperatorsList.Equal, pRoleName);
                //return ((UserRoles)DataAccessHelper.LoadObject(oRole, "SELECT [Id] FROM [AspNetRoles]", filter).ToList()[0]).Id;

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    UserRoles oRole = new UserRoles();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oRole.Name, pRoleName);
                    IRepository<UserRoles> oRepository = new Repository<UserRoles>(uow.DataContext);

                    return oRepository.LoadSP(Filter)[0].Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static long LoadAreaRulesMax()
        {
            //AreaRule oAreaRule = new AreaRule();
            //Filters filter = new Filters();
            try
            {
                //return (Int32)DataAccessHelper.LoadObjectMax(oAreaRule, () => oAreaRule.Id, filter);

                return CommonBAL.GetMaxValue("Id", "tblAreaRules");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<Navigation> GetNavigationList(string pUserId, string pMainUserId, string pRoleId, string pMenuArea, long pParentId)
        {
            try
            {
                //Navigation oNavigation = new Navigation();
                //DbCommand oCmd = DataAccessHelper.CreateStoredProcCommand("sp_GetNavigationList");
                //DataAccessHelper.CreateInParameter(oCmd, "pUserId", System.Data.DbType.String, pUserId);
                //DataAccessHelper.CreateInParameter(oCmd, "pMainUserId", System.Data.DbType.String, pMainUserId);
                //DataAccessHelper.CreateInParameter(oCmd, "pRoleName", System.Data.DbType.String, pRoleName);
                //DataAccessHelper.CreateInParameter(oCmd, "pApplicationArea", System.Data.DbType.String, pMenuArea);
                //DataAccessHelper.CreateInParameter(oCmd, "pParentId", System.Data.DbType.Int64, pParentId);
                //return DataAccessHelper.LoadObject(oNavigation, oCmd).Cast<Navigation>().ToList();

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Navigation oNavigation = new Navigation();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oNavigation.UserId, pUserId);
                    Filter.AddSqlParameters(() => oNavigation.MainUserId, pMainUserId);
                    Filter.AddSqlParameters(() => oNavigation.RoleId, pRoleId);
                    Filter.AddSqlParameters(() => oNavigation.ApplicationArea, pMenuArea);
                    Filter.AddSqlParameters(() => oNavigation.ParentId, pParentId);
                    IRepository<Navigation> oRepository = new Repository<Navigation>(uow.DataContext);

                    var lstMenu= oRepository.LoadSP(Filter);
                    return lstMenu;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MainModule> GetModulesList(string pUserId, string pMainUserId, string pRoleId, int pCheckAreaPermissions)
        {
            try
            {
                //MainModule oModules = new MainModule();
                //DbCommand oCmd = DataAccessHelper.CreateStoredProcCommand("sp_GetUserModulesList");
                //DataAccessHelper.CreateInParameter(oCmd, "pUserId", System.Data.DbType.String, pUserId);
                //DataAccessHelper.CreateInParameter(oCmd, "pMainUserId", System.Data.DbType.String, pMainUserId);
                //DataAccessHelper.CreateInParameter(oCmd, "pRoleName", System.Data.DbType.String, pRoleName);
                //DataAccessHelper.CreateInParameter(oCmd, "pCheckAreaPermissions", System.Data.DbType.Int32, pCheckAreaPermissions);
                //return DataAccessHelper.LoadObject(oModules, oCmd).Cast<MainModule>().ToList();

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    MainModule oModules = new MainModule();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oModules.UserId, pUserId);
                    Filter.AddSqlParameters(() => oModules.MainUserId, pMainUserId);
                    Filter.AddSqlParameters(() => oModules.RoleId, pRoleId);
                    Filter.AddSqlParameters(() => oModules.CheckAreaPermissions, pCheckAreaPermissions);
                    IRepository<MainModule> oRepository = new Repository<MainModule>(uow.DataContext);

                    return oRepository.LoadSP(Filter);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<ApplicationArea> GetApplicationAreasList(string pUserId, string pMainUserId, string pRoleId, int pModuleId, int pParentId, int pCheckAreaPermissions)
        {
            try
            {
                //ApplicationArea oApplicationArea = new ApplicationArea();
                //DbCommand oCmd = DataAccessHelper.CreateStoredProcCommand("sp_GetUserApplicationAreasList");
                //DataAccessHelper.CreateInParameter(oCmd, "pUserId", System.Data.DbType.String, pUserId);
                //DataAccessHelper.CreateInParameter(oCmd, "pMainUserId", System.Data.DbType.String, pMainUserId);
                //DataAccessHelper.CreateInParameter(oCmd, "pRoleName", System.Data.DbType.String, pRoleName);
                //DataAccessHelper.CreateInParameter(oCmd, "pModuleId", System.Data.DbType.Int32, pModuleId);
                //DataAccessHelper.CreateInParameter(oCmd, "pParentId", System.Data.DbType.Int32, pParentId);
                //DataAccessHelper.CreateInParameter(oCmd, "pCheckAreaPermissions", System.Data.DbType.Int32, pCheckAreaPermissions);
                //return DataAccessHelper.LoadObject(oApplicationArea, oCmd).Cast<ApplicationArea>().ToList();

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ApplicationArea oApplicationArea = new ApplicationArea();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oApplicationArea.UserId, pUserId);
                    Filter.AddSqlParameters(() => oApplicationArea.MainUserId, pMainUserId);
                    Filter.AddSqlParameters(() => oApplicationArea.RoleId, pRoleId);
                    Filter.AddSqlParameters(() => oApplicationArea.ModuleId, pModuleId);
                    Filter.AddSqlParameters(() => oApplicationArea.ParentId, pParentId);
                    Filter.AddSqlParameters(() => oApplicationArea.CheckAreaPermissions, pCheckAreaPermissions);
                    IRepository<ApplicationArea> oRepository = new Repository<ApplicationArea>(uow.DataContext);

                    return oRepository.LoadSP(Filter);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<AreaRule> GetAreaRulesList(string pUserId, string pMainUserId, string pRoleId, int pModuleId, int pApplicationAreaId, int pCheckAreaPermissions, string Command = "")
        {
            try
            {
                //AreaRule oAreaRule = new AreaRule();
                //DbCommand oCmd = DataAccessHelper.CreateStoredProcCommand("sp_GetUserAreaRulesList");
                //DataAccessHelper.CreateInParameter(oCmd, "pUserId", System.Data.DbType.String, pUserId);
                //DataAccessHelper.CreateInParameter(oCmd, "pMainUserId", System.Data.DbType.String, pMainUserId);
                //DataAccessHelper.CreateInParameter(oCmd, "pRoleName", System.Data.DbType.String, pRoleName);
                //DataAccessHelper.CreateInParameter(oCmd, "pModuleId", System.Data.DbType.Int32, pModuleId);
                //DataAccessHelper.CreateInParameter(oCmd, "pApplicationAreaId", System.Data.DbType.Int32, pApplicationAreaId);
                //DataAccessHelper.CreateInParameter(oCmd, "pCheckAreaPermissions", System.Data.DbType.Int32, pCheckAreaPermissions);
                //return DataAccessHelper.LoadObject(oAreaRule, oCmd).Cast<AreaRule>().ToList();

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    AreaRule oAreaRule = new AreaRule();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oAreaRule.UserId, pUserId);
                    Filter.AddSqlParameters(() => oAreaRule.MainUserId, pMainUserId);
                    Filter.AddSqlParameters(() => oAreaRule.RoleId, pRoleId);
                    Filter.AddSqlParameters(() => oAreaRule.ModuleId, pModuleId);
                    Filter.AddSqlParameters(() => oAreaRule.ApplicationAreaId, pApplicationAreaId);
                    Filter.AddSqlParameters(() => oAreaRule.CheckAreaPermissions, pCheckAreaPermissions);
                    Filter.AddSqlParameters(() => oAreaRule.Command, Command);
                    IRepository<AreaRule> oRepository = new Repository<AreaRule>(uow.DataContext);
                    var permissions = oRepository.LoadSP(Filter);
                    return permissions;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Navigation> GetMenu(string pUserId, string pMainUserId, string pRoleId, string pMenuArea)
        {
            List<Navigation> lMenuList = new List<Navigation>();

            lMenuList = GetChildMenu(pUserId, pMainUserId, pRoleId, pMenuArea, -1);

            return lMenuList;
        }

        private static List<Navigation> GetChildMenu(string pUserId, string pMainUserId, string pRoleId, string pMenuArea, long pParentId)
        {
            List<Navigation> lReturnList = new List<Navigation>();
            List<Navigation> lSubMenuList = new List<Navigation>();
            int ParentId = 0, ParentLocation = 0;

            lReturnList = GetNavigationList(pUserId, pMainUserId, pRoleId, pMenuArea, pParentId);

            for (int i = lReturnList.Count - 1; i >= 0; i--)
            {
                if (i >= lReturnList.Count)
                {
                    break;
                }
                ParentId = Convert.ToInt32(lReturnList[i].ParentId);
                //lSubMenuList = GetChildMenu(pUserId, pMainUserId, pRoleId, pMenuArea, ParentId);
                if (ParentId > 0)
                {
                    ParentLocation = lReturnList.FindIndex(Navigation.ById(ParentId));
                    if (ParentLocation > 0 && lReturnList.Count > ParentLocation)
                    {
                        if (lReturnList[ParentLocation].SubMenu == null)
                        {
                            lReturnList[ParentLocation].SubMenu = new List<Navigation>();
                        }
                        lReturnList[ParentLocation].SubMenu.Insert(0, lReturnList[i]);
                        lReturnList.RemoveAt(i);
                    }
                }
            }

            return lReturnList;
        }

        public static List<MainModule> GetUserModules(string pUserId, string pMainUserId, string pRoleId, int pCheckAreaPermissions)
        {
            List<MainModule> lModuleList = new List<MainModule>();
            List<ApplicationArea> lApplicationArea = new List<ApplicationArea>();
            int ModuleId = 0;

            lModuleList = GetModulesList(pUserId, pMainUserId, pRoleId, pCheckAreaPermissions);

            for (int i = 0; i < lModuleList.Count(); i++)
            {
                ModuleId = Convert.ToInt32(lModuleList.ElementAt(i).Id);
                lApplicationArea = GetSubApplicationAreas(pUserId, pMainUserId, pRoleId, ModuleId, 0, pCheckAreaPermissions);

                if (lApplicationArea.Count > 0)
                {
                    //var id=lApplicationArea.Find(x => x.Id == 17 || x.Id==18 || x.Id==73).Id;
                    //if(id==17 || id==18 || id==73)
                    //{
                    //    var sd = "";
                    //}
                    lModuleList.ElementAt(i).ApplicationAreas = lApplicationArea;
                }
            }

            return lModuleList;
        }

        private static List<ApplicationArea> GetSubApplicationAreas(string pUserId, string pMainUserId, string pRoleId, int pModuleId, int pParentId, int pCheckAreaPermissions)
        {
            List<ApplicationArea> lReturnList = new List<ApplicationArea>();
            List<ApplicationArea> lSubAreasList = new List<ApplicationArea>();
            List<AreaRule> lAreaRules = new List<AreaRule>();
            int ParentId = 0;
            if (pModuleId == 5)
            {
                var id = pModuleId;
            }
            string Command = "SubUser";
            lReturnList = GetApplicationAreasList(pUserId, pMainUserId, pRoleId, pModuleId, pParentId, pCheckAreaPermissions);

            for (int i = 0; i < lReturnList.Count(); i++)
            {
                ParentId = Convert.ToInt32(lReturnList.ElementAt(i).Id);
                lAreaRules = GetAreaRulesList(pUserId, pMainUserId, pRoleId, pModuleId, ParentId, pCheckAreaPermissions, Command);
                if (lAreaRules.Count() > 0)
                {
                    lReturnList.ElementAt(i).AreaRules = lAreaRules;
                }
                lSubAreasList = GetSubApplicationAreas(pUserId, pMainUserId, pRoleId, pModuleId, ParentId, pCheckAreaPermissions);
                if (lSubAreasList.Count() > 0)
                {
                    lReturnList.ElementAt(i).SubAreas = lSubAreasList;
                }
            }

            return lReturnList;
        }
    }
}
