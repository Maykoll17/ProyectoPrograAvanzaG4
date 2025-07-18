using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoProgramacion.Models
{
    public class Usuario
    {
            [Required]
            [EmailAddress]
            public string Correo { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Contrasenna { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Contrasenna", ErrorMessage = "Las contraseñas no coinciden.")]
            public string ConfirmarContraseña { get; set; }
        }


    
}