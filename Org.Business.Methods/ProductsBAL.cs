using Newtonsoft.Json;
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
    public class ProductsBAL
    {
        public static List<Product> lstUpsellProducts()
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product E = new Product();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.Command, "SELECTUPSELLPRODUCTS");
                    IRepository<Product> oRepository = new Repository<Product>(uow.DataContext);

                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ProductCategory> lstProductCategory()
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product E = new Product();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.Command, "SELECTCATEGORY");
                    IRepository<ProductCategory> oRepository = new Repository<ProductCategory>(uow.DataContext);

                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Product ProductDetail(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product P = new Product();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => P.Id, ProductId);
                    Filter.AddSqlParameters(() => P.Command, "GetDetail");
                    IRepository<Product> oRepository = new Repository<Product>(uow.DataContext);

                    return oRepository.LoadSP(Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       



        public static List<Product> GetProducts()
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product P = new Product();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => P.Command, "GetAll"); // Assuming this command gets all products
                    IRepository<Product> oRepository = new Repository<Product>(uow.DataContext);

                    return oRepository.LoadSP(Filter).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ProductUpsell> ProductUpSells(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductUpsell PU = new ProductUpsell();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => PU.ProductId, ProductId);
                    Filter.AddSqlParameters(() => PU.Command, "SELECTUPSELLS");
                    IRepository<ProductUpsell> oRepository = new Repository<ProductUpsell>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetProducts", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ProductRule> ProductRules(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductRule PU = new ProductRule();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => PU.ProductId, ProductId);
                    Filter.AddSqlParameters(() => PU.Command, "SELECTRULES");
                    IRepository<ProductRule> oRepository = new Repository<ProductRule>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetProducts", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SmtpConfiguration ProductSMTP(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    SmtpConfiguration SC = new SmtpConfiguration();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => SC.ProductId, ProductId);
                    Filter.AddSqlParameters(() => SC.Command, "SELECTSMTP");
                    IRepository<SmtpConfiguration> oRepository = new Repository<SmtpConfiguration>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetProducts", Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static BaseEntity AddEditUpsProduct(long? ProductId, string ProductName, string DBServer, string DBUserName, string DBPassword, string DBServerType, string DataBaseName)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product E = new Product();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.ProductName, ProductName);
                    Filter.AddSqlParameters(() => E.DBServer, DBServer);
                    Filter.AddSqlParameters(() => E.DBUserName, DBUserName);
                    Filter.AddSqlParameters(() => E.DBPassword, DBPassword);
                    Filter.AddSqlParameters(() => E.DBServerType, DBServerType);
                    Filter.AddSqlParameters(() => E.DataBaseName, DataBaseName);

                    if (ProductId > 0)
                    {
                        Filter.AddSqlParameters(() => E.Id, ProductId);
                        Filter.AddSqlParameters(() => E.Command, "UPDATEUPSELLPRODUCT");
                    }
                    else
                    {
                        Filter.AddSqlParameters(() => E.Command, "INSERTUPSELLPRODUCT");
                    }
                    IRepository<BaseEntity> oRepository = new Repository<BaseEntity>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetProducts", Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddEditProductUpsells(long? ProductId, string UpSells, string UpsellIDs)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductUpsell PU = new ProductUpsell();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => PU.ProductId, ProductId);
                    Filter.AddSqlParameters(() => PU.Upsells, UpSells);
                    Filter.AddSqlParameters(() => PU.UpsellIDs, UpsellIDs);
                    Filter.AddSqlParameters(() => PU.Command, "UPDATEUPSELLS");
                    IRepository<ProductUpsell> oRepository = new Repository<ProductUpsell>(uow.DataContext);

                    return oRepository.ExecuteSP("sp_GetProducts", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddEditProductRules(ProductRulesViewModel model, string IDs)
        {
            try
            {
                var strRules = string.Join(" ", model.RulesJson);

                //strRules = JsonConvert.SerializeObject(model.Rules);
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductRule E = new ProductRule();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.Id, model.RuleId);
                    Filter.AddSqlParameters(() => E.IDs, IDs);
                    Filter.AddSqlParameters(() => E.ProductId, model.ProductId);
                    Filter.AddSqlParameters(() => E.Rule, model.Rules);
                    Filter.AddSqlParameters(() => E.RulesJson, strRules);
                    Filter.AddSqlParameters(() => E.IsActive, model.IsActive);
                    Filter.AddSqlParameters(() => E.Subject, model.Subject);
                    Filter.AddSqlParameters(() => E.EmailContent, model.EmailBody);
                    Filter.AddSqlParameters(() => E.Command, "UPDATERULES");
                    IRepository<ProductRule> oRepository = new Repository<ProductRule>(uow.DataContext);

                    return oRepository.ExecuteSP("sp_GetProducts", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ProductRule> lstProductRules(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductRule E = new ProductRule();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.ProductId, ProductId);
                    Filter.AddSqlParameters(() => E.Command, "SELECTRULES");
                    IRepository<ProductRule> oRepository = new Repository<ProductRule>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetProducts", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ProductUpsell> lstProductUpsells(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductRule E = new ProductRule();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.ProductId, ProductId);
                    Filter.AddSqlParameters(() => E.Command, "SELECTUPSELLS");
                    IRepository<ProductUpsell> oRepository = new Repository<ProductUpsell>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetProducts", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool RemoveProduct(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product P = new Product();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => P.Id, ProductId);
                    Filter.AddSqlParameters(() => P.Command, "Remove");
                    IRepository<Product> oRepository = new Repository<Product>(uow.DataContext);

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
