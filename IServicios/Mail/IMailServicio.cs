using IServicios.Comprobante.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.Mail
{
    public interface IMailServicio
    {
        void MandarMailUsuario(string empleadoMail, string RazonSocial, string nombreUsuario, string PasswordPorDefecto);

        void MandarMailFacturaCliente(string clienteMail, FacturaDto facturaDto);
    }
}
