using IServicios.Comprobante.DTOs;
using IServicios.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Mail
{
    public class MailServicio : IMailServicio
    {
        public void MandarMailFacturaCliente(string clienteMail, FacturaDto factura)
        {
            SmtpClient Client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "",
                    Password = ""
                }
            };

            MailAddress FromEmail = new MailAddress("", "E-Commerce");

            MailAddress ToEmail = new MailAddress(clienteMail, "Someone");

            MailMessage Message = new MailMessage()
            {
                From = FromEmail,
                Subject = "Cuenta de Usuario",

                Body = $"<table style = 'max-width: 600px; padding: 10px; margin:0 auto; border-collapse: collapse;'>" +
                       $"  <tr>" +
                       $"    <td style = 'background-color: #34495e; text-align: center; padding: 0'>" +
                       $"      <ul style = 'font-size: 15px;  margin: 10px 0'>" +
                       $"         </ul>" +
                       $"   </td >" +
                       $"</tr>" +
                       $"</table>"

            };

            Message.To.Add(ToEmail);

            Client.Send(Message);
        }

        public void MandarMailUsuario(string empleadoMail, string RazonSocial, string nombreUsuario, string PasswordPorDefecto)
        {
            SmtpClient Client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "@gmail.com",
                    Password = ""
                }
            };

            MailAddress FromEmail = new MailAddress("@gmail.com", "E-Commerce");

            MailAddress ToEmail = new MailAddress(empleadoMail, "Someone");

            MailMessage Message = new MailMessage()
            {
                From = FromEmail,
                Subject = "Cuenta de Usuario",

                Body = $"<table style = 'max-width: 600px; padding: 10px; margin:0 auto; border-collapse: collapse;'>" +
                       $"  <tr>" +
                       $"    <td style = 'background-color: #34495e; text-align: center; padding: 0'>" +
                       $"         <img width = '20%' style = 'display:block; margin: 1.5% 3%' src= 'https://veterinarianuske.com/wp-content/uploads/2016/10/line_separator.png'>" +
                       $"       </a>" +
                       $"  </td>" +
                       $"  </tr>" +
                       $"  <tr>" +
                       $"  <td style = 'padding: 0'>" +
                       $"     <img style = 'padding: 0; display: block' src = 'https://veterinarianuske.com/wp-content/uploads/2018/07/logo-nnske-blanck.jpg' width = '100%'>" +
                       $"  </td>" +
                       $"</tr>" +
                       $"<tr>" +
                       $" <td style = 'background-color: #ecf0f1'>" +
                       $"      <div style = 'color: #34495e; margin: 4% 10% 2%; text-align: justify;font-family: sans-serif'>" +
                       $"            <h1 style = 'color: #e67e22; margin: 0 0 7px' > E-Commerce - {RazonSocial} </h1>" +
                       $"                    <p style = 'margin: 2px; font-size: 15px'>" +
                       $"                      El mejor centro comercial del norte Argentino te da la bienvenida<br>" +
                       $"                      Por el presente correo le enviamos sus datos de Usuario para poder ingresar a nuestro sistema:</p>" +
                       $"      <ul style = 'font-size: 15px;  margin: 10px 0'>" +
                       $"        <li> Usuario: {nombreUsuario}</li>" +
                       $"        <li> Contraseña:{PasswordPorDefecto} </li>" +
                       $"      </ul>" +

                       $"    <div style = 'width: 100%;margin:20px 0; display: inline-block;text-align: center'>" +
                       $"      <img style = 'padding: 0; width: 200px; margin: 5px' src = 'https://veterinarianuske.com/wp-content/uploads/2018/07/tarjetas.png'>" +
                       $"    </div>" +

                       $" </td >" +
                       $"</tr>" +
                       $"</table>"

            };

            Message.To.Add(ToEmail);

            Client.Send(Message);

        }
    }
}
