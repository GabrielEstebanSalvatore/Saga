using IServicio.BaseDto;
using IServicios.Comprobante.DTOs;
using System.Collections.Generic;

namespace IServicios.Comprobante
{
    public interface IFacturaServicio : IComprobanteServicio
    {
        IEnumerable<ComprobantePendienteDto> ObtenerPendientesPago();

        IEnumerable<ComprobantePendienteDto> ObtenerPresupuestos();

        void Modificar(DtoBase dtoEntidad);
    }
}
