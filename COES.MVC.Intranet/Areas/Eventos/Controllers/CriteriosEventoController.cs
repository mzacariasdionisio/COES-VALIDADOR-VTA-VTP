using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Eventos;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Evento;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using log4net;
using COES.MVC.Intranet.Areas.Eventos.Models;
using System.Reflection;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.Evento.Helper;
using System.Linq;
using System.Text;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    //[ValidarSesion]
    public class CriteriosEventoController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        private readonly CriteriosEventoAppServicio serviciosCriteriosEvento = new CriteriosEventoAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private readonly AnalisisFallasAppServicio servAF = new AnalisisFallasAppServicio();
        private readonly EventosAppServicio servicioEvento = new EventosAppServicio();
        private readonly EventoAppServicio servicio = new EventoAppServicio();

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

        #region Analisis de Falla - Criterios
        public ActionResult ListaCriteriosEvento()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            AnalisisFallasAppServicio appAnalisisFallas = new AnalisisFallasAppServicio();

            List<CrCasosEspecialesDTO> lstCespec = serviciosCriteriosEvento.ObtenerCasosEspeciales();
            lstCespec.Add(new CrCasosEspecialesDTO() { CRESPECIALCODI = 0, CREDESCRIPCION = "( TODOS )" });
            ViewBag.CasosEspeciales = lstCespec.OrderBy(x => x.CRESPECIALCODI);

            List<CrCriteriosDTO> lstCrit = serviciosCriteriosEvento.ObtenerCriterios();
            lstCrit.Add(new CrCriteriosDTO() { CRCRITERIOCODI = 0, CREDESCRIPCION = "( TODOS )" });
            ViewBag.Criterios = lstCrit.OrderBy(x => x.CRCRITERIOCODI);

            List<SiEmpresaDTO> ListaEmpresa = servAF.ObtenerEmpresa();
            List<SiEmpresaDTO> ListaEmpresaInvolucrada = new List<SiEmpresaDTO>();
            ListaEmpresaInvolucrada.Add(new SiEmpresaDTO() { Emprcodi = 0, Emprnomb = "( TODOS )" });
            ListaEmpresaInvolucrada.AddRange(ListaEmpresa);
            ViewBag.EmpresaInvolucrada = ListaEmpresaInvolucrada;

            List<EmpresaDTO> ListaEmpresaP =  servicio.ListarEmpresas().Where(x => x.EMPRCODI != 0 && x.EMPRCODI != -1).ToList();
            List<EmpresaDTO> ListaEmpresaPropietaria = new List<EmpresaDTO>();
            ListaEmpresaPropietaria.Add(new EmpresaDTO() { EMPRCODI = 0, EMPRNOMB = "( TODOS )" });
            ListaEmpresaPropietaria.AddRange(ListaEmpresaP);
            ViewBag.EmpresaPropietaria = ListaEmpresaPropietaria;
            ViewBag.TipoEquipo = appAnalisisFallas.ObtenerTipoEquipo();

            ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            ViewBag.FechaFin = DateTime.Now.AddDays(0).ToString(Constantes.FormatoFecha);

            return View();

        }

        [HttpGet]
        public ActionResult DetalleEventoCriterio(int id)
        {
            CriteriosEventoModel modelo = new CriteriosEventoModel();
            try
            {
                CrEventoDTO Evento = serviciosCriteriosEvento.GetByIdCrEvento(id);
                modelo.Crevencodi = id;
                modelo.CodigoEvento = Evento.CODIGO;
                modelo.FechaEvento = Evento.FECHA_EVENTO;
                modelo.NombreEvento = Evento.NOMBRE_EVENTO;
                modelo.Crespecialcodi = Evento.CRESPECIALCODI;
                modelo.CodigoImpugnacion = Evento.CODIGOIMPUGNACION;
                modelo.Impugnacion = Evento.IMPUGNACION;
                modelo.Afeitdecfechaelab = Evento.AFEITDECFECHAELAB;
                modelo.Afecodi = Evento.AFECODI;
                modelo.ListaCasosEspeciales = serviciosCriteriosEvento.ListCasosEspeciales().Where(x=>x.CREESTADO == "A").ToList();
                modelo.ListaCrCriterios = serviciosCriteriosEvento.ObtenerCriterios();

                modelo.EtapaDesicion = serviciosCriteriosEvento.ObtenerCrEtapaEvento(id, 1);
                if (modelo.EtapaDesicion.CREVENTODESCRIPCION == null)
                    modelo.EtapaDesicion.CREVENTODESCRIPCION = Evento.EVENDESCCTAF == null ? Evento.NOMBRE_EVENTO : Evento.EVENDESCCTAF;
                modelo.ListaEmpresaRespDesicion = serviciosCriteriosEvento.ListEmpresaResponsable(modelo.EtapaDesicion.CRETAPACODI);
                List<CrEtapaCriterioDTO> listCriteriosDecision = serviciosCriteriosEvento.ListaCriteriosEtapa(modelo.EtapaDesicion.CRETAPACODI);
                string strlistCriteriosDecision = string.Empty;
                foreach (var item in listCriteriosDecision)
                {
                    strlistCriteriosDecision += item.CRCRITERIOCODI.ToString() + ",";
                }
                modelo.ListaCriteriosDecision = strlistCriteriosDecision;

                modelo.EtapaReconsideracion = serviciosCriteriosEvento.ObtenerCrEtapaEvento(id, 2);
                modelo.ListaEmpresaRespReconsideracion = serviciosCriteriosEvento.ListEmpresaResponsable(modelo.EtapaReconsideracion.CRETAPACODI);
                List<CrEmpresaSolicitanteDTO> lstSolReconsideracion = serviciosCriteriosEvento.ListEmpresaSolicitante(modelo.EtapaReconsideracion.CRETAPACODI);

                foreach (CrEmpresaSolicitanteDTO item in lstSolReconsideracion)
                {
                    item.CRARGUMENTO = item.CRARGUMENTO == null ? "" : item.CRARGUMENTO.Replace("\n", "<br />");
                    item.CRDECISION = item.CRDECISION == null ? "" : item.CRDECISION.Replace("\n", "<br />");
                }
                modelo.ListaEmpresaSolReconsideracion = lstSolReconsideracion;
                List<CrEtapaCriterioDTO> listCriteriosReconsideracion = serviciosCriteriosEvento.ListaCriteriosEtapa(modelo.EtapaReconsideracion.CRETAPACODI);
                string strlistCriteriosReconsideracion = string.Empty;
                foreach (var item in listCriteriosReconsideracion)
                {
                    strlistCriteriosReconsideracion += item.CRCRITERIOCODI.ToString() + ",";
                }
                modelo.ListaCriteriosReconsideracion = strlistCriteriosReconsideracion;

                modelo.EtapaApelacion = serviciosCriteriosEvento.ObtenerCrEtapaEvento(id, 3);
                modelo.ListaEmpresaRespApelacion = serviciosCriteriosEvento.ListEmpresaResponsable(modelo.EtapaApelacion.CRETAPACODI);
                List<CrEmpresaSolicitanteDTO> lstSolApelacion = serviciosCriteriosEvento.ListEmpresaSolicitante(modelo.EtapaApelacion.CRETAPACODI);
                foreach (CrEmpresaSolicitanteDTO item in lstSolApelacion)
                {
                    item.CRARGUMENTO = item.CRARGUMENTO == null ? "" : item.CRARGUMENTO.Replace("\n", "<br />");
                    item.CRDECISION = item.CRDECISION == null ? "" : item.CRDECISION.Replace("\n", "<br />");
                }
                modelo.ListaEmpresaSolApelacion = lstSolApelacion;

                List<CrEtapaCriterioDTO> listCriteriosApelacion = serviciosCriteriosEvento.ListaCriteriosEtapa(modelo.EtapaApelacion.CRETAPACODI);
                string strlistCriteriosApelacion = string.Empty;
                foreach (var item in listCriteriosApelacion)
                {
                    strlistCriteriosApelacion += item.CRCRITERIOCODI.ToString() + ",";
                }
                modelo.ListaCriteriosApelacion = strlistCriteriosApelacion;

                modelo.EtapaArbitraje = serviciosCriteriosEvento.ObtenerCrEtapaEvento(id, 4);
                modelo.ListaEmpresaRespArbitraje = serviciosCriteriosEvento.ListEmpresaResponsable(modelo.EtapaArbitraje.CRETAPACODI);
                List<CrEmpresaSolicitanteDTO> lstSolArbitraje = serviciosCriteriosEvento.ListEmpresaSolicitante(modelo.EtapaArbitraje.CRETAPACODI);
                foreach (CrEmpresaSolicitanteDTO item in lstSolArbitraje)
                {
                    item.CRARGUMENTO = item.CRARGUMENTO == null ? "" : item.CRARGUMENTO.Replace("\n", "<br />");
                    item.CRDECISION = item.CRDECISION == null ? "" : item.CRDECISION.Replace("\n", "<br />");
                }
                modelo.ListaEmpresaSolArbitraje = lstSolArbitraje;
                List<CrEtapaCriterioDTO> listCriteriosArbitraje = serviciosCriteriosEvento.ListaCriteriosEtapa(modelo.EtapaArbitraje.CRETAPACODI);
                string strlistCriteriosArbitraje = string.Empty;
                foreach (var item in listCriteriosArbitraje)
                {
                    strlistCriteriosArbitraje += item.CRCRITERIOCODI.ToString() + ",";
                }
                modelo.ListaCriteriosArbitraje = strlistCriteriosArbitraje;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return View(modelo);
        }

        public PartialViewResult BuscarCriterio(Eventos.Models.CriteriosEventoModel modelo)
        {
            CrEventoDTO oCrevento = new CrEventoDTO();
            try
            {
                oCrevento.DI = modelo.DI; //0
                oCrevento.DF = modelo.DF; //1
                oCrevento.EmpresaPropietaria = modelo.EmpresaPropietaria;  //2
                oCrevento.EmpresaInvolucrada = modelo.EmpresaInvolucrada; //3
                oCrevento.CriterioDecision = modelo.CriterioDecision; //4
                if (modelo.CasosEspeciales == null) { modelo.CasosEspeciales = "0"; }
                oCrevento.CasosEspeciales = modelo.CasosEspeciales; //5
                oCrevento.Impugnacionc = modelo.Impugnacionc; //6
                oCrevento.CriteriosImpugnacion = modelo.CriteriosImpugnacion; //7
                oCrevento.NroPagina = modelo.NroPagina;
                oCrevento.NroRegistros = Constantes.PageSizeEvento;
                List<CrEventoDTO> ListaCriterio = this.ListarCrCriteriosEvento(oCrevento, 1);
                modelo.listaCriterios = ListaCriterio;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return PartialView(modelo);
        }
        #endregion
        #region Casos Epeciales
        public ActionResult CasosEspecialesIndex()
        {
            CasosEspecialesModel model = new CasosEspecialesModel();
            model.EntidadCasosEspeciales = new CrCasosEspecialesDTO();
            
            return View(model);
        }
        /// <summary>
        /// Listado de Casos especiales
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CasosEspecialesLista() 
        {
            CasosEspecialesModel model = new CasosEspecialesModel();
            try
            {
                model.ListaCasosEspeciales = this.serviciosCriteriosEvento.ListCasosEspeciales();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }            
            return PartialView(model);
        }
        /// <summary>
        /// Permite mostrar la vista de edición o creación de Casos especiales
        /// </summary>
        /// <param name="CRESPECIALCODI"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CasosEspecialesEdit(int CRESPECIALCODI)
        {
            CasosEspecialesModel model = new CasosEspecialesModel();
            try
            {
                if (CRESPECIALCODI == 0)
                {
                    model.EntidadCasosEspeciales = new CrCasosEspecialesDTO();
                    model.EntidadCasosEspeciales.CREESTADO = Constantes.EstadoActivo;
                }
                else
                {
                    model.EntidadCasosEspeciales = this.serviciosCriteriosEvento.GetByIdCasosEspeciales(CRESPECIALCODI);
                }
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return PartialView(model);
        }
        /// <summary>
        /// Permite almacenar los datos del periodo
        /// </summary>
        /// <param name="crespecialcodi"></param>
        /// <param name="crdescripcion"></param>
        /// <param name="crestado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CasosEspecialesSave(int crespecialcodi, string crdescripcion, string crestado)
        {
            try
            {
                int result = 1;
                CrCasosEspecialesDTO entity = new CrCasosEspecialesDTO();

                entity.CRESPECIALCODI = crespecialcodi;
                entity.CREDESCRIPCION = crdescripcion;
                entity.CREESTADO = crestado;
                entity.LASTDATE = DateTime.Now;
                entity.LASTUSER = base.UserName;
                this.serviciosCriteriosEvento.SaveCasosEspeciales(entity);
                return Json(result);
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite validar si los casos especiales están asignados a algún criterio
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarCasosEspeciales(int crespecialcodi, string crestado)
        {
            try
            {
                int result = 0;
                if(crestado == "I")
                    result = this.serviciosCriteriosEvento.ValidarCasosEspeciales(crespecialcodi);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar los datos del periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CasosEspecialesDelete(int CRESPECIALCODI)
        {
            try
            {
                this.serviciosCriteriosEvento.DeleteCasosEspeciales(CRESPECIALCODI);
                return Json(1);
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        #endregion
        #region Criterios
        public ActionResult CriteriosIndex()
        {
            CriteriosModel model = new CriteriosModel();

            return View(model);
        }
        /// <summary>
        /// Listado de Casos especiales
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CriteriosLista()
        {
            CriteriosModel model = new CriteriosModel();
            try
            {
                model.ListaCriterios = this.serviciosCriteriosEvento.ListCriterios();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);             
            }           
            return PartialView(model);
        }
        /// <summary>
        /// Permite mostrar la vista de edición o creación de Casos especiales
        /// </summary>
        /// <param name="CRESPECIALCODI"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CriteriosEdit(int CRCRITERIOCODI)
        {
            CriteriosModel model = new CriteriosModel();
            try
            {
                if (CRCRITERIOCODI == 0)
                {
                    model.EntidadCriterios = new CrCriteriosDTO();
                    model.EntidadCriterios.CREESTADO = Constantes.EstadoActivo;
                }
                else
                {
                    model.EntidadCriterios = this.serviciosCriteriosEvento.GetByIdCriterios(CRCRITERIOCODI);
                }
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return PartialView(model);
        }
        /// <summary>
        /// Permite almacenar los datos del periodo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CriteriosSave(int crcriteriocodi, string crdescripcion, string crestado)
        {
            try
            {
                int result = 1;

                CrCriteriosDTO entity = new CrCriteriosDTO();

                entity.CRCRITERIOCODI = crcriteriocodi;
                entity.CREDESCRIPCION = crdescripcion;
                entity.CREESTADO = crestado;
                entity.LASTDATE = DateTime.Now;
                entity.LASTUSER = base.UserName;
                this.serviciosCriteriosEvento.SaveCriterios(entity);
                return Json(result);
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite validar si los criterios están asignados a algún criterio
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarCriterios(int crcriteriocodi, string crestado)
        {
            try
            {
                int result = 0;
                if (crestado == "I")
                    result = this.serviciosCriteriosEvento.ValidarCriterios(crcriteriocodi);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar los datos del periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CriteriosDelete(int CRCRITERIOCODI)
        {
            try
            {
                this.serviciosCriteriosEvento.DeleteCriterios(CRCRITERIOCODI);
                return Json(1);
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        #endregion

        /// <summary>
        /// Permite generar el reporte eventos
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CREtapaEventoVerComentario(int CREVENCODI,int CRETAPA)
        {
            try
            {
                string Texto = this.serviciosCriteriosEvento.ObtenerComentarioXEventoyEtapa(CREVENCODI, CRETAPA);
                ViewBag.Comentario = Texto.Replace("\n", "<br />");
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
            }
            return PartialView();
        }
        /// <summary>
        /// Permite generar el reporte eventos
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReporteCriterios(Eventos.Models.CriteriosEventoModel modelo)
        {
            try
            {
                CrEventoDTO oCrevento = new CrEventoDTO();
                oCrevento.DI = modelo.DI; //0
                oCrevento.DF = modelo.DF; //1
                oCrevento.EmpresaPropietaria = modelo.EmpresaPropietaria;  //2
                oCrevento.EmpresaInvolucrada = modelo.EmpresaInvolucrada; //3
                oCrevento.CriterioDecision = modelo.CriterioDecision; //4
                oCrevento.CasosEspeciales = modelo.CasosEspeciales; //5
                oCrevento.Impugnacionc = modelo.Impugnacionc; //6
                oCrevento.CriteriosImpugnacion = modelo.CriteriosImpugnacion; //7
                oCrevento.NroPagina = modelo.NroPagina;
                oCrevento.NroRegistros = Constantes.PageSizeEvento;
                List<CrEventoDTO> ListaCriterio = this.ListarCrCriteriosEvento(oCrevento, 2);
                ExcelDocument.GenerarReporteCriteriosCTAF(ListaCriterio);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = ex }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Permite exportar los datos del reporte de indicadores de CTAF.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarReporteCriterios()
        {
            //string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteIndicadoresCTAF;
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento + NombreArchivo.ReporteCriteriosCTAF;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteCriteriosCTAF);
        }

        /// <summary>
        /// Registro de Empresas Responsable
        /// </summary>
        /// <param name="cretapacodi"></param>
        /// <param name="codigoEmpresa"></param>
        /// <param name="crevencodi"></param>
        /// <param name="etapa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertarResponsable(int cretapacodi, int codigoEmpresa, int crevencodi, int etapa)
        {
            try
            {
                CrEmpresaResponsableDTO entity = new CrEmpresaResponsableDTO();

                if(cretapacodi == 0)
                {
                    var EtapaEventoDTO = serviciosCriteriosEvento.ObtenerCrEtapaEvento(crevencodi, etapa);
                    if(EtapaEventoDTO.CRETAPACODI == 0)
                    {
                        CrEtapaEventoDTO CrEtapa = new CrEtapaEventoDTO();
                        CrEtapa.CREVENCODI = crevencodi;
                        CrEtapa.CRETAPA = etapa;
                        CrEtapa.LASTUSER = base.User.Identity.Name;
                        CrEtapa.LASTDATE = DateTime.Now;
                        cretapacodi = serviciosCriteriosEvento.SaveCrEtapaEvento(CrEtapa);
                    }                
                }

                if (!serviciosCriteriosEvento.ValidarCrEmpresaResponsable(cretapacodi, codigoEmpresa))
                {
                    entity.EMPRCODI = codigoEmpresa;
                    entity.CRETAPACODI = cretapacodi;
                    entity.LASTUSER = base.User.Identity.Name;
                    entity.LASTDATE = DateTime.Now;
                    serviciosCriteriosEvento.SaveCrEmpresaResponsable(entity);
                }
                    
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = ex }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Registro de historico scada
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TraerEmpresaResponsable(int cretapacodi)
        { 
            JsonResult jRespuesta;
            List<CrEmpresaResponsableDTO> Lista = serviciosCriteriosEvento.ListEmpresaResponsable(cretapacodi);
            jRespuesta = Json(Lista, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }

        /// <summary>
        /// Elminar Empresas responsables
        /// <summary>
        /// <param name="crrespemprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEmpresaResponsable(int crrespemprcodi)
        {
            try
            {
                serviciosCriteriosEvento.EliminarCrEmpresaResponsable(crrespemprcodi);
                return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Registro de Empresas Responsable
        /// </summary>
        /// <param name="cretapacodi"></param>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarResponsable(int cretapacodi, int afecodi, int crevencodi, int etapa)
        {
            try
            {
                CrEmpresaResponsableDTO entity = new CrEmpresaResponsableDTO();

                var EtapaEventoDTO = serviciosCriteriosEvento.ObtenerCrEtapaEvento(crevencodi, etapa);
                if (EtapaEventoDTO.CRETAPACODI == 0)
                {
                    CrEtapaEventoDTO CrEtapa = new CrEtapaEventoDTO();
                    CrEtapa.CREVENCODI = crevencodi;
                    CrEtapa.CRETAPA = etapa;
                    CrEtapa.LASTUSER = base.User.Identity.Name;
                    CrEtapa.LASTDATE = DateTime.Now;
                    cretapacodi = serviciosCriteriosEvento.SaveCrEtapaEvento(CrEtapa);
                }

                List<EmpresaResponsableDTO> lsEmpresaResponsableCompensacion = servAF.ObtenerEmpresaResponsableCompensacion(afecodi);

                foreach (EmpresaResponsableDTO item in lsEmpresaResponsableCompensacion)
                {
                    if (!serviciosCriteriosEvento.ValidarCrEmpresaResponsable(cretapacodi, item.EMPRCODI))
                    {
                        entity.EMPRCODI = item.EMPRCODI;
                        entity.CRETAPACODI = cretapacodi;
                        entity.LASTUSER = base.User.Identity.Name;
                        entity.LASTDATE = DateTime.Now;
                        serviciosCriteriosEvento.SaveCrEmpresaResponsable(entity);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = ex }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Registro de Empresas Responsable
        /// </summary>
        /// <param name="cretapacodi"></param>
        /// <param name="codigoEmpresa"></param>
        /// <param name="crevencodi"></param>
        /// <param name="etapa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertarSolicitante(int cretapacodi, int codigoEmpresa, int crevencodi, int etapa, string argumentos, string desicion)
        {
            try
            {
                CrEmpresaSolicitanteDTO entity = new CrEmpresaSolicitanteDTO();

                if (cretapacodi == 0)
                {
                    var EtapaEventoDTO = serviciosCriteriosEvento.ObtenerCrEtapaEvento(crevencodi, etapa);
                    if (EtapaEventoDTO.CRETAPACODI == 0)
                    {
                        CrEtapaEventoDTO CrEtapa = new CrEtapaEventoDTO();
                        CrEtapa.CREVENCODI = crevencodi;
                        CrEtapa.CRETAPA = etapa;
                        CrEtapa.LASTUSER = base.User.Identity.Name;
                        CrEtapa.LASTDATE = DateTime.Now;
                        cretapacodi = serviciosCriteriosEvento.SaveCrEtapaEvento(CrEtapa);
                    }
                }

                if (!serviciosCriteriosEvento.ValidarCrEmpresaSolicitante(cretapacodi, codigoEmpresa))
                {
                    entity.EMPRCODI = codigoEmpresa;
                    entity.CRETAPACODI = cretapacodi;
                    entity.CRARGUMENTO = argumentos;
                    entity.CRDECISION = desicion;
                    entity.LASTUSER = base.User.Identity.Name;
                    entity.LASTDATE = DateTime.Now;
                    serviciosCriteriosEvento.SaveCrEmpresaSolicitante(entity);
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = ex }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lista Empresas solicitante
        /// </summary>
        /// <param name="cretapacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TraerEmpresaSolicitante(int cretapacodi)
        {
            JsonResult jRespuesta;
            List<CrEmpresaSolicitanteDTO> Lista = serviciosCriteriosEvento.ListEmpresaSolicitante(cretapacodi);

            foreach (CrEmpresaSolicitanteDTO item in Lista)
            {
                item.CRARGUMENTO = item.CRARGUMENTO == null ? "" : item.CRARGUMENTO.Replace("\n", "<br />");
                item.CRDECISION = item.CRDECISION == null ? "" : item.CRDECISION.Replace("\n", "<br />");
            }

            jRespuesta = Json(Lista, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }

        /// <summary>
        /// Elminar Empresas solicitante
        /// <summary>
        /// <param name="crsolemprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEmpresaSolicitante(int crsolemprcodi)
        {
            try
            {
                serviciosCriteriosEvento.EliminarCrEmpresaSolicitante(crsolemprcodi);
                return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Registro de Empresas Solicitante
        /// </summary>
        /// <param name="cretapacodi"></param>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarSolicitante(int cretapacodi, int afecodi, int etapa, int crevencodi)
        {
            try
            {
                CrEmpresaSolicitanteDTO entity = new CrEmpresaSolicitanteDTO();

                var EtapaEventoDTO = serviciosCriteriosEvento.ObtenerCrEtapaEvento(crevencodi, etapa);
                if (EtapaEventoDTO.CRETAPACODI == 0)
                {
                    CrEtapaEventoDTO CrEtapa = new CrEtapaEventoDTO();
                    CrEtapa.CREVENCODI = crevencodi;
                    CrEtapa.CRETAPA = etapa;
                    CrEtapa.LASTUSER = base.User.Identity.Name;
                    CrEtapa.LASTDATE = DateTime.Now;
                    cretapacodi = serviciosCriteriosEvento.SaveCrEtapaEvento(CrEtapa);
                }

                if (etapa == 2)
                {
                    List<ReclamoDTO> lsReclamoReconsideracion = servAF.ObtenerReclamoReconsideracionReconsideracion(afecodi);
                    foreach (ReclamoDTO item in lsReclamoReconsideracion)
                    {
                        if (!serviciosCriteriosEvento.ValidarCrEmpresaSolicitante(cretapacodi, item.EMPRCODI))
                        {
                            entity.EMPRCODI = item.EMPRCODI;
                            entity.CRETAPACODI = cretapacodi;
                            entity.LASTUSER = base.User.Identity.Name;
                            entity.LASTDATE = DateTime.Now;
                            serviciosCriteriosEvento.SaveCrEmpresaSolicitante(entity);
                        }
                    }
                }
                else if (etapa == 3)
                {
                    List<ReclamoDTO> lsReclamoApelacion = servAF.ObtenerReclamoApelacionReconsideracion(afecodi);
                    foreach (ReclamoDTO item in lsReclamoApelacion)
                    {
                        if (!serviciosCriteriosEvento.ValidarCrEmpresaSolicitante(cretapacodi, item.EMPRCODI))
                        {
                            entity.EMPRCODI = item.EMPRCODI;
                            entity.CRETAPACODI = cretapacodi;
                            entity.LASTUSER = base.User.Identity.Name;
                            entity.LASTDATE = DateTime.Now;
                            serviciosCriteriosEvento.SaveCrEmpresaSolicitante(entity);
                        }
                    }
                }
                else if (etapa == 4)
                {
                    List<ReclamoDTO> lsReclamoArbitraje = servAF.ObtenerReclamoArbitrajeReconsideracion(afecodi);

                    foreach (ReclamoDTO item in lsReclamoArbitraje)
                    {
                        if (!serviciosCriteriosEvento.ValidarCrEmpresaSolicitante(cretapacodi, item.EMPRCODI))
                        {
                            entity.EMPRCODI = item.EMPRCODI;
                            entity.CRETAPACODI = cretapacodi;
                            entity.LASTUSER = base.User.Identity.Name;
                            entity.LASTDATE = DateTime.Now;
                            serviciosCriteriosEvento.SaveCrEmpresaSolicitante(entity);
                        }
                    }
                }
                    
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = ex }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Registro de Empresas Solicitante
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarCrEventoEtapas(CrEventoDTO obj)
        {
            try
            {
                int etapas = 4;
                for(int x = 1; x <= etapas; x++)
                {
                    var Etapa1EventoDTO = serviciosCriteriosEvento.ObtenerCrEtapaEvento(obj.CREVENCODI, x);
                    CrEtapaEventoDTO CrEtapa1 = new CrEtapaEventoDTO();
                    if (Etapa1EventoDTO.CRETAPACODI == 0)
                    {
                        CrEtapaEventoDTO CrEtapa = new CrEtapaEventoDTO();
                        CrEtapa.CREVENCODI = obj.CREVENCODI;
                        CrEtapa.CRETAPA = x;
                        CrEtapa.LASTUSER = base.User.Identity.Name;
                        CrEtapa.LASTDATE = DateTime.Now;
                        Etapa1EventoDTO.CRETAPACODI = serviciosCriteriosEvento.SaveCrEtapaEvento(CrEtapa);

                        CrEtapa1.CREVENCODI = obj.CREVENCODI;
                        CrEtapa1.CRETAPA = x;
                        
                        if (x == 1)
                        {
                            var codcriterios = obj.CRCRITERIOCODIDESICION.Split(',');
                            if (!string.IsNullOrEmpty(codcriterios.ToString()) && codcriterios[0] != "null")
                            {
                                foreach (var item in codcriterios)
                                {
                                    CrEtapaCriterioDTO etapacriterio = new CrEtapaCriterioDTO();
                                    etapacriterio.CRCRITERIOCODI = Convert.ToInt32(item);
                                    etapacriterio.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                                    serviciosCriteriosEvento.SaveCrEtapaCriterio(etapacriterio);
                                }
                            }
                            CrEtapa1.CREVENTODESCRIPCION = obj.DESCRIPCION_EVENTO_DECISION;
                            CrEtapa1.CRRESUMENCRITERIO = obj.RESUMEN_DECISION;
                            CrEtapa1.CRCOMENTARIOS_RESPONSABLES = obj.COMENTARIO_EMPRESA_DECISION;
                            CrEtapa1.CRFECHDESICION = DateTime.ParseExact(obj.FECHA_DECISION, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        }                           
                        else if (x == 2)
                        {
                            var codcriteriosRec = obj.CRCRITERIOCODIRECONSIDERACION.Split(',');
                            if (!string.IsNullOrEmpty(codcriteriosRec.ToString()) && codcriteriosRec[0] != "null")
                            {
                                foreach (var item in codcriteriosRec)
                                {
                                    CrEtapaCriterioDTO etapacriterio = new CrEtapaCriterioDTO();
                                    etapacriterio.CRCRITERIOCODI = Convert.ToInt32(item);
                                    etapacriterio.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                                    etapacriterio.LASTUSER = base.User.Identity.Name;
                                    etapacriterio.LASTDATE = DateTime.Now;
                                    serviciosCriteriosEvento.SaveCrEtapaCriterio(etapacriterio);
                                }
                            }
                            
                            CrEtapa1.CRCOMENTARIOS_RESPONSABLES = obj.COMENTARIOS_RECONCIDERACION;
                        }                           
                        else if (x == 3)
                        {
                            var codcriteriosApe = obj.CRCRITERIOCODIAPELACION.Split(',');
                            if (!string.IsNullOrEmpty(codcriteriosApe.ToString()) && codcriteriosApe[0] != "null")
                            {
                                foreach (var item in codcriteriosApe)
                                {
                                    CrEtapaCriterioDTO etapacriterio = new CrEtapaCriterioDTO();
                                    etapacriterio.CRCRITERIOCODI = Convert.ToInt32(item);
                                    etapacriterio.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                                    etapacriterio.LASTUSER = base.User.Identity.Name;
                                    etapacriterio.LASTDATE = DateTime.Now;
                                    serviciosCriteriosEvento.SaveCrEtapaCriterio(etapacriterio);
                                }
                            }

                                
                            CrEtapa1.CRCOMENTARIOS_RESPONSABLES = obj.COMENTARIOS_APELACION;
                        }                           
                        else if (x == 4)
                        {
                            var codcriteriosArb = obj.CRCRITERIOCODIARBITRAJE.Split(',');
                            if (!string.IsNullOrEmpty(codcriteriosArb.ToString()) && codcriteriosArb[0] != "null")
                            {
                                foreach (var item in codcriteriosArb)
                                {
                                    CrEtapaCriterioDTO etapacriterio = new CrEtapaCriterioDTO();
                                    etapacriterio.CRCRITERIOCODI = Convert.ToInt32(item);
                                    etapacriterio.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                                    etapacriterio.LASTUSER = base.User.Identity.Name;
                                    etapacriterio.LASTDATE = DateTime.Now;
                                    serviciosCriteriosEvento.SaveCrEtapaCriterio(etapacriterio);
                                }
                            }
                                
                            CrEtapa1.CRCOMENTARIOS_RESPONSABLES = obj.COMENTARIOS_ARBITRAJE;
                        }

                        
                        CrEtapa1.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                        CrEtapa1.LASTUSER = base.User.Identity.Name;
                        CrEtapa1.LASTDATE = DateTime.Now;
                        serviciosCriteriosEvento.UpdateCrEtapaEvento(CrEtapa1);
                    }
                    else if (Etapa1EventoDTO.CRETAPACODI > 0)
                    {
                        CrEtapa1.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                        CrEtapa1.CREVENCODI = obj.CREVENCODI;
                        CrEtapa1.CRETAPA = x;
                        CrEtapa1.CRFECHDESICION = DateTime.ParseExact(obj.FECHA_DECISION, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        if (x == 1)
                        {
                            serviciosCriteriosEvento.DeleteCrEtapaCriterio(Etapa1EventoDTO.CRETAPACODI);
                            var codcriterios = obj.CRCRITERIOCODIDESICION.Split(',');
                            if(codcriterios != null)
                            {
                                foreach (var item in codcriterios)
                                {
                                    if (item != "null")
                                    {
                                        CrEtapaCriterioDTO etapacriterio = new CrEtapaCriterioDTO();
                                        etapacriterio.CRCRITERIOCODI = Convert.ToInt32(item);
                                        etapacriterio.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                                        etapacriterio.LASTUSER = base.User.Identity.Name;
                                        etapacriterio.LASTDATE = DateTime.Now;
                                        serviciosCriteriosEvento.SaveCrEtapaCriterio(etapacriterio);
                                    }

                                }
                            }
                            
                            CrEtapa1.CRCOMENTARIOS_RESPONSABLES = obj.COMENTARIO_EMPRESA_DECISION;
                        }
                        else if (x == 2)
                        {
                            serviciosCriteriosEvento.DeleteCrEtapaCriterio(Etapa1EventoDTO.CRETAPACODI);
                            var codcriteriosRec = obj.CRCRITERIOCODIRECONSIDERACION.Split(',');
                            foreach (var item in codcriteriosRec)
                            {
                                if (item != "null")
                                {
                                    CrEtapaCriterioDTO etapacriterio = new CrEtapaCriterioDTO();
                                    etapacriterio.CRCRITERIOCODI = Convert.ToInt32(item);
                                    etapacriterio.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                                    etapacriterio.LASTUSER = base.User.Identity.Name;
                                    etapacriterio.LASTDATE = DateTime.Now;
                                    serviciosCriteriosEvento.SaveCrEtapaCriterio(etapacriterio);
                                }
                                    
                            }
                            CrEtapa1.CRCOMENTARIOS_RESPONSABLES = obj.COMENTARIOS_RECONCIDERACION;
                        }
                        else if (x == 3)
                        {
                            serviciosCriteriosEvento.DeleteCrEtapaCriterio(Etapa1EventoDTO.CRETAPACODI);
                            var codcriteriosApe = obj.CRCRITERIOCODIAPELACION.Split(',');
                            foreach (var item in codcriteriosApe)
                            {
                                if (item != "null")
                                {
                                    CrEtapaCriterioDTO etapacriterio = new CrEtapaCriterioDTO();
                                    etapacriterio.CRCRITERIOCODI = Convert.ToInt32(item);
                                    etapacriterio.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                                    etapacriterio.LASTUSER = base.User.Identity.Name;
                                    etapacriterio.LASTDATE = DateTime.Now;
                                    serviciosCriteriosEvento.SaveCrEtapaCriterio(etapacriterio);
                                }
                                    
                            }
                            CrEtapa1.CRCOMENTARIOS_RESPONSABLES = obj.COMENTARIOS_APELACION;
                        }
                        else if (x == 4)
                        {
                            serviciosCriteriosEvento.DeleteCrEtapaCriterio(Etapa1EventoDTO.CRETAPACODI);
                            var codcriteriosArb = obj.CRCRITERIOCODIARBITRAJE.Split(',');
                            foreach (var item in codcriteriosArb)
                            {
                                if (item != "null")
                                {
                                    CrEtapaCriterioDTO etapacriterio = new CrEtapaCriterioDTO();
                                    etapacriterio.CRCRITERIOCODI = Convert.ToInt32(item);
                                    etapacriterio.CRETAPACODI = Etapa1EventoDTO.CRETAPACODI;
                                    etapacriterio.LASTUSER = base.User.Identity.Name;
                                    etapacriterio.LASTDATE = DateTime.Now;
                                    serviciosCriteriosEvento.SaveCrEtapaCriterio(etapacriterio);
                                }
                                    
                            }
                            CrEtapa1.CRCOMENTARIOS_RESPONSABLES = obj.COMENTARIOS_ARBITRAJE;
                        }
                        CrEtapa1.CREVENTODESCRIPCION = obj.DESCRIPCION_EVENTO_DECISION;
                        CrEtapa1.CRRESUMENCRITERIO = obj.RESUMEN_DECISION;
                        CrEtapa1.LASTUSER = base.User.Identity.Name;
                        CrEtapa1.LASTDATE = DateTime.Now;
                        serviciosCriteriosEvento.UpdateCrEtapaEvento(CrEtapa1);
                    }
                }
                if(obj.CRESPECIALCODI > 0)
                    serviciosCriteriosEvento.UpdateCrEvento(obj);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = ex }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Detalle de empresa solicitante
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EmpresaSolicitante(int crsolemprcodi, int tipoSolicitante)
        {
            CriteriosEventoModel model = new CriteriosEventoModel();
            model.DetEmpresaSolicitante = this.serviciosCriteriosEvento.ObtenerEmpresaSolicitante(crsolemprcodi);
            model.TipoSolicitante = tipoSolicitante;
            return PartialView(model);
        }
        /// <summary>
        /// Actualizar Empresa Solicitante
        /// </summary>
        /// <param name="crsolemprcodi"></param>
        /// <param name="argumentos"></param>
        /// <param name="desicion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarEmpresaSolicitante(int crsolemprcodi, string argumentos, string desicion)
        {
            try
            {
                serviciosCriteriosEvento.ActualizarCrEmpresaSolicitante(crsolemprcodi, argumentos, desicion);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = ex }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ObtenerEmpresa()
        {
            JsonResult jRespuesta;
            List<SiEmpresaDTO> ListaEmpresa = servAF.ObtenerEmpresa();
            jRespuesta = Json(ListaEmpresa, JsonRequestBehavior.AllowGet);
            jRespuesta.MaxJsonLength = 500000000;
            return jRespuesta;
        }

        private List<CrEventoDTO> ListarCrCriteriosEvento(CrEventoDTO oCrevento, int tipo)
        {
            List<Responsables> lsResponsables = new List<Responsables>();
            List<Solicitantes> lsSolicitante = new List<Solicitantes>();
            List<CrEventoDTO> lsEventos = new List<CrEventoDTO>();
            List<CrEventoDTO> ListaCriterio = serviciosCriteriosEvento.SqlTraerEtapaxEvento(oCrevento).DistinctBy(x => x.CREVENCODI).ToList();
            List<CrEventoDTO> ListaCriterioFinal = new List<CrEventoDTO>();
            List<CrEventoDTO> ListaCriterioFinal2 = new List<CrEventoDTO>();
            var lstetapas = new List<int> { 1, 2, 3, 4 };

            try
            {
                if (Convert.ToInt32(oCrevento.CriterioDecision) > 0 && Convert.ToInt32(oCrevento.CriteriosImpugnacion) > 0)
                {
                    foreach (CrEventoDTO itemCr in ListaCriterio)
                    {
                        bool exDec = false;
                        bool exOtros = false;
                        List<CrEtapaEventoDTO> lstCriterios = serviciosCriteriosEvento.ObtenerCriterioxEtapaEvento(itemCr.CREVENCODI);

                        foreach (var itemCr2 in lstCriterios)
                        {
                            if (itemCr2.CRCRITERIOCODI == Convert.ToInt32(oCrevento.CriterioDecision) && itemCr2.CRETAPA == 1)
                                exDec = true;

                            if (itemCr2.CRCRITERIOCODI == Convert.ToInt32(oCrevento.CriteriosImpugnacion) && itemCr2.CRETAPA != 1)
                                exOtros = true;
                        }
                        if (exDec && exOtros)
                            ListaCriterioFinal.Add(itemCr);
                    }
                }
                else
                    ListaCriterioFinal = ListaCriterio;

                if (Convert.ToInt32(oCrevento.EmpresaInvolucrada) > 0)
                {
                    foreach (CrEventoDTO itemCr3 in ListaCriterioFinal)
                    {
                        bool exisResp = false;
                        bool exisSol = false;
                        List<CrEmpresaResponsableDTO> Responsable = serviciosCriteriosEvento.SqlObtenerEmpresaResponsablexEvento(itemCr3.CREVENCODI);
                        List<CrEmpresaSolicitanteDTO> Solicitante = serviciosCriteriosEvento.SqlObtenerEmpresaSolicitantexEvento(itemCr3.CREVENCODI);
                        foreach (var resp in Responsable)
                        {
                            if (resp.EMPRCODI == Convert.ToInt32(oCrevento.EmpresaInvolucrada))
                                exisResp = true;
                        }
                        foreach (var solc in Solicitante)
                        {
                            if (solc.EMPRCODI == Convert.ToInt32(oCrevento.EmpresaInvolucrada))
                                exisSol = true;
                        }

                        if (exisResp || exisSol)
                            ListaCriterioFinal2.Add(itemCr3);
                    }
                }
                else
                    ListaCriterioFinal2 = ListaCriterioFinal;

                foreach (var item2 in ListaCriterioFinal2)
                {
                    CrEventoDTO oCrevento2 = new CrEventoDTO();
                    oCrevento2.CREVENCODI = item2.CREVENCODI;
                    oCrevento2.CODIGO = item2.CODIGO;
                    oCrevento2.FECHA_EVENTO = item2.FECHA_EVENTO;
                    oCrevento2.NOMBRE_EVENTO = item2.NOMBRE_EVENTO;
                    oCrevento2.CASOS_ESPECIAL = item2.CASOS_ESPECIAL;
                    oCrevento2.IMPUGNACION = item2.IMPUGNACION;
                    foreach (int etap in lstetapas)
                    {
                        item2.ETAPA = etap;
                        CrEtapaEventoDTO Crevento = serviciosCriteriosEvento.ObtenerCrEtapaEvento(item2.CREVENCODI, item2.ETAPA);
                        int CREVENCODI = item2.CREVENCODI;
                        int CRETAPACODI = Crevento.CRETAPACODI;
                        if (item2.ETAPA == 1)
                        {
                            string descipcionEveEvento = string.Empty;
                            oCrevento2.FECHA_DECISION = Crevento.CRFECHDESICION.ToString(Constantes.FormatoFecha) == "01/01/0001" ? item2.AFEITDECFECHAELAB.ToString(Constantes.FormatoFecha) : Crevento.CRFECHDESICION.ToString(Constantes.FormatoFecha);
                            if(tipo == 1)
                            {
                                descipcionEveEvento = item2.EVENDESCCTAF == null ? item2.NOMBRE_EVENTO.Replace("\n", "<br />") : item2.EVENDESCCTAF.Replace("\n", "<br />");
                                oCrevento2.DESCRIPCION_EVENTO_DECISION = Crevento.CREVENTODESCRIPCION == null ? descipcionEveEvento : Crevento.CREVENTODESCRIPCION.Replace("\n", "<br />");
                                oCrevento2.RESUMEN_DECISION = Crevento.CRRESUMENCRITERIO == null ? "" : Crevento.CRRESUMENCRITERIO.Replace("\n", "<br />");
                            }
                            else
                            {
                                descipcionEveEvento = item2.EVENDESCCTAF == null ? item2.NOMBRE_EVENTO : item2.EVENDESCCTAF;
                                oCrevento2.DESCRIPCION_EVENTO_DECISION = Crevento.CREVENTODESCRIPCION == null ? descipcionEveEvento : Crevento.CREVENTODESCRIPCION;
                                oCrevento2.RESUMEN_DECISION = Crevento.CRRESUMENCRITERIO == null ? "" : Crevento.CRRESUMENCRITERIO;
                            }
                                
                            lsResponsables = serviciosCriteriosEvento.SqlObtenerEmpresaResponsable(CREVENCODI, CRETAPACODI);
                            foreach (var res1 in lsResponsables)
                            {
                                if(tipo == 1)
                                    oCrevento2.RESPONSABLE_DECISION += res1.EMPRNOMB + "<br />";
                                else
                                    oCrevento2.RESPONSABLE_DECISION += res1.EMPRNOMB + "\n";
                            }

                            if (Crevento.CRCOMENTARIOS_RESPONSABLES != null)
                            {                               
                                if(tipo == 1)
                                {
                                    if (Crevento.CRCOMENTARIOS_RESPONSABLES.Length > 30)
                                        Crevento.CRCOMENTARIOS_RESPONSABLES = Crevento.CRCOMENTARIOS_RESPONSABLES.Substring(0, 30);
                                    oCrevento2.COMENTARIO_EMPRESA_DECISION = Crevento.CRCOMENTARIOS_RESPONSABLES.Replace("\n", "<br />");
                                }                               
                                else
                                    oCrevento2.COMENTARIO_EMPRESA_DECISION = Crevento.CRCOMENTARIOS_RESPONSABLES;
                            }
                            else { oCrevento2.COMENTARIO_EMPRESA_DECISION = string.Empty; }

                            List<CrEtapaCriterioDTO> lstCrEtapaCriterios = serviciosCriteriosEvento.ListaCriteriosEtapaEvento(Crevento.CRETAPACODI);
                            foreach (var crit1 in lstCrEtapaCriterios)
                            {
                                if(tipo == 1)
                                    oCrevento2.CRITERIO_DECISION += crit1.CREDESCRIPCION + "<br />";
                                else
                                    oCrevento2.CRITERIO_DECISION += crit1.CREDESCRIPCION + "\n";
                            }
                        }
                        if (item2.ETAPA == 2)
                        {
                            lsSolicitante = serviciosCriteriosEvento.SqlObtenerEmpresaSolicitante(CREVENCODI, CRETAPACODI);
                            foreach (var soli2 in lsSolicitante)
                            {
                                if(tipo == 1)
                                {
                                    oCrevento2.EMPR_SOLI_RECONSIDERACION += soli2.EMPRNOMB + "<br />";
                                    oCrevento2.ARGUMENTO_RECONCIDERACION += soli2.CRARGUMENTO == null ? "" : soli2.CRARGUMENTO.Replace("\n", "<br />") + "<br /><br />";
                                    oCrevento2.DECISION_RECONCIDERACION += soli2.CRDECISION == null ? "" : soli2.CRDECISION.Replace("\n", "<br />") + "<br /><br />";
                                }
                                else
                                {
                                    oCrevento2.EMPR_SOLI_RECONSIDERACION += soli2.EMPRNOMB + "\n";
                                    oCrevento2.ARGUMENTO_RECONCIDERACION += soli2.CRARGUMENTO == null ? "" : soli2.CRARGUMENTO + "\n\n";
                                    oCrevento2.DECISION_RECONCIDERACION += soli2.CRDECISION == null ? "" : soli2.CRDECISION + "\n\n";
                                }
                            }

                            lsResponsables = serviciosCriteriosEvento.SqlObtenerEmpresaResponsable(CREVENCODI, CRETAPACODI);
                            foreach (var res2 in lsResponsables)
                            {
                                if(tipo == 1)
                                    oCrevento2.RESPONSABLE_RECONCIDERACION += res2.EMPRNOMB + "<br />";
                                else
                                    oCrevento2.RESPONSABLE_RECONCIDERACION += res2.EMPRNOMB + "\n";
                            }
                            if (Crevento.CRCOMENTARIOS_RESPONSABLES != null)
                            {                            
                                if(tipo == 1)
                                {
                                    if (Crevento.CRCOMENTARIOS_RESPONSABLES.Length > 30)
                                        Crevento.CRCOMENTARIOS_RESPONSABLES = Crevento.CRCOMENTARIOS_RESPONSABLES.Substring(0, 30);
                                    oCrevento2.COMENTARIOS_RECONCIDERACION = Crevento.CRCOMENTARIOS_RESPONSABLES.Replace("\n", "<br />");
                                }                                   
                                else
                                    oCrevento2.COMENTARIOS_RECONCIDERACION = Crevento.CRCOMENTARIOS_RESPONSABLES;
                            }
                            else { oCrevento2.COMENTARIOS_RECONCIDERACION = string.Empty; }

                            List<CrEtapaCriterioDTO> lstCrEtapaCriterios2 = serviciosCriteriosEvento.ListaCriteriosEtapaEvento(Crevento.CRETAPACODI);
                            foreach (var crit2 in lstCrEtapaCriterios2)
                            {
                                if(tipo == 1)
                                    oCrevento2.CRITERIOS_RECONSIDERACION += crit2.CREDESCRIPCION + "<br />";
                                else
                                    oCrevento2.CRITERIOS_RECONSIDERACION += crit2.CREDESCRIPCION + "\n";
                            }
                        }
                        if (item2.ETAPA == 3)
                        {
                            lsSolicitante = serviciosCriteriosEvento.SqlObtenerEmpresaSolicitante(CREVENCODI, CRETAPACODI);
                            foreach (var soli3 in lsSolicitante)
                            {
                                if(tipo == 1)
                                {
                                    oCrevento2.EMPR_SOLI_APELACION += soli3.EMPRNOMB + "<br />";
                                    oCrevento2.ARGUMENTO_APELACION += soli3.CRARGUMENTO == null ? "" : soli3.CRARGUMENTO.Replace("\n", "<br />") + "<br /><br />";
                                    oCrevento2.DECISION_APELACION += soli3.CRDECISION == null ? "" : soli3.CRDECISION.Replace("\n", "<br />") + "<br /><br />";
                                }
                                else
                                {
                                    oCrevento2.EMPR_SOLI_APELACION += soli3.EMPRNOMB + "\n";
                                    oCrevento2.ARGUMENTO_APELACION += soli3.CRARGUMENTO == null ? "" : soli3.CRARGUMENTO + "\n\n";
                                    oCrevento2.DECISION_APELACION += soli3.CRDECISION == null ? "" : soli3.CRDECISION + "\n\n";
                                }
                            }

                            lsResponsables = serviciosCriteriosEvento.SqlObtenerEmpresaResponsable(CREVENCODI, CRETAPACODI);
                            foreach (var res3 in lsResponsables)
                            {
                                if(tipo == 1)
                                    oCrevento2.RESPONSABLE_APELACION += res3.EMPRNOMB + "<br />";
                                else
                                    oCrevento2.RESPONSABLE_APELACION += res3.EMPRNOMB + "\n";
                            }
                            if (Crevento.CRCOMENTARIOS_RESPONSABLES != null)
                            {                              
                                if(tipo == 1)
                                {
                                    if (Crevento.CRCOMENTARIOS_RESPONSABLES.Length > 30)
                                        Crevento.CRCOMENTARIOS_RESPONSABLES = Crevento.CRCOMENTARIOS_RESPONSABLES.Substring(0, 30);
                                    oCrevento2.COMENTARIOS_APELACION = Crevento.CRCOMENTARIOS_RESPONSABLES.Replace("\n", "<br />");
                                }                                   
                                else
                                    oCrevento2.COMENTARIOS_APELACION = Crevento.CRCOMENTARIOS_RESPONSABLES;
                            }
                            else { oCrevento2.COMENTARIOS_APELACION = string.Empty; }

                            List<CrEtapaCriterioDTO> lstCrEtapaCriterios3 = serviciosCriteriosEvento.ListaCriteriosEtapaEvento(Crevento.CRETAPACODI);
                            foreach (var crit3 in lstCrEtapaCriterios3)
                            {
                                if(tipo == 1)
                                    oCrevento2.CRITERIOS_APELACION += crit3.CREDESCRIPCION + "<br />";
                                else
                                    oCrevento2.CRITERIOS_APELACION += crit3.CREDESCRIPCION + "\n";
                            }
                        }
                        if (item2.ETAPA == 4)
                        {
                            lsSolicitante = serviciosCriteriosEvento.SqlObtenerEmpresaSolicitante(CREVENCODI, CRETAPACODI);
                            foreach (var soli4 in lsSolicitante)
                            {
                                if(tipo == 1)
                                {
                                    oCrevento2.EMPR_SOLI_ARBITRAJE += soli4.EMPRNOMB + "<br />";
                                    oCrevento2.ARGUMENTO_ARBITRAJE += soli4.CRARGUMENTO == null ? "" : soli4.CRARGUMENTO.Replace("\n", "<br />") + "<br /><br />";
                                    oCrevento2.DECISION_ARBITRAJE += soli4.CRDECISION == null ? "" : soli4.CRDECISION.Replace("\n", "<br />") + "<br /><br />";
                                }
                                else
                                {
                                    oCrevento2.EMPR_SOLI_ARBITRAJE += soli4.EMPRNOMB + "\n";
                                    oCrevento2.ARGUMENTO_ARBITRAJE += soli4.CRARGUMENTO == null ? "" : soli4.CRARGUMENTO + "\n\n";
                                    oCrevento2.DECISION_ARBITRAJE += soli4.CRDECISION == null ? "" : soli4.CRDECISION + "\n\n";
                                }
                            }

                            lsResponsables = serviciosCriteriosEvento.SqlObtenerEmpresaResponsable(CREVENCODI, CRETAPACODI);
                            foreach (var res4 in lsResponsables)
                            {
                                if(tipo == 1)
                                    oCrevento2.RESPONSABLE_ARBITRAJE += res4.EMPRNOMB + "<br />";
                                else
                                    oCrevento2.RESPONSABLE_ARBITRAJE += res4.EMPRNOMB + "\n";
                            }
                            if (Crevento.CRCOMENTARIOS_RESPONSABLES != null)
                            {                             
                                if(tipo == 1)
                                {
                                    if (Crevento.CRCOMENTARIOS_RESPONSABLES.Length > 30)
                                        Crevento.CRCOMENTARIOS_RESPONSABLES = Crevento.CRCOMENTARIOS_RESPONSABLES.Substring(0, 30);
                                    oCrevento2.COMENTARIOS_ARBITRAJE = Crevento.CRCOMENTARIOS_RESPONSABLES.Replace("\n", "<br />");
                                }                                
                                else
                                    oCrevento2.COMENTARIOS_ARBITRAJE = Crevento.CRCOMENTARIOS_RESPONSABLES;
                            }
                            else { oCrevento2.COMENTARIOS_ARBITRAJE = string.Empty; }

                            List<CrEtapaCriterioDTO> lstCrEtapaCriterios4 = serviciosCriteriosEvento.ListaCriteriosEtapaEvento(Crevento.CRETAPACODI);
                            foreach (var crit4 in lstCrEtapaCriterios4)
                            {
                                if(tipo == 1)
                                    oCrevento2.CRITERIOS_ARBITRAJE += crit4.CREDESCRIPCION + "<br />";
                                else
                                    oCrevento2.CRITERIOS_ARBITRAJE += crit4.CREDESCRIPCION + "\n";
                            }

                        }
                    }
                    lsEventos.Add(oCrevento2);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return lsEventos;
        }
    }
}
