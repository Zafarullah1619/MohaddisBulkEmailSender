using Newtonsoft.Json;
using Org.Business.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASE.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                var jsonString = "[{\"Type\":\"Bought\",\"Upsells\":[\"18\",\"19\"]},{\"Operator\":\"∩\",\"Type\":\"NotBought\",\"Upsells\":[\"19\",\"21\",\"22\"]}]";
                var result = JsonConvert.DeserializeObject<List<RuleViewModel>>(jsonString);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index", "SubscriberProduct");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}