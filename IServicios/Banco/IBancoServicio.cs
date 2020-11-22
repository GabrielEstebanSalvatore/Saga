namespace IServicios.Comprobante
{
    public interface IBancoServicio : IServicio.Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
