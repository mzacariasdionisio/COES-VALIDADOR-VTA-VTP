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
    public class DemandaBarrasController : Controller
    {
        PronosticoDemandaAppServicio servicio = new PronosticoDemandaAppServicio();
        public ActionResult Index()
        {
            return View();
        }

        #region Módulo de Demanda en barras por Áreas
        /// <summary>
        /// Inicia la opción de demanda ejecutada por áreas 
        /// </summary>
        /// <returns></returns>
        public ActionResult DemandaBarras()
        {
            PronosticoModel model = new PronosticoModel();
            model.Mensaje = "Puede consultar la demnada en barras agrupada por áreas operativas";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            model.idSein = ConstantesProdem.PtomedicodiASein;
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.ListArea = UtilProdem.ListAreaOperativa(true);
            model.ListVersionFecha = this.servicio.ListVersionesPronosticoPorFecha(model.Fecha, 
                model.Fecha);
            //model.ListVersion = this.servicio.ListVersionMedicionGrp();
            return PartialView(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Identificador de la version a consultar</param>
        /// <returns></returns>
        public JsonResult DemandaBarrasDatos(int idArea, string regFecha, int idVersion)
        {
            object res = this.servicio.DemandaBarrasDatos(idArea, regFecha, idVersion);
            return Json(res);
        }

        //Assetec 20220303
        /// <summary>
        /// Actualiza los datos mostrados de una medición
        /// </summary>
        /// <param name="idArea">Identificador del area</param>
        /// <param name="idVersion">Identificador de la version</param>
        /// <param name="fecha"> fecha del calendario(de los 7)</param>
        /// <param name="grafica"> indica el tipo de informacion que se visuliza en la grafica</param>
        /// <returns></returns>
        public JsonResult ActualizarMedicionDemandaBarra(int idArea, int idVersion, string fecha, string grafica)
        {
            object res = this.servicio.ActualizacionMedicionDemandaBarraByCalendario(idArea, fecha, idVersion, grafica);

            return Json(res);
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idArea">Identificador del Area</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <param name="dataBarra">Datos de las Barras</param>
        /// <param name="idVersion">Identificador de la Version</param>
        /// <returns></returns>
        public JsonResult DemandaBarrasSave(int idArea, string regFecha, PrnMediciongrpDTO dataMedicion, PrnMediciongrpDTO dataBarra, int idVersion)
        {
            object res = this.servicio.DemandaBarraSave(idArea, regFecha, dataMedicion, dataBarra, User.Identity.Name, idVersion);
            return Json(res);
        }
        #endregion

        #region Módulo de Pronostico por Barras
        /// <summary>
        /// Inicia la opción de pronostico por barras
        /// </summary>
        /// <returns></returns>
        public ActionResult PronosticoPorBarras()
        {
            PronosticoModel model = new PronosticoModel();
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);
            model.ListBarraCP = this.servicio.GetModeloActivo(ConstantesProdem.RegStrTodos).
                GroupBy(x => x.Prnredbarracp).
                Select(y => y.First()).ToList();
            //model.ListVersion = this.servicio.ListVersionMedicionGrp();
            model.ListVersionFecha = this.servicio.ListVersionesPronosticoPorFecha(model.Fecha, 
                model.Fecha);
            model.ListRelacionTna = this.servicio.ListaRelacionTna();
            model.ListRelacionTna = model.ListRelacionTna
                    .Where(x => x.Detalle.Count() > 1)
                    .ToList();

            if (model.ListBarraCP.Count != 0)
            {
                model.Mensaje = "Puede consultar y ejecutar el proceso de pronostico de" +
                " la demanda a nivel de barras CP";
                model.TipoMensaje = ConstantesProdem.MsgInfo;
            }
            else
            {
                model.Mensaje = "No se ha encontrado una versión activa para la reducción de red...";
                model.TipoMensaje = ConstantesProdem.MsgError;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Carga los datos y les da formato para la presentación
        /// </summary>
        /// <param name="idBarra">Identificador de la tabla PR_GRUPO</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Identificador de la versión</param>
        /// <param name="idAgrupacion">Identificador de la Agrupacion</param>
        /// <returns></returns>
        public JsonResult PronosticoPorBarrasDatos(int idBarra, string regFecha, int idVersion, int idAgrupacion)
        {
            //Assetec 20220224
            int flag = (idBarra == -1) ? 2 : 1;
            object res = this.servicio.PronosticoPorBarrasDatos(idBarra, idAgrupacion, regFecha, idVersion, flag);

            return Json(res);
            //Assetec 20220224
        }

        //Assetec 20220224
        /// <summary>
        /// Actualiza los datos mostrados de una medición
        /// </summary>
        /// <param name="idBarra">Identificador de la barra</param>
        /// <param name="idVersion">Identificador de la version</param>
        /// <param name="idAgrupacion"> Identificador de la agrupacion</param>
        /// <param name="fecha"> fecha del calendario(de los 7)</param>
        /// <param name="grafica"> indica el tipo de informacion que se visuliza en la grafica</param>
        /// <returns></returns>
        public JsonResult ActualizarMedicionPronosticoBarra(int idBarra, int idVersion, int idAgrupacion, string fecha, string grafica)
        {
            int flag = (idBarra == -1) ? 2 : 1;
            object res = this.servicio.ActualizacionMedicionPronosticoBarraByCalendario(idBarra, idAgrupacion, fecha, idVersion, grafica, flag);

            return Json(res);
        }

        /// <summary>
        /// Ejecuta el proceso de pronóstico de la demanda
        /// </summary>
        /// <param name="regFecha">Fecha de registro</param>
        /// <param name="numIteraciones">Número de iteraciones solicitadas</param>
        /// <param name="idTipo">Tipo de pronóstico solicitado</param>
        /// <param name="selBarras">Barras CP seleccionadas</param>
        /// <param name="nomVersion">Nombre de la versión a registrar</param>
        /// <param name="idMetodo">Identificador del tipo de proceso [S:Suavizado, P:Promedio]</param>
        /// <param name="idFuente">Identificador de la fuente de información a utilizar [tna, pr03]</param>
        /// <param name="nroDiaAporte">Número de días de cálculo de factor de aporte</param>
        /// <param name="valNegativo">Valor de corrección para valores negativos</param>
        /// <returns></returns>
        public JsonResult PronosticoPorBarrasEjecutar(string regFecha, int numIteraciones, 
            string idTipo, List<int> selBarras, 
            string nomVersion, string idMetodo, 
            string idFuente, int nroDiaAporte,
            decimal valNegativo)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;

            //Valida la versión
            if (string.IsNullOrEmpty(nomVersion))
            {
                typeMsg = ConstantesProdem.MsgError;
                dataMsg = "El nombre de la versión no puede estar vacio";
                return Json(new { typeMsg, dataMsg });
            }
            int idVersion = this.servicio.ValidarVersionPronosticoPorBarras(nomVersion);
            if (idVersion == 0)
            {
                typeMsg = ConstantesProdem.MsgError;
                dataMsg = "Ya existe una versión con ese nombre, ingrese uno nuevo";
                return Json(new { typeMsg, dataMsg });
            }
            try
            {
                string idBarra = (selBarras.Count != 0) ? string.Join(",", selBarras) : "0";
                this.servicio.PronosticoPorBarrasEjecutar(regFecha,
                    numIteraciones, idTipo,
                    idBarra, idMetodo, idFuente, 
                    idVersion, valNegativo,
                    User.Identity.Name);
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

        ///Assetec 20220301
        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idBarra">Identificador de la tabla PR_GRUPO</param>
        /// <param name="idAgrupacion">Identificador de la tabla PRN_RELACIONTNA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Identificador de la versión</param>
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <param name="dataBase">Datos del pronostico total mostrado en la grilla</param>
        /// <returns></returns>
        public JsonResult PronosticoPorBarrasSave(int idBarra, int idAgrupacion, string regFecha, int idVersion, PrnMediciongrpDTO dataMedicion, PrnMediciongrpDTO dataBase)
        {
            int flag = (idBarra == -1) ? 2 : 1;
            object res = this.servicio.PronosticoPorBarrasSave(idBarra, idAgrupacion, regFecha, idVersion, dataMedicion, dataBase, User.Identity.Name, flag);
            return Json(res);
        }

        /// <summary>
        /// Duplica la versión seleccionada
        /// </summary>
        /// <param name="refVersion">Identificador de la versión seleccionada (filtro "Versiones")</param>
        /// <param name="refFecha">Fecha de consulta de la versión original</param>
        /// <param name="nomVersion">Nombre de la nueva versión (Duplicado)</param>
        /// <param name="regFecha">Nueva fecha de la versión duplicada</param>
        /// <param name="flgActualizar">Flag que indica si es un nuevo registro o una actualización</param>
        /// <returns></returns>
        public JsonResult PronosticoPorBarrasDuplicarVersion(int refVersion, string refFecha,
            string nomVersion, string regFecha, bool flgActualizar)
        {
            object res = this.servicio.PronosticoPorBarrasDuplicarVersion(refVersion, refFecha,
                nomVersion, regFecha, flgActualizar);
            return Json(res);
        }

        /// <summary>
        /// Devuelve la lista de versiones de pronósticos creados
        /// </summary>
        /// <param name="fechaIni">Fecha de inicio de la consulta</param>
        /// <param name="fechaFin">Fecha de termino de la consulta</param>
        /// <returns></returns>
        public JsonResult PronosticoPorBarrasActualizarVersion(string fechaIni, string fechaFin)
        {
            List<PrnVersiongrpDTO> res = this.servicio
                .ListVersionesPronosticoPorFecha(fechaIni, fechaFin);
            return Json(res);
        }

        /// <summary>
        /// Metodo para exportar el pronóstico
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio para la exportación</param>
        /// <param name="fechaFin">Fecha de termino para la exportación</param>
        /// <param name="idVersion">Identificador de la versión a exportar</param>
        /// <returns></returns>
        public JsonResult PronosticoPorBarrasExportar(string fechaInicio, string fechaFin, int idVersion)
        {
            string ruta = this.servicio.PronosticoPorBarrasExportar(fechaInicio, fechaFin, idVersion);
            return Json(ruta);
        }

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string path = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString() + file;
            string app = Constantes.AppExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);
            return File(bytes, app, file);
        }
        #endregion

        #region Módulo de Traslado de Carga

        /// <summary>
        /// Inicia la opción de Traslado de Carga 
        /// </summary>
        /// <returns></returns>
        public ActionResult TrasladoCarga()
        {
            PronosticoModel model = new PronosticoModel();
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.ListBarrasOrigen = this.servicio.GetListBarrasCPTraslado(ConstantesProdem.Prcatecodi, -1);
            model.ListBarrasDestino = this.servicio.GetListBarrasCPTraslado(ConstantesProdem.Prcatecodi, -1);
            model.ListVersionFecha = this.servicio.ListVersionesPronosticoPorFecha(model.Fecha,
                model.Fecha);
            model.Mensaje = "Puede realizar el traslado de carga entre Barras CP";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            return PartialView(model);
        }

        /// <summary>
        /// Trae la lista de mediciones de las barras segun los filtros
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="idVersion"></param>
        /// <param name="idOrigen"></param>
        /// <param name="idDestino"></param>
        /// <returns></returns>
        public JsonResult ObtenerMedicionesBarras(string fecha, int idVersion, int idOrigen, int idDestino)
        {
            object res = this.servicio.ObtenerMedicionesBarras(idVersion, idOrigen, idDestino, fecha);
            return Json(res);
        }

        /// <summary>
        /// Trae la lista de Barras para los filtros
        /// </summary>
        /// <param name="idVersion">Identificador de la versión seleccionada en presentación</param>
        /// <returns></returns>
        public JsonResult CargaFiltrosBarras(int idVersion)
        {
            PronosticoModel model = new PronosticoModel();
            model.ListBarrasOrigen = this.servicio.GetListBarrasCPTraslado(ConstantesProdem.Prcatecodi, idVersion);
            model.ListBarrasDestino = this.servicio.GetListBarrasCPTraslado(ConstantesProdem.Prcatecodi, idVersion);
            return Json(model);
        }

        /// <summary>
        /// Trae la lista de mediciones de las barras segun los filtros
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="idVersion"></param>
        /// <param name="idOrigen"></param>
        /// <param name="idDestino"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult ActualizarMedicionTraslado(string fecha, int idVersion, int idOrigen, int idDestino, decimal[][] data)
        {
            object resMesssage = this.servicio.ActualizarMedicionesPorTrasladoCarga(fecha, idVersion, idOrigen, idDestino, data);
            return Json(resMesssage);
        }
        #endregion
    }
}