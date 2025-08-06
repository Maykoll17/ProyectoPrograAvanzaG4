using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoProgramacion.Models.EF;

namespace ProyectoProgramacion.Controllers
{
    public class AdminAreasController : Controller
    {
        private SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

        // GET: AdminAreas
        public ActionResult Index()
        {
            var lista = db.AreaRecreativa.ToList();
            return View(lista);
        }

        // GET: AdminAreas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminAreas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaRecreativa area)
        {
            if (ModelState.IsValid)
            {
                db.AreaRecreativa.Add(area);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(area);
        }

        // GET: AdminAreas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var area = db.AreaRecreativa.Find(id);
            if (area == null)
                return HttpNotFound();

            return View(area);
        }

        // POST: AdminAreas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AreaRecreativa area)
        {
            if (ModelState.IsValid)
            {
                db.Entry(area).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(area);
        }

        // GET: AdminAreas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var area = db.AreaRecreativa.Find(id);
            if (area == null)
                return HttpNotFound();

            return View(area);
        }

        // POST: AdminAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var area = db.AreaRecreativa.Find(id);
            db.AreaRecreativa.Remove(area);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}


