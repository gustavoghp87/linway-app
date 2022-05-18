﻿using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public class DProdVendido
    {
        public readonly static Action<ProdVendido> addProdVendido = AddProdVendido;
        public readonly static Func<ProdVendido, ProdVendido> addProdVendidoReturnsWithId = AddProdVendidoReturnsWithId;
        public readonly static Action<ProdVendido> deleteProdVendido = DeleteProdVendido;
        public readonly static Func<string, string> editDescripcion = EditDescripcion;
        public readonly static Action<ProdVendido> editProdVendido = EditProdVendido;
        public readonly static Action<ICollection<ProdVendido>> editProdVendidos = EditProdVendidos;
        public readonly static Func<List<ProdVendido>> getProdVendido = GetProdVendidos;

        private static readonly IServiceBase<ProdVendido> _service = ServicesObjects.ServProdVendido;
        private static void AddProdVendido(ProdVendido prodVendido)
        {
            bool response = _service.Add(prodVendido);
            if (!response) Console.WriteLine("Algo falló al agregar Producto Vendido a la base de datos");
        }
        private static ProdVendido AddProdVendidoReturnsWithId(ProdVendido prodVendido)
        {
            bool response = _service.Add(prodVendido);
            if (!response) Console.WriteLine("Algo falló al agregar Producto Vendido a la base de datos (2)");
            return prodVendido;
        }
        private static void DeleteProdVendido(ProdVendido prodVendido)
        {
            bool response = _service.Delete(prodVendido);
            if (!response) Console.WriteLine("Algo falló al eliminar Producto Vendido de la base de datos");
        }
        private static string EditDescripcion(string description)
        {
            if (description.Contains("-")) { description = description.Substring(0, description.IndexOf("-") - 1); };
            if (description.Contains(".")) { description = description.Substring(0, description.IndexOf(".") - 1); };
            return description;
        }
        private static void EditProdVendido(ProdVendido prodVendido)
        {
            bool response = _service.Edit(prodVendido);
            if (!response) Console.WriteLine("Algo falló al editar Producto Vendido en la base de datos");
        }
        private static void EditProdVendidos(ICollection<ProdVendido> prodVendidos)
        {
            bool response = _service.EditMany(prodVendidos);
            if (!response) Console.WriteLine("Algo falló al editar Productos Vendidos en la base de datos");
        }
        private static List<ProdVendido> GetProdVendidos()
        {
            return _service.GetAll();
        }
    }
}
