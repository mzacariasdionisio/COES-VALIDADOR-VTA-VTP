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
    public class DepuracionController : Controller
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
        /// Inicia la opción de depuración por puntos
        /// </summary>
        /// <returns></returns>
        public ActionResult PorPuntos()
        {
            DepuracionModel model = new DepuracionModel();
            model.Mensaje = "Puede realizar la depuración de la información reportada" +
                " a nivel de puntos de medición";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            model.IdModulo = ConstantesProdem.SMDepuracionByPunto;

            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaIni = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);

            model.ListUbicacion = this.servicio.PR03Ubicaciones();
            model.ListTipoDemanda = UtilProdem.ListTipoDemandaDiaria();
            model.ListTipoEmpresa = UtilProdem.ListTipoEmpresa();
            model.ListEmpresa = this.servicio.PR03Empresas().
                Where(x => x.Tipoemprcodi == ConstantesProdem.TipoemprcodiDistribuidores).ToList();
            model.ListPtomedicion = this.servicio.PR03Puntos().
                Where(x => x.Tipoemprcodi == ConstantesProdem.TipoemprcodiDistribuidores).ToList();
            model.ListJustificacion = this.servicio.ListaMotivo();

            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de depuración por agrupaciones
        /// </summary>
        /// <returns></returns>
        public ActionResult PorAgrupaciones()
        {
            DepuracionModel model = new DepuracionModel();
            model.Mensaje = "Puede realizar la depuración de la información reportada" +
                " a nivel de agrupaciones";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            model.IdModulo = ConstantesProdem.SMDepuracionByAgrupacion;

            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);

            model.ListTipoDemanda = UtilProdem.ListTipoDemandaDiaria();
            model.ListArea = UtilProdem.ListAreaOperativa(false);
            model.ListEmpresa = this.servicio.PR03Empresas();
            model.ListAgrupacion = servicio.ListMeAgrupacion();

            return PartialView(model);
        }

        /// <summary>
        /// Lista el detalle de la tabla segun filtros
        /// </summary>
        /// <param name="start">Parámetro del DataTable</param>
        /// <param name="length">Parámetro del DataTable</param>
        /// <param name="idModulo">Identificador del módulo seleccionado</param>
        /// <param name="idTipo">Identificador del tipo de información a buscar</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataFiltros">Datos de los filtros seleccionados</param>
        /// <returns></returns>
        public JsonResult List(int start, int length, int idModulo, string regFecha, DepuracionModel dataFiltros)
        {
            object res = new object();
            DepuracionModel model = new DepuracionModel();
            switch (idModulo)
            {
                case ConstantesProdem.SMParametosByPunto:
                    {
                        string idArea = (dataFiltros.SelUbicacion.Count != 0) ? string.Join(",", dataFiltros.SelUbicacion) : "0";
                        string idEmpresa = (dataFiltros.SelEmpresa.Count != 0) ? string.Join(",", dataFiltros.SelEmpresa) : "0";
                        string idPunto = (dataFiltros.SelPuntos.Count != 0) ? string.Join(",", dataFiltros.SelPuntos) : "0";
                        string idPerfil = (dataFiltros.SelPerfil.Count != 0) ? string.Join(",", dataFiltros.SelPerfil) : "0";
                        string idClasificacion = (dataFiltros.SelClasificacion.Count != 0) ? string.Join(",", dataFiltros.SelClasificacion) : "0";
                        string idAreaOperativa = (dataFiltros.SelAreaOperativa.Count != 0) ? string.Join(",", dataFiltros.SelAreaOperativa) : "0";
                        res = this.servicio.DepuracionPorPuntosList(start, length, dataFiltros.SelTipoEmpresa, 
                            dataFiltros.SelTipoDemanda, regFecha, idArea, idEmpresa, idPunto, idPerfil, idClasificacion,
                            idAreaOperativa, dataFiltros.SelJustificacion, dataFiltros.SelBarra, dataFiltros.SelOrden);
                    }
                    break;
                case ConstantesProdem.SMParametosByAgrupacion:
                    {
                        string idArea = (dataFiltros.SelArea.Count != 0) ? string.Join(",", dataFiltros.SelArea) : "0";
                        string idEmpresa = (dataFiltros.SelEmpresa.Count != 0) ? string.Join(",", dataFiltros.SelEmpresa) : "0";
                        string idPunto = (dataFiltros.SelAgrupacion.Count != 0) ? string.Join(",", dataFiltros.SelAgrupacion) : "0";
                        res = this.servicio.DepuracionPorAgrupacionesList(start, length, dataFiltros.SelPronostico,
                            idArea, idEmpresa, idPunto, dataFiltros.SelTipoDemanda, regFecha);
                    }
                    break;
            }
            
            return Json(res);
        }

        /// <summary>
        /// Carga los datos y les da formato para la presentación
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idTipoDemanda">Identificador del tipo de información a buscar</param>
        /// <param name="idTipoEmpresa">Identificador del tipo de empresa</param>
        /// <param name="idModulo">Identificador del submodulo seleccionado</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public JsonResult Datos(int idPunto, int idTipoDemanda, int idTipoEmpresa, int idModulo, string regFecha)
        {
            object res = new object();
            switch (idModulo)
            {
                case ConstantesProdem.SMParametosByPunto:
                    {
                        res = this.servicio.DepuracionPorPuntosDatos(idPunto, idTipoDemanda, idTipoEmpresa, regFecha);
                    }
                    break;
                case ConstantesProdem.SMParametosByAgrupacion:
                    {
                        res = this.servicio.DepuracionPorAgrupacionesDatos(idPunto, idTipoDemanda, regFecha);
                    }
                    break;
            }

            return Json(res);
        }

        /// <summary>
        /// Actualiza el perfil patrón
        /// </summary>
        /// <param name="idModulo">Identificador del módulo</param>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFechaA">Fecha del intervalo correspondiente al intervalo de la mañana</param>
        /// <param name="regFechaB">Fecha del intervalo correspondiente al intervalo de la tarde</param>
        /// <param name="esLunes">Flag que indica si se debe considerar la fecB</param>
        /// <param name="tipoPatron">Parámetro que indica el tipo de obtención del perfil patrón</param>
        /// <param name="dsvPatron">Parámetro que indica el porcentaje de desviación respecto al perfil patrón</param>
        /// <param name="dataMediciones">Mediciones que conforman el perfil patrón mostrado</param>
        /// <returns></returns>
        public JsonResult UpdatePatron(int idModulo, int idPunto, string regFechaA, string regFechaB,
            bool esLunes, string tipoPatron, decimal? dsvPatron, List<decimal[]> dataMediciones)
        {
            object res;
            res = this.servicio.DepuracionUpdatePatron(idModulo, idPunto, regFechaA, regFechaB,
                esLunes, tipoPatron, dsvPatron, dataMediciones);
            return Json(res);
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idTipoDemanda">Identificador del tipo de demanda</param>
        /// <param name="regFecha">Fecha del registro (solo valida el día de la semana)</param>
        /// <param name="dataMedicion">Datos de la nueva medición defecto</param>
        /// <returns></returns>
        public JsonResult Save(int idPunto, int idTipoDemanda, string regFecha, PrnMedicion48DTO dataMedicion)
        {
            object res = this.servicio.DepuracionSave(idPunto, idTipoDemanda, regFecha, dataMedicion);
            return Json(res);
        }


        /// <summary>
        /// Actualiza los filtros segun lo seleccionado
        /// </summary>
        /// <param name="dataFiltros"></param>
        /// <returns></returns>
        public JsonResult UpdateFiltros(DepuracionModel dataFiltros)
        {
            PerfilesModel model = new PerfilesModel();

            model.ListPtomedicion = this.servicio.PR03Puntos();
            model.ListEmpresa = this.servicio.PR03Empresas();

            if (dataFiltros.SelById != null)
            {
                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => x.Ptomedicodi.Equals(dataFiltros.SelById)).ToList();

                return Json(model);
            }
            if (dataFiltros.SelUbicacion.Count != 0)
            {
                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => dataFiltros.SelUbicacion.Contains(x.Areacodi)).ToList();
            }
            if (dataFiltros.SelTipoEmpresa != 0)
            {
                model.ListEmpresa = this.servicio.PR03Empresas().
                    Where(x => x.Tipoemprcodi.Equals(dataFiltros.SelTipoEmpresa)).ToList();

                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => x.Tipoemprcodi.Equals(dataFiltros.SelTipoEmpresa)).ToList();
            }
            if (dataFiltros.SelEmpresa.Count != 0)
            {
                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => dataFiltros.SelEmpresa.Contains(x.Emprcodi ?? 0)).ToList();
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Elimina los ajustes automáticos y manuales
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idLectura">Identificador de la tabla ME_LECTURA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public JsonResult EliminarAjustes(int idPunto, int idLectura, string regFecha)
        {
            object res = this.servicio.DepuracionEliminarAjuste(idPunto, idLectura, regFecha);
            return Json(res);
        }

        /// <summary>
        /// Exporta el reporte por puntos de medición
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        public JsonResult ExportarReporte(string fecIni, string fecFin, int idTipoEmpresa)
        {
            string ruta = this.servicio.DepuracionPorPuntosReporte(fecIni, fecFin, idTipoEmpresa);
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

        #region Módulo de Reprocesamiento de información
        /// <summary>
        /// Inicia la opción de reprocesamiento de información
        /// </summary>
        /// <returns></returns>
        public ActionResult Reprocesar()
        {
            DepuracionModel model = new DepuracionModel();
            model.Mensaje = "Puede reprocesar la información reportada por los agentes(Distribuidores) desde extranet." +
                " Se volvera a clasificar, depurar automáticamente  y se calculará el tipo de perfíl de demanda." +
                " (!)ADVERTENCIA: Se perderan los ajustes previos";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.ListTipoDemanda = UtilProdem.ListTipoDemandaDiaria();
            model.ListTipoEmpresa = UtilProdem.ListTipoEmpresa();
            model.ListUbicacion = this.servicio.PR03Ubicaciones();
            model.ListEmpresa = this.servicio.PR03Empresas();
            model.ListPtomedicion = this.servicio.PR03Puntos();

            return PartialView(model);
        }

        /// <summary>
        /// Detalle de la información seleccionada
        /// </summary>
        /// <param name="start">Parámetro del DataTable</param>
        /// <param name="length">Parámetro del DataTable</param>
        /// <param name="dataFiltros">Datos de los filtros seleccionados</param>
        /// <returns></returns>
        public JsonResult ReprocesarList(int start, int length, DepuracionModel dataFiltros)
        {
            object res;
            List<MePtomedicionDTO> entitys = this.servicio.PR03Puntos();
            
            if (dataFiltros.SelById != null)
            {
                entitys = entitys.Where(x => x.Ptomedicodi.Equals(dataFiltros.SelById)).ToList();
                res = this.servicio.ReprocesarList(start, length, entitys);
                return Json(res);
            }
            if (dataFiltros.SelUbicacion.Count != 0)
            {
                entitys = entitys.Where(x => dataFiltros.SelUbicacion.Contains(x.Areacodi)).ToList();
            }
            if (dataFiltros.SelListTipoEmpresa.Count != 0)
            {
                entitys = entitys.Where(x => dataFiltros.SelListTipoEmpresa.Contains(x.Tipoemprcodi)).ToList();
            }
            if (dataFiltros.SelEmpresa.Count != 0)
            {
                entitys = entitys.Where(x => dataFiltros.SelEmpresa.Contains(x.Emprcodi ?? 0)).ToList();
            }
            if (dataFiltros.SelPuntos.Count != 0)
            {
                entitys = entitys.Where(x => dataFiltros.SelPuntos.Contains(x.Ptomedicodi)).ToList();
            }

            res = this.servicio.ReprocesarList(start, length, entitys);

            return Json(res);
        }

        /// <summary>
        /// Ejecuta los procesos de clasificación, depuración y categorización del tipo de perfil
        /// </summary>
        /// <param name="dataFiltros">Datos de los filtros seleccionados</param>
        /// <returns></returns>
        public JsonResult ReprocesarEjecutar(DepuracionModel dataFiltros)
        {
            object res;
            List<MePtomedicionDTO> entitys = this.servicio.PR03Puntos();

            if (dataFiltros.SelById != null)
            {
                entitys = entitys.Where(x => x.Ptomedicodi.Equals(dataFiltros.SelById)).ToList();
                res = this.servicio.ReprocesarEjecutar(dataFiltros.Fecha,
                    dataFiltros.SelFuente, dataFiltros.SelTipoDemanda, entitys, User.Identity.Name);
                return Json(res);
            }
            if (dataFiltros.SelUbicacion.Count != 0)
            {
                entitys = entitys.Where(x => dataFiltros.SelUbicacion.Contains(x.Areacodi)).ToList();
            }
            if (dataFiltros.SelListTipoEmpresa.Count != 0)
            {
                entitys = entitys.Where(x => dataFiltros.SelListTipoEmpresa.Contains(x.Tipoemprcodi)).ToList();
            }
            if (dataFiltros.SelEmpresa.Count != 0)
            {
                entitys = entitys.Where(x => dataFiltros.SelEmpresa.Contains(x.Emprcodi ?? 0)).ToList();
            }
            if (dataFiltros.SelPuntos.Count != 0)
            {
                entitys = entitys.Where(x => dataFiltros.SelPuntos.Contains(x.Ptomedicodi)).ToList();
            }

            res = this.servicio.ReprocesarEjecutar(dataFiltros.Fecha,
                dataFiltros.SelFuente, dataFiltros.SelTipoDemanda, entitys, User.Identity.Name);

            return Json(res);
        }

        /// <summary>
        /// Actualiza los filtros segun lo seleccionado
        /// </summary>
        /// <param name="dataFiltros"></param>
        /// <returns></returns>
        public JsonResult ReprocesarUpdateFiltros(DepuracionModel dataFiltros)
        {
            PerfilesModel model = new PerfilesModel();

            model.ListPtomedicion = this.servicio.PR03Puntos();
            model.ListEmpresa = this.servicio.PR03Empresas();

            if (dataFiltros.SelById != null)
            {
                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => x.Ptomedicodi.Equals(dataFiltros.SelById)).ToList();

                return Json(model);
            }
            if (dataFiltros.SelUbicacion.Count != 0)
            {
                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => dataFiltros.SelUbicacion.Contains(x.Areacodi)).ToList();
            }
            if (dataFiltros.SelListTipoEmpresa.Count != 0)
            {
                model.ListEmpresa = this.servicio.PR03Empresas().
                    Where(x => dataFiltros.SelListTipoEmpresa.Contains(x.Tipoemprcodi)).ToList();

                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => dataFiltros.SelListTipoEmpresa.Contains(x.Tipoemprcodi)).ToList();
            }
            if (dataFiltros.SelEmpresa.Count != 0)
            {
                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => dataFiltros.SelEmpresa.Contains(x.Emprcodi ?? 0)).ToList();
            }
            if (dataFiltros.SelPuntos.Count != 0)
            {
                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => dataFiltros.SelPuntos.Contains(x.Ptomedicodi)).ToList();
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }

        #endregion

        // -----------------------------------------------------------------------------------------------------------------
        // ASSETEC 14-03-2022 metodos tabla BITACORA
        // -----------------------------------------------------------------------------------------------------------------
        #region Bitacora
        /// <summary>
        /// Registra manualmente la depuracion en la bitacora
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public JsonResult SaveBitacora(int? Emprcodi,
                                       string Medifecha,
                                       List<string> ArrIntervalohras,
                                       List<decimal> ArrConstipico,
                                       List<decimal> ArrConsprevisto,
                                       int Ptomedicodi,
                                       int? Lectcodi,
                                       int? Tipoemprcodi,
                                       string Valor)
        {
            object res = this.servicio.SaveBitacora(Emprcodi,
                                                    Medifecha,
                                                    ArrIntervalohras,
                                                    ArrConstipico,
                                                    ArrConsprevisto,
                                                    Ptomedicodi,
                                                    Lectcodi,
                                                    Tipoemprcodi,
                                                    Valor);

            return Json(res);
        }

        #endregion
        // ------------------------------------ FIN ASSETEC 14-03-2022 -----------------------------------------------------

    }
}
