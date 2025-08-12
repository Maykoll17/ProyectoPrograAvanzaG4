using ProyectoProgramacion.Models;
using ProyectoProgramacion.Models.EF;
using ProyectoProgramacion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ProyectoProgramacion.Controllers
{
    public class HomeController : Controller
    {

        readonly Utilitarios service = new Utilitarios();


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

        [HttpPost]
        public ActionResult Registro(Autenticacion autenticacion)
        {
            using (var dbContext = new SistemaAlquilerEntities1())
            {

                var result = dbContext.sp_InsertUsuario(
                    autenticacion.Nombre,
                    autenticacion.Cedula,
                    autenticacion.Telefono,
                    autenticacion.Contrasenna,
                    autenticacion.Correo,
                    autenticacion.Fecha_Nacimiento
                    );

                if (result > 0)
                    return RedirectToAction("Index", "Home");

                ViewBag.Mensaje = "No se pudo registrar su información";
                return View();
            }
        }

        #endregion



        #region RecuperarContrasenna

        [HttpGet]
        public ActionResult RecuperarContrasenna()
        {
            return View(new Autenticacion());
        }

        [HttpPost]
        public ActionResult RecuperarContrasenna(Autenticacion autenticacion)
        {
            using (var dbContext = new SistemaAlquilerEntities1())
            {
                var result = dbContext.Usuario.FirstOrDefault(u => u.Correo == autenticacion.Correo);

                if (result != null)
                {
                    var Contrasenna = service.GenerarPassword();
                    result.Contrasenna = Contrasenna;
                    dbContext.SaveChanges();

                    StringBuilder mensaje = new StringBuilder();
                    mensaje.Append($"Estimado {result.Nombre}<br>");
                    mensaje.Append("Se ha generado una solicitud de recuperación de contraseña a su nombre.<br><br>");
                    mensaje.Append($"Su contraseña temporal es: {Contrasenna}<br><br>");
                    mensaje.Append("Procure realizar el cambio de su contraseña en cuanto ingrese al sistema.<br>");
                    mensaje.Append("Muchas gracias.");

                    if (service.EnviarCorreo(result.Correo, mensaje.ToString(), "Solicitud de acceso"))
                        return RedirectToAction("Index", "Home");

                    ViewBag.Mensaje = "No se pudo realizar la notificación de su acceso al sistema";
                    return View(autenticacion);
                }

                ViewBag.Mensaje = "No se pudo recuperar su acceso al sistema";
                return View(autenticacion);
            }
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