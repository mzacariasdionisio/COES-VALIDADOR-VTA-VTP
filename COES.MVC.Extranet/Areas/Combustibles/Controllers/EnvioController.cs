using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.Combustibles.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.StockCombustibles;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Extranet.Areas.Combustibles.Controllers
{
    public class EnvioController : BaseController
    {
        readonly CombustibleAppServicio servicio = new CombustibleAppServicio();
        readonly SeguridadServicioClient seguridad = new SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #region Variables session

        public string Empresas
        {
            get
            {
                return (Session[ConstantesCombustibles.SesionEmpresas] != null) ?
                   Session[ConstantesCombustibles.SesionEmpresas].ToString() : ConstantesAppServicio.ParametroDefecto;
            }
            set { Session[ConstantesCombustibles.SesionEmpresas] = value; }
        }

        #endregion

        #endregion

        #region Principal

        //
        // GET: /Combustibles/Envio/
        public ActionResult Index(int? carpeta)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            CombustibleModel model = new CombustibleModel();
            model.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);
            model.IdEstado = carpeta.GetValueOrDefault(0) <= 0 ? ConstantesCombustibles.EstadoSolicitud : carpeta.Value;

            ListarEmpresaAgente(out List<SiEmpresaDTO> listaEmpresas, out List<EqEquipoDTO> listaCentral);
            model.ListaEmpresas = listaEmpresas;
            model.ListaCentral = listaCentral;

            this.Empresas = string.Join(",", (model.ListaEmpresas.Select(x => x.Emprcodi).Distinct().ToList()));

            //popup
            if (model.ListaEmpresas.Count > 0)
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;

            model.ListaCentral2 = listaCentral.Where(x => x.Emprcodi == model.IdEmpresa).ToList();
            if (model.ListaCentral2.Count > 0)
                model.IdEquipo = model.ListaCentral2[0].Equicodi;

            model.ListaCombustible = new List<SiFuenteenergiaDTO>();
            if (model.IdEquipo > 0)
                model.ListaCombustible = servicio.ListarFenergXCentral((int)ConstantesCombustibles.Interfaz.Extranet, model.IdEquipo);

            return View(model);
        }

        /// <summary>
        /// Permite cargar las centrales de la empresa seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFiltroCentralXEmpresa(string idEmpresa)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (this.IdOpcion == null) throw new ArgumentException(Constantes.MensajeModuloNoPermitido);

                ListarEmpresaAgente(out List<SiEmpresaDTO> listaEmpresas, out List<EqEquipoDTO> listaCentral);

                if (idEmpresa == null) idEmpresa = string.Empty;
                var empresas = idEmpresa.Trim().Split(',').Select(x => int.Parse(x)).ToList();

                model.ListaCentral = listaCentral.Where(x => empresas.Contains(x.Emprcodi.Value)).ToList();
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
        /// Permite generar la lista de combustible al seleccionar una central
        /// </summary>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFiltroCombustibleXCentral(int idCentral)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (this.IdOpcion == null) throw new ArgumentException(Constantes.MensajeModuloNoPermitido);

                ListarEmpresaAgente(out List<SiEmpresaDTO> listaEmpresas, out List<EqEquipoDTO> listaCentral);

                model.ListaCombustible = servicio.ListarFenergXCentral((int)ConstantesCombustibles.Interfaz.Extranet, idCentral);
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
        /// Listar empresas permitidas segun usuario
        /// </summary>
        /// <param name="opAccesoEmpresa"></param>
        /// <returns></returns>
        private void ListarEmpresaAgente(out List<SiEmpresaDTO> listaEmpresas, out List<EqEquipoDTO> listaCentral)
        {
            servicio.ListarEmpresasYCentralFormatoPr31((int)ConstantesCombustibles.Interfaz.Extranet, out List<SiEmpresaDTO> listaEmpAll, out List<EqEquipoDTO> listaCentralAll);

            bool permisoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);

            listaEmpresas = new List<SiEmpresaDTO>();
            if (permisoEmpresas)
            {
                listaEmpresas = listaEmpAll;
            }
            else
            {
                var lstEmpresa = this.seguridad.ObtenerEmpresasPorUsuario(base.UserName).ToList();
                foreach (var reg in listaEmpAll)
                {
                    var find = lstEmpresa.Find(x => x.EMPRCODI == (short)reg.Emprcodi);
                    if (find != null)
                    {
                        listaEmpresas.Add(reg);
                    }
                }
            }

            listaCentral = listaCentralAll.Where(x => listaEmpAll.Select(y => y.Emprcodi).ToList().Contains(x.Emprcodi.Value)).ToList();
        }

        /// <summary>
        /// Pagina de los envios
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="estados"></param>
        /// <param name="centrales"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <param name="tipocombustibles"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string empresas, int estado, string centrales, string finicios, string ffins)
        {
            empresas = string.IsNullOrEmpty(empresas) || empresas == ConstantesAppServicio.ParametroDefecto ? this.Empresas : empresas;
            DateTime fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = fechaFin.AddDays(1);

            CombustibleModel model = new CombustibleModel();
            model.IndicadorPagina = false;

            if (fechaInicio < fechaFin.AddYears(-1).AddDays(-1))
                throw new ArgumentException("El lapso de tiempo no puede ser mayor a un año.");

            int nroRegistros = servicio.GetTotalEnvio(empresas, centrales, estado, fechaInicio, fechaFin, ConstantesCombustibles.CombustiblesLiquidosYSolidos, "-1");

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Devuelve Vista Parcial de la lista de Envio de Combustibles
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="estados"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="centrales"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <param name="tipocombustibles"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Lista(string empresas, string centrales, int nroPaginas, string finicios, string ffins, int idEstado)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (this.IdOpcion == null) throw new ArgumentException(Constantes.MensajeModuloNoPermitido);

                DateTime fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddDays(1);

                if (fechaInicio < fechaFin.AddYears(-1).AddDays(-1))
                    throw new ArgumentException("El lapso de tiempo no puede ser mayor a un año.");

                empresas = string.IsNullOrEmpty(empresas) || empresas == ConstantesAppServicio.ParametroDefecto ? this.Empresas : empresas;

                model.AccionEditar = true;

                string url = Url.Content("~/");
                servicio.GenerarHtmlEnvio(url, empresas, centrales, idEstado, fechaInicio, fechaFin, nroPaginas, Constantes.PageSize
                                            , out string htmlCarpeta, out string htmlListado, (int)ConstantesCombustibles.Interfaz.Extranet);
                model.HtmlCarpeta = htmlCarpeta;
                model.HtmlListado = htmlListado;
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
        /// Genera el archivo excel en el servidor web del reporte solicitado
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="estados"></param>
        /// <param name="centrales"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="tipocombustibles"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporte(string empresas, string centrales, string finicios, string ffins, int idEstado)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddDays(1);

                if (fechaInicio < fechaFin.AddYears(-1).AddDays(-1))
                    throw new ArgumentException("El lapso de tiempo no puede ser mayor a un año.");

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                servicio.GenerarArchivoExcelEnvios(ruta, empresas, centrales, fechaInicio, fechaFin, idEstado, pathLogo);
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
        /// Descarga el archivo excel generador por GenerarArchivoReporte
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = ConstantesCombustibles.NombreReporteEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
            string fullPath = ruta + nombreArchivo;

            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        #region Cancelar Envío

        /// <summary>
        /// Agente cancela el envío.
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelarEnvio(int idEnvio, string motivo)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (this.IdOpcion == null) throw new ArgumentException(Constantes.MensajeModuloNoPermitido);

                if (string.IsNullOrEmpty(motivo))
                {
                    throw new ArgumentException("No ingresó motivo.");
                }

                servicio.CancelarEnvioExtranetPr31(idEnvio, motivo, base.UserEmail);
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

        #endregion

        #region Envío - Información Formulario

        /// <summary>
        /// Muestra el formulario para el envio de combustibles liquido
        /// </summary>
        /// <returns></returns>
        public ActionResult EnvioCombustible(int? idEnvio, int? idEmpresa, int? idEquipo, int? idGrupo, int? idFenergcodi)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            CombustibleModel model = new CombustibleModel();

            model.IdEnvio = idEnvio ?? 0;

            if (idEnvio > 0)
            {
                CbEnvioDTO envio = servicio.GetByIdCbEnvio(idEnvio.Value);
                model.IdEmpresa = envio.Emprcodi;
                model.IdEquipo = envio.Equicodi;
                model.IdGrupo = envio.Grupocodi;
                model.IdFenerg = envio.Fenergcodi;
                model.Emprnomb = envio.Emprnomb;
                model.Equinomb = envio.Equinomb;
                model.Fenergnomb = envio.Fenergnomb;

                model.IdEstado = envio.Estenvcodi;
                model.AccionEditar = envio.EsEditableExtranet;
            }
            else
            {
                if (idEmpresa.GetValueOrDefault(0) <= 0 || idEquipo.GetValueOrDefault(0) <= 0 || idFenergcodi.GetValueOrDefault(0) <= 0) return base.RedirectToHomeDefault();

                model.IdEmpresa = idEmpresa.Value;
                model.IdEquipo = idEquipo.Value;
                model.IdGrupo = idGrupo.Value;
                model.IdFenerg = idFenergcodi.Value;
                model.Emprnomb = servicio.GetByIdSiEmpresa(model.IdEmpresa).Emprnomb;
                model.Equinomb = servicio.GetByIdEqEquipo(model.IdEquipo).Equinomb;
                model.Fenergnomb = servicio.GetByIdSiFuenteenergia(model.IdFenerg).Fenergnomb;

                model.IdEstado = ConstantesCombustibles.EstadoSolicitud;
                model.AccionEditar = true;
            }

            model.IdTipoCombustible = CombustibleAppServicio.GetEstcomcodiByFenergcodi(model.IdFenerg);

            #region Crear carpetas

            // Para obtener un identificador - datos del usuario           
            // Obtener un identificador unico
            string nombreCarpetaTemporal = ConstantesCombustibles.FolderPR31 + "//" + ConstantesCombustibles.CarpetaTemporal + "_" + base.UserName;

            // Para obtener path base de acuerdo al modulo
            string path = "//";
            string pathTmp = path + nombreCarpetaTemporal;

            FileServer fs = new FileServer();

            // borramos la carpeta (quitamos los archivos temporales)
            List<int> lcnpLiq = CombustibleAppServicio.ListarCarpetaXTipocombustible(model.IdTipoCombustible);
            foreach (var cnp in lcnpLiq)
                fs.DeleteFolder(pathTmp + "//" + cnp);

            // creamos la nueva carpeta vacia
            foreach (var cnp in lcnpLiq)
                FileServer.CreateFolder(pathTmp, cnp.ToString(), null);

            #endregion

            return View(model);
        }

        /// <summary>
        /// Envia model para visualizacion de grilla excel de combustible
        /// </summary>
        /// <param name="tipoCombustible"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="idCombustible"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEnvio, int idEmpresa, int equicodi, int grupocodi, int idFenergcodi)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.IdEnvio = idEnvio;
                model.IdEmpresa = idEmpresa;
                model.IdEquipo = equicodi;
                model.IdFenerg = idFenergcodi;
                model.IdTipoCombustible = CombustibleAppServicio.GetEstcomcodiByFenergcodi(model.IdFenerg);

                model.ModeloWeb = servicio.GetHandsonCombustible((int)ConstantesCombustibles.Interfaz.Extranet, idEnvio, model.IdTipoCombustible, idEmpresa, grupocodi, equicodi, idFenergcodi);
                model.AccionEditar = model.ModeloWeb.Editable;

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
        public JsonResult ActualizarGrilla(string dataJson)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                HandsonCombustible modeloWeb = serializer.Deserialize<HandsonCombustible>(dataJson);

                model.ModeloWeb = servicio.ActualizarHandsonFormateoYResultado(modeloWeb, false);
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
        /// Obtenemos el tipo de cambio por fecha 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ObtenerTipoCambio(string fecha)
        {
            var model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                DateTime objFecha = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                decimal tipocambio = servicio.GetTipoCambio(objFecha);
                if (tipocambio > 0)
                {
                    model.TipoCambio = tipocambio;
                    model.Resultado = "1";
                }
                else
                {
                    throw new ArgumentException("No existe Tipo de cambio para la fecha seleccionada");
                }
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

        ///// <summary>
        ///// Obtenemos el tipo de cambio por fecha 
        ///// </summary>
        ///// <param name="fecha"></param>
        ///// <returns></returns>
        //public JsonResult ObtenerCombustibleAlmacenado(string fecha, int grupocodi, int fenergcodi)
        //{
        //    var model = new CombustibleModel();

        //    try
        //    {
        //        base.ValidarSesionJsonResult();
        //        DateTime objFecha = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

        //        model.CombustibleAlmacenado = servicio.GetStockCombutibleInicial(objFecha, grupocodi, fenergcodi);
        //        model.Resultado = "1";
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(NameController, ex);
        //        model.Resultado = "-1";
        //        model.Mensaje = ex.Message;
        //        model.Detalle = ex.StackTrace;
        //    }

        //    return Json(model);
        //}

        /// <summary>
        /// Obtenemos el tipo de cambio por fecha 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ObtenerVolumenRecepcion(string fecha, int equicodi, int grupocodi, int fenergcodi, int cbenvcodi)
        {
            var model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                DateTime objFecha = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.CombustibleAlmacenado = servicio.GetStockCombustibleXDia(objFecha, grupocodi, fenergcodi, ConstantesStockCombustibles.StrTptoRecepcion);
                model.MensajeFechaRecepcion = servicio.ValidacionFechaRecepcionExistenteBD(cbenvcodi, objFecha, equicodi, fenergcodi);
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
        /// Graba los datos enviados del formato consumo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarDatosCombustible(string dataJson, int idEnvio, int idEmpresa, int equicodi, int grupocodi, int idFenergcodi)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                HandsonCombustible modeloWeb = serializer.Deserialize<HandsonCombustible>(dataJson);

                string nombreCarpetaTemporal = ConstantesCombustibles.FolderPR31 + "//" + ConstantesCombustibles.CarpetaTemporal + "_" + base.UserName;
                string path = "//";
                string pathTmp = path + nombreCarpetaTemporal;

                int cbenvcodi = servicio.RealizarSolicitudCostoCombustible(idEnvio, idEmpresa, equicodi, grupocodi, idFenergcodi, base.UserEmail, "A", "P"
                                                                , modeloWeb, path, pathTmp);

                model.Resultado = "1";
                if (cbenvcodi == 0)//no existe cambio
                    model.Resultado = "2";
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

        #endregion

        #region Envío - Documentos

        /// <summary>
        /// Graba los archivos directamente en la carpeta del id seleccionado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadTemporal(int concepcodi)
        {
            try
            {
                base.ValidarSesionUsuario();

                // Obtener path temporal  
                string nombreCarpetaTemporal = ConstantesCombustibles.FolderPR31 + "//" + ConstantesCombustibles.CarpetaTemporal + "_" + base.UserName + "//" + concepcodi;

                // Para obtener path base de acuerdo al modulo
                string path = "//" + nombreCarpetaTemporal;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string sNombreArchivo = file.FileName;

                    string myFilePath = "C:\\" + sNombreArchivo;
                    string ext = Path.GetExtension(myFilePath);
                    string sArchivoDestino = "archivo_pr31_" + DateTime.Now.Ticks + ext;

                    if (FileServer.VerificarExistenciaFile(null, path + "//" + sArchivoDestino, null))
                    {
                        FileServer.DeleteBlob(path + "\\" + sNombreArchivo, null);
                    }
                    FileServer.UploadFromStream(file.InputStream, path + "//", sArchivoDestino, null);

                    return Json(new { success = true, nuevonombre = sArchivoDestino }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Permite descargar el archivo al explorador
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoTemporal(string fileName, int concepcodi)
        {
            base.ValidarSesionUsuario();

            // Obtener path temporal  
            string nombreCarpetaTemporal = ConstantesCombustibles.FolderPR31 + "//" + ConstantesCombustibles.CarpetaTemporal + "_" + base.UserName + "//" + concepcodi;

            // Para obtener path base de acuerdo al modulo
            string path = "//" + nombreCarpetaTemporal;

            //Manejo de carpetas
            string pathTemporal = path + "//" + fileName;

            byte[] buffer = FileServer.DownloadToArrayByte(pathTemporal, "");
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        /// <summary>
        /// Permite descargar el archivo al explorador
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoEnvio(string fileName, int concepcodi, int idEnvio)
        {
            base.ValidarSesionUsuario();

            var envio = servicio.GetByIdCbEnvio(idEnvio);

            // Para obtener path base de acuerdo al modulo
            string path = CombustibleAppServicio.GetPathEmpresaEnvio("//", envio.Emprcodi, idEnvio) + concepcodi;

            //Manejo de carpetas
            string pathTemporal = path + "//" + fileName;

            byte[] buffer = FileServer.DownloadToArrayByte(pathTemporal, "");
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion
    }
}
