using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.MVC.Intranet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Globalization;
using COES.Servicios.Aplicacion.General;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class EnvioController : Controller
    {
        //
        // GET: /Hidrologia/Envio/
        HidrologiaAppServicio logic = new HidrologiaAppServicio();
        private GeneralAppServicio logicGeneral;

        public EnvioController()
        {

            logicGeneral = new GeneralAppServicio();
        }
        /// <summary>
        /// Index de inicio de controller Envio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();                   
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            //model.ListaFormatoEnvio = Tools.ObtenerListaTipoInfo();
            //model.ListaFrecuencia = Tools.ObtenerListaFrecuencia();
            
            model.ListaFormato = this.logic.ListMeFormatos().Where(x => x.Modcodi == ConstantesHidrologia.IdModulo).ToList(); //lista de todos los formatos para hidrologia
            model.ListaLectura = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesHidrologia.IdOrigenHidro).ToList();
            model.ListaEstadoEnvio = this.logic.ListMeEstadoenvios();
            List<int> listaFormatCodi = new List<int>();
            List<int> listaFormatPeriodo = new List<int>();
            foreach (var reg in model.ListaFormato)
            {
                listaFormatCodi.Add(reg.Formatcodi);
                listaFormatPeriodo.Add((int)reg.Formatperiodo);
            }
            model.StrFormatCodi = String.Join(",", listaFormatCodi);
            model.StrFormatPeriodo = String.Join(",", listaFormatPeriodo);

            return View(model);
        }
   
        /// <summary>
        /// Devuelve vista parcial para mostrar listado de envío
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsEstado"></param>
        /// <param name="nPaginas"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string idsEmpresa, string fechaIni, string fechaFin, string idsFormato, string idsLectura, string idsEstado, int nPaginas)
        {
            HidrologiaModel model = new HidrologiaModel();
            DateTime fechaInicial = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicial = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            var lista = this.logic.GetListaMultipleMeEnvios(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaInicial, fechaFinal, nPaginas, Constantes.PageSize);
            model.ListaEnvio = lista;
            return PartialView(model);
        }

        /// <summary>
        /// Lista los formatos de acuerdo a la lectura seleccionada
        /// </summary>
        /// <param name="idLectura"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarFormatosXLectura(string sLectura, string sEmpresa)
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaFormato = this.logic.GetByModuloLecturaMeFormatosMultiple(ConstantesHidrologia.IdModulo, sLectura, sEmpresa);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string idsEmpresa, string fechaIni, string fechaFin, string idsFormato, string idsLectura, string idsEstado)
        {
            HidrologiaModel model = new HidrologiaModel();
            model.IndicadorPagina = false;
            //var formato = logic.GetByIdMeFormato(idTipoInformacion);
            //formato.ListaHoja = logic.GetByCriteriaMeFormatohojas(idTipoInformacion);
            DateTime fechaInicial = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicial = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            int nroRegistros = this.logic.TotalListaMultipleMeEnvios(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaInicial, fechaFinal);

            if (nroRegistros > 0)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            
            return PartialView(model);
        }

        // exporta el reporte general consultado a archivo excel
        [HttpPost]
        public JsonResult GenerarArchivoReporteXLS(string idsEmpresa, string fechaIni, string fechaFin, string idsFormato, string idsLectura, string idsEstado)
        {
            int indicador = 1;

            HidrologiaModel model = new HidrologiaModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            DateTime fechaInicial = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            try
            {
                if (fechaIni != null)
                {
                    fechaInicial = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFin != null)
                {
                    fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                var lista = this.logic.GetListaMultipleMeEnviosXLS(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaInicial, fechaFinal);
                model.ListaEnvio = lista;
                ExcelDocument.GenerarArchivosEnviados(model,ruta);

                indicador = 1;

            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreReporteArchivosEnviados;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
        /// <summary>
        /// Genera Archivo del envio solicitado
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoEnvio(int idEnvio)
        {
            int indicador = 1;

            HidrologiaModel model = new HidrologiaModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            try
            {
                indicador = this.logic.GenerarArchivoEnvio(idEnvio, ruta);
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);        
        }

        /// <summary>
        /// Permite exportar el reporte general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarEnvio()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreArchivoEnvio;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
    }
}
