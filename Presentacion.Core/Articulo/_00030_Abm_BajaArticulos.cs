using IServicio.Articulo;
using IServicios.Articulo;
using IServicios.Articulo.DTOs;
using PresentacionBase.Formularios;
using StructureMap;
using System.Windows.Forms;

namespace Presentacion.Core.Articulo
{
    public partial class _00030_Abm_BajaArticulos : FormAbm
    {
        private readonly IMotivoBajaServicio _motivoBajaServicio;

        private readonly IArticuloServicio _articuloServicio;

        public _00030_Abm_BajaArticulos(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();
            _motivoBajaServicio = ObjectFactory.GetInstance<IMotivoBajaServicio>();
            _articuloServicio = ObjectFactory.GetInstance<IArticuloServicio>();
        }

        public override void CargarDatos(long? entidadId)
        {
            PoblarComboBox(cmbMotivoBaja,
                _motivoBajaServicio.Obtener(string.Empty),
                "Descripcion",
                "Id");

            PoblarComboBox(cmbArticulo,
               _articuloServicio.Obtener(string.Empty),
               "Descripcion",
               "Id");

            if (entidadId.HasValue) // Eliminar o Modificar
            {
                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);

                var entidad = (MotivoBajaDto)_motivoBajaServicio.Obtener(entidadId.Value);

                if (entidad == null)
                {
                    MessageBox.Show("Ocurrio un error al Obtener el registro seleccionado");
                    Close();
                }

                txtObservacion.Text = entidad.Descripcion;
            }
            else
            {
                txtObservacion.Clear();
                txtObservacion.Focus();
            }
        }

        public override void EjecutarComandoNuevo()
        {
            _motivoBajaServicio.Insertar(new MotivoBajaDto
            {
                Descripcion = txtObservacion.Text,
                Eliminado = false,
                ArticuloId = (long)cmbArticulo.SelectedValue,
                MotivoBajaId = (long)cmbMotivoBaja.SelectedValue,
            }) ;
        }

        public override void EjecutarComandoModificar()
        {
            _motivoBajaServicio.Modificar(new MotivoBajaDto
            {
                Id = EntidadId.Value,
                Descripcion = txtObservacion.Text,
                Eliminado = false,
                ArticuloId = (long)cmbArticulo.SelectedValue,
                MotivoBajaId = (long)cmbMotivoBaja.SelectedValue,
            });
        }
        public override void EjecutarComandoEliminar()
        {
            _motivoBajaServicio.Eliminar(EntidadId.Value);
        }

        public override bool VerificarDatosObligatorios()
        {
            if (string.IsNullOrEmpty(txtObservacion.Text))
                return false;

            if (cmbMotivoBaja.Items.Count <= 0)
                return false;

            return true;
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj, tieneValorAsociado);

            txtObservacion.Text = "";
        }

        private void cmbArticulo_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            var articuloSeleccionado = cmbArticulo.SelectedValue;

            var articuloBase = _articuloServicio.Obtener((long)articuloSeleccionado);

            var articuloDto = (ArticuloDto)articuloBase;

            nudStockActual.Value = articuloDto.StockActual;

            //PoblarComboBox(cmbMotivoBaja, _motivoBajaServicio.Obtener(string.Empty), "Descripcion", "Id");
        }


    } 
}
