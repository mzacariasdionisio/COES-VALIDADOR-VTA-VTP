using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Models;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Dominio.DTO.Sic;
using System.Configuration;
using COES.MVC.Intranet.Helper;
using log4net;
using COES.MVC.Intranet.Controllers;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Controllers
{
    public class ConfiguracionEstimadorController : Controller
    {
        PronosticoDemandaAppServicio servicio = new PronosticoDemandaAppServicio();
        /// <summary>
        /// Inicia el módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Inicia la opción de configuración por fórmulas
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PorFormulas()
        {
            ConfiguracionEstimadorModel model = new ConfiguracionEstimadorModel
            {
                FechaIni = DateTime.Now.ToString(ConstantesProdem.FormatoFecha),
                FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha),
                ListFormulasEstimador = this.servicio.ListaPerfilRuleForEstimador(),
                Mensaje = "Permite configurar ciertos parámetros para las fórmulas...",
                TipoMensaje = ConstantesProdem.MsgInfo
            };

            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de configuración de asociaciones
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PorAsociacion()
        {
            ConfiguracionEstimadorModel model = new ConfiguracionEstimadorModel
            {
                Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha)
            };
            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de consulta de aportes
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PorAportes()
        {
            ConfiguracionEstimadorModel model = new ConfiguracionEstimadorModel {
                ListRegistros = this.servicio.ListaRegistros(),
                ListRegistrosBarras = this.servicio.ListaRelacionTnaBarras(-1),
                Mensaje = "Permite consultar mediciones de los registros creados...",
                TipoMensaje = ConstantesProdem.MsgInfo
            };

            return PartialView(model);
        }

        /// <summary>
        /// Lista el detalle de la tabla segun filtros
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="dataFiltros"></param>
        /// <returns></returns>
        public JsonResult ParametrosList(int start, int length, ParametrosModel dataFiltros)
        {
            object res = this.servicio.ListConfiguracionFormulaByFiltros(start, length, dataFiltros.FechaIni,dataFiltros.FechaFin, dataFiltros.SelFormula);
            return Json(res);
        }

        /// <summary>
        /// Registra los parametros seleccionados
        /// </summary>
        /// <param name="idModulo"></param>
        /// <param name="dataFiltros"></param>
        /// <param name="dataParametros"></param>
        /// <param name="dataMedicion"></param>
        /// <returns></returns>
        public JsonResult ParametrosSave(ParametrosModel dataFiltros, PrnConfiguracionFormulaDTO dataParametros, PrnMedicion48DTO dataMedicion)
        {
            ParametrosModel model = new ParametrosModel();
            DateTime dDesde = DateTime.ParseExact(dataFiltros.FechaIni, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dHasta = DateTime.ParseExact(dataFiltros.FechaFin, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            object resMesssage = this.servicio.ParametrosFormulasSave(dDesde, dHasta, dataParametros, dataFiltros.SelFormula, User.Identity.Name);
            return Json(resMesssage);
        }

        /// <summary>
        /// Obtiene los parámetros por defecto
        /// </summary>
        /// <returns></returns>
        public JsonResult ParametrosGetDefecto()
        {
            int idDefecto = ConstantesProdem.DefectoByPunto;
            PrnConfiguracionFormulaDTO entity = this.servicio.ParametrosFormulasGetDefecto(idDefecto);
            return Json(entity);
        }

        /// <summary>
        /// Actualiza los parámetros por defecto
        /// </summary>
        /// <param name="dataParametros"></param>
        /// <returns></returns>
        public JsonResult ParametrosUpdateDefecto(PrnConfiguracionFormulaDTO dataParametros)
        {
            int idDefecto = ConstantesProdem.DefectoByPunto;
            object resMesssage = this.servicio.ParametrosFormulasUpdateDefecto(idDefecto, dataParametros, User.Identity.Name);
            return Json(resMesssage);
        }

        /// <summary>
        /// Obtiene los datos de la interfaz
        /// </summary>
        /// <param name="idRegistro"></param>
        /// <param name="idBarra"></param>
        /// <returns></returns>
        public JsonResult ObtenerMedicionesFactorAporte(int idRegistro, List<int> idBarra)
        {
            string fechaRegistro = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            DateTime parseFecha = DateTime.ParseExact(fechaRegistro,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            object res = this.servicio.ConfiguracionEstimadorDatos(idRegistro, idBarra, parseFecha);
            return Json(res);
        }

        /// <summary>
        /// Actualiza los datos mostrados de una medición
        /// </summary>
        /// <param name="idRegistro"></param>
        /// <param name="idBarra"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ActualizarMedicion(int idRegistro, List<int> idBarra, string fecha)
        {
            DateTime sFecha = DateTime.ParseExact(fecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            List<PrnMediciongrpDTO> mediciones = this.servicio.ObtenerMedicionesFactorAporte(idRegistro, idBarra, sFecha);      
            List<ParametrosModel> modelos = new List<ParametrosModel>();
            ParametrosModel modelo = new ParametrosModel();
            foreach (var item in mediciones)
            {
                modelo = new ParametrosModel();
                modelo.dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, item);
                modelo.barraNombre = item.Gruponomb;
                modelos.Add(modelo);
            }
            
            return Json(modelos);
        }

        /// <summary>
        /// Actualiza los parámetros por defecto
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public JsonResult FiltraListaBarras(int codigo)
        {
            List<PrnRelacionTnaDTO> lista = this.servicio.ListaRelacionTnaBarras(codigo);
            return Json(lista);
        }

        #region Métodos del submódulo de Relación TNA

        /// <summary>
        /// Actualiza la lista de relaciones segun tipo
        /// </summary>
        /// <param name="idTipo">Identificador del tipo de relación</param>
        /// <returns></returns>
        public JsonResult FiltrarRelaciones(string idTipo)
        {
            ConfiguracionEstimadorModel model = new ConfiguracionEstimadorModel();
            model.ListRelacion = this.servicio.ListaRelacionTna();
            if (idTipo == "radial")
                model.ListRelacion = model.ListRelacion
                    .Where(x => x.Detalle.Count() == 1)
                    .ToList();
            if (idTipo == "anillo")
                model.ListRelacion = model.ListRelacion
                    .Where(x => x.Detalle.Count() > 1)
                    .ToList();
            return Json(model.ListRelacion);
        }

        /// <summary>
        /// Devuelve el modelo de datos para la presentación
        /// </summary>
        /// <param name="idRelacion">Identificador de relación</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public JsonResult ObtenerDatosRelacion(int idRelacion, 
            string regFecha)
        {
            object res = this.servicio
                .CfgEstimadorRelacionDatos(idRelacion, regFecha);
            return Json(res);
        }

        /// <summary>
        /// Actualiza la medición historica segun la fecha parametro
        /// </summary>
        /// <param name="idRelacion">Identificador de relación</param>
        /// <param name="fecha">Fecha del registro</param>
        /// <returns></returns>
        public JsonResult ActualizarMedicionRelacion(int idRelacion, string fecha)
        {
            decimal[] res = new decimal[ConstantesProdem.Itv30min];
            if (idRelacion == -1) return Json(res);   
            
            ConfiguracionEstimadorModel model = new ConfiguracionEstimadorModel();
            DateTime parseFecha = DateTime.ParseExact(fecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListRelacion = this.servicio.ListaRelacionTna();
            PrnRelacionTnaDTO entity = model.ListRelacion
                .FirstOrDefault(x => x.Reltnacodi == idRelacion)
                ?? new PrnRelacionTnaDTO();

            res = this.servicio.ObtenerMedicionesCalculadas(entity.Reltnaformula, parseFecha);
            return Json(res);
        }

        /// <summary>
        /// Genera las listas y objetos para el popup de asociación
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerarRelacion()
        {
            ConfiguracionEstimadorModel model = new ConfiguracionEstimadorModel();
            model.ListFormulasEstimador = this.servicio.ListaPerfilRuleForEstimador();
            //model.ListFormulasTna = this.servicio.ListFormulasByUsuario();
            model.ListBarrasCP = this.servicio.GetListBarrasCP(ConstantesProdem.Prcatecodi);
            model.ListRelacion = this.servicio.ListaRelacionTna();            
            return Json(model);
        }

        /// <summary>
        /// Registra las relaciones entre barras y fórmulas
        /// </summary>
        /// <param name="dataRelaciones">Lista de entidades para el registro o actualización</param>
        /// <returns></returns>
        public JsonResult RegistrarRelacion(List<PrnRelacionTnaDTO> dataRelaciones)
        {
            string res = this.servicio.CfgEstimadorRegistrarRelacion(dataRelaciones);
            return Json(res);
        }

        /// <summary>
        /// Registra el patrón defecto para la fecha seleccionada
        /// </summary>
        /// <param name="idRelacion">Identificador de la relación TNA</param>
        /// <param name="regFecha">Fecha de referencia para el registro</param>
        /// <param name="datosMedicion">Valores por intervalos del perfil creado</param>
        /// <returns></returns>
        public JsonResult RegistrarPerfilDefecto(int idRelacion,
            string regFecha,
            decimal[] datosMedicion)
        {
            object res = this.servicio
                .CfgEstimadorRelacionRegistrarDefecto(idRelacion,
                regFecha,
                datosMedicion);
            return Json(res);
        }

        #endregion

        #region Métodos del submodulo de Configuración días

        public PartialViewResult PorConfiguracionDias()
        {
            ConfiguracionEstimadorModel model = new ConfiguracionEstimadorModel
            {
                FechaIni = DateTime.Now.ToString(ConstantesProdem.FormatoFecha),
                FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha),                
                Mensaje = "Permite configurar días feriados, atípicos o veda para el pronóstico por barras",
                TipoMensaje = ConstantesProdem.MsgInfo
            };

            return PartialView(model);
        }

        /// <summary>
        /// Lista de datos para la tabla de configuración de días
        /// </summary>
        /// <param name="fechaIni">Fecha de inicio para la busqueda</param>
        /// <param name="fechaFin">Fecha de termino para la busqueda</param>
        /// <returns></returns>
        public JsonResult ConfiguracionDiasDatos(string fechaIni, string fechaFin)
        {
            List<string[]> entidades = this.servicio.CfgEstimadorCfgDiaDatos(fechaIni, fechaFin);
            return Json(entidades);
        }

        /// <summary>
        /// Registra la configuración de días para el modelo TNA
        /// </summary>
        /// <param name="parametros">Parametros de configuración a registrar</param>
        /// <param name="fechaIni">Fecha de inicio del rango de días</param>
        /// <param name="fechaFin">Fecha de termino del rango de días</param>
        /// <returns></returns>
        public JsonResult ConfiguracionDiasRegistrar(PrnConfiguracionDiaDTO parametros,
            string fechaIni, 
            string fechaFin)
        {
            object res = this.servicio.CfgEstimadorCfgDiaRegistrar(parametros, fechaIni, fechaFin);
            return Json(res);
        }

        #endregion

        /// <summary>
        /// Exportar la grilla a un documento excel.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="header"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public JsonResult Exportar(string[][] form, string[] header, int modulo, string nombre)
        {
            int[] ancho = new int[header.Length];
            for (int i = 0; i < header.Length; i++)
            {
                ancho[i] = 50;
            }


            PrnFormatoExcel data = new PrnFormatoExcel()
            {
                Contenido = form,
                Cabecera = header,
                AnchoColumnas = ancho,
                Titulo = (modulo == 3) ? "REPORTE CONSULTA APORTE" : "REPORTE ASOCIACION BARRA",
                Subtitulo1 = "",
                Subtitulo2 = "sub2"
            };
            nombre = nombre.Replace(" ", String.Empty);
            string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
            string filename = (modulo == 3) ? "Reporte_consulta_aporte_" + nombre : "Reporte_asociacion_barra_" + nombre;
            string reporte = this.servicio.ExportarReporteSimple(data, pathFile, filename);

            return Json(reporte);
        }

        /// <summary>
        /// Para la descarga del excel
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString() + file;
            string app = Constantes.AppExcel;
            return File(path, app, sFecha + "_" + file);
        }
    }
}