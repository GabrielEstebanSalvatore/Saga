using PresentacionBase.Formularios;

namespace Presentacion.Core.FormaPago
{
    public partial class _00048_Abm_Banco : FormAbm
    {
        public _00048_Abm_Banco(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();
        }
    }
}
