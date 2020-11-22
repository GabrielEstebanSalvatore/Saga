using Aplicacion.Constantes;
using Dominio.UnidadDeTrabajo;
using IServicio.Configuracion;
using IServicio.Configuracion.DTOs;
using IServicio.Persona;
using IServicio.Persona.DTOs;
using IServicio.Usuario;
using PresentacionBase.Formularios;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using static Aplicacion.Constantes.Clases.Password;

namespace Presentacion.Core.Empleado
{
    public partial class EmpleadoLookUp : FormLookUp
    {
        private readonly IEmpleadoServicio _empleadoServicio;
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private readonly IConfiguracionServicio _configuracionServicio;

        private ConfiguracionDto configuracion;

        public EmpleadoLookUp(IEmpleadoServicio empleadoServicio,
            IUsuarioServicio usuarioServicio,
            IUnidadDeTrabajo unidadDeTrabajo,
            IConfiguracionServicio configuracionServicio)
        {
            InitializeComponent();
            _empleadoServicio = empleadoServicio;
            _usuarioServicio = usuarioServicio;
            _unidadDeTrabajo = unidadDeTrabajo;
            _configuracionServicio = configuracionServicio;

            configuracion = null;
            btnCrearUsuario.Visible = false;

        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _empleadoServicio.Obtener(typeof(EmpleadoDto), cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Legajo"].Visible = true;
            dgv.Columns["Legajo"].Width = 60;
            dgv.Columns["Legajo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Legajo"].HeaderText = "Legajo";
            dgv.Columns["Legajo"].DisplayIndex = 0;

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
            dgv.Columns["Mail"].DisplayIndex = 3;

            dgv.Columns["Eliminado"].Visible = true;
            dgv.Columns["Eliminado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Eliminado"].Width = 100;
            dgv.Columns["Eliminado"].HeaderText = "Eliminado";
            dgv.Columns["Eliminado"].DisplayIndex = 5;
        }

        public override void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;

            entidadId = (long)dgvGrilla["Id", e.RowIndex].Value;

            // Obtener el Objeto completo seleccionado
            EntidadSeleccionada = dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        public override void btnNuevo_Click(object sender, EventArgs e)
        {
            Expression<Func<Dominio.Entidades.Usuario, bool>> filtro = x =>
           x.EmpleadoId == entidadId.Value;
          
            var usuario = _unidadDeTrabajo.UsuarioRepositorio.Obtener(filtro);
            
            var empleado = _empleadoServicio.Obtener(typeof(EmpleadoDto), (long)entidadId);

            if (usuario != null)
            {
                btnCrearUsuario.Enabled = false;
            }
            else
            {
                btnCrearUsuario.Visible = false;
                btnCrearUsuario.Enabled = false;

            }

            txtApellido.Text = empleado.Apellido;
            txtNombre.Text = empleado.Nombre;
            txtDni.Text = empleado.Dni;
            txtTelefono.Text = empleado.Telefono;
            txtDomicilio.Text = empleado.Direccion;
            txtProvincia.Text = empleado.Provincia;
            txtDepartamento.Text = empleado.Departamento;
            txtLocalidad.Text = empleado.Localidad;
            txtMail.Text = empleado.Mail;
            
        }

        public override void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = _unidadDeTrabajo.UsuarioRepositorio.Obtener(entidadId.Value);

                if (usuario == null)
                {
                    var empleado = _empleadoServicio.Obtener(typeof(EmpleadoDto), (long)entidadId);

                    var nuevoUsuario = _usuarioServicio.Crear(empleado.Id, empleado.Apellido, empleado.Nombre);

                    MessageBox.Show("Usuario creado correctamente");

                    btnActualizar.PerformClick();

                    return;

                }

                MessageBox.Show("El usuario ya posee una cuenta");
                return;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    
    }
}
