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
        // GET: Producto
        public ActionResult Index()
        {
            List<Producto> lstProductos = (List<Producto>)Session["productos"];
            if (lstProductos == null)
                lstProductos = GetProductos();

            return View(lstProductos);
        }

        // GET: Producto/Details/5
        public ActionResult Details(int id)
        {
            List<Producto> lstProductos = (List<Producto>)Session["productos"];
            if (lstProductos == null)
                lstProductos = GetProductos();

            for (int i = 0; i < lstProductos.Count; i++)
            {
                if (lstProductos[i].id == id)
                    return View(lstProductos[i]);
            }

            return View(new Producto());
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
                List<Producto> lstProductos = (List<Producto>)Session["productos"];
                if (lstProductos == null)
                    lstProductos = GetProductos();
                HttpPostedFileBase image = Request.Files[0];

                string rutaFisica = Server.MapPath("~/Content/Producto/");

                try
                {
                    if (!Directory.Exists(rutaFisica))
                        Directory.CreateDirectory(rutaFisica);

                    Producto oProducto = new Producto("0000" + lstProductos.Count.ToString(), collection[2], Convert.ToInt32(collection[3]), Convert.ToDouble(collection[4]), image.FileName);
                    oProducto.id = lstProductos.Count;
                    lstProductos.Add(oProducto);

                    image.SaveAs(rutaFisica + image.FileName);

                    Session["productos"] = lstProductos;

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
            List<Producto> lstProductos = (List<Producto>)Session["productos"];
            if (lstProductos == null)
                lstProductos = GetProductos();

            for (int i = 0; i < lstProductos.Count; i++)
            {
                if (lstProductos[i].id == id)
                    return View(lstProductos[i]);
            }

            return View(new Producto());
        }

        // POST: Producto/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            List<Producto> lstProductos = (List<Producto>)Session["productos"];
            if (lstProductos == null)
                lstProductos = GetProductos();
            if (SonValidos(collection))
            {
                HttpPostedFileBase image = Request.Files[0];

                string nuevaImagen = image.FileName != "" ? image.FileName : "NoDisponible";

                try
                {
                    for (int i = 0; i < lstProductos.Count; i++)
                    {
                        if (lstProductos[i].id == id)
                        {
                            lstProductos[i].codigo = collection[2];
                            lstProductos[i].nombre = collection[3];
                            lstProductos[i].cantidad = Convert.ToInt32(collection[4]);
                            lstProductos[i].precio = Convert.ToDouble(collection[5]);
                            lstProductos[i].rutaImagen = nuevaImagen != "NoDisponible" ? nuevaImagen : collection[6] != "NoDisponible" ? collection[6] : nuevaImagen;
                        }

                    }

                    if (nuevaImagen != "NoDisponible")
                    {
                        string rutaFisica = Server.MapPath("~/Content/Producto/");
                        image.SaveAs(rutaFisica + image.FileName);
                    }

                    Session["productos"] = lstProductos;

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(new Producto());
                }
            }
            else
            {
                ViewBag.Mensaje = "Verifique los campos";
                for (int i = 0; i < lstProductos.Count; i++)
                {
                    if (lstProductos[i].id == id)
                        return View(lstProductos[i]);
                }

                return View(new Producto());
            }
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int id)
        {
            List<Producto> lstProductos = (List<Producto>)Session["productos"];
            if (lstProductos == null)
                lstProductos = GetProductos();

            for (int i = 0; i < lstProductos.Count; i++)
            {
                if (lstProductos[i].id == id)
                    return View(lstProductos[i]);
            }

            return View(new Producto());
        }

        // POST: Producto/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            List<Producto> lstProductos = (List<Producto>)Session["productos"];
            if (lstProductos == null)
                lstProductos = GetProductos();

            try
            {
                for (int i = 0; i < lstProductos.Count; i++)
                {
                    if (lstProductos[i].id == id)
                        lstProductos.RemoveAt(i);
                }

                if (collection[1] != "NoDisponible")
                {
                    string rutaFisica = Server.MapPath("~/Content/Producto/") + collection[1];
                    System.IO.File.Delete(rutaFisica);
                }

                Session["productos"] = lstProductos;

                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Producto());
            }
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

        public ActionResult GetImagen(string imagen)
        {
            if (imagen != "NoDisponible")
            {
                string rutaFisica = Server.MapPath("~/Content/Producto/")+ imagen;
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
