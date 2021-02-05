using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace _100DaysOfCode_ASP_MVC.Models
{
    public class model_producto : conexion
    {
        private SqlCommand com;
        private SqlDataAdapter adapter;

        public model_producto()
        {
            EstablecerConexion();
        }

        public DataTable BuscarProducto(Producto oProducto)
        {
            DataTable dt = new DataTable();
            com = new SqlCommand();
            adapter = new SqlDataAdapter();

            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "BUSCAR_PRODUCTOS";
            com.Connection = conn;
            com.Parameters.Add("@codigo", SqlDbType.VarChar).Value = oProducto.codigo;
            com.Parameters.Add("@nombre", SqlDbType.VarChar).Value = oProducto.nombre;
            com.Parameters.Add("@cantidad", SqlDbType.VarChar).Value = oProducto.cantidad > 0 ? oProducto.cantidad.ToString() : "";
            com.Parameters.Add("@precio", SqlDbType.VarChar).Value = oProducto.precio > 0 ? oProducto.precio.ToString() : "";
            adapter.SelectCommand = com;
            conn.Open();
            adapter.Fill(dt);
            conn.Close();

            return dt;
        }
        public int AgregarProducto(Producto oProducto)
        {
            com = new SqlCommand();

            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "AGREGAR_PRODUCTO";
            com.Parameters.Add("@nombre", SqlDbType.VarChar).Value = oProducto.nombre;
            com.Parameters.Add("@cantidad", SqlDbType.Int).Value = oProducto.cantidad;
            com.Parameters.Add("@precio", SqlDbType.Decimal).Value = oProducto.precio;
            com.Parameters.Add("@rutaImagen", SqlDbType.VarChar).Value = oProducto.rutaImagen;

            com.Connection = conn;
            conn.Open();
            int result = com.ExecuteNonQuery();
            conn.Close();

            return result;
        }
        public int ModificarProducto(Producto oProducto)
        {
            com = new SqlCommand();

            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "MODIFICAR_PRODUCTO";
            com.Parameters.Add("@id", SqlDbType.Int).Value = oProducto.id;
            com.Parameters.Add("@nombre", SqlDbType.VarChar).Value = oProducto.nombre;
            com.Parameters.Add("@cantidad", SqlDbType.Int).Value = oProducto.cantidad;
            com.Parameters.Add("@precio", SqlDbType.Decimal).Value = oProducto.precio;
            com.Parameters.Add("@rutaImagen", SqlDbType.VarChar).Value = oProducto.rutaImagen;

            com.Connection = conn;
            conn.Open();
            int result = com.ExecuteNonQuery();
            conn.Close();

            return result;
        }
        public int EliminarProducto(int id)
        {
            com = new SqlCommand();

            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "ELIMINAR_PRODUCTO";
            com.Parameters.Add("@id", SqlDbType.Int).Value = id;

            com.Connection = conn;
            conn.Open();
            int result = com.ExecuteNonQuery();
            conn.Close();

            return result;
        }
    }
}