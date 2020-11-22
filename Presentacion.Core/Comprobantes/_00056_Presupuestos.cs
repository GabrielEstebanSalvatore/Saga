using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;
using Presentacion.Core.FormaPago;
using PresentacionBase.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Core.Comprobantes
{
    public partial class _00056_Presupuestos : FormBase
    {
        private readonly IFacturaServicio _facturaServicio;
        private ComprobantePendienteDto comprobanteSeleccionado;
        public _00056_Presupuestos(IFacturaServicio facturaServicio)
        {
            InitializeComponent();
            _facturaServicio = facturaServicio;
            comprobanteSeleccionado = null;
            cargarGrilla();
        }

        private void cargarGrilla()
        {
            dgvGrillaPresupuestos.DataSource = _facturaServicio.ObtenerPresupuestos();

            FormatearGrilla(dgvGrillaPresupuestos);
        }

        
        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Numero"].Visible = true;
            dgv.Columns["Numero"].Width = 100;
            dgv.Columns["Numero"].HeaderText = "Nro Comprobante";

            dgv.Columns["ClienteApyNom"].Visible = true;
            dgv.Columns["ClienteApyNom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ClienteApyNom"].HeaderText = "Cliente";

            dgv.Columns["MontoPagarStr"].Visible = true;
            dgv.Columns["MontoPagarStr"].Width = 150;
            dgv.Columns["MontoPagarStr"].HeaderText = "Total";

        }

       

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvGrillaPresupuestos_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("EN CONSTRUCCIÓN");

            /*var fFormaDePago = new _00044_FormaPago(comprobanteSeleccionado);
            fFormaDePago.ShowDialog();

            if (fFormaDePago.RealizoVenta)
            {
                MessageBox.Show("Los datos se grabaron correctamente");
            }*/

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ActualizarDatos(dgvGrillaPresupuestos, string.Empty);
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Quita Ruido molesto enter
                btnBuscar.PerformClick(); // Hago un Click por Codigo
            }
        }
        private void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            FormatearGrilla(dgv);


        }

        private void dgvGrillaPresupuestos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrillaPresupuestos.RowCount <= 0)
            {
                comprobanteSeleccionado = null;
                return;
            }

            comprobanteSeleccionado =
                (ComprobantePendienteDto)dgvGrillaPresupuestos.Rows[e.RowIndex].DataBoundItem;
        }
    }
}
