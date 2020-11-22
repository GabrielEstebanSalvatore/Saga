using Aplicacion.Constantes;
using IServicio.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Core.Sesion
{

    public partial class _00054_InicioSesion : Form
    {
        private readonly ISeguridadServicio _seguridadServicio;

        public bool puedeAccederAlSistema { get; set; }

        public _00054_InicioSesion(ISeguridadServicio seguridadServicio)
        {
            InitializeComponent();

            _seguridadServicio = seguridadServicio;

            //TODO - USUARIO
            txtUsuario.Text = "sgabriel";
            txtPassword.Text = "P$assword123";
        

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            if (UsuarioAdmin.Usuario == txtUsuario.Text && UsuarioAdmin.Password == txtPassword.Text)
            {
                Identidad.Usuario = "Administrador";

                puedeAccederAlSistema = true;
                
                this.Close();
                
                return; 
            }
            var puedeAcceder = _seguridadServicio.VerificarAcceso(txtUsuario.Text, txtPassword.Text);

            if (puedeAcceder == false)
            {
                MessageBox.Show("Usuario o Constraseña incorrectos");
                txtPassword.Clear();

            }
            else
            {
                var CurrentUsuario = _seguridadServicio.ObtenerUsuarioLogin(txtUsuario.Text);

                if (CurrentUsuario == null) throw new Exception("Ocurrió un error al obtener el usuario");

                if (CurrentUsuario.EstaBloqueado)
                {
                    MessageBox.Show($"El usuario {CurrentUsuario.ApyNomEmpleado} esta BLOQUEADO", 
                        "Atención", 
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Stop);

                    txtUsuario.Clear();
                    txtPassword.Clear();
                    return;               
                }

                Identidad.EmpleadoId = CurrentUsuario.EmpleadoId;
                Identidad.Nombre = CurrentUsuario.NombreEmpleado;
                Identidad.Apellido = CurrentUsuario.ApellidoEmpleado;
                Identidad.Foto = CurrentUsuario.FotoEmpleado;

                Identidad.UsuarioId = CurrentUsuario.Id;
                Identidad.Usuario = CurrentUsuario.NombreUsuario;

                puedeAccederAlSistema = puedeAcceder;

                this.Close();
                //ClienteSesion.CurrentUsuario = CurrentUsuario;
            }
        }
    }
}
