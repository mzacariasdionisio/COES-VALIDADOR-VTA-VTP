using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.GMM.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.GMM;
using COES.Framework.Base.Tools;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace COES.MVC.Intranet.Areas.GMM.Controllers
{
    public class IncumplimientosController : BaseController
    {
        IncumplimientoAppServicio servicio = new IncumplimientoAppServicio();
        DetIncumplimientoAppServicio servicioDetIncu = new DetIncumplimientoAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(AgentesController));

        private const string _estadoRegistroEmpresaActivo = "A";

        public IncumplimientosController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            //base.OnException(filterContext);

            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("IncumplimientosController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("IncumplimientosController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ListarIncumplimientos(string anho, string mes, string situacionEmpresa, string ruc,
            string razonSocial)
        {
            IncumplimientoModel model = new IncumplimientoModel();
            int _anho = 0;
            bool _try = int.TryParse(anho, out _anho);
            switch (situacionEmpresa)
            {
                case "1":
                    model.listadoIncumplimientos = this.servicio.ListarFiltroIncumplimientoDeudora(_anho, mes, razonSocial, ruc);
                    break;
                case "2":
                    model.listadoIncumplimientos = this.servicio.ListarFiltroIncumplimientoAfectada(_anho, mes, razonSocial, ruc);
                    break;
                default:
                    break;
            }
            return PartialView("ListadoIncumplimientos", model);
        }

        public JsonResult ObtenerIncumplimiento(int incucodi)
        {
            GmmIncumplimientoDTO oGmmIncumplimientoDTO = new GmmIncumplimientoDTO();
            oGmmIncumplimientoDTO = this.servicio.GetByIdEdit(incucodi);
            oGmmIncumplimientoDTO.TipoEmpresa = oGmmIncumplimientoDTO.TIPOEMPRCODI;
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oGmmIncumplimientoDTO));
        }
        [HttpPost]
        public PartialViewResult ListarEmpresas(string empresa, int tipoEmpresa)
        {
            IncumplimientoModel model = new IncumplimientoModel();
            model.ListEmpresaCliente = this.servicio.ListarMaestroEmpresaCliente(empresa, tipoEmpresa, _estadoRegistroEmpresaActivo).OrderBy(p => p.Emprrazsocial).ToList();
            return PartialView("ListarEmpresas", model);
        }
        [HttpPost]
        public PartialViewResult ListarEmpresasAgentes(string razonsocial)
        {
            IncumplimientoModel model = new IncumplimientoModel();
            model.ListEmpresaAgente = this.servicio.ListaEmpresasAgentes(razonsocial).OrderBy(p => p.Emprnombrecomercial).ToList();
            return PartialView("ListarEmpresasAgentes", model);
        }
        /// <summary>
        /// Obtiene la lista de archivos, segun código de Empresa seleccionado
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarArchivos(int incucodi)
        {
            IncumplimientoModel model = new IncumplimientoModel();
            model.listadoArchivos = this.servicioDetIncu.ListarArchivos(incucodi);

            return PartialView("ListaIncumplimientoArchivos", model);
        }

        [HttpPost]
        public ActionResult ListarTipoInforme()
        {
            List<GmmTipInformeDTO> lista = new List<GmmTipInformeDTO>();
            lista = this.servicioDetIncu.ListarTipoInforme();

            var jsonData = new
            {
                data = from f in lista.AsEnumerable()
                       select new
                       {
                           f.TINFCODI,
                           f.TINFINFORME
                       }
            };

            return Json(jsonData);

        }

        /// <summary>
        /// Guarda o actualiza una Incumplimiento
        /// </summary>
        /// <returns></returns>
        public ActionResult GuardarIncumplimiento(int codigoIncumplimiento, int empresaAgente, int tipoemprcodi,
            int empresa, int anho, string mes, string montoAfectado, bool esNuevo)
        {
            GmmIncumplimientoDTO oGmmIncumplimientoDTO = new GmmIncumplimientoDTO();

            oGmmIncumplimientoDTO.INCUCODI = codigoIncumplimiento;
            oGmmIncumplimientoDTO.INCUANIO = anho;
            oGmmIncumplimientoDTO.INCUMES = mes;
            oGmmIncumplimientoDTO.EMPGCODI = empresaAgente;
            oGmmIncumplimientoDTO.TIPOEMPRCODI = tipoemprcodi;
            oGmmIncumplimientoDTO.EMPRCODI = empresa;
            decimal _rtn = 0;
            decimal.TryParse(montoAfectado, out _rtn); ;
            oGmmIncumplimientoDTO.INCUMONTO = decimal.ToInt32(_rtn);
            int nuevo_id = 0;

            if (esNuevo)
            {
                oGmmIncumplimientoDTO.INCUUSUCREACION = User.Identity.Name;
                nuevo_id = this.servicio.Save(oGmmIncumplimientoDTO);
                oGmmIncumplimientoDTO.INCUCODI = nuevo_id;
                this.servicio.UpdateTrienio(oGmmIncumplimientoDTO);
            }
            else
            {
                oGmmIncumplimientoDTO.INCUCODI = codigoIncumplimiento;
                oGmmIncumplimientoDTO.INCUUSUMODIFICACION = User.Identity.Name;
                this.servicio.Update(oGmmIncumplimientoDTO);
                this.servicio.UpdateTrienio(oGmmIncumplimientoDTO);
            }
            //RcaCargaEsencialDTO oRcaCargaEsencialDTO = new RcaCargaEsencialDTO();

            //oRcaCargaEsencialDTO.Emprcodi = empresa;
            //oRcaCargaEsencialDTO.Equicodi = puntoMedicion;
            //oRcaCargaEsencialDTO.Rccarecarga = carga;
            //oRcaCargaEsencialDTO.Rccaredocumento = documento;
            //var rccarefecharecepcion = DateTime.ParseExact(fechaRecepcion, "dd/MM/yyyy", null);
            //oRcaCargaEsencialDTO.Rccarefecharecepcion = new DateTime(rccarefecharecepcion.Year, rccarefecharecepcion.Month, rccarefecharecepcion.Day
            //    , DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);

            //oRcaCargaEsencialDTO.Rccareestado = estado;
            //oRcaCargaEsencialDTO.Rccarenombarchivo = archivo;
            //oRcaCargaEsencialDTO.Rccareestregistro = _estadoRegistroNoEliminado;
            //oRcaCargaEsencialDTO.Rccareusucreacion = User.Identity.Name;
            //oRcaCargaEsencialDTO.Rccarefeccreacion = DateTime.Now;
            //oRcaCargaEsencialDTO.Rccareusumodificacion = User.Identity.Name;
            //oRcaCargaEsencialDTO.Rccarefecmodificacion = DateTime.Now;
            //oRcaCargaEsencialDTO.Rccareorigen = OrigenIntranet;

            //if (esNuevo)
            //{
            //    this.servicio.SaveRcaCargaEsencial(oRcaCargaEsencialDTO);
            //}
            //else
            //{
            //    oRcaCargaEsencialDTO.Rccarecodi = codigoCargaEsencial;
            //    this.servicio.UpdateRcaCargaEsencial(oRcaCargaEsencialDTO);
            //}

            return Json(new { success = true, message = "Ok" });
        }

        public ActionResult GuardarArchivo(int pIncucodigo, string pTipoinforme, string pFecharecepcion, string pArchivo)
        {
            GmmDetIncumplimientoDTO oGmmDetIncumplimientoDTO = new GmmDetIncumplimientoDTO();

            oGmmDetIncumplimientoDTO.INCUCODI = pIncucodigo;
            oGmmDetIncumplimientoDTO.TINFCODI = pTipoinforme;
            oGmmDetIncumplimientoDTO.DINCFECRECEPCION = DateTime.ParseExact(pFecharecepcion, "dd/MM/yyyy", null);
            oGmmDetIncumplimientoDTO.DINCARCHIVO = pArchivo;

            this.servicioDetIncu.Save(oGmmDetIncumplimientoDTO);

            string pathTemporal = @"IntranetTest\GMM\";

            FileServer.CopiarFileAlterFinal(AppDomain.CurrentDomain.BaseDirectory + @"Uploads\",
                pathTemporal, pArchivo, null);

            return Json(new { success = true, message = "Ok" });

        }

        public ActionResult EliminarArchivo(int pDetdinccodi)
        {

            this.servicioDetIncu.Delete(pDetdinccodi);

            return Json(new { success = true, message = "Ok" });

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
                Log.Fatal("descarga no ejecutada Incumplimientos GMM", ex);
                return null;
            }
        }
    }
}
