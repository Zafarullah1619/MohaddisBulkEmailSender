using Org.Business.Objects;
using Org.DataAccess;
using Org.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Methods
{
    public class AspNetRoleBAL
    {
        public static List<AspNetRole> GetAspRoles()
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    AspNetRole p = new AspNetRole();
                    Filters Filter = new Filters();

                    IRepository<AspNetRole> oRepository = new Repository<AspNetRole>(uow.DataContext);
                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static RolesDefaultPermission GetRolesDefaultPermissions(string roleId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    RolesDefaultPermission p = new RolesDefaultPermission();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.RoleId, roleId);

                    IRepository<RolesDefaultPermission> oRepository = new Repository<RolesDefaultPermission>(uow.DataContext);
                    return oRepository.LoadSP(Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateRolesDefaultPermissions(string RoleId, string SessionAdminUserId, string CurrentUserId, string AreaRights)
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
                    RolesDefaultPermission permissions = new RolesDefaultPermission();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => permissions.RoleId, RoleId);
                    Filter.AddSqlParameters(() => permissions.SessionAdminUserId, SessionAdminUserId);
                    Filter.AddSqlParameters(() => permissions.CurrentUserId, CurrentUserId);
                    Filter.AddSqlParameters(() => permissions.RightsAssigned, AreaRights);
                    IRepository<RolesDefaultPermission> oRepository = new Repository<RolesDefaultPermission>(uow.DataContext);

                    return oRepository.ExecuteSP("sp_UpdateRolesDefaultPermissions", Filter);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
