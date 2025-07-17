using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class CargaDatosCmgCPController : BaseController
    {
        private readonly HidrologiaAppServicio servicio = new HidrologiaAppServicio();

        #region Declaración de variables

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

        public CargaDatosCmgCPController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstHidrologia.SesionNombreArchivo] != null) ?
                    Session[ConstHidrologia.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstHidrologia.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public List<CmVolumenDetalleDTO> ListaDetalleImportar
        {
            get
            {
                return (Session[ConstHidrologia.SesionDatosImportacion] != null) ?
                    (List<CmVolumenDetalleDTO>)Session[ConstHidrologia.SesionDatosImportacion] : new List<CmVolumenDetalleDTO>();
            }
            set { Session[ConstHidrologia.SesionDatosImportacion] = value; }
        }

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CargaDatosCmgCPModel model = new CargaDatosCmgCPModel();
            model.Fecha = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaAnterior = DateTime.Today.AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);
            model.TienePermisoNuevo = true;

            return View(model);
        }

        /// <summary>
        /// Obtener datos generales del handson para armar la tabla web
        /// </summary>
        /// <param name="sFecha"></param>
        /// <param name="periodoH"></param>
        /// <param name="volcalcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerHandsonVolumenCaudalCmgCP(string sFecha, int periodoH, int volcalcodi)
        {
            CargaDatosCmgCPModel model = new CargaDatosCmgCPModel();

            try
            {
                DateTime fecha = DateTime.ParseExact(sFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                CmVolumenCalculoDTO objCab = null;
                List<CmVolumenDetalleDTO> listaDetalle;
                if (volcalcodi >= 0)
                {
                    servicio.ListarCmVolumenDetalleXEnvio(volcalcodi, fecha, periodoH, out listaDetalle, out objCab);
                }
                else
                {
                    listaDetalle = this.ListaDetalleImportar;
                }

                if (listaDetalle.Any())
                {
                    model.Handson = servicio.ArmarHandsonVolumenCaudalCmgCP(fecha, listaDetalle);
                    model.CabCalculo = objCab;
                    model.Resultado = "1";
                }
                else { model.Resultado = "0"; }
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
        /// Guarda una serie hidrologica desde la pestaña DETALLES
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="anioSerie"></param>
        /// <param name="mesSerie"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="codigoBase"></param>
        /// <param name="stringJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarHandsonVolumenCaudalCmgCP(string sFecha, int periodoH, int volcalcodi, string stringJson)
        {
            CargaDatosCmgCPModel model = new CargaDatosCmgCPModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha = DateTime.ParseExact(sFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                List<CmVolumenDetalleDTO> listaDetalle = servicio.ListarVolumenCaudalCmgCPFromHandson(stringJson);

                CmVolumenCalculoDTO objCab = new CmVolumenCalculoDTO()
                {
                    Volcalfecha = fecha,
                    Volcalperiodo = periodoH,
                    Volcaltipo = ConstHidrologia.CalculoVolumenReproceso,
                    Volcalusucreacion = base.UserName,
                    Volcalfeccreacion = DateTime.Now
                };

                //Guardar información importada                
                int envio = servicio.GuardarVolumenCaudalCmgCP(objCab, listaDetalle);
                model.CodigoEnvio = envio;

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

        [HttpPost]
        public JsonResult RecalcularVolumenCaudalCmgCP(string sFecha, int periodoH, int volcalcodi, string stringJson)
        {
            CargaDatosCmgCPModel model = new CargaDatosCmgCPModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha = DateTime.ParseExact(sFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                List<CmVolumenDetalleDTO> listaDetalle = servicio.ListarVolumenCaudalCmgCPFromHandson(stringJson);

                CmVolumenCalculoDTO objCab = new CmVolumenCalculoDTO()
                {
                    Volcalfecha = fecha,
                    Volcalperiodo = periodoH,
                    Volcaltipo = ConstHidrologia.CalculoVolumenReproceso,
                    Volcalusucreacion = base.UserName,
                    Volcalfeccreacion = DateTime.Now
                };

                //recalculo
                listaDetalle = servicio.RecalcularVolumenDetalle(listaDetalle, fecha, periodoH);

                model.Handson = servicio.ArmarHandsonVolumenCaudalCmgCP(fecha, listaDetalle);
                model.CabCalculo = null;
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
        /// Actualizar De BD
        /// </summary>
        /// <param name="sFecha"></param>
        /// <param name="periodoH"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarDeBD(string sFecha, int periodoH)
        {
            CargaDatosCmgCPModel model = new CargaDatosCmgCPModel();

            try
            {
                base.ValidarSesionJsonResult();

                //if (DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha) == sFecha) throw new ArgumentException("No está permitido el recalculo");

                DateTime fecha = DateTime.ParseExact(sFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                servicio.EjecutarProcesoCalculoCmVolumenDetalle(fecha, periodoH, base.UserName, ConstHidrologia.CalculoVolumenReproceso);

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
        /// Listado Historial
        /// </summary>
        /// <param name="sFecha"></param>
        /// <param name="periodoH"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoHistorial(string sFecha, int periodoH)
        {
            CargaDatosCmgCPModel model = new CargaDatosCmgCPModel();

            try
            {
                base.ValidarSesionJsonResult();
                DateTime fecha = DateTime.ParseExact(sFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaEnvio = servicio.GetByCriteriaCmVolumenCalculos(fecha, periodoH).OrderByDescending(x => x.Volcalcodi).ToList();

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
        /// 
        /// </summary>
        /// <param name="volcalcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarVersionCalculo(string sFecha, int periodoH, int volcalcodi)
        {
            CargaDatosCmgCPModel model = new CargaDatosCmgCPModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha = DateTime.ParseExact(sFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                servicio.GenerarArchivoExcelVolumenEmbalse(ruta, fecha, periodoH, volcalcodi, out string nombreArchivo);

                model.Resultado = nombreArchivo;

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
        /// Devuelve un archivo excel exportado
        /// </summary>
        /// <returns></returns>
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

        /// <summary>
        /// Importar excel 
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string fileName = ruta + fileRandom + ".xlsx";

                    this.NombreFile = fileName;
                    this.ListaDetalleImportar = new List<CmVolumenDetalleDTO>();
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Leer archivo y guardar informacion del excel importado
        /// </summary>
        /// <returns></returns>
        public JsonResult LeerArchivoExcelVolumenCaudalCmgCP()
        {
            CargaDatosCmgCPModel model = new CargaDatosCmgCPModel();
            try
            {
                base.ValidarSesionJsonResult();

                // Inicializa y lee excel importado
                List<CmVolumenDetalleDTO> listaDetalle = servicio.LeerArchivoExcelVolumenEmbalse(this.NombreFile);
                this.ListaDetalleImportar = listaDetalle;

                FileInfo fileInfo = new FileInfo(this.NombreFile);
                if (fileInfo.Exists)
                {
                    ToolsFormato.BorrarArchivo(this.NombreFile);
                }

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
    }
}
