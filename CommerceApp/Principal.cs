using System;
using System.Windows.Forms;
using Aplicacion.Constantes;
using IServicios.Caja;
using Presentacion.Core.Articulo;
using Presentacion.Core.Banco;
using Presentacion.Core.Caja;
using Presentacion.Core.Cliente;
using Presentacion.Core.Comprobantes;
using Presentacion.Core.CondicionIva;
using Presentacion.Core.Configuracion;
using Presentacion.Core.Departamento;
using Presentacion.Core.Empleado;
using Presentacion.Core.FormaPago;
using Presentacion.Core.Localidad;
using Presentacion.Core.Provincia;
using Presentacion.Core.Sesion;
using Presentacion.Core.Usuario;
using PresentacionBase.Formularios;
using StructureMap;

namespace CommerceApp
{
    public partial class Principal : Form
    {

        private readonly ICajaServicio _cajaServicio;
        public Principal(ICajaServicio cajaServicio)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            _cajaServicio = cajaServicio;

            Verificacion();
            sidebar.Visible = false;
            pBox.Cursor = Cursors.Default;
            itemArticulo.Visible = false;
            itemCaja.Visible = false;
           
        }
      
        public void Verificacion()
        {
            if (Identidad.Usuario == null || Identidad.Usuario == string.Empty)
            {

                administraciónToolStripMenuItem.Enabled = false;
                lblNombreEmpleado.Visible = false;
                imgImagenCliente.Visible = false;

                btnArticulo.Visible = false;
                btnCaja.Visible = false;
                btnCliente.Visible = false;
                btnCompra.Visible = false;
                btnVentas.Visible = false;
                btnConfiguracion.Visible = false;

                itemArticulo.Visible = false;
                itemCaja.Visible = false;

                iniciarSesionToolStripMenuItem.Text = "Log-In";
                return;
            }
            
            administraciónToolStripMenuItem.Enabled = true;
            iniciarSesionToolStripMenuItem.Text = "Log-Out";

            btnArticulo.Visible = true;
            btnCaja.Visible = true;
            btnCliente.Visible = true;
            btnCompra.Visible = true;
            btnVentas.Visible = true;
            btnConfiguracion.Visible = true;

            lblNombreEmpleado.Visible = true;
            imgImagenCliente.Visible = true;

            itemArticulo.Visible = true;
            itemCaja.Visible = true;
            lblNombreEmpleado.Text = $"{Identidad.Nombre } {Identidad.Apellido}";
            imgImagenCliente.Image = Imagen.ConvertirImagen(Identidad.Foto);

            return;

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Identidad.Limpiar();
            Close();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (sidebar.Visible == false)
            {
                sidebar.Visible = true;

                return;
            }

            sidebar.Visible = false;
            return;
        }

        private void consultaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00001_Provincia>().ShowDialog();
        }

        private void nuevaProvinciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00002_Abm_Provincia(TipoOperacion.Nuevo).ShowDialog();
        }
        private void consultaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00003_Departamento>().ShowDialog();
        }

        private void nuevoDepartamentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00004_Abm_Departamento(TipoOperacion.Nuevo).ShowDialog();
        }
        private void consultaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00005_Localidad>().ShowDialog();
        }

        private void nuevaLocalidadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00006_AbmLocalidad(TipoOperacion.Nuevo).ShowDialog();
        }

        private void consultaDeCondicionIvaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00013_CondicionIva>().ShowDialog();
        }

        private void nuevaCondicionIvaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00014_Abm_CondicionIva(TipoOperacion.Nuevo).ShowDialog();
        }

        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00012_Configuracion>().ShowDialog();
        }

     
        private void consultaToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00019_Rubro>().ShowDialog();
        }

        private void nuevoRubroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00020_Abm_Rubro(TipoOperacion.Nuevo).ShowDialog();
        }

        private void consultaToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00021_Marca>().ShowDialog();
        }

        private void nuevaMarcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00022_Abm_Marca(TipoOperacion.Nuevo).ShowDialog();
        }

        private void consultaToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00023_UnidadDeMedida>().ShowDialog();
        }
        private void nuevaUnidadDeMedidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00024_Abm_UnidadDeMedida(TipoOperacion.Nuevo).ShowDialog();
        }

        private void consultaToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00025_Iva>().ShowDialog();
        }

        private void nuevoIvaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00026_Abm_Iva(TipoOperacion.Nuevo).ShowDialog();
        }

       

        private void consultaToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00051_PuestoTrabajo>().ShowDialog();
        }

        private void nuevoPuestoDeTrabajoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00052_Abm_PuestoTrabajo(TipoOperacion.Nuevo).ShowDialog();
        }

        private void crearEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00007_Empleado>().ShowDialog();
        }

        private void nuevoEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00008_Abm_Empleado(TipoOperacion.Nuevo).ShowDialog();
        }

        private void buscarEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<EmpleadoLookUp>().ShowDialog();
        }

        
        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Identidad.Usuario == null || Identidad.Usuario == string.Empty)
            {
                ObjectFactory.GetInstance<Presentacion.Core.Sesion.InicioSesion>().ShowDialog();
                Verificacion();
                return;

            }

            Identidad.Limpiar();
            Verificacion();
            
        }

        private void consultaToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00011_Usuario>().ShowDialog();
        }

        private void consultaToolStripMenuItem10_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00009_Cliente>().ShowDialog();
        }

        private void nuevoClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00010_Abm_Cliente(TipoOperacion.Nuevo).ShowDialog();
        }

        private void consultaToolStripMenuItem11_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00032_ListaPrecio>().ShowDialog(); 
        }

        private void nuevaListaDePreciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00033_Abm_ListaPrecio(TipoOperacion.Nuevo).ShowDialog();
        }

        private void consultaToolStripMenuItem12_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00031_ActualizarPrecios>().ShowDialog();
        }

       

      

        private void btnAministracion_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00012_Configuracion>().ShowDialog();
        }

        private void btnArticulo_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00017_Articulo>().ShowDialog();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00009_Cliente>().ShowDialog();
        }


     
        private void consultaToolStripMenuItem15_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00017_Articulo>().ShowDialog();
        }

        private void nuevoArtículoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00018_Abm_Articulo(TipoOperacion.Nuevo).ShowDialog();
        }

    
        private void nuevoMotivoDeBajaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new _00030_Abm_BajaArticulos(TipoOperacion.Nuevo).ShowDialog();
        }

        private void consultaToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00029_BajaDeArticulos>().ShowDialog();
        }

        private void consultaToolStripMenuItem14_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00027_MotivoBaja>().ShowDialog();
        }

        private void nuevoMotivoDeBajaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new _00028_Abm_MotivoBaja(TipoOperacion.Nuevo).ShowDialog();
        }

        private void HoraFecha_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
            lblFecha.Text = DateTime.Now.ToShortDateString();
        }

        private void consultaToolStripMenuItem8_Click_1(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00054_Banco>().ShowDialog();
        }
        
        private void nuevoBancoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00055_Abm_Banco(TipoOperacion.Nuevo).ShowDialog();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            if (Identidad.EmpleadoId == 0 || Identidad.UsuarioId == 0)
            {
                MessageBox.Show("ACCESO DENEGADO");
                return;
            }

            if (_cajaServicio.VerificarSiExisteCajaAbierta(Identidad.UsuarioId))
            {
                ObjectFactory.GetInstance<_00050_Venta>().Show();
            }
            else
            {
                if (MessageBox.Show("La caja aún no fue abierta. Desea abrir una?", "Atencion",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    var fCaja = ObjectFactory.GetInstance<_00039_AperturaCaja>();
                    fCaja.ShowDialog();
                    if (fCaja.CajaAbierta)
                    {
                        ObjectFactory.GetInstance<_00050_Venta>().Show();
                    }
                }
            }
        }

        private void cobroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00049_CobroDiferido>().ShowDialog();


        }

        private void btnCaja_Click(object sender, EventArgs e)
        {
            if (!_cajaServicio.VerificarSiExisteCajaAbierta(Identidad.UsuarioId))
            {
                ObjectFactory.GetInstance<_00039_AperturaCaja>().ShowDialog();
            }
            else
            {
                MessageBox.Show($"Ya se encuentra abierta una caja para el usuario {Identidad.Nombre} {Identidad.Apellido}");
            }
        }

        private void consultaToolStripMenuItem9_Click_1(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00038_Caja>().ShowDialog();
        }

        private void presupuestosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00056_Presupuestos>().ShowDialog();
        }

        private void consultaToolStripMenuItem13_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00045_Tarjeta>().ShowDialog();
        }

        private void nuevaTarjetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new _00046_Abm_tarjeta(TipoOperacion.Nuevo).ShowDialog();
        }

        private void pagoCtaCteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance < _00034_ClienteCtaCte>().ShowDialog();
        }
    
    }
}
