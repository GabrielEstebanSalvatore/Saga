using IServicio.Articulo;
using IServicios.Articulo;
using PresentacionBase.Formularios;
using System.Windows.Forms;

namespace Presentacion.Core.Articulo
{
    public partial class _00027_MotivoBaja : FormConsulta
    {
        //private readonly IArticuloServicio _articuloServicio;
        private readonly IMotivoBajaServicio _motivoBajaServicio;
        public _00027_MotivoBaja(IMotivoBajaServicio motivoBajaServicio)
        {
            InitializeComponent();
            _motivoBajaServicio = motivoBajaServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _motivoBajaServicio.Obtener(cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }
        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = "Motivo de Baja";


            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].Width = 100;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";

        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00028_Abm_MotivoBaja(tipoOperacion, id);
            form.ShowDialog();

            btnActualizar.PerformClick();

            return form.RealizoAlgunaOperacion;
        }
    }
}
