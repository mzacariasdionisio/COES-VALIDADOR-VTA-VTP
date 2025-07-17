
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class CentralSDDPController : BaseController
    {
        private readonly ProgramacionAppServicio pmpoServicio = new ProgramacionAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ParametrosFechasController));
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

        public int? IdTopologia
        {
            get
            {
                return (Session[ConstantesPMPO.SesionIdTopologia] != null) ?
                    (int)(Session[ConstantesPMPO.SesionIdTopologia]) : -1;
            }
            set { Session[ConstantesPMPO.SesionIdTopologia] = value; }
        }
        #endregion

        #region Métodos 

        /// <summary>
        /// Index Configuración Base (centrales sddp)
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? mtopcodi, int? acn)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CentralSDDPModel model = new CentralSDDPModel();

            try
            {
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                bool valorLayout;

                DateTime fechaDeCalculo;

                string titulo = "Configuración Base";
                if (mtopcodi == null)
                {
                    mtopcodi = 0; //Topologia base
                    valorLayout = false;
                    fechaDeCalculo = DateTime.Now;
                }
                else
                {
                    var objEscenario = pmpoServicio.GetByIdMpTopologia(mtopcodi.Value);
                    DateTime fechaConsulta = objEscenario.Mtopfecha.Value;
                    titulo = titulo + ": " + objEscenario.Mtopnomb + " (Versión " + objEscenario.Mtopversion + ")";
                    valorLayout = true;

                    //modulo fechas
                    PmoMesDTO mesOperativo = pmpoServicio.ListarSemanaMesDeAnho(fechaConsulta.Year, ConstantesPMPO.AccionEditar, null).Find(x => x.Pmmesfecinimes.Month == fechaConsulta.Month);
                    fechaDeCalculo = mesOperativo.Pmmesfecini; //primer sabado del mes operativo
                }




                model.ListaTotalCentralesHidro = pmpoServicio.ListarTodasCentralesHidroelectricas(fechaDeCalculo);
                model.ListaTotalEmbalses = pmpoServicio.ListarTodosEmbalses();
                model.StrListaCentralesHidro = pmpoServicio.ObtenerCadenaDeCentralesHidro(model.ListaTotalCentralesHidro);
                model.StrListaEmbalses = pmpoServicio.ObtenerCadenaDeEmbalses(model.ListaTotalEmbalses);
                model.ListaFormatoPtosSemanal = new List<FormatoPtoMedicion>();
                model.ListaFormatoPtosMensual = new List<FormatoPtoMedicion>();



                model.UsarLayoutModulo = valorLayout;
                model.Titulo = titulo;
                model.TopologiaMostrada = mtopcodi;

                if (acn == null)
                {
                    acn = 2;
                }
                model.AccionEsc = acn.Value;
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
        /// Devuelve el html del listado de centrales sddp
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCentralesSppd(int topcodi, int permisoLectura, string strListaCH, string strListaEmb)
        {
            CentralSDDPModel model = new CentralSDDPModel();
            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                this.IdTopologia = topcodi;

                if (permisoLectura == 3)
                    tienePermiso = false;


                List<EqEquipoDTO> lstTotalCHBD = pmpoServicio.FormatoCentralesHidroBD(strListaCH);
                List<EqEquipoDTO> lstTotalEmbBD = pmpoServicio.FormatoEmbalsesBD(strListaEmb);
                model.HtmlListadoCentralesSDDP = pmpoServicio.HtmlListadoCentralesSddp(url, tienePermiso, topcodi, lstTotalCHBD, lstTotalEmbBD);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Devuelve los ptos de medicion segun tipo (semanal/mensual) y formato
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarPtosMedicion(int equicodi)
        {
            CentralSDDPModel model = new CentralSDDPModel();

            try
            {
                List<FormatoPtoMedicion> lstPtos = pmpoServicio.ObtenerPtosByFormatoYEquipo(equicodi);
                model.ListaFormatoPtosSemanal = lstPtos;
                model.ListaFormatoPtosMensual = lstPtos;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }


        /// <summary>
        /// Actualiza el campo orden de las centrales sddp cuando se reordena el listado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="direction"></param>
        public void UpdateOrder(int id, int fromPosition, int toPosition, string direction)
        {
            int topologia = this.IdTopologia.Value;
            pmpoServicio.ActualizarOrdenCentralesSddp(fromPosition, toPosition, direction, topologia);
        }

        /// <summary>
        /// Devuelve el html de la pestaña Detalles
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="topcodi"></param>
        /// <param name="centralCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarDetalles(int accion, int topcodi, int recurcodi, string strListaCH, string strListaEmb)
        {

            CentralSDDPModel model = new CentralSDDPModel();
            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                string url = Url.Content("~/");
                model.TienePermisoNuevo = tienePermiso;
                model.Accion = accion;


                List<EqEquipoDTO> lstTotalCHBD = pmpoServicio.FormatoCentralesHidroBD(strListaCH);
                List<EqEquipoDTO> lstTotalEmbBD = pmpoServicio.FormatoEmbalsesBD(strListaEmb);

                CentralSddp centralSddp = new CentralSddp();

                if (accion == ConstantesPMPO.AccionVerDetalles)
                    model.TienePermisoNuevo = false; //Desactiva botones

                if (accion == ConstantesPMPO.AccionCrear)
                {
                    centralSddp = pmpoServicio.ObtenerDatosDeCentralSddp(accion, topcodi, null, lstTotalCHBD, lstTotalEmbBD);
                }
                else
                {
                    if (accion == ConstantesPMPO.AccionEditar || accion == ConstantesPMPO.AccionVerDetalles)
                    {
                        centralSddp = pmpoServicio.ObtenerDatosDeCentralSddp(accion, topcodi, recurcodi, lstTotalCHBD, lstTotalEmbBD);
                    }
                }
                model.ListaCodigosSddp = pmpoServicio.ListarCodigoSDDP(ConstantesPMPO.TsddpPlantaHidraulica.ToString()).Where(x => x.Sddpestado == ConstantesPMPO.Activo).ToList();
                model.ListaEstacionesHidro = pmpoServicio.ListarEstacionesParaTopologia();
                model.ListaCentralesSddp = pmpoServicio.ListaRecursosPorTopologiaYEstado(topcodi, ConstantesPMPO.EstadoActivo); // centrales sddp existentes para cierto escenario

                model.EsNuevo = accion == ConstantesPMPO.AccionCrear ? true : false;
                model.CentralSddp = centralSddp;

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            PartialViewResult salida = PartialView(model);
            return salida;
        }

        /// <summary>
        /// Devuelve Informacion al mostrar Pestaña Detalles
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="topcodi"></param>
        /// <param name="centralCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarDatosCentralSddp(int accion, int topcodi, int? recurcodi, string strListaCH, string strListaEmb)
        {
            CentralSDDPModel model = new CentralSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.TienePermisoNuevo = tienePermiso;

                List<EqEquipoDTO> lstTotalCHBD = pmpoServicio.FormatoCentralesHidroBD(strListaCH);
                List<EqEquipoDTO> lstTotalEmbBD = pmpoServicio.FormatoEmbalsesBD(strListaEmb);

                CentralSddp centralSddp = new CentralSddp();

                if (accion == ConstantesPMPO.AccionVerDetalles)
                    model.TienePermisoNuevo = false; //Desactiva funcionalidades en el handsonTable               

                if (accion == ConstantesPMPO.AccionCrear)
                {
                    centralSddp = pmpoServicio.ObtenerDatosDeCentralSddp(accion, topcodi, null, lstTotalCHBD, lstTotalEmbBD); //solo para lista Embalses y CH
                }
                else
                {
                    if (accion == ConstantesPMPO.AccionEditar || accion == ConstantesPMPO.AccionVerDetalles)
                    {
                        centralSddp = pmpoServicio.ObtenerDatosDeCentralSddp(accion, topcodi, recurcodi, lstTotalCHBD, lstTotalEmbBD);

                    }
                }

                model.ListaCentralesHidro = centralSddp.ListaCentralesHidro;
                model.ListaEmbalses = centralSddp.ListaEmbalses;
                model.ListaEvaporacion = pmpoServicio.ObtenerListaEvaporacion(topcodi, recurcodi);
                model.ListaVolumenArea = pmpoServicio.ObtenerListaVolumenArea(topcodi, recurcodi);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Devuelve el html de la tabla de CH
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="recurcodi"></param>
        /// <param name="lstCentralesHidro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarCentralesHidro(int accion, int recurcodi, List<CentralHidroelectrica> lstCentralesHidro)
        {
            CentralSDDPModel model = new CentralSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                string url = Url.Content("~/");
                model.TienePermisoNuevo = tienePermiso;

                model.HtmlListadoCentralesHidro = pmpoServicio.HtmlListadoCentralesHidroelectricas(url, accion, recurcodi, lstCentralesHidro, tienePermiso);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// valida duplicados en la tabla de centrales hidroelectrica y devuelve el listado actualizado (al agregar o editar)
        /// </summary>
        /// <param name="accionCentralHidro"></param>
        /// <param name="recurcodi"></param>
        /// <param name="centralHidroCodi"></param>
        /// <param name="centralHidroNombre"></param>
        /// <param name="factor"></param>
        /// <param name="lstCentralesHidroEnPantalla"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarListaCentralesHidro(int accionCentralHidro, int topcodi, int recurcodi, int centralHidroCodi, string centralHidroNombre, decimal factor, string tipoConexion, string strListaCH, List<CentralHidroelectrica> lstCentralesHidroEnPantalla)
        {
            CentralSDDPModel model = new CentralSDDPModel();
            List<CentralHidroelectrica> lstCentralesH = new List<CentralHidroelectrica>();
            try
            {
                base.ValidarSesionJsonResult();

                if (lstCentralesHidroEnPantalla == null)
                    lstCentralesHidroEnPantalla = new List<CentralHidroelectrica>();

                string msgValidacion = pmpoServicio.ValidarCentralHidroAgregada(accionCentralHidro, centralHidroCodi, lstCentralesHidroEnPantalla);
                if (msgValidacion != "") throw new ArgumentException(msgValidacion);


                if (accionCentralHidro == ConstantesPMPO.AccionCrear)
                {
                    lstCentralesH = pmpoServicio.AgregarCentralHidro(centralHidroCodi, centralHidroNombre, factor, lstCentralesHidroEnPantalla);
                }
                else
                {
                    if (accionCentralHidro == ConstantesPMPO.AccionEditar)
                    {
                        lstCentralesH = pmpoServicio.ActualizarCentralHidro(centralHidroCodi, factor, lstCentralesHidroEnPantalla);
                    }
                    else
                    {
                        if (accionCentralHidro == ConstantesPMPO.AccionEliminar)
                            lstCentralesH = pmpoServicio.EliminarCentralHidro(centralHidroCodi, lstCentralesHidroEnPantalla);
                    }

                }

                List<EqEquipoDTO> lstTotalCHBD = pmpoServicio.FormatoCentralesHidroBD(strListaCH);

                model.ListaCentralesHidro = lstCentralesH;
                model.CentralSddp = pmpoServicio.ObtenerDatosOpcionSiCoesSeleccionado(lstCentralesH, lstTotalCHBD, topcodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Devuelve el html de la tabla de embalses
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="recurcodi"></param>
        /// <param name="lstEmbalses"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmbalses(int accion, int recurcodi, List<Embalse> lstEmbalses)
        {
            CentralSDDPModel model = new CentralSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                string url = Url.Content("~/");
                model.TienePermisoNuevo = tienePermiso;

                lstEmbalses = lstEmbalses != null ? pmpoServicio.FormatearEmbalse(lstEmbalses) : new List<Embalse>();
                model.HtmlListadoEmbalse = pmpoServicio.HtmlListadoEmbalses(url, accion, recurcodi, lstEmbalses, tienePermiso);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// valida duplicados en la tabla de embalses y devuelve el listado actualizado (al agregar o editar)
        /// </summary>
        /// <param name="accionEmbalse"></param>
        /// <param name="recurcodi"></param>
        /// <param name="embalseCodi"></param>
        /// <param name="embalseNombre"></param>
        /// <param name="factor"></param>
        /// <param name="tipoVolumen"></param>
        /// <param name="fuenteSemanal"></param>
        /// <param name="fuenteMensual"></param>
        /// <param name="lstEmbalsesEnPantalla"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarListaEmbalses(int accionEmbalse, int recurcodi, int embalseCodi, string embalseNombre, decimal factor, int tipoVolumen, string ptoSemanal, string ptoMensual, List<Embalse> lstEmbalsesEnPantalla)
        {
            CentralSDDPModel model = new CentralSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (lstEmbalsesEnPantalla == null)
                    lstEmbalsesEnPantalla = new List<Embalse>();

                string msgValidacion = pmpoServicio.ValidarEmbalseAgregada(accionEmbalse, embalseCodi, lstEmbalsesEnPantalla);
                if (msgValidacion != "") throw new ArgumentException(msgValidacion);

                if (accionEmbalse == ConstantesPMPO.AccionCrear)
                    model.ListaEmbalses = pmpoServicio.AgregarEmbalse(embalseCodi, embalseNombre, factor, tipoVolumen, ptoSemanal, ptoMensual, lstEmbalsesEnPantalla);
                if (accionEmbalse == ConstantesPMPO.AccionEditar)
                    model.ListaEmbalses = pmpoServicio.ActualizarEmbalse(embalseCodi, factor, tipoVolumen, ptoSemanal, ptoMensual, lstEmbalsesEnPantalla);
                if (accionEmbalse == ConstantesPMPO.AccionEliminar)
                    model.ListaEmbalses = pmpoServicio.EliminarEmbalse(embalseCodi, lstEmbalsesEnPantalla);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Da de baja una central SDDP
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="centralCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarCentralSddp(int topcodi, int recurcodi)
        {
            CentralSDDPModel model = new CentralSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                string msg = pmpoServicio.ValidarEliminacionCentralSddp(topcodi, recurcodi);
                if (msg != "") throw new ArgumentException(msg);

                pmpoServicio.EliminarCentralSddp(topcodi, recurcodi, base.UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Guarda central sddp
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="topcodi"></param>
        /// <param name="centralSddp"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarCentralSddp(int accion, int topcodi, CentralSddp centralSddp, int recurcodiAEditar)
        {
            CentralSDDPModel model = new CentralSDDPModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                //valida que no se Agregue dos veces un mismo CODIGO SDDP
                if (accion == ConstantesPMPO.AccionCrear)
                {
                    string strValidacionCS = pmpoServicio.ValidarCentralSddpRepetida(topcodi, centralSddp);
                    if (strValidacionCS != "") throw new ArgumentException(strValidacionCS);
                }

                //valida que haya cambios
                if (accion == ConstantesPMPO.AccionEditar)
                {
                    string strValidacionCambios = "";

                    if (strValidacionCambios != "") throw new ArgumentException(strValidacionCambios);
                }

                int idRecurso = pmpoServicio.EnviarDatosAGuardar(accion, topcodi, centralSddp, recurcodiAEditar, base.UserName);

                model.Resultado = "1";

            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Actualiza el listado de CH
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarListadoCentralesHidro(int mtopcodi)
        {
            CentralSDDPModel model = new CentralSDDPModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);


                DateTime fechaDeCalculo;
                if (mtopcodi == 0) //Topologia base
                {
                    fechaDeCalculo = DateTime.Now;
                }
                else
                {
                    var objEscenario = pmpoServicio.GetByIdMpTopologia(mtopcodi);
                    DateTime fechaConsulta = objEscenario.Mtopfecha.Value;

                    //modulo fechas
                    PmoMesDTO mesOperativo = pmpoServicio.ListarSemanaMesDeAnho(fechaConsulta.Year, ConstantesPMPO.AccionEditar, null).Find(x => x.Pmmesfecinimes.Month == fechaConsulta.Month);
                    fechaDeCalculo = mesOperativo.Pmmesfecini; //primer sabado del mes operativo
                }

                model.ListaTotalCentralesHidro = pmpoServicio.ListarTodasCentralesHidroelectricas(fechaDeCalculo);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Genera la data del excel exportado de centrales sddp
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="strListaCH"></param>
        /// <param name="strListaEmb"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarExcelCentralesSddp(int topcodi, string titulo, string strListaCH, string strListaEmb)
        {
            CentralSDDPModel model = new CentralSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                List<EqEquipoDTO> lstTotalCHBD = pmpoServicio.FormatoCentralesHidroBD(strListaCH);
                List<EqEquipoDTO> lstTotalEmbBD = pmpoServicio.FormatoEmbalsesBD(strListaEmb);
                pmpoServicio.GenerarArchivoExcelCentralesSddp(ruta, topcodi, lstTotalCHBD, lstTotalEmbBD, titulo, out string nameFile);
                model.Resultado = nameFile;

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite exportar archivos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        /// <summary>
        /// Devuelve el numero del sddp
        /// </summary>
        /// <param name="codigoSddp"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerSddpNum(int codigoSddp)
        {
            CentralSDDPModel model = new CentralSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                List<PmoSddpCodigoDTO> lstSddps = pmpoServicio.ListarCodigoSDDP(ConstantesPMPO.TsddpPlantaHidraulica.ToString()).Where(x => x.Sddpestado == ConstantesPMPO.Activo).ToList();
                PmoSddpCodigoDTO buscado = lstSddps.Find(x => x.Sddpcodi == codigoSddp);
                model.SddpNum = buscado != null ? buscado.Sddpnum.ToString() : "";
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
        #endregion
    }
}