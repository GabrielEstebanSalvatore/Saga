using IServicio.Articulo.DTOs;
using IServicios.Articulo.DTOs;
using System.Collections.Generic;

namespace IServicio.Articulo
{
    public interface IArticuloServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);

        int ObtenerSiguienteNroCodigo();

        int ObtenerCantidadArticulos();

        ArticuloVentaDto ObtenerPorCodigo(string codigo, long listaPrecioId, long depositoId);

        IEnumerable<ArticuloVentaDto> ObtenerLookUp(string cadenaBuscar, long listaPrecioId);
    }
}
