using IServicios.Caja.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.Caja
{
    public interface ICajaServicio
    {
        bool VerificarSiExisteCajaAbierta(long usuarioId);

        IEnumerable<CajaDto> Obtener(string cadenaBuscar, bool filtroPorFecha, DateTime fechaDesde, DateTime fechaHasta);

        decimal ObtenerMontoCajaAnterior(long usuarioId);

        void Abrir(long usuarioId, decimal monto, DateTime fecha);

        CajaDto Obtener(long cajaId);

        void CerrarCaja(CajaDto dto);
    }
}
