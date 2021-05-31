using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Services
{
    class ServicioReparto : IServicioReparto
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicioReparto(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(Reparto reparto)
        {
            return _unitOfWork.RepoReparto.Add(reparto);
        }
        public bool Delete(Reparto reparto)
        {
            return _unitOfWork.RepoReparto.Delete(reparto);
        }
        public bool Edit(Reparto reparto)
        {
            return _unitOfWork.RepoReparto.Edit(reparto);
        }
        public Reparto Get(long id)
        {
            return _unitOfWork.RepoReparto.Get(id);
        }
        public List<Reparto> GetAll()
        {
            return _unitOfWork.RepoReparto.GetAll();
        }

        public void LimpiarDatos(Reparto reparto)
        {
            reparto.Ta = 0;
            reparto.Te = 0;
            reparto.Td = 0;
            reparto.Tt = 0;
            reparto.Tae = 0;
            reparto.TotalB = 0;
            reparto.Tl = 0;
            reparto.Pedidos.Clear();
        }
        public bool AgregarPedidoAReparto(long clientId, string dia, string repartoNombre, List<ProdVendido> lstProdVendidos)
        {
            if (lstProdVendidos == null || lstProdVendidos.Count == 0) return false;
            Cliente cliente = _unitOfWork.RepoCliente.Get(clientId);
            if (cliente == null) return false;
            List<DiaReparto> dias = _unitOfWork.RepoDiaReparto.GetAll();
            if (dias == null) return false;
            DiaReparto diaRep = dias.Find(x => x.Dia == dia);
            if (diaRep == null) return false;
            Reparto reparto = diaRep.Reparto.ToList().Find(x => x.Nombre == repartoNombre);
            if (reparto == null) return false;

            Pedido pedidoViejo = reparto.Pedidos.ToList().Find(x => x.ClienteId == cliente.Id);
            // saber si el cliente ya tenía un pedido para este reparto

            if (pedidoViejo == null)            // no tiene, crear pedido de cero
            {
                Pedido nuevoPedido = new Pedido
                {
                    ClienteId = cliente.Id,
                    Direccion = cliente.Direccion,
                    RepartoId = reparto.Id,
                    ProdVendidos = lstProdVendidos,
                    Entregar = 1,
                    Productos = "",
                    A = 0,
                    Ae = 0,
                    D = 0,
                    E = 0,
                    Id = 0,
                    L = 0,
                    T = 0
                };
                foreach (var prodVendido in lstProdVendidos)
                {
                    nuevoPedido.Productos += prodVendido.Descripcion + " | ";
                }
                _unitOfWork.RepoPedido.Add(nuevoPedido);
            }
            else        // sí tiene, sumar pedido a lo pedido
            {
                foreach (var prodVendido in lstProdVendidos)
                {
                    pedidoViejo.Productos += prodVendido.Descripcion + " | ";
                }
                _unitOfWork.RepoPedido.Edit(pedidoViejo);
            }

            return true;
        }
        //private static Reparto ModificarContadores(Reparto reparto, long cant, string descripcion)
        //{
        //    if (EsPolvo(descripcion))
        //    {
        //        SumarPolvo(reparto, cant, descripcion);
        //    }
        //    if ((!EsPolvo(descripcion)) && (!EsOtro(descripcion)))
        //    {
        //        reparto.Tl += cant;
        //    }
        //    return reparto;
        //}
        //private static bool EsPolvo(string cadena)
        //{
        //    bool es = false;
        //    if ((cadena.Contains("pol - p")) || (cadena.Contains("san - p"))
        //        || (cadena.Contains("bón - p")) || (cadena.Contains("son - p"))
        //        || (cadena.Contains("ial - p"))
        //    )
        //    {
        //        es = true;
        //    }
        //    return es;
        //}
        //private static Reparto SumarPolvo(Reparto reparto, long c, string cadena)
        //{
        //    if (cadena.Contains("pol - p"))
        //    {
        //        reparto.Tt += (c / 20);
        //    }
        //    if (cadena.Contains("san - p"))
        //    {
        //        reparto.Td += (c / 20);
        //    }
        //    if (cadena.Contains("bón - p"))
        //    {
        //        reparto.Te += (c / 20);
        //    }
        //    if (cadena.Contains("son - p"))
        //    {
        //        reparto.Ta += (c / 20);
        //    }
        //    if (cadena.Contains("ial - p"))
        //    {
        //        reparto.Tae += (c / 20);
        //    }
        //    reparto.TotalB = reparto.Tt + reparto.Td + reparto.Te + reparto.Ta + reparto.Tae;
        //    return reparto;
        //}

        //private static bool EsOtro(string cadena)
        //{
        //    bool es = false;
        //    if ((cadena.Contains("olsas")) || (cadena.Contains("cobrar")) || (cadena.Contains("Tal/"))
        //        || (cadena.Contains("queador")) || (cadena.Contains("BONIFI")) || (cadena.Contains("pendiente"))
        //        || (cadena.Contains("actura")) || (cadena.Contains("favor")) || (cadena.Contains("escuento"))
        //    )
        //    {
        //        es = true;
        //    }
        //    return es;
        //}
    }
}
