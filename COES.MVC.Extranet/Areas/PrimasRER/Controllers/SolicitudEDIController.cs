using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Web.Mvc;
using COES.MVC.Extranet.Areas.PrimasRER.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.PrimasRER;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using System.Reflection;
using System.Configuration;
using COES.Dominio.DTO.Sic;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using System.Globalization;
using COES.Framework.Base.Tools;

namespace COES.MVC.Extranet.Areas.PrimasRER.Controllers
{
    public class SolicitudEDIController : BaseController
    {
        // GET: PrimasRER/SolicitudEDI

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        INDAppServicio servicioIndisponibilidad = new INDAppServicio();
        PrimasRERAppServicio servicioPrimasRER = new PrimasRERAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public SolicitudEDIController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

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

        public ActionResult Index(int pericodi = 0)
        {
            base.ValidarSesionUsuario();

            #region Autentificando Empresa

            PrimasRERModel model = new PrimasRERModel();
            int iEmprCodi = 0;
            List<SeguridadServicio.EmpresaDTO> listTotal = ListaTotalEmpresas();

            TempData["RER_Emprnro"] = listTotal.Count;
            if (listTotal.Count == 1)
            {
                TempData["RER_Emprnomb"] = listTotal[0].EMPRNOMB;
                Session["RER_Emprcodi"] = listTotal[0].EMPRCODI;
                model.EntidadEmpresa = this.servicioEmpresa.GetByIdEmpresa(listTotal[0].EMPRCODI);
            }
            else if (Session["RER_Emprcodi"] != null)
            {
                iEmprCodi = Convert.ToInt32(Session["RER_Emprcodi"].ToString());
                model.EntidadEmpresa = this.servicioEmpresa.GetByIdEmpresa(iEmprCodi);
                TempData["RER_Emprnomb"] = model.EntidadEmpresa.EmprNombre;
            }
            else if (listTotal.Count > 1)
            {
                TempData["RER_Emprnomb"] = "";
                model.Pericodi = pericodi;
                return View("Index", model);
            }
            else
            {
                //No hay empresa asociada a la cuenta
                TempData["RER_Emprnomb"] = "";
                TempData["RER_Emprnro"] = -1;
                model.Pericodi = pericodi;
                return View("Index", model);
            }

            #endregion

            #region INDEX
            model.ListaPeriodos = this.servicioIndisponibilidad.ListPeriodo()
                                        .Where(x => x.Iperihorizonte == ConstantesIndisponibilidades.HorizonteMensual)
                                        .ToList();
            if (model.ListaPeriodos != null && model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                //El periodo por defecto es el mes anterior a la fecha del sistema
                DateTime fechaSistema = DateTime.Now;
                int iperianiomes = int.Parse($"{fechaSistema.Year}{fechaSistema.AddMonths(-1).Month:D2}");
                var periodoAnterior = model.ListaPeriodos.FirstOrDefault(per => per.Iperianiomes == iperianiomes);
                pericodi = (periodoAnterior != null) ? periodoAnterior.Ipericodi : model.ListaPeriodos[0].Ipericodi;
            }

            model.Pericodi = pericodi;
            model.bNuevo = true;//(base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name));
            #endregion
            return View("Index", model);
        }

        #region Elegir / Cambiar de Empresa
        [HttpPost]
        public ActionResult EscogerEmpresa()
        {
            base.ValidarSesionUsuario();
            List<SeguridadServicio.EmpresaDTO> listTotal = ListaTotalEmpresas().OrderBy(empresa => empresa.EMPRNOMB).ToList();
            PrimasRERModel model = new PrimasRERModel
            {
                ListaEmpresas = new List<COES.Dominio.DTO.Transferencias.EmpresaDTO>()
            };
            foreach (var item in listTotal)
            {
                model.ListaEmpresas.Add(new COES.Dominio.DTO.Transferencias.EmpresaDTO { EmprCodi = item.EMPRCODI, EmprNombre = item.EMPRNOMB });
            }

            if (Session["RER_Emprcodi"] != null)
            {
                int iEmprCodi = Convert.ToInt32(Session["RER_Emprcodi"].ToString());
                model.EntidadEmpresa = model.ListaEmpresas.FirstOrDefault(empr => empr.EmprCodi == iEmprCodi);
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmpresaElegida(int EmprCodi)
        {
            COES.Dominio.DTO.Transferencias.EmpresaDTO empresaDTO = this.servicioEmpresa.GetByIdEmpresa(EmprCodi);

            if (empresaDTO != null)
            {
                Session["RER_Emprcodi"] = empresaDTO.EmprCodi;
            }

            return RedirectToAction("Index");
        }
        #endregion

        public JsonResult ValidarPeriodo(int ipericodi) {
            PrimasRERModel model = new PrimasRERModel
            {
                bAccion = AccesoAccionSolicitudEDI(ipericodi, out string mensajeError),
                sMensajeError = mensajeError
            };
            return Json(model);
        }

        /// <summary>
        /// Permite listar las solicitudes EDI
        /// </summary>
        /// <param name="ipericodi">periodo</param>
        /// <returns></returns>
        public PartialViewResult Listado(int ipericodi)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            int emprcodi = Convert.ToInt32(Session["RER_Emprcodi"].ToString());
            bool accesoAccion = AccesoAccionSolicitudEDI(ipericodi, out _);

            model.EntidadEmpresa = this.servicioEmpresa.GetByIdEmpresa(emprcodi);
            model.ListaSolicitudEDI = this.servicioPrimasRER.ListarSolicitudesEDIPorEmpresaYPeriodo(emprcodi, ipericodi);
            model.bEditar = accesoAccion; 
            model.bEliminar = accesoAccion; 

            return PartialView(model);
        }

        public ActionResult New(int ipericodi)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel
            {
                EntidadSolicitudEDI = new Dominio.DTO.Transferencias.RerSolicitudEdiDTO(),
                EntidadEnergiaUnidad = new Dominio.DTO.Transferencias.RerEnergiaUnidadDTO()
            };
            if (model.EntidadSolicitudEDI == null || model.EntidadEnergiaUnidad == null)
            {
                return HttpNotFound();
            }

            int emprcodi = Convert.ToInt32(Session["RER_Emprcodi"].ToString());
            model.EntidadSolicitudEDI.Rersedcodi = 0;
            model.EntidadSolicitudEDI.Emprcodi = emprcodi;
            model.EntidadSolicitudEDI.Ipericodi = ipericodi;
            model.EntidadSolicitudEDI.Rercencodi = 0;
            model.EntidadSolicitudEDI.Reroricodi = 0;
            model.EntidadEnergiaUnidad.Rereuenergiaunidad = "";

            DateTime periodo;
            try
            {
                IndPeriodoDTO indPeriodo = servicioIndisponibilidad.GetByIdIndPeriodo(ipericodi);
                model.iperimes = indPeriodo.Iperimes;
                model.iperianio = indPeriodo.Iperianio;
                periodo = new DateTime(indPeriodo.Iperianio, indPeriodo.Iperimes, 1);
                model.Fechainicio = periodo.ToString("dd/MM/yyyy");
                model.Fechafin = periodo.ToString("dd/MM/yyyy");
                model.Horainicio = periodo.ToString("HH:mm");
                model.Horafin = periodo.ToString("HH:mm");
            }
            catch (Exception ex)
            {
                TempData["sMensajeError"] = "Se ha producido un error";
                return new RedirectResult(Url.Action("Index", "SolicitudEDI"));
            }

            Log.Info("ListaCentral - ListarCentralesRER");
            model.ListaCentral = this.servicioPrimasRER.ListarRerCentralPorEmpresaYContrato(emprcodi, periodo);
            model.EntidadCentralRER = new Dominio.DTO.Transferencias.RerCentralDTO
            {
                Rercencodi = model.EntidadSolicitudEDI.Rercencodi
            };

            Log.Info("ListaOrigen - ListarOrigen");
            model.ListaOrigen = this.servicioPrimasRER.ListarOrigen();
            model.EntidadOrigen = new Dominio.DTO.Transferencias.RerOrigenDTO
            {
                Reroricodi = model.EntidadSolicitudEDI.Reroricodi
            };

            TempData["sMaxSizeSustento"] = ConfigurationManager.AppSettings[ConstantesPrimasRER.MaxSizeSustento].ToString();

            model.bGrabar = AccesoAccionSolicitudEDI(ipericodi, out _); //base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult Edit(int rersedcodi = 0)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            Log.Info("EntidadSolicitudEDI - BuscarSolicitudEDI");
            model.EntidadSolicitudEDI = this.servicioPrimasRER.BuscarSolicitudEDI(rersedcodi);
            if (model.EntidadSolicitudEDI == null)
            {
                return HttpNotFound();
            }

            TempData["sMaxSizeSustento"] = ConfigurationManager.AppSettings[ConstantesPrimasRER.MaxSizeSustento].ToString();

            DateTime periodo;
            try
            {
                IndPeriodoDTO indPeriodo = servicioIndisponibilidad.GetByIdIndPeriodo(model.EntidadSolicitudEDI.Ipericodi);
                periodo = new DateTime(indPeriodo.Iperianio, indPeriodo.Iperimes, 1);
                model.iperimes = indPeriodo.Iperimes;
                model.iperianio = indPeriodo.Iperianio;
            }
            catch (Exception ex)
            {
                TempData["sMensajeError"] = "Se ha producido un error";
                return new RedirectResult(Url.Action("Index", "SolicitudEDI"));
            }

            model.Fechainicio = model.EntidadSolicitudEDI.Rersedfechahorainicio.ToString("dd/MM/yyyy");
            model.Fechafin = model.EntidadSolicitudEDI.Rersedfechahorafin.ToString("dd/MM/yyyy");
            model.Horainicio = model.EntidadSolicitudEDI.Rersedfechahorainicio.ToString("HH:mm");
            model.Horafin = model.EntidadSolicitudEDI.Rersedfechahorafin.ToString("HH:mm");

            Log.Info("ListaCentral - ListarCentralesRER");
            model.ListaCentral = this.servicioPrimasRER.ListarRerCentralPorEmpresaYContrato(model.EntidadSolicitudEDI.Emprcodi, periodo);
            model.EntidadCentralRER = new Dominio.DTO.Transferencias.RerCentralDTO
            {
                Rercencodi = model.EntidadSolicitudEDI.Rercencodi
            };
            if (model.ListaCentral != null && model.ListaCentral.Count > 0)
            {
                model.EntidadCentralRER.Equinomb = model.ListaCentral.Where(c => c.Rercencodi == model.EntidadSolicitudEDI.Rercencodi).First().Equinomb;
            }

            Log.Info("ListaOrigen - ListarOrigen");
            model.ListaOrigen = this.servicioPrimasRER.ListarOrigen();
            model.EntidadOrigen = new Dominio.DTO.Transferencias.RerOrigenDTO
            {
                Reroricodi = model.EntidadSolicitudEDI.Reroricodi
            };

            TempData["sMaxSizeSustento"] = ConfigurationManager.AppSettings[ConstantesPrimasRER.MaxSizeSustento].ToString();

            model.bGrabar = AccesoAccionSolicitudEDI(model.EntidadSolicitudEDI.Ipericodi, out _); //base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(PrimasRERModel model)
        {
            base.ValidarSesionUsuario();
            string fileName = string.Empty;

            bool esNuevaSolicitud = false;
            model.EsNuevaSolicitud = false;
            if (model != null && model.EntidadSolicitudEDI != null)
            {
                esNuevaSolicitud = model.EntidadSolicitudEDI.Rersedcodi == 0;
                model.EsNuevaSolicitud = esNuevaSolicitud;
            }

            try
            {
                bool seguridadEmpresa = ListaTotalEmpresas().Any(emp => emp.EMPRCODI == model.EntidadSolicitudEDI.Emprcodi);
                if (seguridadEmpresa)
                {
                    var file = model.ArchivoSustento[0];

                    //Contruir las fechas según los parámetros
                    model.EntidadSolicitudEDI.Rersedfechahorainicio = UtilPrimasRER.ConstruirDateTime(model.Fechainicio + " " + model.Horainicio, ConstantesPrimasRER.FormatoFechaHora);
                    model.EntidadSolicitudEDI.Rersedfechahorafin = UtilPrimasRER.ConstruirDateTime(model.Fechafin + " " + model.Horafin, ConstantesPrimasRER.FormatoFechaHora);

                    bool esSolicitudValida = this.servicioPrimasRER.ValidarSolicitudEDI(model.EntidadSolicitudEDI, (file != null), out List<string> listaErrores, out List<string> listaInfo);

                    if (esSolicitudValida)
                    {
                        #region Recuperar lista energía unidad del formulario
                        List<COES.Dominio.DTO.Transferencias.RerEnergiaUnidadDTO> listaEnergiaUnidad;
                        if (model.jsonListaEnergiaUnidad == null)
                        {
                            listaEnergiaUnidad = new List<COES.Dominio.DTO.Transferencias.RerEnergiaUnidadDTO>();
                        }
                        else
                        {
                            listaEnergiaUnidad = JsonConvert.DeserializeObject<List<COES.Dominio.DTO.Transferencias.RerEnergiaUnidadDTO>>(model.jsonListaEnergiaUnidad);
                        }
                        #endregion

                        #region Validación Energía Unidad
                        bool esEnergiaUnidadValida = true;
                        StringBuilder MensajeError = new StringBuilder();
                        bool modificoEnergiaUnidad = (model.EntidadSolicitudEDI.Rersedcodi > 0 && (listaEnergiaUnidad != null && listaEnergiaUnidad.Count > 0));

                        if (esNuevaSolicitud || modificoEnergiaUnidad)
                        {
                            esEnergiaUnidadValida = this.servicioPrimasRER.ValidaListaEnergiaUnidad(
                                listaEnergiaUnidad,
                                model.EntidadSolicitudEDI.Ipericodi,
                                model.EntidadSolicitudEDI.Rercencodi,
                                model.EntidadSolicitudEDI.Rersedfechahorainicio,
                                model.EntidadSolicitudEDI.Rersedfechahorafin,
                                out int RegError,
                                out MensajeError
                            );
                        }
                        #endregion

                        if (esEnergiaUnidadValida)
                        {
                            #region Guardar Sustento
                            string rutaSustentoAnterior = string.Empty;
                            if (file != null)
                            {
                                string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.RutaArchivoSustento].ToString();
                                string extension = string.Empty;
                                string nombreArchivo = string.Empty;
                                var fecha = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                                var rerCentral = this.servicioPrimasRER.GetByIdRerCentral(model.EntidadSolicitudEDI.Rercencodi);
                                extension = System.IO.Path.GetExtension(file.FileName);
                                nombreArchivo = model.EntidadSolicitudEDI.Ipericodi + "_" + rerCentral.Equicodi + "_" + fecha + extension;
                                fileName = path + nombreArchivo;
                                file.SaveAs(fileName);

                                rutaSustentoAnterior = path + model.EntidadSolicitudEDI.Rersedsustento;
                                model.EntidadSolicitudEDI.Rersedsustento = nombreArchivo;
                            }
                            #endregion
                            #region Guardar Solicitud EDI
                            Log.Info("Guardar información - GuardarSolicitudEDI");
                            this.servicioPrimasRER.GuardarSolicitudEDI(model.EntidadSolicitudEDI, listaEnergiaUnidad, User.Identity.Name, rutaSustentoAnterior);
                            #endregion
                            #region Index
                            TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                            if (listaInfo.Count > 0)
                            {
                                TempData["sMensajeExito"] = string.Join("</br>", listaInfo);
                            }
                            return new RedirectResult(Url.Action("Index", "SolicitudEDI", new { pericodi = model.EntidadSolicitudEDI.Ipericodi }));
                            #endregion
                        }
                        else
                        {
                            #region Cargar datos del formulario
                            IndPeriodoDTO indPeriodo = servicioIndisponibilidad.GetByIdIndPeriodo(model.EntidadSolicitudEDI.Ipericodi);
                            DateTime periodo = new DateTime(indPeriodo.Iperianio, indPeriodo.Iperimes, 1);

                            Log.Info("ListaCentral - ListarCentralesRER");
                            model.ListaCentral = this.servicioPrimasRER.ListarRerCentralPorEmpresaYContrato(model.EntidadSolicitudEDI.Emprcodi, periodo);
                            model.EntidadCentralRER = new Dominio.DTO.Transferencias.RerCentralDTO
                            {
                                Rercencodi = model.EntidadSolicitudEDI.Rercencodi
                            };
                            if (model.ListaCentral != null && model.ListaCentral.Count > 0)
                            {
                                model.EntidadCentralRER.Equinomb = model.ListaCentral.Where(c => c.Rercencodi == model.EntidadSolicitudEDI.Rercencodi).First().Equinomb;
                            }

                            Log.Info("ListaOrigen - ListarOrigen");
                            model.ListaOrigen = this.servicioPrimasRER.ListarOrigen();
                            model.EntidadOrigen = new Dominio.DTO.Transferencias.RerOrigenDTO
                            {
                                Reroricodi = model.EntidadSolicitudEDI.Reroricodi
                            };
                            TempData["sMaxSizeSustento"] = ConfigurationManager.AppSettings[ConstantesPrimasRER.MaxSizeSustento].ToString();

                            model.sMensajeError = MensajeError.ToString();
                            model.bGrabar = AccesoAccionSolicitudEDI(model.EntidadSolicitudEDI.Ipericodi, out _); //base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                            return PartialView(model);
                            #endregion
                        }
                    }
                    else
                    {
                        #region Cargar datos del formulario
                        IndPeriodoDTO indPeriodo = servicioIndisponibilidad.GetByIdIndPeriodo(model.EntidadSolicitudEDI.Ipericodi);
                        DateTime periodo = new DateTime(indPeriodo.Iperianio, indPeriodo.Iperimes, 1);

                        Log.Info("ListaCentral - ListarCentralesRER");
                        model.ListaCentral = this.servicioPrimasRER.ListarRerCentralPorEmpresaYContrato(model.EntidadSolicitudEDI.Emprcodi, periodo);
                        model.EntidadCentralRER = new Dominio.DTO.Transferencias.RerCentralDTO
                        {
                            Rercencodi = model.EntidadSolicitudEDI.Rercencodi
                        };
                        if (model.ListaCentral != null && model.ListaCentral.Count > 0)
                        {
                            model.EntidadCentralRER.Equinomb = model.ListaCentral.Where(c => c.Rercencodi == model.EntidadSolicitudEDI.Rercencodi).First().Equinomb;
                        }

                        Log.Info("ListaOrigen - ListarOrigen");
                        model.ListaOrigen = this.servicioPrimasRER.ListarOrigen();
                        model.EntidadOrigen = new Dominio.DTO.Transferencias.RerOrigenDTO
                        {
                            Reroricodi = model.EntidadSolicitudEDI.Reroricodi
                        };
                        TempData["sMaxSizeSustento"] = ConfigurationManager.AppSettings[ConstantesPrimasRER.MaxSizeSustento].ToString();
                        
                        model.sMensajeError = string.Join("</br>", listaErrores).Replace("\r\n", "");
                        model.bGrabar = AccesoAccionSolicitudEDI(model.EntidadSolicitudEDI.Ipericodi, out _); //base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                        return PartialView(model);
                        #endregion
                    }
                }
                else
                {
                    TempData["sMensajeError"] = "Se ha producido un error al guardar la información";
                    return new RedirectResult(Url.Action("Index", "SolicitudEDI", new { pericodi = model.EntidadSolicitudEDI.Ipericodi }));
                }
            }
            catch (Exception ex)
            {
                #region Cargar datos del formulario
                IndPeriodoDTO indPeriodo = servicioIndisponibilidad.GetByIdIndPeriodo(model.EntidadSolicitudEDI.Ipericodi);
                DateTime periodo = new DateTime(indPeriodo.Iperianio, indPeriodo.Iperimes, 1);

                Log.Info("ListaCentral - ListarCentralesRER");
                model.ListaCentral = this.servicioPrimasRER.ListarRerCentralPorEmpresaYContrato(model.EntidadSolicitudEDI.Emprcodi, periodo);
                model.EntidadCentralRER = new Dominio.DTO.Transferencias.RerCentralDTO
                {
                    Rercencodi = model.EntidadSolicitudEDI.Rercencodi
                };
                if (model.ListaCentral != null && model.ListaCentral.Count > 0) { 
                    model.EntidadCentralRER.Equinomb = model.ListaCentral.Where(c => c.Rercencodi == model.EntidadSolicitudEDI.Rercencodi).First().Equinomb;
                }
                Log.Info("ListaOrigen - ListarOrigen");
                model.ListaOrigen = this.servicioPrimasRER.ListarOrigen();
                model.EntidadOrigen = new Dominio.DTO.Transferencias.RerOrigenDTO
                {
                    Reroricodi = model.EntidadSolicitudEDI.Reroricodi
                };
                TempData["sMaxSizeSustento"] = ConfigurationManager.AppSettings[ConstantesPrimasRER.MaxSizeSustento].ToString();
                
                model.sMensajeError = "Se ha producido un error al insertar la información";
                model.bGrabar = AccesoAccionSolicitudEDI(model.EntidadSolicitudEDI.Ipericodi, out _);//base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                #endregion

                //Borrar archivo sustento si se llegó a crear
                if (fileName != String.Empty && System.IO.File.Exists(fileName)) 
                {
                    System.IO.File.Delete(fileName);
                }

                return PartialView(model);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int rersedcodi = 0)
        {
            base.ValidarSesionUsuario();
            Log.Info("Eliminar el registro - EliminarSolicitudEDI");
            this.servicioPrimasRER.EliminarSolicitudEDI(rersedcodi, User.Identity.Name);
            return "true";
        }

        public ActionResult View(int rersedcodi = 0)
        {
            PrimasRERModel model = new PrimasRERModel();
            Log.Info("EntidadSolicitudEDI - BuscarSolicitudEDI");
            model.EntidadSolicitudEDI = this.servicioPrimasRER.BuscarSolicitudEDIView(rersedcodi);
            IndPeriodoDTO indPeriodo = servicioIndisponibilidad.GetByIdIndPeriodo(model.EntidadSolicitudEDI.Ipericodi);
            model.iperimes = indPeriodo.Iperimes;
            model.iperianio = indPeriodo.Iperianio;
            model.Fechainicio = model.EntidadSolicitudEDI.Rersedfechahorainicio.ToString(ConstantesPrimasRER.FormatoFechaHora);
            model.Fechafin = model.EntidadSolicitudEDI.Rersedfechahorafin.ToString(ConstantesPrimasRER.FormatoFechaHora);
            return PartialView(model);
        }

        #region Métodos privados
        private List<SeguridadServicio.EmpresaDTO> ListaTotalEmpresas()
        {
            List<SeguridadServicio.EmpresaDTO> listTotal;
            bool accesoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);
            if (accesoEmpresas)
            {
                SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
                listTotal = seguridad.ListarEmpresas().Where(x => x.TIPOEMPRCODI == 3 || x.EMPRCODI == 11772 || x.EMPRCODI == 13 || x.EMPRCODI == 67).OrderBy(x => x.EMPRNOMB).ToList();
            }
            else
            {
                listTotal = base.ListaEmpresas;
            }
            return listTotal;
        }

        private bool AccesoAccionSolicitudEDI(int ipericodi, out string mensajeError)
        {
            DateTime fechaSistema = DateTime.Now;
            DateTime? plazoCierreExtranet = this.servicioPrimasRER.ObtenerPlazoCierreExtranet(ipericodi, out mensajeError);
            if (plazoCierreExtranet != null && fechaSistema <= plazoCierreExtranet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Descargar Archivo Excel
        /// <summary>
        /// Exporta un archivo excel con el formato para ingresar la Energía por Unidad de una central
        /// </summary>
        /// <returns>Retorna el nombre del archivo exportado. En caso de haber error, retorna -1</returns>
        public JsonResult ExportarEnergiaUnidadExcel(int iMesPeriodoEDI, int iAnioPeriodoEDI, int rersedcodi, int rercencodi, string fechainicio, string horainicio, string fechafin, string horafin)
        {
            PrimasRERModel model = new PrimasRERModel
            {
                sMensajeError = string.Empty
            };
            string rutaArchivo;
            StringBuilder metodo = new StringBuilder();
            metodo.Append(NameController);
            metodo.Append(".ExportarEnergiaUnidadExcel(int iMesPeriodoEDI, int iAnioPeriodoEDI, int rersedcodi, int rercencodi, string fechainicio, string horainicio, string fechafin, string horafin)");
            metodo.Append("- iMesPeriodoEDI = ");
            metodo.Append(iMesPeriodoEDI);
            metodo.Append("- iAnioPeriodoEDI = ");
            metodo.Append(iAnioPeriodoEDI);
            metodo.Append("- rersedcodi = ");
            metodo.Append(rersedcodi);
            metodo.Append(", rercencodi = ");
            metodo.Append(rercencodi);
            metodo.Append(", fechainicio = ");
            metodo.Append(fechainicio);
            metodo.Append(", horainicio = ");
            metodo.Append(horainicio);
            metodo.Append(", fechafin = ");
            metodo.Append(fechafin);
            metodo.Append(", horafin = ");
            metodo.Append(horafin);
            try
            {
                //Obtener la fecha y hora según los datos del formulario
                DateTime rersedfechahorainicio = UtilPrimasRER.ConstruirDateTime(fechainicio + " " + horainicio, ConstantesPrimasRER.FormatoFechaHora);
                DateTime rersedfechahorafin = UtilPrimasRER.ConstruirDateTime(fechafin + " " + horafin, ConstantesPrimasRER.FormatoFechaHora);

                #region Valida que la fechaHora fin sea menor o igual que "00:00" del 1er dia del mes siguiente de fechaHoraInio
                DateTime dMaximoDiaFin = new DateTime(rersedfechahorainicio.Year, rersedfechahorainicio.Month, 1, 0, 0, 0).AddMonths(1);
                string sMaximoDiaFin = dMaximoDiaFin.ToString("dd/MM/yyyy");
                DateTime dMaximoDiaPeriodoEDI = new DateTime(iAnioPeriodoEDI, iMesPeriodoEDI, 1, 0, 0, 0).AddMonths(1);
                string sMaximoDiaPeriodoEDI = dMaximoDiaPeriodoEDI.ToString("dd/MM/yyyy");

                if (dMaximoDiaFin < rersedfechahorafin) {
                    throw new Exception("La fecha y hora de fin debe ser menor o igual a " + sMaximoDiaFin + " 00:00");
                }

                if (dMaximoDiaPeriodoEDI < rersedfechahorafin)
                {
                    throw new Exception("La fecha y hora de fin debe ser menor o igual a " + sMaximoDiaPeriodoEDI + " 00:00");
                }
                #endregion

                rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.RutaArchivoEnergiaUnidad].ToString();
                Log.Info("Generar Excel energía unidad");
                List<RerExcelHoja> listaExcelHoja = this.servicioPrimasRER.GenerarExcelEnergiaUnidad(rersedcodi, rercencodi, rersedfechahorainicio, rersedfechahorafin);
                Log.Info("Exportación excel");
                model.nombreArchivo = this.servicioPrimasRER.ExportarReporteaExcel(listaExcelHoja, rutaArchivo, "ArchivoRegistrosMWh", false);
                return Json(model);
            }
            catch (Exception e)
            {
                metodo.Append(" , e.Message: ");
                metodo.Append(e.Message);
                Log.Error(metodo.ToString());
                model.sMensajeError = e.Message;
                return Json(model);
            }
        }

        /// <summary>
        /// Descargar archivo
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo a descargar</param>
        /// <param name="tipoRuta">Tipo de archivo</param>
        /// <returns>Retorna el archivo descargado</returns>
        public virtual ActionResult AbrirArchivo(string nombreArchivo, int tipoRuta = 1)
        {
            StringBuilder rutaNombreArchivo = new StringBuilder();
            if (tipoRuta == 1)
            {
                rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesPrimasRER.RutaArchivoEnergiaUnidad].ToString());
            }
            else {
                rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesPrimasRER.RutaArchivoSustento].ToString());
            }
            rutaNombreArchivo.Append(nombreArchivo);

            StringBuilder rutaNombreArchivoDescarga = new StringBuilder();
            rutaNombreArchivoDescarga.Append(nombreArchivo);

            if (tipoRuta == 1)
            {
                string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                //Borrar archivo
                var bytes = System.IO.File.ReadAllBytes(rutaNombreArchivo.ToString());
                System.IO.File.Delete(rutaNombreArchivo.ToString());
                return File(bytes, Constantes.AppExcel, sFecha + "_" + rutaNombreArchivoDescarga.ToString());
            }
            else {
                //Archivo sustento no se debe borrar
                return File(rutaNombreArchivo.ToString(), Constantes.AppExcel, rutaNombreArchivoDescarga.ToString());
            }
        }
        #endregion

        #region Cargar Archivo Excel
        /// <summary>
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.RutaArchivoEnergiaUnidad].ToString();
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lee datos desde el archivo excel
        /// </summary>
        [HttpPost]
        public JsonResult ProcesarArchivo(string sarchivo, int rercencodi, int ipericodi, string fechainicio, string horainicio, string fechafin, string horafin)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.RutaArchivoEnergiaUnidad].ToString();
            int iRegError = 0;
            StringBuilder sMensajeError = new StringBuilder();
            try
            {
                //Obtener la fecha y hora según los datos del formulario
                DateTime fechaHoraInicio = UtilPrimasRER.ConstruirDateTime(fechainicio + " " + horainicio, ConstantesPrimasRER.FormatoFechaHora);
                DateTime fechaHoraFin = UtilPrimasRER.ConstruirDateTime(fechafin + " " + horafin, ConstantesPrimasRER.FormatoFechaHora);
                List<Dominio.DTO.Transferencias.RerEnergiaUnidadDTO> listaEnergiaUnidad = null;

                if (fechaHoraInicio < fechaHoraFin)
                {
                    DataSet ds = this.servicioPrimasRER.GeneraDataset(path + sarchivo, 1);
                    //Validación de los intervalos de fechas en el archivo excel, de acuerdo a las fechas ingresadas en el formulario.
                    bool intervalosExcelValidos = this.servicioPrimasRER.ValidarIntervalosEnergiaUnidadExcel(ds, fechaHoraInicio, fechaHoraFin, out int cantIntervalos);
                    if (intervalosExcelValidos)
                    {
                        listaEnergiaUnidad = this.servicioPrimasRER.GenerarListaEnergiaUnidadExcel(ds, rercencodi, fechaHoraInicio, fechaHoraFin, cantIntervalos);
                        if (listaEnergiaUnidad.Count > 0)
                        {
                            this.servicioPrimasRER.ValidaListaEnergiaUnidad(listaEnergiaUnidad, ipericodi, rercencodi, fechaHoraInicio, fechaHoraFin,
                                                                                         out int RegError, out StringBuilder MensajeError);
                            iRegError = RegError;
                            sMensajeError.Clear();
                            sMensajeError.Append(MensajeError);
                        }
                        else
                        {
                            iRegError = -1;
                            sMensajeError.Clear();
                            sMensajeError.Append("La central no tiene unidades.");
                        }
                    }
                    else
                    {
                        iRegError = -1;
                        sMensajeError.Clear();
                        sMensajeError.Append("El rango de la fecha y hora del archivo Excel, no coincide con el rango de la fecha y hora del fórmulario. Descargue el archivo Excel nuevamente y vuelva a intentarlo.");
                    }
                }
                else
                {
                    iRegError = -1;
                    sMensajeError.Clear();
                    sMensajeError.Append("La fecha y hora de fin debe ser mayor a la fecha y hora de inicio.");
                }

                #region Eliminar archivo excel
                if (System.IO.File.Exists(path + sarchivo))
                {
                    System.IO.File.Delete(path + sarchivo);
                }
                #endregion

                if (listaEnergiaUnidad != null) 
                { 
                    model.jsonListaEnergiaUnidad = JsonConvert.SerializeObject(listaEnergiaUnidad);
                    model.EntidadSolicitudEDI = new Dominio.DTO.Transferencias.RerSolicitudEdiDTO
                    {
                        Rersedtotenergia = Math.Round(listaEnergiaUnidad.Sum(eu => eu.Rereutotenergia), 4)
                    };
                }
                model.iRegError = iRegError;
                model.sMensajeError = sMensajeError.ToString();
                model.sMensaje = "Archivo válido";
                return Json(model);
            }
            catch (Exception e)
            {
                #region Eliminar archivo excel
                if (System.IO.File.Exists(path + sarchivo))
                {
                    System.IO.File.Delete(path + sarchivo);
                }
                #endregion
                model.sMensajeError = e.Message;
                model.iRegError = -1;
                return Json(model);
            }
        }

        #endregion
    }
}