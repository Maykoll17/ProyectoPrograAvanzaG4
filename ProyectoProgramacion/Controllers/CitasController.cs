using ProyectoProgramacion.Models.EF;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ProyectoProgramacion.Controllers
{
    public class CitasController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

        // Helpers de sesión (coinciden con tu HomeController)
        private bool IsLogged() => Session["IdUsuario"] != null;
        private int Rol() => IsLogged() ? Convert.ToInt32(Session["IdRol"]) : 0; // 1=Admin
        private bool IsAdmin() => Rol() == 1;
        private int LoggedId() => Convert.ToInt32(Session["IdUsuario"]);

        // =========================================================
        // USUARIO
        // =========================================================

        // GET: /Citas/Create
        // Permite preseleccionar un apartamento (p.ej. desde su detalle): /Citas/Create?ID_Apartamento=3
        public ActionResult Create(int? ID_Apartamento)
        {
            if (!IsLogged()) return new HttpStatusCodeResult(401);

            var disponibles = db.Apartamento
                                .Where(a => a.Disponible == true)
                                .OrderBy(a => a.Codigo_Apartamento)
                                .Select(a => new { a.ID_Apartamento, Nombre = a.Codigo_Apartamento })
                                .ToList();

            ViewBag.ID_Apartamento = new SelectList(disponibles, "ID_Apartamento", "Nombre", ID_Apartamento);
            return View();
        }

        // POST: /Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int ID_Apartamento, DateTime? FechaCita, string Mensaje)
        {
            if (!IsLogged()) return new HttpStatusCodeResult(401);

            // Validaciones básicas
            if (ID_Apartamento <= 0) ModelState.AddModelError("", "Seleccione un apartamento válido.");
            if (!FechaCita.HasValue) ModelState.AddModelError("", "Debe indicar fecha y hora.");
            else if (FechaCita.Value < DateTime.Now.AddMinutes(30))
                ModelState.AddModelError("", "La fecha/hora debe ser al menos 30 minutos en adelante.");

            // ¿apto disponible?
            var apto = db.Apartamento.FirstOrDefault(a => a.ID_Apartamento == ID_Apartamento && a.Disponible == true);
            if (apto == null) ModelState.AddModelError("", "El apartamento no está disponible o no existe.");

            if (!ModelState.IsValid)
            {
                ViewBag.ID_Apartamento = new SelectList(
                    db.Apartamento.Where(a => a.Disponible == true)
                                  .OrderBy(a => a.Codigo_Apartamento)
                                  .Select(a => new { a.ID_Apartamento, Nombre = a.Codigo_Apartamento }).ToList(),
                    "ID_Apartamento", "Nombre", ID_Apartamento);
                return View();
            }

            var cita = new Cita
            {
                ID_Usuario = LoggedId(),
                ID_Apartamento = ID_Apartamento,
                FechaCita = FechaCita.Value,
                Mensaje = string.IsNullOrWhiteSpace(Mensaje) ? null : Mensaje.Trim(),
                Estado = "Pendiente",
                FechaCreacion = DateTime.Now
            };

            db.Cita.Add(cita);
            db.SaveChanges();

            return RedirectToAction("MisCitas");
        }

        // GET: /Citas/MisCitas
        public ActionResult MisCitas()
        {
            if (!IsLogged()) return new HttpStatusCodeResult(401);

            var id = LoggedId();
            var citas = db.Cita
                          .Where(c => c.ID_Usuario == id)
                          .OrderByDescending(c => c.FechaCreacion)
                          .ToList();

            return View(citas);
        }

        // =========================================================
        // ADMIN
        // =========================================================

        // GET: /Citas/AdminIndex
        // Filtro opcional por estado: ?estado=Pendiente/Pagado/etc
        public ActionResult AdminIndex(string estado = null)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var q = db.Cita.AsQueryable();

            if (!string.IsNullOrWhiteSpace(estado))
                q = q.Where(c => c.Estado == estado);

            var lista = q.OrderBy(c => c.Estado)
                         .ThenBy(c => c.FechaCita)
                         .ToList();

            ViewBag.Estado = estado;
            return View(lista);
        }

        // POST: /Citas/CambiarEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int id, string estado)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var cita = db.Cita.FirstOrDefault(c => c.ID_Cita == id);
            if (cita == null) return HttpNotFound();

            // Estados típicos (ajústalos a tu gusto)
            var validos = new[] { "Pendiente", "Aprobada", "Reprogramar", "Cancelada", "Realizada" };
            if (!validos.Contains((estado ?? "").Trim()))
                return new HttpStatusCodeResult(400, "Estado no válido");

            cita.Estado = estado.Trim();
            db.SaveChanges();

            return RedirectToAction("AdminIndex");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
