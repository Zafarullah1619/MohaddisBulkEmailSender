using System;
using System.Collections.Generic;
using System.Linq;
using Org.Utils;
using Org.Business.Objects;
using Org.DataAccess;

namespace Org.Business.Methods
{
    public class MembersBAL
    {
        public MembersBAL()
        {
        }

        private static List<Member> GetMembersList(string AdminUserId, long OffSet, long PageSize, int SortColumn, string SortOrder, DateTimeOffset? StartRegDate, DateTimeOffset? EndRegDate, int QuickDate, string CustomUserId, string Email, string FullName, int IsBlackListed)
        {
            try
            {
                DateTimeOffset Today = DateTimeOffset.Now.Date;
                if (QuickDate > 0)
                {
                    if (QuickDate == CommonStatus.QuickDates.Today)
                    {
                        StartRegDate = Today;
                        EndRegDate = Today;
                    }
                    else if (QuickDate == CommonStatus.QuickDates.Yesterday)
                    {
                        StartRegDate = Today.AddDays(-1);
                        EndRegDate = Today.AddDays(-1);
                    }
                    else if (QuickDate == CommonStatus.QuickDates.LastSevenDays)
                    {
                        StartRegDate = Today.AddDays(-7);
                        EndRegDate = Today;
                    }
                    else if (QuickDate == CommonStatus.QuickDates.ThisMonth)
                    {
                        StartRegDate = new DateTimeOffset(Today.Year, Today.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset);
                        EndRegDate = Today;
                    }
                    else if (QuickDate == CommonStatus.QuickDates.LastMonth)
                    {
                        StartRegDate = new DateTimeOffset(Today.Year, Today.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset).AddMonths(-1);
                        EndRegDate = new DateTimeOffset(Today.Year, Today.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset).AddDays(-1);
                    }
                    else if (QuickDate == CommonStatus.QuickDates.AllTime)
                    {
                        StartRegDate = Today.AddYears(-200);
                        EndRegDate = Today;
                    }
                }

                if (EndRegDate != null)
                {
                    EndRegDate = EndRegDate.Value.AddDays(1).AddMilliseconds(-1);
                }

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Member p = new Member();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => p.AdminUserId, AdminUserId);
                    Filter.AddSqlParameters(() => p.OffSet, OffSet);
                    Filter.AddSqlParameters(() => p.PageSize, PageSize);
                    Filter.AddSqlParameters(() => p.SortColumn, SortColumn);
                    Filter.AddSqlParameters(() => p.SortOrder, SortOrder);
                    Filter.AddSqlParameters(() => p.StartRegDate, StartRegDate);
                    Filter.AddSqlParameters(() => p.EndRegDate, EndRegDate);
                    Filter.AddSqlParameters(() => p.CustomUserId, CustomUserId);
                    Filter.AddSqlParameters(() => p.Email, Email);
                    Filter.AddSqlParameters(() => p.FullName, FullName);
                    Filter.AddSqlParameters(() => p.IsBlackListed, IsBlackListed);
                    IRepository<Member> oRepository = new Repository<Member>(uow.DataContext);

                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Member> GetMembers(string AdminUserId, long OffSet, long PageSize, int SortColumn, string SortOrder, DateTimeOffset? StartRegDate, DateTimeOffset? EndRegDate, int QuickDate, string CustomUserId, string Email, string FullName, int IsBlackListed)
        {
            try
            {
                return GetMembersList(AdminUserId, OffSet, PageSize, SortColumn, SortOrder, StartRegDate, EndRegDate, QuickDate, CustomUserId, Email, FullName, IsBlackListed);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Configuration Settings()
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Configuration C = new Configuration();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => C.Command, "Select");
                    IRepository<Configuration> oRepository = new Repository<Configuration>(uow.DataContext);

                    return oRepository.LoadSP(Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool UpdateSettings(Configuration config)
        {
            try
            {
                if (config == null)
                {
                    throw new Exception("Object is not instantiated in Client Save Method");
                }

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Configuration C = new Configuration();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => C.PayGenieFee, config.PayGenieFee);
                    Filter.AddSqlParameters(() => C.ActivationFee, config.ActivationFee);
                    //Filter.AddSqlParameters(() => C.ThresholdAmount, config.ThresholdAmount);
                    Filter.AddSqlParameters(() => C.PayGenieFixedFee, config.PayGenieFixedFee);
                    //Filter.AddSqlParameters(() => C.HeatPopularity, config.HeatPopularity);
                    //Filter.AddSqlParameters(() => C.HeatDaysFactor, config.HeatDaysFactor);
                    Filter.AddSqlParameters(() => C.ChargeBackAmount, config.ChargeBackAmount);
                    Filter.AddSqlParameters(() => C.IsPrebillNotification, config.IsPrebillNotification);
                    Filter.AddSqlParameters(() => C.PerbillNotificationSentBefore, config.PerbillNotificationSentBefore);
                    Filter.AddSqlParameters(() => C.PayPalPrimaryEmail, config.PayPalPrimaryEmail);
                    Filter.AddSqlParameters(() => C.IsOPRedirctToSuccess, config.IsOPRedirctToSuccess);
                    //Filter.AddSqlParameters(() => C.IsAllowRefundToSeller, config.IsAllowRefundToSeller);
                    Filter.AddSqlParameters(() => C.ProcessPaymentsGapInMinutes, config.ProcessPaymentsGapInMinutes);
                    Filter.AddSqlParameters(() => C.Command, "Update");

                    IRepository<Configuration> oRepository = new Repository<Configuration>(uow.DataContext);
                    return oRepository.ExecuteSP(Filter);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
