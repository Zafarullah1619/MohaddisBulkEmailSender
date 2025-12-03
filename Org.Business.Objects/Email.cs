using System;
using Org.Utils;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Org.Business.Objects
{
    public class Email : BaseEntity
    {
        [DataField(Type = DbType.String)]
        public string VendorUserId { get; set; }

        [DataField(Type = DbType.String)]
        public string AffiliateUserId { get; set; }

        [DataField(Type = DbType.Int64)]
        public long ProductId { get; set; }

        [DataField(Type = DbType.String)]
        public string CustomerUserId { get; set; }

        [DataField(Type = DbType.Int64)]
        public long TransactionId { get; set; }

        [DataField(Type = DbType.Int64)]
        public long PreapprovedTransactionId { get; set; }

        [DataField(Type = DbType.String)]
        public string TrackingId { get; set; }

        [DataField(Type = DbType.String)]
        public string PayKey { get; set; }

        [DataField(Type = DbType.String)]
        public string FromEmail { get; set; }
        [DataField(Type = DbType.String)]
        public string FromName { get; set; }
        [DataField(Type = DbType.String)]
        public string ToEmail { get; set; }

        [DataField(Type = DbType.String)]
        public string ToName { get; set; }

        [DataField(Type = DbType.String)]
        public string Subject { get; set; }

        [DataField(Type = DbType.String)]
        public string EmailText { get; set; }
        
        [DataField(Type = DbType.Int64)]
        public long AttemptsMade { get; set; }

        [DataField(Type = DbType.Int64)]
        public long EmailTemplateId { get; set; }

        [DataField(Type = DbType.Int64)]
        public long ExternalTemplateId { get; set; }

        [DataField(Type = DbType.String)]
        public string EmailEvent { get; set; }  

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset FirstSentDt { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset LastSentDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecCreatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset RecCreatedDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecUpdatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset RecUpdatedDt { get; set; }

        [NonDBField(Type = DbType.String)]
        public string CallbackUrl { get; set; }
        
    }

    public class EmailViewModel
    {
        public Product Product { get; set; }

        public List<Product> ProductList { get; set; }

        [Required(ErrorMessage = "At least one product must be selected")]
        public long?[] SelectedProductIds { get; set; }

        public SmtpConfiguration EmailSmtpConfiguration { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, ErrorMessage = "Subject cannot exceed 100 characters")]
        public string EmailSubject { get; set; }

        [Required(ErrorMessage = "Email Body is required")]
        [AllowHtml]
        public string Body { get; set; }
    }

    [StoreProcedure("sp_GetSaleEmails")]
    public class SalesEmail : BaseEntity
    {
        [DataField(Type = DbType.String)]
        public string VendorUserId { get; set; }

        [DataField(Type = DbType.String)]
        public string AffiliateUserId { get; set; }

        [DataField(Type = DbType.Int64)]
        public long ProductId { get; set; }

        [DataField(Type = DbType.String)]
        public string CustomerUserId { get; set; }

        [DataField(Type = DbType.Int64)]
        public long TransactionId { get; set; }

        [DataField(Type = DbType.Int64)]
        public long PreapprovedTransactionId { get; set; }

        [DataField(Type = DbType.String)]
        public string TrackingId { get; set; }

        [DataField(Type = DbType.String)]
        public string PayKey { get; set; }

        [DataField(Type = DbType.String)]
        public string FromEmail { get; set; }
        [DataField(Type = DbType.String)]
        public string FromName { get; set; }
        [DataField(Type = DbType.String)]
        public string ToEmail { get; set; }

        [DataField(Type = DbType.String)]
        public string ToName { get; set; }

        [DataField(Type = DbType.String)]
        public string Subject { get; set; }

        [DataField(Type = DbType.String)]
        public string EmailText { get; set; }

        [DataField(Type = DbType.Int64)]
        public long AttemptsMade { get; set; }

        [DataField(Type = DbType.Int64)]
        public long EmailTemplateId { get; set; }
        
        [DataField(Type = DbType.String)]
        public string EmailEvent { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset FirstSentDt { get; set; }
        
        [DataField(Type = DbType.DateTimeOffset)]
        public DateTimeOffset RecCreatedDt { get; set; }
        
        [NonDBField(Type = DbType.String)]
        public string EmailTemplateCodes { get; set; }
        [NonDBField(Type = DbType.Boolean)]
        public bool RegisteredCustomer { get; set; }
        [NonDBField(Type = DbType.String)]
        public string CustomerPassword { get; set; }
        [NonDBField(Type = DbType.String)]
        public string Command { get; set; }

    }
}
