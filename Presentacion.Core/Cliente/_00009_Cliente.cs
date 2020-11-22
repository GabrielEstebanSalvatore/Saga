using System.Windows.Forms;
using IServicio.Persona;
using IServicio.Persona.DTOs;
using PresentacionBase.Formularios;

namespace Presentacion.Core.Cliente
{
    public partial class _00009_Cliente : FormConsulta
    {
        private readonly IClienteServicio _clienteServicio;
        public _00009_Cliente(IClienteServicio clienteServicio)
        {
            InitializeComponent();
            _clienteServicio = clienteServicio;

        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _clienteServicio.Obtener(typeof(ClienteDto), cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);
                  
            dgv.Columns["ApyNom"].Visible = true;
            dgv.Columns["ApyNom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ApyNom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["ApyNom"].HeaderText = "Nombre";
            dgv.Columns["ApyNom"].DisplayIndex = 1;

            dgv.Columns["Direccion"].Visible = true;
            dgv.Columns["Direccion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Direccion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Direccion"].HeaderText = @"Direccón";
            dgv.Columns["Direccion"].DisplayIndex = 2;

            dgv.Columns["Telefono"].Visible = true;
            dgv.Columns["Telefono"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Telefono"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Telefono"].HeaderText = @"Telefono";
            dgv.Columns["Telefono"].DisplayIndex = 3;

            dgv.Columns["Mail"].Visible = true;
            dgv.Columns["Mail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Mail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Mail"].HeaderText = @"Mail";
            dgv.Columns["Mail"].DisplayIndex = 4;

            

            dgv.Columns["Eliminado"].Visible = true;
            dgv.Columns["Eliminado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Eliminado"].Width = 100;
            dgv.Columns["Eliminado"].HeaderText = "Eliminado";
            dgv.Columns["Eliminado"].DisplayIndex = 5;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00010_Abm_Cliente(tipoOperacion, id);
            form.ShowDialog();
            btnActualizar.PerformClick();
            return form.RealizoAlgunaOperacion;
        }
    }
}
