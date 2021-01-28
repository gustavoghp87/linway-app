﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
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
            id = ultimoID;
            fecha = DateTime.Today.ToString().Substring(0, 10);
            cliente = "Venta particular";
        }

        public RegistroVenta(uint ultimo)
        {
            ultimoID = ultimo;
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
            cliente = client;
        }

        public List<ProdVendido> obtenerPV()
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
