using IServicio.Persona;
using IServicio.Persona.DTOs;
using IServicio.Usuario;
using PresentacionBase.Formularios;
using System;
using System.Windows.Forms;

namespace Presentacion.Core.Empleado
{
    public partial class EmpleadoLookUp : FormLookUp
    {
        private readonly IEmpleadoServicio _empleadoServicio;
        private readonly IUsuarioServicio _usuarioServicio;

        public EmpleadoLookUp(IEmpleadoServicio empleadoServicio,
            IUsuarioServicio usuarioServicio)
        {
            InitializeComponent();
            _empleadoServicio = empleadoServicio;
            _usuarioServicio = usuarioServicio;
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

        public override void btnCrearUsuario_Click(object sender, EventArgs e)
        {
           var empleado =  _empleadoServicio.Obtener(typeof(EmpleadoDto), (long) entidadId);
            try
            {
                _usuarioServicio.Crear(empleado.Id, empleado.Apellido, empleado.Nombre);
                MessageBox.Show("Usuario creado correctamente");
            }
            catch (Exception)
            {

                MessageBox.Show("No se pudo crear el usuario");
            }
            
        }

    }
}
