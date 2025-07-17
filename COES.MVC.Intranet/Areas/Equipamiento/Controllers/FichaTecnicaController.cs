using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using COES.Framework.Base.Tools;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FichaTecnicaController : BaseController
    {
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();
        IEODAppServicio servIeod = new IEODAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FichaTecnicaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        #region Configuración de Ficha Técnica

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Configuracion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            //if (ValidarSesion && !base.IsValidSesion) return base.RedirectToLogin();

            FichaTecnicaModel model = new FichaTecnicaModel();

            model.ListaFamilia = this.servIeod.ListarFamilia();
            model.ListaCategoria = this.servFictec.ListarCategoriaGrupoXCatecodi(ConstantesMigraciones.CatecodiParametroFiltro);
            model.ListaFichaPropiedad = this.servFictec.ListFtFictecProps();

            //Inicializa listado del detalle
            model.ListaFichaTecnicaPadre = new List<FtFictecXTipoEquipoDTO>();
            model.ListaEstado = Util.ListaEstado();
            model.ListaNota = new List<FtFictecNotaDTO>();

            return View(model);
        }

        /// <summary>
        /// Lista de ficha tecnica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaTecnicaLista()
        {
            FichaTecnicaModel model = new FichaTecnicaModel();

            try
            {
                model.ListaFichaTecnica = this.servFictec.ListarFichaTecnica(ConstantesAppServicio.ParametroDefecto);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                //model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Formulario de Ficha Tecnica (Tree)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult FichaTecnicaFormulario(int id)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.FichaTecnica = new FtFictecXTipoEquipoDTO();
                model.ListaFamilia = this.servIeod.ListarFamilia();
                model.ListaCategoria = this.servFictec.ListarCategoriaGrupoXCatecodi(ConstantesMigraciones.CatecodiParametroFiltro);
                model.FichaTecnica.Origen = ConstantesFichaTecnica.OrigenTipoEquipo;
                model.ListaFichaTecnicaPadre = new List<FtFictecXTipoEquipoDTO>();
                model.NumOrigenpadre = 0;
                model.ListaEstado = Util.ListaEstado();
                model.ListaNota = new List<FtFictecNotaDTO>();

                if (id > 0)
                {
                    model.FichaTecnica = this.servFictec.GetFichaTecnica(id);
                    model.ListaFichaTecnicaPadre = this.servFictec.ListarFichaTecnicaPadre(model.FichaTecnica.Origen, model.FichaTecnica.Catecodi, model.FichaTecnica.Famcodi)
                                                                .Where(x => x.Fteqcodi != id).ToList();

                    model.ListaNota = this.servFictec.ListFtFictecNotaByFteqcodi(id);
                }
                List<FtFictecItemDTO> listaItems, listaAllItems;
                List<TreeItemFichaTecnica> listaItemsJson;

                FTFiltroReporteExcel objFiltro = servFictec.GetFichaYDatosXEquipoOModo(id, -2, false, ConstantesFichaTecnica.INTRANET, DateTime.Today);
                servFictec.ListarTreeItemsFichaTecnica(objFiltro, out listaAllItems, out listaItems, out listaItemsJson);
                model.ListaItems = listaItems;
                model.ListaItemsJson = listaItemsJson;
                model.ListaAllItems = listaAllItems;

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                StringBuilder jsonTree = new StringBuilder();
                serializer.Serialize(model.ListaItemsJson, jsonTree);
                model.TreeJson = jsonTree.ToString();

                StringBuilder jsonNota = new StringBuilder();
                serializer.Serialize(model.ListaNota, jsonNota);
                model.NotaJson = jsonNota.ToString();

                model.ListaConcepto = servFictec.ListarTodoPrConceptos();
                model.ListaPropiedad = servFictec.ListarTodoEqPropiedades();


                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                //model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Registrar/editar ficha tecnica
        /// </summary>
        /// <param name="dataTree"></param>
        /// <param name="idFT"></param>
        /// <param name="catecodi"></param>
        /// <param name="famcodi"></param>
        /// <param name="nombre"></param>
        /// <param name="tipoOrigen"></param>
        /// <param name="idFTPadre"></param>
        /// <param name="estado"></param>
        /// <param name="esVisibleExt"></param>
        /// <param name="fecVigenciaExt"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaTecnicaGuardar(string dataTree, int idFT, int? catecodi, int? famcodi, string nombre, int tipoOrigen, int? idFTPadre, string estado, string esVisibleExt, string fecVigenciaExt)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                List<TreeItemFichaTecnica> objData = serialize.Deserialize<List<TreeItemFichaTecnica>>(dataTree);

                //Validaciones
                if (nombre == null || nombre.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar nombre.");
                }

                idFTPadre = idFTPadre.GetValueOrDefault(0) > 0 ? idFTPadre : null;

                switch (tipoOrigen)
                {
                    case ConstantesFichaTecnica.OrigenTipoEquipo:
                        if (famcodi.GetValueOrDefault(0) <= 0)
                        {
                            throw new Exception("Debe seleccionar un Tipo de Equipo.");
                        }
                        catecodi = null;
                        break;
                    case ConstantesFichaTecnica.OrigenCategoriaGrupo:
                        if (catecodi.GetValueOrDefault(0) <= 0)
                        {
                            throw new Exception("Debe seleccionar un Categoria de grupo.");
                        }
                        famcodi = null;
                        break;
                }
                if (objData == null || objData.Count == 0)
                {
                    throw new Exception("La configuración debe tener al menos un item.");
                }

                //VALIDAR DUPLICADO POR NOMBRE DE FICHA Y TIPO
                var listaFichas = this.servFictec.ListarFichaTecnica(ConstantesAppServicio.ParametroDefecto);
                listaFichas = idFT > 0 ? listaFichas.Where(x => x.Fteqcodi != idFT).ToList() : listaFichas;

                if (tipoOrigen == ConstantesFichaTecnica.OrigenTipoEquipo)
                {
                    var fichaT = listaFichas.Where(x => x.Fteqnombre.Trim().ToUpper() == nombre.Trim().ToUpper() && x.Famcodi == famcodi.Value).FirstOrDefault();
                    if (fichaT != null)
                        throw new Exception(" Existe Ficha con el mismo nombre y tipo de equipo.");
                }
                if (tipoOrigen == ConstantesFichaTecnica.OrigenCategoriaGrupo)
                {
                    var fichaT = listaFichas.Where(x => x.Fteqnombre.Trim().ToUpper() == nombre.Trim().ToUpper() && x.Catecodi == catecodi.Value).FirstOrDefault();

                    if (fichaT != null)
                        throw new Exception(" Existe Ficha con el mismo nombre y categoría.");
                }


                //Validar que otro hijo no tenga el mismo padre
                if (idFTPadre != null)
                {
                    if (idFT > 0)
                    {
                        FtFictecXTipoEquipoDTO reg = this.servFictec.GetFichaTecnica(idFT);
                        if (reg.Fteqpadre.GetValueOrDefault(0) > 0 && reg.Fteqpadre != idFTPadre)
                        {
                            if (this.servFictec.ExisteFichaTecnicaHijo(idFTPadre.Value, tipoOrigen, catecodi, famcodi))
                            {
                                throw new Exception("Ya existe una configuración de Ficha Técnica con la Ficha Técnica Padre seleccionada.");
                            }
                        }
                    }
                    else
                    {
                        if (this.servFictec.ExisteFichaTecnicaHijo(idFTPadre.Value, tipoOrigen, catecodi, famcodi))
                        {
                            throw new Exception("Ya existe una configuración de Ficha Técnica con la Ficha Técnica Padre seleccionada.");
                        }
                    }
                }

                //Validar que no exista gemelos
                if (idFTPadre != null)
                {
                    var listaFTHijos = servFictec.ListarFichaTecnicaByPadre(idFTPadre.Value);
                    listaFTHijos = idFT > 0 ? listaFTHijos.Where(x => x.Fteqcodi != idFT).ToList() : listaFTHijos;

                    if (tipoOrigen == ConstantesFichaTecnica.OrigenTipoEquipo)
                    {
                        listaFTHijos = listaFTHijos.Where(x => x.Famcodi == famcodi.Value).ToList();

                        if (listaFTHijos.Any())
                            throw new Exception(" La Ficha padre tiene ficha hijo del mismo tipo de equipo.");
                    }
                    else
                    {
                        listaFTHijos = listaFTHijos.Where(x => x.Catecodi == catecodi.Value).ToList();

                        if (listaFTHijos.Any())
                            throw new Exception(" La Ficha padre tiene ficha hijo de la misma categoría.");
                    }
                }

                NotificacionFT fichaCambio = new NotificacionFT();

                //Guardar
                if (idFT > 0)
                {
                    FtFictecXTipoEquipoDTO reg = this.servFictec.GetFichaTecnica(idFT);

                    bool esFichaPerteneceMaestra = false;
                    // obtener ficha técnica de ficha maestra
                    var fichaPrincipal = servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraPortal);
                    if (fichaPrincipal != null)
                    {
                        var listaFictecdetXfm = servFictec.ListarAllFichaTecnicaByMaestra(fichaPrincipal.Fteccodi);
                        esFichaPerteneceMaestra = listaFictecdetXfm.Find(x => x.Fteqcodi == idFT) != null;
                    }

                    if (esFichaPerteneceMaestra)
                    {
                        //valores para notificación
                        fichaCambio.Fteqcodi = reg.Fteqcodi;
                        fichaCambio.Fteqnombre = reg.Fteqnombre;
                        fichaCambio.FteqnombreNew = reg.Fteqnombre != nombre ? nombre : string.Empty;
                        fichaCambio.Fteqestado = Util.EstadoDescripcion(reg.Fteqestado);
                        fichaCambio.FteqestadoNew = reg.Fteqestado != estado ? Util.EstadoDescripcion(estado) : string.Empty;
                        fichaCambio.Fteqfecmodificacion = DateTime.Now;
                        fichaCambio.Ftequsumodificacion = User.Identity.Name;
                    }

                    reg.Fteqnombre = nombre;
                    reg.Fteqpadre = idFTPadre;
                    reg.Fteqfecmodificacion = DateTime.Now;
                    reg.Ftequsumodificacion = User.Identity.Name;
                    reg.Fteqestado = estado;
                    if (esVisibleExt == "S" || esVisibleExt == "N")
                        reg.Fteqflagext = esVisibleExt == "S" ? 1 : (esVisibleExt == "N" ? 0 : 0);
                    else
                        reg.Fteqflagext = null;

                    if (fecVigenciaExt.Trim() != "")
                        reg.Fteqfecvigenciaext = DateTime.ParseExact(fecVigenciaExt, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    else
                        reg.Fteqfecvigenciaext = null;

                    this.servFictec.UpdateFichaTecnica(reg);
                }
                else
                {
                    FtFictecXTipoEquipoDTO reg = new FtFictecXTipoEquipoDTO();
                    reg.Fteqnombre = nombre;
                    reg.Fteqpadre = idFTPadre;
                    reg.Famcodi = famcodi;
                    reg.Catecodi = catecodi;
                    reg.Fteqestado = ConstantesAppServicio.Activo;
                    reg.Fteqfeccreacion = DateTime.Now;
                    reg.Ftequsucreacion = User.Identity.Name;

                    if (esVisibleExt == "S" || esVisibleExt == "N")
                        reg.Fteqflagext = esVisibleExt == "S" ? 1 : (esVisibleExt == "N" ? 0 : 0);
                    else
                        reg.Fteqflagext = null;

                    if (fecVigenciaExt.Trim() != "")
                        reg.Fteqfecvigenciaext = DateTime.ParseExact(fecVigenciaExt, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    else
                        reg.Fteqfecvigenciaext = null;

                    idFT = this.servFictec.SaveFichaTecnica(reg);
                }

                List<NotificacionFTItems> listaCambios = new List<NotificacionFTItems>();

                this.servFictec.GuardarTreeItemsFichaTecnica(idFT, objData, User.Identity.Name, ref listaCambios);

                //Enviar Notificación cambio
                var lstCorreosAdminFT = this.ObtenerListaCorreosAdminFT();
                servFictec.EnviarNotificacionConfiguracionFT(fichaCambio, listaCambios, lstCorreosAdminFT);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Copiar ficha tecnica
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaTecnicaCopiar(int idFT)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaRegistro = DateTime.Now;
                var usuario = User.Identity.Name;
                FtFictecXTipoEquipoDTO reg = this.servFictec.GetFichaTecnica(idFT);

                if (reg == null)
                    throw new Exception("La Ficha Técnica no existe o ha sido eliminada.");

                //GUARDAR COPIA FICHA
                reg.Fteqcodi = 0;
                reg.Fteqnombre = reg.Fteqnombre + "-COPIA";
                reg.Fteqfecmodificacion = null;
                reg.Ftequsumodificacion = null;
                reg.Fteqpadre = null;
                reg.Fteqfeccreacion = fechaRegistro;
                reg.Ftequsucreacion = usuario;
                reg.Fteqflagext = ConstantesFichaTecnica.ValorNo;
                reg.Fteqfecvigenciaext = null;
                var idFTCopia = servFictec.SaveFichaTecnica(reg);

                // GUARDAR COPIA DE NOTA
                var listaNotas = servFictec.ListFtFictecNotaByFteqcodi(idFT);
                foreach (var item in listaNotas)
                {
                    item.Ftnotacodi = 0;
                    item.Ftnotafecmodificacion = null;
                    item.Ftnotausumodificacion = null;
                    item.Ftnotafeccreacion = fechaRegistro;
                    item.Ftnotausucreacion = usuario;
                    servFictec.SaveFtFictecNota(item);
                }

                List<FtFictecItemDTO> listaItems, listaAllItems;
                List<TreeItemFichaTecnica> listaItemsJson;

                FTFiltroReporteExcel objFiltro = servFictec.GetFichaYDatosXEquipoOModo(idFT, -2, false, ConstantesFichaTecnica.INTRANET, DateTime.Today);
                servFictec.ListarTreeItemsFichaTecnica(objFiltro, out listaAllItems, out listaItems, out listaItemsJson);
                servFictec.CopiarFichaTecnica(idFT, idFTCopia, listaItemsJson, usuario);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }


        /// <summary>
        /// Eliminar ficha tecnica
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaTecnicaEliminar(int idFT)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                FtFictecXTipoEquipoDTO reg = this.servFictec.GetFichaTecnica(idFT);

                if (reg == null)
                {
                    throw new Exception("La Ficha Técnica no existe o ha sido eliminada.");
                }

                NotificacionFT fichaCambio = new NotificacionFT();
                List<NotificacionFTItems> listaCambios = new List<NotificacionFTItems>();
                bool esFichaPerteneceMaestra = false;
                // obtener ficha técnica de ficha maestra
                var fichaPrincipal = servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraPortal);
                if (fichaPrincipal != null)
                {
                    var listaFictecdetXfm = servFictec.ListarAllFichaTecnicaByMaestra(fichaPrincipal.Fteccodi);
                    esFichaPerteneceMaestra = listaFictecdetXfm.Find(x => x.Fteqcodi == idFT) != null;
                }

                if (esFichaPerteneceMaestra)
                {
                    //valores para notificación
                    fichaCambio.Fteqcodi = reg.Fteqcodi;
                    fichaCambio.Fteqnombre = reg.Fteqnombre;
                    fichaCambio.FteqnombreNew = string.Empty;
                    fichaCambio.Fteqestado = Util.EstadoDescripcion(reg.Fteqestado);
                    fichaCambio.FteqestadoNew = Util.EstadoDescripcion(ConstantesAppServicio.Baja);
                    fichaCambio.Fteqfecmodificacion = DateTime.Now;
                    fichaCambio.Ftequsumodificacion = User.Identity.Name;
                }

                reg.Fteqfecmodificacion = DateTime.Now;
                reg.Ftequsumodificacion = User.Identity.Name;
                this.servFictec.DeleteFichaTecnica(reg);

                //Enviar Notificación cambio (eliminación)
                var lstCorreosAdminFT = this.ObtenerListaCorreosAdminFT();
                servFictec.EnviarNotificacionConfiguracionFT(fichaCambio, listaCambios, lstCorreosAdminFT);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Listar los conceptos de una categoria de grupo
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaPrConcepto(int? catecodi)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (catecodi.GetValueOrDefault(0) <= -2)
                {
                    throw new Exception("Debe seleccionar un Categoria de grupo.");
                }
                model.ListaConcepto = this.servFictec.ListPrConceptoByCatecodi(catecodi.Value.ToString(), true);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Listar las propiedad de un tipo de equipo
        /// </summary>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaEqPropiedad(int? famcodi)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (famcodi.GetValueOrDefault(0) <= -2)
                {
                    throw new Exception("Debe seleccionar un Tipo de Equipo.");
                }

                model.ListaPropiedad = this.servFictec.ListEqPropiedadByFamcodi(famcodi.Value, true);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Listar las fichas padres segun Tipo de Equipo o Categoria de Grupo
        /// </summary>
        /// <param name="tipoOrigen"></param>
        /// <param name="catecodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaFichaTecnicaPadre(int tipoOrigen, int? catecodi, int? famcodi)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaFichaTecnicaPadre = this.servFictec.ListarFichaTecnicaPadre(tipoOrigen, catecodi, famcodi);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Listar las notas de una ficha tecnica
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult FichaTecnicaNotaLista(int idFT)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();

            model.ListaNota = new List<FtFictecNotaDTO>();
            if (idFT > 0)
            {
                model.ListaNota = this.servFictec.ListFtFictecNotaByFteqcodi(idFT);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                StringBuilder jsonNota = new StringBuilder();
                serializer.Serialize(model.ListaNota, jsonNota);
                model.NotaJson = jsonNota.ToString();
            }

            return PartialView(model);
        }

        /// <summary>
        /// Registrar/editar ficha tecnica
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaTecnicaNotaGuardar(int idNota, int idFT, int numero, string desc)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fichaTecnica = this.servFictec.GetFichaTecnica(idFT);

                //Validaciones
                if (numero <= 0)
                {
                    throw new Exception("Debe ingresar número válido.");
                }

                if (desc == null || desc.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar descripción.");
                }

                if (fichaTecnica == null)
                {
                    throw new Exception("No es una ficha técnica válida");
                }

                //Guardar
                if (idNota > 0)
                {
                    FtFictecNotaDTO reg = this.servFictec.GetByIdFtFictecNota(idNota);
                    reg.Ftnotanum = numero;
                    reg.Ftnotdesc = desc;
                    reg.Ftnotafecmodificacion = DateTime.Now;
                    reg.Ftnotausumodificacion = User.Identity.Name;

                    this.servFictec.UpdateFtFictecNota(reg);
                }
                else
                {
                    FtFictecNotaDTO reg = new FtFictecNotaDTO();
                    reg.Ftnotanum = numero;
                    reg.Ftnotdesc = desc;
                    reg.Ftnotestado = 1;
                    reg.Fteqcodi = idFT;
                    reg.Ftnotafeccreacion = DateTime.Now;
                    reg.Ftnotausucreacion = User.Identity.Name;

                    this.servFictec.SaveFtFictecNota(reg);
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Eliminar ficha tecnica
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaTecnicaNotaEliminar(int idNota)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                FtFictecNotaDTO reg = this.servFictec.GetByIdFtFictecNota(idNota);

                if (reg == null)
                {
                    throw new Exception("La nota no existe o ha sido eliminada.");
                }

                reg.Ftnotafecmodificacion = DateTime.Now;
                reg.Ftnotausumodificacion = User.Identity.Name;

                this.servFictec.DeleteFtFictecNota(reg);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesFichaTecnica.ModuloManualUsuario;
            string nombreArchivo = ConstantesFichaTecnica.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesFichaTecnica.FolderRaizFichaTecnicaModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        #endregion

        #region Ficha Maestra

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult FichaMaestra()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FichaTecnicaModel model = new FichaTecnicaModel();

            return View(model);
        }

        /// <summary>
        /// Lista de ficha maestra
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaMaestraLista()
        {
            FichaTecnicaModel model = new FichaTecnicaModel();

            try
            {
                model.ListaFichaMaestra = this.servFictec.ListarFichaMaestra(ConstantesAppServicio.ParametroDefecto);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                //model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Formulario de Ficha Maestra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult FichaMaestraFormulario(int id)
        {
            base.ValidarSesionJsonResult();

            FichaTecnicaModel model = new FichaTecnicaModel();
            List<EstadoParametro> listaEstado = new List<EstadoParametro>();
            listaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Proyecto, EstadoDescripcion = ConstantesAppServicio.ProyectoDesc });
            listaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Baja, EstadoDescripcion = ConstantesAppServicio.BajaDesc });
            listaEstado.Add(new EstadoParametro { EstadoCodigo = ConstantesAppServicio.Anulado, EstadoDescripcion = ConstantesAppServicio.AnuladoDesc });
            model.ListaEstado = listaEstado;

            if (id == 0)
            {
                model.FichaMaestra = new FtFichaTecnicaDTO();
                model.FichaMaestra.Ftecestado = ConstantesAppServicio.Proyecto;
                model.ListaFichaTecnicaNoSelec = this.servFictec.ListarFichaTecnicaSinPadre();
                model.ListaFichaTecnicaSelec = new List<FtFictecXTipoEquipoDTO>();
            }
            else
            {
                model.FichaMaestra = this.servFictec.GetFichaMaestra(id);
                var listaFichaTecnicaAll = this.servFictec.ListarFichaTecnicaSinPadre();
                var listaFichaTecnicaSelec = this.servFictec.ListarFichaTecnicaByMaestra(id);
                List<int> listaCodiSelec = listaFichaTecnicaSelec.Select(x => x.Fteqcodi).ToList();

                model.ListaFichaTecnicaNoSelec = listaFichaTecnicaAll.Where(x => !listaCodiSelec.Contains(x.Fteqcodi)).ToList();
                model.ListaFichaTecnicaSelec = listaFichaTecnicaAll.Where(x => listaCodiSelec.Contains(x.Fteqcodi)).ToList();

                if (model.FichaMaestra.Ftecprincipal == 1)
                {
                    model.ListaEstado = Util.ListaEstado().Where(x => x.EstadoCodigo == ConstantesAppServicio.Baja).ToList();
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// Registrar/editar ficha maestra
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaMaestraGuardar(int id, string nombre, int checkPrincipal, List<int> listaSelec, string estado, int ambiente)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                //Validaciones
                if (nombre == null || nombre.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar nombre.");
                }
                nombre = nombre.Trim();

                if (listaSelec == null || listaSelec.Count == 0)
                {
                    throw new Exception("Debe selecionar una ficha técnica");
                }

                var listaFichaTecnicaAll = this.servFictec.ListarFichaTecnicaSinPadre();

                DateTime fecha = DateTime.Now;
                string usuario = User.Identity.Name;

                //ficha maestra actual del ambiente
                var fichaMaestra = servFictec.GetFichaMaestraPrincipal(ambiente);
                int? idFMAnterior = null;
                if (fichaMaestra != null)
                    idFMAnterior = fichaMaestra.Fteccodi;

                NotificacionFM fichaMaestraCambio = new NotificacionFM();
                List<NotificacionFMDetails> listaCambios = new List<NotificacionFMDetails>();
                //Guardar
                if (id > 0)
                {
                    var listaFM = this.servFictec.ListarFichaMaestra(ConstantesAppServicio.ParametroDefecto).Where(x => x.Ftecestado != ConstantesAppServicio.Baja && x.Fteccodi != id && x.Ftecnombre.ToUpper().Trim() == nombre).ToList();
                    if (listaFM.Count > 0)
                    {
                        throw new Exception("El nombre de la Ficha Maestra ya existe.");
                    }

                    FtFichaTecnicaDTO reg = this.servFictec.GetFichaMaestra(id);
                    bool hayCambio = reg.Ftecprincipal == 1 || checkPrincipal == 1; //cambio cuando la ficha es oficial u otra ficha pasa a oficial
                    bool esCambioDeFichaOficial = reg.Ftecprincipal != checkPrincipal;

                    //valores para notificación
                    if (hayCambio)
                    {
                        fichaMaestraCambio.Fteccodi = reg.Fteccodi;
                        fichaMaestraCambio.Ftecnombre = reg.Ftecnombre;
                        fichaMaestraCambio.FtecnombreNew = reg.Ftecnombre != nombre ? nombre : string.Empty;

                        if (esCambioDeFichaOficial)
                        {
                            if (checkPrincipal == 1)
                            {
                                fichaMaestraCambio.Ftecprincipal = "No oficial";
                                fichaMaestraCambio.FtecprincipalNew = "Oficial";
                            }
                            else
                            {
                                fichaMaestraCambio.Ftecprincipal = "Oficial";
                                fichaMaestraCambio.FtecprincipalNew = "No oficial";
                                estado = ConstantesAppServicio.Baja;
                            }
                        }
                        else
                        {
                            fichaMaestraCambio.Ftecprincipal = "Oficial";
                            fichaMaestraCambio.FtecprincipalNew = "";
                        }

                        fichaMaestraCambio.Fteqfecmodificacion = DateTime.Now;
                        fichaMaestraCambio.Ftequsumodificacion = User.Identity.Name;
                        fichaMaestraCambio.Ambiente = ambiente;
                    }

                    reg.Ftecnombre = nombre;
                    reg.Ftecprincipal = checkPrincipal;
                    reg.Ftecfecmodificacion = fecha;
                    reg.Ftecusumodificacion = usuario;
                    reg.Ftecestado = checkPrincipal == 1 ? ConstantesAppServicio.Activo : estado;
                    reg.Ftecambiente = ambiente;

                    this.servFictec.UpdateFichaMaestra(reg);
                }
                else
                {
                    var listaFM = this.servFictec.ListarFichaMaestra(ConstantesAppServicio.ParametroDefecto).Where(x => x.Ftecestado != ConstantesAppServicio.Baja && x.Ftecnombre.ToUpper().Trim() == nombre).ToList();
                    if (listaFM.Count > 0)
                    {
                        throw new Exception("El nombre de la Ficha Maestra ya existe.");
                    }

                    FtFichaTecnicaDTO reg = new FtFichaTecnicaDTO();
                    reg.Ftecnombre = nombre;
                    reg.Ftecprincipal = checkPrincipal;
                    reg.Ftecestado = checkPrincipal == 1 ? ConstantesAppServicio.Activo : ConstantesAppServicio.Proyecto;
                    reg.Ftecfeccreacion = fecha;
                    reg.Ftecusucreacion = usuario;
                    reg.Ftecambiente = ambiente;

                    id = this.servFictec.SaveFichaMaestra(reg);

                    //valores para notificación
                    if (checkPrincipal == 1)//si se marco como oficial
                    {
                        fichaMaestraCambio.Fteccodi = id;
                        fichaMaestraCambio.Ftecnombre = string.Empty;
                        fichaMaestraCambio.FtecnombreNew = nombre;
                        fichaMaestraCambio.Ftecprincipal = string.Empty;
                        fichaMaestraCambio.FtecprincipalNew = "Oficial";
                        fichaMaestraCambio.Fteqfecmodificacion = DateTime.Now;
                        fichaMaestraCambio.Ftequsumodificacion = User.Identity.Name;
                        fichaMaestraCambio.Ambiente = ambiente;
                    }
                }

                this.servFictec.GuardarFichaMaestra(id, listaSelec, fecha, usuario, idFMAnterior, ambiente, ref listaCambios);

                //Enviar Notificación cambio
                var lstCorreosAdminFT = this.ObtenerListaCorreosAdminFT();
                servFictec.EnviarNotificacionFM(fichaMaestraCambio, listaCambios, lstCorreosAdminFT, ambiente);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Eliminar ficha maestra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaMaestraEliminar(int id)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                FtFichaTecnicaDTO reg = this.servFictec.GetFichaMaestra(id);

                if (reg == null)
                {
                    throw new Exception("La Ficha Maestra no existe o ha sido eliminada.");
                }

                reg.Ftecfecmodificacion = DateTime.Now;
                reg.Ftecusumodificacion = User.Identity.Name;
                reg.Ftecprincipal = ConstantesFichaTecnica.FichaMaestraNoPrincipal;
                this.servFictec.DeleteFichaMaestra(reg);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        #endregion

        #region Visualización Ficha Técnica

        #region Index

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FichaTecnicaModel model = new FichaTecnicaModel();
            model.ListaFichaTecnicaSelec = new List<FtFictecXTipoEquipoDTO>();
            model.ListaFichaMaestra = new List<FtFichaTecnicaDTO>();

            try
            {
                List<FtFichaTecnicaDTO> listaFichaMaestra = this.servFictec.ListarFichaMaestra(ConstantesAppServicio.ParametroDefecto)
                                                                .Where(x => x.Ftecestado == ConstantesAppServicio.Proyecto || x.Ftecestado == ConstantesAppServicio.Activo).ToList();

                listaFichaMaestra = listaFichaMaestra.Where(x => x.Ftecambiente != ConstantesFichaTecnica.FichaMaestraIntranet).ToList();
                foreach (var obj in listaFichaMaestra)
                {
                    if (obj.Ftecprincipal == 1) obj.Ftecnombre += " (OFICIAL)";
                }
                if (listaFichaMaestra.Count == 0)
                {
                    model.ListaFichaMaestra.Add(new FtFichaTecnicaDTO() { Fteccodi = 0, Ftecnombre = "--SELECCIONE--" });
                }

                model.ListaFichaMaestra.AddRange(listaFichaMaestra);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return View(model);
        }

        /// <summary>
        /// Listar Empresas por Ficha Tecnica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaFichaTecnicaXMaestra(int idFTmaestra)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                model.ListaFichaTecnicaSelec = this.servFictec.ListarFichaTecnicaByMaestra(idFTmaestra);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Listar Empresas por Ficha Tecnica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaEmpresaXFichaTecnica(int idFT)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                var fichaTecnica = servFictec.GetFichaTecnica(idFT);
                model.Origen = fichaTecnica.Origen;
                model.TipoElementoId = model.Origen == ConstantesFichaTecnica.OrigenTipoEquipo ? fichaTecnica.Famcodi.Value : -2;

                model.ListaEmpresa = this.servFictec.ListarEmpresaByFichaTecnica(idFT);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Lista de Elementos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ElementosListado(int idFicha, int iEmpresa, string iEstado)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                model.ListaElemento = this.servFictec.ListarElementoFichaTecnica(idFicha, iEmpresa, iEstado);
                model.IdFicha = idFicha;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                //model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        [HttpPost]
        public JsonResult UpdateVisibilidadEquiposModos(int idElemento, int idFT, int tipoOculto, string opcion)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                var fichaTecnica = servFictec.GetFichaTecnica(idFT);
                model.Origen = fichaTecnica.Origen;

                DateTime fecha = DateTime.Now;
                string usuario = User.Identity.Name;
                NotificacionEqVisualizacion elementoCambio = new NotificacionEqVisualizacion();

                //Guardar o actualizar equipos o modos no visibles
                servFictec.ActualizarVisibilidadEquiposModos(idFT, idElemento, model.Origen, tipoOculto, opcion, fecha, usuario, ref elementoCambio);

                //Enviar notificación por cambios en visualización
                var lstCorreosAdminFT = this.ObtenerListaCorreosAdminFT();
                // obtener ficha técnica de ficha maestra
                var fichaPrincipal = servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraPortal);
                var listaFictecdetXfm = servFictec.ListarAllFichaTecnicaByMaestra(fichaPrincipal.Fteccodi);
                var fichaPerteneceMaestra = listaFictecdetXfm.Find(x => x.Fteqcodi == idFT);
                if (fichaPerteneceMaestra != null) // pertenece a la maestra oficial
                {
                    if (model.Origen == ConstantesFichaTecnica.OrigenTipoEquipo)
                    {
                        EqEquipoDTO eq = servFictec.GetByIdEqEquipo(idElemento);
                        elementoCambio.Codigo = eq.Equicodi;
                        //elementoCambio.TipoId = eq.Famcodi.Value;
                        elementoCambio.Nombre = eq.Equinomb;
                        elementoCambio.Abreviatura = eq.Equiabrev;
                        elementoCambio.Empresa = eq.EMPRNOMB;
                        elementoCambio.Ubicacion = eq.AREANOMB;
                        elementoCambio.Usuario = usuario;
                        elementoCambio.Fecha = fecha;
                    }
                    else
                    {
                        PrGrupoDTO gr = servFictec.GetByIdPrGrupo(idElemento);
                        elementoCambio.Codigo = gr.Grupocodi;
                        //elementoCambio.TipoId = gr.Catecodi;
                        elementoCambio.Nombre = gr.Gruponomb;
                        elementoCambio.Abreviatura = gr.Grupoabrev;
                        elementoCambio.Empresa = gr.Emprnomb;
                        elementoCambio.Usuario = usuario;
                        elementoCambio.Fecha = fecha;
                    }

                    //Enviar Notificación cambio
                    //var lstCorreosAdminFT = this.ObtenerListaCorreosAdminFT();
                    if (tipoOculto == ConstantesFichaTecnica.TipoOcultoPortal)
                    {
                        servFictec.EnviarNotificacionEquiposVisualizacion(elementoCambio, lstCorreosAdminFT, tipoOculto);
                    }

                    if (tipoOculto == ConstantesFichaTecnica.TipoOcultoExtranet)
                    {
                        var listaElementos = this.servFictec.ListarElementoFichaTecnica(idFT, -2, "-2");
                        var existeElemento = listaElementos.Find(x => x.Codigo == idElemento);
                        //Enviar notificación cambio visibilidad extranet
                        if (existeElemento != null)
                            servFictec.EnviarNotificacionEquiposVisualizacion(elementoCambio, lstCorreosAdminFT, tipoOculto);
                    }
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Exportación de Excel Masivo por Ficha
        /// </summary>
        /// <param name="sFechaIni"></param>
        /// <param name="sFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarMasivoFichaTecnica(int idFT, int opcionComentario, int iEmpresa, string iEstado)
        {
            string ruta = string.Empty, nombre = string.Empty;
            string[] datos = new string[2];
            try
            {
                bool incluirColumnaComentario = opcionComentario == 1;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                bool flagSoloEqActivo = false;
                this.servFictec.ReporteMasivoFichaTecnicaExcel(idFT, pathLogo, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, incluirColumnaComentario, false, true, iEmpresa, iEstado, out ruta, out nombre);

                datos[0] = ruta;
                datos[1] = nombre;

                var jsonResult = Json(datos);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        #endregion

        #region Detalle

        /// <summary>
        /// Detalle de una Ficha Tecnica
        /// </summary>
        /// <param name="tipo">equipo o grupo</param>
        /// <param name="idTipo">codigo famcodi o catecodi</param>
        /// <param name="idElemento">codigo de equipo o grupo</param>
        /// <returns></returns>
        public ActionResult IndexDetalle(int idFicha, int idElemento)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FichaTecnicaModel model = new FichaTecnicaModel();

            model.Elemento = new ElementoFichaTecnica();
            model.Elemento.Existe = ConstantesFichaTecnica.FichaMaestraNoPrincipal;

            var listaEmpresa = new List<SiEmpresaDTO>();

            try
            {
                model.Elemento = this.servFictec.GetElementoDetalleExistente(idFicha, idElemento);
                model.TipoElemento = model.Elemento.Tipo;
                model.TipoElementoId = model.Elemento.TipoId;
                model.Codigo = idElemento;

                if (model.TipoElementoId == 1) // tipo subestación
                {
                    //obtener empresas
                    listaEmpresa = servFictec.ListaEmpresasXFichaSubestacion(model.Elemento.Fteqcodi, model.Elemento.Areacodi.Value, false);
                }

                model.IdFicha = idFicha;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Mensaje = ex.ToString();
            }

            model.ListaEmpresa = new List<SiEmpresaDTO>();
            if (listaEmpresa.Count != 1)
                model.ListaEmpresa.Add(new SiEmpresaDTO() { Emprcodi = -2, Emprnomb = "--TODOS--" });
            model.ListaEmpresa.AddRange(listaEmpresa);

            return View(model);
        }

        /// <summary>
        /// Metodo para obtener valores de la visualización  de ficha y vista previa
        /// </summary>
        /// <param name="idFM"></param>
        /// <param name="tipo"></param>
        /// <param name="idTipo"></param>
        /// <param name="idElemento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarDetalleFichaTecnica(int idFicha, int idElemento)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            //idElemento = 1;
            try
            {
                List<FtFictecItemDTO> listaTreeItems, listaAllItems;
                List<TreeItemFichaTecnica> listaItemsJson;
                List<FtFictecNotaDTO> listaNota;
                List<FtFictecXTipoEquipoDTO> listaHijo;
                FtFictecXTipoEquipoDTO fichaTecnica;
                List<EqEquipoDTO> listaEquipo;
                List<PrGrupoDTO> listaGrupo;

                bool flagSoloEqActivo = false;
                this.servFictec.ReporteDetalleFichaTecnica(idFicha, idElemento, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, DateTime.Today,
                    out listaAllItems, out listaTreeItems, out listaItemsJson, out listaNota, out listaHijo, out fichaTecnica,
                    out listaEquipo, out listaGrupo, out bool flagExisteComentario);

                model.ListaAllItems = listaAllItems;
                model.ListaTreeItems = listaTreeItems;
                model.ListaItemsJson = listaItemsJson;

                model.ListaNota = listaNota;

                model.ListaHijo = listaHijo;
                model.FichaTecnica = fichaTecnica;
                model.ListaEquipo = listaEquipo;
                model.ListaGrupo = listaGrupo;

                model.IdFicha = idFicha;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Exportación de Excel del Detalle
        /// </summary>
        /// <param name="sFechaIni"></param>
        /// <param name="sFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarFichaTecnica(int idFicha, int idElemento, int opcionComentario)
        {
            string[] datos = new string[2];
            try
            {
                bool incluirColumnaComentario = opcionComentario == 1;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                bool flagSoloEqActivo = false;
                this.servFictec.ReporteDetalleFichaTecnicaExcel(pathLogo, idFicha, idElemento, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, incluirColumnaComentario, false, DateTime.Now, false,
                                                            out string ruta, out string nombre);

                datos[0] = ruta;
                datos[1] = nombre;

                var jsonResult = Json(datos);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        #endregion

        /// <summary>
        /// Permite descargar el archivo 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcel()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #endregion

        #region Visualización Ficha Técnica Extranet

        #region Index

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexFichaExtranet()
        {

            FichaTecnicaModel model = new FichaTecnicaModel();
            //model.ListaFichaTecnicaSelec = new List<FtFictecXTipoEquipoDTO>();
            //model.ListaFichaMaestra = new List<FtFichaTecnicaDTO>();

            try
            {
                if (!base.IsValidSesionView()) return base.RedirectToLogin();

                model.FichaMaestra = this.servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraExtranet);

                if (model.FichaMaestra != null)
                {
                    model.ListaFichaTecnicaSelec = this.servFictec.ListarFichaTecnicaByMaestra(model.FichaMaestra.Fteccodi);
                }
                else
                    model.ListaFichaTecnicaSelec = new List<FtFictecXTipoEquipoDTO>();

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }


        /// <summary>
        /// Listar Empresas por Ficha Tecnica extranet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaEmpresaXFichaTecnicaExtranet(int idFT)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                var fichaTecnica = servFictec.GetFichaTecnica(idFT);
                model.Origen = fichaTecnica.Origen;
                model.TipoElementoId = model.Origen == ConstantesFichaTecnica.OrigenTipoEquipo ? fichaTecnica.Famcodi.Value : -2;

                model.ListaEmpresa = this.servFictec.ListarEmpresaByFichaTecnica(idFT);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Lista de Elementos de ficha extranet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ElementosListadoExtranet(int idFicha, int iEmpresa, string iEstado)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                model.ListaElemento = this.servFictec.ListarElementoFichaTecnica(idFicha, iEmpresa, iEstado);
                model.IdFicha = idFicha;

                //quitar los equipos o grupos ocultos para extranet
                //model.ListaElemento = model.ListaElemento.Where(x => x.FtverocultoExtranet != "S").ToList();

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        #endregion

        #region Detalle

        /// <summary>
        /// Detalle de una Ficha Tecnica Extranet
        /// </summary>
        /// <param name="idFicha"></param>
        /// <param name="idElemento"></param>
        /// <returns></returns>
        public ActionResult IndexDetalleExtranet(int idFicha, int idElemento)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FichaTecnicaModel model = new FichaTecnicaModel();

            model.Elemento = new ElementoFichaTecnica();
            model.Elemento.Existe = ConstantesFichaTecnica.FichaMaestraNoPrincipal;

            var listaEmpresa = new List<SiEmpresaDTO>();

            try
            {
                model.Elemento = this.servFictec.GetElementoDetalleExistente(idFicha, idElemento);
                model.TipoElemento = model.Elemento.Tipo;
                model.TipoElementoId = model.Elemento.TipoId;
                model.Codigo = idElemento;

                if (model.TipoElementoId == 1) // tipo subestación
                {
                    //obtener empresas
                    listaEmpresa = servFictec.ListaEmpresasXFichaSubestacion(model.Elemento.Fteqcodi, model.Elemento.Areacodi.Value, false);
                }

                model.IdFicha = idFicha;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Mensaje = ex.ToString();
            }

            model.ListaEmpresa = new List<SiEmpresaDTO>();
            if (listaEmpresa.Count != 1)
                model.ListaEmpresa.Add(new SiEmpresaDTO() { Emprcodi = -2, Emprnomb = "--TODOS--" });
            model.ListaEmpresa.AddRange(listaEmpresa);

            return View(model);
        }

        /// <summary>
        /// Metodo para obtener valores de la visualización  de ficha y vista previa
        /// </summary>
        /// <param name="idFicha"></param>
        /// <param name="idElemento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarDetalleFichaTecnicaExtranet(int idFicha, int idElemento)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            //idElemento = 1;
            try
            {
                List<FtFictecItemDTO> listaTreeItems, listaAllItems;
                List<TreeItemFichaTecnica> listaItemsJson;
                List<FtFictecNotaDTO> listaNota;
                List<FtFictecXTipoEquipoDTO> listaHijo;
                FtFictecXTipoEquipoDTO fichaTecnica;
                List<EqEquipoDTO> listaEquipo;
                List<PrGrupoDTO> listaGrupo;

                bool flagSoloEqActivo = false;
                this.servFictec.ReporteDetalleFichaTecnica(idFicha, idElemento, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, DateTime.Today,
                    out listaAllItems, out listaTreeItems, out listaItemsJson, out listaNota, out listaHijo, out fichaTecnica,
                    out listaEquipo, out listaGrupo, out bool flagExisteComentario);

                model.ListaAllItems = listaAllItems;
                model.ListaTreeItems = listaTreeItems;
                model.ListaItemsJson = listaItemsJson;

                model.FlagExisteComentario = flagExisteComentario ? 1 : 0;
                model.ListaNota = listaNota;

                model.ListaHijo = listaHijo;
                model.FichaTecnica = fichaTecnica;
                model.ListaEquipo = listaEquipo;
                model.ListaGrupo = listaGrupo;

                model.IdFicha = idFicha;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Exportación de Excel del Detalle
        /// </summary>
        /// <param name="idFicha"></param>
        /// <param name="idElemento"></param>
        /// <param name="opcionComentario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarFichaTecnicaExtranet(int idFicha, int idElemento, int opcionComentario)
        {
            string[] datos = new string[2];
            try
            {
                bool incluirColumnaComentario = opcionComentario == 1;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                bool flagSoloEqActivo = false;
                this.servFictec.ReporteDetalleFichaTecnicaExcel(pathLogo, idFicha, idElemento, ConstantesFichaTecnica.EXTRANET, flagSoloEqActivo, incluirColumnaComentario, false, DateTime.Now, false,
                                                            out string ruta, out string nombre);

                datos[0] = ruta;
                datos[1] = nombre;

                var jsonResult = Json(datos);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        #endregion

        #endregion

        #region Utilidad

        /// <summary>
        /// Obtener Lista de correos con rol de administrador de ficha técnica
        /// </summary>
        /// <returns></returns>
        private List<string> ObtenerListaCorreosAdminFT()
        {
            List<string> lstCorreos = new List<string>();

            var listaUsuriosFT = seguridad.ObtenerUsuariosPorRol(ConstantesFichaTecnica.RolAdministradorFichaTecnica);

            foreach (var item in listaUsuriosFT)
            {
                lstCorreos.Add(item.UserEmail);
            }

            lstCorreos = lstCorreos.Where(x => x.Contains("@coes")).ToList();

            return lstCorreos;
        }

        #endregion
    }
}