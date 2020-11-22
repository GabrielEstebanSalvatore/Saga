using Aplicacion.Constantes;
using IServicios.Caja;
using IServicios.Caja.DTOs;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Core.Caja
{
    public partial class _00038_Caja : FormBase
    {

        private readonly ICajaServicio _cajaServicio;
        private CajaDto _cajaSeleccionada;

        public _00038_Caja(ICajaServicio cajaServicio)
        {
            InitializeComponent();
            _cajaServicio = cajaServicio;
            _cajaSeleccionada = null;

        }

        private void btnAbrirCaja_Click(object sender, EventArgs e)
        {
            if (!_cajaServicio.VerificarSiExisteCajaAbierta(Identidad.UsuarioId))
            {
                var fAbrirCaja = ObjectFactory.GetInstance<_00039_AperturaCaja>();
                fAbrirCaja.ShowDialog();

                ActualizarDatos(string.Empty, false, DateTime.Today, DateTime.Today);
            }
            else
            {
                MessageBox.Show($"Ya se encuentra una caja abierta para el usuario {Identidad.Nombre} {Identidad.Apellido}");
            }
        }

        private void ActualizarDatos(string cadenaBuscar, bool filtroPorFecha, DateTime fechaDesde, DateTime fechaHasta)
        {
            dgvGrilla.DataSource = _cajaServicio.Obtener(cadenaBuscar, filtroPorFecha, fechaDesde, fechaHasta);
            FormatearGrilla(dgvGrilla);
        }

        private void chkRangoFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaDesde.Enabled = chkRangoFecha.Checked;
            dtpFechaHasta.Enabled = chkRangoFecha.Checked;

            if (!chkRangoFecha.Checked) return;
            dtpFechaDesde.Value = DateTime.Now;
            dtpFechaHasta.Value = DateTime.Now;
        }

        private void dtpFechaDesde_ValueChanged(object sender, EventArgs e)
        {
             
            dtpFechaHasta.Value = dtpFechaDesde.Value;
            dtpFechaHasta.MinDate = dtpFechaDesde.Value;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ActualizarDatos(!string.IsNullOrEmpty(txtBuscar.Text) ? txtBuscar.Text : string.Empty ,
                chkRangoFecha.Checked, dtpFechaDesde.Value, dtpFechaHasta.Value);
        }

        private void _00038_Caja_Load(object sender, EventArgs e)
        {
            dtpFechaDesde.Value = DateTime.Today;
            dtpFechaHasta.Value = DateTime.Today;
            txtBuscar.Clear();
            ActualizarDatos(!string.IsNullOrEmpty(txtBuscar.Text) ? txtBuscar.Text : string.Empty,
               chkRangoFecha.Checked, dtpFechaDesde.Value, dtpFechaHasta.Value);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["UsuarioApertura"].Visible = true;
            dgv.Columns["UsuarioApertura"].Width = 150;
            dgv.Columns["UsuarioApertura"].HeaderText = @"Usuario Apertura";
            dgv.Columns["UsuarioApertura"].DisplayIndex = 0;

            dgv.Columns["FechaAperturaStr"].Visible = true;
            dgv.Columns["FechaAperturaStr"].Width = 70;
            dgv.Columns["FechaAperturaStr"].HeaderText = @"Fecha Apertura";
            dgv.Columns["FechaAperturaStr"].DisplayIndex = 1;

            dgv.Columns["MontoAperturaStr"].Visible = true;
            dgv.Columns["MontoAperturaStr"].Width = 70;
            dgv.Columns["MontoAperturaStr"].HeaderText = @"Monto Apertura";
            dgv.Columns["MontoAperturaStr"].DisplayIndex = 2;

            dgv.Columns["UsuarioCierre"].Visible = true;
            dgv.Columns["UsuarioCierre"].Width = 150;
            dgv.Columns["UsuarioCierre"].HeaderText = @"Usuario Cierre";
            dgv.Columns["UsuarioCierre"].DisplayIndex = 3;

            dgv.Columns["FechaCierreStr"].Visible = true;
            dgv.Columns["FechaCierreStr"].Width = 70;
            dgv.Columns["FechaCierreStr"].HeaderText = @"Fecha Cierre";
            dgv.Columns["FechaCierreStr"].DisplayIndex = 4;

            dgv.Columns["MontoCierreStr"].Visible = true;
            dgv.Columns["MontoCierreStr"].Width = 70;
            dgv.Columns["MontoCierreStr"].HeaderText = @"Monto Cierre";
            dgv.Columns["MontoCierreStr"].DisplayIndex = 5;

            //-------------------------------------------------

            dgv.Columns["TotalEntradaEfectivo"].Visible = true;
            dgv.Columns["TotalEntradaEfectivo"].Width = 70;
            dgv.Columns["TotalEntradaEfectivo"].HeaderText = @"Efectivo Entrada";
            dgv.Columns["TotalEntradaEfectivo"].DisplayIndex = 6;

            dgv.Columns["TotalEntradaTarjeta"].Visible = true;
            dgv.Columns["TotalEntradaTarjeta"].Width = 70;
            dgv.Columns["TotalEntradaTarjeta"].HeaderText = @"Tarjeta Entrada";
            dgv.Columns["TotalEntradaTarjeta"].DisplayIndex = 7;

            dgv.Columns["TotalEntradaCheque"].Visible = true;
            dgv.Columns["TotalEntradaCheque"].Width = 70;
            dgv.Columns["TotalEntradaCheque"].HeaderText = @"Cheque Entrada";
            dgv.Columns["TotalEntradaCheque"].DisplayIndex = 8;

            dgv.Columns["TotalEntradaCtaCte"].Visible = true;
            dgv.Columns["TotalEntradaCtaCte"].Width = 70;
            dgv.Columns["TotalEntradaCtaCte"].HeaderText = @"CtaCte Entrada";
            dgv.Columns["TotalEntradaCtaCte"].DisplayIndex = 9;

            //-------------------------------------------------

            dgv.Columns["TotalSalidaEfectivo"].Visible = true;
            dgv.Columns["TotalSalidaEfectivo"].Width = 70;
            dgv.Columns["TotalSalidaEfectivo"].HeaderText = @"Efectivo Salida";
            dgv.Columns["TotalSalidaEfectivo"].DisplayIndex = 10;
                             
            dgv.Columns["TotalSalidaTarjeta"].Visible = true;
            dgv.Columns["TotalSalidaTarjeta"].Width = 70;
            dgv.Columns["TotalSalidaTarjeta"].HeaderText = @"Tarjeta Salida";
            dgv.Columns["TotalSalidaTarjeta"].DisplayIndex = 11;
                          
            dgv.Columns["TotalSalidaCheque"].Visible = true;
            dgv.Columns["TotalSalidaCheque"].Width = 70;
            dgv.Columns["TotalSalidaCheque"].HeaderText = @"Cheque Salida";
            dgv.Columns["TotalSalidaCheque"].DisplayIndex = 12;
                              
            dgv.Columns["TotalSalidaCtaCte"].Visible = true;
            dgv.Columns["TotalSalidaCtaCte"].Width = 70;
            dgv.Columns["TotalSalidaCtaCte"].HeaderText = @"CtaCte Salida";
            dgv.Columns["TotalSalidaCtaCte"].DisplayIndex = 13;

        }

        private void btnCierreCaja_Click(object sender, EventArgs e)
        {
            var fCierreCaja = new _00040_CierreCaja(_cajaSeleccionada.Id);

            fCierreCaja.ShowDialog();

            ActualizarDatos(string.Empty, chkRangoFecha.Checked, dtpFechaDesde.Value, dtpFechaHasta.Value);
        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount <= 0)
            {
                _cajaSeleccionada = null;
                return;
            }

            _cajaSeleccionada = (CajaDto)dgvGrilla.Rows[e.RowIndex].DataBoundItem;




        }
    }
}
