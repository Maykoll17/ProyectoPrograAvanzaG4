using System.ComponentModel.DataAnnotations;

namespace RentalPropertyManagement.Models
{
    public class ApplicationRequest
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Número de Teléfono")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Dirección Actual")]
        public string CurrentAddress { get; set; }

        [Required]
        [Display(Name = "Motivo de la Solicitud")]
        public string Reason { get; set; }

        [Display(Name = "Información Adicional")]
        public string AdditionalInfo { get; set; }
    }
}
