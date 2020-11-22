namespace IServicio.Rubro
{
    public interface IRubroServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
