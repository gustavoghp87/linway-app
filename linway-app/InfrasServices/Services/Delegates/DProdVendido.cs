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
        public readonly static Action<ProdVendido> deleteProdVendido = DeleteProdVendido;
        public readonly static Func<string, string> editDescripcion = EditDescripcion;
        public readonly static Action<ProdVendido> editProdVendido = EditProdVendido;
        public readonly static Func<List<ProdVendido>> getProdVendido = GetProdVendidos;
        public readonly static Func<string, ProdVendido> getProdVendidoPorNombre = GetProdVendidoPorNombre;
        public readonly static Func<string, ProdVendido> getProdVendidoPorNombreExacto = GetProdVendidoPorNombreExacto;

        private static readonly IServiceBase<ProdVendido> _service = ServicesObjects.ServProdVendido;
        private static void AddProdVendido(ProdVendido prodVendido)
        {
            bool response = _service.Add(prodVendido);
            if (!response) Console.WriteLine("Algo falló al agregar Producto Vendido a la base de datos");
        }
        private static void DeleteProdVendido(ProdVendido prodVendido)
        {
            bool response = _service.Delete(prodVendido);
            if (!response) Console.WriteLine("Algo falló al eliminar Producto Vendido de la base de datos");
        }
        private static string EditDescripcion(string descripcion)
        {
            if (descripcion.Contains("-")) { descripcion = descripcion.Substring(0, descripcion.IndexOf("-")); };
            if (descripcion.Contains(".")) { descripcion = descripcion.Substring(0, descripcion.IndexOf(".")); };
            return descripcion;
        }
        private static void EditProdVendido(ProdVendido prodVendido)
        {
            bool response = _service.Edit(prodVendido);
            if (!response) Console.WriteLine("Algo falló al editar Producto Vendido en la base de datos");
        }
        private static List<ProdVendido> GetProdVendidos()
        {
            return _service.GetAll();
        }
        private static ProdVendido GetProdVendidoPorNombre(string descripcion)
        {
            List<ProdVendido> lst = GetProdVendidos();
            return lst.Find(x => x.Descripcion.ToLower().Contains(descripcion.ToLower()) && x.Estado != null && x.Estado != "Eliminado");
        }
        private static ProdVendido GetProdVendidoPorNombreExacto(string descripcion)
        {
            List<ProdVendido> lst = GetProdVendidos();
            return lst.Find(x => x.Descripcion.Contains(descripcion) && x.Estado != null && x.Estado != "Eliminado");
        }
    }
}