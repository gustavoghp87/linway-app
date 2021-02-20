using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
{
    [Serializable]

    class RegistroVenta
    {
        static uint UltimoID;
        [DisplayName("ID:")]
        public uint Id { get; set; }
        [DisplayName("FECHA")]
        public string Fecha { get; set; }
        [DisplayName("CLIENTE")]
        public string Cliente { get; set; }
        private readonly List<ProdVendido> productosVendidos = new List<ProdVendido>();

        public RegistroVenta()
        {
            UltimoID++;
            Id = UltimoID;
            Fecha = DateTime.Today.ToString().Substring(0, 10);
            Cliente = "Venta particular";
        }

        public RegistroVenta(uint ultimo)
        {
            UltimoID = ultimo;
        }

        public RegistroVenta(uint Id, string Fecha, string Cliente)
        {
            this.Id = Id;
            this.Fecha = Fecha;
            this.Cliente = Cliente;
        }

        public void RecibirListaVentas(List<Venta> ventas, List<Producto> productos)
        {
            foreach (Venta vActual in ventas)
            {
                float precio = productos.Find(x => x.Nombre.Equals(vActual.Producto)).Precio;
                ProdVendido nuevoPV = new ProdVendido(vActual.Producto, vActual.Cantidad, precio * vActual.Cantidad);
                productosVendidos.Add(nuevoPV);
            }
        }

        public void RecibirProdVendidos(List<ProdVendido> pv, string client)
        {
            productosVendidos.AddRange(pv);
            Cliente = client;
        }

        public List<ProdVendido> ObtenerPV()
        {
            return productosVendidos;
        }

        public float ObtenerTotal()
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
