using ProyectoProgramacion.Models.EF;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ProyectoProgramacion.Controllers
{
    public class CitasController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

     
        private bool IsLogged() => Session["IdUsuario"] != null;
        private int Rol() => IsLogged() ? Convert.ToInt32(Session["IdRol"]) : 0; // 1=Admin
        private bool IsAdmin() => Rol() == 1;
        private int LoggedId() => Convert.ToInt32(Session["IdUsuario"]);

     

        // GET: /Citas/Create
      
        [HttpGet]
        public ActionResult Create(int? ID_Apartamento)
        {
            if (!IsLogged()) return new HttpStatusCodeResult(401);

            var disponibles = db.Apartamento
                                .Where(a => a.Disponible == true)
                                .OrderBy(a => a.Codigo_Apartamento)
                                .Select(a => new { a.ID_Apartamento, Nombre = a.Codigo_Apartamento })
                                .ToList();
            ViewBag.ID_Apartamento = new SelectList(disponibles, "ID_Apartamento", "Nombre", ID_Apartamento);

            
            var horas = Enumerable.Range(18, 0)
                .ToList();

            var listaHoras = new System.Collections.Generic.List<string>();
            var inicio = new TimeSpan(9, 0, 0);
            var fin = new TimeSpan(18, 0, 0);
            for (var t = inicio; t <= fin; t = t.Add(new TimeSpan(0, 30, 0)))
                listaHoras.Add($"{t.Hours:D2}:{t.Minutes:D2}");

            ViewBag.Horas = new SelectList(listaHoras);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int ID_Apartamento, DateTime Fecha, string Hora, string Mensaje)
        {
            if (!IsLogged()) return new HttpStatusCodeResult(401);

           
            if (ID_Apartamento <= 0) ModelState.AddModelError("", "Seleccione un apartamento.");
            if (Fecha.Date < DateTime.Today) ModelState.AddModelError("", "La fecha debe ser hoy o posterior.");
            if (string.IsNullOrWhiteSpace(Hora)) ModelState.AddModelError("", "Seleccione una hora.");

            DateTime fechaHora = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(Hora))
            {
                
                if (!DateTime.TryParseExact(
                        $"{Fecha:yyyy-MM-dd} {Hora}",
                        "yyyy-MM-dd HH:mm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out fechaHora))
                {
                    ModelState.AddModelError("", "Hora inválida.");
                }
            }

            if (!ModelState.IsValid)
            {
              
                var disponibles = db.Apartamento
                                    .Where(a => a.Disponible == true)
                                    .OrderBy(a => a.Codigo_Apartamento)
                                    .Select(a => new { a.ID_Apartamento, Nombre = a.Codigo_Apartamento })
                                    .ToList();
                ViewBag.ID_Apartamento = new SelectList(disponibles, "ID_Apartamento", "Nombre", ID_Apartamento);

                var listaHoras = new System.Collections.Generic.List<string>();
                var inicio = new TimeSpan(9, 0, 0);
                var fin = new TimeSpan(18, 0, 0);
                for (var t = inicio; t <= fin; t = t.Add(new TimeSpan(0, 30, 0)))
                    listaHoras.Add($"{t.Hours:D2}:{t.Minutes:D2}");
                ViewBag.Horas = new SelectList(listaHoras, Hora);

                return View();
            }

            var cita = new Cita
            {
                ID_Usuario = LoggedId(),
                ID_Apartamento = ID_Apartamento,
                FechaCita = fechaHora,
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int id, string estado)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var cita = db.Cita.FirstOrDefault(c => c.ID_Cita == id);
            if (cita == null) return HttpNotFound();

          
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
