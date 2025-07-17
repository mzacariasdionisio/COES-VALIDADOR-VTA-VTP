using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.MercadoMayorista.Helper;
using COES.MVC.Publico.Areas.MercadoMayorista.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.MercadoMayorista.Controllers
{
    public class CostosMarginalesController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CostosMarginalesController));
        /// <summary>
        /// Creación de la instancia del servicio correspondiente
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();

        /// <summary>
        /// Pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CostoMarginalModel model = new CostoMarginalModel();
            DateTime fecha = DateTime.Now;
            model.FechaConsulta = fecha.ToString(Constantes.FormatoFechaISO);//DateTime.Now.ToString(Constantes.FormatoFechaISO);
            model.PathPrincipal = this.servicio.GetPathPrincipal();
            List<CmParametroDTO> colores = this.servicio.ListCmParametros();
            model.ListaColores = colores;
            model.FechaInicio = fecha.ToString(Constantes.FormatoFechaISO);
            model.FechaFin = fecha.ToString(Constantes.FormatoFechaISO);
            return View(model);
        }

        /// <summary>
        /// Muestra la página de resultados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaProceso(string fecha)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            DateTime fechaVigenciaPR07 = DateTime.ParseExact(
                ConfigurationManager.AppSettings[ConstantesCortoPlazo.FechaVigenciaPR07], Constantes.FormatoFecha,
                CultureInfo.InvariantCulture);

            int version = ConstantesCortoPlazo.VersionCMOriginal;
            if (fechaConsulta.Subtract(fechaVigenciaPR07).TotalDays >= 0) version = ConstantesCortoPlazo.VersionCMPR07;


            List<CmCostomarginalDTO> result = this.servicio.ObtenerCorridasPorFecha(fechaConsulta, version);
            return Json(result.OrderByDescending(x => x.Indicador).ToList());
        }

        /// <summary>
        /// Permite mostrar el mapa de coordenadas
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>        
        [HttpPost]
        public PartialViewResult Mapa(int correlativo, string defecto)
        {
            CostoMarginalModel model = new CostoMarginalModel();

            model.ListaCoordenada = this.ObtenerEstructuraMapa(correlativo, string.Empty, defecto);

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el mapa de coordenadas
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>        
        [HttpPost]
        public PartialViewResult MapaFiltro(int correlativo, string colores, string defecto)
        {
            CostoMarginalModel model = new CostoMarginalModel();
            model.ListaCoordenada = this.ObtenerEstructuraMapa(correlativo, colores, defecto);
            return PartialView("Mapa", model);
        }

        /// <summary>
        /// Permite obtener la estructura del mapa
        /// </summary>
        /// <param name="correlativo"></param>
        /// <param name="colores"></param>
        /// <returns></returns>
        private string ObtenerEstructuraMapa(int correlativo, string colores, string defecto)
        {
            string resultado = string.Empty;

            List<CmCostomarginalDTO> listOriginal = this.servicio.ObtenerCostosMarginalesMapa(correlativo);
            List<CmCostomarginalDTO> list = new List<CmCostomarginalDTO>();
            if (defecto == Constantes.SI)
            {
                list = listOriginal.Where(x => x.IndDefecto == Constantes.SI).ToList();
            }
            else
            {
                list = listOriginal;
            }

            List<CmParametroDTO> listColores = this.servicio.ListCmParametros();
            List<CmParametroDTO> listaColores = new List<CmParametroDTO>();

            if (!string.IsNullOrEmpty(colores))
            {
                colores = colores.Remove(colores.Length - 1);
                List<int> ids = colores.Split(',').Select(int.Parse).ToList();
                listaColores = listColores.Where(x => ids.Any(y => x.Cmparcodi == y)).ToList();
            }
            else
            {
                listaColores = listColores;
            }

            List<CmCostomarginalDTO> listCM = this.FiltrarPorRango(list, listaColores);

            if (list.Count > 0)
            {
                string str = string.Empty;
                int index = 0;
                foreach (CmCostomarginalDTO item in listCM)
                {
                    string descripcion = string.Format(
                    @"<div class='popup-title'><div class='leyenda-titulo-color' style='background-color:{4}'></div><div class='leyenda-titulo-texto'><span>Barra: {0}</span></div></div><div class='content-registro' style='margin-top:5px'><table style='width:100%'><tr><td class='registro-label'>Energía</td><td class='registro-control'>{1}</td></tr><tr><td class='registro-label'>Congestión</td><td class='registro-control'>{2}</td></tr><tr><td class='registro-label'>Total</td><td class='registro-control'>{3}</td></tr></table></div>",
                    item.Cnfbarnombre, (item.Cmgnenergia != null) ? "S/. / MWh " + ((decimal)item.Cmgnenergia).ToString("#,###.00") : "",
                                          (item.Cmgncongestion != null) ? "S/. / MWh " + ((decimal)item.Cmgncongestion).ToString("#,###.00") : "",
                                          (item.Cmgntotal != null) ? "S/. / MWh " + ((decimal)item.Cmgntotal).ToString("#,###.00") : "", item.Color
                    );

                    if (index < list.Count - 1)
                        str += string.Format(@"[ coorx: {0}, coory: {1}, descripcion: &{2}&, color: &{3}&],", item.Cnfbarcoorx, item.Cnfbarcoory, descripcion, item.Color);
                    else
                        str += string.Format(@"[ coorx: {0}, coory: {1}, descripcion: &{2}&, color: &{3}&]", item.Cnfbarcoorx, item.Cnfbarcoory, descripcion, item.Color);

                    index++;
                }

                resultado = "[" + str.Replace('&', '\"').Replace('[', '{').Replace(']', '}') + "]";
            }
            else
            {
                resultado = "[]";
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener el valor
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> FiltrarPorRango(List<CmCostomarginalDTO> list, List<CmParametroDTO> colores)
        {
            foreach (CmCostomarginalDTO item in list)
            {
                decimal total = (decimal)item.Cmgntotal;
                foreach (CmParametroDTO itemColor in colores)
                {
                    if (total >= itemColor.Cmparinferior && total < itemColor.Cmparsuperior)
                    {
                        item.Color = itemColor.Cmparvalor;
                        break;
                    }
                }
            }

            return list.Where(x => !string.IsNullOrEmpty(x.Color)).ToList();
        }

        /// <summary>
        /// Muestra los datos de una corrida
        /// </summary>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int correlativo, string defecto)
        {
            CostoMarginalModel model = new CostoMarginalModel();
            List<CmCostomarginalDTO> listOriginal = this.servicio.ObtenerCostosMarginalesMapa(correlativo);
            List<CmCostomarginalDTO> list = new List<CmCostomarginalDTO>();
            if (defecto == Constantes.SI)
            {
                list = listOriginal.Where(x => x.IndDefecto == Constantes.SI).ToList();
            }
            else
            {
                list = listOriginal;
            }

            model.Listado = list;

            return PartialView(model);
        }


        /// <summary>
        /// Permite mostrar los archivos relacionado la corrida
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Folder(string fecha, int correlativo)
        {
            CostoMarginalModel model = new CostoMarginalModel();
            DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            string path = this.servicio.GetPathCorrida(fechaProceso, correlativo);
            model.PathResultado = path;
            return Json(model);
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fecha, int correlativo, string defecto)
        {
            try
            {
                CostoMarginalModel model = new CostoMarginalModel();
                List<CmCostomarginalDTO> listOriginal = this.servicio.ObtenerCostosMarginalesMapa(correlativo);
                List<CmCostomarginalDTO> list = new List<CmCostomarginalDTO>();
                if (defecto == Constantes.SI)
                {
                    list = listOriginal.Where(x => x.IndDefecto == Constantes.SI).ToList();
                }
                else
                {
                    list = listOriginal;
                }

                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.ReporteTransferencias;
                string file = NombreArchivo.ReporteCostoMarginalNodal;

                OperacionHelper.GenerarReporteCostosMarginales(list, path, file);

                return Json(1);
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string file = NombreArchivo.ReporteCostoMarginalNodal;
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.ReporteTransferencias + file;
            return File(fullPath, Constantes.AppExcel, file);
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarMasivo(string fechaInicio, string fechaFin, string defecto)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicio))
                {
                    fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFin))
                {
                    fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                string inddefecto = (string.IsNullOrEmpty(defecto)) ? "-1" : defecto;
                List<CmCostomarginalDTO> listado = this.servicio.ObtenerReporteCostosMarginalesWeb(fecInicio, fecFin, inddefecto);

                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.ReporteTransferencias;
                string file = NombreArchivo.ReporteCostoMarginalNodal;

                OperacionHelper.GenerarReporteMasivoCM(listado, path, file);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarMasivo()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.ReporteTransferencias + NombreArchivo.ReporteCostoMarginalNodal;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteCostoMarginalNodal);
        }

        /// <summary>
        /// Vista de revisados
        /// </summary>
        /// <returns></returns>
        public ActionResult Revisados()
        {
            return View();
        }
    }
}
