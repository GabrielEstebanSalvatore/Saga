using IServicio.Persona.DTOs;
using IServicios.CuentaCorriente;
using IServicios.CuentaCorriente.DTOs;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion.Core.Cliente
{
    public partial class _00034_ClienteCtaCte : FormBase
    {
        private ClienteDto _clienteSeleccionado;
        private ICuentaCorrienteServicio _cuentaCorrienteServicio;
        public _00034_ClienteCtaCte(ICuentaCorrienteServicio cuentaCorrienteServicio)
        {
            InitializeComponent();

            _cuentaCorrienteServicio = cuentaCorrienteServicio;
            _clienteSeleccionado = null;
            dgvGrilla.DataSource = new List<CuentaCorrienteDto>();
            FormatearGrilla(dgvGrilla);
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            var fClienteLookUp = ObjectFactory.GetInstance<ClienteLookUp>();
            fClienteLookUp.ShowDialog();

            if (fClienteLookUp.EntidadSeleccionada != null) 
            {
                _clienteSeleccionado = (ClienteDto)fClienteLookUp.EntidadSeleccionada;

                txtApyNom.Text = _clienteSeleccionado.ApyNom;
                txtCelular.Text = _clienteSeleccionado.Telefono;
                txtDni.Text = _clienteSeleccionado.Dni;

                CargarDatos();
            }
            else
            {
                txtCelular.Clear();
                txtApyNom.Clear();
                txtDni.Clear();
                _clienteSeleccionado = null;

                dgvGrilla.DataSource = new List<CuentaCorrienteDto>();

                var fechaInicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                dtpFechaDesde.Value = fechaInicioMes;

                txtTotal.Text = 0.ToString("C");
            }
        }

        private void CargarDatos()
        {
            var resultado = _cuentaCorrienteServicio.Obtener(_clienteSeleccionado.Id, dtpFechaDesde.Value, dtpFechaHasta.Value, rdbDeuda.Checked);
            dgvGrilla.DataSource = resultado;

            FormatearGrilla(dgvGrilla);
            txtTotal.Text = resultado.Sum(x => x.Monto) >= 0
                ? resultado.Sum(x => x.Monto).ToString("C")
                : (resultado.Sum(x => x.Monto)*-1 ).ToString("C");

            txtTotal.BackColor = resultado.Sum(x => x.Monto) >= 0
            ? Color.DarkGreen
            : Color.DarkRed;
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = @"Descripción";

            dgv.Columns["FechaStr"].Visible = true;
            dgv.Columns["FechaStr"].Width = 100;
            dgv.Columns["FechaStr"].HeaderText = "Fecha";

            dgv.Columns["HoraStr"].Visible = true;
            dgv.Columns["HoraStr"].Width = 100;
            dgv.Columns["HoraStr"].HeaderText = "Hora";
                         
            dgv.Columns["MontoStr"].Visible = true;
            dgv.Columns["MontoStr"].Width = 100;
            dgv.Columns["MontoStr"].HeaderText = "Monto";

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (_clienteSeleccionado == null) return;

            CargarDatos();
        }

        private void dtpFechaDesde_ValueChanged(object sender, EventArgs e)
        {
            if (_clienteSeleccionado == null) return;
            CargarDatos();
        }

        private void dtpFechaHasta_ValueChanged(object sender, EventArgs e)
        {
            if (_clienteSeleccionado == null) return;
            CargarDatos();
        }

        private void rdbDeuda_CheckedChanged(object sender, EventArgs e)
        {
            if (_clienteSeleccionado == null) return;
            CargarDatos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
             if (_clienteSeleccionado == null) return;
            this.Close();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (_clienteSeleccionado == null) return;
            CargarDatos();
        }

        private void btnRealizarPago_Click(object sender, EventArgs e)
        {
            if (_clienteSeleccionado != null)
            {
                var fPagoDeuda = new _00035_ClientePagoCtaCte(_clienteSeleccionado);
                fPagoDeuda.ShowDialog();

                if (fPagoDeuda.RealizoPago)
                {
                    CargarDatos();
                }
            }
        }
    }
}
