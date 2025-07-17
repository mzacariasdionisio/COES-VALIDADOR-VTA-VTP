using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PotenciaFirme;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Controller
{
    public class AutomatizacionController : BaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AutomatizacionController));
        private static readonly string NameController = "AutomatizacionController";


        private readonly PotenciaFirmeRemunerableAppServicio _pfrService;
        public AutomatizacionController()
        {
            _pfrService = new PotenciaFirmeRemunerableAppServicio();
        }

        /// <inheritdoc />
        /// <summary>
        /// Protected de log de errores page
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

        #region UNIFILAR

        /// <summary>
        /// Muestra la vista principal de Unifilar
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public ActionResult Indexunifilar(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _pfrService.GetPeriodoActual();
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

                CrearDirectorioRemunerable();

                if (pericodi.HasValue)
                {
                    model.IdPeriodo = pericodi.Value;
                    var regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                    model.AnioActual = regPeriodo.FechaIni.Year;
                    model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);

        }

        /// <summary>
        /// Listado de Unifilar
        /// </summary>
        /// <param name="pfrpercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoUnifilar(int pfrpercodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            PfrPeriodoDTO pfrPeriodo = _pfrService.GetByIdPfrPeriodo(pfrpercodi);

            model.ListaParametros = _pfrService
                .ListarParametrosConfiguracionPorFecha(pfrPeriodo.FechaFin, ConstantesPotenciaFirmeRemunerable.ConcepcodiIngresos);
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            string path = "";
            //Crear path unifilar
            path = base.PathFiles + "//" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile + ConstantesPotenciaFirmeRemunerable.SNombreCarpetaUnifilar + "\\";
            //path = base.PathFiles + "//";

            model.ListaDocumentos = FileServer.ListarArhivos(path, null);
            List<string> nombres = new List<string>();
            foreach (var item in model.ListaDocumentos)
            {
                nombres.Add(item.FileName);
            }

            var periodofiltro = pfrPeriodo.Pfrperanio.ToString() + pfrPeriodo.Pfrpermes.ToString();
            var filesperiodo = nombres.Where(x => x.Contains(periodofiltro.ToString())).ToList();

            string nombrefinal = string.Empty;
            if (filesperiodo.Count != 0)
            {
                filesperiodo = filesperiodo.OrderByDescending(x => x).ToList();
                nombrefinal = filesperiodo.First();
            }
            else
            {
                nombres = nombres.OrderByDescending(x => x).ToList();
                if (nombres.Count > 0)
                {
                    nombrefinal = nombres.First();
                }
            }

            model.DiagramaUnifilar = nombrefinal;

            return PartialView("_ListadoUnifilar", model);
        }

        /// <summary>
        /// metodos archivos Unifilar
        /// </summary>
        /// <param name="sFecha"></param>
        /// <param name="sModulo"></param>
        /// <param name="nroOrden"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadUnifilar(int pfrpercodi)
        {
            base.ValidarSesionUsuario();

            PfrPeriodoDTO pfrPeriodo = _pfrService.GetByIdPfrPeriodo(pfrpercodi);

            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            string sNombreArchivo = string.Empty;
            string pathActual = "";
            string sNombreOriginal = "";
            string rootPath = FileServer.GetDirectory();
            string currentUserSession = HttpContext.Session.SessionID;

            // Ruta base de Potencia Firme Remunerable         
            string pathBasePFR = base.PathFiles + "\\" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile;

            // crear y Obtener path carpeta actual
            string nombreCarpetaActual = ConstantesPotenciaFirmeRemunerable.SNombreCarpetaUnifilar;
            FileServer.CreateFolder(pathBasePFR, nombreCarpetaActual, null); // para asegurarnos de su existencia
            pathActual = base.PathFiles + "//" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile + nombreCarpetaActual + "\\";
            //path = base.PathFiles + "//";

            var documentos = FileServer.ListarArhivos(pathActual, null);
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreOriginal = file.FileName;

                    var periodofiltro = pfrPeriodo.Pfrperanio.ToString() + pfrPeriodo.Pfrpermes.ToString();
                    string[] partes = sNombreOriginal.Split('_');
                    if (documentos.Count != 0 && partes.Length != 3)
                    {
                        throw new Exception("Debe descargar el formato del archivo y subir con el mismo nombre");
                    }

                    string nombre = documentos.Count == 0 ? partes[0] : partes[2];

                    int indexOf = nombre.LastIndexOf('.');
                    string extension = nombre.Substring(indexOf + 1, nombre.Length - indexOf - 1);

                    sNombreOriginal = nombre.Substring(0, indexOf) + "." + extension;

                    //obtener lista por peridodo y poner versión
                    var listaDocumentos = FileServer.ListarArhivos(pathActual, null);
                    var version = ObtenerVersion(pfrpercodi, listaDocumentos);
                    version++;

                    var fecha = string.Format("{0}{1}", pfrPeriodo.Pfrperanio, pfrPeriodo.Pfrpermes);

                    sNombreArchivo = fecha + "_" + +version + "_" + sNombreOriginal;

                    if (FileServer.VerificarExistenciaFile(null, pathActual + "\\" + sNombreArchivo, null))
                    {
                        FileServer.DeleteBlob(pathActual + "\\" + sNombreArchivo, null);
                    }

                    FileServer.UploadFromStream(file.InputStream, pathActual, sNombreArchivo, null);
                }
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = "Error: " + ex.Message;
                model.Resultado = "-1";
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// obtener la version de unifilar para un determinado periodo
        /// </summary>
        /// <param name="pfrpercodi"></param>
        /// <param name="listaDocumentos"></param>
        /// <returns></returns>
        public int ObtenerVersion(int pfrpercodi, List<FileData> listaDocumentos)
        {
            PfrPeriodoDTO pfrPeriodo = _pfrService.GetByIdPfrPeriodo(pfrpercodi);

            List<string> nombres = new List<string>();
            foreach (var item in listaDocumentos)
            {
                nombres.Add(item.FileName);
            }
            var periodofiltro = pfrPeriodo.Pfrperanio.ToString() + pfrPeriodo.Pfrpermes.ToString();
            var filesperiodo = nombres.Where(x => x.Contains(periodofiltro.ToString())).ToList();

            string nombrefinal = string.Empty;
            int version = 0;
            int num = 0;
            if (filesperiodo.Count != 0)
            {
                filesperiodo = filesperiodo.OrderByDescending(x => x).ToList();
                nombrefinal = filesperiodo.First();
                string[] separadas = nombrefinal.Split('_');
                Int32.TryParse(separadas[1], out num);
                version = num;
            }
            else
            {
                nombres = nombres.OrderByDescending(x => x).ToList();
                if (nombres.Count > 0)
                {
                    nombrefinal = nombres.First();
                    string[] separadas = nombrefinal.Split('_');
                    Int32.TryParse(separadas[1], out num);
                    version = num;
                }
            }

            return version;
        }

        #endregion

        #region General

        /// <summary>
        /// Listar periodo por año en formato JSON
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(anio);
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
        /// Permite descargar el formato para Unifilar
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato(string nombre, int orden)
        {
            try
            {
                // crear y Obtener path carpeta actual
                string nombreCarpetaActual = orden == 1 ? ConstantesPotenciaFirmeRemunerable.SNombreCarpetaUnifilar : ConstantesPotenciaFirmeRemunerable.SNombreCarpetaCargaGams;

                string pathBaseUnifilar = ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile + "\\" + nombreCarpetaActual + "\\" + nombre;

                Stream stream = FileServer.DownloadToStream(pathBaseUnifilar, null);
                int indexOf = nombre.LastIndexOf('.');
                string extension = nombre.Substring(indexOf + 1, nombre.Length - indexOf - 1);

                if (stream != null)
                    return File(stream, extension, nombre);
                else
                {
                    Log.Info("Download - No se encontro el archivo: " + pathBaseUnifilar);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Crear directorios 
        /// </summary>
        private void CrearDirectorioRemunerable()
        {
            string path = base.PathFiles;
            if (!FileServer.VerificarExistenciaDirectorio(path, null))
            {
                throw new Exception("Error, No existe la carpeta que almacena los archivos");
            }

            string pathBasePFR = base.PathFiles + "\\" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile;

            //Crear path unifilar
            string nombreCarpetaActual1 = ConstantesPotenciaFirmeRemunerable.SNombreCarpetaUnifilar;
            FileServer.CreateFolder(pathBasePFR, nombreCarpetaActual1, null);
            if (!FileServer.VerificarExistenciaDirectorio(pathBasePFR + nombreCarpetaActual1, null))
            {
                throw new Exception("Error, No existe la carpeta que almacena los archivos Unifilar.");
            }

            //gams
            string nombreCarpetaActual2 = ConstantesPotenciaFirmeRemunerable.SNombreCarpetaCargaGams;
            FileServer.CreateFolder(pathBasePFR, nombreCarpetaActual2, null);

            if (!FileServer.VerificarExistenciaDirectorio(pathBasePFR + nombreCarpetaActual2, null))
            {
                throw new Exception("Error, No existe la carpeta que almacena los archivos Gams.");
            }
        }

        #endregion

        #region CÓDIGO FUENTE GAMS

        /// <summary>
        /// Muestra la vista principal de Unifilar
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public ActionResult IndexFuenteGams(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _pfrService.GetPeriodoActual();
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

                CrearDirectorioRemunerable();

                if (pericodi.HasValue)
                {
                    model.IdPeriodo = pericodi.Value;
                    var regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                    model.AnioActual = regPeriodo.FechaIni.Year;
                    model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);

        }

        /// <summary>
        /// Listado de Unifilar
        /// </summary>
        /// <param name="pfrpercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoFuenteGams(int pfrpercodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            PfrPeriodoDTO pfrPeriodo = _pfrService.GetByIdPfrPeriodo(pfrpercodi);
            model.FuenteGams1 = string.Empty;
            model.FuenteGams2 = string.Empty;

            model.ListaParametros = _pfrService
                .ListarParametrosConfiguracionPorFecha(pfrPeriodo.FechaFin, ConstantesPotenciaFirmeRemunerable.ConcepcodiIngresos);
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            string path = "";
            //Crear path para código fuente GAMS
            path = base.PathFiles + "//" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile + ConstantesPotenciaFirmeRemunerable.SNombreCarpetaCargaGams + "\\";
            //path = base.PathFiles + "//";
            model.ListaDocumentos = FileServer.ListarArhivos(path, null);

            //>>>>>>>>>>>>>>
            List<string> nombres = new List<string>();
            foreach (var item in model.ListaDocumentos)
            {
                nombres.Add(item.FileName);
            }

            var periodofiltro = pfrPeriodo.Pfrperanio.ToString() + pfrPeriodo.Pfrpermes.ToString();
            var filesperiodo = nombres.Where(x => x.Contains(periodofiltro.ToString())).ToList();

            if (filesperiodo.Count != 0)
            {
                filesperiodo = filesperiodo.OrderByDescending(x => x).ToList();
                var parte1 = filesperiodo.Where(x => x.Contains("P1")).OrderByDescending(x => x).ToList();
                var parte2 = filesperiodo.Where(x => x.Contains("P2")).OrderByDescending(x => x).ToList();

                model.FuenteGams1 = parte1.Count == 0 ? _pfrService.ObtenerNombreFuenteGamsActual(nombres, "P1") : parte1.First();
                model.FuenteGams2 = parte2.Count == 0 ? _pfrService.ObtenerNombreFuenteGamsActual(nombres, "P2") : parte2.First();
            }
            else
            {
                nombres = nombres.OrderByDescending(x => x).ToList();
                if (nombres.Count > 0)
                {
                    model.FuenteGams1 = _pfrService.ObtenerNombreFuenteGamsActual(nombres, "P1");
                    model.FuenteGams2 = _pfrService.ObtenerNombreFuenteGamsActual(nombres, "P2");
                }
            }

            return PartialView("_ListadoFuenteGams", model);
        }

        /// <summary>
        /// metodos archivos Unifilar
        /// </summary>
        /// <param name="sFecha"></param>
        /// <param name="sModulo"></param>
        /// <param name="nroOrden"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFuenteGams(int pfrpercodi, int orden)
        {
            base.ValidarSesionUsuario();

            PfrPeriodoDTO pfrPeriodo = _pfrService.GetByIdPfrPeriodo(pfrpercodi);

            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            string sNombreArchivo = string.Empty;
            string pathActual = "";
            string sNombreOriginal = "";
            string rootPath = FileServer.GetDirectory();
            string currentUserSession = HttpContext.Session.SessionID;

            // Ruta base de Potencia Firme Remunerable         
            string pathBasePFR = base.PathFiles + "\\" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile;

            // crear y Obtener path carpeta actual
            string nombreCarpetaActual = ConstantesPotenciaFirmeRemunerable.SNombreCarpetaCargaGams;
            FileServer.CreateFolder(pathBasePFR, nombreCarpetaActual, null); // para asegurarnos de su existencia
            pathActual = base.PathFiles + "//" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile + nombreCarpetaActual + "\\";

            //path = base.PathFiles + "//";

            var documentos = FileServer.ListarArhivos(pathActual, null);
            List<string> nombres = new List<string>();
            foreach (var item in documentos)
            {
                nombres.Add(item.FileName);
            }
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreOriginal = file.FileName;
                    string parte = orden == 1 ? "P1" : "P2";

                    var periodofiltro = pfrPeriodo.Pfrperanio.ToString() + pfrPeriodo.Pfrpermes.ToString();
                    string[] partes = sNombreOriginal.Split('_');
                    var filtrodocumento = nombres.Where(x => x.Contains(parte)).OrderByDescending(x => x).ToList();

                    if (filtrodocumento.Count != 0 && partes.Length != 4)
                    {
                        throw new Exception("Debe descargar el formato del archivo y subir con el mismo nombre");
                    }

                    string nombre = filtrodocumento.Count == 0 ? partes[0] : partes[3];

                    int indexOf = nombre.LastIndexOf('.');
                    string extension = nombre.Substring(indexOf + 1, nombre.Length - indexOf - 1);

                    sNombreOriginal = nombre.Substring(0, indexOf) + "." + extension;

                    //>>>>>>>>>>>>>>>>>>>>>>OBTENER VERSIÓN SEGÚN EL NÚMERO DE PARTE>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    var filesperiodo = nombres.Where(x => x.Contains(periodofiltro.ToString())).ToList();
                    filesperiodo = filesperiodo.Where(x => x.Contains(parte)).ToList();
                    nombres = nombres.OrderByDescending(x => x).ToList();
                    string nombrefinal = string.Empty;
                    int version = 0;
                    int num = 0;
                    //string parte = orden == 1 ? "P1": "P2";
                    if (filesperiodo.Count != 0)
                    {
                        filesperiodo = filesperiodo.OrderByDescending(x => x).ToList();
                        nombrefinal = filesperiodo.First();

                        string[] separadas = nombrefinal.Split('_');
                        Int32.TryParse(separadas[1], out num);
                        version = num;
                    }
                    version++;
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    var fecha = string.Format("{0}{1}", pfrPeriodo.Pfrperanio, pfrPeriodo.Pfrpermes);

                    sNombreArchivo = fecha + "_" + +version + "_" + parte + "_" + sNombreOriginal;

                    if (FileServer.VerificarExistenciaFile(null, pathActual + "\\" + sNombreArchivo, null))
                    {
                        FileServer.DeleteBlob(pathActual + "\\" + sNombreArchivo, null);
                    }

                    FileServer.UploadFromStream(file.InputStream, pathActual, sNombreArchivo, null);
                }
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = "Error: " + ex.Message;
                model.NRegistros = -1;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        #endregion
    }
}