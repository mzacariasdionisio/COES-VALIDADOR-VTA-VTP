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
    public class ConsultaEstimadorController : Controller
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
        /// Inicia la opción de consulta por generadores
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PorGeneradores()
        {
            ConsultaEstimadorModel model = new ConsultaEstimadorModel {
                ListaUnidadesEstimador = this.servicio.ListUnidadesEstimadorByTipo(ConstantesProdem.EtmrawtpGenerador),
                ListaAsociacion = this.servicio.ListaAsociacionByTipo(ConstantesProdem.EtmrawtpStrGenerador),
                ListaVariables = this.servicio.ListVariableByTipo(ConstantesProdem.EtmrawtpStrGenerador),
                Mensaje = "Puede realizar la asociacion de generadores y consultar sus mediciones..",
                TipoMensaje = ConstantesProdem.MsgInfo,
                idModulo = ConstantesProdem.EtmrawtpGenerador
            };

            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de consulta por líneas
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PorLineas()
        {
            ConsultaEstimadorModel model = new ConsultaEstimadorModel {
                ListaUnidadesEstimador = this.servicio.ListUnidadesEstimadorByTipo(ConstantesProdem.EtmrawtpLinea),
                ListaAsociacion = this.servicio.ListaAsociacionByTipo(ConstantesProdem.EtmrawtpStrLinea),
                ListaVariables = this.servicio.ListVariableByTipo(ConstantesProdem.EtmrawtpStrLinea),
                Mensaje = "Puede realizar la asociacion de líneas y consultar sus mediciones..",
                TipoMensaje = ConstantesProdem.MsgInfo,
                idModulo = ConstantesProdem.EtmrawtpLinea
            };
            
            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de consulta por transformadores
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PorTransformadores()
        {
            ConsultaEstimadorModel model = new ConsultaEstimadorModel
            {
                ListaUnidadesEstimador = this.servicio.ListUnidadesEstimadorByTipo(ConstantesProdem.EtmrawtpTrafo),
                ListaAsociacion = this.servicio.ListaAsociacionByTipo(ConstantesProdem.EtmrawtpStrTrafo),
                ListaVariables = this.servicio.ListVariableByTipo(ConstantesProdem.EtmrawtpStrTrafo),
                Mensaje = "Puede realizar la asociacion de transformadores y consultar sus mediciones..",
                TipoMensaje = ConstantesProdem.MsgInfo,
                idModulo = ConstantesProdem.EtmrawtpTrafo
            };

            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de consulta por barras
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PorBarras()
        {
            ConsultaEstimadorModel model = new ConsultaEstimadorModel
            {
                ListaUnidadesEstimador = this.servicio.ListUnidadesEstimadorByTipo(ConstantesProdem.EtmrawtpBarra),
                ListaAsociacion = this.servicio.ListaAsociacionByTipo(ConstantesProdem.EtmrawtpStrBarra),
                ListaVariables = this.servicio.ListVariableByTipo(ConstantesProdem.EtmrawtpStrBarra),
                Mensaje = "Puede realizar la asociacion de barras y consultar sus mediciones..",
                TipoMensaje = ConstantesProdem.MsgInfo,
                idModulo = ConstantesProdem.EtmrawtpBarra
            };

            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de consulta por cargas
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PorCargas()
        {
            ConsultaEstimadorModel model = new ConsultaEstimadorModel
            {
                ListaUnidadesEstimador = this.servicio.ListUnidadesEstimadorByTipo(ConstantesProdem.EtmrawtpCarga),
                ListaAsociacion = this.servicio.ListaAsociacionByTipo(ConstantesProdem.EtmrawtpStrCarga),
                ListaVariables = this.servicio.ListVariableByTipo(ConstantesProdem.EtmrawtpStrCarga),
                Mensaje = "Puede realizar la asociacion de cargas y consultar sus mediciones..",
                TipoMensaje = ConstantesProdem.MsgInfo,
                idModulo = ConstantesProdem.EtmrawtpCarga
            };

            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de consulta por shunts
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PorShunts()
        {
            ConsultaEstimadorModel model = new ConsultaEstimadorModel
            {
                ListaUnidadesEstimador = this.servicio.ListUnidadesEstimadorByTipo(ConstantesProdem.EtmrawtpShunt),
                ListaAsociacion = this.servicio.ListaAsociacionByTipo(ConstantesProdem.EtmrawtpStrShunt),
                ListaVariables = this.servicio.ListVariableByTipo(ConstantesProdem.EtmrawtpStrShunt),
                Mensaje = "Puede realizar la asociacion de shunts y consultar sus mediciones..",
                TipoMensaje = ConstantesProdem.MsgInfo,
                idModulo = ConstantesProdem.EtmrawtpShunt
            };

            return PartialView(model);
        }

        public PartialViewResult PorImportados()
        {
            ConsultaEstimadorModel model = new ConsultaEstimadorModel
            {                
                Mensaje = @"Seleccionar fecha y directorio de carga en \\fs",
                TipoMensaje = ConstantesProdem.MsgInfo
            };
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene los datos de la interfaz
        /// </summary>
        /// <param name="idUnidad"></param>
        /// <param name="idAsociacion"></param>
        /// <param name="idVariable"></param>
        /// <param name="idFuente"></param>
        /// <returns></returns>
        public JsonResult ObtenerMediciones(int idUnidad, int idAsociacion, int idVariable, 
            int idFuente, List<string> selHistoricos,  int modulo)
        {
            string strModulo = string.Empty;
            string fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            DateTime parseFecha = DateTime.ParseExact(fecha, 
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            parseFecha = parseFecha.AddDays(2);
            int id = (idUnidad != -1) ? idUnidad : idAsociacion;
            string tipo = (idUnidad != -1) ? "unidad" : "asociacion";
            object res = this.servicio.ConsultaEstimadorDatos(id, idVariable, idFuente, tipo, parseFecha, selHistoricos, modulo);
            return Json(res);
        }

        /// <summary>
        /// Inicia el Popup Asociaciones
        /// </summary>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public JsonResult GenerarAsociacion(int modulo)
        {
            string strModulo = string.Empty;
            if (modulo == ConstantesProdem.EtmrawtpGenerador)
                strModulo = ConstantesProdem.EtmrawtpStrGenerador;
            if (modulo == ConstantesProdem.EtmrawtpBarra)
                strModulo = ConstantesProdem.EtmrawtpStrBarra;
            if (modulo == ConstantesProdem.EtmrawtpTrafo)
                strModulo = ConstantesProdem.EtmrawtpStrTrafo;
            if (modulo == ConstantesProdem.EtmrawtpCarga)
                strModulo = ConstantesProdem.EtmrawtpStrCarga;
            if (modulo == ConstantesProdem.EtmrawtpShunt)
                strModulo = ConstantesProdem.EtmrawtpStrShunt;
            if (modulo == ConstantesProdem.EtmrawtpLinea)
                strModulo = ConstantesProdem.EtmrawtpStrLinea;

            ConsultaEstimadorModel model = new ConsultaEstimadorModel {
                ListaAsociacion = this.servicio.ListaAsociacionByTipo(strModulo),
                ListaUnidadesEstimador = this.servicio.ListUnidadesEstimadorByTipo(modulo),
                ListaAsociacionDetalle = this.servicio.ListAsociacionPuntosMedicionByTipo(strModulo)
            };

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        /// <summary>
        /// Registra las asociaciones creadas
        /// </summary>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public JsonResult RegistrarAsociacion(int modulo, List<PrnAsociacionDTO> datos)
        {
            List<PrnAsociacionDTO> lista = this.servicio.SaveAsociacion(modulo, datos);
            return Json(lista);
        }

        /// <summary>
        /// Actualiza los datos mostrados de una medición
        /// </summary>
        /// <param name="idUnidad"></param>
        /// <param name="idAsociacion"></param>
        /// <param name="idVariable"></param>
        /// <param name="idFuente"></param>
        /// <param name="fecha"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public JsonResult ActualizarMedicion(int idUnidad, int idAsociacion, int idVariable, int idFuente, string fecha, int modulo)
        {            
            int id = (idUnidad != -1) ? idUnidad : idAsociacion;
            DateTime sFecha = DateTime.ParseExact(fecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            decimal[] dArray;
            if (idUnidad == -1)
                dArray = this.servicio.ArrayEstimadorRawPorAsociacion(id, sFecha, idVariable, idFuente, modulo);
            else 
                dArray = this.servicio.ArrayEstimadorRawPorUnidad(id, sFecha, idVariable, idFuente, modulo);
            return Json(dArray);
        }

        /// <summary>
        /// Exportar la grilla a un documento excel.
        /// </summary>
        /// <param name="form">Contenido del reporte</param>
        /// <param name="header">Cabecera del reporte</param>
        /// <param name="nombre">Nombre del archivo a exportar</param>
        /// <returns></returns>
        public JsonResult Exportar(string[][] form, string[] header, string nombre)
        {
            nombre = nombre.Replace(" ", String.Empty);

            PrnFormatoExcel data = new PrnFormatoExcel()
            {

                Contenido = form,
                Cabecera = header,
                AnchoColumnas = new int[] {
                    50,50,50,50,50,50,50,50
                },
                Titulo = "CONSULTA ESTIMADOR",
                Subtitulo1 = nombre,
                Subtitulo2 = "sub2"
            };
            
            string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
            string filename = "Reporte_estimador_TNA_" + nombre;
            string reporte = this.servicio.ExportarReporteSimple(data, pathFile, filename);

            return Json(reporte);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string path = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString() + file;
            string app = Constantes.AppExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);
            return File(bytes, app, file);
        }

        /// <summary>
        /// Registra las asociaciones creadas
        /// </summary>
        /// <param name="idUnidad"></param>
        /// <param name="idAsociacion"></param>
        /// <returns></returns>
        public JsonResult ListaVariablesCentrales(int idUnidad, int idAsociacion)
        {
            List<PrnVariableDTO> lista = new List<PrnVariableDTO>();

            if (idUnidad == -1)
            {
                lista = this.servicio.ListVariableByTipo(ConstantesProdem.EtmrawtpStrBarra)
                    .Where(x=> x.Prnvarcodi == 15 || x.Prnvarcodi == 16)
                    .ToList();
            }
            if (idAsociacion == -1) 
            {
                lista = this.servicio.ListVariableByTipo(ConstantesProdem.EtmrawtpStrBarra);
            }
            return Json(lista);
        }

        #region Métodos del submódulo de Importación de archivos
        /// <summary>
        /// Importa y procesa los archivos raw de una dirección para la fecha indicada
        /// </summary>
        /// <param name="fechaImportacion">Fecha de los archivos a procesar</param>
        /// <param name="direccion">Dirección de los archivos a procesar</param>
        /// <returns></returns>
        public JsonResult ImportarArchivos(string fechaImportacion, string direccion)
        {
            //string direccionValida = (!direccion.EndsWith(@"\")) ? direccion + @"\" : direccion;
            object res = this.servicio.CfgEstimadorImportarArchivos(fechaImportacion, direccion);
            return Json(res);
        }

        #endregion
    }
}