using MySql.Data.MySqlClient;
using Org.Business.Objects;
using Org.DataAccess;
using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Org.Business.Methods
{
    public class SubscriberProductsBAL
    {
        public static BaseEntity AddSubscriber(string Email, string Name, long ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductSubscribers E = new ProductSubscribers();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.Email, Email);
                    Filter.AddSqlParameters(() => E.Name, Name);
                    Filter.AddSqlParameters(() => E.ProductId, ProductId);
                    Filter.AddSqlParameters(() => E.Command, "INSERTSUBSCRIBER");
                    IRepository<BaseEntity> oRepository = new Repository<BaseEntity>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetSubscribers", Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Dictionary<string, object>> GetContentForProduct(long productId, string contentQuery)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product product = ProductsBAL.ProductDetail(productId);

                    if (product == null)
                    {
                        // Product not found, return early or log a warning
                        Logger.LogRelativeMessage("Product not found for ID: " + productId, true); // Log the exception
                        return new List<Dictionary<string, object>>();
                    }


                    // Determine the content query based on DBServertype

                    IRepository<List<Dictionary<string, object>>> contentRepository = new Repository<List<Dictionary<string, object>>>(uow.DataContext);
                    var content = new List<Dictionary<string, object>>();

                    if (product.DBServerType == "MSSQL")
                    {

                        string connectionString = GenerateConnectionString(product);
                        content = contentRepository.ExecuteMSSQLQuery(connectionString, contentQuery);

                        if (content == null)
                        {
                            Logger.LogRelativeMessage("Content Not Found for: " + product.ProductName + " " + product.DBServerType, true); // Log the exception
                        }

                        return content;

                    }



                    else if (product.DBServerType == "MySQL")
                    {

                        string connectionString = GenerateConnectionString(product);
                        content = contentRepository.ExecuteMYSQLQuery(connectionString, contentQuery);

                        if (content == null)
                        {
                            Logger.LogRelativeMessage("Content Not Found for: " + product.ProductName + " " + product.DBServerType, true); // Log the exception
                        }

                        return content;

                    }

                    else
                    {
                        // No valid content query, return early or log a warning
                        Logger.LogRelativeMessage("No valid DB ServerType to handle: " + product.DBServerType, true); // Log the exception
                        return new List<Dictionary<string, object>>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> GetColumnNamesAsPlaceHolders(long productId, string contentQuery)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product product = ProductsBAL.ProductDetail(productId);

                    if (product == null)
                    {
                        // Product not found, return early or log a warning
                        Logger.LogRelativeMessage("Product not found for ID: " + productId, true); // Log the exception
                        return new List<string>();
                    }

                    // Determine the content query based on DBServertype
                    IRepository<List<Dictionary<string, object>>> contentRepository = new Repository<List<Dictionary<string, object>>>(uow.DataContext);
                    List<string> columnNames = new List<string>();

                    if (product.DBServerType == "MSSQL")
                    {
                        string connectionString = GenerateConnectionString(product);
                        columnNames = contentRepository.GetMSSQLColumnNames(connectionString, contentQuery);
                    }
                    else if (product.DBServerType == "MySQL")
                    {
                        string connectionString = GenerateConnectionString(product);
                        columnNames = contentRepository.GetMySQLColumnNames(connectionString, contentQuery);
                    }
                    else
                    {
                        // No valid content query, return early or log a warning
                        Logger.LogRelativeMessage("No valid DB ServerType to handle: " + product.DBServerType, true); // Log the exception
                        return new List<string>();
                    }

                    return columnNames;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<Product> lstSubscriberProducts()
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product E = new Product();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.Command, "SELECTSUBSCRIBERPRODUCTS");
                    IRepository<Product> oRepository = new Repository<Product>(uow.DataContext);

                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Product> lstSubscriberProducts(bool IsSendEmail = false)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Product E = new Product();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.IsSendEmail, IsSendEmail);
                    Filter.AddSqlParameters(() => E.Command, "SELECTSUBSCRIBERPRODUCTSFOREmail");
                    IRepository<Product> oRepository = new Repository<Product>(uow.DataContext);

                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ContentQuery ProductContentQuery(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ContentQuery C = new ContentQuery();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => C.ProductId, ProductId);
                    Filter.AddSqlParameters(() => C.Command, "GetInlineQuery");
                    IRepository<ContentQuery> oRepository = new Repository<ContentQuery>(uow.DataContext);

                    return oRepository.LoadSP(Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static BaseEntity AddEditSubsProduct(long? ProductId, string ProductName, string DBServer, string DBUserName, string DBPassword, string DBServerType, string DataBaseName, bool IsSendEmail)
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
                    Filter.AddSqlParameters(() => E.IsSendEmail, IsSendEmail);
                    if (ProductId > 0)
                    {
                        Filter.AddSqlParameters(() => E.Id, ProductId);
                        Filter.AddSqlParameters(() => E.Command, "UPDATEUPSUBSCRIBERPRODUCT");
                    }
                    else
                    {
                        Filter.AddSqlParameters(() => E.Command, "INSERTSUBSCRIBERPRODUCT");
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

        public static BaseEntity AddEditSubsProductEmailSetting(long? ProductId, string Subject, string Body, string Placeholders)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    Filters Filter = new Filters();
                    EmailSettings existingEmailSettings = SubscriberProductsBAL.SubscriberProductEmailSettings(ProductId);

                    if (existingEmailSettings != null)
                    {
                        // Update existing email settings
                        existingEmailSettings.Subject = Subject;
                        existingEmailSettings.Placeholders = Placeholders;
                        existingEmailSettings.Body = Body;



                        Filter.AddSqlParameters(() => existingEmailSettings.ProductId, ProductId);
                        Filter.AddSqlParameters(() => existingEmailSettings.Subject, Subject);
                        Filter.AddSqlParameters(() => existingEmailSettings.Placeholders, Placeholders);
                        Filter.AddSqlParameters(() => existingEmailSettings.Body, Body);
                        Filter.AddSqlParameters(() => existingEmailSettings.Command, "UpdateProductEmailSetting");
                    }
                    else
                    {
                        // Insert new email settings
                        EmailSettings newEmailSettings = new EmailSettings
                        {
                            ProductId = (long)ProductId,
                            Subject = Subject,
                            Body = Body
                        };

                        Filter.AddSqlParameters(() => newEmailSettings.ProductId, ProductId);
                        Filter.AddSqlParameters(() => newEmailSettings.Subject, Subject);
                        Filter.AddSqlParameters(() => newEmailSettings.Placeholders, Placeholders);
                        Filter.AddSqlParameters(() => newEmailSettings.Body, Body);
                        Filter.AddSqlParameters(() => newEmailSettings.Command, "InsertProductEmailSetting");
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



        public static BaseEntity AddEditInlineQuery(long? ProductId, string InlineQuery)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ContentQuery CQ = SubscriberProductsBAL.ProductContentQuery(ProductId); // Get the existing content query for the given ProductId
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => CQ.InlineQuery, InlineQuery);

                    if (CQ != null) // Check if content query already exists
                    {
                        Filter.AddSqlParameters(() => CQ.ProductId, ProductId); // Use the ProductId property
                        Filter.AddSqlParameters(() => CQ.Command, "UPDATEINLINEQUERY");
                    }
                    else
                    {
                        CQ = new ContentQuery { ProductId = (long)ProductId, InlineQuery = InlineQuery }; // Create a new content query
                        Filter.AddSqlParameters(() => CQ.ProductId, ProductId);
                        Filter.AddSqlParameters(() => CQ.Command, "INSERTINLINEQUERY");
                    }

                    IRepository<BaseEntity> oRepository = new Repository<BaseEntity>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetContentQuery", Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ProductSubscribers> lstSubscribers(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductSubscribers PU = new ProductSubscribers();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => PU.ProductId, ProductId);
                    Filter.AddSqlParameters(() => PU.Command, "SELECTALLACTIVE");
                    IRepository<ProductSubscribers> oRepository = new Repository<ProductSubscribers>(uow.DataContext);

                    return oRepository.LoadSP("sp_GetSubscribers", Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CheckExistingSubscribers(long ProductId, string Email)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductSubscribers PU = new ProductSubscribers();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => PU.ProductId, ProductId);
                    Filter.AddSqlParameters(() => PU.Email, Email);
                    Filter.AddSqlParameters(() => PU.Command, "CHECKSUBSCRIBER");
                    IRepository<ProductSubscribers> oRepository = new Repository<ProductSubscribers>(uow.DataContext);

                    List<ProductSubscribers> subscribers = oRepository.LoadSP("sp_GetSubscribers", Filter);
                    return subscribers.Count > 0;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static BaseEntity UnsubscribeSubscriber(string email, long? productId = null, long? subscriberId = null)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    ProductSubscribers PU = new ProductSubscribers();
                    Filters Filter = new Filters();

                    // Set IsActive to false for unsubscription
                    PU.IsActive = false;

                    if (subscriberId.HasValue)
                    {
                        Filter.AddSqlParameters(() => PU.Id, subscriberId.Value);
                        Filter.AddSqlParameters(() => PU.IsActive, false);
                        Filter.AddSqlParameters(() => PU.Command, "UNSUBSCRIBE");
                    }
                    else if (productId.HasValue)
                    {
                        Filter.AddSqlParameters(() => PU.ProductId, productId.Value);
                        Filter.AddSqlParameters(() => PU.Email, email);
                        Filter.AddSqlParameters(() => PU.IsActive, false);
                        Filter.AddSqlParameters(() => PU.Command, "UNSUBSCRIBE");
                    }
                    else
                    {
                        Filter.AddSqlParameters(() => PU.Email, email);
                        Filter.AddSqlParameters(() => PU.IsActive, false);
                        Filter.AddSqlParameters(() => PU.Command, "UNSUBSCRIBE");
                    }

                    IRepository<BaseEntity> oRepository = new Repository<BaseEntity>(uow.DataContext);
                    return oRepository.LoadSP("sp_GetSubscribers", Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogRelativeMessage($"Error unsubscribing subscriber: {ex.Message}", true);
                throw ex;
            }
        }

        public static EmailSettings SubscriberProductEmailSettings(long? ProductId)
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    EmailSettings E = new EmailSettings();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => E.ProductId, ProductId);
                    Filter.AddSqlParameters(() => E.Command, "GetProductEmailSetting");
                    IRepository<EmailSettings> oRepository = new Repository<EmailSettings>(uow.DataContext);

                    return oRepository.LoadSP(Filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Dictionary<string, object>> CleanContent(List<Dictionary<string, object>> content)
        {
            var modifiedContent = new List<Dictionary<string, object>>();

            foreach (var contentRow in content)
            {
                var modifiedRow = new Dictionary<string, object>();

                foreach (var columnName in contentRow.Keys)
                {
                    object fieldValueObj = contentRow[columnName];
                    string fieldValue;

                    // Preserve Unicode when converting to string
                    if (fieldValueObj == null || fieldValueObj == DBNull.Value)
                    {
                        fieldValue = string.Empty;
                    }
                    else if (fieldValueObj is string)
                    {
                        // Already a string, preserve it directly
                        fieldValue = (string)fieldValueObj;
                    }
                    else if (fieldValueObj is byte[])
                    {
                        // If it's a byte array, decode as UTF-8
                        fieldValue = Encoding.UTF8.GetString((byte[])fieldValueObj);
                    }
                    else
                    {
                        // For other types, convert to string (preserves Unicode if already correct)
                        fieldValue = Convert.ToString(fieldValueObj);
                    }

                    // Decode HTML entities
                    fieldValue = HttpUtility.HtmlDecode(fieldValue);

                    // Remove all HTML tags and store as plain text
                    fieldValue = Regex.Replace(fieldValue, "<[^>]+>", "");

                    // Add the modified field to the modified row
                    modifiedRow[columnName] = fieldValue;
                }

                // Add the modified row to the modified content
                modifiedContent.Add(modifiedRow);
            }

            return modifiedContent;
        }

        public static string GenerateConnectionString(Product product)
        {
            // Generate connection string based on DBServertype
            if (product.DBServerType == "MSSQL")
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = product.DBServer;
                builder.InitialCatalog = product.DataBaseName;
                builder.UserID = product.DBUserName;
                builder.Password = product.DBPassword;
                // Enable Unicode support for SQL Server
                builder.IntegratedSecurity = false;
                // SQL Server uses NVARCHAR for Unicode, connection string handles it by default
                // But we ensure proper encoding by not forcing any character set restrictions

                return builder.ConnectionString;
            }
            else if (product.DBServerType == "MySQL")
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = product.DBServer;
                builder.Port = 3306; // Set the port number here
                builder.Database = product.DataBaseName;
                builder.UserID = product.DBUserName;
                builder.Password = product.DBPassword;
                // Add UTF-8 support for MySQL to handle Unicode characters like Urdu
                // Use indexer to set charset as it's not a direct property
                builder["CharSet"] = "utf8mb4";

                return builder.ConnectionString;
            }
            else
            {
                // Handle other database types
                return string.Empty;
            }
        }

    }
}










/* public static List<Dictionary<string, object>> GetContentForProduct(long productId, string contentQuery)
 {
     try
     {
         using (IUnitOfWork uow = new UnitOfWork())
         {
             Product product = ProductsBAL.ProductDetail(productId);

             if (product == null)
             {
                 // Product not found, return early or log a warning
                 Logger.LogRelativeMessage("Product not found for ID: " + productId, true); // Log the exception
                 return new List<Dictionary<string, object>>();
             }


             // Determine the content query based on DBServertype

             IRepository<Dictionary<string, object>> contentRepository = new Repository<Dictionary<string, object>>(uow.DataContext);
             List<Dictionary<string, object>> contentList = new List<Dictionary<string, object>>();

             if (product.DBServerType == "MSSQL")
             {

                 string connectionString = GenerateConnectionString(product);
                 contentList = contentRepository.ExecuteMSSQLQuery(connectionString, contentQuery);



                 if (contentList == null)
                 {
                     Logger.LogRelativeMessage("Content Not Found for: " + product.ProductName + " " + product.DBServerType, true); // Log the exception
                 }

                 return contentList;

             }



             else if (product.DBServerType == "MYSQL")
             {

                 string connectionString = GenerateConnectionString(product);
                 contentList = contentRepository.ExecuteMYSQLQuery(connectionString, contentQuery);

                 if (contentList == null)
                 {
                     Logger.LogRelativeMessage("Content Not Found for: " + product.ProductName + " " + product.DBServerType, true); // Log the exception
                 }

                 return contentList;

             }

             else

             {
                 // No valid content query, return early or log a warning
                 Logger.LogRelativeMessage("No valid DB ServerType to handle: " + product.DBServerType, true); // Log the exception
                 return null;
             }
         }
     }
     catch (Exception ex)
     {
         throw ex;
     }
 }*/


/*public static List<ProductContent> lstUploadedContent(string DBServer, string DBName, string DBUserName, string DBUserPass)
    {
        try
        {
            string ConnString = "Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";Integrated Security=True;";
            using (IUnitOfWork uow = new UnitOfWork(ConnString, "Yes"))
            {
                string inlineQuery = "SELECT * FROM tblContent WHERE CONVERT(date, RecCreated) = CONVERT(date, @TodayDate)";

                Filters filter = new Filters();
                filter.AddSqlParameters(() => new ProductContent().RecCreatedDt, DateTime.Today); // Filter by today's date

                IRepository<ProductContent> repository = new Repository<ProductContent>(uow.DataContext);
                return repository.LoadSQLInlineQuery(inlineQuery, filter);
            }
        }
        catch (Exception ex)
        {
            Logger.LogRelativeMessage("lstUploadedContent Exception:: " + ex.Message, true);
            return new List<ProductContent>();
        }
    }
}*/