using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Publico.Models;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Publico.Controllers
{
    public class OrganizacionController : Controller
    {
        //
        // GET: /Organizacion/

        PortalAppServicio servicio = new PortalAppServicio();

        public ActionResult QuienesSomos(){
            return View();
        }

        public ActionResult MisionVisionValores()
        {
            return View();
        }

        public ActionResult CodigoEtica()
        {
            return View();
        }

        public ActionResult EstructuraOrganizacional()
        {
            return View();
        }

        public ActionResult Asamblea()
        {
            return View();
        }

        public ActionResult Directorio()
        {
            return View();
        }

        public ActionResult Politicas()
        {
            return View();
        }

        public ActionResult TerminosyCondiciones()
        {
            return View();
        }

        public ActionResult SalaPrensa()
        {
            HomeModel model = new HomeModel();
            model.ListaSalaPrensa = this.servicio.ListarComunicados().Where(x => x.Composition != 1 && x.Comestado == ConstantesAppServicio.Activo && x.Comtipo == "S" && x.Comfechaini <= DateTime.Now && DateTime.Now <= x.Comfechafin).OrderByDescending(x => x.Comfecha).ToList();

            string path = ConfigurationManager.AppSettings["RutaComunicados"].ToString();
            foreach (var item in model.ListaSalaPrensa)
            {
                string ruta = path + item.Comcodi + ".jpg";
                byte[] imagen = null;
                if (System.IO.File.Exists(ruta))
                {
                    imagen = System.IO.File.ReadAllBytes(path + item.Comcodi + ".jpg");
                }

                if (imagen != null)
                {
                    string mimeType = "image/" + "jpg";
                    string base64 = Convert.ToBase64String(imagen);
                    item.ComImagen = string.Format("data:{0};base64,{1}", mimeType, base64);
                }
                else item.ComImagen = null;
            }
            
            return View(model);
        }

        public ActionResult AppMovil()
        {
            return View();
        }
    }
}
