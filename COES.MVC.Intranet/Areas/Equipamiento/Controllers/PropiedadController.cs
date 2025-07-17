using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Areas.Evaluacion.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Evaluacion;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EvaluacionHelperCalculos = COES.Servicios.Aplicacion.Evaluacion.Helper;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class PropiedadController : BaseController
    {
        private EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        private CalculosAppServicio calculos = new CalculosAppServicio();

        #region Declaración de variables de Sesión

        private List<DatoComboBox> listadoFichaTecnica = new List<DatoComboBox>();
        private List<DatoComboBox> listadoSINO = new List<DatoComboBox>();
        private List<DatoComboBox> listadoTipo = new List<DatoComboBox>();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public PropiedadController()
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
            listadoTipo.Add(new DatoComboBox { Descripcion = "FORMULA_FAMILIA", Valor = "FORMULA_FAMILIA" });
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

            PropiedadModel model = new PropiedadModel();

            model.ListaFichatecnica = listadoFichaTecnica;
            model.ListaFamilia = appEquipamiento.ListEqFamilias().OrderBy(x => x.Famnomb).ToList();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);// verificar rol

            //eliminar los archivos temporales en reporte
            appEquipamiento.EliminarArchivosReporte();

            return View(model);
        }

        /// <summary>
        /// ListadoPropiedades
        /// </summary>
        /// <param name="famcodi"></param>
        /// <param name="fichaTecnica"></param>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoPropiedades(int famcodi, string fichaTecnica, string nombre, int estado)
        {
            PropiedadModel model = new PropiedadModel();

            //famcodi = famcodi == 0 ? -2 : famcodi;

            model.ListaPropiedad = appEquipamiento.GetByCriteriaEqPropiedades(famcodi, fichaTecnica, nombre, estado);

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// ObtenerPropiedad
        /// </summary>
        /// <param name="propcodi"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPropiedad(int propcodi, bool editable)
        {
            PropiedadModel model = new PropiedadModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaFichatecnica = listadoSINO;
                model.ListaFamilia = appEquipamiento.ListEqFamilias().OrderBy(x => x.Famnomb).ToList();
                model.ListaPropiedad = appEquipamiento.ListEqPropiedads();
                model.ListaTipoDato = listadoTipo;

                model.Entidad = new EqPropiedadDTO();

                if (propcodi == -1)
                {
                    model.AccionNuevo = true;

                    model.Entidad.Propfeccreacion = DateTime.Now;
                }
                else
                {
                    model.Entidad = appEquipamiento.GetByIdEqPropiedad(propcodi);
                    model.Entidad.StrPropliminf = model.Entidad.Propliminf != null ? model.Entidad.Propliminf.ToString() : "";
                    model.Entidad.StrProplimsup = model.Entidad.Proplimsup != null ? model.Entidad.Proplimsup.ToString() : "";
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

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// PropiedadGuardar
        /// </summary>
        /// <param name="dataJson"></param>
        /// <param name="opcion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PropiedadGuardar(string dataJson, int opcion)
        {
            PropiedadModel model = new PropiedadModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                model.Entidad = new EqPropiedadDTO();
                string usuarioModificacion = base.UserName;
                DateTime fechaModificacion = DateTime.Now;

                int idPropiedad = -1;
                if (opcion == ConstantesEquipamientoAppServicio.AccionNuevo)
                {
                    model.Entidad = serializer.Deserialize<EqPropiedadDTO>(dataJson);
                    model.Entidad.Propusucreacion = usuarioModificacion;
                    model.Entidad.Propfeccreacion = fechaModificacion;

                    #region GESPROTECT -  Validar Formula 20250312
                    if(model.Entidad.Proptipo == "FORMULA_FAMILIA" && !string.IsNullOrEmpty(model.Entidad.Propformula))
                    {
                        var calculosEquipo = calculos.ListValidarFormulas(model.Entidad.Famcodi, model.Entidad.Propformula);

                        EvaluacionHelperCalculos.n_calc.EvaluarFormulas(calculosEquipo);

                        if (calculosEquipo.Any(p => p.Identificador.ToUpper() == "EVALUADOR" && p.Estado == 0))
                        {
                            throw new ArgumentException("Error: La fórmula no es válida.");
                        }

                    }
                    #endregion

                    //Validar duplicado propiedad
                    var listadoPropiedadesActuales = appEquipamiento.GetByCriteriaEqPropiedades(-2, "T", string.Empty, -1);
                    var entidad = appEquipamiento.ObtenerRegistroPorCriterio(model.Entidad, listadoPropiedadesActuales);
                    if (entidad != null)
                    {
                        throw new ArgumentException("Error: Existe Propiedad con el mismo nombre y tipo de equipo.");
                    }

                    // Graba los cambio en la BD
                    idPropiedad = appEquipamiento.SaveEqPropiedad(model.Entidad);
                    model.IdPropiedad = idPropiedad;
                }
                else
                {
                    var propiedadEdit = serializer.Deserialize<EqPropiedadDTO>(dataJson);
                    model.Entidad = appEquipamiento.GetByIdEqPropiedad(propiedadEdit.Propcodi);

                    //capturar valores
                    model.Entidad.Propnomb = propiedadEdit.Propnomb ?? "";
                    model.Entidad.Propnombficha = propiedadEdit.Propnombficha ?? "";
                    model.Entidad.Propabrev = propiedadEdit.Propabrev ?? "";
                    model.Entidad.Propdefinicion = propiedadEdit.Propdefinicion ?? "";
                    model.Entidad.Proptipo = propiedadEdit.Proptipo ?? "";
                    model.Entidad.Propunidad = propiedadEdit.Propunidad ?? "";
                    model.Entidad.Famcodi = propiedadEdit.Famcodi;
                    model.Entidad.Propfichaoficial = propiedadEdit.Propfichaoficial ?? "";
                    model.Entidad.Proptipolong1 = propiedadEdit.Proptipolong1;
                    model.Entidad.Proptipolong2 = propiedadEdit.Proptipolong2;
                    model.Entidad.Propactivo = propiedadEdit.Propactivo;
                    model.Entidad.Propocultocomentario = propiedadEdit.Propocultocomentario;
                    //model.Entidad.Propcodipadre = propiedadEdit.Propcodipadre;
                    model.Entidad.Propusumodificacion = usuarioModificacion;
                    model.Entidad.Propfecmodificacion = fechaModificacion;
                    model.Entidad.Propliminf = propiedadEdit.Propliminf;
                    model.Entidad.Proplimsup = propiedadEdit.Proplimsup;
                    model.Entidad.Propflagcolor = propiedadEdit.Propflagcolor;
                    model.Entidad.Propformula = propiedadEdit.Propformula ?? "";

                    #region GESPROTECT -  Validar Formula 20250312
                    if (model.Entidad.Proptipo == "FORMULA_FAMILIA" && !string.IsNullOrEmpty(model.Entidad.Propformula))
                    {
                        var calculosEquipo = calculos.ListValidarFormulas(model.Entidad.Famcodi, model.Entidad.Propformula);

                        EvaluacionHelperCalculos.n_calc.EvaluarFormulas(calculosEquipo);

                        if (calculosEquipo.Any(p => p.Identificador.ToUpper() == "EVALUADOR" && p.Estado == 0))
                        {
                            model.Resultado = "-1";
                            model.StrMensaje = "Error: La fórmula no es válida.";
                            return Json(model);
                        }

                    }
                    #endregion

                    // Graba los cambio en la BD
                    appEquipamiento.UpdateEqPropiedad(model.Entidad);
                    idPropiedad = model.Entidad.Propcodi;
                    model.IdPropiedad = idPropiedad;
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

        public ActionResult PropiedadesImportacion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            PropiedadModel model = new PropiedadModel();

            return View(model);
        }

        /// <summary>
        /// ExportarPropiedades
        /// </summary>
        /// <param name="famcodi"></param>
        /// <param name="fichaTecnica"></param>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarPropiedades(int famcodi, string fichaTecnica, string nombre, int estado)
        {
            PropiedadModel model = new PropiedadModel();

            try
            {
                base.ValidarSesionJsonResult();

                //famcodi = famcodi == 0 ? -2 : famcodi;
                var listaPropiedades = appEquipamiento.GetByCriteriaEqPropiedades(famcodi, fichaTecnica, nombre, estado);

                //string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                string fileName = ConstantesEquipamientoAppServicio.NombrePlantillaExcelPropiedades;
                string pathOrigen = ConstantesFichaTecnica.FolderRaizFichaTecnica + ConstantesFichaTecnica.Plantilla;
                string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                appEquipamiento.GenerarExcelPropiedades(listaPropiedades, pathDestino, fileName);

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
            string strArchivoTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes + file;

            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, sFecha + "_" + file);
        }

        /// <summary>
        /// UploadPropiedad
        /// </summary>
        /// <param name="sFecha"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadPropiedad(string sFecha)
        {
            try
            {
                base.ValidarSesionUsuario();
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string sNombreArchivo = sFecha + "_" + file.FileName;

                    if (FileServer.VerificarExistenciaFile(null, sNombreArchivo, path))
                    {
                        //FileServer.DeleteBlob(sNombreArchivo, path + ConstantesEquipamientoAppServicio.Reportes);
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

            PropiedadModel model = new PropiedadModel();

            string fileName = sFecha + "_" + sFileName;
            model.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes);
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
        /// ImportarPropiedadesExcel
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarPropiedadesExcel(string fileName)
        {
            PropiedadModel model = new PropiedadModel();
            model.ListaPropiedadesCorrectas = new List<EqPropiedadDTO>();
            model.ListaPropiedadesErrores = new List<EqPropiedadDTO>();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                // Ruta de los archivos EXCEL leidos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;

                // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                appEquipamiento.ValidarPropiedadesAImportarXLSX(path, fileName, base.UserName,
                                                       out List<EqPropiedadDTO> lstRegPropiedadesCorrectos,
                                                       out List<EqPropiedadDTO> lstRegPropiedadesErroneos,
                                                       out List<EqPropiedadDTO> listaNuevo,
                                                       out List<EqPropiedadDTO> listaModificado);

                model.ListaPropiedadesCorrectas = lstRegPropiedadesCorrectos;
                model.ListaPropiedadesErrores = lstRegPropiedadesErroneos;

                //validación si existen errores
                if (lstRegPropiedadesErroneos.Any())
                {
                    string filenameCSV = appEquipamiento.GenerarArchivoPropiedadesErroneasCSV(path, lstRegPropiedadesErroneos);
                    model.FileName = filenameCSV;

                    throw new Exception("¡No se guardó la información! Existen datos o registros que no permiten cargar el archivo completo.");
                }

                //validación si no tiene datos
                if (lstRegPropiedadesCorrectos.Count() == 0)
                {
                    throw new Exception("Por favor ingrese un documento con registros nuevos y/o actualizados.");
                }

                //Ejecución de la grabación de propiedades de un archivo Excel
                appEquipamiento.GuardarDatosMasivosPropiedades(listaNuevo, listaModificado, base.UserName);

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
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes + file;

            string app = ConstantesEquipamientoAppServicio.AppCSV;

            // lo guarda el CSV en la carpeta de descarga
            return File(path, app, file);
        }

        /// <summary>
        /// EliminarArchivosImportacionNuevo
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        [HttpPost]
        public int EliminarArchivosImportacionNuevo(string nombreArchivo)
        {
            base.ValidarSesionUsuario();

            string nombreFile = string.Empty;

            PropiedadModel modelArchivos = new PropiedadModel();
            modelArchivos.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes);
            foreach (var item in modelArchivos.ListaDocumentos)
            {
                string subString = item.FileName;
                if (subString == nombreArchivo)
                {
                    nombreFile = item.FileName;
                    break;
                }
            }

            if (FileServer.VerificarExistenciaFile(null, nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes))
            {
                FileServer.DeleteBlob(nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes);
            }

            return -1;
        }

        #endregion

        #region GESPROTECT

        public JsonResult ObtenerPropiedadFunciones(int famcodi)
        {
            PropiedadModel model = new PropiedadModel();
            try
            {
                base.ValidarSesionJsonResult();
               
                model.ListaPropiedad = calculos.ListPropiedades(famcodi);
                model.ListaFunciones = calculos.ListFunciones();             

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(model);
        }

        #endregion
    }
}
