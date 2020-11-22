using Dominio.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{   
    [Table("Comprobante_CuentaCorriente")]
    [MetadataType(typeof(ICuentaCorrienteCliente))]
    public class CuentaCorrienteCliente : Comprobante
    {
        public long ClienteId { get; set; }
         
        public virtual Cliente Cliente { get; set; }
    }
}
