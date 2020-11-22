using IServicio.Departamento;
using IServicio.Localidad;
using IServicio.Persona;
using IServicio.Persona.DTOs;
using IServicio.Provincia;
using Presentacion.Core.CondicionIva;
using Presentacion.Core.Departamento;
using Presentacion.Core.Localidad;
using Presentacion.Core.Provincia;
using PresentacionBase.Formularios;
using StructureMap;
using System.Windows.Forms;
using static Aplicacion.Constantes.ValidacionDatosEntrada;

namespace Presentacion.Core.Cliente
{
    public partial class _00010_Abm_Cliente : FormAbm
    {
        private readonly IClienteServicio _clienteServicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly ICondicionIvaServicio _condicionIvaServicio;

        public _00010_Abm_Cliente(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();
            _clienteServicio = ObjectFactory.GetInstance<IClienteServicio>();
            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _localidadServicio = ObjectFactory.GetInstance<ILocalidadServicio>();
            _condicionIvaServicio = ObjectFactory.GetInstance<ICondicionIvaServicio>();

            nudLimiteCompra.Maximum = 1000000;
            nudLimiteCompra.Minimum = 100;

            txtTelefono.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoSimbolos(sender, args);
                NoLetras(sender, args);
            };

            txtApellido.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoSimbolos(sender, args);
                NoNumeros(sender, args);
            };

            txtNombre.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoSimbolos(sender, args);
                NoNumeros(sender, args);
            };

            txtDni.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoSimbolos(sender, args);
                NoLetras(sender, args);

            };

            
        }

        public override void CargarDatos(long? entidadId)
        {
            if (entidadId.HasValue)
            {

                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);


                var entidad = (ClienteDto)_clienteServicio.Obtener(typeof(ClienteDto), entidadId.Value);

                if (entidad == null)
                {
                    MessageBox.Show("Ocuriro un error al obtener el registro seleciconado");
                    Close();
                }

                txtApellido.Text = entidad.Apellido;
                txtNombre.Text = entidad.Nombre;
                txtDni.Text = entidad.Dni;
                txtTelefono.Text = entidad.Telefono;
                txtDomicilio.Text = entidad.Direccion;
                chkActivarCuentaCorriente.Checked = entidad.ActivarCtaCte;
                chkLimiteCompra.Checked = entidad.TieneLimiteCompra;
                nudLimiteCompra.Value = entidad.MontoMaximoCtaCte;


                PoblarComboBox(cmbProvincia, _provinciaServicio.Obtener(string.Empty), "Descripcion", "Id");
                cmbProvincia.SelectedValue = entidad.ProvinciaId;

                PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia(entidad.ProvinciaId), "Descripcion", "Id");
                cmbDepartamento.SelectedValue = entidad.DepartamentoId;

                PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento(entidad.DepartamentoId), "Descripcion", "Id");
                cmbLocalidad.SelectedValue = entidad.LocalidadId;

                PoblarComboBox(cmbCondicionIva, _condicionIvaServicio.Obtener(string.Empty), "Descripcion", "Id");
                cmbCondicionIva.SelectedValue = entidad.CondicionIvaId;

                txtMail.Text = entidad.Mail;
                
            }
            else
            {
                LimpiarControles(this);

                PoblarComboBox(cmbCondicionIva, _condicionIvaServicio.Obtener(string.Empty), "Descripcion", "Id");

                PoblarComboBox(cmbProvincia, _provinciaServicio.Obtener(string.Empty), "Descripcion", "Id");

                if (cmbProvincia.Items.Count > 0)
                {
                    PoblarComboBox(cmbDepartamento,
                        _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue)
                        , "Descripcion",
                        "Id");

                    if (cmbDepartamento.Items.Count > 0)
                    {
                        PoblarComboBox(cmbLocalidad,
                            _localidadServicio.ObtenerPorDepartamento((long)cmbProvincia.SelectedValue),
                            "Descripcion",
                            "Id");
                    }
                }
            }
        }
        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj, tieneValorAsociado);
        }
        public override void EjecutarComandoNuevo()
        {
            if (!ValidarEmail(txtMail.Text, error, txtMail))
            {
                MessageBox.Show("Formato de correo Iconrrecto");
                return;
            }

            var nuevoCliente = new ClienteDto
            {
                Apellido = txtApellido.Text,
                Nombre = txtNombre.Text,
                Dni = txtDni.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDomicilio.Text,
                ProvinciaId = (long)cmbProvincia.SelectedValue,
                DepartamentoId = (long)cmbDepartamento.SelectedValue,
                LocalidadId = (long)cmbLocalidad.SelectedValue,
                Mail = txtMail.Text,
                CondicionIvaId = (long)cmbCondicionIva.SelectedValue,
                ActivarCtaCte = chkActivarCuentaCorriente.Checked,
                TieneLimiteCompra = chkLimiteCompra.Checked,
                MontoMaximoCtaCte = nudLimiteCompra.Value,
                Eliminado = false
            };

            _clienteServicio.Insertar(nuevoCliente);

            LimpiarControles(this);


        }

        public override void EjecutarComandoModificar()
        {
            var modificarCliente = new ClienteDto
            {
                Id = EntidadId.Value,
                Apellido = txtApellido.Text,
                Nombre = txtNombre.Text,
                Dni = txtDni.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDomicilio.Text,
                ProvinciaId = (long)cmbProvincia.SelectedValue,
                DepartamentoId = (long)cmbDepartamento.SelectedValue,
                LocalidadId = (long)cmbLocalidad.SelectedValue,
                Mail = txtMail.Text,
                CondicionIvaId = (long)cmbCondicionIva.SelectedValue,
                ActivarCtaCte = chkActivarCuentaCorriente.Checked,
                TieneLimiteCompra = chkLimiteCompra.Checked,
                MontoMaximoCtaCte = nudLimiteCompra.Value,
                Eliminado = false
            };

            _clienteServicio.Modificar(modificarCliente);
        }

        public override void EjecutarComandoEliminar()
        {
           _clienteServicio.Eliminar(typeof(ClienteDto), EntidadId.Value);
        }

        public override bool VerificarDatosObligatorios()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
                return false;

            if (string.IsNullOrEmpty(txtApellido.Text))
                return false;

            if (string.IsNullOrEmpty(txtDni.Text))
                return false;

            if (string.IsNullOrEmpty(txtTelefono.Text))
                return false;


            if (cmbProvincia.Items.Count <= 0)
                return false;

            if (cmbDepartamento.Items.Count <= 0)
                return false;

            return true;
        }

        private void btnNuevaProvincia_Click(object sender, System.EventArgs e)
        {
            var formNuevaProvincia = new _00002_Abm_Provincia(TipoOperacion.Nuevo);
            formNuevaProvincia.ShowDialog();

            if (formNuevaProvincia.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbProvincia,
                    _provinciaServicio.Obtener(string.Empty),
                    "Descripcion",
                    "Id");
            }
        }

        private void btnNuevoDepartamento_Click(object sender, System.EventArgs e)
        {
            var formNuevoDepartamento = new _00004_Abm_Departamento(TipoOperacion.Nuevo);
            formNuevoDepartamento.ShowDialog();

            if (formNuevoDepartamento.RealizoAlgunaOperacion)
            {
                if (cmbProvincia.Items.Count > 0) // si tiene algo
                {
                    PoblarComboBox(cmbDepartamento,
                        _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue)
                        , "Descripcion",
                        "Id");
                }
            }
        }

        private void btnNuevaLocalidad_Click(object sender, System.EventArgs e)
        {
            var formNuevoDepartamento = new _00006_AbmLocalidad(TipoOperacion.Nuevo);
            formNuevoDepartamento.ShowDialog();
        }

        private void btnNuevaCondicionIva_Click(object sender, System.EventArgs e)
        {
            var formNuevoDepartamento = new _00014_Abm_CondicionIva(TipoOperacion.Nuevo);
            formNuevoDepartamento.ShowDialog();

           
        }

        private void cmbProvincia_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (cmbProvincia.Items.Count <= 0) return;

            PoblarComboBox(cmbDepartamento,
                _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue), "Descripcion", "Id");

            if (cmbDepartamento.Items.Count <= 0) return;

            PoblarComboBox(cmbLocalidad,
                _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
        }

        private void cmbDepartamento_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (cmbDepartamento.Items.Count <= 0) return;

            PoblarComboBox(cmbLocalidad,
                _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");

        }
    }
}
