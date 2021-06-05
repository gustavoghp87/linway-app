using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms.Delegates
{
    public static class DClientes
    {
        public delegate List<Cliente> DGetClientes();
        public delegate Cliente DGetCliente(long clientId);
        public delegate Cliente DGetClientePorDireccion(string direccion);
        public delegate Cliente DGetClientePorDireccionExacta(string direccion);
        public delegate bool DAddCliente(Cliente cliente);
        public delegate void DEditCliente(Cliente cliente);

        public readonly static DAddCliente addCliente = new DAddCliente(AddCliente);
        public readonly static DEditCliente editCliente = new DEditCliente(EditCliente);
        public readonly static DGetClientes getClientes = new DGetClientes(GetClientes);
        public readonly static DGetCliente getCliente = new DGetCliente(GetCliente);
        public readonly static DGetClientePorDireccion getClientePorDireccion
            = new DGetClientePorDireccion(GetClientePorDireccion);
        public readonly static DGetClientePorDireccionExacta getClientePorDireccionExacta
            = new DGetClientePorDireccionExacta(GetClientePorDireccionExacta);

        public static List<Cliente> GetClientes()
        {
            return Form1._servCliente.GetAll();
        }
        public static Cliente GetCliente(long clientId)
        {
            return Form1._servCliente.Get(clientId);
        }
        public static Cliente GetClientePorDireccion(string direccion)
        {
            return GetClientes().Find(x => x.Direccion.ToLower().Contains(direccion.ToLower()));
        }
        public static Cliente GetClientePorDireccionExacta(string direccion)
        {
            return GetClientes().Find(x => x.Direccion.Contains(direccion));
        }
        public static bool AddCliente(Cliente cliente)
        {
            return Form1._servCliente.Add(cliente);
        }
        public static void EditCliente(Cliente cliente)
        {
            bool response = Form1._servCliente.Edit(cliente);
            if (!response) MessageBox.Show("Falló editando Cliente en base de datos");
        }
        public static void DeleteCliente(Cliente cliente)
        {
            bool response = Form1._servCliente.Delete(cliente);
            if (!response) MessageBox.Show("Falló guardado Cliente en base de datos");
        }

    }
}
