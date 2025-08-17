using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoProgramacion.Models.EF;

namespace ProyectoProgramacion.Controllers
{
    [Authorize]
    public class AdminResidentesController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

     
        public ActionResult Index()
        {
            var lista = db.Usuario.Where(u => u.IdRol == 2)
                                  .OrderBy(u => u.Nombre)
                                  .ToList();
            return View(lista);
        }

        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario model)
        {
            if (ModelState.IsValid)
            {
                model.IdRol = 2;
                db.Usuario.Add(model);
                db.SaveChanges();
                TempData["ok"] = "Residente creado.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var u = db.Usuario.Find(id);
            if (u == null || u.IdRol != 2) return HttpNotFound();
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario model)
        {
            if (ModelState.IsValid)
            {
                model.IdRol = 2;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ok"] = "Cambios guardados.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var u = db.Usuario.Find(id);
            if (u == null || u.IdRol != 2) return HttpNotFound();
            return View(u);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var u = db.Usuario.Find(id);
            db.Usuario.Remove(u);
            db.SaveChanges();
            TempData["ok"] = "Residente eliminado.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}