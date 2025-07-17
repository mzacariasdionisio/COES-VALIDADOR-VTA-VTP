using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.InformesOsinergmin.Helper;
using COES.MVC.Intranet.Areas.InformesOsinergmin.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.InformeOsinerming;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.InformesOsinergmin.Controllers
{
    public class IndicadoresRPFController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        ReporteMensualAppServicio servicio = new ReporteMensualAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            IndicadoresRPFModel model = new IndicadoresRPFModel();
            model.FechaInicio = DateTime.Now.AddMonths(-6).ToString(Constantes.FormatoMesAnio);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoMesAnio);

            return View(model);
        }

        /// <summary>
        /// Permite mostrar el listado
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listar(string fechaInicio, string fechaFin)
        {
            IndicadoresRPFModel model = new IndicadoresRPFModel();

            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoMesAnio,
                CultureInfo.InvariantCulture);

            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoMesAnio,
                CultureInfo.InvariantCulture);

            List<PsuRpfhidDTO> list = this.servicio.GetByCriteriaPsuRpfhids(fecInicio, fecFin);

            model.Listado = list;

            return PartialView(model);
        }

        /// <summary>
        /// Muestra la pantalla de edicion o registro 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(string fecha)
        {
            IndicadoresRPFModel model = new IndicadoresRPFModel();

            if (string.IsNullOrEmpty(fecha))
            {
                model.Entidad = new PsuRpfhidDTO();
                model.IndNuevo = Constantes.SI;
                model.Rpfhidfecha = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio);
            }
            else 
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                model.Entidad = this.servicio.GetByIdPsuRpfhid(fechaConsulta);
                model.IndNuevo = Constantes.NO;
            }

            return PartialView(model);        
        }

        /// <summary>
        /// Permite eliminar
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(string fecha)
        {
            /*
             * Este es el boton que se borro de la vista para quirtar la opcion de eliminar
             * <a href="JavaScript:eliminarRegistro('@item.Rpfhidfecha.ToString("MM yyyy")', '@item.Rpfhidfecha.ToString("MM-yyyy")');" title="Eliminar Registro"><img src="~/Content/Images/btn-cancel.png" alt=""></a>
             */
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoMesAnio,
                  CultureInfo.InvariantCulture);
                this.servicio.DeletePsuRpfhid(fechaConsulta);                
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
            
        }


        /// <summary>
        /// Permite grabar
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(IndicadoresRPFModel model)
        {
            try
            {
                PsuRpfhidDTO entity = new PsuRpfhidDTO();
                if (model.Eneindhidra != null && model.Potindhidra !=null && model.Rpfenetotal !=null && model.Rpfpotmedia !=null)
                {
                    entity.Eneindhidra = (decimal)model.Eneindhidra;
                    entity.Potindhidra = (decimal)model.Potindhidra;
                    entity.Rpfenetotal = (decimal)model.Rpfenetotal;
                    entity.Rpfpotmedia = (decimal)model.Rpfpotmedia;
                    entity.Lastuser = User.Identity.Name;
                    entity.Lastdate = DateTime.Now;
                        
                    if (model.Eneindhidra >= 0 && model.Potindhidra >= 0 && model.Rpfenetotal >= 0 && model.Rpfpotmedia >= 0)
                    {
                        int mes = Int32.Parse(model.Rpfhidfecha.Substring(0, 2));
                        int anho = Int32.Parse(model.Rpfhidfecha.Substring(3, 4));
                        DateTime fechatipo = new DateTime(anho, mes, 1);
                        //DateTime fecha = DateTime.ParseExact("01/" + mes + "/" + anho, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        entity.Rpfhidfecha = fechatipo;
                        if (model.IndNuevo == Constantes.SI)
                        {
                            if (this.servicio.GetByIdPsuRpfhid(fechatipo) == null)
                            {
                                this.servicio.SavePsuRpfhid(entity, model.IndNuevo);
                                return Json(1);
                            }
                            else
                            {
                                return Json(-2);
                            }
                        }
                        else
                        {
                            if (this.servicio.GetByIdPsuRpfhid(fechatipo) != null)
                            {
                                this.servicio.SavePsuRpfhid(entity, model.IndNuevo);
                                return Json(1);
                            }
                            else
                            {
                                return Json(-1);
                            }
                        }
                    }
                }
                else
                {
                    return Json(-3);
                }
                return Json(-1);
            }
            catch(Exception e)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Exporta
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarIndicadorRPF(string fechaInicio, string fechaFin)
        {
            int result = 1;
            try
            {
                if (fechaInicio == null || fechaFin == null)
                {
                    result = -2;
                    return Json(result);
                }
                DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                
                List<PsuRpfhidDTO> list = this.servicio.GetByCriteriaPsuRpfhids(fecInicio, fecFin);
                string path = HttpContext.Server.MapPath("~/") + ConstantesInformes.RutaReporte;

                ExcelDocument.GenerarReporteRPF(list, fecInicio, fecFin, path);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Descarga el formato
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarIndicadorRPF()
        {            
            string fullPath = HttpContext.Server.MapPath("~/") + ConstantesInformes.RutaReporte +  "InformesRPF.xlsx";
            return File(fullPath, Constantes.AppExcel, "InformesRPF.xlsx");
        }
    }
}
