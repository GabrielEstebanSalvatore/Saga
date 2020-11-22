using Aplicacion.Constantes;
using IServicio.Articulo;
using IServicio.Configuracion;
using IServicio.Configuracion.DTOs;
using IServicio.ListaPrecio;
using IServicio.ListaPrecio.DTOs;
using IServicio.Persona;
using IServicio.Persona.DTOs;
using IServicios.Articulo.DTOs;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;
using IServicios.Contador;
using IServicios.PuestoTrabajo;
using Presentacion.Core.Articulo;
using Presentacion.Core.Cliente;
using Presentacion.Core.Comprobantes.Clases;
using Presentacion.Core.Empleado;
using Presentacion.Core.FormaPago;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using static Aplicacion.Constantes.ValidacionDatosEntrada;

namespace Presentacion.Core.Comprobantes
{
    public partial class _00050_Venta : FormBase
    {
        private ClienteDto _clienteSeleccionado;
        private EmpleadoDto _vendedorSeleccionado;
        private ConfiguracionDto _configuracion;
        private FacturaView _factura;
        private ArticuloVentaDto _articuloSeleccionado;
        private ItemView _itemSeleccionado;

        private bool _permiteAgregarPorCantidad;
        private bool _articuloConPrecioAlternativo;
        private bool _autorizaPermisoListaPrecio;
        private bool _ingresoPorCodigoBascula;
        private bool _cambiarCantidadConErrorPorValidacion;

        private readonly IClienteServicio _clienteServicio;
        private readonly IPuestoTrabajoServicio _puestoTrabajoServicio;
        private readonly IListaPrecioServicio _listaPrecioServicio;
        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly IEmpleadoServicio _empleadoServicio;
        private readonly IContadorServicio _contadorServicio;
        private readonly IArticuloServicio _articuloServicio;
        private readonly IFacturaServicio _facturaServicio;

        public _00050_Venta(
            IClienteServicio clienteServicio,
            IPuestoTrabajoServicio puestoTrabajoServicio,
            IListaPrecioServicio listaPrecioServicio,
            IConfiguracionServicio configuracionServicio,
            IEmpleadoServicio empleadoServicio,
            IContadorServicio contadorServicio,
            IArticuloServicio articuloServicio,
            IFacturaServicio facturaServicio)
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // -------------------------------------------- Servicios  

            _clienteServicio = clienteServicio;
            _puestoTrabajoServicio = puestoTrabajoServicio;
            _listaPrecioServicio = listaPrecioServicio;
            _configuracionServicio = configuracionServicio;
            _empleadoServicio = empleadoServicio;
            _contadorServicio = contadorServicio;
            _articuloServicio = articuloServicio;
            _facturaServicio = facturaServicio;

            //----------------------------------------------

            dgvGrilla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvGrilla.AllowUserToResizeRows = false;

            txtCodigo.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoLetras(sender, args);
            };

            // ----------------------------- Asignacion de Variables Privadas 

            _clienteSeleccionado = null;
            _vendedorSeleccionado = null;
            _articuloSeleccionado = null;
            _itemSeleccionado = null;

            _permiteAgregarPorCantidad = false;
            _articuloConPrecioAlternativo = false;
            _autorizaPermisoListaPrecio = false;
            _ingresoPorCodigoBascula = false;
            _cambiarCantidadConErrorPorValidacion = false;

            _factura = new FacturaView();

            _configuracion = _configuracionServicio.Obtener();

            if (_configuracion == null)
            {
                MessageBox.Show("Antes de comenzar por favor cargue la configuracion del Sistema");
                Close();
            }
            // ----------------------------- Eventos 

        }

        private void _00050_Venta_Load(object sender, System.EventArgs e)
        {
            CargarCabecera();
            CargarCuerpo();
            CargarPie();
        }

        private void btnBuscarCliente_Click(object sender, System.EventArgs e)
        {
            var lookUpCliente = ObjectFactory.GetInstance<ClienteLookUp>();
            lookUpCliente.ShowDialog();
            if (lookUpCliente.EntidadSeleccionada != null)
             {
                 _clienteSeleccionado = (ClienteDto)
                 lookUpCliente.EntidadSeleccionada;
                 AsignarDatosCliente((ClienteDto)
                 lookUpCliente.EntidadSeleccionada);
             }
             else
             {
                 _clienteSeleccionado = ObtenerClienteConsumidorFinal();
                 AsignarDatosCliente(_clienteSeleccionado);
             }
        }
        private void cmbPuestoVenta_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            CambiarTituloDelPuntoVenta();
        }

        private void btnBuscarVendedor_Click(object sender, System.EventArgs e)
        {
            var lookUpVendedor = ObjectFactory.GetInstance<EmpleadoLookUp>();
            lookUpVendedor.ShowDialog();
            if (lookUpVendedor.EntidadSeleccionada != null)
            {
                _vendedorSeleccionado = (EmpleadoDto)
                lookUpVendedor.EntidadSeleccionada;
                AsignarDatosVendedor((EmpleadoDto)
                lookUpVendedor.EntidadSeleccionada);
            }
            else
            {
                _vendedorSeleccionado = ObtenerVendedorPorDefecto();
                AsignarDatosVendedor(_vendedorSeleccionado);
            }
        }
        /*
        private void cmbTipoComprobante_SelectionChangeCommitted(object sender, System.EventArgs e)
        {

           txtNroComprobante.Text = _contadorServicio.ObtenerSiguienteNumeroComprobante((TipoComprobante)cmbTipoComprobante.SelectedItem).ToString();

        }*/

        private void nudDescuento_ValueChanged(object sender, System.EventArgs e)
        {
            _factura.Descuento = nudDescuento.Value;
            CargarPie();
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (txtCodigo.Text.Contains("*"))
                    {
                        if (AsignarArticuloAlternativo(txtCodigo.Text))
                        {
                            btnAgregar.PerformClick();
                            return;
                        }
                    }

                    if (txtCodigo.Text.Length == 13)
                    {
                        if (_configuracion.ActivarBascula
                            && _configuracion.CodigoBascula == txtCodigo.Text.Substring(0, 4))
                        {
                            if (AsignarArticuloPorBascula(txtCodigo.Text))
                            {
                                btnAgregar.PerformClick();
                                return;
                            }
                        }
                        else
                        {
                            _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(txtCodigo.Text,
                                (long)cmbListaPrecio.SelectedValue,
                                _configuracion.DepositoVentaId);
                        }
                    }
                    else
                    {
                        _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(txtCodigo.Text,
                            (long)cmbListaPrecio.SelectedValue,
                            _configuracion.DepositoVentaId);
                    }

                    if (_articuloSeleccionado != null)
                    {
                        if (_permiteAgregarPorCantidad)
                        {
                            txtCodigo.Text = _articuloSeleccionado.CodigoBarra;
                            txtDescripcion.Text = _articuloSeleccionado.Descripcion;
                            txtPrecioUnitario.Text = _articuloSeleccionado.PrecioStr;
                            nudCantidad.Focus();
                            nudCantidad.Select(0, nudCantidad.Text.Length);
                            return;
                        }
                        else
                        {
                            btnAgregar.PerformClick();
                        }
                    }
                    else
                    {
                        LimpiarParaNuevoItem();
                    }
                }
            }

            e.Handled = false;
        }

        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                // F5
                case 116:
                    _permiteAgregarPorCantidad = !_permiteAgregarPorCantidad;
                    nudCantidad.Enabled = _permiteAgregarPorCantidad;
                    break;
                // F8
                case 119:

                    var lookUpArticulo = new ArticuloLookUp((long)cmbListaPrecio.SelectedValue);
                    lookUpArticulo.ShowDialog();

                    if (lookUpArticulo.EntidadSeleccionada != null)
                    {
                        _articuloSeleccionado = (ArticuloVentaDto)lookUpArticulo.EntidadSeleccionada;

                        if (_permiteAgregarPorCantidad)
                        {
                            txtCodigo.Text = _articuloSeleccionado.CodigoBarra;
                            txtDescripcion.Text = _articuloSeleccionado.Descripcion;
                            txtPrecioUnitario.Text = _articuloSeleccionado.PrecioStr;
                            nudCantidad.Focus();
                            nudCantidad.Select(0, nudCantidad.Text.Length);
                            return;
                        }
                        else
                        {
                            btnAgregar.PerformClick();
                            LimpiarParaNuevoItem();
                        }
                    }
                    else
                    {
                        LimpiarParaNuevoItem();
                    }

                    break;
            }
        }

        private void btnAgregar_Click(object sender, System.EventArgs e)
        {


            if (_articuloSeleccionado != null)
            {
                var listaPrecioSeleccionada = (ListaPrecioDto)cmbListaPrecio.SelectedItem;

                if (listaPrecioSeleccionada.NecesitaAutorizacion)
                {
                    if (!_autorizaPermisoListaPrecio)
                    {
                        var fAutorizacion = ObjectFactory.GetInstance<AutorizacionListaPrecio>();
                        fAutorizacion.ShowDialog();

                        if (!fAutorizacion.PermisoAutorizado) return;

                        _autorizaPermisoListaPrecio = fAutorizacion.PermisoAutorizado;
                        AgregarItem(_articuloSeleccionado, (long)cmbListaPrecio.SelectedValue, nudCantidad.Value);
                    }
                    else
                    {
                        AgregarItem(_articuloSeleccionado, (long)cmbListaPrecio.SelectedValue, nudCantidad.Value);
                    }
                }
                else
                {
                    AgregarItem(_articuloSeleccionado, (long)cmbListaPrecio.SelectedValue, nudCantidad.Value);
                }
            }
            else //TODO PRE PARCIAL
            {
                if (txtCodigo.Text.Contains("*"))
                {
                    if (AsignarArticuloAlternativo(txtCodigo.Text))
                    {
                        btnAgregar.PerformClick();
                        return;
                    }
                }

                if (txtCodigo.Text.Length == 13)
                {
                    if (_configuracion.ActivarBascula
                        && _configuracion.CodigoBascula == txtCodigo.Text.Substring(0, 4))
                    {
                        if (AsignarArticuloPorBascula(txtCodigo.Text))
                        {
                            btnAgregar.PerformClick();
                            return;
                        }
                    }
                    else
                    {
                        _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(txtCodigo.Text,
                            (long)cmbListaPrecio.SelectedValue,
                            _configuracion.DepositoVentaId);
                    }
                }
                else
                {
                    _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(txtCodigo.Text,
                        (long)cmbListaPrecio.SelectedValue,
                        _configuracion.DepositoVentaId);
                }

                if (_articuloSeleccionado != null)
                {
                    if (_permiteAgregarPorCantidad)
                    {
                        txtCodigo.Text = _articuloSeleccionado.CodigoBarra;
                        txtDescripcion.Text = _articuloSeleccionado.Descripcion;
                        txtPrecioUnitario.Text = _articuloSeleccionado.PrecioStr;
                        nudCantidad.Focus();
                        nudCantidad.Select(0, nudCantidad.Text.Length);
                        return;
                    }
                    else
                    {
                        if (_articuloSeleccionado != null)
                        {
                            var listaPrecioSeleccionada = (ListaPrecioDto)cmbListaPrecio.SelectedItem;

                            if (listaPrecioSeleccionada.NecesitaAutorizacion)
                            {
                                if (!_autorizaPermisoListaPrecio)
                                {
                                    var fAutorizacion = ObjectFactory.GetInstance<AutorizacionListaPrecio>();
                                    fAutorizacion.ShowDialog();

                                    if (!fAutorizacion.PermisoAutorizado) return;

                                    _autorizaPermisoListaPrecio = fAutorizacion.PermisoAutorizado;
                                    AgregarItem(_articuloSeleccionado, (long)cmbListaPrecio.SelectedValue, nudCantidad.Value);
                                }
                                else
                                {
                                    AgregarItem(_articuloSeleccionado, (long)cmbListaPrecio.SelectedValue, nudCantidad.Value);
                                }
                            }
                            else
                            {
                                AgregarItem(_articuloSeleccionado, (long)cmbListaPrecio.SelectedValue, nudCantidad.Value);
                            }
                        }
                    }
                }
                else
                {
                    LimpiarParaNuevoItem();
                }
            }

            LimpiarParaNuevoItem();
            CargarCuerpo();
            CargarPie();
        }

        private void AgregarItem(ArticuloVentaDto articulo, long listaPrecioId, decimal cantidad)
        {
            // Limite de Venta por cantidad
            if (articulo.TieneRestriccionPorCantidad)
            {
                var totalArticulosItems = _factura.Items
                    .Where(x => x.ArticuloId == articulo.Id)
                    .Sum(x => x.Cantidad);

                if (cantidad + totalArticulosItems > articulo.Limite)
                {
                    _cambiarCantidadConErrorPorValidacion = true;

                    var mensajeLimiteVenta = $"El articulo {articulo.Descripcion.ToUpper()} tiene una restricción"
                                  + Environment.NewLine
                                  + $"de Venta por una Cantidad Maxima de {articulo.Limite}.";

                    MessageBox.Show(mensajeLimiteVenta, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (articulo.TieneRestriccionHorario)
            {
                if (VerificarLimiteHorarioVenta(articulo.HoraDesde, articulo.HoraHasta))
                {
                    _cambiarCantidadConErrorPorValidacion = true;

                    var mensajeLimiteHorario = $"El articulo {articulo.Descripcion.ToUpper()} tiene una restricción"
                                             + Environment.NewLine
                                             + $"de Venta por horario entre {articulo.HoraDesde.ToShortTimeString()} hasta {articulo.HoraHasta.ToShortTimeString()}.";

                    MessageBox.Show(mensajeLimiteHorario, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (!articulo.PermiteStockNegativo)
            {
                if (!VerificarStock(articulo, nudCantidad.Value))
                {
                    _cambiarCantidadConErrorPorValidacion = true;

                    var mensajeStock = $"No hay Stock suficiente para el articulo {articulo.Descripcion.ToUpper()}"
                                       + Environment.NewLine
                                       + $"Stock Actual disponible: {articulo.Stock}.";

                    MessageBox.Show(mensajeStock, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (_articuloConPrecioAlternativo || _ingresoPorCodigoBascula)
            {
                _factura.Items.Add(AsignarDatosItem(articulo, listaPrecioId, cantidad));
            }
            else
            {
                if (_configuracion.UnificarRenglonesIngresarMismoProducto)
                {
                    var item = _factura.Items.FirstOrDefault(x => x.ArticuloId == articulo.Id
                                                                  && x.ListaPrecioId == listaPrecioId
                                                                  && (!x.EsArticuloAlternativo && !x.IngresoPorBascula));

                    if (item == null || item.EsArticuloAlternativo || item.IngresoPorBascula) // Primera vez por ingresar
                    {
                        _factura.Items.Add(AsignarDatosItem(articulo, listaPrecioId, cantidad));
                    }
                    else
                    {
                        item.Cantidad += cantidad;
                    }
                }
                else
                {
                    _factura.Items.Add(AsignarDatosItem(articulo, listaPrecioId, cantidad));
                }
            }
        }

        private ItemView AsignarDatosItem(ArticuloVentaDto articulo, long listaPrecioId, decimal cantidad)
        {
            _factura.ContadorItem++;
            return new ItemView
            {
                Id = _factura.ContadorItem,
                Descripcion = articulo.Descripcion,
                Iva = articulo.Iva,
                Precio = articulo.Precio,
                CodigoBarra = articulo.CodigoBarra,
                Cantidad = cantidad,
                ListaPrecioId = listaPrecioId,
                ArticuloId = articulo.Id,
                EsArticuloAlternativo = _articuloConPrecioAlternativo,
                IngresoPorBascula = _ingresoPorCodigoBascula
            };
        }

        private bool VerificarStock(ArticuloVentaDto articulo, decimal cantidad)
        {
            var totalArticulosItems = _factura.Items
                 .Where(x => x.ArticuloId == articulo.Id)
                 .Sum(x => x.Cantidad);

            return totalArticulosItems + cantidad <= articulo.Stock;
        }

        private bool VerificarLimiteHorarioVenta(DateTime limiteHoraDesde, DateTime limiteHoraHasta)
        {
            var _horaDesdeSistena = DateTime.Now.Hour;
            var _minutoDesdeSistema = DateTime.Now.Minute;
            // ------------------------------------------ //
            var _horaDesdeInicioDia = 0;
            var _minutoDesdeInicioDia = 0;
            // ------------------------------------------ //
            var _horaDesdeFinDia = 23;
            var _minutoDesdeFinDia = 59;


            if (limiteHoraDesde <= limiteHoraHasta) // Mismo Dia
            {
                if (_horaDesdeSistena >= limiteHoraDesde.Hour && _minutoDesdeSistema >= limiteHoraDesde.Minute)
                {
                    if (_horaDesdeSistena < limiteHoraHasta.Hour)
                    {
                        return true;
                    }
                    else if (_horaDesdeSistena == limiteHoraHasta.Hour && _minutoDesdeSistema <= limiteHoraHasta.Minute)
                    {
                        return true;
                    }
                }

            }
            else // Dias Diferentes -> Ej: 11:00 PM hasta 06:00 AM
            {
                if (_horaDesdeSistena >= limiteHoraDesde.Hour)
                {
                    // Rango 1
                    return _horaDesdeSistena >= limiteHoraDesde.Hour
                           && _minutoDesdeSistema >= limiteHoraDesde.Minute
                           && _horaDesdeSistena <= _horaDesdeFinDia
                           && _minutoDesdeSistema <= _minutoDesdeFinDia;
                }
                else
                {
                    // Rango 2

                    if (_horaDesdeSistena >= _horaDesdeInicioDia && _minutoDesdeSistema >= _minutoDesdeInicioDia)
                    {
                        if (_horaDesdeSistena < limiteHoraHasta.Hour)
                        {
                            return true;
                        }
                        else if (_horaDesdeSistena == limiteHoraHasta.Hour &&
                                 _minutoDesdeSistema <= limiteHoraHasta.Minute)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void nudCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAgregar.PerformClick();
            }
        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount > 0 )
            {
                _itemSeleccionado = (ItemView)dgvGrilla.Rows[e.RowIndex].DataBoundItem;
            }
            else
            {
                _itemSeleccionado = null;
            }
        }

        private void dgvGrilla_DoubleClick(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;


            var respaldoItem = _itemSeleccionado;
            var cantidadRespaldo = _itemSeleccionado.Cantidad;

            var articuloSelecionado = _articuloServicio.Obtener(respaldoItem.ArticuloId);
                              

            if (respaldoItem.IngresoPorBascula || respaldoItem.EsArticuloAlternativo)
            {
                MessageBox.Show("No se permite cambiar la cantidad");
                return;
            }

            var cambiarCantidadItem = new CambiarCantidad(_itemSeleccionado, (ArticuloDto)articuloSelecionado);

            cambiarCantidadItem.ShowDialog();

            if (cambiarCantidadItem.Item != null)
            {
                var item = _factura.Items.FirstOrDefault(x => x.Id == cambiarCantidadItem.Item.Id);

                _factura.Items.Remove(item);

                if (cambiarCantidadItem.Item.Cantidad > 0)
                {
                    _articuloSeleccionado = _articuloServicio
                        .ObtenerPorCodigo(_itemSeleccionado.CodigoBarra, _itemSeleccionado.ListaPrecioId, _configuracion.DepositoVentaId);

                    nudCantidad.Value = cambiarCantidadItem.Item.Cantidad;



                    //item.Cantidad 

                    btnAgregar.PerformClick();

                    if (_cambiarCantidadConErrorPorValidacion)
                    {
                        respaldoItem.Cantidad = cantidadRespaldo;
                        _factura.Items.Add(_itemSeleccionado);
                        _cambiarCantidadConErrorPorValidacion = false;
                    }
                }

            }

            LimpiarParaNuevoItem();
            CargarCuerpo();
            CargarPie();

        }

        private void btnEliminarItem_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;


            if (MessageBox.Show($"Esta seguro de eliminar el item {_itemSeleccionado.Descripcion}",
                "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                _factura.Items.Remove(_itemSeleccionado);
                LimpiarParaNuevoItem();
                CargarCuerpo();
                CargarPie();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var mensajeCancelar = "hay artículos cargados en la lista"
                + Environment.NewLine
                + "¿Desea cancelar la venta?";

            if (MessageBox.Show(mensajeCancelar, "Atencion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                == DialogResult.OK)
            {
                LimpiarParaNuevoFactura();
            }
        }

        // ----------------------------------------  Metodos Privados   


        private void CargarCabecera()
        {
            _clienteSeleccionado = ObtenerClienteConsumidorFinal();
            AsignarDatosCliente(_clienteSeleccionado);

            // ============================================================================= //

            lblFechaActual.Text = DateTime.Today.ToShortDateString();

            // ============================================================================= //

            PoblarComboBox(cmbTipoComprobante, Enum.GetValues(typeof(TipoComprobante)));
            cmbTipoComprobante.SelectedItem = TipoComprobante.B;

            //txtNroComprobante.Text = _contadorServicio.ObtenerSiguienteNumeroComprobante(TipoComprobante.B).ToString();

            // ============================================================================= //

            PoblarComboBox(cmbPuestoVenta,
                _puestoTrabajoServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id");

            if (cmbPuestoVenta.Items.Count == 0)
            {
                //TODO - PUESTO DE TRABAJO
                MessageBox.Show("Por favor Cargue primeramente los puntos de Ventas");
                var fPuestoTrabajo = ObjectFactory.GetInstance<_00051_PuestoTrabajo>();
                fPuestoTrabajo.Show();
                Close();
            }

            CambiarTituloDelPuntoVenta();

            // ============================================================================= //

            PoblarComboBox(cmbListaPrecio,
                _listaPrecioServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id");

            if (cmbListaPrecio.Items.Count == 0)
            {
                MessageBox.Show("Por favor Cargue primeramente las listas de Precio");
                Close();
            }

            cmbListaPrecio.SelectedValue = _configuracion.ListaPrecioPorDefectoId;

            // ============================================================================= //

            _vendedorSeleccionado = ObtenerVendedorPorDefecto();
            AsignarDatosVendedor(_vendedorSeleccionado);

        }

        private void CargarCuerpo()
        {
            dgvGrilla.DataSource = _factura.Items.ToList();
            FormatearGrilla(dgvGrilla);

            if (_factura.Items.Any())
            {
                var ultimoItem = _factura.Items.Last();

                lblUltimaDescripcion.Text = ultimoItem.Descripcion.ToUpper();
                lblPrecioPorCantidad.Text = $"{ultimoItem.Cantidad} X {ultimoItem.PrecioStr} = {ultimoItem.SubTotalStr}";
            }
            else
            {
                lblUltimaDescripcion.Text = string.Empty;
                lblPrecioPorCantidad.Text = string.Empty;
            }
        }

        private void CargarPie()
        {
            txtSubTotal.Text = _factura.SubTotalStr;
            nudDescuento.Value = _factura.Descuento;
            txtTotal.Text = _factura.TotalStr;
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);
            dgv.Columns["CodigoBarra"].Visible = true;
            dgv.Columns["CodigoBarra"].Width = 100;
            dgv.Columns["CodigoBarra"].HeaderText = "Código";
            dgv.Columns["CodigoBarra"].DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleLeft;

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].HeaderText = "Articulo";
            dgv.Columns["Descripcion"].AutoSizeMode =
            DataGridViewAutoSizeColumnMode.Fill;

            dgv.Columns["Iva"].Visible = true;
            dgv.Columns["Iva"].Width = 100;
            dgv.Columns["Iva"].HeaderText = "Iva";
            dgv.Columns["Iva"].DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleRight;

            dgv.Columns["PrecioStr"].Visible = true;
            dgv.Columns["PrecioStr"].Width = 120;
            dgv.Columns["PrecioStr"].HeaderText = "Precio";
            dgv.Columns["PrecioStr"].DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleRight;

            dgv.Columns["Cantidad"].Visible = true;
            dgv.Columns["Cantidad"].Width = 120;
            dgv.Columns["Cantidad"].HeaderText = "Cantidad";
            dgv.Columns["Cantidad"].DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["SubTotalStr"].Visible = true;
            dgv.Columns["SubTotalStr"].Width = 120;
            dgv.Columns["SubTotalStr"].HeaderText = "Sub-Total";
            dgv.Columns["SubTotalStr"].DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleRight;
        }

        private void AsignarDatosVendedor(EmpleadoDto empleado)
        {
            txtVendedor.Text = empleado.ApyNom;
        }

        private EmpleadoDto ObtenerVendedorPorDefecto()
        {
            return (EmpleadoDto)_empleadoServicio.Obtener(typeof(EmpleadoDto),
            Identidad.EmpleadoId);
        }

        private void CambiarTituloDelPuntoVenta()
        {
            this.Text = $"TPV - {cmbPuestoVenta.Text}";
        }

        private ClienteDto ObtenerClienteConsumidorFinal()
        {

           var clientes = (List<ClienteDto>)_clienteServicio.Obtener(typeof(ClienteDto),
            Aplicacion.Constantes.Cliente.ConsumidorFinal);

            if (clientes.FirstOrDefault() == null)
            {
                MessageBox.Show("El cliente Consumidor Final no Existe");
                Close();
            }
            return clientes.FirstOrDefault();
        }

        private void AsignarDatosCliente(ClienteDto cliente)
        {
            txtDniCliente.Text = cliente.Dni;
            txtCliente.Text = cliente.ApyNom;
            txtDomicilioCliente.Text = cliente.Direccion;
            txtCondicionIvaCliente.Text = cliente.CondicionIva;
            txtTelefonoCliente.Text = cliente.Telefono;
        }

        private bool AsignarArticuloAlternativo(string codigo)
        {
            _articuloConPrecioAlternativo = true;

            var codigoArticulo = codigo.Substring(0, codigo.IndexOf('*'));

            if (!string.IsNullOrEmpty(codigoArticulo))
            {
                _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(codigoArticulo, (long)cmbListaPrecio.SelectedValue, _configuracion.DepositoVentaId);

                if (_articuloSeleccionado != null)
                {
                    var precioAlternativo = codigo.Substring(codigo.IndexOf('*') + 1);

                    if (!string.IsNullOrEmpty(precioAlternativo))
                    {
                        if (decimal.TryParse(precioAlternativo, out decimal _precio))
                        {
                            _articuloSeleccionado.Precio = _precio;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return false;
        }

        private bool AsignarArticuloPorBascula(string codigoBascula)
        {
            decimal _precioBascula = 0;
            decimal _pesoBascula = 0;

            _ingresoPorCodigoBascula = true;

            int.TryParse(codigoBascula.Substring(4, 3), out int codigoArticulo);

            var precioPesoArticulo = codigoBascula.Substring(7, 5);

            if (_configuracion.EsImpresionPorPrecio)
            {
                if (!decimal.TryParse(precioPesoArticulo.Insert(3, ","), NumberStyles.Number,
                    new CultureInfo("es-Ar"), out _precioBascula))
                {
                    return false;
                }
            }
            else
            {
                if (!decimal.TryParse(precioPesoArticulo.Insert(2, ","), NumberStyles.Number,
                    new CultureInfo("es-Ar"), out _pesoBascula))
                {
                    return false;
                }
            }

            _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(codigoArticulo.ToString(), (long)cmbListaPrecio.SelectedValue, _configuracion.DepositoVentaId);

            if (_articuloSeleccionado != null)
            {
                if (_configuracion.EsImpresionPorPrecio)
                {
                    _articuloSeleccionado.Precio = _precioBascula;
                }
                else
                {
                    nudCantidad.Value = _pesoBascula;
                }
            }

            return false;
        }

        private void LimpiarParaNuevoItem()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            txtPrecioUnitario.Clear();
            nudCantidad.Value = 1;
            nudCantidad.Enabled = false;
            _permiteAgregarPorCantidad = false;
            _articuloConPrecioAlternativo = false;
            _ingresoPorCodigoBascula = false;
            _articuloSeleccionado = null;
            txtCodigo.Focus();
        }

       

        private void LimpiarParaNuevoFactura()
        {
            _factura = new FacturaView();
            CargarCabecera();
            CargarCuerpo();
            CargarPie();
            LimpiarParaNuevoItem();
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            _factura.Cliente = _clienteSeleccionado;
            _factura.Vendedor = _vendedorSeleccionado;
            _factura.TipoComprobante = (TipoComprobante)cmbTipoComprobante.SelectedItem;
            _factura.PuntoVentaId = (long)cmbPuestoVenta.SelectedValue;
            _factura.UsuarioId = Identidad.UsuarioId;
         

            if (_configuracion.PuestoCajaSeparado)
            {
                try
                {
                    var nuevoComprobante = new FacturaDto()
                    {
                        EmpleadoId = _factura.Vendedor.Id,
                        ClienteId = _factura.Cliente.Id,
                        TipoComprobante = _factura.TipoComprobante,
                        Descuento = _factura.Descuento,
                        SubTotal = _factura.SubTotal,
                        Fecha = DateTime.Now,
                        Estado = Estado.Pendiente,
                        PuestoTrabajoId = _factura.PuntoVentaId,
                        
                        Iva105 = 0,
                        Iva21 = 0,
                        Total = _factura.Total,
                        UsuarioId = _factura.UsuarioId,
                        VieneVentas = true,
                        Eliminado = false
                    };

                    foreach (var item in _factura.Items)
                    {
                        nuevoComprobante.Items.Add(new DetalleComprobanteDto
                        {
                            Cantidad = item.Cantidad,
                            Iva = item.Iva,
                            Descripcion = item.Descripcion,
                            Precio = item.Precio,
                            ArticuloId = item.ArticuloId,
                            Codigo = item.CodigoBarra,
                            SubTotal = item.SubTotal,
                            Eliminado = false,
                        });
                    }

                    _facturaServicio.Insertar(nuevoComprobante);

                    MessageBox.Show("Los datos se grabaron correctamente");
                    LimpiarParaNuevoFactura();

                }
                catch (DbEntityValidationException ex)
                {
                    var error = ex.EntityValidationErrors.SelectMany(v => v.ValidationErrors)
                        .Aggregate(string.Empty,
                            (current, validationError) =>
                                current +
                                ($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}. {Environment.NewLine}"
                                ));

                
                    throw new Exception($"Ocurrio un error grave al grabar la Factura. Error: {error}");
                }
            }
            else
            {
                var fFormaDePago = new _00044_FormaPago(_factura);
                fFormaDePago.ShowDialog();

                if (fFormaDePago.RealizoVenta)
                {
                    LimpiarParaNuevoFactura();
                    txtCodigo.Focus();
                }
            }
        }

        private void btnPresupuesto_Click(object sender, EventArgs e)
        {
            _factura.Cliente = _clienteSeleccionado;
            _factura.Vendedor = _vendedorSeleccionado;
            _factura.TipoComprobante = (TipoComprobante)cmbTipoComprobante.SelectedItem;
            _factura.PuntoVentaId = (long)cmbPuestoVenta.SelectedValue;
            _factura.UsuarioId = Identidad.UsuarioId;

            try
            {
                var nuevoComprobante = new FacturaDto()
                {

                    EmpleadoId = _factura.Vendedor.Id,
                    ClienteId = _factura.Cliente.Id,
                    TipoComprobante = _factura.TipoComprobante,
                    Descuento = _factura.Descuento,
                    SubTotal = _factura.SubTotal,
                    Fecha = DateTime.Now,
                    Estado = Estado.Presupuesto,
                    PuestoTrabajoId = _factura.PuntoVentaId,

                    Iva105 = 0,
                    Iva21 = 0,
                    Total = _factura.Total,
                    UsuarioId = _factura.UsuarioId,
                    VieneVentas = true,
                    Eliminado = false

                };

                foreach (var item in _factura.Items)
                {
                    nuevoComprobante.Items.Add(new DetalleComprobanteDto
                    {
                        Cantidad = item.Cantidad,
                        Iva = item.Iva,
                        Descripcion = item.Descripcion,
                        Precio = item.Precio,
                        ArticuloId = item.ArticuloId,
                        Codigo = item.CodigoBarra,
                        SubTotal = item.SubTotal,
                        Eliminado = false,
                    });
                }

                _facturaServicio.Insertar(nuevoComprobante);

                MessageBox.Show("Los datos se grabaron correctamente");
                LimpiarParaNuevoFactura();

            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.SelectMany(v => v.ValidationErrors)
                    .Aggregate(string.Empty,
                        (current, validationError) =>
                            current +
                            ($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}. {Environment.NewLine}"
                            ));


                throw new Exception($"Ocurrio un error grave al grabar la Factura. Error: {error}");
            }
        }
    }
}
