using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.RechazoCarga.Models;
using COES.MVC.Extranet.Areas.RechazoCarga.Helper;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.RechazoCarga;
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
    public class EsquemaUnifilarController : BaseController
    {
        //
        // GET: /RechazoCarga/EsquemaUnifilar/

        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);
        private const int codUsuarioLibre = 4;
        private const string estadoRegistroNoEliminado = "1";
        private const string estadoRegistroEmpresaActivo = "A";
        private const int familiaEquipo = 45;
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EsquemaUnifilarController));

 protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("EsquemaUnifilarController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("EsquemaUnifilarController", ex);
                throw;
            }
        }
        public EsquemaUnifilarController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            EsquemaUnifilarModel model = new EsquemaUnifilarModel();

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
        public PartialViewResult ListarEsquemaUnifilar(string empresa, string codigoSuministro, string fecIni, string fecFin)
        {
            EsquemaUnifilarModel model = new EsquemaUnifilarModel();
            //model.ListEsquemaUnifilar = servicio.ListarEsquemaUnifilarFiltro(empresa, codigoSuministro, fecIni, fecFin, ConstantesRechazoCarga.OrigenExtranet);
            model.ListEsquemaUnifilar = servicio.ListarEsquemaUnifilarExcel(empresa, codigoSuministro, fecIni, fecFin, ConstantesRechazoCarga.OrigenExtranet);
            return PartialView("Lista", model);
        }

        public JsonResult ObtenerListaEmpresas(string empresa)
        {
            List<SiEmpresaDTO> listaEmpresas = this.servicio.ListaEmpresasRechazoCarga(empresa, codUsuarioLibre, estadoRegistroEmpresaActivo);
            
            return Json(listaEmpresas);
        }

        [HttpPost]
        public PartialViewResult ListarEsquemaUnifilarHistorial(int emprcodi, int equicodi)
        {
            EsquemaUnifilarModel model = new EsquemaUnifilarModel();
            model.ListEsquemaUnifilar = servicio.ListarEsquemaUnifilarHistorial(emprcodi, equicodi);

            return PartialView("ListaEsquemaUnifilarHistorial", model);
        }

        public ActionResult EliminarEsquemaUnifilar(int rccarecodi)
        {
            this.servicio.DeleteRcaEsquemaUnifilar(rccarecodi);

            return Json(new { success = true, message = "Ok" });
        }
        public JsonResult ObtenerEsquemaUnifilar(int rccarecodi)
        {
            RcaEsquemaUnifilarDTO oRcaEsquemaUnifilarDTO = new RcaEsquemaUnifilarDTO();//servicio.GetDataByIdHoraOperacion(pecacodi, hopcodi);
            oRcaEsquemaUnifilarDTO = servicio.ObtenerEsquemaUnifilarlPorCodigo(rccarecodi);
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oRcaEsquemaUnifilarDTO));
        }

        public ActionResult GuardarEsquemaUnifilar(int rcesqucodi,int emprcodi, int equicodi, string archivo, bool EsNuevo)
        {
            RcaEsquemaUnifilarDTO rcaEsquemaUnifilarDTO = new RcaEsquemaUnifilarDTO();

            rcaEsquemaUnifilarDTO.Emprcodi = emprcodi;
            rcaEsquemaUnifilarDTO.Equicodi = equicodi;
            rcaEsquemaUnifilarDTO.Rcesqudocumento = archivo;
            //var fechaRecepcion = DateTime.ParseExact(rcesqufecharecepcion, "dd/MM/yyyy", null);
            //rcaEsquemaUnifilarDTO.Rcesqufecharecepcion = new DateTime(fechaRecepcion.Year, fechaRecepcion.Month, fechaRecepcion.Day
            //    , DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            rcaEsquemaUnifilarDTO.Rcesqufecharecepcion = DateTime.Now;
            //rcaEsquemaUnifilarDTO.Rcesqufecharecepcion = DateTime.ParseExact(rcesqufecharecepcion, "dd/MM/yyyy", null);
            rcaEsquemaUnifilarDTO.Rcesquestado = ConstantesRechazoCarga.EstadoVigente.ToString();
            rcaEsquemaUnifilarDTO.Rcesqunombarchivo = archivo;
            rcaEsquemaUnifilarDTO.Rcesquestregistro = estadoRegistroNoEliminado;
            rcaEsquemaUnifilarDTO.Rcesquusucreacion = this.UserName;
            rcaEsquemaUnifilarDTO.Rcesqufeccreacion = DateTime.Now;
            rcaEsquemaUnifilarDTO.Rcesquusumodificacion = this.UserName;
            rcaEsquemaUnifilarDTO.Rcesqufecmodificacion = DateTime.Now;
            rcaEsquemaUnifilarDTO.Rcesquorigen = ConstantesRechazoCarga.OrigenExtranet;

            if (EsNuevo)
            {
                this.servicio.SaveRcaEsquemaUnifilar(rcaEsquemaUnifilarDTO);
            }
            else
            {
                rcaEsquemaUnifilarDTO.Rcesqucodi = rcesqucodi;
                this.servicio.UpdateRcaEsquemaUnifilar(rcaEsquemaUnifilarDTO);
            }
            

            return Json(new { success = true, message = "Ok" });
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
                log.Error("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult EditEsquemaUnifilar()
        {
            base.ValidarSesionUsuario();
            EditEsquemaUnifilarModel model = new EditEsquemaUnifilarModel();

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
            

            return View("EditEsquemaUnifilar", model);
        }

        [HttpPost]
        public PartialViewResult ListarEmpresas(string empresa)
        {
            EsquemaUnifilarModel model = new EsquemaUnifilarModel();
            model.ListSiEmpresa = this.servicio.ListaEmpresasRechazoCarga(empresa, codUsuarioLibre, estadoRegistroEmpresaActivo);
            return PartialView("ListarEmpresas", model);
        }
        public JsonResult ObtenerListaPuntoMedicion(int codigoEmpresa)
        {
            List<EqEquipoDTO> listaPuntoMedicion = this.servicio.ObtenerEquiposPorFamilia(codigoEmpresa, familiaEquipo);

            return Json(listaPuntoMedicion);
        }
    }
}
