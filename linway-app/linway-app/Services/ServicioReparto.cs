﻿using linway_app.Models;
using linway_app.Repositories.Interfaces;
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

        //public void AgregarRepartoPorVenta(Reparto reparto, List<Venta> lstVentas, Cliente cliente)
        //{
        //    if (cliente == null) return;
        //    Pedido pedido = reparto.Pedidos.ToList().Find(x => x.ClienteId == cliente.Id);
        //    if (pedido == null)
        //    {
        //        pedido = new Pedido
        //        {
        //            ClienteId = cliente.Id,
        //            Direccion = cliente.Direccion,
        //            RepartoId = reparto.Id,
        //            A = 0, D = 0, E = 0, T = 0, Ae = 0, L = 0,
        //            Cliente = cliente,
        //            Entregar = 1,
        //            Productos = 
        //        };
        //        reparto.Pedidos.Add(pedido);
        //    }
        //    var servicio = new ServicioPedido(pedido);
        //    servicio.ModificarPorV(lstVentas);
        //    pedido = servicio._pedido;
        //    foreach (Venta venta in lstVentas)
        //    {
        //        Producto prod = _unitOfWork.RepoProducto.GetAll().Find(x => x.Id == venta.ProductoId);
        //        ModificarContadores(reparto, venta.Cantidad, prod.Nombre);
        //    }
        //}
        public bool AgregarPedidoARepartoPorNota(Reparto reparto, Cliente cliente, List<ProdVendido> lstProdVendidos)
        {
            Pedido pedidoViejo = reparto.Pedidos.ToList().Find(x => x.ClienteId == cliente.Id);

            // saber si el cliente ya tenía un pedido para este reparto

            // no tiene, crear pedido de cero
            if (pedidoViejo == null)
            {
                string direccion = cliente.Direccion;
                Pedido nuevoPedido = new Pedido
                {
                    ClienteId = cliente.Id,
                    Direccion = direccion.Contains("-") ? direccion.Substring(0, direccion.IndexOf('-')) : direccion,
                    RepartoId = reparto.Id,
                    ProdVendidos = lstProdVendidos,
                    Cliente = cliente,
                    Reparto = reparto,
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
                DiaReparto diaRep = reparto.DiaReparto;
                foreach (var rep in diaRep.Reparto)
                {
                    if (rep.Id == reparto.Id) rep.Pedidos.Add(nuevoPedido);
                }
                cliente.Pedido.Add(nuevoPedido);
                reparto.Pedidos.Add(nuevoPedido);
                _unitOfWork.RepoCliente.Edit(cliente);
                _unitOfWork.RepoReparto.Edit(reparto);
                _unitOfWork.RepoPedido.Add(nuevoPedido);
                _unitOfWork.RepoDiaReparto.Edit(diaRep);
            }
            // sí tiene, sumar pedido a lo pedido
            else
            {
                foreach (var prodVendido in lstProdVendidos)
                {
                    pedidoViejo.Productos += prodVendido.Descripcion + " | ";
                    pedidoViejo.Reparto = reparto;
                    pedidoViejo.ProdVendidos.ToList().AddRange(lstProdVendidos);
                }
                // modificar cliente y reparto y dia reparto
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
