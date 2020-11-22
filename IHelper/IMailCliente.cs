using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IServicios.Articulo.DTOs;
using IServicios.Comprobante.DTOs;

namespace IHelper
{
    public interface IMailCliente
    {
        void MandarMailUsuario(string empleadoMail, string RazonSocial, string nombreUsuario, string PasswordPorDefecto);

        void MandarMailFacturaCliente(string clienteMail, FacturaDto facturaDto);
    }
}
