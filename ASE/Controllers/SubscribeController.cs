using Org.Business.Methods;
using Org.Business.Objects;
using Org.Utils;
using System;
using System.Web.Mvc;

namespace ASE.Controllers
{
    [AllowAnonymous]
    public class SubscribeController : Controller
    {
        // GET: Subscribe/form/{productId}
        public ActionResult Form(long? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                ViewBag.ErrorMessage = "Invalid product ID";
                return View();
            }

            // Verify product exists
            var product = ProductsBAL.ProductDetail(id.Value);
            if (product == null)
            {
                ViewBag.ErrorMessage = "Product not found";
                return View();
            }

            ViewBag.ProductId = id.Value;
            ViewBag.ProductName = product.ProductName;
            return View();
        }

        // POST: Subscribe/form/{productId}
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Form(long? id, string email, string name = "")
        {
            // Enable CORS for iframe embedding
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
            Response.AppendHeader("Access-Control-Allow-Headers", "Content-Type");

            try
            {
                if (!id.HasValue || id.Value <= 0)
                {
                    return Json(new { success = false, message = "Invalid product ID" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(email))
                {
                    return Json(new { success = false, message = "Email is required" }, JsonRequestBehavior.AllowGet);
                }

                // Validate email format - RFC 5322 compliant
                var emailRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", 
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                
                if (!emailRegex.IsMatch(email))
                {
                    return Json(new { success = false, message = "Invalid email format. Please enter a valid email address." }, JsonRequestBehavior.AllowGet);
                }

                // Check if subscriber already exists
                bool existingSubscriber = SubscriberProductsBAL.CheckExistingSubscribers(id.Value, email);
                if (existingSubscriber)
                {
                    return Json(new { success = false, message = "This email is already subscribed" }, JsonRequestBehavior.AllowGet);
                }

                // Use empty string if name is not provided (don't use email as name)
                string subscriberName = string.IsNullOrWhiteSpace(name) ? string.Empty : name;

                // Add subscriber
                var result = SubscriberProductsBAL.AddSubscriber(email, subscriberName, id.Value);

                if (result != null && result.Id > 0)
                {
                    Logger.LogRelativeMessage($"New subscriber added: {email} for Product ID: {id.Value}", true);
                    return Json(new { success = true, message = "Successfully subscribed! Thank you." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Failed to subscribe. Please try again." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.LogRelativeMessage($"Error subscribing: {ex.Message}", true);
                return Json(new { success = false, message = "An error occurred. Please try again later." }, JsonRequestBehavior.AllowGet);
            }
        }

        // Handle OPTIONS for CORS preflight
        [HttpOptions]
        [AllowAnonymous]
        public ActionResult Form()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
            Response.AppendHeader("Access-Control-Allow-Headers", "Content-Type");
            return new HttpStatusCodeResult(200);
        }
    }
}
