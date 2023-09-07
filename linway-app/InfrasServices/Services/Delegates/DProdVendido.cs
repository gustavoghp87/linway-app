using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public class DProdVendido
    {
        public readonly static Predicate<ICollection<ProdVendido>> addProdVendidos = AddProdVendidos;
        public readonly static Predicate<ProdVendido> deleteProdVendido = DeleteProdVendido;
        public readonly static Func<string, string> editDescripcion = EditDescripcion;
        public readonly static Predicate<ProdVendido> editProdVendido = EditProdVendido;
        public readonly static Predicate<ICollection<ProdVendido>> editProdVendidos = EditProdVendidos;
        public readonly static Func<List<ProdVendido>> getProdVendidos = GetProdVendidos;

        private static readonly IServiceBase<ProdVendido> _service = ServicesObjects.ServProdVendido;

        private static bool AddProdVendidos(ICollection<ProdVendido> prodVendidos)
        {
            bool success = _service.AddMany(prodVendidos);
            return success;
        }
        private static bool DeleteProdVendido(ProdVendido prodVendido)
        {
            bool success = _service.Delete(prodVendido);
            return success;
        }
        private static string EditDescripcion(string description)
        {
            if (description.Contains("-")) { description = description.Substring(0, description.IndexOf("-") - 1); };
            if (description.Contains(".")) { description = description.Substring(0, description.IndexOf(".") - 1); };
            return description;
        }
        private static bool EditProdVendido(ProdVendido prodVendido)
        {
            bool success = _service.Edit(prodVendido);
            return success;
        }
        private static bool EditProdVendidos(ICollection<ProdVendido> prodVendidos)
        {
            if (prodVendidos == null || prodVendidos.Count == 0) return false;
            bool success = _service.EditMany(prodVendidos);
            return success;
        }
        private static List<ProdVendido> GetProdVendidos()
        {
            List<ProdVendido> prodVendidos = _service.GetAll();
            return prodVendidos;
        }
    }
}
