using System.Web.Mvc;

namespace ProyectoProgramacion.Models
{

    public class FiltroSesion : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            if (session["IdUsuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }


    public class FiltroAdministrador : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sesion = filterContext.HttpContext.Session;

            if (sesion["IdUsuario"] == null || sesion["IdRol"]?.ToString() != "2")
            {
                filterContext.Result = new RedirectResult("~/Home/Usuario");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}