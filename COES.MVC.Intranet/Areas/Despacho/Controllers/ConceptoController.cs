using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Despacho.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Despacho.Helper;
using log4net;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Controllers;
using System.Reflection;
using System.Web.Script.Serialization;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;

namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class ConceptoController : BaseController
    {
        private DespachoAppServicio appDespacho = new DespachoAppServicio();

        #region Declaración de variables de Sesión

        private List<DatoComboBox> listadoFichaTecnica = new List<DatoComboBox>();
        private List<DatoComboBox> listadoSINO = new List<DatoComboBox>();
        private List<DatoComboBox> listadoTipo = new List<DatoComboBox>();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public ConceptoController()
        {
            listadoFichaTecnica.Add(new DatoComboBox { Descripcion = "-TODOS-", Valor = "T" });
            listadoFichaTecnica.Add(new DatoComboBox { Descripcion = "Si", Valor = "S" });
            listadoFichaTecnica.Add(new DatoComboBox { Descripcion = "No", Valor = "N" });

            listadoSINO.Add(new DatoComboBox { Descripcion = "SI", Valor = "S" });
            listadoSINO.Add(new DatoComboBox { Descripcion = "NO", Valor = "N" });

            listadoTipo.Add(new DatoComboBox { Descripcion = "DECIMAL", Valor = "DECIMAL" });
            listadoTipo.Add(new DatoComboBox { Descripcion = "ENTERO", Valor = "ENTERO" });
            listadoTipo.Add(new DatoComboBox { Descripcion = "CARACTER", Valor = "CARACTER" });
            listadoTipo.Add(new DatoComboBox { Descripcion = "FECHA", Valor = "FECHA" });
            listadoTipo.Add(new DatoComboBox { Descripcion = "AÑO", Valor = "AÑO" });
            listadoTipo.Add(new DatoComboBox { Descripcion = "ARCHIVO", Valor = "ARCHIVO" });
            listadoTipo.Add(new DatoComboBox { Descripcion = "FORMULA", Valor = "FORMULA" });
            log4net.Config.XmlConfigurator.Configure();
        }
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

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            ConceptoModel model = new ConceptoModel();

            model.ListaFichatecnica = listadoFichaTecnica;
            model.ListaCategoria = appDespacho.ListarCategoriaGrupo().OrderBy(x => x.Catenomb).ToList();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);// verificar rol

            //eliminar los archivos temporales en reporte
            appDespacho.EliminarArchivosReporte();

            return View(model);
        }

        /// <summary>
        /// ListadoConceptos
        /// </summary>
        /// <param name="catecodi"></param>
        /// <param name="fichaTecnica"></param>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoConceptos(int catecodi, string fichaTecnica, string nombre, int estado)
        {
            ConceptoModel model = new ConceptoModel();

            model.ListaConcepto = appDespacho.ListarConceptoxFiltro(catecodi, fichaTecnica, nombre, estado);

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// ObtenerConcepto
        /// </summary>
        /// <param name="concepcodi"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerConcepto(int concepcodi, bool editable)
        {
            ConceptoModel model = new ConceptoModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaFichatecnica = listadoSINO;
                model.ListaCategoria = appDespacho.ListarCategoriaGrupo().OrderBy(x => x.Catenomb).ToList();
                model.ListaTipoDato = listadoTipo;

                model.Entidad = new PrConceptoDTO();

                if (concepcodi == 0)
                {
                    model.AccionNuevo = true;

                    model.Entidad.Concepfeccreacion = DateTime.Now;
                }
                else
                {
                    model.Entidad = appDespacho.GetByIdPrConcepto(concepcodi);
                    model.Entidad.StrConcepliminf = model.Entidad.Concepliminf != null ? model.Entidad.Concepliminf.ToString() : "";
                    model.Entidad.StrConceplimsup = model.Entidad.Conceplimsup != null ? model.Entidad.Conceplimsup.ToString() : "";
                    model.AccionEditar = editable ? true : model.AccionEditar;

                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// ConceptoGuardar
        /// </summary>
        /// <param name="dataJson"></param>
        /// <param name="opcion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConceptoGuardar(string dataJson, int opcion)
        {
            ConceptoModel model = new ConceptoModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                model.Entidad = new PrConceptoDTO();
                string usuarioModificacion = base.UserName;
                DateTime fechaModificacion = DateTime.Now;

                PrConceptoDTO miObj = serializer.Deserialize<PrConceptoDTO>(dataJson);

                int idConcepto = 0;
                if (opcion == ConstantesDespacho.AccionNuevo)
                {
                    model.Entidad = miObj;
                    model.Entidad.Concepusucreacion = usuarioModificacion;
                    model.Entidad.Concepfeccreacion = fechaModificacion;

                    //Valido que no exista duplicados (ConcepAbrev - Concepactivo)
                    List<PrConceptoDTO> listadoConceptosActuales = appDespacho.ListarConceptoxFiltro(-2, "T", string.Empty, -1);
                    List<PrConceptoDTO> listadoConceptosActualesActivos = listadoConceptosActuales.Where(x => x.Concepactivo == "1").ToList();
                    PrConceptoDTO duplicado = listadoConceptosActualesActivos.Find(x => x.Concepabrev.Trim() == miObj.Concepabrev.Trim());
                    if(duplicado != null)
                    {
                        throw new ArgumentException("Error: Existe Concepto activo con la misma abreviatura."); 
                    }

                    //Validar concepto
                    //var listadoConceptosActuales = appDespacho.ListarConceptoxFiltro(-2, "T", string.Empty, -1);
                    var entidad = appDespacho.ObtenerRegistroPorCriterio(model.Entidad, listadoConceptosActuales);
                    if (entidad != null)
                    {
                        throw new ArgumentException("Error: Existe Concepto con el mismo nombre y Categoría de grupo.");
                    }

                    // Graba los cambio en la BD
                    idConcepto = appDespacho.SavePrConcepto(model.Entidad);
                    model.IdConcepto = idConcepto;
                }
                else
                {
                    var conceptoEdit = miObj;
                    model.Entidad = appDespacho.GetByIdPrConcepto(conceptoEdit.Concepcodi);

                    //Valido que no exista duplicados (ConcepAbrev - Concepactivo)
                    List<PrConceptoDTO> listadoConceptosActuales = appDespacho.ListarConceptoxFiltro(-2, "T", string.Empty, -1);
                    List<PrConceptoDTO> listadoConceptosActualesActivos = listadoConceptosActuales.Where(x => x.Concepactivo == "1" && x.Concepcodi != conceptoEdit.Concepcodi).ToList();
                    PrConceptoDTO duplicado = listadoConceptosActualesActivos.Find(x => x.Concepabrev.Trim() == miObj.Concepabrev.Trim());
                    if (duplicado != null)
                    {
                        throw new ArgumentException("Error: Existe Concepto activo con la misma abreviatura.");
                    }

                    //capturar valores
                    model.Entidad.Concepdesc = conceptoEdit.Concepdesc ?? "";
                    model.Entidad.Concepnombficha = conceptoEdit.Concepnombficha ?? "";
                    model.Entidad.Concepabrev = conceptoEdit.Concepabrev ?? "";
                    model.Entidad.Concepdefinicion = conceptoEdit.Concepdefinicion ?? "";
                    model.Entidad.Conceptipo = conceptoEdit.Conceptipo ?? "";
                    model.Entidad.Concepunid = conceptoEdit.Concepunid ?? "";
                    model.Entidad.Catecodi = conceptoEdit.Catecodi;
                    model.Entidad.Concepfichatec = conceptoEdit.Concepfichatec ?? "N";
                    model.Entidad.Conceptipolong1 = conceptoEdit.Conceptipolong1;
                    model.Entidad.Conceptipolong2 = conceptoEdit.Conceptipolong2;
                    model.Entidad.Concepactivo = conceptoEdit.Concepactivo;
                    model.Entidad.Conceppropeq = conceptoEdit.Conceppropeq;
                    model.Entidad.Concepocultocomentario = conceptoEdit.Concepocultocomentario;
                    model.Entidad.Concepliminf = conceptoEdit.Concepliminf;
                    model.Entidad.Conceplimsup = conceptoEdit.Conceplimsup;
                    model.Entidad.Concepflagcolor = conceptoEdit.Concepflagcolor;

                    model.Entidad.Concepusumodificacion = usuarioModificacion;
                    model.Entidad.Concepfecmodificacion = fechaModificacion;

                    // Graba los cambio en la BD
                    appDespacho.UpdatePrConcepto(model.Entidad);
                    idConcepto = model.Entidad.Concepcodi;
                    model.IdConcepto = idConcepto;
                }

                model.StrMensaje = opcion == 3 ? "¡La Información se grabó correctamente!" : "¡La Información se actualizó correctamente!";
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #region Descargar - Importar

        public ActionResult ConceptosImportacion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            ConceptoModel model = new ConceptoModel();

            return View(model);
        }

        /// <summary>
        /// ExportarConceptos
        /// </summary>
        /// <param name="famcodi"></param>
        /// <param name="fichaTecnica"></param>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarConceptos(int famcodi, string fichaTecnica, string nombre, int estado)
        {
            ConceptoModel model = new ConceptoModel();

            try
            {
                base.ValidarSesionJsonResult();

                //famcodi = famcodi == 0 ? -2 : famcodi;
                var listaConceptos = appDespacho.ListarConceptoxFiltro(famcodi, fichaTecnica, nombre, estado);

                string fileName = ConstantesDespacho.NombrePlantillaExcelConceptos;
                string pathOrigen = ConstantesFichaTecnica.FolderRaizFichaTecnica + ConstantesFichaTecnica.Plantilla;
                string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                appDespacho.GenerarExcelConceptos(listaConceptos, pathDestino, fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// AbrirArchivo
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual FileResult AbrirArchivo(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string strArchivoTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes + file;

            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, sFecha + "_" + file);
        }

        /// <summary>
        /// UploadConcepto
        /// </summary>
        /// <param name="sFecha"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadConcepto(string sFecha)
        {
            try
            {
                base.ValidarSesionUsuario();
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string sNombreArchivo = sFecha + "_" + file.FileName;

                    if (FileServer.VerificarExistenciaFile(null, sNombreArchivo, path))
                    {
                        //FileServer.DeleteBlob(sNombreArchivo, path + ConstantesDespacho.Reportes);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// MostrarArchivoImportacion
        /// </summary>
        /// <param name="sFecha"></param>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarArchivoImportacion(string sFecha, string sFileName)
        {
            base.ValidarSesionUsuario();

            ConceptoModel model = new ConceptoModel();

            string fileName = sFecha + "_" + sFileName;
            model.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes);
            var documento = new FileData();

            foreach (var item in model.ListaDocumentos)
            {
                if (String.Equals(item.FileName, fileName))
                {
                    model.Documento = new FileData();
                    model.Documento = item;
                    break;
                }
            }

            return Json(model);
        }

        /// <summary>
        /// ImportarConceptosExcel
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarConceptosExcel(string fileName)
        {
            ConceptoModel model = new ConceptoModel();
            model.ListaConceptosCorrectos = new List<PrConceptoDTO>();
            model.ListaConceptosErrores = new List<PrConceptoDTO>();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                // Ruta de los archivos EXCEL leidos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes;

                // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                appDespacho.ValidarConceptosAImportarXLSX(path, fileName, base.UserName,
                                                       out List<PrConceptoDTO> lstRegConceptosCorrectos,
                                                       out List<PrConceptoDTO> lstRegConceptosErroneos,
                                                       out List<PrConceptoDTO> listaNuevo,
                                                       out List<PrConceptoDTO> listaModificado);

                model.ListaConceptosCorrectos = lstRegConceptosCorrectos;
                model.ListaConceptosErrores = lstRegConceptosErroneos;

                //validación si existen errores
                if (lstRegConceptosErroneos.Any())
                {
                    string filenameCSV = appDespacho.GenerarArchivoConceptosErroneasCSV(path, lstRegConceptosErroneos);
                    model.FileName = filenameCSV;

                    throw new Exception("¡No se guardó la información! Existen datos o registros que no permiten cargar el archivo completo.");
                }

                //validación si no tiene datos
                if (lstRegConceptosCorrectos.Count() == 0)
                {
                    throw new Exception("Por favor ingrese un documento con registros nuevos y/o actualizados.");
                }

                //Ejecución de la grabación de conceptos de un archivo Excel
                appDespacho.GuardarDatosMasivosConceptos(listaNuevo, listaModificado, base.UserName);

                model.StrMensaje = "¡La Información se grabó correctamente!";
            }
            catch (Exception ex)
            {
                model.StrMensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        /// <summary>
        /// AbrirArchivoCSV
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivoCSV(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes + file;

            string app = ConstantesDespacho.AppCSV;

            // lo guarda el CSV en la carpeta de descarga
            return File(path, app, file);
        }

        /// <summary>
        /// Metodo para eliminar los archivos de conceptos importar
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <returns>Entero</returns>
        [HttpPost]
        public int EliminarArchivosImportacionNuevo(string nombreArchivo)
        {
            base.ValidarSesionUsuario();

            string nombreFile = string.Empty;

            ConceptoModel modelArchivos = new ConceptoModel();
            modelArchivos.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes);
            foreach (var item in modelArchivos.ListaDocumentos)
            {
                string subString = item.FileName;
                if (subString == nombreArchivo)
                {
                    nombreFile = item.FileName;
                    break;
                }
            }

            if (FileServer.VerificarExistenciaFile(null, nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes))
            {
                FileServer.DeleteBlob(nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes);
            }

            return -1;
        }

        #endregion
    }
}
