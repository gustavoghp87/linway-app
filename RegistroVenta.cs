using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    [Serializable]
    class RegistroVenta
    {
        static uint ultimoID;
        [DisplayName("ID:")]
        public uint id { get; set; }
        [DisplayName("FECHA")]
        public string fecha { get; set; }
        [DisplayName("CLIENTE")]
        public string cliente { get; set; }

        private List<ProdVendido> productosVendidos = new List<ProdVendido>();

        public RegistroVenta()
        {
            ultimoID++;
            this.id = ultimoID;
            this.fecha = DateTime.Today.ToString().Substring(0, 10);
            this.cliente = "Venta particular";
        }

        public RegistroVenta(uint ultimo)
        {
            ultimoID = ultimo;
        }

        public void recibirListaVentas(List<Venta> ventas, List<Producto> productos)
        {
            foreach (Venta vActual in ventas)
            {
                float precio = productos.Find(x => x.Nombre.Equals(vActual.Producto)).Precio;
                ProdVendido nuevoPV = new ProdVendido(vActual.Producto, vActual.Cantidad, precio*vActual.Cantidad);
                this.productosVendidos.Add(nuevoPV);
            }
        }

        public void recibirProdVendidos(List<ProdVendido> pv, string client)
        {
            this.productosVendidos.AddRange(pv);
            this.cliente = client;
        }

        public List<ProdVendido> obtenerPV()
        {
            return this.productosVendidos;
        }

        public float obtenerTotal()
        {
            float total = 0;
            foreach (ProdVendido pvActual in productosVendidos)
            {
                total += pvActual.Subtotal;
            }
            return total;
        }
       
    }
}
