using System.Windows.Forms;
using IServicios.Articulo;
using PresentacionBase.Formularios;

namespace Presentacion.Core.Articulo
{
    public partial class _00029_BajaDeArticulos : FormConsulta
    {
        private readonly IMotivoBajaServicio _motivoBajaServicio;
        private readonly IBajaArticuloServicio _bajaArticuloServicio;

        public _00029_BajaDeArticulos(IMotivoBajaServicio motivoBajaServicio,
            IBajaArticuloServicio bajaArticuloServicio)
        {
            InitializeComponent();

            _motivoBajaServicio = motivoBajaServicio;
            _bajaArticuloServicio = bajaArticuloServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _bajaArticuloServicio.Obtener(cadenaBuscar);
            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Observacion"].Visible = true;
            dgv.Columns["Observacion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Observacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Observacion"].HeaderText = @"Observación";

            dgv.Columns["Articulo"].Visible = true;
            dgv.Columns["Articulo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Articulo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Articulo"].HeaderText = @"Articulo";

            dgv.Columns["Cantidad"].Visible = true;
            dgv.Columns["Cantidad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Cantidad"].HeaderText = @"Cantidad de baja";


            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].Width = 60;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var formulario = new _00030_Abm_BajaArticulos(tipoOperacion, id);

            formulario.ShowDialog();

            return formulario.RealizoAlgunaOperacion;
        }


    }
}
