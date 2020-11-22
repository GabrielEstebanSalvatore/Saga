using Dominio.UnidadDeTrabajo;
using IServicio.Persona;
using IServicio.Persona.DTOs;
using Servicios.Base;
using System;
using System.Collections.Generic;

namespace Servicios.Persona
{
    public class PersonaServicio : IPersonaServicio
    {
        protected readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private Dictionary<Type, string> _diccionario;

        public PersonaServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _diccionario = new Dictionary<Type, string>();

            InicializadorDiccionario();
        }

        private void InicializadorDiccionario()
        {
            _diccionario.Add(typeof(EmpleadoDto), "Servicios.Persona.Empleado");
            _diccionario.Add(typeof(ClienteDto), "Servicios.Persona.Cliente");
        }

        public void AgregarOpcionDiccionario(Type type, string value)
        {
            _diccionario.Add(type, value);
        }

        public long Insertar(PersonaDto entidad)
        {
            var persona = GenericInstance<Persona>.InstanciarEntidad(entidad, _diccionario);

            return persona.Insertar(entidad);
        }

        public void Eliminar(Type tipoEntidad, long id)
        {
            var persona = GenericInstance< Persona >.InstanciarEntidad(tipoEntidad, _diccionario);

            persona.Eliminar(id);
        }

        public IEnumerable<PersonaDto> Obtener(Type tipo, string cadenaBuscar, bool mostrarTodos = true)
        {
            var persona = GenericInstance<Persona>.InstanciarEntidad(tipo, _diccionario);

            return persona.Obtener(cadenaBuscar, mostrarTodos);
        }

        public PersonaDto Obtener(Type tipo, long id)
        {
            var persona = GenericInstance<Persona>.InstanciarEntidad(tipo, _diccionario);

            return persona.Obtener(id);
        }

        public void Modificar(PersonaDto entidad)
        {
            var persona = GenericInstance< Persona >.InstanciarEntidad(entidad, _diccionario);

            persona.Modificar(entidad);
        }

        // ====================================================================== //
        // =================         Metodos Privados           ================= //
        // ====================================================================== //

        /*private Persona InstanciarEntidad(string tipoEntidad)
        {
            var tipoObjeto = Type.GetType(tipoEntidad);

            if (tipoObjeto == null) return null;

            var entidad = Activator.CreateInstance(tipoObjeto) as Persona;

            return entidad;
        }

        private Persona InstanciaPersona(PersonaDto entidad)
        {
            if (!_diccionario.TryGetValue(entidad.GetType(), out var tipoEntidad))
                throw new Exception($"No hay {entidad.GetType()} para Instanciar.");

            var persona = InstanciarEntidad(tipoEntidad);

            if (persona == null) throw new Exception($"Ocurrió un error al Instanciar {entidad.GetType()}");

            return persona;
        }

        private Persona InstanciarPersona(Type tipo)
        {
            if (!_diccionario.TryGetValue(tipo, out var tipoEntidad))
                throw new Exception($"No hay {tipoEntidad} para Instanciar.");

            var persona = InstanciarEntidad(tipoEntidad);

            if (persona == null) throw new Exception($"Ocurrió un error al Instanciar {tipo}");

            return persona;
        }*/
    }
}
