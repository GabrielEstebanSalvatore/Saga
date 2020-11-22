namespace IServicio.UnidadMedida
{
    public interface IUnidadMedidaServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
