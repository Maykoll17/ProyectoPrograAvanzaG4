using ProyectoProgramacion.Models;
using ProyectoProgramacion.Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Edificio = ProyectoProgramacion.Models.EF.Edificio;

namespace ProyectoProgramacion.Services
{
    public class Utilitarios
    {
        public bool EnviarCorreo(string destinatario, string mensaje, string asunto)
        {
            try
            {
                var remitente = ConfigurationManager.AppSettings["CorreoRemitente"];
                var contrasena = ConfigurationManager.AppSettings["CorreoPassword"];

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(remitente),
                    To = { destinatario },
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = true,
                };

                SmtpClient smtp = new SmtpClient("smtp.office365.com", 587)
                {
                    Credentials = new NetworkCredential(remitente, contrasena),
                    EnableSsl = true
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public string GenerarPassword(int longitud = 8)
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var sb = new StringBuilder(longitud);

            for (int i = 0; i < longitud; i++)
            {
                int index = random.Next(caracteres.Length);
                sb.Append(caracteres[index]);
            }

            return sb.ToString();
        }



        public List<Edificio> ConsultarDatosEdificios(string filtro)
        {
            using (var dbContext = new SistemaAlquilerEntities1())
            {
                List<Edificio> result;

                if (filtro == "Todos")
                    result = dbContext.Edificio.ToList();
                else
                    result = dbContext.Edificio.Where(x => x.ID_Edificio == 0).ToList();

                return result;
            }
        }

    }
}