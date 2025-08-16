using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoProgramacion.Models.EF;

namespace ProyectoProgramacion.Controllers
{
    public class PagosController : Controller
    {
        private readonly SistemaAlquilerEntities1 db = new SistemaAlquilerEntities1();

      
        private bool IsLogged() => Session["IdUsuario"] != null;
        private int Rol() => IsLogged() ? Convert.ToInt32(Session["IdRol"]) : 0; 
        private bool IsAdmin() => Rol() == 1;
        private int? LoggedUserId() => IsLogged() ? (int?)Convert.ToInt32(Session["IdUsuario"]) : null;

      
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

        public ActionResult Details(int? id)
        {
            if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var pago = db.Pago.FirstOrDefault(p => p.ID_Pago == id.Value);
            if (pago == null) return HttpNotFound();

            
            if (!IsAdmin())
            {
                if (!IsLogged()) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                var idUsuario = LoggedUserId().Value;
                if (pago.Contrato == null || pago.Contrato.ID_Usuario != idUsuario)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(pago);
        }

        
        public ActionResult Create(int? idContrato = null)
        {
            if (!IsLogged()) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (IsAdmin())
            {
                
                ViewBag.ID_Contrato = new SelectList(
                    db.Contrato.OrderBy(c => c.ID_Contrato).ToList(),
                    "ID_Contrato", "ID_Contrato", idContrato
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

                ViewBag.ID_Contrato = new SelectList(contratosPropios, "ID_Contrato", "ID_Contrato", idContrato);
            }

            
            ViewBag.Metodo_Pago = new SelectList(new[] { "SINPE", "Transferencia" });

            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago, HttpPostedFileBase comprobante)
        {
            if (!IsLogged()) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

          
            if (string.IsNullOrWhiteSpace(pago.Metodo_Pago)) pago.Metodo_Pago = "SINPE";
            var metodo = pago.Metodo_Pago.Trim();
            if (metodo != "SINPE" && metodo != "Transferencia")
                ModelState.AddModelError("", "Método inválido. Debe ser SINPE o Transferencia.");

            
            if (pago.ID_Contrato <= 0)
                ModelState.AddModelError("", "Debe seleccionar un contrato válido.");

            if (pago.Monto_Pago <= 0)
                ModelState.AddModelError("", "Debe indicar un monto válido.");

            if (string.IsNullOrWhiteSpace(pago.Numero_SINPE))
                ModelState.AddModelError("", "Debe indicar el número SINPE/comprobante (referencia).");

            if (comprobante == null || comprobante.ContentLength == 0)
                ModelState.AddModelError("", "Debe adjuntar la imagen del comprobante.");

          
            if (!IsAdmin() && pago.ID_Contrato > 0)
            {
                var idUsuario = LoggedUserId().Value;
                bool esSuyo = db.Contrato.Any(c => c.ID_Contrato == pago.ID_Contrato && c.ID_Usuario == idUsuario);
                if (!esSuyo)
                    ModelState.AddModelError("", "No puedes registrar pagos de contratos que no son tuyos.");
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

                ViewBag.Metodo_Pago = new SelectList(new[] { "SINPE", "Transferencia" }, metodo);
                return View(pago);
            }

           
            string carpeta = Server.MapPath("~/Uploads/Comprobantes");
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string ext = Path.GetExtension(comprobante.FileName);
            string fileName = $"pago_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{ext}";
            string fullPath = Path.Combine(carpeta, fileName);
            comprobante.SaveAs(fullPath);

            if (pago.Fecha_Pago == default(DateTime))
                pago.Fecha_Pago = DateTime.Today;

            pago.Estado = "Pendiente";
            pago.Metodo_Pago = metodo;
            pago.Comprobante_URL = Url.Content("~/Uploads/Comprobantes/" + fileName);

            db.Pago.Add(pago);
            db.SaveChanges();

            return IsAdmin() ? RedirectToAction("AdminIndex") : RedirectToAction("Index");
        }

        
        public ActionResult AdminIndex()
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var pagos = db.Pago
                          .OrderByDescending(p => p.Fecha_Pago)
                          .ToList();

            return View(pagos);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarEstado(int id, string nuevoEstado)
        {
            if (!IsAdmin()) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var pago = db.Pago.FirstOrDefault(p => p.ID_Pago == id);
            if (pago == null) return HttpNotFound();

            if (!string.IsNullOrWhiteSpace(nuevoEstado))
            {
                pago.Estado = nuevoEstado.Trim(); // <- aquí cambiamos Estado
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
