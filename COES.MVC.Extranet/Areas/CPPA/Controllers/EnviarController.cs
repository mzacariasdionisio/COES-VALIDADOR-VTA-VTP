using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Controllers;
using COES.Framework.Base.Tools;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Globalization;
using COES.MVC.Extranet.Areas.CPPA.Models;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.CPPA;
using System.Text.Json;
using System.IO;
using COES.Servicios.Aplicacion.CPPA.Helper;
using ExcelLibrary.BinaryFileFormat;
using DocumentFormat.OpenXml.Drawing.Charts;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
///
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Policy;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using COES.Servicios.Aplicacion.General.Helper;
using COES.MVC.Extranet.Helper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using WebGrease.Activities;

namespace COES.MVC.Extranet.Areas.CPPA.Controllers
{
    public class EnviarController : BaseController
    {
        private readonly CPPAAppServicio servicio = new CPPAAppServicio();
        private readonly EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EnviarController));

        private readonly HttpClient _httpClient;

        public EnviarController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _httpClient = new HttpClient();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("InformacionOperativaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("InformacionOperativaController", ex);
                throw;
            }
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="ajuste"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(int? anio, string ajuste, int? revision)
        {
            base.ValidarSesionUsuario();
            CargaArchivosIntegrantesModel model = new CargaArchivosIntegrantesModel();
            List<CpaRevisionDTO> ListRevision = new List<CpaRevisionDTO>();
            model.Resultado = 0;
            try
            {
                string token = RefreshToken();
                if (token == "-1") {
                    model.Resultado = -1;
                    return View(model);
                }
                // Crear el encabezado Authorization con el token JWT
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var url = $"{ConfigurationManager.AppSettings["ApiCPPA"] + ConstantesCPPA.urlApiCppa}ListarRevisionesPresupuestales";
                // Consulta al servicio web
                var response = await _httpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ApiResponse<ConsultarRevisionResponse>>(jsonResponse);

                // Validamos la respuesta
                if (resultado != null && resultado.response != null && resultado.response.Count > 0)
                {
                    ListRevision = resultado.response.Select(item => new CpaRevisionDTO
                    {
                        Cpaapanio = item.Cpaapanio,
                        Cpaapajuste = item.Cpaapajuste,
                        Cpaapanioejercicio = item.Cpaapanioejercicio,
                        Cparestado = item.Cparestado,
                        Cparcodi = item.Cparcodi,
                        Cparrevision = item.Cparrevision
                    }).ToList();
                }

                model.ListaAnio = servicio.AniosRevision(ListRevision);
                ViewBag.ListRevision = Newtonsoft.Json.JsonConvert.SerializeObject(ListRevision);

                if (anio.HasValue)
                {
                    ViewBag.Anio = anio;
                    ViewBag.Ajuste = ajuste;
                    ViewBag.Revision = revision;
                }

                if (TempData["Emprcodi"] != null && TempData["Emprnomb"] != null)
                {
                    ViewBag.Emprcodi = TempData["Emprcodi"];
                    ViewBag.Emprnomb = TempData["Emprnomb"];
                }
                else
                {

                    #region Autentificando Empresa
                    int iEmprCodi = 0;
                    List<SeguridadServicio.EmpresaDTO> listTotal = ListaTotalEmpresas();

                    ViewBag.ListaEmpresas = listTotal.Count;
                    if (listTotal.Count == 1)
                    {
                        ViewBag.Emprnomb = listTotal[0].EMPRNOMB;
                        ViewBag.Emprcodi = listTotal[0].EMPRCODI;
                        model.EntidadEmpresa = this.servicioEmpresa.GetByIdEmpresa(listTotal[0].EMPRCODI);
                    }
                    else if (ViewBag.Emprcodi != null)
                    {
                        iEmprCodi = Convert.ToInt32(ViewBag.Emprcodi.ToString());
                        model.EntidadEmpresa = this.servicioEmpresa.GetByIdEmpresa(iEmprCodi);
                        ViewBag.Emprnomb = model.EntidadEmpresa.EmprNombre;
                    }
                    else if (listTotal.Count > 1)
                    {
                        ViewBag.Emprnomb = "";
                        ViewBag.Emprnro = 2;
                        //model.Pericodi = pericodi;
                        return View("Index", model);
                    }
                    else
                    {
                        //No hay empresa asociada a la cuenta
                        ViewBag.Emprnomb = "";
                        ViewBag.Emprnro = -1;
                        //model.Pericodi = pericodi;
                        return View("Index", model);
                    }
                    #endregion

                }

                model.Resultado = 1;
            }
            catch (Exception e)
            {
                model.Resultado = -1;
            }

            return View(model);
        }


        /// <summary>
        /// Permite registrar los documentos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GrabarDocumentos()
        {
            CargaArchivosIntegrantesModel model = new CargaArchivosIntegrantesModel();

            try
            {
                string token = RefreshToken();
                // Crear el encabezado Authorization con el token JWT
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                model.Resultado = 0;
                string revision = Request.Form["revision"];
                string idRevision = Request.Form["cparcodi"];
                string ajuste = Request.Form["ajuste"];
                string emprcodi = Request.Form["emprcodi"];
                int anio = Convert.ToInt32(Request.Form["anio"]);

                var archivos = Request.Files;
                var formData = new MultipartFormDataContent();

                if (archivos.Count > 0)
                {
                    foreach (string archivoKey in archivos)
                    {
                        var archivo = archivos[archivoKey];
                        if (archivo != null && archivo.ContentLength > 0)
                        {
                            byte[] buffer = new byte[archivo.ContentLength];
                            using (var stream = archivo.InputStream)
                            {
                                await stream.ReadAsync(buffer, 0, (int)archivo.ContentLength);
                            }
                            var archivoContent = new ByteArrayContent(buffer);
                            archivoContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(archivo.ContentType);
                            formData.Add(archivoContent, "files", archivo.FileName);
                        }
                    }

                    string apiUrl = $"{ConfigurationManager.AppSettings["ApiCPPA"] + ConstantesCPPA.urlApiCppa}CargarDocumentos?revision={revision}&CodRevision={idRevision}&ajuste={ajuste}&CodEmpresa={emprcodi}&anio={anio}";
                    var response = await _httpClient.PostAsync(apiUrl, formData);

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    var responseString = await response.Content.ReadAsStringAsync();
                    //    var resultado = JsonConvert.DeserializeObject<ApiResponse<dynamic>>(responseString);
                    //    model.sMensaje = resultado.mensaje;
                    //    model.sStatus = resultado.resultado;
                    //    return Json(model);
                    //}
                    //else
                    //{
                    //    model.sMensaje = "Error al procesar los archivos...";
                    //    model.sStatus = -1;
                    //    return Json(model);
                    //}
                    if (response.IsSuccessStatusCode)
                    {
                        // Deserializar la respuesta estructurada
                        var apiResponse = await response.Content.ReadAsAsync<ApiResponse<ArchivoJsonResponse>>();

                        model.sMensaje = apiResponse.mensaje;
                        model.sStatus = apiResponse.resultado;

                        return Json(model);
                    }
                    else
                    {
                        // Intentar leer el mensaje de error estructurado
                        try
                        {
                            var errorResponse = await response.Content.ReadAsAsync<ApiResponse<ArchivoJsonResponse>>();
                            model.sMensaje = errorResponse.mensaje ?? "Error al procesar los archivos";
                            model.sStatus = -1;
                        }
                        catch
                        {
                            model.sMensaje = "Error al procesar los archivos";
                            model.sStatus = -1;
                        }
                        return Json(model);
                    }
                }

                model.sMensaje = "No se han cargado archivos. No se realizó el envío.";
                model.sStatus = -1;
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                model.sMensaje = $"Error inesperado: {ex.Message}";
                model.sStatus = -1;
                model.Resultado = -1;
            }

            return Json(model);
        }


        /// <summary>
        /// Lista los documentos segun parametros 
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public async Task<JsonResult> ListaDocumentosGrilla(int cparcodi, int emprcodi)
        {
            CargaArchivosIntegrantesModel model = new CargaArchivosIntegrantesModel();
            model.Resultado = 0;

            try
            {
                string token = RefreshToken();
                // Crear el encabezado Authorization con el token JWT
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var url = $"{ConfigurationManager.AppSettings["ApiCPPA"] + ConstantesCPPA.urlApiCppa}ListarDocumentos?CodRevision={cparcodi}&CodEmpresa={emprcodi}";
                // Consulta al servicio web
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ApiResponse<ConsultarEnvioResponse>>(jsonResponse);

                    // Validamos la respuesta
                    if (resultado != null && resultado.response != null && resultado.response.Count > 0)
                    {
                        model.ListaDocumentosGrilla = resultado.response.Select(item => new CpaDocumentosDTO
                        {
                            Cpadoccodi = item.Cpadoccodi,
                            Cpadocfeccreacion = item.Cpadocfeccreacion,
                            Cpadocusucreacion = item.Cpadocusucreacion,
                            Cpaapanio = item.Cpaapanio,
                            Cpaapajuste = item.Cpaapajuste,
                            Cparcodi = item.Cparcodi,
                            Cparrevision = item.Cparrevision,
                            Cpadoccodenvio = item.Cpadoccodenvio,
                            Emprcodi = item.Emprcodi,
                            Emprnomb = item.Emprnomb
                        }).ToList();

                        return Json(model);
                    }
                    else
                    {
                        model.ListaDocumentosGrilla = new List<CpaDocumentosDTO>();
                        model.sStatus = resultado.resultado;
                        model.sMensaje = resultado.mensaje;
                        return Json(model);
                    }
                }
                else
                {
                    return Json(new { mensaje = "Error al consultar los documentos", statusCode = response.StatusCode });
                }
            }
            catch (Exception)
            {
                model.Resultado = -1;
            }

            return Json(model);
        }


        /// <summary>
        /// Permite descargar el documento seleccionado. 
        /// </summary>
        /// <param name="rutaArchivo"></param>
        /// <returns></returns>
        public async Task<ActionResult> DescargarDocumento(string rutaArchivo)
        {
            object jsonData = null;

            try
            {
                string token = RefreshToken();
                // Crear el encabezado Authorization con el token JWT
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                string apiUrl = $"{ConfigurationManager.AppSettings["ApiCPPA"] + ConstantesCPPA.urlApiCppa}DescargarArchivos?rutaArchivo={Uri.EscapeDataString(rutaArchivo)}";

                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsAsync<ApiResponse<ArchivoJsonResponse>>();

                    if (apiResponse.resultado == 0)
                    {
                        var fileData = apiResponse.response.First();

                        var JsonResult = Json(new
                        {
                            success = true,
                            fileName = fileData.FileName,
                            fileBytes = fileData.FileBytes,
                            resultado = 1
                        }, JsonRequestBehavior.AllowGet);

                        JsonResult.MaxJsonLength = Int32.MaxValue;

                        return JsonResult;

                        //return Json(new
                        //{
                        //    success = true,
                        //    fileName = fileData.FileName,
                        //    fileBytes = fileData.FileBytes,
                        //    resultado = 1
                        //}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = apiResponse.mensaje ?? "No se pudo descargar el archivo.",
                            resultado = -1
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "No se pudo descargar el archivo...",
                        resultado = -1
                    });
                }
            }
            catch (Exception)
            {
                jsonData = new
                {
                    success = false,
                    resultado = -1
                };
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Interfaz donde se visualiza los documentos registrados por envio
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="anio"></param>
        /// <param name="ajuste"></param>
        /// <param name="revision"></param>
        /// <param name="revisionName"></param>
        /// <param name="envio"></param>
        /// <param name="emprcodi"></param>
        /// <param name="emprnomb"></param>
        /// <returns></returns>
        public ActionResult ListaDetalleGrilla(int documento, int anio, string ajuste, int revision, string revisionName, string envio, int emprcodi, string emprnomb)
        {
            ViewBag.Documento = documento;
            ViewBag.Anio = anio;
            ViewBag.Ajuste = ajuste;
            ViewBag.Revision = revision;
            ViewBag.RevisionName = revisionName;
            ViewBag.Envio = envio;
            ViewBag.Emprcodi = emprcodi;
            ViewBag.Emprnomb = emprnomb;

            TempData["Emprcodi"] = emprcodi;
            TempData["Emprnomb"] = emprnomb;

            return View();
        }


        /// <summary>
        /// Lista el detalle de un documento
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        public async Task<JsonResult> ListaDocumentosDetalleGrilla(int documento)
        {
            CargaArchivosIntegrantesModel model = new CargaArchivosIntegrantesModel();
            model.Resultado = 0;
            try
            {
                string token = RefreshToken();
                // Crear el encabezado Authorization con el token JWT
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var url = $"{ConfigurationManager.AppSettings["ApiCPPA"] + ConstantesCPPA.urlApiCppa}ListarDocumentoDetalle?CodDocumento={documento}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ApiResponse<ConsultarDetalleEnvioResponse>>(jsonResponse);

                    // Validamos la respuesta
                    if (resultado != null && resultado.response != null && resultado.response.Count > 0)
                    {
                        var listaDocumentosDetalleGrilla = resultado.response.Select(item => new CpaDocumentosDetalleDTO
                        {
                            Cpaddtcodi = item.Cpaddtcodi,
                            Cpadoccodi = item.Cpadoccodi,
                            Cpaddtruta = item.Cpaddtruta,
                            Cpaddtnombre = item.Cpaddtnombre,
                            Cpaddttamano = item.Cpaddttamano,
                            Cpaddtfeccreacion = item.Cpaddtfeccreacion,
                            Cpaddtusucreacion = item.Cpaddtusucreacion
                        }).ToList();

                        model.ListaDocumentosDetalleGrilla = listaDocumentosDetalleGrilla;
                        model.Resultado = 1;
                        return Json(model);
                    }
                    else
                    {
                        return Json(new { mensaje = "No se encontraron documentos." });
                    }
                }
                else
                {
                    return Json(new { mensaje = "Error al consultar los documentos", statusCode = response.StatusCode });
                }
            }
            catch (Exception ex)
            {
                model.Resultado = -1;
            }

            return Json(model);
        }


        [HttpPost]
        public ActionResult EscogerEmpresa()
        {
            base.ValidarSesionUsuario();
            List<SeguridadServicio.EmpresaDTO> listTotal = ListaTotalEmpresas().OrderBy(empresa => empresa.EMPRNOMB).ToList();
            CargaArchivosIntegrantesModel model = new CargaArchivosIntegrantesModel
            {
                ListaEmpresas = new List<COES.Dominio.DTO.Transferencias.EmpresaDTO>()
            };
            foreach (var item in listTotal)
            {
                model.ListaEmpresas.Add(new COES.Dominio.DTO.Transferencias.EmpresaDTO { EmprCodi = item.EMPRCODI, EmprNombre = item.EMPRNOMB });
            }

            return PartialView(model);
        }


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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmpresaElegida(int EmprCodi)
        {
            COES.Dominio.DTO.Transferencias.EmpresaDTO empresaDTO = this.servicioEmpresa.GetByIdEmpresa(EmprCodi);

            if (empresaDTO != null)
            {
                TempData["Emprcodi"] = empresaDTO.EmprCodi;
                TempData["Emprnomb"] = empresaDTO.EmprNombre;

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Metodo para descargar los archvos zipeados desde la interfaz principal
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="envio"></param>
        /// <param name="anio"></param>
        /// <param name="ajuste"></param>
        /// <param name="revision"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> DescargaDocumentosZipeados(string documento, string envio, string anio, string ajuste, string revision, string empresa)
        {
            object jsonData = null;

            try
            {
                string token = RefreshToken();
                // Crear el encabezado Authorization con el token JWT
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                string apiUrl = $"{ConfigurationManager.AppSettings["ApiCPPA"] + ConstantesCPPA.urlApiCppa}DescargarArchivosZipeados?CodDocumento={documento}&CodEnvio={envio}&anio={anio}&ajuste={ajuste}&CodRevision={revision}&CodEmpresa={empresa}";


                List<CpaDocumentosDetalleDTO> detalle = new List<CpaDocumentosDetalleDTO>();
                // Consulta al servicio web
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Deserializar la respuesta estructurada
                    var apiResponse = await response.Content.ReadAsAsync<ApiResponse<ArchivoJsonResponse>>();

                    if (apiResponse.resultado == 0)
                    {
                        var fileData = apiResponse.response.First();

                        var JsonResult = Json(new
                        {
                            success = true,
                            fileName = fileData.FileName,
                            fileBytes = fileData.FileBytes,
                            resultado = 1
                        }, JsonRequestBehavior.AllowGet);

                        JsonResult.MaxJsonLength = Int32.MaxValue;

                        return JsonResult;

                        //return Json(new
                        //{
                        //    success = true,
                        //    fileName = fileData.FileName,
                        //    fileBytes = fileData.FileBytes,
                        //    resultado = 1
                        //}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            mensaje = apiResponse.mensaje ?? "El servicio no devolvió datos",
                            resultado = -1
                        });
                    }
                }
                else
                {
                    try
                    {
                        var errorResponse = await response.Content.ReadAsAsync<ApiResponse<ArchivoJsonResponse>>();
                        return Json(new
                        {
                            success = false,
                            mensaje = errorResponse.mensaje ?? $"Error en el servicio: {response.StatusCode}",
                            resultado = -1
                        });
                    }
                    catch
                    {
                        return Json(new
                        {
                            success = false,
                            mensaje = $"Error al consultar los documentos: {response.StatusCode}",
                            resultado = -1
                        });
                    }
                }

            }
            catch (Exception)
            {
                jsonData = new
                {
                    success = false,
                    resultado = -1
                };
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public string RefreshToken()
        {
            string accessToken = Session[DatosSesion.SesionTokenApiSeguridad] as string;
            string refreshToken = Session[DatosSesion.SesionTokenRefreshApiSeguridad] as string;

            if (accessToken == "-1")
            {
                return accessToken;
            }
            else
            {
                var url = ConfigurationManager.AppSettings["ApiSeguridad"] + ConstantesCPPA.urlApiSeguridadRefresh;

                var requestData = new
                {
                    access_Token = accessToken,
                    refresh_Token = refreshToken
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(url, content).Result;

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta como cadena de forma sincrónica
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    var tokenResponse = JsonConvert.DeserializeObject<dynamic>(responseData);
                    string access_Token = tokenResponse?.access_Token;
                    string refresh_Token = tokenResponse?.refresh_Token;
                    Session[DatosSesion.SesionTokenApiSeguridad] = access_Token;
                    Session[DatosSesion.SesionTokenRefreshApiSeguridad] = refresh_Token;
                }
                else
                {
                    Session[DatosSesion.SesionTokenApiSeguridad] = "-1";//El -1 significa que no se autentico con el api seguridad
                }
            }
            accessToken = Session[DatosSesion.SesionTokenApiSeguridad] as string;
            return accessToken;
        }
    }
}