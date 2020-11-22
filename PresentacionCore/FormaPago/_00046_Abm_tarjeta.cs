using PresentacionBase.Formularios;

namespace Presentacion.Core.FormaPago
{
    public partial class _00046_Abm_tarjeta : FormAbm
    {
        public _00046_Abm_tarjeta(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();
        }
    }
}
