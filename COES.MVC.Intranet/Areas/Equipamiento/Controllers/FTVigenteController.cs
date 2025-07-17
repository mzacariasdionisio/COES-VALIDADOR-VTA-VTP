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

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTVigenteController : BaseController
    {
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();
        IEODAppServicio servIeod = new IEODAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTVigenteController));
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

        #region Visualización Ficha Técnica Vigente

        #region Index

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            FichaTecnicaModel model = new FichaTecnicaModel();

            try
            {
                if (!base.IsValidSesionView()) return base.RedirectToLogin();

                model.FichaMaestra = this.servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraIntranet);

                if (model.FichaMaestra != null)
                {
                    model.ListaFichaTecnicaSelec = this.servFictec.ListarFichaTecnicaByMaestra(model.FichaMaestra.Fteccodi);
                }
                else
                    model.ListaFichaTecnicaSelec = new List<FtFictecXTipoEquipoDTO>();

                model.TienePermisoAdmin = base.TieneRolAdministradorFicha() ? 1 : 0;// verificar si tiene permisos administrador de ficha

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
        /// Listar Empresas por Ficha Tecnica vigente
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
                model.FlagCheckComent = fichaTecnica.Fteqflagmostrarcoment;
                model.FlagCheckSust = fichaTecnica.Fteqflagmostrarsust;
                model.FlagCheckFech = fichaTecnica.Fteqflagmostrarfech;
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
        /// Lista de Elementos de ficha vigente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ElementosListado(int idFicha, int iEmpresa)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                model.ListaElemento = this.servFictec.ListarElementoFichaTecnica(idFicha, iEmpresa, "A");
                model.IdFicha = idFicha;

                //quitar los equipos o grupos ocultos para intranet
                model.ListaElemento = model.ListaElemento.Where(x => x.FtverocultoIntranet != "S").ToList();

                //Verificar si alguna ficha tiene comentario
                model.FlagExisteComentario = 0;
                DateTime fechaConsulta = DateTime.Now;
                bool flagSoloEqActivo = true;

                foreach (var item in model.ListaElemento)
                {
                    List<FtFictecItemDTO> listaTreeItems, listaAllItems;
                    List<TreeItemFichaTecnica> listaItemsJson;
                    List<FtFictecNotaDTO> listaNota;
                    List<FtFictecXTipoEquipoDTO> listaHijo;
                    FtFictecXTipoEquipoDTO fichaTecnica;
                    List<EqEquipoDTO> listaEquipo;
                    List<PrGrupoDTO> listaGrupo;

                    this.servFictec.ReporteDetalleFichaTecnica(idFicha, item.Codigo, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, fechaConsulta,
                                                                out listaAllItems, out listaTreeItems, out listaItemsJson, out listaNota, out listaHijo, out fichaTecnica,
                                                                out listaEquipo, out listaGrupo, out bool flagExisteComentario);

                    if (flagExisteComentario)
                    {
                        model.FlagExisteComentario = 1;
                        break;
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

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Exportación de Excel Masivo por Ficha
        /// </summary>
        /// <param name="sFechaIni"></param>
        /// <param name="sFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarMasivoFichaTecnica(int idFT, bool opcionComentario, bool opcionSustento, int iEmpresa)
        {
            string ruta = string.Empty, nombre = string.Empty;
            string[] datos = new string[2];
            try
            {
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                bool flagSoloEqActivo = true;
                this.servFictec.ReporteMasivoFichaTecnicaExcel(idFT, pathLogo, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, opcionComentario, opcionSustento, false, iEmpresa, "A", out ruta, out nombre);

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
        /// Detalle de una Ficha Tecnica vigente
        /// </summary>
        /// <param name="idFicha"></param>
        /// <param name="idElemento"></param>
        /// <returns></returns>
        public ActionResult IndexDetalle(int idFicha, int idElemento)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FichaTecnicaModel model = new FichaTecnicaModel();

            model.Elemento = new ElementoFichaTecnica();
            model.Elemento.Existe = ConstantesFichaTecnica.FichaMaestraNoPrincipal;
            model.FechaConsulta = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            var listaEmpresa = new List<SiEmpresaDTO>();

            try
            {
                model.Elemento = this.servFictec.GetElementoDetalleExistente(idFicha, idElemento);
                model.TipoElemento = model.Elemento.Tipo;
                model.TipoElementoId = model.Elemento.TipoId;
                model.Codigo = idElemento;
                model.TienePermisoAdmin = base.TieneRolAdministradorFicha() ? 1 : 0;

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
        public JsonResult GenerarDetalleFichaTecnica(int idFicha, int idElemento, string fecha)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            //idElemento = 1;
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                List<FtFictecItemDTO> listaTreeItems, listaAllItems;
                List<TreeItemFichaTecnica> listaItemsJson;
                List<FtFictecNotaDTO> listaNota;
                List<FtFictecXTipoEquipoDTO> listaHijo;
                FtFictecXTipoEquipoDTO fichaTecnica;
                List<EqEquipoDTO> listaEquipo;
                List<PrGrupoDTO> listaGrupo;

                //Dado que ficha vigente se comporta similar a Portal
                bool flagSoloEqActivo = true;

                this.servFictec.ReporteDetalleFichaTecnica(idFicha, idElemento, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, fechaConsulta,
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
        public JsonResult ExportarFichaTecnica(int idFicha, int idElemento, bool opcionComentario, bool opcionSustento, string fecha)
        {
            string[] datos = new string[2];
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                //Dado que ficha vigente se comporta similar a Portal
                bool flagSoloEqActivo = true;
                bool esAdmin = base.TieneRolAdministradorFicha();
                this.servFictec.ReporteDetalleFichaTecnicaExcel(pathLogo, idFicha, idElemento, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, opcionComentario, opcionSustento, fechaConsulta, esAdmin,
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

        #region Configuración ficha vigente

        #region Index

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexConfiguracion()
        {

            FichaTecnicaModel model = new FichaTecnicaModel();

            try
            {
                if (!base.IsValidSesionView()) return base.RedirectToLogin();

                model.FichaMaestra = this.servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraIntranet);

                if (model.FichaMaestra != null)
                {
                    model.ListaFichaTecnicaSelec = this.servFictec.ListarFichaTecnicaByMaestra(model.FichaMaestra.Fteccodi);
                }
                else
                    model.ListaFichaTecnicaSelec = new List<FtFictecXTipoEquipoDTO>();

                model.TienePermisoAdmin = base.TieneRolAdministradorFicha() ? 1 : 0;// verificar si tiene permisos administrador de ficha

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
        /// Lista de Elementos de ficha vigente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ElementosListadoConfig(int idFicha, int iEmpresa, string iEstado)
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
                var fichaPrincipal = servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraIntranet);
                var listaFictecdetXfm = servFictec.ListarAllFichaTecnicaByMaestra(fichaPrincipal.Fteccodi);
                var fichaPerteneceMaestra = listaFictecdetXfm.Find(x => x.Fteqcodi == idFT);
                if (fichaPerteneceMaestra != null) // pertenece a la maestra oficial intranet
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
                    if (tipoOculto == ConstantesFichaTecnica.TipoOcultoIntranet)
                    {
                        var listaElementos = this.servFictec.ListarElementoFichaTecnica(idFT, -2, "-2");
                        var existeElemento = listaElementos.Find(x => x.Codigo == idElemento);
                        //Enviar notificación cambio visibilidad intranet
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

        [HttpPost]
        public JsonResult UpdateFichaVisibilidaChecks(int idFT, bool flagCheck, int tipoCheck)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                servFictec.ActualizarFichaVisibilidadCheck(idFT, flagCheck, tipoCheck);

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
        public JsonResult ExportarMasivoFichaTecnicaConfig(int idFT, bool opcionComentario, bool opcionSustento, bool opcionEqOcultos, int iEmpresa, string iEstado)
        {
            string ruta = string.Empty, nombre = string.Empty;
            string[] datos = new string[2];
            try
            {

                //bool incluirColumnaComentario = opcionComentario == 1;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                bool flagSoloEqActivo = false;
                this.servFictec.ReporteMasivoFichaTecnicaExcel(idFT, pathLogo, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, opcionComentario, opcionSustento, opcionEqOcultos, iEmpresa, iEstado, out ruta, out nombre);

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
        /// Detalle de una Ficha Tecnica vigente
        /// </summary>
        /// <param name="idFicha"></param>
        /// <param name="idElemento"></param>
        /// <returns></returns>
        public ActionResult IndexDetalleConfiguracion(int idFicha, int idElemento)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FichaTecnicaModel model = new FichaTecnicaModel();

            model.Elemento = new ElementoFichaTecnica();
            model.Elemento.Existe = ConstantesFichaTecnica.FichaMaestraNoPrincipal;
            model.FechaConsulta = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            var listaEmpresa = new List<SiEmpresaDTO>();

            try
            {
                model.Elemento = this.servFictec.GetElementoDetalleExistente(idFicha, idElemento);
                model.TipoElemento = model.Elemento.Tipo;
                model.TipoElementoId = model.Elemento.TipoId;
                model.Codigo = idElemento;
                model.TienePermisoAdmin = base.TieneRolAdministradorFicha() ? 1 : 0;

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
        public JsonResult GenerarDetalleFichaTecnicaConfig(int idFicha, int idElemento, string fecha)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            //idElemento = 1;
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                List<FtFictecItemDTO> listaTreeItems, listaAllItems;
                List<TreeItemFichaTecnica> listaItemsJson;
                List<FtFictecNotaDTO> listaNota;
                List<FtFictecXTipoEquipoDTO> listaHijo;
                FtFictecXTipoEquipoDTO fichaTecnica;
                List<EqEquipoDTO> listaEquipo;
                List<PrGrupoDTO> listaGrupo;

                bool flagSoloEqActivo = false;
                this.servFictec.ReporteDetalleFichaTecnica(idFicha, idElemento, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, fechaConsulta,
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
        public JsonResult ExportarFichaTecnicaConfig(int idFicha, int idElemento, bool opcionComentario, bool opcionSustento, string fecha)
        {
            string[] datos = new string[2];
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                bool flagSoloEqActivo = false;
                bool esAdmin = base.TieneRolAdministradorFicha();
                this.servFictec.ReporteDetalleFichaTecnicaExcel(pathLogo, idFicha, idElemento, ConstantesFichaTecnica.INTRANET, flagSoloEqActivo, opcionComentario, opcionSustento, fechaConsulta, esAdmin,
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
    }
}
