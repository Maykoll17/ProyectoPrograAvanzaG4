using ProyectoProgramacion.Models.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoProgramacion.Controllers
{
    public class ApartamentoController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

        private bool IsLogged() => Session["IdUsuario"] != null;
        private int Rol() => IsLogged() ? Convert.ToInt32(Session["IdRol"]) : 0;
        private bool IsAdmin() => Rol() == 1;

        // ==============================
        // CONSULTAR APARTAMENTOS
        // ==============================
        public ActionResult ConsultarApartamento()
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var apartamentos = db.Apartamento.ToList();
            return View(apartamentos);
        }

        // ==============================
        // REGISTRAR APARTAMENTO
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

                    // Carpeta apartamentos
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
                                string ruta = Path.Combine(carpeta, $"{apto.ID_Apartamento}_{indice}{ext}");
                                foto.SaveAs(ruta);

                                // Guardar en tabla FotoApartamento
                                db.FotoApartamento.Add(new FotoApartamento
                                {
                                    ID_Apartamento = apto.ID_Apartamento,
                                    UrlFoto = "/Apartamentos/" + $"{apto.ID_Apartamento}_{indice}{ext}"
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
        // ACTUALIZAR APARTAMENTO
        // ==============================
        [HttpGet]
        public ActionResult ActualizarApartamento(int id)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var apto = db.Apartamento.Find(id);
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
                        ViewBag.Mensaje = "Apartamento no existe.";
                        return View(apto);
                    }

                    entidad.Codigo_Apartamento = apto.Codigo_Apartamento;
                    entidad.ID_Edificio = apto.ID_Edificio;
                    entidad.Piso = apto.Piso;
                    entidad.Metros_Cuadrados = apto.Metros_Cuadrados;
                    entidad.Cantidad_Habitantes = apto.Cantidad_Habitantes;
                    entidad.Cant_Sanitarios = apto.Cant_Sanitarios;
                    entidad.Disponible = apto.Disponible;

                    // Carpeta apartamentos
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
                                string ruta = Path.Combine(carpeta, $"{apto.ID_Apartamento}_{indice}{ext}");
                                foto.SaveAs(ruta);

                                db.FotoApartamento.Add(new FotoApartamento
                                {
                                    ID_Apartamento = apto.ID_Apartamento,
                                    UrlFoto = "/Apartamentos/" + $"{apto.ID_Apartamento}_{indice}{ext}"
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
    }
}
