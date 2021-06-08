using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services
{
    public class ServicioPedido : IServicioPedido
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicioPedido(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(Pedido pedido)
        {
            pedido.Estado = "Activo";
            return _unitOfWork.RepoPedido.Add(pedido);
        }
        public bool Delete(Pedido pedido)
        {
            pedido.Estado = "Eliminado";
            return _unitOfWork.RepoPedido.Edit(pedido);
        }
        public bool Edit(Pedido pedido)
        {
            return _unitOfWork.RepoPedido.Edit(pedido);
        }
        public Pedido Get(long id)
        {
            Pedido pedido = _unitOfWork.RepoPedido.Get(id);
            return pedido == null || pedido.Estado == null || pedido.Estado == "Eliminado"
                ? null : pedido;
        }
        public List<Pedido> GetAll()
        {
            List<Pedido> lst = _unitOfWork.RepoPedido.GetAll();
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            return lst;
        }

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
        public bool AgregarDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId)
        {
            NotaDeEnvio nota = _unitOfWork.RepoNotaDeEnvio.Get(notaDeEnvioId);
            List<DiaReparto> dias = _unitOfWork.RepoDiaReparto.GetAll();
            DiaReparto dia = dias.Find(x => x.Dia == diaDeReparto);
            Reparto reparto = dia.Reparto.ToList().Find(x => x.Nombre == nombreReparto);
            Pedido pedido = reparto.Pedidos.ToList().Find(x => x.ClienteId == nota.ClienteId);

            if (pedido == null) // no existe pedido para este cliente este día y reparto, se hace pedido
            {
                Pedido nuevoPedido = new Pedido
                {
                    ClienteId = nota.ClienteId,
                    Cliente = nota.Cliente,
                    RepartoId = reparto.Id,
                    Direccion = nota.Cliente.Direccion,
                    Entregar = 1,
                    ProductosText = "",
                    ProdVendidos = nota.ProdVendidos,
                    Reparto = reparto
                };
                foreach (var prodVendido in nota.ProdVendidos)
                {
                    nuevoPedido.ProductosText += prodVendido.Descripcion + " | ";
                }
                _unitOfWork.RepoPedido.Add(nuevoPedido);
            }
            else
            {
                foreach (var prodVendido in nota.ProdVendidos)
                {
                    pedido.ProductosText += prodVendido.Descripcion + " | ";
                    pedido.ProdVendidos.Add(prodVendido);
                }
                _unitOfWork.RepoPedido.Edit(pedido);
            }

            return true;
        }
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

        //private void ModificarRenglon(Pedido pedido, long cant, string prod)
        //{
        //if (EsPolvo(prod))
        //{
        //    SumarPolvo(cant, prod);
        //}

        //if ((!EsPolvo(prod)) && (!EsOtro(prod)) && !(prod.Contains("cobrar")))
        //{
        //    SumarLiquido(pedido, cant);
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
        //pedido.Entregar = 1;
        //}

        //private bool EsPolvo(string cadena)
        //{
        //    bool es = false;
        //    if ((cadena.Contains("pol - p")) || (cadena.Contains("san - p")) || (cadena.Contains("bón - p")) || (cadena.Contains("son - p")) || (cadena.Contains("ial - p")))
        //    {
        //        es = true;
        //    }

        //    return es;
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

        //private bool EsOtro(string cadena)
        //{
        //    bool es = false;
        //    if ((cadena.Contains("olsas")) || (cadena.Contains("BONIFI")) || (cadena.Contains("Tal/"))
        //        || (cadena.Contains("queador")) || (cadena.Contains("pendiente")) || (cadena.Contains("actura"))
        //        || (cadena.Contains("favor")) || (cadena.Contains("escuento")))
        //    {
        //        es = true;
        //    }
        //    return es;
        //}

        //private static bool EsProducto(string descripcion)
        //{
        //    bool es = false;
        //    if ((descripcion.Contains("pendiente")) || (descripcion.Contains("BONIFI")) || (descripcion.Contains("actura"))
        //        || (descripcion.Contains("favor")) || (descripcion.Contains("escuento")))
        //    {
        //        es = true;
        //    }
        //    return es;
        //}
    }
}
