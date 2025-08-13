using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoProgramacion.Models.EF;

namespace ProyectoProgramacion.Controllers
{
    public class PagosController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();


        private bool IsLogged() => Session["ID_Usuario"] != null;
        private int Rol() => IsLogged() ? Convert.ToInt32(Session["Rol"]) : 0;
        private bool IsAdmin() => Rol() == 1;
        private int? LoggedUserId() => IsLogged() ? (int?)Convert.ToInt32(Session["ID_Usuario"]) : null;




        public ActionResult Index()
        {
            if (!IsLogged()) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (IsAdmin()) return RedirectToAction("AdminIndex");

            var idUsuario = LoggedUserId().Value;

            var pagos = db.Pago
                          .Where(p => p.Contrato.ID_Usuario == idUsuario)
                          .OrderByDescending(p => p.Fecha_Pago)
                          .ToList();

            return View(pagos);
        }

        // GET: /Pagos/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var pago = db.Pago.FirstOrDefault(p => p.ID_Pago == id.Value);
            if (pago == null) return HttpNotFound();

            // Seguridad: usuario normal solo ve pagos de sus contratos
            if (!IsAdmin())
            {
                if (!IsLogged()) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                var idUsuario = LoggedUserId().Value;

                if (pago.Contrato == null || pago.Contrato.ID_Usuario != idUsuario)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(pago);
        }


        public ActionResult Create()
        {
            if (!IsLogged()) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (IsAdmin())
            {
                ViewBag.ID_Contrato = new SelectList(
                    db.Contrato.OrderBy(c => c.ID_Contrato).ToList(),
                    "ID_Contrato", "ID_Contrato"
                );
            }
            else
            {
                var idUsuario = LoggedUserId().Value;
                var contratosPropios = db.Contrato
                                         .Where(c => c.ID_Usuario == idUsuario && c.Estado == "Activo")
                                         .OrderBy(c => c.ID_Contrato)
                                         .Select(c => new { c.ID_Contrato })
                                         .ToList();

                ViewBag.ID_Contrato = new SelectList(contratosPropios, "ID_Contrato", "ID_Contrato");
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            if (!IsLogged()) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);


            if (pago.ID_Contrato <= 0)
                ModelState.AddModelError("", "Debe seleccionar un contrato válido.");

            if (!IsAdmin())
            {
                var idUsuario = LoggedUserId().Value;
                bool contratoEsDelUsuario = db.Contrato.Any(c => c.ID_Contrato == pago.ID_Contrato && c.ID_Usuario == idUsuario);
                if (!contratoEsDelUsuario)
                    ModelState.AddModelError("", "No puedes registrar pagos sobre contratos que no son tuyos.");
            }

            if (!ModelState.IsValid)
            {

                if (IsAdmin())
                {
                    ViewBag.ID_Contrato = new SelectList(
                        db.Contrato.OrderBy(c => c.ID_Contrato).ToList(),
                        "ID_Contrato", "ID_Contrato", pago.ID_Contrato
                    );
                }
                else
                {
                    var idUsuario = LoggedUserId().Value;
                    var contratosPropios = db.Contrato
                                             .Where(c => c.ID_Usuario == idUsuario && c.Estado == "Activo")
                                             .OrderBy(c => c.ID_Contrato)
                                             .Select(c => new { c.ID_Contrato })
                                             .ToList();
                    ViewBag.ID_Contrato = new SelectList(contratosPropios, "ID_Contrato", "ID_Contrato", pago.ID_Contrato);
                }
                return View(pago);
            }

            if (pago.Fecha_Pago == default(DateTime))
                pago.Fecha_Pago = DateTime.Today;

            db.Pago.Add(pago);
            db.SaveChanges();

            return IsAdmin() ? RedirectToAction("AdminIndex") : RedirectToAction("Index");
        }


        // SECCIÓN ADMIN

        // GET: /Pagos/AdminIndex
        public ActionResult AdminIndex()
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var pagos = db.Pago
                          .OrderByDescending(p => p.Fecha_Pago)
                          .ToList();

            return View(pagos);
        }

        // POST: /Pagos/ActualizarEstado
        // Reutilizamos Metodo_Pago como "estado" visible (Pagado / Pendiente / Rechazado) 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarEstado(int id, string nuevoEstado)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var pago = db.Pago.FirstOrDefault(p => p.ID_Pago == id);
            if (pago == null) return HttpNotFound();

            if (!string.IsNullOrWhiteSpace(nuevoEstado))
            {
                pago.Metodo_Pago = nuevoEstado.Trim();
                db.SaveChanges();
            }

            return RedirectToAction("AdminIndex");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}

