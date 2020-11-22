using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacion.Constantes;
using Dominio.UnidadDeTrabajo;
using IServicio.BaseDto;
using IServicio.Persona.DTOs;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;

namespace Servicios.Comprobante
{
    public class FacturaServicio : ComprobanteServicio, IFacturaServicio
    {
        public FacturaServicio(IUnidadDeTrabajo unidadDeTrabajo)
        : base(unidadDeTrabajo)
        {
        }
        public void Modificar(DtoBase dtoEntidad)
        {

            var dto = (FacturaDto) dtoEntidad;

            var entidad = _unidadDeTrabajo.FacturaRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la Condicion de Iva");

            entidad.Total = dto.Total;
            entidad.Estado = Estado.Pagada;

            _unidadDeTrabajo.FacturaRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();


        }

   
        public IEnumerable<ComprobantePendienteDto> ObtenerPendientesPago()
        {
            return _unidadDeTrabajo.FacturaRepositorio.Obtener(x => !x.EstaEliminado
                && x.Estado == Estado.Pendiente, "Cliente, DetalleComprobantes")
                .Select(x => new ComprobantePendienteDto
                {
                    Id = x.Id,
                    Cliente=new ClienteDto 
                    { 
                        Id = x.Cliente.Id,
                        Eliminado = x.Cliente.EstaEliminado,
                        Dni = x.Cliente.Dni,
                        Nombre = x.Cliente.Nombre,
                        Apellido = x.Cliente.Apellido,
                        Telefono = x.Cliente.Telefono,
                        Direccion = x.Cliente.Direccion,
                        ActivarCtaCte = x.Cliente.ActivarCtaCte,
                        TieneLimiteCompra = x.Cliente.TieneLimiteCompra,
                        MontoMaximoCtaCte = x.Cliente.MontoMaximoCtaCte
                    } ,
                    ClienteApyNom = $"{x.Cliente.Apellido} {x.Cliente.Nombre}",
                    Fecha = x.Fecha,
                    Direccion = x.Cliente.Direccion,
                    Dni = x.Cliente.Dni,
                    Telefono = x.Cliente.Telefono,
                    MontoPagar = x.Total,
                    Numero = x.Numero,
                    TipoComprobante = x.TipoComprobante,
                    Eliminado = x.EstaEliminado,
                    Items = x.DetalleComprobantes.Select(d => new DetallePendienteDto
                    {
                        Id = d.Id,
                        Descripcion=d.Descripcion,
                        Cantidad = d.Cantidad,
                        Precio = d.Precio,
                        SubTotal = d.SubTotal,
                        Eliminado = d.EstaEliminado,

                        
                    }).ToList()
                })
                .OrderByDescending(x=>x.Fecha)
                .ToList();
        }

        public IEnumerable<ComprobantePendienteDto> ObtenerPresupuestos()
        {
            return _unidadDeTrabajo.FacturaRepositorio.Obtener(x => !x.EstaEliminado
                && x.Estado == Estado.Presupuesto, "Cliente, DetalleComprobantes")
                .Select(x => new ComprobantePendienteDto
                {
                    Id = x.Id,
                    Cliente = new ClienteDto
                    {
                        Id = x.Cliente.Id,
                        Eliminado = x.Cliente.EstaEliminado,
                        Dni = x.Cliente.Dni,
                        Nombre = x.Cliente.Nombre,
                        Apellido = x.Cliente.Apellido,
                        Telefono = x.Cliente.Telefono,
                        Direccion = x.Cliente.Direccion,
                        ActivarCtaCte = x.Cliente.ActivarCtaCte,
                        TieneLimiteCompra = x.Cliente.TieneLimiteCompra,
                        MontoMaximoCtaCte = x.Cliente.MontoMaximoCtaCte
                    },
                    ClienteApyNom = $"{x.Cliente.Apellido} {x.Cliente.Nombre}",
                    Fecha = x.Fecha,
                    Direccion = x.Cliente.Direccion,
                    Dni = x.Cliente.Dni,
                    Telefono = x.Cliente.Telefono,
                    MontoPagar = x.Total,
                    Numero = x.Numero,
                    TipoComprobante = x.TipoComprobante,
                    Eliminado = x.EstaEliminado,
                    Items = x.DetalleComprobantes.Select(d => new DetallePendienteDto
                    {
                        Id = d.Id,
                        Descripcion = d.Descripcion,
                        Cantidad = d.Cantidad,
                        Precio = d.Precio,
                        SubTotal = d.SubTotal,
                        Eliminado = d.EstaEliminado,


                    }).ToList()
                })
                .OrderByDescending(x => x.Fecha)
                .ToList();
        }
    }
}
