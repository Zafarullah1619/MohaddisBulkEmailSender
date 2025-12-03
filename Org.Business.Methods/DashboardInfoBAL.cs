using Org.Business.Objects;
using Org.DataAccess;
using Org.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Org.Business.Methods
{
    public class DashboardInfoBAL
    {
        public static DashboardInfo GetDashboardInfo(string UserId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    DashboardInfo E = new DashboardInfo();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.UserId, UserId);
                    IRepository<DashboardInfo> oRepository = new Repository<DashboardInfo>(uow.DataContext);

                    return oRepository.LoadSP(Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
