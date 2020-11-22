using Dominio.UnidadDeTrabajo;

using IServicio.Configuracion;
using IServicio.Configuracion.DTOs;
using IServicio.Persona;
using IServicio.Persona.DTOs;
using IServicio.Usuario;
using IServicio.Usuario.DTOs;
using IServicios.Mail;
using PresentacionBase.Formularios;
using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using static Aplicacion.Constantes.Clases.Password;

namespace Presentacion.Core.Usuario
{
    public partial class _00011_Usuario : Form
    {
        protected long? empleadoId = null;
        protected long? usuarioId = null;
        protected object EntidadSeleccionada;

        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IEmpleadoServicio _empleadoServicio;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private readonly IConfiguracionServicio _configuracionServicio;

        private readonly IMailServicio _mailCliente;

        private ConfiguracionDto configuracion;

        public _00011_Usuario(IUsuarioServicio usuarioServicio,
            IEmpleadoServicio empleadoServicio,
            IUnidadDeTrabajo unidadDeTrabajo,
            IConfiguracionServicio configuracionServicio,
            IMailServicio mailCliente)
        {
            InitializeComponent();
            _usuarioServicio = usuarioServicio;
            _empleadoServicio = empleadoServicio;
            _unidadDeTrabajo = unidadDeTrabajo;
            _configuracionServicio = configuracionServicio;

            _mailCliente = mailCliente;

            configuracion = null;
        }
        private void _00011_Usuario_Load(object sender, EventArgs e)
        {
            ActualizarDatos(dgvGrilla, string.Empty);
            
        }
        public void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _usuarioServicio.Obtener(string.Empty, false);
            FormatearGrilla(dgvGrilla);
        }
        
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ActualizarDatos(dgvGrilla, string.Empty);
        }
      

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarDatos(dgvGrilla, string.Empty);
            txtBuscar.Clear();
            txtBuscar.Focus();
        }

        private void btnBloqDesbloq_Click(object sender, EventArgs e)
        {
            _usuarioServicio.Bloquear(empleadoId.Value);
            ActualizarDatos(dgvGrilla, string.Empty);
        }

        public void FormatearGrilla(DataGridView dgv)
        {
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].Visible = false;

                dgv.Columns[i].HeaderCell.Style.Alignment
                    = DataGridViewContentAlignment.MiddleCenter;
            }

            dgv.Columns["ApyNomEmpleado"].Visible = true;
            dgv.Columns["ApyNomEmpleado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ApyNomEmpleado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["ApyNomEmpleado"].HeaderText = "Nombre";
            dgv.Columns["ApyNomEmpleado"].DisplayIndex = 0;

            dgv.Columns["NombreUsuario"].Visible = true;
            dgv.Columns["NombreUsuario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["NombreUsuario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["NombreUsuario"].HeaderText = @"NombreUsuario";
            dgv.Columns["NombreUsuario"].DisplayIndex = 1;

            dgv.Columns["EstaBloqueado"].Visible = true;
            dgv.Columns["EstaBloqueado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["EstaBloqueado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["EstaBloqueado"].HeaderText = @"EstaBloqueado";
            dgv.Columns["EstaBloqueado"].DisplayIndex = 2;

        }
        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;

            empleadoId = (long)dgvGrilla["EmpleadoId", e.RowIndex].Value;
            usuarioId = (long)dgvGrilla["Id", e.RowIndex].Value;
            // Obtener el Objeto completo seleccionado
            EntidadSeleccionada = dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
              _usuarioServicio.ResetPassword(empleadoId.Value);
                MessageBox.Show("Password reseteado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = _unidadDeTrabajo.UsuarioRepositorio.Obtener(usuarioId.Value);

                if (usuario == null)
                {
                    var empleado = _empleadoServicio.Obtener(typeof(EmpleadoDto), (long)empleadoId);

                    var nuevoUsuario = _usuarioServicio.Crear(empleado.Id, empleado.Apellido, empleado.Nombre);

                    configuracion = _configuracionServicio.Obtener();

                
                    if (MessageBox.Show("¿Deséa enviar su cuenta por correo?", "Atencion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        try
                        {
                           _mailCliente.MandarMailUsuario(empleado.Mail, configuracion.RazonSocial, nuevoUsuario.Nombre, PasswordPorDefecto);
                            MessageBox.Show("Mensaje enviado", "Done");
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(ex.Message);
                        }       

                    }

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
