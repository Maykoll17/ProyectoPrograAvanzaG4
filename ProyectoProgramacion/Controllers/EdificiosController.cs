using ProyectoProgramacion.Models.EF;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoProgramacion.Controllers
{
    public class EdificiosController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

        private bool IsLogged() => Session["IdUsuario"] != null;
        private int Rol() => IsLogged() ? Convert.ToInt32(Session["IdRol"]) : 0;
        private bool IsAdmin() => Rol() == 1;

        public ActionResult ConsultarEdificios()
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var data = db.Edificio.ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult RegistrarEdificio()
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);
            return View(new Edificio());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarEdificio(Edificio edificio, HttpPostedFileBase FotoEdificio)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                try
                {
                    edificio.Foto = "";
                    db.Edificio.Add(edificio);
                    db.SaveChanges();

                    if (FotoEdificio != null && FotoEdificio.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(FotoEdificio.FileName);
                        string carpeta = Server.MapPath("~/Edificios");

                        if (!Directory.Exists(carpeta))
                            Directory.CreateDirectory(carpeta);

                        string ruta = Path.Combine(carpeta, edificio.ID_Edificio + extension);
                        FotoEdificio.SaveAs(ruta);

                        edificio.Foto = "/Edificios/" + edificio.ID_Edificio + extension;
                        db.SaveChanges();
                    }

                    return RedirectToAction("ConsultarEdificios");
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = "Error al registrar edificio: " + ex.Message;
                }
            }

            return View(edificio);
        }

        [HttpGet]
        public ActionResult ActualizarEdificio(long id)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var edificio = db.Edificio.Find(id);
            if (edificio == null)
                return RedirectToAction("ConsultarEdificios");

            return View(edificio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarEdificio(Edificio edificio, HttpPostedFileBase FotoEdificio)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                try
                {
                    var entidad = db.Edificio.FirstOrDefault(e => e.ID_Edificio == edificio.ID_Edificio);
                    if (entidad == null)
                    {
                        ViewBag.Mensaje = "El edificio no existe.";
                        return View(edificio);
                    }

                    entidad.Nombre = edificio.Nombre;
                    entidad.Direccion = edificio.Direccion;
                    entidad.Cantidad_Pisos = edificio.Cantidad_Pisos;

                    if (FotoEdificio != null && FotoEdificio.ContentLength > 0)
                    {
                        if (!string.IsNullOrEmpty(entidad.Foto))
                        {
                            string rutaAnterior = Server.MapPath(entidad.Foto);
                            if (System.IO.File.Exists(rutaAnterior))
                                System.IO.File.Delete(rutaAnterior);
                        }

                        string extension = Path.GetExtension(FotoEdificio.FileName);
                        string carpeta = Server.MapPath("~/Edificios");
                        if (!Directory.Exists(carpeta))
                            Directory.CreateDirectory(carpeta);

                        string ruta = Path.Combine(carpeta, entidad.ID_Edificio + extension);
                        FotoEdificio.SaveAs(ruta);

                        entidad.Foto = "/Edificios/" + entidad.ID_Edificio + extension;
                    }

                    db.SaveChanges();
                    return RedirectToAction("ConsultarEdificios");
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = "Error al actualizar edificio: " + ex.Message;
                }
            }

            return View(edificio);
        }
    }
}
