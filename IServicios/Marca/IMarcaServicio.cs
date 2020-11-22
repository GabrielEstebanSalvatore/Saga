namespace IServicio.Marca
{
    public interface IMarcaServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
