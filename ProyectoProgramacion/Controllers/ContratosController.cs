using System;
using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacion.Models.EF;

namespace ProyectoProgramacion.Controllers
{
    public class ContratosController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

        // Helpers de sesión (mismo esquema que HomeController)
        private bool IsLogged() => Session["IdUsuario"] != null;
        private int Rol() => IsLogged() ? Convert.ToInt32(Session["IdRol"]) : 0; // 1=Admin
        private bool IsAdmin() => Rol() == 1;
        private int LoggedId() => Convert.ToInt32(Session["IdUsuario"]);

     

        // GET: /Contratos/AdminIndex
        public ActionResult AdminIndex()
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            var data = db.Contrato
                         .OrderByDescending(c => c.ID_Contrato)
                         .ToList(); // EF generó nav props Usuario y Apartamento

            return View(data);
        }

        // GET: /Contratos/Create
        public ActionResult Create()
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            // Solo usuarios
            var usuarios = db.Usuario
                             .Where(u => u.IdRol == 2)
                             .OrderBy(u => u.Nombre)
                             .Select(u => new { u.ID_Usuario, Nombre = u.Nombre + " (" + u.Cedula + ")" })
                             .ToList();

            // Solo apartamentos disponibles
            var disponibles = db.Apartamento
                                .Where(a => a.Disponible == true)
                                .OrderBy(a => a.Codigo_Apartamento)
                                .Select(a => new { a.ID_Apartamento, Nombre = a.Codigo_Apartamento })
                                .ToList();

            ViewBag.ID_Usuario = new SelectList(usuarios, "ID_Usuario", "Nombre");
            ViewBag.ID_Apartamento = new SelectList(disponibles, "ID_Apartamento", "Nombre");

            return View();
        }

        // POST: /Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(403);

            // Validaciones mínimas
            if (contrato.ID_Usuario <= 0) ModelState.AddModelError("", "Seleccione un usuario.");
            if (contrato.ID_Apartamento <= 0) ModelState.AddModelError("", "Seleccione un apartamento disponible.");
            if (contrato.Monto_Mensual <= 0) ModelState.AddModelError("", "Monto mensual inválido.");
            if (contrato.Fecha_Inicio == default(DateTime) || contrato.Fecha_Fin == default(DateTime) || contrato.Fecha_Fin <= contrato.Fecha_Inicio)
                ModelState.AddModelError("", "Rango de fechas inválido.");

            // Asegurar disponibilidad del apartamento
            var apto = db.Apartamento.FirstOrDefault(a => a.ID_Apartamento == contrato.ID_Apartamento && a.Disponible == true);
            if (apto == null) ModelState.AddModelError("", "El apartamento seleccionado no está disponible.");

            if (!ModelState.IsValid)
            {
                // Rellenar selects otra vez
                ViewBag.ID_Usuario = new SelectList(
                    db.Usuario.Where(u => u.IdRol == 2).OrderBy(u => u.Nombre)
                      .Select(u => new { u.ID_Usuario, Nombre = u.Nombre + " (" + u.Cedula + ")" }).ToList(),
                    "ID_Usuario", "Nombre", contrato.ID_Usuario);

                ViewBag.ID_Apartamento = new SelectList(
                    db.Apartamento.Where(a => a.Disponible == true).OrderBy(a => a.Codigo_Apartamento)
                      .Select(a => new { a.ID_Apartamento, Nombre = a.Codigo_Apartamento }).ToList(),
                    "ID_Apartamento", "Nombre", contrato.ID_Apartamento);

                return View(contrato);
            }

            contrato.Estado = string.IsNullOrWhiteSpace(contrato.Estado) ? "Activo" : contrato.Estado.Trim();

            db.Contrato.Add(contrato);
            // Marcar el apartamento como no disponible
            apto.Disponible = false;
            db.SaveChanges();

            return RedirectToAction("AdminIndex");
        }

        // =======================
        // USUARIO (ver sus contratos y estado mensual)
        // =======================

        // GET: /Contratos/MisContratos
        public ActionResult MisContratos()
        {
            if (!IsLogged()) return new HttpStatusCodeResult(401);
            var id = LoggedId();
            var hoy = DateTime.Today;

            var lista = db.Contrato
                          .Where(c => c.ID_Usuario == id)
                          .OrderByDescending(c => c.Estado == "Activo")
                          .ThenByDescending(c => c.Fecha_Inicio)
                          .ToList()
                          .Select(c => new MisContratosVM
                          {
                              ID_Contrato = c.ID_Contrato,
                              CodigoApartamento = c.Apartamento?.Codigo_Apartamento,
                              Fecha_Inicio = c.Fecha_Inicio,
                              Fecha_Fin = c.Fecha_Fin,
                              Monto_Mensual = c.Monto_Mensual,
                              Estado = c.Estado,
                              // ¿Pagó este mes?
                              PagadoEsteMes = db.Pago.Any(p =>
                                  p.ID_Contrato == c.ID_Contrato &&
                                  p.Fecha_Pago.Year == hoy.Year &&
                                  p.Fecha_Pago.Month == hoy.Month &&
                                  p.Monto_Pago >= c.Monto_Mensual)
                          })
                          .ToList();

            return View(lista);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }

    // ViewModel para "MisContratos"
    public class MisContratosVM
    {
        public int ID_Contrato { get; set; }
        public string CodigoApartamento { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public double Monto_Mensual { get; set; }
        public string Estado { get; set; }
        public bool PagadoEsteMes { get; set; }
    }
}
