using Microsoft.AspNetCore.Mvc;
using RentalPropertyManagement.Models;


namespace RentalPropertyManagement.Controllers
{
    public class ApplicationRequestController : Controller
    {
        [HttpGet]
        public ActionResult Request()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Request(ApplicationRequest model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Call stored procedure to save data
                return Content("Solicitud enviada con éxito."); // You can redirect to a success page too
            }
            return View(model);
        }
    }
}
