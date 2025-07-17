using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Models;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using log4net;
using COES.MVC.Intranet.Controllers;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Controllers
{
    public class ConfiguracionController : Controller
    {
        PronosticoDemandaAppServicio servicio = new PronosticoDemandaAppServicio();

        public ActionResult Index()
        {
            return View();
        }

        #region Modulo de Parámetros
        /// <summary>
        /// Inicia el módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Parametros()
        {
            return PartialView();
        }

        /// <summary>
        /// Inicia la opción de parametros por puntos de medición
        /// </summary>
        /// <returns></returns>
        public ActionResult ParametrosPorPuntos()
        {
            ParametrosModel model = new ParametrosModel();
            model.IdModulo = ConstantesProdem.SMParametosByPunto;
            model.FechaIni = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);
            model.Mensaje = "Puede realizar la modificación de los parámetros" +
                " a nivel de PUNTOS DE MEDICIÓN relacionados al proceso de Pronóstico de la demanada";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            List<PrnClasificacionDTO> listProdem = servicio.ListProdemPuntos(0);
            model.ListArea = servicio.GetAreasOperativas();
            model.ListSubestacion = servicio.ListaFiltradaSubestaciones(listProdem);
            model.ListEmpresa = servicio.ListaFiltradaEmpresas(listProdem, 0, 0);
            model.ListPtomedicion = servicio.ListaFiltradaPtoMedicion(listProdem, 0, 0, 0);

            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de parametros por agrupaciones
        /// </summary>
        /// <returns></returns>
        public ActionResult ParametrosPorAgrupaciones()
        {
            ParametrosModel model = new ParametrosModel();
            model.IdModulo = ConstantesProdem.SMParametosByAgrupacion;
            model.FechaIni = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);
            model.Mensaje = "Puede realizar la modificación de los parámetros" +
                " a nivel de AGRUPACIONES relacionadas al proceso de Pronóstico de la demanada";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            List<PrnClasificacionDTO> listProdem = servicio.ListProdemPuntos(0);
            model.ListEmpresa = servicio.ListaFiltradaEmpresas(listProdem, 0, 0);
            model.ListAgrupacion = servicio.ListMeAgrupacion();
            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de parametros por barras
        /// </summary>
        /// <returns></returns>
        public ActionResult ParametrosPorBarras()
        {
            ParametrosModel model = new ParametrosModel();
            model.IdModulo = ConstantesProdem.SMParametosByBarras;
            model.FechaIni = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);
            model.Mensaje = "Puede realizar la modificación de los parámetros" +
                " a nivel de BARRAS PM";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            model.ListBarra = this.servicio.ListPrGrupoBarra();
            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de parametros por áreas
        /// </summary>
        /// <returns></returns>
        public ActionResult ParametrosPorAreas()
        {
            ParametrosModel model = new ParametrosModel();
            model.IdModulo = ConstantesProdem.SMParametosByAreas;
            model.FechaIni = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);
            model.Mensaje = "Puede realizar la modificación de los parámetros" +
                " a nivel de ÁREAS relacionadas al proceso de Pronóstico de la demanada";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            model.ListArea = servicio.GetAreasOperativas();
            return PartialView(model);
        }

        /// <summary>
        /// Inicia la opción de parametros por barras
        /// </summary>
        /// <returns></returns>
        public ActionResult ParametrosPorSA()
        {
            ParametrosModel model = new ParametrosModel();
            model.Mensaje = "Puede realizar la modificación del parámetro SERVICIOS AUXILIARES" +
                " de las BARRAS PM relacionadas al proceso de Pronóstico de la demanada";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            model.ListArea = servicio.GetAreasOperativas();
            model.ListBarraPM = servicio.GetListBarrasPM(ConstantesProdem.Prcatecodi, "0");
            model.ListBarraCP = servicio.GetListBarrasCP(ConstantesProdem.Prcatecodi);
            return PartialView(model);
        }

        /// <summary>
        /// Registra los parametros seleccionados
        /// </summary>
        /// <param name="idModulo"></param>
        /// <param name="dataFiltros"></param>
        /// <param name="dataParametros"></param>
        /// <param name="dataMedicion"></param>
        /// <returns></returns>
        public JsonResult ParametrosSave(int idModulo, ParametrosModel dataFiltros,
            PrnConfiguracionDTO dataParametros, PrnMedicion48DTO dataMedicion)
        {
            object resMesssage = new object();
            ParametrosModel model = new ParametrosModel();

            switch (idModulo)
            {
                case ConstantesProdem.SMParametosByPunto:
                    {
                        this.ParametrosGetFiltros(idModulo, dataFiltros, ref model);
                        DateTime dDesde = DateTime.ParseExact(dataFiltros.FechaIni, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                        DateTime dHasta = DateTime.ParseExact(dataFiltros.FechaFin, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                        resMesssage = this.servicio.ParametrosSave(dDesde, dHasta, dataParametros, model.ListPtomedicion, User.Identity.Name);
                    }
                    break;
                case ConstantesProdem.SMParametosByAgrupacion:
                    {
                        this.ParametrosGetFiltros(idModulo, dataFiltros, ref model);
                        DateTime dDesde = DateTime.ParseExact(dataFiltros.FechaIni, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                        DateTime dHasta = DateTime.ParseExact(dataFiltros.FechaFin, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                        resMesssage = this.servicio.ParametrosSave(dDesde, dHasta, dataParametros, model.ListPtomedicion, User.Identity.Name);
                    }
                    break;
                case ConstantesProdem.SMParametosByAreas:
                    {
                        this.ParametrosGetFiltros(idModulo, dataFiltros, ref model);
                        DateTime dDesde = DateTime.ParseExact(dataFiltros.FechaIni, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                        DateTime dHasta = DateTime.ParseExact(dataFiltros.FechaFin, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                        resMesssage = this.servicio.ParametrosSave(dDesde, dHasta, dataParametros, model.ListPtomedicion, User.Identity.Name);
                    }
                    break;
            }

            return Json(resMesssage);
        }

        /// <summary>
        /// Registra los parametros seleccionados para las barras
        /// </summary>
        /// <param name="dataFiltros"></param>
        /// <param name="dataParametros"></param>
        /// <returns></returns>
        public JsonResult ParametrosBarrasSave(ParametrosModel dataFiltros, PrnConfigbarraDTO dataParametros)
        {
            object resMesssage = new object();
            DateTime dDesde = DateTime.ParseExact(dataFiltros.FechaIni, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dHasta = DateTime.ParseExact(dataFiltros.FechaFin, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            resMesssage = this.servicio.ParametrosBarrasSave(dDesde, dHasta, dataParametros, dataFiltros.SelBarra, User.Identity.Name);

            return Json(resMesssage);
        }

        /// <summary>
        /// Lista el detalle de la tabla segun filtros
        /// </summary>
        /// <param name="idModulo"></param>
        /// <param name="dataFiltros"></param>
        /// <returns></returns>
        public JsonResult ParametrosList(int start, int length, int idModulo, ParametrosModel dataFiltros)
        {
            object res = new object();
            ParametrosModel model = new ParametrosModel();
            switch (idModulo)
            {
                case ConstantesProdem.SMParametosByPunto:
                    {
                        this.ParametrosGetFiltros(idModulo, dataFiltros, ref model);
                        res = this.servicio.ParametrosList(start, length, dataFiltros.FechaIni, dataFiltros.FechaFin,
                            ConstantesProdem.DefectoByPunto, model.ListPtomedicion);
                    }
                    break;
                case ConstantesProdem.SMParametosByAgrupacion:
                    {
                        this.ParametrosGetFiltros(idModulo, dataFiltros, ref model);
                        res = this.servicio.ParametrosList(start, length, dataFiltros.FechaIni, dataFiltros.FechaFin,
                            ConstantesProdem.DefectoByAgrupacion, model.ListPtomedicion);
                    }
                    break;
                case ConstantesProdem.SMParametosByAreas:
                    {
                        this.ParametrosGetFiltros(idModulo, dataFiltros, ref model);
                        res = this.servicio.ParametrosList(start, length, dataFiltros.FechaIni, dataFiltros.FechaFin,
                            ConstantesProdem.DefectoByArea, model.ListPtomedicion);
                    }
                    break;
                case ConstantesProdem.SMParametosByBarras:
                    {
                        res = this.servicio.ParametrosBarrasList(start, length, dataFiltros.FechaIni,
                            dataFiltros.FechaFin, dataFiltros.SelBarra);
                    }

                    break;
            }

            return Json(res);
        }

        /// <summary>
        /// Lista el detalle de la tabla segun filtros
        /// </summary>
        /// <param name="dataFiltros"></param>
        /// <returns></returns>
        public JsonResult ParametrosSAList(ParametrosModel dataFiltros)
        {
            object res = new object();
            res = this.servicio.ParametrosSAData(dataFiltros.SelBarraPM, dataFiltros.SelBarraCP);
            //var res = servicio.PerdidasTransversalesCPDisponibles("0");
            return Json(res);
        }

        /// <summary>
        /// Lista de barrasCp que servirán para filtrar las de pm
        /// </summary>
        /// <param name="barracp"></param>
        /// <returns></returns>
        public JsonResult ParametrosSAUpdateBarraPM(List<int> barracp)
        {
            ParametrosModel model = new ParametrosModel();
            string inbarraCP = "";
            if (barracp.Count == 0)
            {
                inbarraCP = "0";
            }
            else
            {

                inbarraCP = string.Join(",", barracp);
            }
            model.ListBarraPM = servicio.GetListBarrasPM(ConstantesProdem.Prcatecodi, inbarraCP);

            var JsonResult = Json(model.ListBarraPM);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Registra los datos
        /// </summary>
        /// <param name="dataFiltros"></param>
        /// <returns></returns>
        public JsonResult ParametrosSASave(ParametrosModel dataFiltros)
        {
            object res = new object();

            for (int i = 0; i < dataFiltros.id.Count() - 1; i++)
            {
                //Se elminan los registros
                this.servicio.DeletePrnMediciongrpSA(dataFiltros.id[i], ConstantesProdem.PrnmgrtServicioAuxiliar);
                this.servicio.ParametrosSASave(dataFiltros.id[i], dataFiltros.data[i], User.Identity.Name);
            }

            return Json(res);
        }

        /// <summary>
        /// Actualiza los filtros segun lo seleccionado
        /// </summary>
        /// <param name="idModulo"></param>
        /// <param name="dataFiltros"></param>
        /// <returns></returns>
        public JsonResult ParametrosUpdateFiltros(int idModulo, ParametrosModel dataFiltros)
        {
            ParametrosModel model = new ParametrosModel();
            this.ParametrosGetFiltros(idModulo, dataFiltros, ref model);
            return Json(model);
        }

        /// <summary>
        /// Obtiene los parámetros por defecto
        /// </summary>
        /// <returns></returns>
        public JsonResult ParametrosGetDefecto(int idModulo)
        {
            int idDefecto = 0;
            if (idModulo == ConstantesProdem.SMParametosByPunto) idDefecto = ConstantesProdem.DefectoByPunto;
            if (idModulo == ConstantesProdem.SMParametosByAgrupacion) idDefecto = ConstantesProdem.DefectoByAgrupacion;
            if (idModulo == ConstantesProdem.SMParametosByAreas) idDefecto = ConstantesProdem.DefectoByArea;
            PrnConfiguracionDTO entity = this.servicio.ParametrosGetDefecto(idDefecto);
            return Json(entity);
        }

        /// <summary>
        /// Obtiene los parámetros por defecto de las barras
        /// </summary>
        /// <returns></returns>
        public JsonResult ParametrosBarrasGetDefecto()
        {
            PrnConfigbarraDTO entity = this.servicio.ParametrosBarrasGetDefecto();
            return Json(entity);
        }

        /// <summary>
        /// Actualiza los parámetros por defecto
        /// </summary>
        /// <param name="idModulo"></param>
        /// <param name="dataParametros"></param>
        /// <returns></returns>
        public JsonResult ParametrosUpdateDefecto(int idModulo, PrnConfiguracionDTO dataParametros)
        {
            int idDefecto = 0;
            if (idModulo == ConstantesProdem.SMParametosByPunto) idDefecto = ConstantesProdem.DefectoByPunto;
            if (idModulo == ConstantesProdem.SMParametosByAgrupacion) idDefecto = ConstantesProdem.DefectoByAgrupacion;
            if (idModulo == ConstantesProdem.SMParametosByAreas) idDefecto = ConstantesProdem.DefectoByArea;
            object resMesssage = this.servicio.ParametrosUpdateDefecto(idDefecto, dataParametros, User.Identity.Name);
            return Json(resMesssage);
        }

        /// <summary>
        /// Actualiza los parámetros por defecto de las barras
        /// </summary>
        /// <param name="dataParametros"></param>
        /// <returns></returns>
        public JsonResult ParametrosBarrasUpdateDefecto(PrnConfigbarraDTO dataParametros)
        {
            object resMesssage = this.servicio.ParametrosBarrasUpdateDefecto(dataParametros, User.Identity.Name);
            return Json(resMesssage);
        }

        /// <summary>
        /// Obtiene los filtros actualizados
        /// </summary>
        /// <param name="idModulo"></param>
        /// <param name="dataFiltros"></param>
        public void ParametrosGetFiltros(int idModulo, ParametrosModel dataFiltros, ref ParametrosModel dataModelo)
        {
            switch (idModulo)
            {
                case ConstantesProdem.SMParametosByPunto:
                    {
                        dataModelo.ListPtomedicion = servicio.ListProdemPuntos(0);
                        if (dataFiltros.SelById != null)
                        {
                            dataModelo.ListPtomedicion = dataModelo.ListPtomedicion.
                                Where(x => x.Ptomedicodi.Equals(dataFiltros.SelById)).ToList();
                            return;
                        }
                        if (dataFiltros.SelSubestacion.Count != 0)
                        {
                            dataModelo.ListPtomedicion = dataModelo.ListPtomedicion.
                                Where(x => dataFiltros.SelSubestacion.Contains(x.Areacodi)).ToList();
                        }
                        if (dataFiltros.SelEmpresa.Count != 0)
                        {
                            dataModelo.ListPtomedicion = dataModelo.ListPtomedicion.
                                Where(x => dataFiltros.SelEmpresa.Contains(x.Emprcodi)).ToList();
                        }
                        if (dataFiltros.SelPuntos.Count != 0)
                        {
                            dataModelo.ListPtomedicion = dataModelo.ListPtomedicion.
                                Where(x => dataFiltros.SelPuntos.Contains(x.Ptomedicodi)).ToList();
                        }
                    }
                    break;
                case ConstantesProdem.SMParametosByAgrupacion:
                    {
                        dataModelo.ListAgrupacion = servicio.ListMeAgrupacion();
                        if (dataFiltros.SelById != null)
                        {
                            dataModelo.ListAgrupacion = dataModelo.ListAgrupacion.
                                Where(x => x.Ptomedicodi.Equals(dataFiltros.SelById)).ToList();
                            ConvertMePtomedicionToPrnClasificacion(idModulo, ref dataModelo);
                            return;
                        }
                        if (dataFiltros.SelEmpresa.Count != 0)
                        {
                            dataModelo.ListAgrupacion = dataModelo.ListAgrupacion.
                                Where(x => dataFiltros.SelEmpresa.Contains((int)x.Emprcodi)).ToList();
                        }
                        if (dataFiltros.SelAgrupacion.Count != 0)
                        {
                            dataModelo.ListAgrupacion = dataModelo.ListAgrupacion.
                                Where(x => dataFiltros.SelAgrupacion.Contains(x.Ptomedicodi)).ToList();
                        }
                        ConvertMePtomedicionToPrnClasificacion(idModulo, ref dataModelo);
                    }
                    break;
                case ConstantesProdem.SMParametosByAreas:
                    {
                        dataModelo.ListArea = servicio.GetAreasOperativas();
                        if (dataFiltros.SelById != null)
                        {
                            dataModelo.ListArea = dataModelo.ListArea.
                                Where(x => x.Ptomedicodi.Equals(dataFiltros.SelById)).ToList();
                            ConvertMePtomedicionToPrnClasificacion(idModulo, ref dataModelo);
                            return;
                        }
                        if (dataFiltros.SelAreas.Count != 0)
                        {
                            dataModelo.ListArea = dataModelo.ListArea.
                                Where(x => dataFiltros.SelAreas.Contains(x.Ptomedicodi)).ToList();
                        }
                        ConvertMePtomedicionToPrnClasificacion(idModulo, ref dataModelo);
                    }
                    break;
                case ConstantesProdem.SMParametosByBarras:

                    break;
            }
        }

        /// <summary>
        /// Convierte una lista de MePtomedicion a PrnClasificacion
        /// </summary>
        /// <param name="dataModelo"></param>
        public void ConvertMePtomedicionToPrnClasificacion(int idModulo, ref ParametrosModel dataModelo)
        {
            PrnClasificacionDTO tempEntity = new PrnClasificacionDTO();
            List<PrnClasificacionDTO> tempEntitys = new List<PrnClasificacionDTO>();

            if (idModulo == ConstantesProdem.SMParametosByAgrupacion)
            {
                foreach (var item in dataModelo.ListAgrupacion)
                {
                    tempEntity = new PrnClasificacionDTO();
                    tempEntity.Ptomedicodi = item.Ptomedicodi;
                    tempEntity.Ptomedidesc = item.Ptomedidesc;
                    tempEntitys.Add(tempEntity);
                }
                dataModelo.ListPtomedicion = tempEntitys;
            }
            if (idModulo == ConstantesProdem.SMParametosByAreas)
            {
                foreach (var item in dataModelo.ListArea)
                {
                    tempEntity = new PrnClasificacionDTO();
                    tempEntity.Ptomedicodi = item.Ptomedicodi;
                    tempEntity.Ptomedidesc = item.Ptomedidesc;
                    tempEntitys.Add(tempEntity);
                }
                dataModelo.ListPtomedicion = tempEntitys;
            }
        }

        #endregion

        #region Módulo de Variables Exógenas
        [HttpPost]
        public ActionResult Exogenas()
        {

            ExogenasModel model = new ExogenasModel();

            model.FechaIni = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);
            model.Mensaje = "Puede realizar la consulta de las variables exógenas obtenidas";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            model.ListaCiudad = servicio.ListVarexoCiudad().OrderBy(x => x.Areacodi).ToList();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ExogenaList(string fecini, string fecfin, string[] ciud)
        {
            object res = this.servicio.ExogenaModelo(fecini, fecfin, ciud);
            return Json(res);
        }

        /// <summary>
        /// Obtiene y registra las variables exogenas desde la api Weather Wunderground
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarExogena()
        {
            ExogenasModel model = new ExogenasModel();
            try
            {
                this.servicio.ObtenerVariablesExogenas();
                model.TipoMensaje = ConstantesProdem.MsgSuccess;
                model.Mensaje = "Proceso de obtención de variables exógenas exitoso!";
            }
            catch (Exception e)
            {
                model.TipoMensaje = ConstantesProdem.MsgError;
                model.Mensaje = e.Message;
                return Json(model);
            }

            return Json(model);
        }

        #endregion

        #region Módulo de Motivos
        [HttpPost]
        public ActionResult Motivos()
        {
            MotivosModel model = new MotivosModel();
            //model.IdModulo = ConstantesProdem2.SMParametosByBarras;
            model.Mensaje = "Puede realizar el registro de Motivos" +
                " al proceso de Pronóstico de la demanda";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            return PartialView(model);
        }

        public JsonResult MotivoList()
        {
            MotivosModel model = new MotivosModel();
            model.ListaMotivos = this.servicio.ListaMotivo();
            return Json(model.ListaMotivos);
        }

        public JsonResult MotivoSave(string nombre, string abrev)
        {
            string msg = string.Empty;
            EveSubcausaeventoDTO entidad = new EveSubcausaeventoDTO();

            entidad.Causaevencodi = ConstantesProdem.VarExoPronostico;
            entidad.Subcausadesc = nombre;
            entidad.Subcausaabrev = abrev;

            this.servicio.SaveMotivo(entidad);
            return Json(msg);
        }


        public JsonResult MotivoUpdate(int id, string nombre, string abrev)
        {
            string msg = string.Empty;
            EveSubcausaeventoDTO entidad = new EveSubcausaeventoDTO();

            entidad.Causaevencodi = ConstantesProdem.VarExoPronostico;
            entidad.Subcausacodi = id;
            entidad.Subcausadesc = nombre;
            entidad.Subcausaabrev = abrev;

            this.servicio.UpdateMotivo(entidad);
            return Json(msg);
        }
        #endregion}

        #region Módulo de Formulas
        public ActionResult Formulas()
        {

            FormulasModel model = new FormulasModel();
            List<PrnClasificacionDTO> listaProdem = new List<PrnClasificacionDTO>();
            listaProdem = this.servicio.ListProdemPuntos(0);

            // Log.Info("Lista Areas Operativas por Nivel - ListPrnArea");
            model.ListaAreaOperativa = this.servicio.ListPrnArea(ConstantesProdem.NvlAreaOperativa);
            model.ListaEmpresa = this.servicio.ListaFiltradaEmpresas(listaProdem, ConstantesProdem.TipoemprcodiDistribuidores, 0);
            model.ListaPtomedicion = this.servicio.ListaFiltradaPtoMedicion(listaProdem, 0, ConstantesProdem.TipoemprcodiDistribuidores, 0);
            model.ListaAgrupacion = this.servicio.ListMeAgrupacion();
            model.Mensaje = "Puede realizar la creacion de formulas.";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            return View(model);

        }

        /// <summary>
        /// Muestra las formulas relacionadas y no relacionadas al punto de medición
        /// </summary>
        /// <param name="idPunto"></param>
        /// <returns></returns>
        public JsonResult FormulaDetalle(int idPunto)
        {
            FormulasModel model = new FormulasModel();
            //  Log.Info("Lista Formulas Relacionadas - ListFormulasRelacionadas");
            model.DtSeleccionado = this.servicio.ListFormulasRelacionadas(idPunto);
            // Log.Info("Lista Formulas No Relacionadas - FormulasRestantesByLista");
            model.DtTodos = this.servicio.FormulasRestantesByLista(model.DtSeleccionado);

            return Json(model);
        }

        /// <summary>
        /// Actualiza los fitros de busqueda
        /// </summary>
        /// <param name="idArea"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FormulaUpdateList(int idArea, int idEmpresa, int idTipo)//ActualizarListas
        {
            FormulasModel model = new FormulasModel();
            List<PrnClasificacionDTO> listaProdem = new List<PrnClasificacionDTO>();
            listaProdem = this.servicio.ListProdemPuntos(idArea);

            if (idTipo == 1)//Distribuidores
            {
                model.ListaEmpresa = this.servicio.ListaFiltradaEmpresas(listaProdem, ConstantesProdem.TipoemprcodiDistribuidores, 0);
                model.ListaPtomedicion = this.servicio.ListaFiltradaPtoMedicion(listaProdem, 0, ConstantesProdem.TipoemprcodiDistribuidores, idEmpresa);
            }
            else//Usuarios Libres agrupados
            {
                model.ListaEmpresa = this.servicio.ListaFiltradaEmpresas(listaProdem, ConstantesProdem.TipoemprcodiUsuLibres, 0);
                model.ListaAgrupacion = this.servicio.ListMeAgrupacion();
                if (idEmpresa != 0)
                {
                    model.ListaAgrupacion = model.ListaAgrupacion.Where(x => x.Emprcodi == idEmpresa).ToList();
                }
            }

            return Json(model);
        }

        /// <summary>
        /// Registra las formulas relacionadas al punto de medición
        /// </summary>
        /// <param name="idPunto"></param>
        /// <param name="listaFormulas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FormulaSave(int idPunto, List<PrnFormularelDTO> listaFormulas)
        {
            // base.ValidarSesionUsuario();
            string m = string.Empty;
            string u = User.Identity.Name.ToString();
            List<PrnFormularelDTO> Formulas = this.servicio.ListFormulasRelacionadas(idPunto);
            //  Log.Info("Registra la Relación - DemandaAreaFlujoSave");
            m = this.servicio.DemandaAreaFlujoSave(idPunto, listaFormulas, Formulas, u);

            return Json(m);
        }

        #endregion

        #region Módulo de Agrupaciones

        public ActionResult Agrupaciones()
        {
            AgrupacionesModel model = new AgrupacionesModel();
            model.ListArea = UtilProdem.ListAreaOperativa(false);
            model.ListEmpresa = this.servicio.ListEmpresasPR03(ConstantesProdem.RegStrTodos,
                ConstantesProdem.RegStrTodos);
            model.ListAgrupacion = this.servicio.ListAgrupacionesActivas(ConstantesProdem.RegStrTodos,
                ConstantesProdem.RegStrTodos, ConstantesProdem.RegStrTodos, 1);

            model.Mensaje = "Puede realizar la creacion y actualización de agrupaciones.";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            return View(model);
        }

        /// <summary>
        /// Muestra las agrupaciones registradas y activas
        /// </summary>
        /// <param name="start">Parámetro del DataTable, índice del registro inicial de la página</param>
        /// <param name="length">Parámetro del DataTable, cantidad de registros por página</param>
        /// <param name="dataFiltros">Filtros seleccionados</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgrupacionesList(int start, int length, AgrupacionesModel dataFiltros)
        {
            string idArea = (dataFiltros.SelArea.Count != 0) ? string.Join(",", dataFiltros.SelArea) : "0";
            string idEmpresa = (dataFiltros.SelEmpresa.Count != 0) ? string.Join(",", dataFiltros.SelEmpresa) : "0";
            string idPunto = (dataFiltros.SelAgrupacion.Count != 0) ? string.Join(",", dataFiltros.SelAgrupacion) : "0";

            object res = this.servicio.AgrupacionesList(start, length, idArea, idEmpresa, idPunto, dataFiltros.EsPronostico);
            return Json(res);
        }

        /// <summary>
        /// Edita una agrupación activa existente
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION corresponde a una agrupación</param>
        /// <returns></returns>
        public JsonResult AgrupacionesData(int idPunto)
        {
            object res = this.servicio.AgrupacionesData(idPunto);
            var JsonResult = Json(res);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        /// <summary>
        /// Registra o actualiza una agrupación
        /// </summary>
        /// <param name="selPuntos">Puntos de medición de medición seleccionados para la agrupación</param>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION corresponde a una agrupación</param>
        /// <param name="idArea">Identificador de la tabla EQ_AREA corresponde a un área operativa</param>
        /// <param name="esPronostico">Flag que indica si la agrupación participa del pronóstico por áreas</param>
        /// <param name="nomAgrupacion">Nombre de la agrupación a registrar o actualizar</param>
        /// <param name="idAgrupacion">Identificador de la tabla PRN_PUNTOAGRUPACION representa una agrupación</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgrupacionesSave(int[] selPuntos, int idPunto, int idArea, int esPronostico, string nomAgrupacion, int idAgrupacion)
        {
            object res = this.servicio.AgrupacionesSave(selPuntos,
                idPunto, idArea, esPronostico, nomAgrupacion, idAgrupacion, User.Identity.Name);
            return Json(res);
        }

        /// <summary>
        /// Da de baja a una agrupación
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION corresponde a una agrupación</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgrupacionesDelete(int idPunto)//ELiminar//DeleteAgrupacion
        {
            string typeMsg = ConstantesProdem.MsgSuccess;
            string dataMsg = "Registro eliminado";

            MePtomedicionDTO entity = this.servicio.GetByIdMePtomedicion(idPunto);
            if (entity.Ptomedicodi != 0) entity.Ptomediestado = "B";
            this.servicio.UpdatePtomedicion(entity);
            return Json(new { typeMsg, dataMsg });
        }

        /// <summary>
        /// Actualiza los filtros segun criterios
        /// </summary>
        /// <param name="dataFiltros">Filtros seleccionados</param>
        /// <returns></returns>
        public JsonResult AgrupacionesUpdateFiltros(AgrupacionesModel dataFiltros)
        {
            AgrupacionesModel model = new AgrupacionesModel();
            string idArea = (dataFiltros.SelArea.Count != 0) ? string.Join(",", dataFiltros.SelArea) : "0";
            string idEmpresa = (dataFiltros.SelEmpresa.Count != 0) ? string.Join(",", dataFiltros.SelEmpresa) : "0";

            model.ListAgrupacion = this.servicio.ListAgrupacionesActivas(idArea,
                ConstantesProdem.RegStrTodos, idEmpresa, dataFiltros.EsPronostico);

            return Json(model);
        }

        #endregion

        #region Módulo de Perfiles

        /// <summary>
        /// Inicia la opción de perfiles
        /// </summary>
        /// <returns></returns>
        public ActionResult Perfiles()
        {
            PerfilesModel model = new PerfilesModel();
            model.Mensaje = "Puede consultar los perfiles patrón y actualizar los perfiles defecto.";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);

            model.ListUbicacion = this.servicio.PR03Ubicaciones();
            model.ListTipoEmpresa = UtilProdem.ListTipoEmpresa();
            model.ListEmpresa = this.servicio.PR03Empresas();
            model.ListPtomedicion = this.servicio.PR03Puntos();

            return View(model);
        }

        /// <summary>
        /// Carga los datos y les da formato para la presentación
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public JsonResult PerfilesDatos(int idPunto, string regFecha)
        {
            object res;
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            res = this.servicio.PerfilesDatos(idPunto, parseFecha);

            return Json(res);
        }

        /// <summary>
        /// Actualiza los filtros segun lo seleccionado
        /// </summary>
        /// <param name="dataFiltros"></param>
        /// <returns></returns>
        public JsonResult PerfilesUpdateFiltros(PerfilesModel dataFiltros)
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
            if (dataFiltros.SelTipoEmpresa.Count != 0)
            {
                model.ListEmpresa = this.servicio.PR03Empresas().
                    Where(x => dataFiltros.SelTipoEmpresa.Contains(x.Tipoemprcodi)).ToList();

                model.ListPtomedicion = model.ListPtomedicion.
                    Where(x => dataFiltros.SelTipoEmpresa.Contains(x.Tipoemprcodi)).ToList();
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
        public JsonResult PerfilesUpdatePatron(int idPunto, string regFechaA, string regFechaB,
            bool esLunes, string tipoPatron, decimal? dsvPatron, List<decimal[]> dataMediciones)
        {
            object res;
            res = this.servicio.PerfilesUpdatePatron(idPunto, regFechaA, regFechaB,
                esLunes, tipoPatron, dsvPatron, dataMediciones);
            return Json(res);
        }

        /// <summary>
        /// Registra el nuevo perfil defecto
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro (solo valida el día de la semana)</param>
        /// <param name="dataMedicion">Datos de la nueva medición defecto</param>
        /// <returns></returns>
        public JsonResult PerfilesSave(int idPunto, string regFecha, PrnMedicion48DTO dataMedicion)
        {
            object res = this.servicio.PerfilesSave(idPunto, regFecha, dataMedicion);
            return Json(res);
        }

        /// <summary>
        /// Metodo para exportar el perfil patron
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult PerfilesPatronExportar(string fechaInicio, string fechaFin)
        {
            string ruta = this.servicio.PerfilesPatronExcel(fechaInicio, fechaFin);
            return Json(ruta);

        }

        /// <summary>
        /// Metodo para exportar el perfil patron
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult PerfilesPatronExportarporFecha(string fechaInicio, string fechaFin)
        {
            string ruta = this.servicio.PerfilesPatronExcelPorFecha(fechaInicio, fechaFin);
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

        #region Perdidas Transversales
        [HttpPost]
        public ActionResult PerdidasTransversales()
        {
            PerdidasTransversalesModel model = new PerdidasTransversalesModel();

            model.ListBarraCPTransversales = servicio.ListaBarrasPerdidasTransversales();

            return PartialView(model);
        }

        /// <summary>
        /// Obtiene la lista de formulas
        /// </summary>
        /// <returns></returns>
        public JsonResult ListarFormulas()
        {
            List<MePerfilRuleDTO> entidades = servicio.ListaFormulas();

            return Json(entidades);
        }

        /// <summary>
        /// Lista las barras registradas con su perdida transversal en la tabla PRN_PRDTRANSVERSAL
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult ListarPerdidasTransversales()
        {
            List<PrnPrdTransversalDTO> entidades = servicio.ListPerdidasTransvBarraFormulas();

            return Json(entidades);
        }

        /// <summary>
        /// Obtiene los datos de las listas y tablas del popup de las relaciones de perdidas transversales con las barras y formulas
        /// </summary>
        /// <param name="idPrGrupo">Identificador de la barra</param>
        /// <returns>JsonResult</returns>
        public JsonResult ListarRelacionesPerdidasTransv(int idPerdTransv, string nomPerdTransv)
        {
            object res = servicio.ListarRelacionesPerdidasTransvBarraFormula(idPerdTransv, nomPerdTransv);

            return Json(res);
        }

        /// <summary>
        /// Registra las relaciones formadas en el popup
        /// </summary>
        /// <param name="nombPerdida">Nombre de la perdida</param>
        /// <param name="listaSeleccionados">Lista de registros seleccionados para la relación</param>
        /// <returns>JsonResult</returns>
        public JsonResult RegistrarRelacionPerdidaTransv(string nombPerdida, List<PrnPrdTransversalDTO> listaSeleccionados)
        {
            object res = servicio.SavePerdidaTransversal(nombPerdida, listaSeleccionados);

            return Json(res);
        }

        /// <summary>
        /// Eliminar las perdidas transversales
        /// </summary>
        /// <param name="nombPerdida">Nombre de la perdida</param>
        /// <returns></returns>
        public JsonResult EliminarRelacionPerdidaTransv(string nombPerdida)
        {
            object res = servicio.DeletePerdidaTransversal(nombPerdida);

            return Json(res);
        }
        #endregion

        #region Servicios Auxiliares
        /// <summary>
        /// Inicia el módulo de pruebas
        /// </summary>
        /// <returns></returns>
        public ActionResult ServiciosAuxiliares()
        {
            PronosticoModel model = new PronosticoModel();
            model.Mensaje = "Modulo de Servicios Auxiliares para la creación de relaciones entre Barras y Formulas";
            model.TipoMensaje = "info";

            return PartialView(model);
        }

        /// <summary>
        /// Obtiene la lista de barras a mostrar en la tabla principal
        /// </summary>
        /// <returns></returns>
        public JsonResult ListarBarrasFormulas()
        {
            List<PrnServiciosAuxiliaresDTO> entidades = servicio.ListarBarrasFormulas();

            return Json(entidades);
        }

        /// <summary>
        /// Obtiene los datos de las listas y tablas del popup de relación
        /// </summary>
        /// <param name="idPrGrupo">Identificador de la barra</param>
        /// <returns></returns>
        public JsonResult ListarRelaciones(int idPrGrupo)
        {
            object res = servicio.ListarRelacionesBarraFormula(idPrGrupo);

            return Json(res);
        }

        /// <summary>
        /// Registra las relaciones formadas en el popup
        /// </summary>
        /// <param name="listaSeleccionados">Lista de registros seleccionados para la relación</param>
        /// <returns></returns>
        public JsonResult RegistrarRelacion(int idPrGrupo, List<PrnServiciosAuxiliaresDTO> listaSeleccionados)
        {
            object res = servicio.SaveServicioAuxiliar(idPrGrupo, listaSeleccionados);

            return Json(res);
        }
        #endregion

        #region Agrupaciones de UL - Fórmulas
        /// <summary>
        /// Inicia la opción para relacionar Agrupaciones de UL con formulas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AgrupacionULFormula()
        {
            AgrupacionULFormulaModel model = new AgrupacionULFormulaModel();
            model.Mensaje = "Puede consultar las relaciones entre las agrupaciones de usuarios libres y fórmulas";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            return PartialView(model);
        }

        /// <summary>
        /// Lista las agrupaciones de usuarios libres 
        /// </summary>
        /// <returns></returns>
        public JsonResult AgrupacionUsuariosLibresList(int start, int length)
        {
            object data = this.servicio.ListAgrupacionesUsuariosLibres(start, length);

            return Json(data);
        }

        /// <summary>
        /// LIsta las formulas seleccionadas y disponibles para el popup
        /// </summary>
        /// <param name="agrupacion"></param>
        /// <returns></returns>
        public JsonResult ListFormulasByAgrupacionPopUp(int agrupacion)
        {
            AgrupacionULFormulaModel model = new AgrupacionULFormulaModel();

            model.DtSeleccionados = this.servicio.ListFormulasSeleccionadas(agrupacion);
            model.DtDisponibles = this.servicio.ListFormulasDisponibles(agrupacion);

            return Json(model);
        }

        /// <summary>
        /// Registra los flujos de linea relacionados al área operativa
        /// </summary>
        /// <param name="idAgrupacion">Identificador de la Agrupacion</param>
        /// <param name="listFormulas">Lista de las formulas relacionadas a la agrupacion</param>
        /// <returns></returns>
        public JsonResult SaveAgrupacionULFormulas(int idAgrupacion, List<PrnAgrupacionFormulasDTO> listFormulas)
        {
            object res = this.servicio.AgrupacionFormulasSave(idAgrupacion, listFormulas);

            return Json(res);
        }
        #endregion
    }
}
