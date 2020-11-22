using System;
using System.Linq;
using System.Windows.Forms;
using Aplicacion.Constantes;
using IServicios.Caja;
using IServicios.Caja.DTOs;
using PresentacionBase.Formularios;
using StructureMap;

namespace Presentacion.Core.Caja
{
    public partial class _00040_CierreCaja : FormBase
    {
        private readonly long _cajaId;
        private readonly ICajaServicio _cajaServicio;
        private CajaDto _caja;
        public _00040_CierreCaja(long cajaId)
        {
            InitializeComponent();

            _cajaId = cajaId;
            _cajaServicio = ObjectFactory.GetInstance<ICajaServicio>();
            CargarDatos(cajaId);
        }

        private void CargarDatos(long cajaId)
        {
            _caja = _cajaServicio.Obtener(cajaId);

            if (_caja == null )
            {
                MessageBox.Show("Ocurrió un error al obtener la caja");
            }

            txtCajaInicial.Text = _caja.MontoAperturaStr;

            var efectivo = _caja.Detalles
                .Where(x => x.TipoPago == TipoPago.Efectivo)
                .Sum(x => x.Monto)
                .ToString("C");

            var cheque = _caja.Detalles
                .Where(x => x.TipoPago == TipoPago.Cheque)
                .Sum(x => x.Monto)
                .ToString("C");

            var tarjeta = _caja.Detalles
                .Where(x => x.TipoPago == TipoPago.Tarjeta)
                .Sum(x => x.Monto)
                .ToString("C");

            var ctaCte = _caja.Detalles
                .Where(x => x.TipoPago == TipoPago.CtaCte)
                .Sum(x => x.Monto)
                .ToString("C");

            txtTotalEfectivo.Text = efectivo;
            txtVentas.Text = efectivo;

            txtCheque.Text = cheque;
            txtTarjeta.Text = tarjeta;
            txtCtaCte.Text = ctaCte;
        }

        private void btnVerDetalleVenta_Click(object sender, EventArgs e)
        {
            var fComprobantesCaja = new CompobantesCaja(_caja.Comprobantes);
            fComprobantesCaja.ShowDialog();
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            var cerrarCaja = _cajaServicio.Obtener(_caja.Id);

            if (cerrarCaja == null) throw new Exception("No se pudo cerra la caja");

            _cajaServicio.CerrarCaja(cerrarCaja);

            LimpiarControles(this);

            txtTotalEfectivo.Text = "";
            txtVentas.Text = "";
            txtCheque.Text = "";
            txtTarjeta.Text = "";
            txtCtaCte.Text = "";

        }
    }
}
