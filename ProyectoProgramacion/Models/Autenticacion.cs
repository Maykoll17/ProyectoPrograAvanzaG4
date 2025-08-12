using ProyectoProgramacion.Models.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoProgramacion.Models
{
    public class Autenticacion
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Contrasenna { get; set; }
        public string Correo { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }


    }

}