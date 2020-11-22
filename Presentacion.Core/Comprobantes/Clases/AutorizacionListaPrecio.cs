using System;
using System.Windows.Forms;
using Aplicacion.Constantes;
using IServicio.Seguridad;


namespace Presentacion.Core.Comprobantes.Clases
{
    public partial class AutorizacionListaPrecio : Form
    {
        private readonly ISeguridadServicio _seguridad;
        
        private bool _tienePermiso;
        public bool PermisoAutorizado => _tienePermiso;

        public AutorizacionListaPrecio(ISeguridadServicio seguridad)
        {
            InitializeComponent();

            _seguridad = seguridad;
            _tienePermiso = false;
        }

        private void btnVerPassword_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = Char.MinValue;
        }

        private void btnVerPassword_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = Char.Parse("*");
        }

       private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {

                if (UsuarioAdmin.Usuario == txtUsuario.Text && UsuarioAdmin.Password == txtPassword.Text)
                {
                    _tienePermiso = true;

                    this.Close();

                    return;
                }

                _tienePermiso = _seguridad.VerificarAcceso(txtUsuario.Text, txtPassword.Text);

                if (_tienePermiso)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El usuario o el Password son Icorrectos");
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _tienePermiso = false;
            Close();
        }
    }
}
