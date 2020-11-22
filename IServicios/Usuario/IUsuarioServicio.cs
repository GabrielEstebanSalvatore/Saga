using IServicio.Base;

namespace IServicio.Usuario
{
    public interface IUsuarioServicio : IServicioConsulta
    {
        Dominio.Entidades.Usuario Crear(long empleadoId, string apellidoEmpleado, string nombreEmpleado);

        void Bloquear(long usuarioId);

        void ResetPassword(long usuarioId);
    }
}
