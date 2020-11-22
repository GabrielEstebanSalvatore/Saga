using Aplicacion.Constantes;
using Dominio.Entidades;
using Dominio.UnidadDeTrabajo;
using IServicios.Contador;
using IServicios.CuentaCorriente;
using IServicios.CuentaCorriente.DTOs;
using Servicios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Servicios.CuentaCorriente
{
    public class CuentaCorrienteServicio : ICuentaCorrienteServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private readonly IContadorServicio _contadorServicio;

        public CuentaCorrienteServicio(IUnidadDeTrabajo unidadDeTrabajo,
            IContadorServicio contadorServicio)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _contadorServicio = contadorServicio;
        }

        public IEnumerable<CuentaCorrienteDto> Obtener(long clienteId,DateTime fechaDesde, DateTime fechaHasta, bool soloDeuda)
        {
            var _fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
            var _fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);

            Expression<Func<MovimientoCuentaCorriente, bool>> filtro = x => x.ClienteId == clienteId;


            if (soloDeuda)
            {

                filtro = filtro.And(x => x.TipoMovimiento == TipoMovimiento.Egreso);
            }

            return _unidadDeTrabajo.CuentaCorrienteRepositorio.Obtener(filtro)
                .Select(x => new CuentaCorrienteDto
                {
                    Fecha = x.Fecha,
                    Descripcion = x.Descripcion,
                    Monto = (x.Monto * (int) x.TipoMovimiento)

                }).OrderBy(x => x.Fecha)
                .ToList();
                
        }

        public decimal ObtenerDeudaCliente(long clienteId)
        {
            var movimientos = _unidadDeTrabajo.CuentaCorrienteRepositorio.Obtener(x => !x.EstaEliminado && x.ClienteId == clienteId);

            return movimientos.Sum(x => x.Monto * (int)x.TipoMovimiento);
        }

        public void Pagar(long cajaId,long usuarioId, long clienteId, decimal monto, string clienteApyNom)
        {
            try
            {

                var fechaActual = DateTime.Now;
                var nroComprobante = _contadorServicio.ObtenerSiguienteNumeroComprobante(TipoComprobante.CuentaCorriente);

                var movimiento = new MovimientoCuentaCorriente
                {
                    CajaId = cajaId,
                    ClienteId = clienteId,
                    Fecha= fechaActual,
                    Descripcion = $"Pago Cta Cte del cliente {clienteApyNom} - Nro:{nroComprobante} - Monto: {monto.ToString("C")}",
                    TipoMovimiento = TipoMovimiento.Ingreso,
                    UsuarioId = usuarioId,
                    Monto = monto,
                    EstaEliminado = false,
                    cuentaCorrienteClientes = new List<CuentaCorrienteCliente>()
                };

      

                var nuevoComprobante = new CuentaCorrienteCliente
                {
                    ClienteId = clienteId,
                    Descuento = 0,
                    SubTotal = monto,
                    Total = monto,
                    Fecha= fechaActual,
                    Iva105 = 0,
                    Iva21 = 0,
                    Numero = nroComprobante,
                    EstaEliminado = false,
                    UsuarioId = usuarioId,
          
                };

                movimiento.cuentaCorrienteClientes.Add(nuevoComprobante);

                _unidadDeTrabajo.CuentaCorrienteRepositorio.Insertar(movimiento);
                _unidadDeTrabajo.Commit();
            }
            catch (Exception e)
            {

                throw new Exception("Ocurrió un error al pagar la Cuenta Corriente");
            }
        }
    }
}
