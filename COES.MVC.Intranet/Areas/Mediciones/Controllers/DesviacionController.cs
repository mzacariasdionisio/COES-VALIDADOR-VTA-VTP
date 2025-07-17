using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Mediciones.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Mediciones.Controllers
{    
    public class DesviacionController : Controller
    {
        private DateTime fechaExcel;
        private static List<DesviacionDTO> list;
        //
        // GET: /Desviacion/

        public ActionResult Index()
        {
            
            DesviacionModel model = new DesviacionModel();
            model.fecha = DateTime.Now;
            list = null;

            return View(model);
        }

        //
        // GET: /Desviacion/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Desviacion/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Desviacion/Create

        [HttpPost]
        public ActionResult Grilla(String fecha, String control)
        {
            DesviacionModel model = new DesviacionModel();
            string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.ArchivoDesviacion;
            if (list==null)
            {
                model.ListaDesviaciones = null;
            }
            else
            {
                model.ListaDesviaciones = list;
            }
            model.fecha = DateTime.ParseExact(fecha.Substring(0, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            
            return PartialView(model);
        }


        //
        // GET: /Desviacion/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Desviacion/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Desviacion/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Desviacion/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        

        /// <summary>
        /// Permite cargar un archivo al servidor
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(int? chunk, string name)
        {
            try
            {
                
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = file.FileName;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    

                    file.SaveAs(path + Constantes.ArchivoDesviacion);
                    
                }

                
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Permite procesar el archivo cargado en un directorio
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult ProcesarArchivo(String fecha)
        {
            
            DesviacionModel model = new DesviacionModel();
            try
            {
                
                //model.ListaDesviaciones = null;
                //string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.ArchivoDesviacion;
                //list = (new MedicionHelper()).LeerDesdeFormato(path);

                ////tratamos el archivo cargado en el directorio
                //model.ListaDesviaciones = list;

                //if (model.ListaDesviaciones != null)
                //{
                //    DesviacionDTO entity = new DesviacionDTO();
                //    entity.Desvfecha = DateTime.ParseExact(fecha.Substring(0, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //    (new DesviacionAppServicio()).DeleteDesviacion((DesviacionDTO)entity);

                //    for (int j = 0; j < (model.ListaDesviaciones).Count; j++)
                //    {
                //        fechaExcel = DateTime.ParseExact(fecha.Substring(0, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //        model.ListaDesviaciones[j].Desvfecha = fechaExcel;
                //        model.ListaDesviaciones[j].Lastdate = DateTime.Now;
                //        model.ListaDesviaciones[j].Lastuser = User.Identity.Name;
                //        model.fecha = fechaExcel;
                //        model.NroDesviaciones = (new DesviacionAppServicio()).SaveDesviacion((model.ListaDesviaciones)[j]);
                        
                //    }
                    
                //}
                //else
                //{
                    
                //    return Json(-1);

                //}

                
                return Json(1);
            }
            catch
            {
                list = null;
                return Json(-1);
            }
        }


    }
}
