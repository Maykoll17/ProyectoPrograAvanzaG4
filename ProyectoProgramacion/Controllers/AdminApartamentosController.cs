using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoProgramacion.Models.EF;

namespace ProyectoProgramacion.Controllers
{
    [Authorize] 
    public class AdminApartamentosController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

     
        public ActionResult Index()
        {
            var lista = db.Apartamento
                          .OrderBy(a => a.Codigo_Apartamento)
                          .ToList();
            return View(lista);
        }


        public ActionResult Create()
        {
            ViewBag.ID_Edificio = new SelectList(db.Edificio.OrderBy(e => e.Nombre), "ID_Edificio", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Apartamento model)
        {
            if (ModelState.IsValid)
            {
                db.Apartamento.Add(model);
                db.SaveChanges();
                TempData["ok"] = "Apartamento creado correctamente.";
                return RedirectToAction("Index");
            }
            ViewBag.ID_Edificio = new SelectList(db.Edificio.OrderBy(e => e.Nombre), "ID_Edificio", "Nombre", model.ID_Edificio);
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var apto = db.Apartamento.Find(id);
            if (apto == null) return HttpNotFound();

            ViewBag.ID_Edificio = new SelectList(db.Edificio.OrderBy(e => e.Nombre), "ID_Edificio", "Nombre", apto.ID_Edificio);
            return View(apto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Apartamento model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ok"] = "Cambios guardados.";
                return RedirectToAction("Index");
            }
            ViewBag.ID_Edificio = new SelectList(db.Edificio.OrderBy(e => e.Nombre), "ID_Edificio", "Nombre", model.ID_Edificio);
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var apto = db.Apartamento.Find(id);
            if (apto == null) return HttpNotFound();
            return View(apto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var apto = db.Apartamento.Find(id);
            db.Apartamento.Remove(apto);
            db.SaveChanges();
            TempData["ok"] = "Apartamento eliminado.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}