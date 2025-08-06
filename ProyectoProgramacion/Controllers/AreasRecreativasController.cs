using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoProgramacion.Models.EF;

namespace ProyectoProgramacion.Controllers
{
    public class AreasRecreativasController : Controller
    {
        private SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

        // GET: AreasRecreativas (vista pública)
        public ActionResult Index()
        {
            var lista = db.AreaRecreativa.ToList();
            return View(lista);
        }

        // GET: AreasRecreativas/Details/5 (ver más información)
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var area = db.AreaRecreativa.FirstOrDefault(a => a.ID_Area == id);
            if (area == null)
                return HttpNotFound();

            return View(area);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
