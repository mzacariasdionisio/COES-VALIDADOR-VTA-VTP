using System;
using System.Collections;
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
using System.IO;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Controllers
{
    public class PronosticoController : Controller
    {
        PronosticoDemandaAppServicio servicio = new PronosticoDemandaAppServicio();

        public ActionResult Index()
        {
            return View();
        }

        #region Módulo de Demanda Ejecutada por Áreas

        /// <summary>
        /// Inicia la opción de demanda ejecutada por áreas 
        /// </summary>
        /// <returns></returns>
        public ActionResult Demanda()
        {
            PronosticoModel model = new PronosticoModel();
            model.Mensaje = "Puede consultar y depurar la demanda ejecutada" +
                " a nivel de áreas operativas";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            model.idSein = ConstantesProdem.PtomedicodiASein;
            model.idCentro = ConstantesProdem.PtomedicodiACentro;
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);
            model.ListArea = UtilProdem.ListAreaOperativa(true);

            return PartialView(model);
        }

        /// <summary>
        /// Carga los datos y les da formato para la presentación
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public JsonResult DemandaDatos(int idArea, string regFecha)
        {
            object res = this.servicio.DemandaDatos(idArea, regFecha);
            return Json(res);
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <returns></returns>
        public JsonResult DemandaSave(int idPunto, string regFecha, PrnMedicion48DTO dataMedicion)
        {
            object res = this.servicio.DemandaSave(idPunto, regFecha, dataMedicion, User.Identity.Name);
            return Json(res);
        }

        /// <summary>
        /// Obtiene los datos respecto a los flujos de las áreas operativas
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <returns></returns>
        public JsonResult DemandaFlujos(int idArea)
        {
            object res = this.servicio.DemandaFlujos(idArea);
            return Json(res);
        }

        /// <summary>
        /// Registra los flujos de linea relacionados al área operativa
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="listFlujos">Lista de flujos relacionados (objeto {id:x, prioridad:x})</param>
        /// <returns></returns>
        public JsonResult DemandaFlujosSave(int idArea, List<PrnFormularelDTO> listFlujos)
        {
            object res = this.servicio.DemandaFlujosSave(idArea, listFlujos, User.Identity.Name);
            return Json(res);
        }

        /// <summary>
        /// Actualiza el perfil patrón
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFechaA">Fecha del intervalo correspondiente al intervalo de la mañana</param>
        /// <param name="regFechaB">Fecha del intervalo correspondiente al intervalo de la tarde</param>
        /// <param name="esLunes">Flag que indica si se debe considerar la fecB</param>
        /// <param name="tipoPatron">Parámetro que indica el tipo de obtención del perfil patrón</param>
        /// <param name="dsvPatron">Parámetro que indica el porcentaje de desviación respecto al perfil patrón</param>
        /// <param name="dataMediciones">Mediciones que conforman el perfil patrón mostrado</param>
        /// <returns></returns>
        public JsonResult DemandaUpdatePatron(int idPunto, string regFechaA, string regFechaB,
            bool esLunes, string tipoPatron, List<decimal[]> dataMediciones)
        {
            object res;
            res = this.servicio.DemandaUpdatePatron(idPunto, regFechaA, regFechaB,
                esLunes, tipoPatron, dataMediciones);
            return Json(res);
        }

        #endregion

        #region Módulo de Demanda Vegetativa por Áreas

        /// <summary>
        /// Inicia la opción de demanda vegetativa por áreas
        /// </summary>
        /// <returns></returns>
        public ActionResult Vegetativa()
        {
            PronosticoModel model = new PronosticoModel();
            model.Mensaje = "Puede consultar y depurar la demanda vegetativa" +
                " a nivel de áreas operativas";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            model.idSein = ConstantesProdem.PtomedicodiASein;
            model.idCentro = ConstantesProdem.PtomedicodiACentro;
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.ListArea = UtilProdem.ListAreaOperativa(true);

            return PartialView(model);
        }

        /// <summary>
        /// Carga los datos y les da formato para la presentación
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public JsonResult VegetativaDatos(int idArea, string regFecha)
        {
            object res = this.servicio.VegetativaDatos(idArea, regFecha);
            return Json(res);
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <returns></returns>
        public JsonResult VegetativaSave(int idPunto, string regFecha, PrnMedicion48DTO dataMedicion)
        {
            object res = this.servicio.VegetativaSave(idPunto, regFecha, dataMedicion, User.Identity.Name);
            return Json(res);
        }
        
        /// <summary>
        /// Actualiza el perfil patrón
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFechaA">Fecha del intervalo correspondiente al intervalo de la mañana</param>
        /// <param name="regFechaB">Fecha del intervalo correspondiente al intervalo de la tarde</param>
        /// <param name="esLunes">Flag que indica si se debe considerar la fecB</param>
        /// <param name="tipoPatron">Parámetro que indica el tipo de obtención del perfil patrón</param>
        /// <param name="dsvPatron">Parámetro que indica el porcentaje de desviación respecto al perfil patrón</param>
        /// <param name="dataMediciones">Mediciones que conforman el perfil patrón mostrado</param>
        /// <returns></returns>
        public JsonResult VegetativaUpdatePatron(int idPunto, string regFechaA, string regFechaB,
            bool esLunes, string tipoPatron, List<decimal[]> dataMediciones)
        {
            object res;
            res = this.servicio.VegetativaUpdatePatron(idPunto, regFechaA, regFechaB,
                esLunes, tipoPatron, dataMediciones);
            return Json(res);
        }
        #endregion

        #region Módulo de Pronostico por Áreas

        /// <summary>
        /// Inicia la opción de pronostico por áreas
        /// </summary>
        /// <returns></returns>
        public ActionResult PronosticoPorAreas()
        {
            PronosticoModel model = new PronosticoModel();
            model.Mensaje = "Puede consultar y ejecutar el proceso de pronostico de" +
                " la demanda a nivel de áreas operativas";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            model.idSein = ConstantesProdem.PtomedicodiASein;
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.ListArea = UtilProdem.ListAreaOperativa(true);

            return PartialView(model);
        }

        /// <summary>
        /// Carga los datos y les da formato para la presentación
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public JsonResult PronosticoPorAreasDatos(int idArea, string regFecha)
        {
            object res = this.servicio.PronosticoPorAreasDatos(idArea, regFecha);
            return Json(res);
        }

        /// <summary>
        /// Ejecuta el proceso de pronóstico de la demanda
        /// </summary>
        /// <param name="regFecha">Fecha de registro</param>
        /// <param name="numIteraciones">Número de iteraciones solicitadas</param>
        /// <param name="idTipo">Tipo de pronóstico solicitado</param>
        /// <returns></returns>
        public JsonResult PronosticoPorAreasEjecutar(string regFecha, int numIteraciones, string idTipo)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;

            try
            {
                this.servicio.PronosticoPorAreasEjecutar(regFecha, numIteraciones, idTipo);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El proceso se ha realizado de manera exitosa!";
            }
            catch (Exception e)
            {
                typeMsg = ConstantesProdem.MsgError;
                dataMsg = e.Message;
            }

            return Json(new { typeMsg, dataMsg });
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <returns></returns>
        public JsonResult PronosticoPorAreasSave(int idArea, string regFecha, PrnMedicion48DTO dataMedicion)
        {
            object res = this.servicio.PronosticoPorAreasSave(idArea, regFecha, dataMedicion, User.Identity.Name);
            return Json(res);
        }

        #endregion

        #region Módulo de Desviación
        /// <summary>
        /// Inicia la opción de Desviación
        /// </summary>
        /// <returns></returns>
        public ActionResult Desviacion()
        {
            PronosticoModel model = new PronosticoModel();
            model.ListBarraCPActiva = this.servicio.GetListBarrasCP(ConstantesProdem.Prcatecodi);
            model.Mensaje = "Puede realizar la evaluación del pronóstico de la demanda";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene los datos de la interfaz
        /// </summary>
        /// <param name="idRegistro"></param>
        /// <param name="idBarra"></param>
        /// <returns></returns>
        public JsonResult ObtenerMediciones(string tipo, string grafico, int val)
        {
            string fechaRegistro = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);//"13/08/2020"
            DateTime parseFecha = DateTime.ParseExact(fechaRegistro,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            object res = this.servicio.ObtenerMedicionDesviacion(tipo, grafico, val, parseFecha);
            return Json(res);
        }

        /// <summary>
        /// Actualiza los datos mostrados de una medición
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="grafico"></param>
        /// <param name="val"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ActualizarMedicion(string tipo, string grafico, int val, string fecha)
        {
            DateTime sFecha = DateTime.ParseExact(fecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            Tuple<List<PrnMediciongrpDTO>, string, string> res = this.servicio.ObtenerMedicionDesviacionCalendario(tipo, grafico, val, sFecha);
            List<PrnMediciongrpDTO> mediciones = res.Item1;
            List<ParametrosModel> modelos = new List<ParametrosModel>();
            ParametrosModel modelo = new ParametrosModel();
            foreach (var item in mediciones)
            {
                modelo = new ParametrosModel();
                modelo.dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, item);
                modelo.barraNombre = item.Gruponomb;
                modelo.TipoMensaje = res.Item2;
                modelo.Mensaje = res.Item3;
                modelos.Add(modelo);
            }

            return Json(modelos);
        }

        /// <summary>
        /// Exportar la grilla a un documento excel.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="header"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public JsonResult ExportarDesviacion(string[][] form, string[] header, string nombre)
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
                Titulo = "REPORTE DESVIACIÓN",
                Subtitulo1 = "",
                Subtitulo2 = "sub2"
            };
            string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
            string filename = "Reporte_consulta_desviación_" + nombre;
            string reporte = this.servicio.ExportarReporteSimple(data, pathFile, filename);

            return Json(reporte);
        }

        /// <summary>
        /// Para la descarga del excel
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivoDesviacion(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString() + file;
            string app = Constantes.AppExcel;
            return File(path, app, sFecha + "_" + file);
        }
        #endregion
    }
}