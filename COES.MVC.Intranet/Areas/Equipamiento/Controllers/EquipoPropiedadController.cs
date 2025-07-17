using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class EquipoPropiedadController : BaseController
    {
        GeneralAppServicio appGeneral = new GeneralAppServicio();
        EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();

        #region Declaración de variables de Sesión

        readonly List<EstadoModel> lsEstadosEquipo = new List<EstadoModel>();
        readonly List<DatoComboBox> listadoSINO = new List<DatoComboBox>();
        private readonly List<int> _listCentrales = new List<int>();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session["NombreArchivoPropiedades"] != null) ?
                    Session["NombreArchivoPropiedades"].ToString() : null;
            }
            set { Session["NombreArchivoPropiedades"] = value; }
        }

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session["FileNamePropiedades"] != null) ?
                    Session["FileNamePropiedades"].ToString() : null;
            }
            set { Session["FileNamePropiedades"] = value; }
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
                Log.Error(NameController, ex);
                throw;
            }
        }

        public EquipoPropiedadController()
        {
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "A", EstadoDescripcion = "Activo" });
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "B", EstadoDescripcion = "Baja" });
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "F", EstadoDescripcion = "Fuera de COES" });
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "P", EstadoDescripcion = "Proyecto" });
            lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "X", EstadoDescripcion = "Anulado" });

            listadoSINO.Add(new DatoComboBox { Descripcion = "SI", Valor = "S" });
            listadoSINO.Add(new DatoComboBox { Descripcion = "NO", Valor = "N" });

            _listCentrales.Add(4);
            _listCentrales.Add(5);
            _listCentrales.Add(37);
            _listCentrales.Add(39);

            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        public ActionResult Index()
        {
            ValidarSesionUsuario();
            var modelo = new IndexEquipoModel();
            modelo.ListaTipoEmpresa =
                appGeneral.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).OrderBy(t => t.Tipoemprdesc).ToList();
            modelo.ListaTipoEquipo =
                appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            modelo.ListaEstados = lsEstadosEquipo;
            modelo.iEmpresa = 0;
            modelo.iTipoEmpresa = 0;
            modelo.iTipoEquipo = 0;
            modelo.sEstadoCodi = "A";
            modelo.ListaEmpresa = new List<SiEmpresaDTO>();

            modelo.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            bool AccesoNuevo = modelo.TienePermiso;
            bool AccesoEditar = modelo.TienePermiso;

            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";

            modelo.AccesoNuevo = AccesoNuevo;
            modelo.AccesoEditar = AccesoEditar;

            //eliminar los archivos temporales en reporte
            appEquipamiento.EliminarArchivosReporte();

            return View(modelo);
        }

        public JsonResult CargarEmpresas(int idTipoEmpresa)
        {
            var entitys = new List<SiEmpresaDTO>();
            entitys = this.appGeneral.ListadoComboEmpresasPorTipo(idTipoEmpresa).Where(t => t.Emprestado.Trim() != "E").ToList(); ;
            var list = new SelectList(entitys, "Emprcodi", "Emprnomb");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult ListadoEquipos(IndexEquipoModel model)
        {
            int iEmpresa = model.iEmpresa;
            int iFamilia = model.iTipoEquipo;
            int iTipoEmpresa = model.iTipoEmpresa;
            int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
            string sEstado = model.sEstadoCodi;
            string sNombre = model.NombreEquipo;
            var lsResultado = appEquipamiento.ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo,
                sEstado, sNombre, model.NroPagina, Constantes.PageSizeEvento);
            foreach (var oEquipo in lsResultado)
            {
                oEquipo.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado);
                oEquipo.Osigrupocodi = EquipamientoHelper.EstiloEstado(oEquipo.Equiestado);

            }
            model.ListadoEquipamiento = lsResultado;
            return PartialView(model);
        }
        [HttpPost]
        public PartialViewResult PaginadoEquipos(IndexEquipoModel model)
        {
            int iEmpresa = model.iEmpresa;
            int iFamilia = model.iTipoEquipo;
            int iTipoEmpresa = model.iTipoEmpresa;
            int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
            string sEstado = model.sEstadoCodi;
            string sNombre = model.NombreEquipo;
            model.IndicadorPagina = false;
            int nroRegistros = appEquipamiento.TotalEquipamiento(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado,
                sNombre);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ExportarPropiedades(IndexEquipoModel model)
        {
            int result = 1;
            try
            {
                int iEmpresa = model.iEmpresa;
                int iFamilia = model.iTipoEquipo;
                int iTipoEmpresa = model.iTipoEmpresa;
                int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
                string sEstado = model.sEstadoCodi;
                string sNombre = model.NombreEquipo;
                var lsResultado = appEquipamiento.ExportarDatosPropiedades(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, sNombre);
                foreach (var oEquipo in lsResultado)
                {
                    oEquipo.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado);
                    oEquipo.Osigrupocodi = EquipamientoHelper.EstiloEstado(oEquipo.Equiestado);
                }

                string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEquipamiento].ToString();
                string nombreReporteEquipos = NombreArchivo.ReporteEquipos;
                string plantilla = Constantes.PlantillaListadoPropiedadesEquipos;

                ExcelDocumentEquipamiento.GenerarDatosPropiedades(ruta, nombreReporteEquipos, plantilla, lsResultado);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                result = -1;
            }
            return Json(result);
        }

        [HttpGet]
        public virtual ActionResult DescargarPlantilla()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEquipamiento] + NombreArchivo.ReporteEquipos;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteEquipos);
        }

        [HttpGet]
        /// <summary>
        /// Permite descargar el archivo Excel
        /// </summary>
        /// <param name="file">Nombre del archivo</param>
        /// <returns>Archivo</returns>
        public virtual FileResult AbrirArchivo(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes + file;

            string app = ConstantesEquipamientoAppServicio.AppExcel;

            return File(path, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Método encargado de subir el archivo a servidor
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.FileName = fileRandom + "." + "xlsx";
                    string fileName = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEquipamiento] + this.FileName;
                    this.NombreFile = fileName;
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

        public JsonResult ImportarDataPropiedades(IndexEquipoModel model)
        {
            try
            {
                EquipamientoAppServicio equipoServicio = new EquipamientoAppServicio();


                //equipoServicio.ListarPropiedadesPorFiltro()

                var resultado = ExcelDocumentEquipamiento.GuardarDatosPropiedades(this.NombreFile, User.Identity.Name);


                //Validar formato
                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                throw;
            }
        }

        #region Exportar - Importar plantilla Reporte

        public ActionResult PropiedadesMasivoImportacion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            IndexEquipoModel model = new IndexEquipoModel();

            return View(model);
        }

        /// <summary>
        /// ExportarReporte
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporte(IndexEquipoModel model)
        {
            try
            {
                int iEmpresa = model.iEmpresa;
                int iFamilia = model.iTipoEquipo;
                int iTipoEmpresa = model.iTipoEmpresa;
                int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
                string sEstado = model.sEstadoCodi;
                string sNombre = model.NombreEquipo;
                var lsResultado = appEquipamiento.ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, sNombre, 1, int.MaxValue);

                foreach (var oEquipo in lsResultado)
                {
                    // obtener propiedades actualizadas para cada equipo
                    var fechaConsulta = DateTime.Now;
                    string filtroFicha = model.TipoExportacion == ConstantesEquipamientoAppServicio.OpcionReporteFT ? "S" : "-1";
                    appEquipamiento.GetGridExcelWebPropiedadesEquipo(fechaConsulta, oEquipo.Equicodi, filtroFicha, false, out HandsonModel handson, out List<EqPropequiDTO> listadoPropiedades, out List<int> listaDespuesFicha8);
                    oEquipo.PropiedadesEquipo = listadoPropiedades;

                    if (model.TipoExportacion == ConstantesEquipamientoAppServicio.OpcionReporteFT) // si es la opción de reporte propiedades de FT
                        oEquipo.PropiedadesEquipo = oEquipo.PropiedadesEquipo.Where(x => x.Propfichaoficial == ConstantesEquipamientoAppServicio.Si).ToList();

                    oEquipo.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado);
                    oEquipo.Osigrupocodi = EquipamientoHelper.EstiloEstado(oEquipo.Equiestado);
                }

                string fileName = ConstantesEquipamientoAppServicio.NombrePlantillaExcelParametros;
                string pathOrigen = ConstantesFichaTecnica.FolderRaizFichaTecnica + ConstantesFichaTecnica.Plantilla;
                string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;
                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                ExcelDocumentEquipamiento.GenerarReporte(lsResultado, pathDestino, fileName);
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

            IndexEquipoModel model = new IndexEquipoModel();

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
        /// ImportarPropiedadesMasivoExcel
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarPropiedadesMasivoExcel(string fileName)
        {
            //PropiedadModel model = new PropiedadModel();
            IndexEquipoModel model = new IndexEquipoModel();
            model.ListaPropEquiCorrectos = new List<EqPropequiDTO>();
            model.ListaPropEquiErrores = new List<EqPropequiDTO>();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                // Ruta de los archivos EXCEL leidos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;

                // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                appEquipamiento.ValidarPropiedadesMasivoAImportar(path, fileName, base.UserName,
                                                       out List<EqPropequiDTO> lstRegPropiedadesCorrectos,
                                                       out List<EqPropequiDTO> lstRegPropiedadesErroneos,
                                                       out List<EqPropequiDTO> listaNuevo,
                                                       out List<EqPropequiDTO> listaModificado);

                model.ListaPropEquiCorrectos = lstRegPropiedadesCorrectos;
                model.ListaPropEquiErrores = lstRegPropiedadesErroneos;

                //validación si existen errores
                if (lstRegPropiedadesErroneos.Any())
                {
                    string filenameCSV = appEquipamiento.GenerarArchivoLogParametrosErroneosCSV(path, lstRegPropiedadesErroneos);
                    model.FileName = filenameCSV;

                    throw new Exception("¡No se guardó la información! Existen datos o registros que no permiten cargar el archivo completo.");
                }

                //validación si no tiene datos
                if (lstRegPropiedadesCorrectos.Count() == 0)
                {
                    throw new Exception("Por favor ingrese un documento con registros nuevos y/o actualizados.");
                }

                //Ejecución de la grabación de propiedades de un archivo Excel
                appEquipamiento.CargaMasivaPropiedadesEquipos(listaNuevo, listaModificado, base.UserName);

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
        /// Abrir archivo log de errores en formato CSV 
        /// </summary>
        /// <param name="file">Nombre del archivo</param>               
        /// <returns>Cadena del nombre del archivo</returns>
        public virtual ActionResult AbrirArchivoCSV(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes + file;

            string app = ConstantesEquipamientoAppServicio.AppCSV;

            // lo guarda el CSV en la carpeta de descarga
            return File(path, app, file);
        }

        /// <summary>
        /// Metodo para eliminar los archivos de propiedades importar
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <returns>Entero</returns>
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

    }
}