using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.MVC.Intranet.Areas.AporteIntegrates.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.Servicios.Aplicacion.CalculoPorcentajes.Helper;
using COES.Servicios.Aplicacion.DemandaMaxima;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Informe;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class DemandaDistribuidoresULController : BaseController
    {

        // GET: /AporteIntegrantes/DemandaDistribuidoresUL/

        public DemandaDistribuidoresULController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        CalculoPorcentajesAppServicio servicioCalculoPorcentajes = new CalculoPorcentajesAppServicio();
        InformeAppServicio servicioEmpresa = new InformeAppServicio();
        DemandaMaximaAppServicio servicioDemandaMaxima = new DemandaMaximaAppServicio();
        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public ActionResult Index(int caiprscodi = 0, int caiajcodi = 0, int tipoemprcodi = 0, int emprcodi = 0)
        {
            base.ValidarSesionUsuario();
            DemandaDistribuidoresULModel model = new DemandaDistribuidoresULModel();
            Log.Info("Lista de Presupuestos - ListCaiPresupuestos");
            model.ListaPresupuesto = this.servicioCalculoPorcentajes.ListCaiPresupuestos();
            if (model.ListaPresupuesto.Count > 0 && caiprscodi == 0)
            {
                caiprscodi = model.ListaPresupuesto[0].Caiprscodi;

            }
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);
            Log.Info("Lista de Versiones de ajuste - ListCaiAjustes");
            model.ListaAjuste = this.servicioCalculoPorcentajes.ListCaiAjustes(caiprscodi); //Ordenado en descendente
            if (model.ListaAjuste.Count > 0 && caiajcodi == 0)
            {
                caiajcodi = (int)model.ListaAjuste[0].Caiajcodi;
            }

            if (caiprscodi > 0 && caiajcodi > 0)
            {
                Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
                model.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(caiajcodi);
            }
            else
            {
                model.EntidadAjuste = new CaiAjusteDTO();
            }
            Log.Info("Lista Tipoempresa - ListTipoEmpresaCumplimiento");
            model.ListaTipoempresa = this.servicioDemandaMaxima.ListTipoEmpresaCumplimiento();
            if (model.ListaTipoempresa.Count > 0 && tipoemprcodi == 0)
            {
                tipoemprcodi = model.ListaTipoempresa[0].Tipoemprcodi;
            }
            //Traemos la lista de empresas que estan registrados en CAI_AJUSTEEMPRESA
            Log.Info("Lista Empresa - ListaEmpresasPorTipoCumplimiento");
            //model.ListaEmpresasCumplimiento = this.servicioDemandaMaxima.ListaEmpresasPorTipoCumplimiento(tipoemprcodi, IdFormatoPR16);
            model.ListaEmpresasCumplimiento = this.servicioCalculoPorcentajes.ListCaiAjusteempresasByAjusteTipoEmpresa(caiajcodi, tipoemprcodi);
            if (model.ListaEmpresasCumplimiento.Count > 0 && emprcodi == 0)
            {
                emprcodi = model.ListaEmpresasCumplimiento[0].Emprcodi;
            }
            model.Caiprscodi = caiprscodi;
            model.Caiajcodi = caiajcodi;
            model.Tipoemprcodi = tipoemprcodi;
            model.Emprcodi = emprcodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(int caiajcodi, int emprcodi)
        {
            DemandaDistribuidoresULModel model = new DemandaDistribuidoresULModel();
            Log.Info("Lista AjusteEmpresa - ListCaiAjusteempresasByAjusteEmpresa");
            model.ListaAjusteEmpresa = this.servicioCalculoPorcentajes.ListCaiAjusteempresasByAjusteEmpresa(caiajcodi, emprcodi);
            return PartialView(model);
        }

        /// <summary>
        /// Permite importar la información registrada de los Usuarios Libres-Proyectada y Distribuidores-Ejecutada/Proyectada
        /// </summary>
        /// <param name="caiprscodi">Código del Presupuesto</param>
        /// <param name="caiajcodi">Código de la Versión de Presupuesto</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarEjecutadoPoyectado(int caiprscodi = 0, int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            DemandaDistribuidoresULModel model = new DemandaDistribuidoresULModel();
            model.sError = "";
            model.iNumReg = 0;
            string sUser = User.Identity.Name;
            string sCagdcmFuenteDatos;
            int iCagdcmcodi; //Identificador siguiente de la tabla CaiGenerdeman
            string sCagdcmcalidadinfo = "C"; //La información proviene del SGOCOES
            try
            {
                if (caiprscodi > 0 && caiajcodi > 0)
                {
                    //Eliminando Todo el proceso de calculo
                    this.servicioCalculoPorcentajes.EliminarCalculo(caiajcodi);
                    //Eliminando la información del periodo para cada tipo defuente de datos:
                    Log.Info("Elimina registro - DeleteCaiGenerdeman");
                    sCagdcmFuenteDatos = "PU"; //Proyectada de Usuarios libres
                    this.servicioCalculoPorcentajes.DeleteCaiGenerdeman(caiajcodi, sCagdcmFuenteDatos);
                    sCagdcmFuenteDatos = "PD"; //Proyectada de Distribuidores
                    this.servicioCalculoPorcentajes.DeleteCaiGenerdeman(caiajcodi, sCagdcmFuenteDatos);
                    sCagdcmFuenteDatos = "ED"; //Ejecutada de Distribuidores
                    this.servicioCalculoPorcentajes.DeleteCaiGenerdeman(caiajcodi, sCagdcmFuenteDatos);
                    //----------------------------------------------------------------------------------------------------------------------------------------------------
                    //Calculamos la fecha de inicio del Presupuesto
                    Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
                    model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);
                    string sMes = model.EntidadPresupuesto.Caiprsmesinicio.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPresupuesto.Caiprsanio;
                    DateTime dFecInicioEjecutada = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    //Calculamos la fecha de Inicio de ajuste (un dia despues del final Ejecutado):
                    Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
                    model.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(caiajcodi);
                    sMes = model.EntidadAjuste.Caiajmes.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicioAjuste = "01/" + sMes + "/" + model.EntidadAjuste.Caiajanio;
                    //La fecha de ajuste, es el inicio para la data Proyectada
                    DateTime dFecInicioProyectada = DateTime.ParseExact(sFechaInicioAjuste, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime dFecFinEjecutada = dFecInicioProyectada.AddDays(-1); // al quitarle un dia, le estamos colocando al final del dia del mes anterior
                    DateTime dFecFinProyectada = dFecInicioEjecutada.AddMonths(model.EntidadPresupuesto.Caiprsnromeses).AddDays(-1);
                    //----------------------------------------------------------------------------------------------------------------------------------------------------
                    //Recorremos la lista para cada tipo de empresa: UL / Distrib.
                    Log.Info("Lista Tipoempresa - ListTipoEmpresaCumplimiento");
                    model.ListaTipoempresa = this.servicioDemandaMaxima.ListTipoEmpresaCumplimiento();
                    foreach (SiTipoempresaDTO dtoTipoEmpresa in model.ListaTipoempresa)
                    {
                        //4: Usuarios libres - sólo Proyectada
                        //2: Distribuidores
                        int iTipoemprcodi = dtoTipoEmpresa.Tipoemprcodi;
                        int IdLecturaEje = 0;
                        int IdFormatoEje = 0;
                        int IdLecturaProy = 0;
                        int IdFormatoProy = 0;
                        if (iTipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiUsuarioLibre)
                        {
                            //LECTCODI = 218 / CAI-INF PROYECTADA USUARIOS LIBRES
                            IdLecturaProy = ConstantesCalculoPorcentajes.IdLecturaProyectadaUsuariosLibres;
                            //FORMATCODI = 89 / CAI-INF PROYECTADA USUARIOS LIBRES
                            IdFormatoProy = ConstantesCalculoPorcentajes.IdFormatoProyectadaUsuariosLibres;
                            sCagdcmFuenteDatos = "PU";
                        }
                        else if (iTipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor)
                        {
                            //LECTCODI = 216 / INF. EJECUTADA DISTRIBUIDORES
                            IdLecturaEje = ConstantesCalculoPorcentajes.IdLecturaEjecutadaDistribuidores;
                            //FORMATCODI = 87 / CAI-INF EJECUTADA DISTRIBUIDORES
                            IdFormatoEje = ConstantesCalculoPorcentajes.IdFormatoEjecutadaDistribuidores;

                            //LECTCODI = 217 / CAI-INF PROYECTADA DISTRIBUIDORES
                            IdLecturaProy = ConstantesCalculoPorcentajes.IdLecturaProyectadaDistribuidores;
                            //FORMATCODI = 88 / CAI-INF PROYECTADA DISTRIBUIDORES
                            IdFormatoProy = ConstantesCalculoPorcentajes.IdFormatoProyectadaDistribuidores;
                            sCagdcmFuenteDatos = "PD";
                        }
                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        //Procesando por grupo:
                        if (IdLecturaProy > 0)
                        {
                            Log.Info("Obtener Codigo Generado - GetCodigoGeneradoCaiGenerdeman");
                            iCagdcmcodi = this.servicioCalculoPorcentajes.GetCodigoGeneradoCaiGenerdeman();
                            this.servicioCalculoPorcentajes.SaveCaiGenerdemanAsSelectMeMedicion96(iCagdcmcodi, caiajcodi, sCagdcmFuenteDatos, sCagdcmcalidadinfo, "P", sUser, IdFormatoProy, IdLecturaProy, dFecInicioProyectada.ToString("yyyy-MM-dd"), dFecFinProyectada.ToString("yyyy-MM-dd"));
                        }
                        if (IdLecturaEje > 0)
                        {
                            Log.Info("Obtener Codigo Generado - GetCodigoGeneradoCaiGenerdeman");
                            iCagdcmcodi = this.servicioCalculoPorcentajes.GetCodigoGeneradoCaiGenerdeman();
                            this.servicioCalculoPorcentajes.SaveCaiGenerdemanAsSelectMeMedicion96(iCagdcmcodi, caiajcodi, "ED", sCagdcmcalidadinfo, "E", sUser, IdFormatoEje, IdLecturaEje, dFecInicioEjecutada.ToString("yyyy-MM-dd"), dFecFinEjecutada.ToString("yyyy-MM-dd"));
                        }
                    }
                }
                else
                    model.sError = "Debe seleccionar un periodo y versión correcto";
            }
            catch (Exception e)
            {
                model.sError = e.Message;
            }
            return Json(model);
        }

        #region ENVIO DE LA DEMANDA EJECUTADA Y PROYECTADA

        #region Propiedades

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[SesionNombreArchivo] != null) ?
                    Session[SesionNombreArchivo].ToString() : null;
            }
            set { Session[SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[SesionFileName] != null) ?
                    Session[SesionFileName].ToString() : null;
            }
            set { Session[SesionFileName] = value; }
        }

        /// <summary>
        /// Codigo del envio
        /// </summary>
        public int IdEnvio
        {
            get
            {
                return (Session[SesionIdEnvio] != null) ?
                    (int)Session[SesionIdEnvio] : 0;
            }
            set { Session[SesionIdEnvio] = value; }
        }

        /// <summary>
        /// Nombre del formato
        /// </summary>
        public MeFormatoDTO Formato
        {
            get
            {
                return (Session[SesionFormato] != null) ?
                    (MeFormatoDTO)Session[SesionFormato] : new MeFormatoDTO();
            }
            set { Session[SesionFormato] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[SesionMatrizExcel] != null) ?
                    (string[][])Session[SesionMatrizExcel] : new string[1][];
            }
            set { Session[SesionMatrizExcel] = value; }
        }

        /// <summary>
        /// Codigo de formato
        /// </summary>
        //public int IdFormato = 89; //ME_FORMATO : CAI-INF PROYECTADA USUARIOS LIBRES ... de la plantilla: IdFormato = 4 Medidores Demanda en Barras - Distribuidoras
        //public int IdLectura = 218; //me_lectura : INF. PROYECTADA U LIBRES...  de la plantilla: IdLectura = 51 DEMANDA HISTORICA DIARIA MEDID;
        public const string SesionFormato = "SesionFormato";
        public const string SesionIdEnvio = "SesionIdEnvio";
        public const string SesionFileName = "SesionFileName";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";

        #endregion

        /// <summary>
        /// Carga principal de la pantalla de Envio
        /// </summary>
        /// <returns></returns>
        public ActionResult Envio(int caiprscodi, int caiajcodi, int tipoemprcodi, int emprcodi, string intervalo)
        {
            DemandaDistribuidoresULFormatoModel model = new DemandaDistribuidoresULFormatoModel();
            model.IdModulo = ConstantesCalculoPorcentajes.IdModuloCalculoPorcentajes; //MODULO DE CALCULO DE PORCENTAJES
            Log.Info("Empresa - ObtenerEmpresa");
            model.EntidadEmpresa = this.servicioEmpresa.ObtenerEmpresa(emprcodi);
            //Traendo la lista de fechas Proyectadas
            string sCaiajetipoinfo = "E"; //Tipo de información: E:Retiro Energía (Mw.H) / T:Ingreso por transmisión (S/)
            if (intervalo.Equals("E"))
            {
                //EJECUTADO
                model.ListaPeriodos = this.servicioCalculoPorcentajes.ObtenerListaPeriodoEjecutado(sCaiajetipoinfo, caiajcodi, emprcodi);
                //Comparamos con el ajuste anterior, con la finalidad de establecer los REPORTADOS Y PENDIENTES
                int iAnteriorAnioMes = 0;
                CaiAjusteDTO dtoAjusteAnterior = this.servicioCalculoPorcentajes.GetByIdCaiAjusteAnterior(caiajcodi);
                if (dtoAjusteAnterior != null)
                {
                    iAnteriorAnioMes = dtoAjusteAnterior.Caiajanio * 100 + dtoAjusteAnterior.Caiajmes;
                }
                for (int i = 0; i < model.ListaPeriodos.Count; i++)
                {
                    int iFechaPeriodo = Convert.ToInt32(model.ListaPeriodos[i].FechaPeriodo.ToString("yyyyMM"));
                    if (iFechaPeriodo < iAnteriorAnioMes)
                    {
                        model.ListaPeriodos[i].Periodo += " REPORTADO";
                    }
                    else
                    {
                        model.ListaPeriodos[i].Periodo += " PENDIENTE";
                    }
                }
            }
            else
            {
                //PROYECTADO
                model.ListaPeriodos = this.servicioCalculoPorcentajes.ObtenerListaPeriodoProyectado(sCaiajetipoinfo, caiajcodi, emprcodi);
            }
            model.IdEmpresa = emprcodi;
            model.Caiprscodi = caiprscodi;
            model.Caiajcodi = caiajcodi;
            model.Tipoemprcodi = tipoemprcodi;
            model.Emprcodi = emprcodi;
            model.Intervalo = intervalo;
            return View(model);
        }

        /// <summary>
        /// Permite obtener el último envio
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerUltimoEnvio(int idEmpresa, string mes, int Tipoemprcodi, string Intervalo)
        {
            Log.Info("Entidad Formato - GetByIdMeFormato");
            int IdFormato = 0;
            if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiUsuarioLibre && Intervalo.Equals("P"))
            {
                //FORMATCODI = 89 / CAI-INF PROYECTADA USUARIOS LIBRES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaUsuariosLibres;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("E"))
            {
                //FORMATCODI = 87 / CAI-INF EJECUTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoEjecutadaDistribuidores;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("P"))
            {
                //FORMATCODI = 88 / CAI-INF PROYECTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaDistribuidores;
            }
            else
            {
                return Json("Error de asiganción de formato");
            }
            MeFormatoDTO formato = logic.GetByIdMeFormato(IdFormato);
            DateTime fecha = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes.Substring(3), string.Empty, string.Empty, Constantes.FormatoFecha);
            Log.Info("Lista Envio - GetByCriteriaMeEnvios");
            List<MeEnvioDTO> list = this.logic.GetByCriteriaMeEnvios(idEmpresa, IdFormato, fecha);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (list.Count > 0)
            {
                idUltEnvio = list[list.Count - 1].Enviocodi;
            }
            return Json(idUltEnvio);
        }

        /// <summary>
        /// Muestra El formato Excel en la Web 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="desEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarHojaExcelWeb(int idEmpresa, int idEnvio, string mes, int Tipoemprcodi, string Intervalo)
        {
            Log.Info("Lista Formato - GetByModuloLecturaMeFormatos");
            //MODCODI = 11 / CALCULO DE PORCENTAJES
            int IdLectura = 0;
            int IdFormato = 0;
            if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiUsuarioLibre && Intervalo.Equals("P"))
            {
                //LECTCODI = 218 / CAI-INF PROYECTADA USUARIOS LIBRES
                IdLectura = ConstantesCalculoPorcentajes.IdLecturaProyectadaUsuariosLibres;
                //FORMATCODI = 89 / CAI-INF PROYECTADA USUARIOS LIBRES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaUsuariosLibres;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("E"))
            {
                //LECTCODI = 216 / INF. EJECUTADA DISTRIBUIDORES
                IdLectura = ConstantesCalculoPorcentajes.IdLecturaEjecutadaDistribuidores;
                //FORMATCODI = 87 / CAI-INF EJECUTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoEjecutadaDistribuidores;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("P"))
            {
                //LECTCODI = 217 / CAI-INF PROYECTADA DISTRIBUIDORES
                IdLectura = ConstantesCalculoPorcentajes.IdLecturaProyectadaDistribuidores;
                //FORMATCODI = 88 / CAI-INF PROYECTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaDistribuidores;
            }
            else
            {
                return Json("Error de asiganción de Lectura");
            }

            List<MeFormatoDTO> entitys = this.logic.GetByModuloLecturaMeFormatos(ConstantesCalculoPorcentajes.IdModuloCalculoPorcentajes, IdLectura, idEmpresa);
            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcel(idEmpresa, idEnvio, mes.Substring(3), IdFormato);
                Session["DatosJSON"] = jsModel.Handson.ListaExcelData;
                jsModel.Handson.ListaExcelData = new string[0][];
                return Json(jsModel);
            }
            else
            {
                Session["DatosJSON"] = null;
                return Json(-1);
            }
        }

        /// <summary>
        ///Devuelve el model necesario para mostrar en la web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcel(int idEmpresa, int idEnvio, string mes, int IdFormato)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();
            model.IdEnvio = idEnvio;
            ////////// Obtiene el Fotmato ////////////////////////
            Log.Info("Entidad Formato - GetByIdMeFormato");
            model.Formato = logic.GetByIdMeFormato(IdFormato);
            this.Formato = model.Formato;

            Log.Info("Lista Cabecera - GetListMeCabecera");
            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            model.Formato.Formatcols = cabercera.Cabcolumnas;
            model.Formato.Formatrows = cabercera.Cabfilas;
            model.Formato.Formatheaderrow = cabercera.Cabcampodef;

            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;

            int idCfgFormato = 0;
            if (idEnvio <= 0)
            {
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                Log.Info("Entidad Envio - GetByIdMeEnvio");
                var envioAnt = logic.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
            }

            Log.Info("Lista Hojaptomed - GetByCriteria2MeHojaptomeds");
            model.ListaHojaPto = this.logic.GetByCriteria2MeHojaptomeds(idEmpresa, IdFormato, cabercera.Cabquery, model.Formato.FechaProceso, model.Formato.FechaProceso.AddMonths(1).AddDays(-1));
            var cabecerasRow = model.Formato.Formatheaderrow.Split(QueryParametros.SeparadorFila);
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(QueryParametros.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }

            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            FormatoDemandaHelper.GetSizeFormato2(model.Formato);
            Log.Info("Lista Envio - GetByCriteriaMeEnvios");
            model.ListaEnvios = this.logic.GetByCriteriaMeEnvios(idEmpresa, IdFormato, model.Formato.FechaInicio);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (model.ListaEnvios.Count > 0)
            {
                idUltEnvio = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                var reg = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                if (reg != null)
                    model.FechaEnvio = ((DateTime)reg.Enviofecha).ToString(Constantes.FormatoFechaHora);
            }
            /// Verifica si Formato esta en Plazo
            string mensaje = string.Empty;
            int horaini = 0;//Para Formato Tiempo Real
            int horafin = 0;//Para Formato Tiempo Real

            model.EnPlazo = ValidarPlazo(model.Formato);
            if ((idEnvio <= 0) || (idEnvio == idUltEnvio)) // id envio < 0 => es una carga de archivo excel, id envio = 0 => envio nuevo 
            {
                model.Handson.ReadOnly = !ValidarFecha(model.Formato, idEmpresa, out horaini, out horafin);//ValidarFecha(idEmpresa, model.Formato.FechaProceso, idFormato, out mensaje);
                //model.Handson.ReadOnly = !model.EnPlazo;
            }
            else /// id envio > 0 => reenvio
            { /// Es solo para visualizar envios anteriores
                model.Handson.ReadOnly = true;
            }
            Log.Info("Entidad Empresa - GetByIdSiEmpresa");
            var entEmpresa = this.logic.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
                model.Empresa = entEmpresa.Emprnomb;
            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaInicio.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);

            model.Dia = model.Formato.FechaInicio.Day.ToString();
            model.Handson.Width = HandsonConstantes.ColWidth * ((model.ListaHojaPto.Count > HandsonConstantes.ColPorHoja) ? HandsonConstantes.ColPorHoja :
                (model.ListaHojaPto.Count + model.ColumnasCabecera));
            //Genera La vista html complementaria a la grilla Handson, nombre de formato, area coes, fecha de formato, etc.
            model.ViewHtml = FormatoDemandaHelper.GenerarFormatoHtml(model, idEnvio, model.EnPlazo);

            List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.
            int nCol = model.ListaHojaPto.Count;
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia;
            model.Handson.ListaFilaReadOnly = FormatoDemandaHelper.InicializaListaFilaReadOnly(model.Formato.Formatrows, nBloques, !model.Handson.ReadOnly, horaini, horafin);
            model.ListaCambios = new List<CeldaCambios>();
            model.Handson.ListaExcelData = FormatoDemandaHelper.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);
            if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            {
                Log.Info("Obtener Datos Enviados - GetDataFormato");
                lista = this.logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                if (idEnvio > 0) //Si se esta consultando un envio anterior se obtienen los cambios de ese envio.
                    Log.Info("Lista CambioEnvio - GetAllCambioEnvio");
                listaCambios = this.logic.GetAllCambioEnvio(IdFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                /// Cargar Datos en Arreglo para Web
                FormatoDemandaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }
            else
            { // los datos para visualizar en el excel web provienen de un archivo excel cargado por el usuario
                //Carga de archivo Excel
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                FormatoDemandaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }

            #region Filas Cabeceras

            for (var ind = 0; ind < model.ColumnasCabecera; ind++)
            {
                model.Handson.ListaColWidth.Add(160);
            }
            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = model.ColumnasCabecera;
            var cellMerge = new CeldaMerge();
            foreach (var reg in model.ListaHojaPto)
            {
                model.Handson.ListaColWidth.Add(100);
                for (var w = 0; w < model.FilasCabecera; w++)
                {
                    if (column == model.ColumnasCabecera)
                    {
                        model.Handson.ListaExcelData[w] = new string[model.ListaHojaPto.Count + model.ColumnasCabecera];
                        model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                    }
                    var valor = reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null);

                    if (valor != null)
                        model.Handson.ListaExcelData[w][column] = valor.ToString();
                    else
                        model.Handson.ListaExcelData[w][column] = string.Empty;

                    if (listaCabeceraRow[w].IsMerge == 1)
                    {
                        if (listaCabeceraRow[w].TituloRowAnt != model.Handson.ListaExcelData[w][column])
                        {
                            if (column != model.ColumnasCabecera)
                            {
                                if ((column - listaCabeceraRow[w].ColumnIni) > 1)
                                {
                                    cellMerge = new CeldaMerge();
                                    cellMerge.col = listaCabeceraRow[w].ColumnIni;
                                    cellMerge.row = w;
                                    cellMerge.colspan = (column - listaCabeceraRow[w].ColumnIni);
                                    cellMerge.rowspan = 1;
                                    model.Handson.ListaMerge.Add(cellMerge);
                                }
                            }
                            listaCabeceraRow[w].TituloRowAnt = model.Handson.ListaExcelData[w][column];
                            listaCabeceraRow[w].ColumnIni = column;
                        }
                    }
                }
                column++;

            }
            //if ((column - 1) != model.ColumnasCabecera)
            //{
            //    for (var i = 0; i < listaCabeceraRow.Count; i++)
            //    {
            //        if ((listaCabeceraRow[i].TituloRowAnt == model.Handson.ListaExcelData[i][column - 1]))
            //        {
            //            if ((column - listaCabeceraRow[i].ColumnIni) > 1)
            //            {
            //                cellMerge = new CeldaMerge();
            //                cellMerge.col = listaCabeceraRow[i].ColumnIni;
            //                cellMerge.row = i;
            //                cellMerge.colspan = (column - listaCabeceraRow[i].ColumnIni);
            //                cellMerge.rowspan = 1;
            //                model.Handson.ListaMerge.Add(cellMerge);
            //            }
            //        }
            //    }
            //}

            #endregion

            return model;
        }

        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuyera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        protected bool ValidarPlazo(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Valida la fecha
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        protected bool ValidarFecha(MeFormatoDTO formato, int idEmpresa, out int horaini, out int horafin)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            else
            {
                Log.Info("Entidad AmpliacionFecha - GetByIdMeAmpliacionfecha");
                var regfechaPlazo = this.logic.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
            }
            if ((formato.Formatdiaplazo == 0) && (resultado)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;
                if (((hora - 1) % 3) == 0)
                {
                    horaini = hora - 1 - 1 * 3;
                    horafin = hora - 1;
                }
                else
                {
                    horafin = -1;//indica que formato tiempo real no tiene filas habilitadas
                    resultado = false;
                }
            }
            return true;
            //return resultado;
        }

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DescargarDatos()
        {
            string[][] list = (string[][])Session["DatosJSON"];

            var data = list;
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(data);
            result.ContentType = "application/json";

            return result;
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int idEmpresa, string mes, int Tipoemprcodi, string Intervalo)
        {
            int indicador = 0;
            int idEnvio = 0;
            int IdFormato = 0;
            if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiUsuarioLibre && Intervalo.Equals("P"))
            {
                //FORMATCODI = 89 / CAI-INF PROYECTADA USUARIOS LIBRES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaUsuariosLibres;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("E"))
            {
                //FORMATCODI = 87 / CAI-INF EJECUTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoEjecutadaDistribuidores;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("P"))
            {
                //FORMATCODI = 88 / CAI-INF PROYECTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaDistribuidores;
            }
            else
            {
                return Json("Error de asiganción de formato");
            }
            try
            {
                FormatoModel model = BuildHojaExcel(idEmpresa, idEnvio, mes.Substring(3), IdFormato);
                FormatoDemandaHelper.GenerarFileExcel(model);
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras] + this.Formato.Formatnombre + ".xlsx";
            return File(fullPath, Constantes.AppExcel, this.Formato.Formatnombre + ".xlsx");
        }

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            MeArchivoDTO archivo = new MeArchivoDTO();
            MeEnvioDTO envio = new MeEnvioDTO();
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.FileName = fileRandom + "." + "xlsx"; //NombreArchivoDemanda.ExtensionFileUploadHidrologia;
                    string fileName = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.FileName;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lee datos desde excel
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult LeerFileUpExcel(int idEmpresa, string mes, int Tipoemprcodi, string Intervalo)
        {
            int IdFormato = 0;
            if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiUsuarioLibre && Intervalo.Equals("P"))
            {
                //FORMATCODI = 89 / CAI-INF PROYECTADA USUARIOS LIBRES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaUsuariosLibres;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("E"))
            {
                //FORMATCODI = 87 / CAI-INF EJECUTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoEjecutadaDistribuidores;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("P"))
            {
                //FORMATCODI = 88 / CAI-INF PROYECTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaDistribuidores;
            }
            else
            {
                return Json("Error de asiganción de formato");
            }
            int retorno = FormatoDemandaHelper.VerificarIdsFormato(this.NombreFile, idEmpresa, IdFormato);

            if (retorno > 0)
            {
                Log.Info("Entidad Formato - GetByIdMeFormato");
                MeFormatoDTO formato = logic.GetByIdMeFormato(IdFormato);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes.Substring(3), string.Empty, string.Empty, Constantes.FormatoFecha);
                formato.FechaProceso = fechaProceso;
                Log.Info("Lista Cabecera - GetListMeCabecera");
                var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                Log.Info("Lista Hojaptomed - GetByCriteria2MeHojaptomeds");
                var listaPtos = this.logic.GetByCriteria2MeHojaptomeds(idEmpresa, IdFormato, cabercera.Cabquery, formato.FechaProceso, formato.FechaProceso.AddMonths(1).AddDays(-1));
                int nCol = listaPtos.Count;
                int horizonte = formato.Formathorizonte;
                FormatoDemandaHelper.GetSizeFormato2(formato);
                int nBloques = formato.RowPorDia * formato.Formathorizonte;
                this.MatrizExcel = FormatoDemandaHelper.InicializaMatrizExcel(cabercera.Cabfilas, nBloques, cabercera.Cabcolumnas, nCol);
                Boolean isValido = FormatoDemandaHelper.LeerExcelFile(this.MatrizExcel, this.NombreFile, cabercera.Cabfilas, nBloques, cabercera.Cabcolumnas, nCol);
            }
            FormatoDemandaHelper.BorrarArchivo(this.NombreFile);
            return Json(retorno);
        }

        /// <summary>
        /// Graba los datos del archivo Excel Web
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string dataExcel, int idEmpresa, string mes, int Tipoemprcodi, string Intervalo)
        {
            int IdFormato = 0;
            if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiUsuarioLibre && Intervalo.Equals("P"))
            {
                //FORMATCODI = 89 / CAI-INF PROYECTADA USUARIOS LIBRES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaUsuariosLibres;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("E"))
            {
                //FORMATCODI = 87 / CAI-INF EJECUTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoEjecutadaDistribuidores;
            }
            else if (Tipoemprcodi == ConstantesCalculoPorcentajes.TipoEmprCodiDistribuidor && Intervalo.Equals("P"))
            {
                //FORMATCODI = 88 / CAI-INF PROYECTADA DISTRIBUIDORES
                IdFormato = ConstantesCalculoPorcentajes.IdFormatoProyectadaDistribuidores;
            }
            else
            {
                return Json("Error de asiganción de formato");
            }
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            int exito = 0;
            List<string> celdas = new List<string>();
            celdas = dataExcel.Split(',').ToList();
            string empresa = string.Empty;
            Log.Info("Entidad Empresa - GetByIdSiEmpresa");
            var regEmp = this.logic.GetByIdSiEmpresa(idEmpresa);
            //////////////////////////////////////////////////
            if (regEmp != null)
                empresa = regEmp.Emprnomb;

            Log.Info("Entidad Formato - GetByIdMeFormato");
            MeFormatoDTO formato = this.logic.GetByIdMeFormato(IdFormato);

            Log.Info("List Cabecera - GetListMeCabecera");
            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();

            int filaHead = cabercera.Cabfilas;
            int colHead = cabercera.Cabcolumnas;
            Log.Info("Lista Hojaptomed - GetByCriteriaMeHojaptomeds");
            var listaPto = this.servicioCalculoPorcentajes.GetByCriteriaMeHojaptomeds(idEmpresa, IdFormato);
            //this.logic.GetByCriteriaMeHojaptomeds(idEmpresa, IdFormato);
            int nPtos = listaPto.Count();

            /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
            formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes.Substring(3), string.Empty, string.Empty, Constantes.FormatoFecha);
            FormatoDemandaHelper.GetSizeFormato2(formato);
            //////////////////////////////////////////////////////////////////////////

            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = IdFormato;
            config.Emprcodi = idEmpresa;
            Log.Info("Graba Configuracion Formato Envio - GrabarConfigFormatEnvio");
            int idConfig = this.servicioCalculoPorcentajes.GrabarConfigFormatEnvio(config); //int idConfig = logic.GrabarConfigFormatEnvio(config);


            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            Boolean enPlazo = ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = idEmpresa;
            envio.Enviofecha = DateTime.Now;
            envio.Enviofechaperiodo = formato.FechaInicio;
            envio.Envioplazo = (enPlazo) ? "P" : "F";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = DateTime.Now;
            envio.Lastuser = User.Identity.Name;
            envio.Userlogin = User.Identity.Name;
            envio.Formatcodi = IdFormato;
            Log.Info("Inserta registro - SaveMeEnvio");
            this.IdEnvio = logic.SaveMeEnvio(envio);
            model.IdEnvio = this.IdEnvio;
            ///////////////////////////////////////////////////////
            int horizonte = formato.Formathorizonte;
            switch (formato.Formatresolucion)
            {
                case 15:
                    //ConstantesDemanda.ResolucionCuartoHora
                    int total = (nPtos + formato.Formatcols) * (filaHead + 96 * formato.Formathorizonte);
                    int totalRecibido = celdas.Count;

                    var lista96 = FormatoDemandaHelper.LeerExcelWeb96(celdas, listaPto, (int)formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 4 * formato.Formathorizonte, formato.Formatcheckblanco);
                    if (lista96.Count > 0)
                    {
                        try
                        {
                            Log.Info("Graba Datos Cargados - GrabarValoresCargados96");
                            this.logic.GrabarValoresCargados96(lista96, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            envio.Cfgenvcodi = idConfig;
                            Log.Info("Actualiza registro - UpdateMeEnvio");
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Mensaje = "El envío se realizó correctamente";
                        }
                        catch
                        {
                            exito = -1;
                            model.Resultado = -1;
                        }
                    }
                    else
                    {
                        exito = -2;
                        model.Resultado = -2;
                    }
                    break;
            }

            model.Resultado = exito;
            //Enviar Correo de exito de envio
            //FormatoWebHelper.EnviarCorreo(formato.Formatnombre, enPlazo, empresa, formato.FechaInicio, formato.FechaFin, formato.Areaname, User.Identity.Name, (DateTime)envio.Enviofecha, envio.Enviocodi);
            return Json(model);
        }

        #endregion

    }
}
