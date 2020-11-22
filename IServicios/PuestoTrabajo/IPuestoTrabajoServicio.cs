

namespace IServicios.PuestoTrabajo
{
    public interface IPuestoTrabajoServicio : IServicio.Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
