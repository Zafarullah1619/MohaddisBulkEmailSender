using Org.Business.Methods;
using Org.Business.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASE.API.Controllers
{
    public class SubscriberAPIController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Add(ProductSubscribers ps)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(string.Join("; ", errors));
            }

            var existingSubscriber = SubscriberProductsBAL.CheckExistingSubscribers(ps.ProductId, ps.Email);

            if (existingSubscriber)
            {
                // Subscriber with the same email and product ID already exists, return a conflict response
                return Content(HttpStatusCode.Conflict, "Subscriber with the same email and product ID already exists.");
            }
            else 
            {
                var added = SubscriberProductsBAL.AddSubscriber(ps.Email, ps.Name, ps.ProductId);
                return Ok(added);
            }

            
        }
    }
}
