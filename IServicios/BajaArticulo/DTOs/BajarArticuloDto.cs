using IServicio.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.Articulo.DTOs
{
    public class BajarArticuloDto : DtoBase
    {
        public string Articulo { get; set; }
        public long ArticuloId { get; set; }

        public string MotivoBaja { get; set; }
        public long MotivoBajaId { get; set; }

        public decimal Cantidad { get; set; }

        public DateTime Fecha { get; set; }

        public string Observacion { get; set; }
    }
}
