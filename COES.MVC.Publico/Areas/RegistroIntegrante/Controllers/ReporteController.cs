using COES.MVC.Publico.Areas.RegistroIntegrante.Models;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using System.Web.Mvc;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System;
using System.Linq;
using log4net;
using COES.MVC.Publico.Areas.RegistroIntegrante.Helper;
using COES.MVC.Publico.Helper;
using System.Text;

namespace COES.MVC.Publico.Areas.RegistroIntegrante.Controllers
{
    public class ReporteController : Controller
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ReporteController));

        public ReporteController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ReporteController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ReporteController", ex);
                throw;
            }
        }

        /// <summary>
        /// Instancia de la clase ReportesAppServicio
        /// </summary>
        ReportesAppServicio appReporte = new ReportesAppServicio();


        #region Reporte Empresas

        /// <summary>
        /// Reporte  de empresas
        /// </summary>
        /// <returns></returns>
        public ActionResult Empresa()
        {
            RegistroIntegrantesAppServicio appRegistroIntegrante = new RegistroIntegrantesAppServicio();
            GestionSolicitudesAppServicio appSolicitud = new GestionSolicitudesAppServicio();

            ReporteModel model = new ReporteModel();

 
            model.ListaTipoSolicitudes = appSolicitud.ListarTipoSolicitud();
            model.ListaTipoSolicitudes.Add(new RiTiposolicitudDTO()
            {
                Tisocodi = ConstantesRegistroIntegrantes.TodosCodigoEntero,
                Tisonombre = ConstantesRegistroIntegrantes.Todos
            });

            model.ListaTipoEmpresa = appRegistroIntegrante.ListTipoEmpresa();
            model.ListaTipoEmpresa.Add(new SiTipoempresaDTO()
            {
                Tipoemprcodi = ConstantesRegistroIntegrantes.TodosCodigoEntero,
                Tipoemprabrev = ConstantesRegistroIntegrantes.Todos,
                Tipoemprdesc = ConstantesRegistroIntegrantes.Todos
            });


            return View(model);
        }

        /// <summary>
        /// Permite pintar la lista de registros del reporte de empresa
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>        
        /// <param name="tiposolicitud">tipo de solicitud</param>
        /// <param name="nroPagina">numero de pagina</param>
        /// <returns></returns>
        public ActionResult ListarEmpresasPublico(string tipoempresa, string tiposolicitud)
        {
            ReporteModel model = new ReporteModel();

            if (string.IsNullOrEmpty(tipoempresa)) tipoempresa = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = ConstantesRegistroIntegrantes.ParametroDefecto;

            List<SiEmpresaDTO> reporte = this.appReporte.ListarEmpresasPublico(tipoempresa, tiposolicitud);

            model.ListaIntegrantes = reporte;

            return View(model);
        }


        /// <summary>
        /// Permite generar los datos del reporte de empresas para exportar
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>       
        /// <param name="tiposolicitud">tipo solicitud</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteEmpresasPublico(string tipoempresa, string tiposolicitud)
        {
            int indicador = 1;
            try
            {
                ReporteModel model = new ReporteModel();

                if (string.IsNullOrEmpty(tipoempresa)) tipoempresa = ConstantesRegistroIntegrantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = ConstantesRegistroIntegrantes.ParametroDefecto;

                List<SiEmpresaDTO> reporte = this.appReporte.ListarEmpresasPublico(tipoempresa, tiposolicitud);

                model.ListaIntegrantes = reporte;


                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
                string titulo = "REPORTE DE EMPRESAS";
                ExcelDocumentEmpresasPublicas.GernerarArchivoEnvios(reporte, ruta, titulo);
                indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                indicador = -1;
            }
            return Json(indicador);
        }


        /// <summary>
        /// Permite exportar los datos del reporte de empresas
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarExcelEmpresasPublico()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = "ReporteEmpresas.Xlsx";
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Vista de datos históricos
        /// </summary>
        /// <returns></returns>
        public ActionResult Historico()
        {
            ReporteModel model = new ReporteModel();
            List<int> anios = new List<int>();
            int nroAnio = DateTime.Now.Year;

            for (int i = nroAnio; i >= 2010; i--) anios.Add(i);
            model.ListaAnios = anios;

            return View(model);

        }

        /// <summary>
        /// Vista para visualizar datos históricos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListHistorico(int panio, string ptipo)
        {
            List<RiHistoricoDTO> entitys = (new HistoricoAppServicio()).GetByCriteriaRiHistoricos(panio, ptipo);

            StringBuilder str = new StringBuilder();
            str.Append("<table border='0' class='tabla-icono table table-hover' cellspacing='0' cellpadding='0' width='100%' id='tabla'>");
            str.Append("    <thead>");
            str.Append("        <tr> ");
            str.Append("            <th>Año</th>");
            str.Append("            <th>Tipo</th>");
            str.Append("            <th style='width:550px; '>Descripción</th>");
            str.Append("            <th>Fecha</th>");
            str.Append("        </tr>");
            str.Append("    </thead>");
            str.Append("    <tbody>");

            List<int> anios = entitys.Select(x => (int)x.Hisrianio).Distinct().ToList();
            bool flagcss = false;
            string classcss = string.Empty;
            foreach (int anio in anios)
            {
                List<string> tipos = entitys.Where(x => x.Hisrianio == anio).Select(x => x.Hisritipo).Distinct().ToList();
                int anioSpan = entitys.Where(x => x.Hisrianio == anio).Count();

                bool flagAnio = true;
                classcss = string.Empty;
                if (flagcss)
                {                    
                    classcss = "#f2f5f7"; 
                }
                

                foreach (string tipo in tipos)
                {
                    List<RiHistoricoDTO> items = entitys.Where(x => x.Hisrianio == anio && x.Hisritipo == tipo).Distinct().ToList();

                    bool flagTipo = true;                  

                    foreach (RiHistoricoDTO item in items)
                    {
                        str.Append("        <tr style='vertical-align: middle;'>");

                        if (flagAnio) { 
                            if (classcss == "")
                            {
                                str.AppendFormat("            <td rowspan='{1}' style= 'background-color:{2}; text-align:center'>{0}</td>", anio, anioSpan, classcss);
                            }
                            else
                            {
                                str.AppendFormat("            <td rowspan='{1}' class='pintado' style= 'background-color:{2}; text-align:center'>{0}</td>", anio, anioSpan, classcss);
                            }
                        }

                        if (flagTipo)
                        {
                            if (classcss == "")
                            {
                                str.AppendFormat("            <td rowspan='{1}' style= 'background-color:{2}; text-align:center'>{0}</td>", tipo, items.Count(), classcss);
                            }
                            else
                            {
                                str.AppendFormat("            <td rowspan='{1}' class='pintado'  style= 'background-color:{2}; text-align:center'>{0}</td>", tipo, items.Count(), classcss);
                            }
                        }

                        if (classcss == "")
                        {
                            str.AppendFormat("            <td style= 'background-color:{1}'>{0}</td>", item.Hisridesc, classcss);
                            str.AppendFormat("            <td style= 'background-color:{1}; text-align:center'>{0}</td>", ((DateTime)item.Hisrifecha).ToString(Constantes.FormatoFecha), classcss);
                        }
                        else
                        {
                            str.AppendFormat("            <td class='pintado' style= 'background-color:{1}'>{0}</td>", item.Hisridesc, classcss);
                            str.AppendFormat("            <td class='pintado' style= 'background-color:{1}; text-align:center'>{0}</td>", ((DateTime)item.Hisrifecha).ToString(Constantes.FormatoFecha), classcss);
                        }



                        str.Append("        </tr>");

                        flagAnio = false;
                        flagTipo = false;
                    }
                }

                flagcss = (flagcss) ? false : true;
            }

            str.Append("    </tbody>");
            str.Append("<table>");

            ViewBag.Datos = str.ToString();

            return PartialView();
        }


        #endregion


    }
}
