using Microsoft.AspNetCore.Mvc;
using RentalPropertyManagement.Models;


namespace RentalPropertyManagement.Controllers
{
    public class UserRegistrationController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Call stored procedure to save data
                return RedirectToAction("RegistrationSuccess");
            }
            return View(model);
        }

        public ActionResult RegistrationSuccess()
        {
            return View();
        }
    }
}
