using IServicios.CuentaCorriente.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.CuentaCorriente
{
    public interface ICuentaCorrienteServicio 
    {
        decimal ObtenerDeudaCliente(long clienteId);

        IEnumerable<CuentaCorrienteDto> Obtener(long clienteId, DateTime fechaDesde, DateTime fechaHasta, bool soloDeuda);

        void Pagar(long cajaId, long usuarioId, long clienteId, decimal monto, string clienteApyNom);
    }
}
