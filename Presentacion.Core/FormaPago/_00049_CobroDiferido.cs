using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Forms;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;
using PresentacionBase.Formularios;

namespace Presentacion.Core.FormaPago
{
    public partial class _00049_CobroDiferido : FormBase
    {
        private readonly IFacturaServicio _facturaServicio;

        private ComprobantePendienteDto comprobanteSeleccionado;
        public _00049_CobroDiferido(IFacturaServicio facturaServicio)
        {
            InitializeComponent();
            _facturaServicio = facturaServicio;

            comprobanteSeleccionado = null;

            //dgvGrillaPedientePago.DataSource = new List<ComprobantePendienteDto>();
            //FormatearGrilla(dgvGrillaPedientePago);

            //dgvGrillaDetalleComprobante.DataSource = new List<DetalleComprobanteDto>();
            //FormatearGrilla(dgvGrillaDetalleComprobante);
            // Libreria para que refresque cada 60 seg la grilla
            // con las facturas que estan pendientes de pago.

            cargarGrilla();

            /*Observable.Interval(TimeSpan.FromSeconds(60))
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(_ =>
                {
                    dgvGrillaPedientePago.DataSource = null;
                    dgvGrillaPedientePago.DataSource = _facturaServicio.ObtenerPendientesPago();
                    FormatearGrilla(dgvGrillaPedientePago);
                });*/
        }

        private void cargarGrilla()
        {
            dgvGrillaPedientePago.DataSource = _facturaServicio.ObtenerPendientesPago();

            FormatearGrilla(dgvGrillaPedientePago);
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

        private void dgvGrillaPedientePago_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrillaPedientePago.RowCount <= 0)
            {
                comprobanteSeleccionado = null;
                return;
            }
           
            comprobanteSeleccionado =
                (ComprobantePendienteDto)dgvGrillaPedientePago.Rows[e.RowIndex].DataBoundItem;

            if (comprobanteSeleccionado != null)
            {
                nudTotal.Value = comprobanteSeleccionado.MontoPagar;

                dgvGrillaDetalleComprobante.DataSource = null;
                dgvGrillaDetalleComprobante.DataSource = comprobanteSeleccionado.Items.ToList();

                
                FormatearGrillaDetalle(dgvGrillaDetalleComprobante);    
            }
            
            
        }

        private void FormatearGrillaDetalle(DataGridView dgv)
        {
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].Visible = false;
            }

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; ;
            dgv.Columns["Descripcion"].HeaderText = @"Artículo";

            dgv.Columns["PrecioStr"].Visible = true;
            dgv.Columns["PrecioStr"].Width = 100;
            dgv.Columns["PrecioStr"].HeaderText = @"Precio";

            dgv.Columns["Cantidad"].Visible = true;
            dgv.Columns["Cantidad"].Width = 150;
            dgv.Columns["Cantidad"].HeaderText = "Cantidad";
        }

        private void dgvGrillaPedientePago_DoubleClick(object sender, EventArgs e)
        {
            var fFormaDePago = new _00044_FormaPago(comprobanteSeleccionado);
            fFormaDePago.ShowDialog();

            if (fFormaDePago.RealizoVenta)
            {
                MessageBox.Show("Los datos se grabaron correctamente");
            }
        }
    }
}
