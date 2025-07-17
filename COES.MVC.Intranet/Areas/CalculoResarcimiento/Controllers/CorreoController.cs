using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
//using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class CorreoController : BaseController
    {        

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        CalculoResarcimientoAppServicio servicioResarcimiento = new CalculoResarcimientoAppServicio();
        CalidadProductoAppServicio servicioCalidad = new CalidadProductoAppServicio();
        CorreoAppServicio servCorreo = new CorreoAppServicio();

        readonly SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(PuntoEntregaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public String NombreArchivosIPR
        {
            get
            {
                return (Session["NombreArchivosIPR"] != null) ?
                    Session["NombreArchivosIPR"].ToString() : null;
            }
            set { Session["NombreArchivosIPR"] = value; }
        }

        #endregion

        #region Listado Correos


        /// <summary>
        /// Pagina principal de envio de mensaje a agentes
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCorreo()
        {
            CorreoModel model = new CorreoModel();

            model.Anio = DateTime.Now.Year;
            model.ListaPeriodo = this.servicioResarcimiento.ObtenerPeriodosPorAnio(model.Anio);
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }


        /// <summary>
        /// Lista los correos existentes
        /// </summary>
        /// <param name="strTipoCorreo"></param>
        /// <param name="anioDesde"></param>
        /// <param name="anioHasta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCorreos(string strTipoCorreo, int anioDesde, int anioHasta)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                DateTime fechaInicio = new DateTime(anioDesde, 1, 1);
                DateTime fechaFin = new DateTime(anioHasta, 12, 31);

                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.ListadoCorreosEnviados = servicioCalidad.ListarCorreosEnviados(strTipoCorreo, fechaInicio, fechaFin);
                //model.ListadoCorreosEnviados = new List<ReLogcorreoDTO>();
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
        /// Permite obtener los periodos semestrales por anio
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPeriodos(int anio)
        {
            return Json(this.servicioResarcimiento.ObtenerPeriodosPorAnio(anio));
        }


        /// <summary>
        /// Muestra los datos Generales del correo
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="corrcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarDatosGeneralesCorreo(int accion, int relcorcodi)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                if (accion == ConstantesCalidadProducto.VerCorreo)//Ver detalles de un correo
                {
                    ReLogcorreoDTO logCorreo = servicioCalidad.GetByIdReLogcorreo(relcorcodi);
                    RePeriodoDTO periodo = servicioResarcimiento.GetByIdRePeriodo(logCorreo.Repercodi.Value);
                    List<ReEmpresaDTO> listaEmpresas = servicioCalidad.ObtenerEmpresasTotales();

                    model.Anio = periodo.Reperanio.Value;
                    model.Repercodi = periodo.Repercodi;
                    model.IdPlantilla = Int32.Parse(servicioCalidad.ObtenerIdPlantillaXIdTipo(logCorreo.Retcorcodi.Value));
                    model.NombreEmpresa = logCorreo.Relcorempresa != 0 ? listaEmpresas.Find(x => x.Emprcodi == logCorreo.Relcorempresa).Emprnomb.Trim() : "";
                }
                else
                {
                    model.Anio = DateTime.Now.Year;
                    
                }

                EstructuraInterrupcion maestros = new EstructuraInterrupcion();
                maestros.ListaEmpresa = this.servicioResarcimiento.ObtenerEmpresas();

                model.ListaEmpresas = maestros.ListaEmpresa;
                model.ListaPeriodo = this.servicioResarcimiento.ObtenerPeriodosPorAnio(model.Anio);                
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
        /// Verifica si el periodo es semestral y carga la lista de responsables o suministradores
        /// </summary>
        /// <param name="repercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerificarPeriodo(int repercodi)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                
                model.EsSemestral = servicioCalidad.EsSemestral(repercodi);               
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
        /// Deveuelve el listado de empresas a mostrar
        /// </summary>
        /// <param name="repercodi"></param>
        /// <param name="tipoCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LlenarEmpresas(int repercodi, int tipoCorreo)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                model.ListaEmpresas = servicioCalidad.ObtenerEmpresasMostrar(repercodi, tipoCorreo, out bool muestraEmp, out string campo);
                model.MuestraEmpresa = muestraEmp;
                model.CampoEmpresa = campo;
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
        /// Devuelve los datos del correo a mostrar
        /// </summary>
        /// <param name="idCorreo"></param>
        /// <param name="anio"></param>
        /// <param name="repercodi"></param>
        /// <param name="tipoCorreo"></param>
        /// <param name="strEmpresaId"></param>
        /// <returns></returns>
        [HttpPost]        
        public JsonResult DetallarCorreo(int idCorreo, int anio, int repercodi, int tipoCorreo, string strEmpresaId)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                
                base.ValidarSesionJsonResult();
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                int corrcodi = idCorreo;
                SiCorreoDTO correo;
                if (corrcodi != 0)//Ver detalles de un correo
                {                    
                    correo = servicioCalidad.ObtenerInfoCorreo(idCorreo);
                }
                else // nuevo correo
                {
                    int? valNulo = null;
                    int? empresaId = strEmpresaId != null ? (strEmpresaId != "" ? Int32.Parse(strEmpresaId) : valNulo) : valNulo;

                    correo = servicioCalidad.ObtenerMuestraCorreo(anio, repercodi, tipoCorreo, empresaId);
                     
                }

                model.Correo = correo;
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
        /// Envia los correos al grupo de empresas
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="repercodi"></param>
        /// <param name="tipoCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarGrupoCorreo(SiCorreoDTO correo, int repercodi, int tipoCorreo, int idEmpresa, int idArchivoOriginal)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                base.ValidarSesionJsonResult();
               
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                //Direccion temporal donde se suben los archivos
                string pathArchivos = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnviarCorreos.RutaCorreo;

                //Actualizo la plantilla
                servicioCalidad.EnviarMensajeAGrupoEmpresas(correo, repercodi, tipoCorreo, idEmpresa, base.UserName, pathLogo, pathArchivos, idArchivoOriginal);
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
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="corrcodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoEnvio(string fileName, int relcorcodi)
        {
            base.ValidarSesionUsuario();

            byte[] buffer = servicioCalidad.GetBufferArchivoAdjuntoCorreo(relcorcodi, fileName);
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion

        #region Correos de Empresas
        /// <summary>
        /// Pantalla Inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult CorreosEmpresa()
        {            
            CorreoModel model = new CorreoModel();
            model.ListaEmpresas = servicioCalidad.ObtenerEmpresasTotales();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return View(model);
        }

        /// <summary>
        /// Lista los registros de empresas y sus correos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEmpresaCorreos()
        {
            CorreoModel model = new CorreoModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.ListaEmpresasCorreo = servicioCalidad.ListarEmpresasYSusCorreos();
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
        /// Devuelve el listado de correos de una empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerCorreosPorEmpresa(int emprcodi)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                base.ValidarSesionJsonResult();
              
                string correosXEmpresa = "";
                if (emprcodi != -1)
                {
                    correosXEmpresa = servicioCalidad.ListarCorreosPorEmpresa(emprcodi);
                }

                model.ListaCorreosPorEmpresa = correosXEmpresa;
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
        /// Guardar correos de una empresa
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="empresaId"></param>
        /// <param name="correos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarEmpresaCorreos(int accion, int empresaId, string correos)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                base.ValidarSesionJsonResult();                
                string validaciones = servicioCalidad.ValidarEmpresaCorreos(accion, empresaId, correos);
                if (validaciones != "")
                {
                    throw new Exception(validaciones);
                }

                servicioCalidad.GuardarEmpresaCorreos(accion, empresaId, correos, false, base.UserName);
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
        /// Genera el archivo a exportar el listado de puntos de entrega
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarListado()
        {
            CorreoModel model = new CorreoModel();

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "ListaCorreosPorEmpresa.xlsx";

                servicioCalidad.GenerarExportacionEC(ruta, pathLogo, nameFile);
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
        /// Exporta archivo pdf, excel, csv, ...
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
        /// Exporta archivo desde carpeta temporales
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult ExportarDesdeTemporales()
        {
            string nombreArchivo = Request["file_name"];

            //Crear carpeta para generar los excel
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            var patTrabajo = ConstantesCalidadProducto.RutaCarpetaTempResarcimiento;
            string nombreCarpeta = ConstantesCalidadProducto.NombreCarpetaTempResarcimiento;
            //FileServer.DeleteFolderAlter(patTrabajo, ruta);
            //FileServer.CreateFolder(patTrabajo, nombreCarpeta, ruta);

            var rutaFinalArchivos = ruta + patTrabajo + nombreCarpeta + "\\";

           
            //string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, rutaFinalArchivos);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        /// <summary>
        /// Guarda los correos de una empresa desde archivo excel
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarCorreosDesdeArchivo(FormCollection formCollection)
        {
            CorreoModel model = new CorreoModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fileName = formCollection["name"];
                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;

                if (stremExcel != null)
                {
                    List<EmpresaCorreo> listaDataExcel = new List<EmpresaCorreo>();
                    List<EmpresaCorreoErrorExcel> listaDataExcelErrores = new List<EmpresaCorreoErrorExcel>();                    

                    try
                    {    
                        this.servicioCalidad.ObtenerDataExcel(stremExcel, User.Identity.Name, out listaDataExcel, out listaDataExcelErrores);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(NameController, ex);
                        if (ex.Message.Contains("mal posicionada"))
                            throw new Exception("El contenido del archivo Excel no es correcto. " + ex.Message + ".");
                        else
                            throw new Exception("" + ex.Message + ".");
                        //throw new Exception("Contenido del archivo incorrecto. El archivo solo debe contener una tabla con 3 columnas en el siguiente orden: Fecha y Hora, valor Alpha, valor Beta");
                    }

                    //Guardar en BD si el archivo está correcto
                    if (listaDataExcel.Any() && listaDataExcelErrores.Count == 0)
                    {
                        servicioCalidad.GuardarGrupoCorreos(listaDataExcel, User.Identity.Name);
                        model.Resultado = "1";
                    }

                    if (listaDataExcelErrores.Any())
                        model.Resultado = this.servicioCalidad.ObtenerTablaErroresHtml(listaDataExcelErrores);
                }
            }
            catch (Exception ex)
            {
                model.Mensaje = "Error: " + ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Elimina una empresa y sus correos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEmpresaCorreos(int emprcodi)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                base.ValidarSesionJsonResult();
               
                servicioCalidad.EliminarEmpresaCorreos(emprcodi, base.UserName);
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
        /// Genera y descarga el reporte de interrupciones pendientes
        /// </summary>
        /// <param name="idSuministrador"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarYDescargarReporteIntPendientesReportar(int idSuministrador, int idPeriodo)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                var patTrabajo = ConstantesCalidadProducto.RutaCarpetaTempResarcimiento;
                string nombreCarpeta = ConstantesCalidadProducto.NombreCarpetaTempResarcimiento;                

                var rutaFinalArchivos = ruta + patTrabajo + nombreCarpeta + "\\";

                //string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "Interrupciones_Pendientes_Reportar.xlsx";

                servicioCalidad.GenerarReporteIntrrupcionesSinReportar(rutaFinalArchivos, pathLogo, nameFile, idSuministrador, idPeriodo);
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
        /// Permite realizar la validacion de la estructura del archivo adjuntar de interrupciones sin reportar
        /// </summary>
        /// <param name="clasificacion"></param> 
        /// <returns></returns>
        [HttpPost]
        public JsonResult validarReporteIPR()
        {
            try
            {                
                List<string> validaciones = new List<string>();

                var patTrabajo = ConstantesCalidadProducto.RutaCarpetaTempResarcimiento;
                string nombreCarpeta = ConstantesCalidadProducto.NombreCarpetaTempResarcimiento;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                //Generar los .dat
                string rutaCarpetaContenedor = ruta + patTrabajo;
                var rutaFinalArchivo = rutaCarpetaContenedor + nombreCarpeta + "\\";

                servicioCalidad.ValidarReporteIPRExcel(rutaFinalArchivo, NombreArchivosIPR, out validaciones);

                if (validaciones.Count == 0)
                    return Json(new { Result = 1,  Errores = new List<string>() });
                else
                    return Json(new { Result = 2,  Errores = validaciones });
            }
            catch
            {
                return Json(new { Result = -1, Errores = new List<string>() });
            }
        }

        #endregion

        // <summary>
        /// Carga archivos adjuntados
        /// </summary>
        /// <param name="chunks"></param>
        /// <param name="chunk"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                base.ValidarSesionUsuario();

                if (Request.Files.Count == 1)
                {
                    var patTrabajo = ConstantesCalidadProducto.RutaCarpetaTempResarcimiento;
                    string nombreCarpeta = ConstantesCalidadProducto.NombreCarpetaTempResarcimiento;
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    
                    string rutaCarpetaContenedor = ruta + patTrabajo;
                    var rutaFinalArchivo = rutaCarpetaContenedor + nombreCarpeta + "\\";

                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnviarCorreos.RutaCorreo;
                    var file = Request.Files[0];

                    this.NombreArchivosIPR = file.FileName;

                    string fileName = rutaFinalArchivo + file.FileName;

                    if (!Directory.Exists(rutaFinalArchivo))
                    {
                        FileServer.CreateFolder("", nombreCarpeta, rutaCarpetaContenedor);
                    }
                    

                    if (FileServer.VerificarExistenciaFile(null, file.FileName, path))
                    {
                        FileServer.DeleteBlob(file.FileName, path);
                    }

                    file.SaveAs(fileName);

                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
