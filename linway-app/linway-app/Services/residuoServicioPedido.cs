namespace linway_app.Services
{
    public class residuoServicioPedido
    {
        //public static Pedido Limpiar(Pedido pedido)
        //{
        //    pedido.Entregar = 0;
        //    pedido.L = 0;
        //    pedido.Productos = "";
        //    pedido.A = 0;
        //    pedido.E = 0;
        //    pedido.D = 0;
        //    pedido.T = 0;
        //    pedido.Ae = 0;
        //    return pedido;
        //}
        //private static Pedido SumarLiquido(Pedido pedido, long cantidad)
        //{
        //    pedido.L += cantidad;
        //    return pedido;
        //}
        
        //public static List<Venta> CargarVentas(Pedido pedido, List<Venta> lstVentas)
        //{
        //    foreach (Venta venta in lstVentas)
        //    {
        //        foreach (var prodVendido in pedido.ProdVendidos)
        //        {
        //            string prod = "";
        //            if (prodVendido.ProductoId == venta.ProductoId)
        //                prod = prodVendido.Descripcion;
        //            //if (prod == "") return;
        //            if (EsProducto(prodVendido.Descripcion)) pedido.L += prodVendido.Cantidad;
        //            SumarLiquido(pedido, cant);
        //        }
        //    }
        //    return lstVentas;
        //}
        //public static Pedido CargarVentas(Pedido pedido)
        //{
        //    foreach ( ProdVendido prodVendido in pedido.ProdVendidos)
        //    {
        //        if (EsProducto(prodVendido.Descripcion)) pedido.L += prodVendido.Cantidad;
        //    }
        //    return pedido;
        //}

        //if (!EsProducto(prod))
        //{
        //    //Aca agregar para cada producto ( si es polvo, liquido, bolsas o talonario con funciones bool)
        //    if (prod.Contains("olsas"))
        //    {
        //        _pedido.Productos = _pedido.Productos + cant.ToString() + " " + prod.Substring(0, prod.IndexOf('(')) + ". ";
        //    }
        //    else
        //    {
        //        if (prod.Contains("-"))
        //        {
        //            if (EsPolvo(prod))
        //            {
        //                _pedido.Productos = _pedido.Productos + (cant / 20).ToString() + "x20 " + prod.Substring(0, prod.IndexOf('-')) + ". ";
        //            }
        //            else
        //            {
        //                _pedido.Productos = _pedido.Productos + cant.ToString() + " " + prod.Substring(0, prod.IndexOf('-')) + ". ";
        //            }
        //        }
        //        else
        //        {
        //            _pedido.Productos = _pedido.Productos + cant.ToString() + " " + prod + ". ";
        //        }
        //    }
        //}

        //private void SumarPolvo(long c, string cadena)
        //{
        //    if (cadena.Contains("pol - p"))
        //    {
        //        _pedido.T += (c / 20);
        //    }
        //    if (cadena.Contains("san - p"))
        //    {
        //        _pedido.D += (c / 20);
        //    }
        //    if (cadena.Contains("bón - p"))
        //    {
        //        _pedido.E += (c / 20);
        //    }
        //    if (cadena.Contains("son - p"))
        //    {
        //        _pedido.A += (c / 20);
        //    }
        //    if (cadena.Contains("ial - p"))
        //    {
        //        _pedido.Ae += (c / 20);
        //    }
        //}
    }
}
