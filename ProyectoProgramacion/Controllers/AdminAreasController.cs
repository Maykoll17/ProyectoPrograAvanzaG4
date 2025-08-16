using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoProgramacion.Models.EF;

namespace ProyectoProgramacion.Controllers
{
    public class AdminAreasController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

        // ========= Helpers =========
        private string EnsureUploadsFolderAndGetPath()
        {
            var serverFolder = Server.MapPath("~/Uploads/Areas");
            if (!Directory.Exists(serverFolder)) Directory.CreateDirectory(serverFolder);
            return serverFolder;
        }

        private string SaveImage(HttpPostedFileBase imagen)
        {
            if (imagen == null || imagen.ContentLength == 0) return null;

            // Validación sencilla de tipo
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(imagen.FileName)?.ToLowerInvariant();
            if (!allowed.Contains(ext)) return null;

            var folder = EnsureUploadsFolderAndGetPath();
            var fileName = $"area_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(folder, fileName);
            imagen.SaveAs(fullPath);

            // Ruta relativa para usar en <img src="...">
            return Url.Content("~/Uploads/Areas/" + fileName);
        }

        private void SplitHorario(string horario, out string ini, out string fin)
        {
            ini = fin = string.Empty;
            if (string.IsNullOrWhiteSpace(horario)) return;

            // Formato esperado "HH:mm - HH:mm"
            var parts = horario.Split('-');
            if (parts.Length == 2)
            {
                ini = parts[0].Trim();
                fin = parts[1].Trim();
            }
        }

        // ========= LISTA =========
        // GET: AdminAreas
        public ActionResult Index()
        {
            var lista = db.AreaRecreativa
                          .OrderBy(a => a.Nombre)
                          .ToList();
            return View(lista);
        }

        // ========= CREAR =========
        // GET: AdminAreas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminAreas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaRecreativa area, HttpPostedFileBase imagen, string HoraInicio, string HoraFin)
        {
            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(area.Nombre))
                ModelState.AddModelError("Nombre", "El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(HoraInicio) || string.IsNullOrWhiteSpace(HoraFin))
                ModelState.AddModelError("Horario", "Debe seleccionar hora de inicio y fin.");

            // Construir horario normalizado
            area.Horario = (!string.IsNullOrWhiteSpace(HoraInicio) && !string.IsNullOrWhiteSpace(HoraFin))
                ? $"{HoraInicio} - {HoraFin}"
                : area.Horario;

            // Guardar imagen (opcional)
            var url = SaveImage(imagen);
            if (!string.IsNullOrWhiteSpace(url))
                area.ImageUrl = url;

            if (!ModelState.IsValid)
                return View(area);

            db.AreaRecreativa.Add(area);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ========= EDITAR =========
        // GET: AdminAreas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var area = db.AreaRecreativa.Find(id);
            if (area == null) return HttpNotFound();

            // Pre-cargar horas para la vista (si quieres usarlas con <input type="time">)
            SplitHorario(area.Horario, out var ini, out var fin);
            ViewBag.HoraInicio = ini;  // en la vista: value="@ViewBag.HoraInicio"
            ViewBag.HoraFin = fin;     // en la vista: value="@ViewBag.HoraFin"

            return View(area);
        }

        // POST: AdminAreas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AreaRecreativa area, HttpPostedFileBase nuevaImagen, string HoraInicio, string HoraFin)
        {
            if (area == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            
            if (string.IsNullOrWhiteSpace(area.Nombre))
                ModelState.AddModelError("Nombre", "El nombre es obligatorio.");

            if (!string.IsNullOrWhiteSpace(HoraInicio) && !string.IsNullOrWhiteSpace(HoraFin))
                area.Horario = $"{HoraInicio} - {HoraFin}";

            
            var nuevaUrl = SaveImage(nuevaImagen);
            if (!string.IsNullOrWhiteSpace(nuevaUrl))
                area.ImageUrl = nuevaUrl; 

            if (!ModelState.IsValid)
            {
                
                SplitHorario(area.Horario, out var ini, out var fin);
                ViewBag.HoraInicio = ini;
                ViewBag.HoraFin = fin;
                return View(area);
            }

            db.Entry(area).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

     
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var area = db.AreaRecreativa.Find(id);
            if (area == null) return HttpNotFound();

            return View(area);
        }

        // POST: AdminAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var area = db.AreaRecreativa.Find(id);
            if (area == null) return HttpNotFound();

            // (Opcional) Eliminar archivo físico si existe
            if (!string.IsNullOrWhiteSpace(area.ImageUrl))
            {
                var physical = Server.MapPath(area.ImageUrl);
                if (System.IO.File.Exists(physical))
                {
                    try { System.IO.File.Delete(physical); } catch { /* ignorar */ }
                }
            }

            db.AreaRecreativa.Remove(area);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

     
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
