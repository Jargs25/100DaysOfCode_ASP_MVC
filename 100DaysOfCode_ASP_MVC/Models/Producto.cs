﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _100DaysOfCode_ASP_MVC.Models
{
    public class Producto
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public string rutaImagen { get; set; }

        public Producto()
        {

        }

        public Producto(string codigo, string nombre, int cantidad, double precio, string rutaImagen)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.cantidad = cantidad;
            this.precio = precio;
            this.rutaImagen = rutaImagen;
        }
    }
}