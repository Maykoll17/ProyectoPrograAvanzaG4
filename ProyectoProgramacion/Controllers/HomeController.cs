using ProyectoProgramacion.Models;
using ProyectoProgramacion.Models.EF;
using ProyectoProgramacion.Services;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ProyectoProgramacion.Controllers
{
    public class HomeController : Controller
    {
        readonly Utilitarios service = new Utilitarios();

        #region Index (Login)

        [HttpGet]
        public ActionResult Index()
        {
            return View(new Autenticacion());
        }

        [HttpPost]
        public ActionResult Index(Autenticacion autenticacion)
        {
            using (var dbContext = new SistemaAlquilerEntities1())
            {
                var result = dbContext.Usuario
                    .FirstOrDefault(u => u.Correo == autenticacion.Correo
                                      && u.Contrasenna == autenticacion.Contrasenna
                                      );

                if (result != null)
                {
                    Session["IdUsuario"] = result.ID_Usuario;
                    Session["Nombre"] = result.Nombre;
                    Session["Correo"] = result.Correo;
                    Session["IdRol"] = result.IdRol;

                    if (result.IdRol == 1)
                    {
                        return RedirectToAction("Registro", "Home");
                    }
                    else if (result.IdRol == 2)
                    {
                        return RedirectToAction("Principal", "Home");
                    }

                }

                ViewBag.Mensaje = "Correo o contraseña incorrectos";
                return View(autenticacion);
            }
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

                    ViewBag.Mensaje = "No se pudo enviar el correo de recuperación";
                    return View(autenticacion);
                }

                ViewBag.Mensaje = "No se encontró el correo en el sistema";
                return View(autenticacion);
            }
        }

        #endregion

        #region Principal

        [FiltroSesion]
        [HttpGet]
        public ActionResult Principal()
        {
            return View();
        }

        #endregion

        #region CerrarSesion

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        #endregion

    }





}
