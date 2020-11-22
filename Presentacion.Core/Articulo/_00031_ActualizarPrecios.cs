using System;
using System.Windows.Forms;
using IServicio.Articulo;
using IServicio.ListaPrecio;
using IServicio.Marca;
using IServicio.Rubro;
using IServicios.Precios;
using PresentacionBase.Formularios;

namespace Presentacion.Core.Articulo
{
    public partial class _00031_ActualizarPrecios : FormBase
    {
        private readonly IArticuloServicio _articuloServicio;

        private readonly IMarcaServicio _marcaServicio;

        private readonly IRubroServicio _rubroServicio;

        private readonly IListaPrecioServicio _listaPrecioServicio;

        private readonly IPrecioServicio _precioServicio;

        public _00031_ActualizarPrecios(IArticuloServicio articuloServicio,
            IMarcaServicio marcaServicio,
            IRubroServicio rubroServicio,
            IListaPrecioServicio listaPrecioServicio,
            IPrecioServicio precioServicio)
        {
            InitializeComponent();
            _articuloServicio = articuloServicio;
            _marcaServicio = marcaServicio;
            _rubroServicio = rubroServicio;
            _listaPrecioServicio = listaPrecioServicio;
            _precioServicio = precioServicio;


            PoblarComboBox(cmbMarca, _marcaServicio.Obtener(string.Empty),"Descripcion","Id");
            //cmbMarca.SelectedItem = null;

            PoblarComboBox(cmbRubro, _rubroServicio.Obtener(string.Empty),"Descripcion","Id");
           // cmbRubro.SelectedItem = null;

            PoblarComboBox(cmbListaPrecio, _listaPrecioServicio.Obtener(string.Empty),"Descripcion","Id");
            //cmbListaPrecio.SelectedItem = null;
        }

        private void btnSalir_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

       
        private void btnLimpiar_Click(object sender, System.EventArgs e)
        {
            dtpFechaActualizacion.MinDate = DateTime.Today;
            dtpFechaActualizacion.Value = DateTime.Now;

            dtpHora.Value = DateTime.Now;

            nudCodigoDesde.Value = 0;
            nudCodigoHasta.Value = 0;

            chkRubro.Checked = false;
            chkArticulo.Checked = false;
            chkMarca.Checked = false;
            chkListaPrecio.Checked = false;

            nudValor.Value = 0;

            rdbPorcentaje.Checked = false;
            RdbPrecio.Checked = true;

        }

        private void _00031_ActualizarPrecios_Load(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void chkMarca_CheckedChanged(object sender, EventArgs e)
        {
            cmbMarca.Enabled = chkMarca.Checked;
        }

        private void chkRubro_CheckedChanged(object sender, EventArgs e)
        {
            cmbRubro.Enabled = chkRubro.Checked;
        }

        private void chkArticulo_CheckedChanged(object sender, EventArgs e)
        {
            nudCodigoDesde.Enabled = chkArticulo.Checked;
            nudCodigoHasta.Enabled = chkArticulo.Checked;
        }

        private void chkListaPrecio_CheckedChanged(object sender, EventArgs e)
        {
            cmbListaPrecio.Enabled = chkListaPrecio.Checked;
        }

        private void btnEjecutar_Click(object sender, System.EventArgs e)
        {
            try
            {
                var rubroId = chkRubro.Checked
                    ? (long)cmbRubro.SelectedValue
                    : (long?)null;

                var marcaId = chkMarca.Checked
                    ? (long)cmbMarca.SelectedValue
                    : (long?)null;

                var listaPrecioId = chkListaPrecio.Checked
                    ? (long)cmbListaPrecio.SelectedValue
                    : (long?)null;

                var codigoDesde = chkArticulo.Checked
                    ? Convert.ToInt32(nudCodigoDesde.Value)
                    : (int?)null;

                var codigoHasta = chkArticulo.Checked
                    ? Convert.ToInt32(nudCodigoHasta.Value)
                    : (int?)null;

                _precioServicio.ActualizarPrecio(nudValor.Value
                    , rdbPorcentaje.Checked
                    , marcaId
                    , rubroId
                    , codigoDesde
                    , codigoHasta);

                MessageBox.Show("Los Precios se Actualizaron Correctamente");
                Limpiar();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void Limpiar()
        {
            dtpFechaActualizacion.MinDate = DateTime.Today;
            dtpFechaActualizacion.Value = DateTime.Now;

            dtpHora.Value = DateTime.Now;

            nudCodigoDesde.Value = 0;
            nudCodigoHasta.Value = 0;

            chkRubro.Checked = false;
            chkArticulo.Checked = false;
            chkMarca.Checked = false;
            chkListaPrecio.Checked = false;

            nudValor.Value = 0;

            rdbPorcentaje.Checked = false;
            RdbPrecio.Checked = true;
        }

        
    }
}
