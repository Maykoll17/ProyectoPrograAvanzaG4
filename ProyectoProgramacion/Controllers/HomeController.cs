using ProyectoProgramacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoProgramacion.Controllers
{
    public class HomeController : Controller
    {

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        #endregion


        #region Registro

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        #endregion


        #region Solicitud

        [HttpGet]
        public ActionResult Solicitud()
        {
            return View();
        }

        #endregion


    }


}