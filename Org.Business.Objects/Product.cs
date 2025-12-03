using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    [StoreProcedure("sp_GetProducts")]
    public class Product : BaseEntity
    {
        [DataField(Type = DbType.String)]
        public string ProductName { get; set; }

        [DataField(Type = DbType.String)]
        public string DataBaseName { get; set; }

        [DataField(Type = DbType.String)]
        public string DBServer { get; set; }

        [DataField(Type = DbType.String)]
        public string DBUserName { get; set; }

        [DataField(Type = DbType.String)]
        public string DBPassword { get; set; }

        [DataField(Type = DbType.String)]
        public string DBServerType { get; set; }

        [DataField(Type = DbType.Int32)]
        public long ProductCategory { get; set; }

        [DataField(Type = DbType.String)]
        public string RecCreatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecCreatedDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecUpdatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecUpdatedDt { get; set; }
        
        [DataField(Type = DbType.Boolean)]
        public bool IsSendEmail { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Upsells { get; set; }

        [NonDBField(Type = DbType.String)]
        public string ContentQuery { get; set; }

    }

    [StoreProcedure("[sp_GetContentQuery]")]
    public class ContentQuery : BaseEntity
    {

        [DataField(Type = DbType.Int64)]
        public int Id { get; set; }

        [DataField(Type = DbType.String)]
        public string InlineQuery { get; set; }

        [DataField(Type = DbType.Int32)]
        public long ProductId { get; set; }

    }

    [StoreProcedure("[sp_GetProducts]")]
    public class EmailSettings : BaseEntity
    {
        [DataField(Type = DbType.Int64)]
        public int Id { get; set; }

        [DataField(Type = DbType.Int32)]
        public long ProductId { get; set; }

        [DataField(Type = DbType.String)]
        public string Subject { get; set; }

        [DataField(Type = DbType.String)]
        public string Placeholders { get; set; }


        [DataField(Type = DbType.String)]
        public string Body { get; set; }

    }

    public class ProductViewModal
    {
        public Product Product { get; set; }

        public List<ProductRule> lstProductRules { get; set; }
        
        public List<ProductUpsell> lstProductUpsells { get; set; }
        
        public List<ProductCategory> lstProductCategory { get; set; }
        
        public long ProductCategory { get; set; }
        
        public SmtpConfiguration SmtpConfiguration { get; set; }

        public ContentQuery ContentQuery { get; set; }
        
        public long? RuleNumber { get; set; }
        
        public bool? FromScratch { get; set; }

        public EmailSettings emailSettings { get; set; }

        public List<string> PlaceHolders { get; set; }

        public List<string> SelectPlaceholders { get; set; }
    }

    [StoreProcedure("sp_GetProducts")]
    public class ProductRule : BaseEntity
    {
        [DataField(Type = DbType.Int32)]
        public long ProductId { get; set; }

        [DataField(Type = DbType.String)]
        public string Rule { get; set; }

        [DataField(Type = DbType.String)]
        public string RulesJson { get; set; }

        [DataField(Type = DbType.String)]
        public string Subject { get; set; }

        [DataField(Type = DbType.String)]
        public string EmailContent { get; set; }

        [DataField(Type = DbType.String)]
        public string RecCreatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecCreatedDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecUpdatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecUpdatedDt { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsActive { get; set; }

        [NonDBField(Type = DbType.String)]
        public string IDs { get; set; }
    }

    [StoreProcedure("sp_GetProducts")]
    public class ProductUpsell : BaseEntity
    {
        [DataField(Type = DbType.Int32)]
        public long ProductId { get; set; }

        [DataField(Type = DbType.String)]
        public string UpsellName { get; set; }

        [DataField(Type = DbType.String)]
        public string Upsell { get; set; }

        [DataField(Type = DbType.String)]
        public string RecCreatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecCreatedDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecUpdatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecUpdatedDt { get; set; }

        [NonDBField(Type = DbType.String)]
        public string Upsells { get; set; }

        [NonDBField(Type = DbType.String)]
        public string UpsellIDs { get; set; }
    }

    [StoreProcedure("sp_GetProductCategory")]
    public class ProductCategory : BaseEntity
    {
        [DataField(Type = DbType.Int64)]
        public long Id { get; set; }

        [DataField(Type = DbType.String)]
        public string CategoryName { get; set; }

        [DataField(Type = DbType.String)]
        public string RecCreatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecCreatedDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecUpdatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset? RecUpdatedDt { get; set; }
    }

    public class ProductRulesViewModel
    {
        public long RuleId { get; set; }
        public string[] RulesJson { get; set; }
        public string Rules { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public long? ProductId { get; set; }
        public bool IsActive { get; set; }
    }

    public class RuleViewModel
    {
        public string[] Upsells { get; set; }
        public string Type { get; set; }
        public string Operator { get; set; }
    }

    public class EmailData
    {
        public string ToEmail { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public List<string> EmailBodies { get; set; }

        public EmailData()
        {
            EmailBodies = new List<string>();
        }
    }

}
