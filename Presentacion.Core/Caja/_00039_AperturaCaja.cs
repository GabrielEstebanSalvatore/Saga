using Aplicacion.Constantes;
using IServicio.Configuracion;
using IServicios.Caja;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Core.Caja
{
    public partial class _00039_AperturaCaja : Form
    {
        private readonly ICajaServicio _cajaServicio;
        private readonly IConfiguracionServicio _configuracionServicio;

        public bool CajaAbierta { get; set; }

        public _00039_AperturaCaja(ICajaServicio cajaServicio, 
            IConfiguracionServicio configuracionServicio )
        {
            InitializeComponent();

            _cajaServicio = cajaServicio;
            _configuracionServicio = configuracionServicio;

            DoubleBuffered = true;
        }

        private void _00039_AperturaCaja_Load(object sender, EventArgs e)
        {
            var confuguracion = _configuracionServicio.Obtener();

            if (confuguracion.IngresoManualCajaInicial)
            {
                nudMonto.Value = 0;
                nudMonto.Select(0, nudMonto.Text.Length);
                nudMonto.Focus();
            }
            else
            {
                var ultimoValor = _cajaServicio.ObtenerMontoCajaAnterior(Identidad.UsuarioId);

                nudMonto.Value = ultimoValor;
                nudMonto.Select(0, nudMonto.Text.Length);
                nudMonto.Focus();
            }
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            try
            {
                _cajaServicio.Abrir(Identidad.UsuarioId, nudMonto.Value, DateTime.Now);
                MessageBox.Show("Los datos se grabaron correctamente.");
                CajaAbierta = true;
                Close();
            }
            catch (Exception exeption)
            {

                MessageBox.Show(exeption.Message, "ERROR");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            nudMonto.Value = 0;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            CajaAbierta = false;
            Close();
        }
    }
}

