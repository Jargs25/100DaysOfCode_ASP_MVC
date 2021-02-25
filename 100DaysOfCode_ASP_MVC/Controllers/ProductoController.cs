using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _100DaysOfCode_ASP_MVC.Models;
using System.IO;
using _100DaysOfCode_ASP_MVC.WCFProductos;

namespace _100DaysOfCode_ASP_MVC.Controllers
{
    public class ProductoController : Controller
    {
        WCFProductoClient svcProducto = new WCFProductoClient();

        // GET: Producto
        public ActionResult Index(int id = 0)
        {
            Producto[] aryProductos;

            try
            {
                Producto oProducto = GetProducto(Request.Form[4], Convert.ToInt32(Request.Form[5] == "" ? "0" : Request.Form[5]), Convert.ToDouble(Request.Form[6] == "" ? "0" : Request.Form[6]));
                oProducto.codigo = Request.Form[3];
                
                aryProductos = svcProducto.BuscarProductos(oProducto);

                ViewBag.Producto = oProducto;
                ViewBag.Accion = "create";

                if (id > 0)
                {
                    ViewBag.Producto = GetProducto(id, aryProductos);
                    ViewBag.Accion = "edit";
                }
            }
            catch
            {
                aryProductos = svcProducto.BuscarProductos(GetProducto());

                ViewBag.Producto = new Producto();
                ViewBag.Accion = "create";

                if (id > 0)
                {
                    ViewBag.Producto = GetProducto(id, aryProductos);
                    ViewBag.Accion = "edit";
                }
            }

            return View(aryProductos);
        }

        // POST: Producto/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            if (SonValidos(collection))
            {
                HttpPostedFileBase image = Request.Files[0];
                string nuevaImagen = image.FileName != "" ? image.FileName : "NoDisponible";

                try
                {
                    Producto oProducto = GetProducto(collection[4], Convert.ToInt32(collection[5]), Convert.ToDouble(collection[6]));
                    oProducto.rutaImagen = nuevaImagen != "NoDisponible" ? nuevaImagen : "NoDisponible";
                    oProducto.imagen = GetByteArrayImage(image.InputStream);

                    svcProducto.AgregarProducto(oProducto);
                }
                catch
                {
                    ViewBag.Producto = new Producto();
                    ViewBag.Accion = "create";

                    return View("Index",svcProducto.BuscarProductos(GetProducto()));
                }
            }
            else
            {
                ViewBag.Mensaje = "Verifique los campos";
            }

            ViewBag.Producto = new Producto();
            ViewBag.Accion = "create";

            return View("Index", svcProducto.BuscarProductos(GetProducto()));
        }

        // POST: Producto/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            int id = Convert.ToInt32(collection[1]);

            if (SonValidos(collection))
            {
                HttpPostedFileBase image = Request.Files[0];
                string nuevaImagen = image.FileName != "" ? image.FileName : "NoDisponible";

                try
                {
                    Producto oProducto = GetProducto(collection[4], Convert.ToInt32(collection[5]), Convert.ToDouble(collection[6]));
                    oProducto.rutaImagen = nuevaImagen != "NoDisponible" ? nuevaImagen : collection[2] != "NoDisponible" ? collection[2] : nuevaImagen;
                    oProducto.imagen = GetByteArrayImage(image.InputStream);
                    oProducto.id = id;

                    svcProducto.ModificarProducto(oProducto);
                    ViewBag.Producto = oProducto;

                    return RedirectToAction("Index");
                }
                catch
                {
                    Producto[] aryProductos = svcProducto.BuscarProductos(GetProducto());
                    ViewBag.Mensaje = "Verifique los campos";
                    ViewBag.Producto = GetProducto(id, aryProductos);
                    ViewBag.Accion = "edit";

                    return View("Index", aryProductos);
                }
            }
            else
            {
                Producto[] aryProductos = svcProducto.BuscarProductos(GetProducto());
                ViewBag.Mensaje = "Verifique los campos";
                ViewBag.Producto = GetProducto(id, aryProductos);
                ViewBag.Accion = "edit";

                return View("Index", aryProductos);
            }

        }

        // POST: Producto/Delete/5
        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            int id = Convert.ToInt32(collection[1]);
            try
            {
                Producto oProducto = GetProducto();
                oProducto.id = id;
                oProducto.rutaImagen = collection[1];

                svcProducto.EliminarProducto(oProducto);

                return RedirectToAction("Index");
            }
            catch
            {
                Producto[] aryProductos = svcProducto.BuscarProductos(GetProducto());
                ViewBag.Mensaje = "Verifique los campos";
                ViewBag.Producto = GetProducto(id, aryProductos);
                ViewBag.Accion = "edit";

                return View("Index", aryProductos);
            }
        }

        public ActionResult GetImagen(string imagen)
        {
            string rutaFisica = Server.MapPath("~/Content/Producto/")+ imagen;

            if (imagen == "info.jpg" || imagen == "warning.png")
                rutaFisica = Server.MapPath("~/Content/Iconos/") + imagen;

            if (imagen != "NoDisponible" && System.IO.File.Exists(rutaFisica))
            {
                byte[] image = System.IO.File.ReadAllBytes(rutaFisica);
                string ext = imagen.Split('.')[1] == "png" ? "png" : "jpeg";

                return File(image,"image/" + ext);
            }
            else
            {
                return Content("");
            }
        }
        private byte[] GetByteArrayImage(Stream sm)
        {
            MemoryStream ms = new MemoryStream();
            sm.CopyTo(ms);
            return ms.ToArray();
        }
        private bool SonValidos(FormCollection form)
        {
            if (form[4] != "" && form[5] != "")
            {
                return true;
            }

            return false;
        }

        private Producto GetProducto()
        {
            Producto oProducto = new Producto();

            oProducto.id = 0;
            oProducto.codigo = "";
            oProducto.nombre = "";
            oProducto.cantidad = 0;
            oProducto.precio = 0;
            oProducto.rutaImagen = "NoDisponible";

            return oProducto;
        }
        private Producto GetProducto(string nombre, int cantidad, double precio)
        {
            Producto oProducto = new Producto();

            oProducto.id = 0;
            oProducto.codigo = "";
            oProducto.nombre = nombre;
            oProducto.cantidad = cantidad;
            oProducto.precio = precio;
            oProducto.rutaImagen = "NoDisponible";

            return oProducto;
        }
        private Producto GetProducto(int id, Producto[] aryProductos)
        {
            Producto oProducto = new Producto();

            for (int i = 0; i < aryProductos.Length; i++)
            {
                if (aryProductos[i].id == id)
                {
                    oProducto = GetProducto(aryProductos[i].nombre, aryProductos[i].cantidad, aryProductos[i].precio);
                    oProducto.codigo = aryProductos[i].codigo;
                    oProducto.rutaImagen = aryProductos[i].rutaImagen;
                    oProducto.imagen = aryProductos[i].imagen;
                    oProducto.id = aryProductos[i].id;
                    break;
                }
            }

            return oProducto;
        }
    }
}