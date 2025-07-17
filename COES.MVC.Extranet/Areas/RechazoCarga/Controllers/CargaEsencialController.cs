using COES.Dominio.DTO.Sic;
//using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.RechazoCarga.Models;
using COES.MVC.Extranet.Areas.RechazoCarga.Helper;
//using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.RechazoCarga;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using System.Configuration;

namespace COES.MVC.Extranet.Areas.RechazoCarga.Controllers
{
    public class CargaEsencialController : BaseController
    {
        //
        // GET: /RechazoCarga/CargaEsencial/
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);
        private const string _estadoRegistroNoEliminado = "1";
        private const string _estadoRegistroEmpresaActivo = "A";
        private const int _tipoEmpresaUsuarioLibre = 4;
        private const int _familiaEquipo = 45;
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CargaEsencialController));

        public CargaEsencialController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("CargaEsencialController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("CargaEsencialController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {           
            return View();
        }
        
        public ActionResult CargaEsencial()
        {           
            base.ValidarSesionUsuario();
            CargaEsencialModel model = new CargaEsencialModel();

            List<SiEmpresaDTO> listEmpresas = (new FormatoMedicionAppServicio()).GetListaEmpresaFormato(this.IdFormato);

            bool permisoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);

            if (permisoEmpresas)
            {
                model.ListSiEmpresa = this.seguridad.ListarEmpresas().Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
                    OrderBy(x => x.EMPRNOMB).Select(x => new SiEmpresaDTO
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB,
                        Emprrazsocial = x.EMPRRAZSOCIAL
                    }).ToList();
            }
            else
            {
                model.ListSiEmpresa = this.seguridad.ObtenerEmpresasActivasPorUsuario(User.Identity.Name).
                    Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).OrderBy(x => x.EMPRRAZSOCIAL).Select(x => new SiEmpresaDTO
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB,
                        Emprrazsocial = x.EMPRRAZSOCIAL
                    }).ToList();
            }
            
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListarCargaEsencial(string razonSocial, string documento, string cargaIni, 
            string cargaFin, string fecIni, string fecFin)
        {
            CargaEsencialModel model = new CargaEsencialModel();
            //model.ListCargaEsencial = servicio.ListarCargaEsencialFiltro(ConstantesRechazoCarga.EstadoVigente.ToString(), razonSocial, documento, cargaIni, 
            //    cargaFin, fecIni, fecFin, _estadoRegistroNoEliminado, ConstantesRechazoCarga.OrigenExtranet);
            model.ListCargaEsencial = servicio.ListarCargaEsencialExcel(ConstantesRechazoCarga.EstadoVigente.ToString(), razonSocial, documento, cargaIni,
               cargaFin, fecIni, fecFin, _estadoRegistroNoEliminado, ConstantesRechazoCarga.OrigenExtranet);
            var siteRoot = Url.Content("~/");
            model.urlDescarga = siteRoot + ConstantesRechazoCarga.RutaCarga;
            return PartialView("ListarCargaEsencial", model);
        }

        public JsonResult ObtenerCargaEsencial(int rccarecodi)
        {
            RcaCargaEsencialDTO oRcaCargaEsencialDTO = new RcaCargaEsencialDTO();
            oRcaCargaEsencialDTO = servicio.ObtenerCargaEsencialPorCodigo(rccarecodi);
            if (oRcaCargaEsencialDTO.Tipoemprcodi.Equals(_tipoEmpresaUsuarioLibre))
            {
                oRcaCargaEsencialDTO.EsUsuarioLibre = true;
            }
            else
            {
                oRcaCargaEsencialDTO.EsUsuarioLibre = false;
            }
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oRcaCargaEsencialDTO));
        }

        [HttpPost]
        public PartialViewResult ListarCargaEsencialHistorial(int emprcodi, int equicodi)
        {
            CargaEsencialModel model = new CargaEsencialModel();
            model.ListCargaEsencial = servicio.ListarCargaEsencialHistorial(emprcodi, equicodi, _estadoRegistroNoEliminado);

            return PartialView("ListarCargaEsencialHistorial", model);
        }

        /// <summary>
        /// Elimina una carga esencial
        /// </summary>
        /// <param name="rccarecodi"></param>
        /// <returns></returns>
        public ActionResult EliminarCargaEsencial(int rccarecodi)
        {            
            this.servicio.DeleteRcaCargaEsencial(rccarecodi);
            
            return Json(new { success = true, message = "Ok" });
        }

        [HttpGet]
        public ActionResult EditCargaEsencial()
        {
            base.ValidarSesionUsuario();
            EditCargaEsencialModel model = new EditCargaEsencialModel();

            List<SiEmpresaDTO> listEmpresas = (new FormatoMedicionAppServicio()).GetListaEmpresaFormato(this.IdFormato);

            bool permisoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);

            if (permisoEmpresas)
            {
                model.ListSiEmpresa = this.seguridad.ListarEmpresas().Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
                    OrderBy(x => x.EMPRNOMB).Select(x => new SiEmpresaDTO
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB,
                        Emprrazsocial = x.EMPRRAZSOCIAL
                    }).ToList();
            }
            else
            {
                model.ListSiEmpresa = this.seguridad.ObtenerEmpresasActivasPorUsuario(User.Identity.Name).
                    Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).OrderBy(x => x.EMPRRAZSOCIAL).Select(x => new SiEmpresaDTO
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB,
                        Emprrazsocial = x.EMPRRAZSOCIAL
                    }).ToList();
            }                      

            return View("EditCargaEsencial", model);
        }

        [HttpPost]
        public PartialViewResult ListarEmpresas(string empresa, int tipoEmpresa)
        {            
            CargaEsencialModel model = new CargaEsencialModel();
            model.ListSiEmpresa = this.servicio.ListaEmpresasRechazoCarga(empresa, tipoEmpresa, _estadoRegistroEmpresaActivo).OrderBy(p=>p.Emprrazsocial).ToList();
            return PartialView("ListarEmpresas", model);
        }

        /// <summary>
        /// Guarda o actualiza una Carga Esencial
        /// </summary>
        /// <param name="codigoCargaEsencial"></param>
        /// <param name="empresa"></param>
        /// <param name="puntoMedicion"></param>
        /// <param name="carga"></param>
        /// <param name="documento"></param>
        /// <param name="fechaRecepcion"></param>
        /// <param name="estado"></param>
        /// <param name="archivo"></param>
        /// <param name="esNuevo"></param>
        /// <returns></returns>
        public ActionResult GuardarCargaEsencial(int codigoCargaEsencial, int empresa, int puntoMedicion, decimal carga, 
            string archivo, bool esNuevo)
        {
            RcaCargaEsencialDTO oRcaCargaEsencialDTO = new RcaCargaEsencialDTO();

            oRcaCargaEsencialDTO.Emprcodi = empresa;
            oRcaCargaEsencialDTO.Equicodi = puntoMedicion;
            oRcaCargaEsencialDTO.Rccarecarga = carga;
            oRcaCargaEsencialDTO.Rccaredocumento = archivo; 
            //var rccarefecharecepcion = DateTime.ParseExact(fechaRecepcion, "dd/MM/yyyy", null);
            //oRcaCargaEsencialDTO.Rccarefecharecepcion = new DateTime(rccarefecharecepcion.Year, rccarefecharecepcion.Month, rccarefecharecepcion.Day
            //    , DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);            
            oRcaCargaEsencialDTO.Rccarefecharecepcion = DateTime.Now;
            oRcaCargaEsencialDTO.Rccareestado = ConstantesRechazoCarga.EstadoVigente.ToString();
            oRcaCargaEsencialDTO.Rccarenombarchivo = archivo;
            oRcaCargaEsencialDTO.Rccareestregistro = _estadoRegistroNoEliminado;
            oRcaCargaEsencialDTO.Rccareusucreacion = User.Identity.Name;
            oRcaCargaEsencialDTO.Rccarefeccreacion = DateTime.Now;
            oRcaCargaEsencialDTO.Rccareusumodificacion = User.Identity.Name;
            oRcaCargaEsencialDTO.Rccarefecmodificacion = DateTime.Now;
            oRcaCargaEsencialDTO.Rccareorigen = ConstantesRechazoCarga.OrigenExtranet;

            if (esNuevo)
            {
                this.servicio.SaveRcaCargaEsencial(oRcaCargaEsencialDTO);
            }
            else
            {
                oRcaCargaEsencialDTO.Rccarecodi = codigoCargaEsencial;
                this.servicio.UpdateRcaCargaEsencial(oRcaCargaEsencialDTO);
            }           

            return Json(new { success = true, message = "Ok" });
        }

        /// <summary>
        /// Obtiene la lista de punto de medición, segun código de Empresa seleccionado
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public JsonResult ObtenerListaPuntoMedicion(int codigoEmpresa)
        {
            List<EqEquipoDTO> listaPuntoMedicion = this.servicio.ObtenerEquiposPorFamilia(codigoEmpresa, _familiaEquipo);
            
            return Json(listaPuntoMedicion);
        }
        public ActionResult Upload(string fecha)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesRechazoCarga.RutaCarga;

                string extension = string.Empty;
                string nombreArchivo = string.Empty;
                string nombreArchivoFinal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(file.FileName);

                    extension = System.IO.Path.GetExtension(file.FileName);
                    nombreArchivoFinal = nombreArchivo + "_" + fecha + extension;
                    string fileName = path + nombreArchivoFinal;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    //archivo.Nombre = fileName;                    
                    file.SaveAs(fileName);
                }
               
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                
            }
            catch(Exception ex)
            {
                Log.Fatal("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}