using IServicio.Persona.DTOs;
using IServicios.CuentaCorriente;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Windows.Forms;

namespace Presentacion.Core.Cliente
{
    public partial class _00035_ClientePagoCtaCte : FormBase
    {
        private readonly ClienteDto _cliente;
        private readonly ICuentaCorrienteServicio _cuentaCorrienteServicio;
        public bool RealizoPago { get; internal set; }
        public _00035_ClientePagoCtaCte(ClienteDto clienteDto)
        {
            InitializeComponent();
            _cliente = clienteDto;
            _cuentaCorrienteServicio = ObjectFactory.GetInstance<ICuentaCorrienteServicio>();
            RealizoPago = false;            
        }



        private void _00035_ClientePagoCtaCte_Load(object sender, System.EventArgs e)
        {
            if (_cliente != null)
            {
                nudMontoDeuda.Value = _cuentaCorrienteServicio.ObtenerDeudaCliente(_cliente.Id);
                nudMontoDeuda.Select(0, nudMontoPagar.Text.Length);
                nudMontoPagar.Focus();
            }
        }

        private void btnLimpiar_Click(object sender, System.EventArgs e)
        {
            nudMontoPagar.Value = 0;
            nudMontoDeuda.Select(0, nudMontoPagar.Text.Length);
            nudMontoPagar.Focus();
        }

        private void btnPagar_Click(object sender, System.EventArgs e)
        {
            if (nudMontoPagar.Value > 0 )
            {
                if (nudMontoPagar.Value >nudMontoDeuda.Value)
                {
                    var mensaje = "El monto que esta pagando es MAYOR al monto adeudado."
                        + Environment.NewLine
                        + "Desea realizar el pago?";

                    if (MessageBox.Show(mensaje, "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {

                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor ingrese un monto mayor a 0");
                nudMontoDeuda.Select(0, nudMontoPagar.Text.Length);
                nudMontoPagar.Focus();
            }
        }
    }
}
