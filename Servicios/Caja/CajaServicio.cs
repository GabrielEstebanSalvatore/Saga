using Aplicacion.Constantes;
using Dominio.UnidadDeTrabajo;
using IServicios.Caja;
using IServicios.Caja.DTOs;
using IServicios.Comprobante.DTOs;
using Servicios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Caja
{
    public class CajaServicio : ICajaServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public CajaServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Abrir(long usuarioId, decimal monto, DateTime fecha)
        {
            var nuevaCaja = new Dominio.Entidades.Caja
            {
                UsuarioAperturaId = usuarioId,
                FechaApertura = fecha,
                MontoInicial = monto,
                //----------------------------------//
                UsuarioCierreId = (long?)null,
                FechaCierre = (DateTime?)null,
                MontoCierre = (decimal?)null,
                //----------------------------------//
                TotalEntradaCheque = 0m,
                TotalEntradaCtaCte = 0m,
                TotalEntradaTarjeta = 0m,
                TotalEntradaEfectivo = 0m,
                TotalSalidaCheque = 0m,
                TotalSalidaCtaCte = 0m,
                TotalSalidaTarjeta = 0m,
                TotalSalidaEfectivo = 0m,
                //----------------------------------//
                EstaEliminado = false
            };
            _unidadDeTrabajo.CajaRepositorio.Insertar(nuevaCaja);
            _unidadDeTrabajo.Commit();
        }


        public void CerrarCaja(CajaDto dto)
        {
            var entidad = _unidadDeTrabajo.CajaRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la ListaPrecio");


            entidad.UsuarioAperturaId = dto.UsuarioAperturaId;
            entidad.FechaApertura = dto.FechaApertura;
            entidad.MontoInicial = dto.MontoApertura;
            //----------------------------------//
            entidad.UsuarioCierreId = Identidad.UsuarioId;
            entidad.FechaCierre = (DateTime?)DateTime.Now;
            entidad.MontoCierre = (decimal?)dto.MontoCierre;
            //----------------------------------//

            entidad.TotalEntradaCheque = dto.TotalEntradaCheque;
            entidad.TotalEntradaCtaCte = dto.TotalEntradaCtaCte;
            entidad.TotalEntradaTarjeta = dto.TotalEntradaTarjeta;
            entidad.TotalEntradaEfectivo = dto.TotalEntradaEfectivo;
            entidad.TotalSalidaCheque = dto.TotalSalidaCheque;
            entidad.TotalSalidaCtaCte = dto.TotalSalidaCtaCte;
            entidad.TotalSalidaTarjeta = dto.TotalSalidaTarjeta;
            entidad.TotalSalidaEfectivo = dto.TotalSalidaEfectivo;
            //----------------------------------//
            entidad.EstaEliminado = false;
      
            _unidadDeTrabajo.CajaRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public IEnumerable<CajaDto> Obtener(string cadenaBuscar, bool filtroPorFecha, DateTime fechaDesde, DateTime fechaHasta)
        {
            Expression<Func<Dominio.Entidades.Caja, bool>> filtro = x =>
                         !x.EstaEliminado && x.UsuarioApertura.Nombre.Contains
                         (cadenaBuscar);
            var _fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month,fechaDesde.Day, 0, 0, 0);
            var _fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month,fechaHasta.Day, 23, 59, 59);

            if (filtroPorFecha)
            {
                filtro = filtro.And(x => x.FechaApertura >= _fechaDesde &&
                x.FechaApertura <= _fechaHasta);
            }

            return _unidadDeTrabajo.CajaRepositorio.Obtener(filtro,
                "UsuarioApertura, UsuarioCierre")
                .Select(x => new CajaDto
                {
                    Id = x.Id,
                    // ----------------------------------------//
                    UsuarioAperturaId = x.UsuarioAperturaId,
                    UsuarioApertura = x.UsuarioApertura.Nombre,
                    FechaApertura = x.FechaApertura,
                    MontoApertura = x.MontoInicial,
                    // ----------------------------------------//
                    UsuarioCierreId = x.UsuarioCierreId,
                    UsuarioCierre = x.UsuarioCierreId.HasValue ?
                x.UsuarioCierre.Nombre : "----",
                    FechaCierre = x.FechaCierre,
                    MontoCierre = x.MontoCierre,
                    // ----------------------------------------//
                    TotalEntradaCheque = x.TotalEntradaCheque,
                    TotalEntradaCtaCte = x.TotalEntradaCtaCte,
                    TotalEntradaEfectivo = x.TotalEntradaEfectivo,
                    TotalEntradaTarjeta = x.TotalEntradaTarjeta,
                    TotalSalidaCheque = x.TotalSalidaCheque,
                    TotalSalidaCtaCte = x.TotalSalidaCtaCte,
                    TotalSalidaEfectivo = x.TotalSalidaEfectivo,
                    TotalSalidaTarjeta = x.TotalSalidaTarjeta,
                    // ----------------------------------------//
                    Eliminado = x.EstaEliminado
                }).ToList();

        }

        public CajaDto Obtener(long cajaId)
        {

            return _unidadDeTrabajo.CajaRepositorio.Obtener(x => x.Id == cajaId, "UsuarioApertura, UsuarioCierre, DetalleCajas, Movimientos, Movimientos.Comprobante, Movimientos.Comprobante.Empleado")
                .Select(x => new CajaDto
                {
                    Id = x.Id,
                    // ----------------------------------------//
                    UsuarioAperturaId = x.UsuarioAperturaId,
                    UsuarioApertura = x.UsuarioApertura.Nombre,
                    FechaApertura = x.FechaApertura,
                    MontoApertura = x.MontoInicial,
                    // ----------------------------------------//
                    UsuarioCierreId = x.UsuarioCierreId,
                    UsuarioCierre = x.UsuarioCierreId.HasValue ?
                    x.UsuarioCierre.Nombre : "----",
                    FechaCierre = x.FechaCierre,
                    MontoCierre = x.MontoCierre,
                    // ----------------------------------------//
                    TotalEntradaCheque = x.TotalEntradaCheque,
                    TotalEntradaCtaCte = x.TotalEntradaCtaCte,
                    TotalEntradaEfectivo = x.TotalEntradaEfectivo,
                    TotalEntradaTarjeta = x.TotalEntradaTarjeta,
                    TotalSalidaCheque = x.TotalSalidaCheque,
                    TotalSalidaCtaCte = x.TotalSalidaCtaCte,
                    TotalSalidaEfectivo = x.TotalSalidaEfectivo,
                    TotalSalidaTarjeta = x.TotalSalidaTarjeta,
                    // ----------------------------------------//
                    Eliminado = x.EstaEliminado,
                    Detalles = x.DetalleCajas.Select(d => new CajaDetalleDto
                    {
                        Monto = d.Monto,
                        Eliminado = d.EstaEliminado,
                        TipoPago = d.TipoPago
                    }).ToList(),
                    Comprobantes = x.Movimientos.Select(c => new ComprobanteCajaDto
                    {
                        Fecha = c.Comprobante.Fecha,
                        Numero = c.Comprobante.Numero,
                        Total = c.Comprobante.Total,
                        Vendedor = $"{c.Comprobante.Empleado.Apellido} {c.Comprobante.Empleado.Nombre}",
                        Eliminado = c.Comprobante.EstaEliminado


                    }).ToList(),
                }).FirstOrDefault();

        }



        public decimal ObtenerMontoCajaAnterior(long usuarioId)
        {
            var cajasUsuario = _unidadDeTrabajo.CajaRepositorio
                                .Obtener(x => x.UsuarioAperturaId == usuarioId &&
                                x.UsuarioCierre != null);
            var ultimaCaja = cajasUsuario.Where(x => x.FechaApertura ==
            cajasUsuario.Max(f => f.FechaApertura))
            .LastOrDefault();
            return ultimaCaja == null ? 0m : ultimaCaja.MontoCierre.Value;
        }


        public bool VerificarSiExisteCajaAbierta(long usuarioId)
        {
            return _unidadDeTrabajo.CajaRepositorio.Obtener(x => x.UsuarioAperturaId == usuarioId
            && x.UsuarioCierreId == null).Any();
        }




    }
}
