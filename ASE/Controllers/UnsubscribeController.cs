using ASE.Utility;
using Org.Business.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASE.Controllers
{
    public class UnsubscribeController : Controller
    {
        // GET: Unsubscribe
        public ActionResult Index(string Email)
        {
            Email = StringHelper.DecryptURL(Email);

            PushNotificationsBAL.MarkAsUnsubscribed(Email);

            return View();
        }
    }
}