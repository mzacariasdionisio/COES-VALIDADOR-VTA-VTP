using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.InformesOsinergmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.InformeOsinerming;
using System.Globalization;
using COES.MVC.Intranet.Areas.InformesOsinergmin.Helper;

namespace COES.MVC.Intranet.Areas.InformesOsinergmin.Controllers
{
    public class CostoMarginalRealController : Controller
    {
        //
        // GET: /InformesOsinergmin/CostoMarginaReal/
        CostoMarginalRealAppServicio servicio = new CostoMarginalRealAppServicio();

        public ActionResult Index()
        {
            CostoMarginalRealModel model = new CostoMarginalRealModel();
            model.mesInforme = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio);
            string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
            string fileName = path + "cmgdtr" + model.mesInforme.Remove(2, 3) + ".xlsx";
            if (System.IO.File.Exists(fileName))
            {
                model.nombreArchivoExcel = "cmgdtr" + model.mesInforme.Remove(2, 3) + ".xlsx";
            }
            return View(model);
        }

        /// <summary>
        /// Permite subir al servidor el nuevo archivo excel asignándole el nombre del mes
        /// </summary>
        /// <param name="mesInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(string mesInforme)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    //string fileName = path + Constantes.ArchivoPotencia;
                    string fileName = path + "cmgdtr" + mesInforme.Remove(2, 3) + ".xlsx";
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite cargar los datos del CMR desde el excel a la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DatosCMR(string mesInforme)
        {
            try
            {
                /*string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.ArchivoPotencia;
                List<ServicioRpfDTO> list = (new RpfHelper()).LeerDesdeFormato(path);
                this.ListaPotencia = list;*/
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                string fileName = path + "cmgdtr" + mesInforme.Remove(2, 3) + ".xlsx";
                
                if (System.IO.File.Exists(fileName))
                {
                    DateTime fecha = DateTime.ParseExact(mesInforme, Constantes.FormatoMesAnio,
                        CultureInfo.InvariantCulture);
                    List<string> validacion = ExcelDocument.validacion(fileName, fecha);
                    if (validacion.Count == 0)
                    {
                        CostoMarginalRealModel model = new CostoMarginalRealModel();

                        /// Proceso de carga para indicador 5.10
                        model.ListadoCMRDiario = ExcelDocument.ObtenerCMRDiario(fileName);
                        foreach (var item in model.ListadoCMRDiario)
                        {
                            item.Lastdate = DateTime.Now;
                            item.Lastuser = User.Identity.Name;
                            if (this.servicio.GetByIdPsuDesvcmgsnc(item.Desvfecha) == null)
                            {                                
                                this.servicio.SavePsuDesvcmgsnc(item);
                            }
                            else
                            {
                                this.servicio.UpdatePsuDesvcmgsnc(item);
                            }
                        }

                        /// Proceso de carga para indicador 5.2
                        model.CMRMensual = ExcelDocument.ObtenerCMRMensual(fileName);
                        model.CMRMensual.Lastdate = DateTime.Now;
                        model.CMRMensual.Lastuser = User.Identity.Name;

                        if (this.servicio.GetByIdPsuDesvcmg(model.CMRMensual.Desvfecha) == null)
                        {
                            
                            this.servicio.SavePsuDesvcmg(model.CMRMensual);
                        }
                        else
                        {
                            
                            this.servicio.UpdatePsuDesvcmg(model.CMRMensual);
                        }
                    }
                    else
                    {
                        return Json(ExcelDocument.ObtieneListaValidacion(validacion));
                    }
                }
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public PartialViewResult CargaCMRMensual(string mesInforme)
        {
            CostoMarginalRealModel model = new CostoMarginalRealModel();
            model.mesInforme = mesInforme;
            DateTime fecha = DateTime.ParseExact(mesInforme, Constantes.FormatoMesAnio,
                CultureInfo.InvariantCulture);
            model.CMRMensual = servicio.GetByIdPsuDesvcmg(fecha);

            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult CargaCMRDiario(string mesInforme)
        {
            CostoMarginalRealModel model = new CostoMarginalRealModel();
            model.mesInforme = mesInforme;
            DateTime fechaMes = DateTime.ParseExact(mesInforme, Constantes.FormatoMesAnio,
                CultureInfo.InvariantCulture);
            model.ListadoCMRDiario = servicio.GetByCriteriaPsuDesvcmgsncs(fechaMes);
            return PartialView(model);
        }
    }
}
