using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.CalculoResarcimiento.Model;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.CalculoResarcimiento.Controllers
{
    public class IngresoController : BaseController
    {
        /// <summary>
        /// Instancia del servicio de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>        
        CalculoResarcimientoAppServicio servicioResarcimiento = new CalculoResarcimientoAppServicio();

        #region Declaración de variables

        //readonly SeguridadServicioClient seguridad = new SeguridadServicioClient();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(DeclaracionController));
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

        /// <summary>
        /// Permite mostrar la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            IngresoModel model = new IngresoModel();

            model.Anio = DateTime.Now.Year;
            model.ListaPeriodo = this.servicioResarcimiento.ObtenerPeriodosSemestrales(model.Anio);
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
               Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            model.IndicadorEmpresa = (model.ListaEmpresas.Count > 1) ? Constantes.SI : Constantes.NO;
            if (model.ListaEmpresas.Count > 0)
            {
                model.Emprcodi = model.ListaEmpresas[0].Emprcodi;
                model.Emprnombre = model.ListaEmpresas[0].Emprnomb;
            }

            return View(model);
        }

        /// <summary>
        /// Permite mostrar los periodos por anio
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPeriodos(int anio)
        {
            return Json(this.servicioResarcimiento.ObtenerPeriodosSemestrales(anio));
        }

        /// <summary>
        /// Permite mostrar el dialogo de cambio de empresa
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresa()
        {
            IngresoModel model = new IngresoModel();
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
               Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Permite realizar la consulta de las declaraciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Consultar(int empresa, int periodo)
        {
            return Json(this.servicioResarcimiento.ObtenerPorEmpresaPeriodo(empresa, periodo));
        }


        /// <summary>
        /// Permite cargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public ActionResult UploadArchivoIngreso(int? id)
        {
            try
            {
                string extension = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string path = this.servicioResarcimiento.ObtenerPathArchivosResarcimiento();
                    string fileName = string.Empty;
                    string archivo = file.FileName;
                    int indexOf = archivo.LastIndexOf('.');
                    extension = archivo.Substring(indexOf + 1, archivo.Length - indexOf - 1);

                    if (id != null)
                    {
                        fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoIngreso, id.ToString(), extension);
                    }
                    else
                    {
                        fileName = string.Format(ConstantesCalculoResarcimiento.TemporalIngreso, extension);
                    }
                    /*
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);*/

                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileName,
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                    {
                        FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                    }

                    FileServer.UploadFromStream(file.InputStream, ConstantesCalculoResarcimiento.RutaResarcimientos, fileName,
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                    if (id != null)
                    {
                        this.servicioResarcimiento.GrabarArchivoIngreso((int)id, extension);
                    }
                }
                return Json(new { success = true, indicador = 1, extension = extension }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Permite grabar el ingreso por transmision
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="moneda"></param>
        /// <param name="ingreso"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarIngreso(int codigo, int empresa, string moneda, decimal ingreso, int periodo, string archivo)
        {
            string path = this.servicioResarcimiento.ObtenerPathArchivosResarcimiento();
            return Json(this.servicioResarcimiento.GrabarIngresoExtranet(path, codigo, empresa, moneda, ingreso, periodo, archivo, base.UserName));
        }

        /// <summary>
        /// Permite obtener los envios de interrpciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Envios(int idEmpresa, int idPeriodo)
        {
            return Json(this.servicioResarcimiento.ObtenerEnviosInterrupciones(idEmpresa, idPeriodo,
                ConstantesCalculoResarcimiento.EnvioIngresoTransmision));
        }

        /// <summary>
        /// Permite descargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoIngreso(int? id, string extension)
        {
            string fileName = string.Empty;
            if (id != null)
                fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoIngreso, id.ToString(), extension);            
            else
                fileName = string.Format(ConstantesCalculoResarcimiento.TemporalIngreso, extension);
            /*
            string fullPath = this.servicioResarcimiento.ObtenerPathArchivosResarcimiento() +
                fileName;
            return File(fullPath, Constantes.AppExcel, fileName);*/
            Stream stream = FileServer.DownloadToStream(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
              ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);


            return File(stream, extension, fileName);
        }

        /// <summary>
        /// Permite eliminar el archivo de una interrupcion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarArchivoIngreso(int? id, string extension)
        {
            string fileName = string.Empty;
            if (id != null)
            {
                fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoIngreso, id.ToString(), extension);
                this.servicioResarcimiento.GrabarArchivoIngreso((int)id, string.Empty);
            }
            else
                fileName = string.Format(ConstantesCalculoResarcimiento.TemporalIngreso, extension);
            /*
            string fullPath = this.servicioResarcimiento.ObtenerPathArchivosResarcimiento() +
                fileName;

            return Json(this.servicioResarcimiento.EliminarArchivoInterrupcion(fullPath));*/
            return Json(this.servicioResarcimiento.EliminarArchivoInterrupcion(fileName));
        }
    }
}