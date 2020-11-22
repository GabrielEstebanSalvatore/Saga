using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IServicio.Persona;
using IServicio.Persona.DTOs;
using PresentacionBase.Formularios;


namespace Presentacion.Core.Cliente
{
    public partial class ClienteLookUp : FormLookUp
    {
        private readonly IClienteServicio _clienteServicio;

        public ClienteLookUp(IClienteServicio clienteServicio)
        {
            InitializeComponent();

            _clienteServicio = clienteServicio;
            EntidadSeleccionada = null;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            var clientes = (List<ClienteDto>)_clienteServicio
              .Obtener(typeof(ClienteDto), !string.IsNullOrEmpty(cadenaBuscar) ? cadenaBuscar : string.Empty, false);

            dgv.DataSource = clientes.Where(x => x.Dni != Aplicacion.Constantes.Cliente.ConsumidorFinal).ToList();

            FormatearGrilla(dgv);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["ApyNom"].Width = 100;
            dgv.Columns["ApyNom"].HeaderText = "Apellido y Nombre";
            dgv.Columns["ApyNom"].Visible = true;
            dgv.Columns["ApyNom"].DisplayIndex = 0;

            dgv.Columns["Dni"].Width = 100;
            dgv.Columns["Dni"].HeaderText = "DNI";
            dgv.Columns["Dni"].Visible = true;
            dgv.Columns["Dni"].DisplayIndex = 1;

            dgv.Columns["Telefono"].Width = 100;
            dgv.Columns["Telefono"].HeaderText = "Teléfono";
            dgv.Columns["Telefono"].Visible = true;
            dgv.Columns["Telefono"].DisplayIndex = 2;

            dgv.Columns["CtaCteStr"].Width = 100;
            dgv.Columns["CtaCteStr"].HeaderText = "Cta Cte";
            dgv.Columns["CtaCteStr"].Visible = true;
            dgv.Columns["CtaCteStr"].DisplayIndex =3;
            dgv.Columns["CtaCteStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["LimiteCompraStr"].Width = 70;
            dgv.Columns["LimiteCompraStr"].HeaderText = "Limite";
            dgv.Columns["LimiteCompraStr"].Visible = true;
            dgv.Columns["LimiteCompraStr"].DisplayIndex = 4;
            dgv.Columns["LimiteCompraStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["MontoMaximoCtaCteStr"].Width = 100;
            dgv.Columns["MontoMaximoCtaCteStr"].HeaderText = "Monto Limite";
            dgv.Columns["MontoMaximoCtaCteStr"].Visible = true;
            dgv.Columns["MontoMaximoCtaCteStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["MontoMaximoCtaCteStr"].DisplayIndex = 5;
        }
     
        public override void btnSalir_Click(object sender, EventArgs e)
        {
            base.btnSalir_Click(sender, e);

            Close();
        }
        public override void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            base.dgvGrilla_RowEnter(sender, e);

            var clienteSeleccionado = _clienteServicio.Obtener(typeof(ClienteDto), (long)entidadId);

            txtLegajo.Visible = false;
            lblLegajo.Visible = false;
            txtApellido.Text = clienteSeleccionado.Apellido;
            txtNombre.Text = clienteSeleccionado.Nombre;
            txtDni.Text = clienteSeleccionado.Dni;
            txtDomicilio.Text = clienteSeleccionado.Direccion;
            txtProvincia.Text = clienteSeleccionado.Provincia;
            txtDepartamento.Text = clienteSeleccionado.Departamento;
            txtLocalidad.Text = clienteSeleccionado.Localidad;
            txtMail.Text = clienteSeleccionado.Mail;
            txtTelefono.Text = clienteSeleccionado.Telefono;


        }
    }
}
