using IServicio.BaseDto;
using System.Collections.Generic;

namespace IServicio.Iva
{
    public interface IIvaServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);

      
    }
}
