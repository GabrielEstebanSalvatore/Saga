using System.ComponentModel.DataAnnotations;

namespace Dominio.MetaData
{
    public interface ICuentaCorrienteCliente : IComprobante
    {
         [Required(ErrorMessage ="El campo {0} es obligatorio")]

        long ClienteId { get; set; }
    }
}
