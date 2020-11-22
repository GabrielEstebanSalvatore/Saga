using IServicio.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.Articulo.DTOs
{
    public class MotivoBajaDto : DtoBase
    {
        public string Descripcion { get; set; }
        public long ArticuloId { get; set; }
        public long MotivoBajaId { get; set; }
    }
}
