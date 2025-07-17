using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.GMM.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.GMM;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using COES.Framework.Base.Tools;
using System.IO;
using COES.Servicios.Aplicacion.General;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Intranet.Areas.GMM.Controllers
{
    public class AgentesController : BaseController
    {
        //
        // GET: /GMM/Agente/
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        AgenteAppServicio servicio = new AgenteAppServicio();
        //
        // GET: /GMM/Garantia/
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        GarantiaAppServicio servicioGarantia = new GarantiaAppServicio();

        private const string _estadoRegistroEmpresaActivo = "A";


        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(AgentesController));

        public AgentesController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("AgentesController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("AgentesController", ex);
                throw;
            }
        }

        private GeneralAppServicio logicGeneral;

        public ActionResult Index()
        {
            FormatoModel model = new FormatoModel();
            logicGeneral = new GeneralAppServicio();
            model.ListaEmpresas = logicGeneral.ListarEmpresasPorTipoEmpresa(-1);
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListarAgentes(string razonSocial, string documento, string tipoParticipante, string tipoModalidad,
            string fecIni, string fecFin, string estado, bool dosMasIncumplimientos)
        {
            AgenteModel model = new AgenteModel();
            model.listadoAgentes = servicio.ListarAgentes(razonSocial, documento, tipoParticipante, tipoModalidad,
             fecIni, fecFin, estado, dosMasIncumplimientos);
            return PartialView("ListadoAgentes", model);
        }

        public JsonResult ObtenerAgente(int empgcodi)
        {
            GmmEmpresaDTO oGmmEmpresaDTO = new GmmEmpresaDTO();
            oGmmEmpresaDTO = this.servicio.GetByIdEdit(empgcodi);
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oGmmEmpresaDTO));
        }

        [HttpPost]
        public ActionResult EliminarAgente(int empgcodi)
        {
            GmmEmpresaDTO entity = new GmmEmpresaDTO();

            entity.EMPGFECMODIFICACION = DateTime.Today;
            entity.EMPGCODI = empgcodi;
            entity.EMPGUSUMODIFICACION = User.Identity.Name;

            bool respuesta = this.servicio.Delete(entity);


            // Oficializar archivo
            base.ValidarSesionUsuario();


            return Json(new { success = respuesta, message = "Ok" });
        }

        [HttpPost]
        public PartialViewResult ListarMaestroEmpresas(string empresa)
        {
            AgenteModel model = new AgenteModel();
            model.ListMaestroEmpresa = this.servicio.ListarMaestroEmpresa(empresa, _estadoRegistroEmpresaActivo).OrderBy(p => p.Emprrazsocial).ToList();
            return PartialView("ListarEmpresas", model);
        }

        /// <summary>
        /// Obtiene la lista de modalidades, segun código de Empresa seleccionado
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ObtenerListaModalidades(int empgcodi)
        {
            AgenteModel model = new AgenteModel();
            model.listadoModalidades = this.servicio.ListarModalidades(empgcodi);

            return PartialView("ListaAgenteModalidades", model);
        }

        /// <summary>
        /// Obtiene la lista de estados, segun código de Empresa seleccionado
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ObtenerListaEstados(int empgcodi)
        {
            AgenteModel model = new AgenteModel();
            model.listadoEstados = this.servicio.ListarEstados(empgcodi);

            return PartialView("ListaAgenteEstados", model);
        }

        /// <summary>
        /// Obtiene la lista de incumplimientos, segun código de Empresa seleccionado
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ObtenerListaIncumplimientos(int empgcodi)
        {
            AgenteModel model = new AgenteModel();
            model.listadoIncumplimientos = this.servicio.ListarIncumplimientos(empgcodi);

            return PartialView("ListaAgenteIncumplimientos", model);
        }

        /// <summary>
        /// Guarda o actualiza un Agente
        /// </summary>
        /// <returns></returns>
        public ActionResult GuardarAgente(int pEmpgcodi, string pEmpgfecingreso, string pEmpgtipopart,
            string pEmpgestado, int pEmprcodi, string pEmpgComentario, bool pEsNuevo, string estadoEmpresaIni)
        {
            GmmEmpresaDTO oGmmEmpresaDTO = new GmmEmpresaDTO();

            if (pEmpgestado.Equals(estadoEmpresaIni)) oGmmEmpresaDTO.flagcambioEstado = false; else oGmmEmpresaDTO.flagcambioEstado = true;

            oGmmEmpresaDTO.EMPGFECINGRESO = DateTime.ParseExact(pEmpgfecingreso, "dd/MM/yyyy", null);
            oGmmEmpresaDTO.EMPGTIPOPART = pEmpgtipopart;
            oGmmEmpresaDTO.EMPGESTADO = pEmpgestado;
            oGmmEmpresaDTO.EMPRCODI = pEmprcodi;
            oGmmEmpresaDTO.EMPGCOMENTARIO = pEmpgComentario;
            oGmmEmpresaDTO.EMPGUSUCREACION = User.Identity.Name;
            oGmmEmpresaDTO.EMPGFECCREACION = DateTime.Now;
            oGmmEmpresaDTO.EMPGUSUMODIFICACION = User.Identity.Name;
            oGmmEmpresaDTO.EMPGFECMODIFICACION = DateTime.Now;
            oGmmEmpresaDTO.EMPGCODI = pEmpgcodi;
            GmmEmpresaDTO resultado = new GmmEmpresaDTO();
            if (pEsNuevo)
                resultado = this.servicio.Save(oGmmEmpresaDTO);
            else
                resultado = this.servicio.Update(oGmmEmpresaDTO);

            if (resultado.EMPGCODI > 0)
                return Json(new { success = true, message = "Ok", gnmEmpresa = resultado });
            else
                return Json(new { success = false, message = "Hubo un problema al registrar un agente, comuniquese con sistemas", gnmEmpresa = resultado });


        }

        /// <summary>
        /// Guarda o actualiza una Garantia
        /// </summary>
        /// <returns></returns>
        public ActionResult GuardarGarantia(int pEmpgcodi, string pFecini, string pFecfin,
            decimal pMontoGarantia, string pArchivo, string pTipoCertificado, string pTipoModalidad, int pGaraCodi, string Nuevo)
        {
            GmmGarantiaDTO oGmmGarantiaDTO = new GmmGarantiaDTO();

            //oGmmEmpresaDTO.EMPGFECINGRESO = DateTime.ParseExact(pEmpgfecingreso, "dd/MM/yyyy", null);
            oGmmGarantiaDTO.GARACODI = pGaraCodi;
            oGmmGarantiaDTO.GARAFECINICIO = DateTime.ParseExact(pFecini, "dd/MM/yyyy", null);
            oGmmGarantiaDTO.GARAFECFIN = DateTime.ParseExact(pFecfin, "dd/MM/yyyy", null);
            oGmmGarantiaDTO.GARAMONTOGARANTIA = pMontoGarantia;
            oGmmGarantiaDTO.GARAARCHIVO = pArchivo;
            oGmmGarantiaDTO.TCERCODI = pTipoCertificado;
            oGmmGarantiaDTO.TMODCODI = pTipoModalidad;
            oGmmGarantiaDTO.GARAUSUCREACION = User.Identity.Name;
            oGmmGarantiaDTO.GARAUSUMODIFICACION = User.Identity.Name;
            oGmmGarantiaDTO.GARAESTADO = "1";
            oGmmGarantiaDTO.EMPGCODI = pEmpgcodi;
            int resultado = 0;
            if (Nuevo == "1")
                resultado = this.servicioGarantia.Save(oGmmGarantiaDTO);
            else
                resultado = this.servicioGarantia.Update(oGmmGarantiaDTO);


            // Oficializar archivo
            base.ValidarSesionUsuario();
            string pathTemporal = @"IntranetTest\GMM\";

            FileServer.CopiarFileAlterFinal(AppDomain.CurrentDomain.BaseDirectory + @"Uploads\",
                pathTemporal, pArchivo, null);
            if (resultado > 0)
                return Json(new { success = true, message = "Ok" });
            else
                return Json(new { success = false, message = "Hubo un problema al guardar una modalidad, comuniquese con sistemas" });
        }

        /// <summary>
        /// Elimina una Garantia
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EliminarGarantia(int pGaraCodi)
        {
            GmmGarantiaDTO oGmmGarantiaDTO = new GmmGarantiaDTO();

            //oGmmEmpresaDTO.EMPGFECINGRESO = DateTime.ParseExact(pEmpgfecingreso, "dd/MM/yyyy", null);
            oGmmGarantiaDTO.GARACODI = pGaraCodi;
            oGmmGarantiaDTO.GARAUSUMODIFICACION = User.Identity.Name;

            bool respuesta = this.servicioGarantia.Delete(oGmmGarantiaDTO);


            // Oficializar archivo
            base.ValidarSesionUsuario();


            return Json(new { success = respuesta, message = "Ok" });
        }

        /// <summary>
        /// Obtiene una garantia para la edición
        /// </summary>
        /// <param name="garacodi">Codigo de garantia</param>
        /// <returns></returns>
        public JsonResult ObtenerGarantia(int garacodi)
        {
            GmmGarantiaDTO oEntidad = new GmmGarantiaDTO();
            oEntidad = this.servicio.ObtieneGarantiaById(garacodi);
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oEntidad));
        }
        public ActionResult Upload(string fecha)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

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
            catch (Exception ex)
            {
                Log.Fatal("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public virtual ActionResult descarga(string fileName)
        {
            base.ValidarSesionUsuario();

            try
            {
                //Manejo de carpetas
                //Constantes.RutaArchivosGMME;
                string pathTemporal = @"IntranetTest\GMM\" + fileName;

                byte[] buffer = FileServer.DownloadToArrayByte(pathTemporal, "");

                return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                Log.Fatal("descarga no ejecutada Agente GMM", ex);
                return null;
            }
        }

    }
}
