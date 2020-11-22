using IServicios.Caja.DTOs;
using PresentacionBase.Formularios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentacion.Core.Caja
{
    public partial class CompobantesCaja : FormBase
    {
        public CompobantesCaja(List<ComprobanteCajaDto> comprobantes)
        {
            InitializeComponent();

            dgvGrilla.DataSource = comprobantes.ToList();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
