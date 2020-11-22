using Aplicacion.Constantes;
using IServicio.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.Comprobante.DTOs
{
    public class FormaPagoDto : DtoBase
    {
        public TipoPago TipoPago { get; set; }
        public decimal Monto { get; set; }
    }
}
