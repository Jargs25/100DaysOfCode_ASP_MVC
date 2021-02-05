using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _100DaysOfCode_ASP_MVC.Models;
using System.IO;

namespace _100DaysOfCode_ASP_MVC.Controllers
{
    public class ProductoController : Controller
    {
        model_producto mProducto = new model_producto();
        // GET: Producto
        public ActionResult Index()
        {
            return View(GetProductos(new Producto()));
        }

        // GET: Producto/Details/5
        public ActionResult Details(int id)
        {
            return View(GetProducto(id));
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            if (SonValidos(collection))
            {
                HttpPostedFileBase image = Request.Files[0];
                string nuevaImagen = image.FileName != "" ? image.FileName : "NoDisponible";

                string rutaFisica = Server.MapPath("~/Content/Producto/");

                try
                {
                    if (!Directory.Exists(rutaFisica))
                        Directory.CreateDirectory(rutaFisica);

                    Producto oProducto = new Producto("", collection[2], Convert.ToInt32(collection[3]), Convert.ToDouble(collection[4]), nuevaImagen != "NoDisponible" ? nuevaImagen : "NoDisponible");

                    mProducto.AgregarProducto(oProducto);

                    if (nuevaImagen != "NoDisponible")
                    {
                        image.SaveAs(rutaFisica + image.FileName);
                    }

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                ViewBag.Mensaje = "Verifique los campos";
                return View();
            }
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int id)
        {
            return View(GetProducto(id));
        }

        // POST: Producto/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (SonValidos(collection))
            {
                HttpPostedFileBase image = Request.Files[0];
                string nuevaImagen = image.FileName != "" ? image.FileName : "NoDisponible";

                try
                {
                    Producto oProducto = new Producto("", collection[3], Convert.ToInt32(collection[4]), Convert.ToDouble(collection[5]), nuevaImagen != "NoDisponible" ? nuevaImagen : collection[6] != "NoDisponible" ? collection[6] : nuevaImagen);
                    oProducto.id = id;

                    mProducto.ModificarProducto(oProducto);

                    if (nuevaImagen != "NoDisponible")
                    {
                        string rutaFisica = Server.MapPath("~/Content/Producto/");
                        image.SaveAs(rutaFisica + image.FileName);
                    }

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(new Producto());
                }
            }
            else
            {
                return View(GetProducto(id));
            }
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int id)
        {
            return View(GetProducto(id));
        }

        // POST: Producto/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                mProducto.EliminarProducto(id);

                string rutaFisica = Server.MapPath("~/Content/Producto/") + collection[1];
                if (collection[1] != "NoDisponible" && System.IO.File.Exists(rutaFisica))
                {
                    System.IO.File.Delete(rutaFisica);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Producto());
            }
        }

        private Producto GetProducto(int id)
        {
            System.Data.DataTable dt = mProducto.BuscarProducto(new Producto());

            Producto item = new Producto();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt.Rows[i].ItemArray[0]) == id)
                {
                    item.id = Convert.ToInt32(dt.Rows[i].ItemArray[0]);
                    item.codigo = dt.Rows[i].ItemArray[1].ToString();
                    item.nombre = dt.Rows[i].ItemArray[2].ToString();
                    item.cantidad = Convert.ToInt32(dt.Rows[i].ItemArray[3]);
                    item.precio = Convert.ToDouble(dt.Rows[i].ItemArray[4]);
                    item.rutaImagen = dt.Rows[i].ItemArray[5].ToString();
                    break;
                }
            }

            return item;
        }
        private List<Producto> GetProductos()
        {
            List<Producto> lstProductos = new List<Producto>();

            lstProductos.Add(new Producto("00001", "Producto1", 1, 10.50, "NoDisponible"));
            lstProductos.Add(new Producto("00002", "Producto2", 2, 11.00, "NoDisponible"));
            lstProductos.Add(new Producto("00003", "Producto3", 3, 11.50, "NoDisponible"));
            lstProductos.Add(new Producto("00004", "Producto4", 4, 12.00, "NoDisponible"));
            lstProductos[0].id = 0;
            lstProductos[1].id = 1;
            lstProductos[2].id = 2;
            lstProductos[3].id = 3;

            return lstProductos;
        }
        private List<Producto> GetProductos(Producto oProducto)
        {
            List<Producto> lstProductos = new List<Producto>();
            model_producto mProducto = new model_producto();

            System.Data.DataTable dt = mProducto.BuscarProducto(oProducto);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Producto item = new Producto();
                item.id = Convert.ToInt32(dt.Rows[i].ItemArray[0]);
                item.codigo = dt.Rows[i].ItemArray[1].ToString();
                item.nombre = dt.Rows[i].ItemArray[2].ToString();
                item.cantidad = Convert.ToInt32(dt.Rows[i].ItemArray[3]);
                item.precio = Convert.ToDouble(dt.Rows[i].ItemArray[4]);
                item.rutaImagen = dt.Rows[i].ItemArray[5].ToString();

                lstProductos.Add(item);
            }

            return lstProductos;
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
        public bool SonValidos(FormCollection form)
        {
            if (form[2] != "" && form[3] != "")
            {
                return true;
            }

            return false;
        }
    }
}
