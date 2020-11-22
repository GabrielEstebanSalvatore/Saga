using Aplicacion.Constantes;
using Dominio.Entidades;
using IServicio.Configuracion;
using IServicios.Comprobante.DTOs;
using IServicios.Contador;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;

namespace Servicios.Comprobante
{
    class CuentaCorriente : Comprobante
    {
        private readonly IContadorServicio _contadorServicio;
        private readonly IConfiguracionServicio _configuracionServicio;

        public CuentaCorriente()
        : base()
        {
            _contadorServicio = ObjectFactory.GetInstance<IContadorServicio>();
            _configuracionServicio = ObjectFactory.GetInstance<IConfiguracionServicio>();
        }

        public override long Insertar(ComprobanteDto comprobante)
        {
      
            using (var tran = new TransactionScope())
            {
                try
                {
                    int numeroComprobante = 0;

                    var cajaActual = _unidadDeTrabajo.CajaRepositorio.Obtener
                    (x => x.UsuarioAperturaId == comprobante.UsuarioId
                    && x.UsuarioCierreId == null).FirstOrDefault();

                    if (cajaActual == null)
                        throw new Exception("Ocurrio un error al obtener la Caja");
                    
                    var ctaCteCompDto = (CtaCteComprobanteDto)comprobante;

                    CuentaCorrienteCliente _CtaCteNueva = new CuentaCorrienteCliente();

                    numeroComprobante = _contadorServicio.ObtenerSiguienteNumeroComprobante(comprobante.TipoComprobante);

                    _CtaCteNueva = new CuentaCorrienteCliente
                        {
                            ClienteId = ctaCteCompDto.ClienteId,
                            Descuento = ctaCteCompDto.Descuento,
                            EmpleadoId = ctaCteCompDto.EmpleadoId,
                            Fecha = ctaCteCompDto.Fecha,
                            Iva105 = ctaCteCompDto.Iva105,
                            Iva21 = ctaCteCompDto.Iva21,
                            Numero = numeroComprobante,
                            SubTotal = ctaCteCompDto.SubTotal,
                            Total = ctaCteCompDto.Total,
                            TipoComprobante = ctaCteCompDto.TipoComprobante,
                            UsuarioId = ctaCteCompDto.UsuarioId,
                            DetalleComprobantes = new List<DetalleComprobante>(),
                            Movimientos = new List<Movimiento>(),
                            FormaPagos = new List<FormaPago>(),
                            EstaEliminado = false
                        };



                    _CtaCteNueva.Movimientos.Add(new Movimiento
                        {
                            ComprobanteId = _CtaCteNueva.Id,
                            CajaId = cajaActual.Id,
                            Fecha = ctaCteCompDto.Fecha,
                            Monto = ctaCteCompDto.Total,
                            UsuarioId = ctaCteCompDto.UsuarioId,
                            TipoMovimiento = TipoMovimiento.Ingreso,
                            Descripcion = $"Fa- { ctaCteCompDto.TipoComprobante.ToString() }-Nro { numeroComprobante }",
                            EstaEliminado = false
                        });

                    foreach (var fp in ctaCteCompDto.FormasDePagos)
                    {

                        var fpCtaCTe = (FormaPagoCtaCteDto)fp;
                        _CtaCteNueva.FormaPagos.Add(new FormaPagoCtaCte
                        {
                            Monto = fpCtaCTe.Monto,
                            TipoPago = TipoPago.CtaCte,
                            ClienteId = fpCtaCTe.ClienteId,
                            EstaEliminado = false
                        });

                        _CtaCteNueva.Movimientos.Add(new MovimientoCuentaCorriente
                        {
                            ComprobanteId = _CtaCteNueva.Id,
                            CajaId = cajaActual.Id,
                            Fecha = ctaCteCompDto.Fecha,
                            Monto = fpCtaCTe.Monto,
                            UsuarioId = ctaCteCompDto.UsuarioId,
                            TipoMovimiento =
                                        TipoMovimiento.Egreso,
                            Descripcion = $"Fa- { ctaCteCompDto.TipoComprobante.ToString() }-Nro { numeroComprobante }",
                            ClienteId = fpCtaCTe.ClienteId,
                            EstaEliminado = false
                        });
                        cajaActual.TotalEntradaCtaCte +=
                        fpCtaCTe.Monto;
                        cajaActual.DetalleCajas.Add(new DetalleCaja
                        {
                            Monto = fpCtaCTe.Monto,
                            TipoPago = TipoPago.CtaCte
                        });


                        _unidadDeTrabajo.CajaRepositorio.Modificar(cajaActual);
                    }

                    _unidadDeTrabajo.FacturaRepositorio.Modificar(_CtaCteNueva);
          
                    _unidadDeTrabajo.Commit();
                    tran.Complete();
                    return 0;
                }
                catch (DbEntityValidationException ex)
                {
                    var error = ex.EntityValidationErrors.SelectMany(v => v.ValidationErrors)
                        .Aggregate(string.Empty,
                            (current, validationError) =>
                                current +
                                ($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}. {Environment.NewLine}"
                                ));

                    tran.Dispose();
                    throw new Exception($"Ocurrio un error grave al grabar la Factura. Error: {error}");
                }

            }
        }
    }
    }

