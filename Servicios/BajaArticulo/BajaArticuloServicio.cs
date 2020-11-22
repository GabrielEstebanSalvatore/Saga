using Dominio.Entidades;
using Dominio.UnidadDeTrabajo;
using IServicio.BaseDto;
using IServicios.Articulo;
using IServicios.Articulo.DTOs;
using Servicios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Servicios.Articulo
{
    public class BajaArticuloServicio : IBajaArticuloServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        public BajaArticuloServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }
        public void Eliminar(long id)
        {
            _unidadDeTrabajo.BajaArticuloRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {

            using (var tran = new TransactionScope())
            {
                try
                {
                    var dto = (BajarArticuloCrudDto)dtoEntidad;

                    var entidad = new Dominio.Entidades.BajaArticulo
                    {
                        ArticuloId = dto.ArticuloId,
                        MotivoBajaId = dto.MotivoBajaId,
                        Cantidad = dto.Cantidad,
                        Fecha = dto.Fecha,
                        Observacion = dto.Observacion,
                        EstaEliminado = false
                    };

                    _unidadDeTrabajo.BajaArticuloRepositorio.Insertar(entidad);

                    var bajarStock = _unidadDeTrabajo.StockRepositorio.Obtener(x => x.ArticuloId == dto.ArticuloId);

                    if (!bajarStock.Any()) throw new Exception("El artyículo seleccionad no posee stock");

                    foreach (var stock in bajarStock)
                    {
                        var stockNuevo = new Stock
                        {
                            Articulo = stock.Articulo,
                            ArticuloId = stock.ArticuloId,
                            Deposito = stock.Deposito,
                            DepositoId = stock.DepositoId,
                            EstaEliminado = stock.EstaEliminado,
                            Cantidad = stock.Cantidad - dto.Cantidad
                        };

                        _unidadDeTrabajo.StockRepositorio.Insertar(stockNuevo);
                    }

                    _unidadDeTrabajo.Commit();
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    throw new Exception(ex.Message);
                }
            }
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (BajarArticuloCrudDto)dtoEntidad;

            var entidad = _unidadDeTrabajo.BajaArticuloRepositorio.Obtener(dto.Id);

            if(entidad == null) throw new Exception("Ocurrio un Error al Obtener el la baja de stock");

            entidad.ArticuloId = dto.ArticuloId;
            entidad.MotivoBajaId = dto.MotivoBajaId;
            entidad.Cantidad = dto.Cantidad;
            entidad.Fecha = dto.Fecha;
            entidad.Observacion = dto.Observacion;
        
            _unidadDeTrabajo.BajaArticuloRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }
       

        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.BajaArticuloRepositorio.Obtener(id, "stock, Articulo");

            return new BajarArticuloDto
            {
                Id = entidad.Id,
                Eliminado = entidad.EstaEliminado,
                ArticuloId = entidad.ArticuloId,
                MotivoBajaId = entidad.MotivoBajaId,
                Cantidad = entidad.Cantidad,
                Fecha = entidad.Fecha,
                Observacion = entidad.Observacion,
                Articulo = entidad.Articulo.Descripcion,
                MotivoBaja = entidad.MotivoBaja.Descripcion
                
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            var codigo = -1;

            int.TryParse(cadenaBuscar, out codigo);

            Expression<Func<Dominio.Entidades.BajaArticulo, bool>> filtro = x =>
                x.Articulo.Descripcion.Contains(cadenaBuscar)
                || x.Articulo.Codigo == codigo
                || x.Articulo.CodigoBarra == cadenaBuscar
                || x.MotivoBaja.Descripcion.Contains(cadenaBuscar);

            if (!mostrarTodos)
            {
                filtro = filtro.And(x => !x.EstaEliminado);
            }

            return _unidadDeTrabajo.BajaArticuloRepositorio.Obtener(filtro, "Articulo, MotivoBaja")
                .Select(x => new BajarArticuloDto
                {
                    Id = x.Id,
                    Eliminado = x.EstaEliminado,
                    ArticuloId = x.ArticuloId,
                    Articulo = x.Articulo.Descripcion,
                    MotivoBajaId = x.MotivoBajaId,
                    MotivoBaja = x.MotivoBaja.Descripcion,
                    Cantidad = x.Cantidad,
                    Fecha = x.Fecha,
                    Observacion = x.Observacion,

                })
                .OrderBy(x => x.Observacion)
                .ToList();
        }

        public IEnumerable<BajarArticuloDto> ObtenerLookUp(string cadenaBuscar)
        {
            var fechaActual = DateTime.Now;
            
            Expression<Func<Dominio.Entidades.BajaArticulo, bool>> filtro = x =>
              x.Observacion.Contains(cadenaBuscar)
            || x.Articulo.Descripcion.Contains(cadenaBuscar)
            || x.MotivoBaja.Descripcion.Contains(cadenaBuscar);

            return _unidadDeTrabajo.BajaArticuloRepositorio.Obtener(filtro, "Articulo, MotivoBaja")
                .Select(x => new BajarArticuloDto
                {
                    Id = x.Id,
                    Eliminado = x.EstaEliminado,
                    ArticuloId = x.ArticuloId,
                    MotivoBajaId = x.MotivoBajaId,
                    Cantidad = x.Cantidad,
                    Fecha = x.Fecha,
                    Observacion = x.Observacion,
                    Articulo = x.Articulo.Descripcion,
                    MotivoBaja = x.MotivoBaja.Descripcion

                })
                .OrderBy(x => x.Observacion)
                .ToList();
        }
    }
}
