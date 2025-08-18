using ProyectoProgramacion.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoProgramacion.Controllers
{
    public class ApartamentoController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

        // Helpers de sesión
        private bool IsLogged() => Session["IdUsuario"] != null;
        private int Rol() => IsLogged() ? Convert.ToInt32(Session["IdRol"]) : 0; // 1 = Admin, 2 = Usuario
        private bool IsAdmin() => Rol() == 1;

        // ==============================
        // ADMIN: CONSULTAR APARTAMENTOS
        // ==============================
        public ActionResult ConsultarApartamento()
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var apartamentos = db.Apartamento
                                 .Include(a => a.Edificio)
                                 .Include(a => a.FotoApartamento)
                                 .ToList();
            return View(apartamentos);
        }

        // ==============================
        // ADMIN: REGISTRAR APARTAMENTO
        // ==============================
        [HttpGet]
        public ActionResult RegistrarApartamento()
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            ViewBag.Edificios = new SelectList(db.Edificio.ToList(), "ID_Edificio", "Nombre");
            return View(new Apartamento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarApartamento(Apartamento apto, IEnumerable<HttpPostedFileBase> Fotos)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                try
                {
                    db.Apartamento.Add(apto);
                    db.SaveChanges();

                    // Carpeta para fotos de apartamentos
                    string carpeta = Server.MapPath("~/Apartamentos");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    if (Fotos != null)
                    {
                        int indice = 1;
                        foreach (var foto in Fotos)
                        {
                            if (foto != null && foto.ContentLength > 0)
                            {
                                string ext = Path.GetExtension(foto.FileName);
                                string nombreArchivo = $"{apto.ID_Apartamento}_{indice}{ext}";
                                string ruta = Path.Combine(carpeta, nombreArchivo);
                                foto.SaveAs(ruta);

                                // Guardar en tabla FotoApartamento
                                db.FotoApartamento.Add(new FotoApartamento
                                {
                                    ID_Apartamento = apto.ID_Apartamento,
                                    UrlFoto = "/Apartamentos/" + nombreArchivo
                                });

                                indice++;
                            }
                        }
                        db.SaveChanges();
                    }

                    return RedirectToAction("ConsultarApartamento");
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = "Error al registrar apartamento: " + ex.Message;
                }
            }

            ViewBag.Edificios = new SelectList(db.Edificio.ToList(), "ID_Edificio", "Nombre", apto.ID_Edificio);
            return View(apto);
        }

        // ==============================
        // ADMIN: ACTUALIZAR APARTAMENTO
        // ==============================
        [HttpGet]
        public ActionResult ActualizarApartamento(int id)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var apto = db.Apartamento
                         .Include(a => a.FotoApartamento)
                         .FirstOrDefault(a => a.ID_Apartamento == id);

            if (apto == null) return RedirectToAction("ConsultarApartamento");

            ViewBag.Edificios = new SelectList(db.Edificio.ToList(), "ID_Edificio", "Nombre", apto.ID_Edificio);
            return View(apto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarApartamento(Apartamento apto, IEnumerable<HttpPostedFileBase> Fotos)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                try
                {
                    var entidad = db.Apartamento.FirstOrDefault(a => a.ID_Apartamento == apto.ID_Apartamento);
                    if (entidad == null)
                    {
                        ViewBag.Mensaje = "El apartamento no existe.";
                        ViewBag.Edificios = new SelectList(db.Edificio.ToList(), "ID_Edificio", "Nombre", apto.ID_Edificio);
                        return View(apto);
                    }

                    // Actualizar campos
                    entidad.Codigo_Apartamento = apto.Codigo_Apartamento;
                    entidad.ID_Edificio = apto.ID_Edificio;
                    entidad.Piso = apto.Piso;
                    entidad.Metros_Cuadrados = apto.Metros_Cuadrados;
                    entidad.Cantidad_Habitantes = apto.Cantidad_Habitantes;
                    entidad.Cant_Sanitarios = apto.Cant_Sanitarios;
                    entidad.Disponible = apto.Disponible;

                    // Guardar nuevas fotos (si vienen)
                    string carpeta = Server.MapPath("~/Apartamentos");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    if (Fotos != null)
                    {
                        int indice = db.FotoApartamento.Count(f => f.ID_Apartamento == apto.ID_Apartamento) + 1;

                        foreach (var foto in Fotos)
                        {
                            if (foto != null && foto.ContentLength > 0)
                            {
                                string ext = Path.GetExtension(foto.FileName);
                                string nombreArchivo = $"{apto.ID_Apartamento}_{indice}{ext}";
                                string ruta = Path.Combine(carpeta, nombreArchivo);
                                foto.SaveAs(ruta);

                                db.FotoApartamento.Add(new FotoApartamento
                                {
                                    ID_Apartamento = apto.ID_Apartamento,
                                    UrlFoto = "/Apartamentos/" + nombreArchivo
                                });

                                indice++;
                            }
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction("ConsultarApartamento");
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = "Error al actualizar apartamento: " + ex.Message;
                }
            }

            ViewBag.Edificios = new SelectList(db.Edificio.ToList(), "ID_Edificio", "Nombre", apto.ID_Edificio);
            return View(apto);
        }

      










        public ActionResult Details(int id)
        {
            var apto = db.Apartamento
                         .Include("Edificio")
                         .Include("FotoApartamento")
                         .FirstOrDefault(a => a.ID_Apartamento == id);

            if (apto == null) return HttpNotFound();

            return View(apto);
        }


       
        // Lista de apartamentos disponibles
        public ActionResult Disponibles()
        {
            var disponibles = db.Apartamento
                                .Include(a => a.Edificio)
                                .Include(a => a.FotoApartamento)
                                .Where(a => a.Disponible == true)
                                .OrderBy(a => a.Codigo_Apartamento)
                                .ToList();

            return View(disponibles);
        }

        // GET: /Apartamento/Detalle/5
        
        public ActionResult Detalle(int id)
        {
            var apto = db.Apartamento
                         .Include(a => a.Edificio)
                         .Include(a => a.FotoApartamento)
                         .FirstOrDefault(a => a.ID_Apartamento == id);

            if (apto == null) return HttpNotFound();
            return View(apto);
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
