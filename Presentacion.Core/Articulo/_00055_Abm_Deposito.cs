using System.Windows.Forms;
using IServicio.Deposito;
using IServicio.Deposito.DTOs;
using PresentacionBase.Formularios;
using StructureMap;
using static Aplicacion.Constantes.ValidacionDatosEntrada;

namespace Presentacion.Core.Articulo
{
    public partial class _00055_Abm_Deposito : FormAbm
    {
        private readonly IDepositoSevicio _depositoServicio;

        public _00055_Abm_Deposito(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _depositoServicio = ObjectFactory.GetInstance<IDepositoSevicio>();

            AsignarEvento_EnterLeave(this);

            txtDescripcion.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoSimbolos(sender, args);
            };

            txtUbicacion.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoSimbolos(sender, args);
            };
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var resultado = (DepositoDto)_depositoServicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado");
                    Close();
                }

                txtDescripcion.Text = resultado.Descripcion;
                txtUbicacion.Text = resultado.Ubicacion;

                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);
            }
            else
            {
                btnEjecutar.Text = "Nuevo";
            }
        }

        public override bool VerificarDatosObligatorios()
        {
            return !string.IsNullOrEmpty(txtDescripcion.Text);
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _depositoServicio.VerificarSiExiste(txtDescripcion.Text, id);
        }

        public override void EjecutarComandoNuevo()
        {
            _depositoServicio.Insertar(new DepositoDto
            {
                Descripcion = txtDescripcion.Text, 
                Ubicacion = txtUbicacion.Text,
                Eliminado = false
            });
        }

        public override void EjecutarComandoModificar()
        {
            _depositoServicio.Modificar(new DepositoDto
            {
                Id = EntidadId.Value,
                Descripcion = txtDescripcion.Text,
                Ubicacion = txtUbicacion.Text,
                Eliminado = false
            });
        }

        public override void EjecutarComandoEliminar()
        {
            _depositoServicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);

            txtDescripcion.Focus();
        }
    }
}
