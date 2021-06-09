using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DReparto
    {
        public delegate void DAddReparto(Reparto pedido);
        public delegate void DAddPedidoAReparto(Reparto reparto, Cliente cliente,
            List<ProdVendido> lstProdVendidos);
        public delegate List<Reparto> DGetRepartos();
        public delegate List<Reparto> DGetRepartosPorDia(string diaReparto);
        public delegate Reparto DGetRepartoPorNombre(string dia, string nombre);
        public delegate void DEditReparto(Reparto reparto);

        public readonly static DAddReparto addReparto
            = new DAddReparto(AddReparto);
        public readonly static DAddPedidoAReparto addPedidoAReparto
            = new DAddPedidoAReparto(AddPedidoAReparto);
        public readonly static DGetRepartos getRepartos
            = new DGetRepartos(GetRepartos);
        public readonly static DGetRepartosPorDia getRepartosPorDia
            = new DGetRepartosPorDia(GetRepartosPorDia);
        public readonly static DGetRepartoPorNombre getRepartoPorNombre
            = new DGetRepartoPorNombre(GetRepartoPorNombre);
        public readonly static DEditReparto editReparto
            = new DEditReparto(EditReparto);

        private static void AddReparto(Reparto reparto)
        {
            bool response = Form1._servReparto.Add(reparto);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Reparto a la base de datos");
        }
        private static void AddPedidoAReparto(Reparto reparto, Cliente cliente,
            List<ProdVendido> lstProdVendidos)
        {
            bool response = AgregarPedidoAReparto(cliente.Id,
                reparto.DiaReparto.Dia, reparto.Nombre, lstProdVendidos);
            if (!response) MessageBox.Show("Algo falló al agregar Pedido a Reparto en base de datos");
        }
        private static List<Reparto> GetRepartos()
        {
            return Form1._servReparto.GetAll();
        }
        private static List<Reparto> GetRepartosPorDia(string diaReparto)
        {
            try
            {
                List<DiaReparto> lstDiasRep = Form1._servDiaReparto.GetAll();
                List<Reparto> lstRepartos = lstDiasRep.Find(x => x.Dia == diaReparto).Reparto.ToList();
                return lstRepartos;
            }
            catch
            {
                return null;
            }
        }
        private static Reparto GetRepartoPorNombre(string dia, string nombre)
        {
            try
            {
                return Form1._servDiaReparto.GetAll()
                    .Find(x => x.Dia == dia).Reparto.ToList()
                    .Find(x => x.Nombre == nombre);
            }
            catch { return null; }
        }
        private static void EditReparto(Reparto reparto)
        {
            bool response = Form1._servReparto.Edit(reparto);
            if (!response) MessageBox.Show("Algo falló al editar el Reparto en la base de datos");
        }

        public static bool AgregarPedidoAReparto(long clientId, string dia, string repartoNombre, List<ProdVendido> lstProdVendidos)
        {
            if (lstProdVendidos == null || lstProdVendidos.Count == 0) return false;
            Cliente cliente = Form1._servCliente.Get(clientId);
            if (cliente == null) return false;
            List<DiaReparto> dias = Form1._servDiaReparto.GetAll();
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
                    ProductosText = "",
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
                    nuevoPedido.ProductosText += prodVendido.Descripcion + " | ";
                }
                Form1._servPedido.Add(nuevoPedido);
            }
            else        // sí tiene, sumar pedido a lo pedido
            {
                foreach (var prodVendido in lstProdVendidos)
                {
                    pedidoViejo.ProductosText += prodVendido.Descripcion + " | ";
                }
                Form1._servPedido.Edit(pedidoViejo);
            }

            return true;
        }
        public static void LimpiarDatos(Reparto reparto)
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
    }
}
