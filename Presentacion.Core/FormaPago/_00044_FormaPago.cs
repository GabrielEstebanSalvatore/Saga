using Aplicacion.Constantes;
using IServicio.Persona.DTOs;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;
using IServicios.CuentaCorriente;
using Presentacion.Core.Cliente;
using Presentacion.Core.Comprobantes.Clases;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Drawing;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace Presentacion.Core.FormaPago
{
    public partial class _00044_FormaPago : FormBase
    {
        private FacturaView _factura;
        private ComprobantePendienteDto _facturaPendiente;
        private bool _vieneDeVentas;

        private readonly IBancoServicio _bancoServicio;
        private readonly ITarjetaServicio _tarjetaServicio;
        private readonly IFacturaServicio _facturaServicio;
        private readonly ICuentaCorrienteServicio _cuentaCorrienteServicio;
        public bool RealizoVenta { get; set; } = false;

        public _00044_FormaPago() 
        {
            InitializeComponent();
            _bancoServicio = ObjectFactory.GetInstance<IBancoServicio>();
            _tarjetaServicio = ObjectFactory.GetInstance<ITarjetaServicio>();
            _facturaServicio = ObjectFactory.GetInstance<IFacturaServicio>();
            _cuentaCorrienteServicio = ObjectFactory.GetInstance<ICuentaCorrienteServicio>();
            PoblarComboBox(cmbBanco, _bancoServicio.Obtener(string.Empty, false),
            "Descripcion", "Id");
           PoblarComboBox(cmbTarjeta, _tarjetaServicio.Obtener(string.Empty,
           false), "Descripcion", "Id");
        }
        public _00044_FormaPago(FacturaView factura) 
            : this()// Ventas
        {
           
            _factura = factura;
            _vieneDeVentas = true;

            CargarDatos(_factura);
        }
        public _00044_FormaPago(ComprobantePendienteDto factura)
            :this()
        {
            _facturaPendiente = factura;
            _vieneDeVentas = false;
            CargarDatos(factura);
        }

        private void CargarDatos(ComprobantePendienteDto factura)
        {

            txtTotalAbonar.Text = factura.MontoPagarStr;
            nudMontoEfectivo.Value = factura.MontoPagar;
            nudMontoCheque.Value = 0;
            txtNumeroCheque.Clear();
            dtpFechaVencimientoCheque.Value = DateTime.Now;
            nudMontoCtaCte.Value = 0;
            txtApellido.Text = factura.Cliente.ApyNom;
            txtDni.Text = factura.Cliente.Dni;
            txtTelefono.Text = factura.Cliente.Telefono;
            txtDireccion.Text = factura.Cliente.Direccion;

            txtMontoAdeudado.Text = factura.Cliente.Dni != Aplicacion.Constantes.Cliente.ConsumidorFinal
                ? _cuentaCorrienteServicio.ObtenerDeudaCliente(factura.Cliente.Id).ToString("C")
                : 0.ToString("C");

            nudMontoTarjeta.Value = 0;
            txtNumeroTarjeta.Clear();
            txtCuponPago.Clear();
            nudCantidadCuotas.Value = 1;
        }

        private void CargarDatos(FacturaView factura)
        {
            txtTotalAbonar.Text = factura.TotalStr;
            nudMontoEfectivo.Value = factura.Total;
            nudMontoCheque.Value = 0;
            txtNumeroCheque.Clear();
            dtpFechaVencimientoCheque.Value = DateTime.Now;
            nudMontoCtaCte.Value = 0;
            txtApellido.Text = factura.Cliente.ApyNom;
            txtDni.Text = factura.Cliente.Dni;
            txtTelefono.Text = factura.Cliente.Telefono;
            txtDireccion.Text = factura.Cliente.Direccion;

            txtMontoAdeudado.Text = factura.Cliente.Dni != Aplicacion.Constantes.Cliente.ConsumidorFinal 
                ? _cuentaCorrienteServicio.ObtenerDeudaCliente(factura.Cliente.Id).ToString("C")
                :  0.ToString("C");
            

            nudMontoTarjeta.Value = 0;
            txtNumeroTarjeta.Clear();
            txtCuponPago.Clear();
            nudCantidadCuotas.Value = 1;
        }

        private void nudMontoEfectivo_ValueChanged(object sender, EventArgs e)
        {
            nudTotalEfectivo.Value = nudMontoEfectivo.Value; 
        }

        private void nudMontoTarjeta_ValueChanged(object sender, EventArgs e)
        {
            nudTotalTarjeta.Value = nudMontoTarjeta.Value;
        }

        private void nudMontoCheque_ValueChanged(object sender, EventArgs e)
        {
            nudTotalCheque.Value = nudMontoCheque.Value;
        }

        private void nudMontoCtaCte_ValueChanged(object sender, EventArgs e)
        {
            nudTotalCtaCte.Value = nudMontoCtaCte.Value;
        }

        private void nudTotalEfectivo_ValueChanged(object sender, EventArgs e)
        {
            nudTotal.Value = nudTotalCheque.Value
                            + nudTotalEfectivo.Value
                            + nudTotalCtaCte.Value
                            + nudTotalTarjeta.Value;
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            var fLookUpCliente = ObjectFactory.GetInstance<ClienteLookUp>();
            fLookUpCliente.ShowDialog();

            if (fLookUpCliente.EntidadSeleccionada != null)
            {
                var _cliente = (ClienteDto)fLookUpCliente.EntidadSeleccionada;

                if (_cliente.ActivarCtaCte)
                {
                    txtApellido.Text = _cliente.ApyNom;
                    txtDni.Text = _cliente.Dni;
                    txtTelefono.Text = _cliente.Telefono;
                    txtDireccion.Text = _cliente.Direccion;

                    txtMontoAdeudado.Text = _cliente.Dni != Aplicacion.Constantes.Cliente.ConsumidorFinal
                     ? _cuentaCorrienteServicio.ObtenerDeudaCliente(_cliente.Id).ToString("C")
                     : 0.ToString("C");
                }
                else
                {
                    MessageBox.Show($"El cliente {_cliente.ApyNom} no tiene Activa la Cuenta Corriente", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void nudPagaCon_ValueChanged(object sender, EventArgs e)
        {
            //TODO
            //nudTotalEfectivo.Value = nudPagaCon.Value;
            //nudMontoEfectivo.Value = nudPagaCon.Value;
            CalcularVuelto();
        }

        private void CalcularVuelto()
        {
            nudVuelto.Value = nudTotalEfectivo.Value - nudPagaCon.Value >= 0
                ? nudPagaCon.Value - nudTotalEfectivo.Value
                : (nudTotalEfectivo.Value - nudPagaCon.Value) * -1;
            nudVuelto.BackColor = nudTotalEfectivo.Value - nudPagaCon.Value >= 0
            ? Color.Red
            : Color.Green;
            nudVuelto.ForeColor = Color.White;
        }

        private void nudTotal_ValueChanged(object sender, EventArgs e)
        {
            if (nudPagaCon.Value > 0)
            {
                CalcularVuelto();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            RealizoVenta = false;
            Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (nudVuelto.Value > 0)
                {
                    if (MessageBox.Show("El total que esta por abonar es superior al monto a pagar.Desea continuar ? ",
                    "Atención", MessageBoxButtons.OKCancel,MessageBoxIcon.Question)
                    == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else if (nudVuelto.Value < 0)
                {
                    MessageBox.Show("El total que esta por abonar es inferior al monto a pagar",
                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var _facturaNueva = new FacturaDto();
                _facturaNueva.VieneVentas = _vieneDeVentas;

                if (_vieneDeVentas)
                {
                    
                    _facturaNueva.EmpleadoId = _factura.Vendedor.Id;
                    _facturaNueva.ClienteId = _factura.Cliente.Id;
                    _facturaNueva.TipoComprobante = _factura.TipoComprobante;
                    _facturaNueva.Descuento = _factura.Descuento;
                    _facturaNueva.SubTotal = _factura.SubTotal;
                    _facturaNueva.Total = _factura.Total;
                    _facturaNueva.Estado = Estado.Pagada;
                    _facturaNueva.PuestoTrabajoId = _factura.PuntoVentaId;
                    _facturaNueva.Fecha = DateTime.Now;
                    _facturaNueva.Iva105 = 0;
                    _facturaNueva.Iva21 = 0;
                    _facturaNueva.UsuarioId = _factura.UsuarioId;
                    foreach (var item in _factura.Items)
                    {
                        _facturaNueva.Items.Add(new DetalleComprobanteDto
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
                }
                else
                {
                    _facturaNueva.Id = _facturaPendiente.Id;
                    _facturaNueva.Estado = Estado.Pendiente;
                    _facturaNueva.VieneVentas = false;
                    _facturaNueva.TipoComprobante = _facturaPendiente.TipoComprobante;
                    _facturaNueva.UsuarioId = Identidad.UsuarioId;
                    _facturaNueva.Fecha = DateTime.Now;
                    _facturaNueva.Total = _facturaPendiente.MontoPagar;
                }
                // Formas de Pago
                if (nudTotalEfectivo.Value > 0)
                {
                    _facturaNueva.FormasDePagos.Add(new FormaPagoDto
                    {
                        Monto = nudTotalEfectivo.Value,
                        TipoPago = TipoPago.Efectivo,
                        Eliminado = false
                    });
                }
                if (nudTotalTarjeta.Value > 0)
                {
                    _facturaNueva.FormasDePagos.Add(new FormaPagoTarjetaDto
                    {
                        TipoPago = TipoPago.Tarjeta,
                        CantidadCuotas = (int)nudCantidadCuotas.Value,
                        CuponPago = txtCuponPago.Text,
                        Monto = nudTotalTarjeta.Value,
                        NumeroTarjeta = txtNumeroTarjeta.Text,
                        TarjetaId = (long)cmbTarjeta.SelectedValue,
                        Eliminado = false
                    });
                }
                if (nudTotalCheque.Value > 0)
                {
                    _facturaNueva.FormasDePagos.Add(new FormaPagoChequeDto
                    {
                        BancoId = (long)cmbBanco.SelectedValue,
                        ClienteId = _factura.Cliente.Id,
                        FechaVencimiento = dtpFechaVencimientoCheque.Value,
                        Monto = nudTotalCheque.Value,
                        Numero = txtNumeroCheque.Text,
                        TipoPago = TipoPago.Cheque,
                        Eliminado = false
                    });
                }
                if (nudTotalCtaCte.Value > 0)
                {
                    var deuda = _cuentaCorrienteServicio.ObtenerDeudaCliente(_factura.Cliente.Id);

                    if (_vieneDeVentas)
                    {
                      
                        if (_factura.Cliente.ActivarCtaCte)
                        {
                            if (_factura.Cliente.TieneLimiteCompra &&
                            _factura.Cliente.MontoMaximoCtaCte < deuda + nudTotalCtaCte.Value)
                            {
                                var menssajeCtaCte = $"El cliente {_factura.Cliente.ApyNom} esta por arriba del limite Permitido."
                                + Environment.NewLine
                                + $" El limite es { _factura.Cliente.MontoMaximoCtaCte.ToString("C")}";
                                MessageBox.Show(menssajeCtaCte, "Atención",
                                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            _facturaNueva.FormasDePagos.Add(new FormaPagoCtaCteDto
                            {
                                TipoPago = TipoPago.CtaCte,
                                ClienteId = _factura.Cliente.Id,
                                Monto = nudTotalCtaCte.Value,
                                Eliminado = false,
                            });
                        }
                    }
                    else // Si viene de Pendiente
                    {
                
                        if (_facturaPendiente.Cliente.ActivarCtaCte)
                        {
                            if (_facturaPendiente.Cliente.TieneLimiteCompra &&
                            _facturaPendiente.Cliente.MontoMaximoCtaCte < deuda + nudTotalCtaCte.Value)
                            {
                                var menssajeCtaCte =
                                   $"El cliente {_facturaPendiente.Cliente.ApyNom} esta por arriba del limite Permitido."
                                + Environment.NewLine
                                + $" El limite es { _facturaPendiente.Cliente.MontoMaximoCtaCte.ToString("C")}";
                                MessageBox.Show(menssajeCtaCte, "Atención",
                                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            _facturaNueva.FormasDePagos.Add(new FormaPagoCtaCteDto
                            {
                                TipoPago = TipoPago.CtaCte,
                                ClienteId = _facturaPendiente.Cliente.Id,
                                Monto = nudTotalCtaCte.Value,
                                Eliminado = false,
                            });
                        }

                        
                    }//TODO PRE PARCIAL
                }
                if (!_vieneDeVentas)
                {
                    _facturaServicio.Modificar(_facturaNueva);
                    MessageBox.Show("Los datos se grabaron correctamente");
                    RealizoVenta = true;
                    Close();
                }
                else
                {
                    _facturaServicio.Insertar(_facturaNueva);
                    MessageBox.Show("Los datos se grabaron correctamente");
                    RealizoVenta = true;
                    Close();

                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                RealizoVenta = false;
            }
            
        }
    }
}
