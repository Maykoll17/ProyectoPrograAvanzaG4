using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoProgramacion.Controllers
{
    public class AdminController : Controller
    {
        // Apartamentos
        public ActionResult ApartamentosList()
        {
            return View();
        }

        public ActionResult ApartamentoForm(int? id)
        {
            ViewBag.Id = id;
            return View();
        }

        // Residentes
        public ActionResult ResidentesList()
        {
            return View();
        }

        public ActionResult ResidenteForm(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
