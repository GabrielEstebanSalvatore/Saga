using IServicios.Articulo.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Core.Comprobantes.Clases
{
    public partial class CambiarCantidad : Form
    {
        private ItemView _itemSelecionado;
        public ItemView Item => _itemSelecionado;

        public ArticuloDto _articuloConLimite;

        public CambiarCantidad(ItemView item, ArticuloDto limiteDeVentas)
        {
            InitializeComponent();

            _itemSelecionado = item;
            _articuloConLimite = limiteDeVentas;
        }

        private void CambiarCantidad_Load(object sender, EventArgs e)
        {
            if (_itemSelecionado == null)
            {
                MessageBox.Show("Ocurrió un error al obtener el artículo");
                Close();    
            }
           
            lblArticulo.Text = _itemSelecionado.Descripcion;
            nudCantidad.Value = _itemSelecionado.Cantidad;
            nudCantidad.Select(0, nudCantidad.Text.Length);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _itemSelecionado = null;
            _articuloConLimite = null;
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (_articuloConLimite.LimiteVenta <= nudCantidad.Value)
            {
                MessageBox.Show($"El valor a modificar tiene que ser menor de {_articuloConLimite.LimiteVenta}");
                return;
            }
            _itemSelecionado.Cantidad = nudCantidad.Value;
            Close();
        }
    }
}
