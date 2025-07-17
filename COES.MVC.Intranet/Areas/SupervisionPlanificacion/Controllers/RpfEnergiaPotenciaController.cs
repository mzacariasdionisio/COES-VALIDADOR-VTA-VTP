using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.SupervisionPlanificacion.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.HidraulicoRPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SupervisionPlanificacion.Controllers
{
    public class RpfEnergiaPotenciaController : Controller
    {
        //
        // GET: /RPF/

        public ActionResult Index()
        {
            RpfEnergiaPotenciaModel model = new RpfEnergiaPotenciaModel();

            model.RpfhidfechaIni = DateTime.Now.AddMonths(-1);
            model.RpfhidfechaFin = DateTime.Now.AddMonths(1);

            return View(model);
        }

        //
        // GET: /RPF/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /RPF/Create

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Grilla(String fechaini, String fechafin)
        {
            RpfEnergiaPotenciaModel model = new RpfEnergiaPotenciaModel();

            model.RpfhidfechaIni = DateTime.ParseExact("01/" + fechaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.RpfhidfechaFin = DateTime.ParseExact("01/" + fechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListaRPF = (new RpfEnergiaPotenciaAppServicio()).ListRPF(model.RpfhidfechaIni, model.RpfhidfechaFin);

            return PartialView(model);

        }

        //
        // POST: /RPF/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RPF/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /RPF/Edit/5

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
        // GET: /RPF/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /RPF/Delete/5

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




        public ActionResult Detalle(String fecha)
        {

            RpfEnergiaPotenciaModel model = new RpfEnergiaPotenciaModel();
            string codigo = Request[RequestParameter.EventoId];
            
            if (codigo == "undefined")
            {
                model = null;
            }
            else
            {
                model.fecha = DateTime.ParseExact(codigo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model.Entidad = (new RpfEnergiaPotenciaAppServicio()).GetByDateRPF(model.fecha);
            }

            return View(model);
        }


        [HttpPost]

        public JsonResult Grabar(String fecha, decimal rpftotal, decimal rpfmedia, decimal eneind, decimal potind, String parametr)
        {
            try
            {
                RpfEnergiaPotenciaModel model = new RpfEnergiaPotenciaModel();
                RpfEnergiaPotenciaDTO DTOmodel = new RpfEnergiaPotenciaDTO();

                DTOmodel.Rpfenetotal = rpftotal;
                DTOmodel.Rpfpotmedia = rpfmedia;
                DTOmodel.Eneindhidra = eneind;
                DTOmodel.Potindhidra = potind;
                DTOmodel.Rpfhidfecha = DateTime.ParseExact("01/" + fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DTOmodel.Lastuser = User.Identity.Name;
                DTOmodel.Lastdate = DateTime.Now;

                  if ((new RpfEnergiaPotenciaAppServicio()).GetByDateRPF(DTOmodel.Rpfhidfecha) == null)
                    {
                        if (parametr == "new")
                        {
                            model.NroRPF = (new RpfEnergiaPotenciaAppServicio()).SaveRPF(DTOmodel);
                        }
                        else
                        { return Json(2); }
                    }
                    else
                    {
                        
                        if (parametr == "update")
                        {
                            model.NroRPF = (new RpfEnergiaPotenciaAppServicio()).UpdateRPF(DTOmodel);
                        }
                        else
                        { return Json(3); }
                    }
                

                return Json(1);
            }
            catch (Exception e)
            {
                return Json(-1);
            }
        }



        [HttpPost]

        public JsonResult Eliminar(String fecha)
        {
            try
            {
                RpfEnergiaPotenciaModel model = new RpfEnergiaPotenciaModel();
                model.Entidad = (new RpfEnergiaPotenciaAppServicio()).GetByDateRPF(DateTime.ParseExact("01/" + fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture));
               
                if (fecha == null || model.Entidad == null)
                {
                    return Json(-1);
                }
                else
                {
                    (new RpfEnergiaPotenciaAppServicio()).DeleteRPF(DateTime.ParseExact("01/" + fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture));
                    return Json(1);
                }
            }
            catch (Exception e)
            {
                return Json(-1);
            }
        }

    }
}
