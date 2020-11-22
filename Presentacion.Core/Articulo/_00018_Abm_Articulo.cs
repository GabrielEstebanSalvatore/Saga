using Aplicacion.Constantes;
using IServicio.Articulo;
using IServicio.Articulo.DTOs;
using IServicio.Iva;
using IServicio.Marca;
using IServicio.Rubro;
using IServicio.UnidadMedida;
using IServicios.Articulo.DTOs;
using PresentacionBase.Formularios;
using StructureMap;
using System.Windows.Forms;

namespace Presentacion.Core.Articulo
{
    public partial class _00018_Abm_Articulo : FormAbm
    {
        private readonly IArticuloServicio _articuloServicio;
        private readonly IRubroServicio _rubroServicio;
        private readonly IMarcaServicio _marcaServicio;
        private readonly IUnidadMedidaServicio _unidadMedidaServicio;
        private readonly IIvaServicio _ivaServicio;

     
        public _00018_Abm_Articulo(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _articuloServicio = ObjectFactory.GetInstance<IArticuloServicio>();
            _marcaServicio = ObjectFactory.GetInstance<IMarcaServicio>();
            _rubroServicio = ObjectFactory.GetInstance<IRubroServicio>();
            _unidadMedidaServicio = ObjectFactory.GetInstance<IUnidadMedidaServicio>();
            _ivaServicio = ObjectFactory.GetInstance<IIvaServicio>();

            PoblarComboBox(cmbMarca, _marcaServicio.Obtener(string.Empty),
            "Descripcion",
            "Id");
            cmbMarca.SelectedItem = null;

            PoblarComboBox(cmbRubro, _rubroServicio.Obtener(string.Empty),
                "Descripcion",
                "Id");
            cmbRubro.SelectedItem = null;

            PoblarComboBox(cmbUnidad, _unidadMedidaServicio.Obtener(string.Empty),
                "Descripcion",
                "Id");
            cmbUnidad.SelectedItem = null;

            PoblarComboBox(cmbIva, _ivaServicio.Obtener(string.Empty),
                "Descripcion",
                "Id");
            cmbIva.SelectedItem = null;
        }

        public override void CargarDatos(long? entidadId)
        {
   

            if (entidadId.HasValue)//Eliminar o Modificar
            {
                groupPrecio.Enabled = false;
                nudStock.Enabled = false;

               var resultado = (ArticuloDto)_articuloServicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al Obtener el registro seleccionado");
                    Close();
                }

                // ==================================================== // 
                // =============== Datos del Articulo ========== // 
                // ==================================================== // 

                txtCodigo.Text = resultado.Codigo.ToString();
                txtcodigoBarra.Text = resultado.CodigoBarra;
                txtDescripcion.Text = resultado.Descripcion;
                txtAbreviatura.Text = resultado.Abreviatura;
                txtDetalle.Text = resultado.Detalle;
                txtUbicacion.Text = resultado.Ubicacion;
                cmbMarca.SelectedValue = resultado.MarcaId;
                cmbRubro.SelectedValue = resultado.RubroId;
                cmbUnidad.SelectedValue = resultado.UnidadMedidaId;
                cmbIva.SelectedValue = resultado.IvaId;

                // ==================================================== // 
                // =============== Datos del Generales ========== // 
                // ==================================================== // 

                nudStockMin.Value = resultado.StockMinimo;
                chkDescontarStock.Checked = resultado.DescuentaStock;
                chkPermitirStockNeg.Checked = resultado.PermiteStockNegativo;
                chkActivarLimite.Checked = resultado.ActivarLimiteVenta;
                nudLimiteVenta.Value = resultado.LimiteVenta;
                chkActivarHoraVenta.Checked = resultado.ActivarHoraVenta;
                dtpHoraDesde.Value = resultado.HoraLimiteVentaDesde;
                dtpHoraHasta.Value = resultado.HoraLimiteVentaHasta;

                // ==================================================== // 
                // =============== Foto del Articulo ========== //
                // ==================================================== // 

                imgFoto.Image = Imagen.ConvertirImagen(resultado.Foto);
                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);
            }

            else // Nuevo
            {
                btnEjecutar.Text = "Grabar";
                LimpiarControles(this);
            }
        }

        public override bool VerificarDatosObligatorios()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text)) return false;

            if (string.IsNullOrEmpty(txtcodigoBarra.Text)) return false;

            if (string.IsNullOrEmpty(txtDescripcion.Text)) return false;

            if (cmbMarca.Items.Count <= 0) return false;

            if (cmbRubro.Items.Count <= 0) return false;

            if (cmbUnidad.Items.Count <= 0) return false;

            if (cmbIva.Items.Count <= 0) return false;

            return true;
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _articuloServicio.VerificarSiExiste(txtDescripcion.Text, id);
        }
        public override void EjecutarComandoNuevo()
        {
            var articuloNuevo = new ArticuloCrudDto { };


            //------------Datos Articulos----------//

            articuloNuevo.Codigo = int.Parse(txtCodigo.Text);
            articuloNuevo.CodigoBarra = txtcodigoBarra.Text;
            articuloNuevo.Descripcion = txtDescripcion.Text;
            articuloNuevo.Abreviatura = txtAbreviatura.Text;

            //codigoProveedor
            articuloNuevo.Detalle = txtDetalle.Text;
            articuloNuevo.Ubicacion = txtUbicacion.Text;
            articuloNuevo.MarcaId = (long)cmbMarca.SelectedValue;
            articuloNuevo.RubroId = (long)cmbRubro.SelectedValue;
            articuloNuevo.UnidadMedidaId = (long)cmbUnidad.SelectedValue;
            articuloNuevo.IvaId = (long)cmbIva.SelectedValue;
            articuloNuevo.PrecioCosto = (decimal) nudPrecioCosto.Value;

            //------------Datos Generales----------//
            articuloNuevo.StockMinimo = nudStockMin.Value;
            articuloNuevo.StockActual = nudStock.Value;
            articuloNuevo.ActivarHoraVenta = chkActivarHoraVenta.Checked;
            articuloNuevo.ActivarLimiteVenta = chkActivarLimite.Checked;
            articuloNuevo.LimiteVenta = nudLimiteVenta.Value;
            articuloNuevo.HoraLimiteVentaDesde = dtpHoraDesde.Value;
            articuloNuevo.HoraLimiteVentaHasta = dtpHoraHasta.Value;
            articuloNuevo.PermiteStockNegativo = chkPermitirStockNeg.Checked;
            articuloNuevo.DescuentaStock = chkDescontarStock.Checked;
                
            articuloNuevo.Foto = Imagen.ConvertirImagen(imgFoto.Image);
            articuloNuevo.Eliminado = false;
                

            _articuloServicio.Insertar(articuloNuevo);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new ArticuloCrudDto
            {
                Id = EntidadId.Value,
                Codigo = int.Parse(txtCodigo.Text),
                CodigoBarra = txtcodigoBarra.Text,
                Descripcion = txtDescripcion.Text,
                Abreviatura = txtAbreviatura.Text,
                Detalle = txtDetalle.Text,
                Ubicacion = txtUbicacion.Text,
                MarcaId = (long)cmbMarca.SelectedValue,
                RubroId = (long)cmbRubro.SelectedValue,
                UnidadMedidaId = (long)cmbUnidad.SelectedValue,
                IvaId = (long)cmbIva.SelectedValue,
                PrecioCosto = nudPrecioCosto.Value, 
                //------------------------------------------------// 
                StockActual = nudStock.Value, 
                StockMinimo = nudStockMin.Value,
                DescuentaStock = chkDescontarStock.Checked,
                PermiteStockNegativo = chkPermitirStockNeg.Checked,
                ActivarLimiteVenta = chkActivarLimite.Checked,
                LimiteVenta = nudLimiteVenta.Value,
                ActivarHoraVenta = chkActivarHoraVenta.Checked,
                HoraLimiteVentaDesde = dtpHoraDesde.Value,
                HoraLimiteVentaHasta = dtpHoraHasta.Value,
                //------------------------------------------------// 
                Foto = Imagen.ConvertirImagen(imgFoto.Image),
                Eliminado = false
            };

            _articuloServicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            _articuloServicio.Eliminar(EntidadId.Value);
        }
        
        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj, tieneValorAsociado); 
            txtCodigo.Text = _articuloServicio.ObtenerSiguienteNroCodigo().ToString();
            txtcodigoBarra.Focus();
        }
        

        private void btnNuevaMarca_Click(object sender, System.EventArgs e)
        {
            var formNuevaMarca = new _00022_Abm_Marca(TipoOperacion.Nuevo);
            formNuevaMarca.ShowDialog();

            if (formNuevaMarca.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbMarca, _marcaServicio.Obtener(string.Empty),
                "Descripcion",
                "Id");
            }
        }


        private void btnNuevoRubro_Click(object sender, System.EventArgs e)
        {
            var formNuevaRubro = new _00020_Abm_Rubro(TipoOperacion.Nuevo);
            formNuevaRubro.ShowDialog();

            if (formNuevaRubro.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbRubro, _rubroServicio.Obtener(string.Empty),
                "Descripcion",
                "Id");
            }
        }

        private void btnNuevaUnidad_Click(object sender, System.EventArgs e)
        {
            var formNuevaUnidadMedida = new _00024_Abm_UnidadDeMedida(TipoOperacion.Nuevo);
            formNuevaUnidadMedida.ShowDialog();

            if (formNuevaUnidadMedida.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbUnidad, _unidadMedidaServicio.Obtener(string.Empty),
               "Descripcion",
               "Id");
            }
        }

        private void btnNuevoIva_Click(object sender, System.EventArgs e)
        {
            var formNuevoIva = new _00026_Abm_Iva(TipoOperacion.Nuevo);
            formNuevoIva.ShowDialog();

            if (formNuevoIva.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbIva, _ivaServicio.Obtener(string.Empty),
                "Descripcion",
                "Id");
            }
        }

      
        private void btnAgregarImagen_Click(object sender, System.EventArgs e)
        {

            OpenFileDialog getImage = new OpenFileDialog();

            getImage.InitialDirectory = @"C:\\";
            getImage.Filter = "Archivos de Imagenes (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (getImage.ShowDialog() == DialogResult.OK)
            {
                imgFoto.ImageLocation = getImage.FileName;
            }
            else
            {
                MessageBox.Show("No se seleccionó ninguna imagen");
            }
        }

        private void chkActivarLimite_CheckedChanged(object sender, System.EventArgs e)
        {
            nudLimiteVenta.Enabled = !nudLimiteVenta.Enabled;
        }

        private void chkActivarHoraVenta_CheckedChanged(object sender, System.EventArgs e)
        {
            dtpHoraDesde.Enabled = !dtpHoraDesde.Enabled;
            dtpHoraHasta.Enabled = !dtpHoraHasta.Enabled;
        }
    }
}
