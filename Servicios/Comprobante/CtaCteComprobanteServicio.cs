using Dominio.UnidadDeTrabajo;
using IServicios.Comprobante.DTOs;

namespace Servicios.Comprobante
{
    public class CtaCteComprobanteServicio : ComprobanteServicio
    {
        public CtaCteComprobanteServicio(IUnidadDeTrabajo unidadDeTrabajo) : base(unidadDeTrabajo)
        {

        }
        public override long Insertar(ComprobanteDto dto)
        {
            return base.Insertar(dto);
        }
    }
}
