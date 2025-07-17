using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Proteccion.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.MVC.Intranet.Controllers;
using log4net;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.Proteccion.Helper;
using FormatoHelper = COES.MVC.Intranet.Areas.Proteccion.Helper.FormatoHelper;
using System.Configuration;
using Newtonsoft.Json;

namespace COES.MVC.Intranet.Areas.Proteccion.Controllers
{
    public class ActualizacionMasivaController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(UbicacionCOESController));

        ProyectoActualizacionAppServicio servicioArea = new ProyectoActualizacionAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();

        #region propiedades

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreArchivo
        {
            get
            {
                return (Session[DatosSesionProteccion.SesionNombreArchivo] != null) ?
                    Session[DatosSesionProteccion.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionProteccion.SesionNombreArchivo] = value; }
        }


        /// <summary>
        /// Ruta y nombre del archivo
        /// </summary>
        public String RutaCompletaArchivo
        {
            get
            {
                return (Session[DatosSesionProteccion.RutaCompletaArchivo] != null) ?
                    Session[DatosSesionProteccion.RutaCompletaArchivo].ToString() : null;
            }
            set { Session[DatosSesionProteccion.RutaCompletaArchivo] = value; }
        }

        #endregion


        public ActualizacionMasivaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ActualizacionMasivaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ActualizacionMasivaController", ex);
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            CargaMasivaModel modelo = new CargaMasivaModel();
            modelo.ListTipoUso = equipoProteccion.ListPropCatalogoData(4);

            modelo.ListTipoUso.Insert(0, new EprPropCatalogoDataDTO { Eqcatdcodi = 0, Eqcatddescripcion = "TODOS" });
         
            ViewBag.FechaInicio = DateTime.Now.AddMonths(-3).ToString("dd/MM/yyyy");
            ViewBag.FechaFin = DateTime.Now.ToString("dd/MM/yyyy");

            return View(modelo);
        }

        
        public PartialViewResult ListaActualizacionMasivaLog(int tipoUsoId, string usuario, string fechaInicio, string fechaFin)
        {
            CargaMasivaModel model = new CargaMasivaModel();
            model.ListCargaMasiva = servicioArea.ListaCargaMasiva(tipoUsoId, usuario, fechaInicio, fechaFin).ToList();
           
            return PartialView("~/Areas/Proteccion/Views/ActualizacionMasiva/ListaActualizacionMasiva.cshtml", model);
        }

        public ActionResult CargaMasiva()
        {
            CargaMasivaModel modelo = new CargaMasivaModel();
            modelo.ListTipoUso = equipoProteccion.ListPropCatalogoData(4);

            ViewBag.CodigoReleTipoUsoGeneral = ConstantesProteccion.CodigoReleTipoUsoGeneral;

            return View("~/Areas/Proteccion/Views/ActualizacionMasiva/CargaMasiva.cshtml",modelo);
        }

        [HttpPost]
        public JsonResult ExportarRele(int tipoUsoId)
        {
            CargaMasivaModel model = new CargaMasivaModel();

            try
            {
                base.ValidarSesionJsonResult();

                switch (tipoUsoId)
                {
                    case ConstantesProteccion.CodigoReleTipoUsoGeneral:
                        {
                            string fileName = ConstantesProteccion.NombrePlantillaExcelReleUsoGeneral;
                            string pathOrigen = ConstantesProteccion.FolderGestProtec +"/"+ ConstantesProteccion.Plantilla;
                            string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesProteccion.RutaReportes;

                            FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                            servicioArea.GenerarExcelPlantilla(pathDestino, fileName, tipoUsoId);

                            model.Resultado = "1";
                            model.NombreArchivo = fileName;
                            model.StrMensaje = "";
                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoMandoSincronizado:
                        {
                            string fileName = ConstantesProteccion.NombrePlantillaExcelReleMandoSincronizado;
                            string pathOrigen = ConstantesProteccion.FolderGestProtec +"/" + ConstantesProteccion.Plantilla;
                            string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesProteccion.RutaReportes;

                            FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                            servicioArea.GenerarExcelPlantilla(pathDestino, fileName, tipoUsoId);

                            model.Resultado = "1";
                            model.NombreArchivo = fileName;
                            model.StrMensaje = "";
                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoTorsional:
                        {
                            string fileName = ConstantesProteccion.NombrePlantillaExcelReleTorsional;
                            string pathOrigen = ConstantesProteccion.FolderGestProtec + "/" + ConstantesProteccion.Plantilla;
                            string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesProteccion.RutaReportes;

                            FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                            servicioArea.GenerarExcelPlantilla(pathDestino, fileName, tipoUsoId);

                            model.Resultado = "1";
                            model.NombreArchivo = fileName;
                            model.StrMensaje = "";
                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoPmu:
                        {
                            string fileName = ConstantesProteccion.NombrePlantillaExcelRelePmu;
                            string pathOrigen = ConstantesProteccion.FolderGestProtec + "/"+ ConstantesProteccion.Plantilla;
                            string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesProteccion.RutaReportes;

                            FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                            servicioArea.GenerarExcelPlantilla(pathDestino, fileName, tipoUsoId);

                            model.Resultado = "1";
                            model.NombreArchivo = fileName;
                            model.StrMensaje = "";
                            break;
                        }
                }

               
            }
            catch (Exception ex)
            {
                log.Error("ActualizacionMasivaController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        public virtual FileResult AbrirArchivo(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesProteccion.RutaReportes + file;

            string app = ConstantesProteccion.AppExcel;

            return File(path, app, sFecha + "_" + file);
        }

        #region Metodos HandsonTable

        private List<string> ObtenerTitulosColumnas()
        {
            return new List<string>() {"", "Ítem", "Código de la Empresa", "Código de la Celda", "Código del Relé", "Nombre Relé","Fecha", "Estado",
                "Nivel de Tensión", "Sistema Relé", "Marca", "Modelo", "Ip","Is","Vp","Vs","Protecciones Coordinables", "Activo",
                "Interruptor que comanda","Delta de tensión","Delta de Ángulo","Delta de Frecuencia","Activo","U>[p.u.]","t>[s]","U>>[p.u.]",
                "t>>[s]","Activo","I>[A]","Activo","Acción", "Proyecto"};
        }

        private List<string> ObtenerTitulosColumnasMandoSincronizado()
        {
            return new List<string>() {"", "Ítem", "Código de la Empresa", "Código de la Celda", "Código del Relé", "Nombre Relé","Fecha", "Estado",
                "Nivel de Tensión", "Sistema Relé", "Marca", "Modelo", "Interruptor que comanda","Mando", "Proyecto"};
        }

        private List<string> ObtenerTitulosColumnasTorsional()
        {
            return new List<string>() {"", "Ítem", "Código de la Empresa", "Código de la Celda", "Código del Relé", "Nombre Relé","Fecha", "Estado",
                "Nivel de Tensión", "Sistema Relé", "Marca", "Modelo", "Medida de mitigación","Implementado", "Proyecto"};
        }

        private List<string> ObtenerTitulosColumnasPmu()
        {
            return new List<string>() {"", "Ítem", "Código de la Empresa", "Código de la Celda", "Código del Relé", "Nombre Relé","Fecha", "Estado",
                "Nivel de Tensión", "Sistema Relé", "Marca", "Modelo", "Acción","Implementado", "Proyecto"};
        }

        [HttpPost]
        public JsonResult LeerExcelSubido(int tipoUsoId)
        {
            try
            {
                string[][] matrizDatos = null;
                Respuesta matrizValida = new Respuesta();
                FormatoModel model = null;

                switch (tipoUsoId)
                {
                    case ConstantesProteccion.CodigoReleTipoUsoGeneral:
                        {
                            var titulos = ObtenerTitulosColumnas();
                            matrizDatos = FormatoHelper.LeerExcelCargado(this.RutaCompletaArchivo, titulos, 2, out matrizValida);

                            if (matrizValida.Exito)
                            {
                                model = FormatearModeloDesdeMatriz(matrizDatos);                              
                            }

                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoMandoSincronizado:
                        {
                            var titulos = ObtenerTitulosColumnasMandoSincronizado();
                            matrizDatos = FormatoHelper.LeerExcelCargadoOtro(this.RutaCompletaArchivo, titulos, 2, out matrizValida);

                            if (matrizValida.Exito)
                            {
                                model = FormatearModeloDesdeMatrizOtro(matrizDatos);
                            }

                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoTorsional:
                        {
                            var titulos = ObtenerTitulosColumnasTorsional();
                            matrizDatos = FormatoHelper.LeerExcelCargadoOtro(this.RutaCompletaArchivo, titulos, 2, out matrizValida);

                            if (matrizValida.Exito)
                            {
                                model = FormatearModeloDesdeMatrizOtro(matrizDatos);
                            }

                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoPmu:
                        {
                            var titulos = ObtenerTitulosColumnasPmu();
                            matrizDatos = FormatoHelper.LeerExcelCargadoOtro(this.RutaCompletaArchivo, titulos, 2, out matrizValida);

                            if (matrizValida.Exito)
                            {
                                model = FormatearModeloDesdeMatrizOtro(matrizDatos);                               
                            }

                            break;
                        }
                }

                if (matrizValida.Exito)
                {                    
                    FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta { Exito = true, Datos = model });
                }
                else
                {
                    return Json(matrizValida);
                }

            }
            catch (Exception ex)
            {
                log.Error("LeerExcelSubido", ex);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult CargarPlantillaVacia(int tipoUsoId)
        {
            try
            {
                var lista = new List<EprCargaMasivaDetalleDTO>();
                string[][] matrizDatos = null;
                FormatoModel model = null;

                switch (tipoUsoId)
                {
                    case ConstantesProteccion.CodigoReleTipoUsoGeneral:
                        {
                            matrizDatos = GenerarData(lista, true);
                            model = FormatearModeloDesdeMatriz(matrizDatos);
                            break;
                        }

                    case ConstantesProteccion.CodigoReleTipoUsoMandoSincronizado:
                        {
                            matrizDatos = GenerarDataMandoSincronizado(lista, true);
                            model = FormatearModeloDesdeMatrizOtro(matrizDatos);
                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoTorsional:
                        {
                            matrizDatos = GenerarDataTorsional(lista, true);
                            model = FormatearModeloDesdeMatrizOtro(matrizDatos);
                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoPmu:
                        {
                            matrizDatos = GenerarDataPmu(lista, true);
                            model = FormatearModeloDesdeMatrizOtro(matrizDatos);
                            break;
                        }
                }                          
                
              
                return Json(new Respuesta { Exito = true, Datos = model });
            }
            catch (Exception ex)
            {
                log.Error("CargarPlantillaVacia", ex);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        private FormatoModel FormatearModeloDesdeMatriz(string[][] matrizDatos)
        {
            FormatoModel model = new FormatoModel();
            ConfigurarFormatoModelo(model);
            model.Handson.ListaExcelData = matrizDatos;
            return model;
        }

        private FormatoModel FormatearModeloDesdeMatrizOtro(string[][] matrizDatos)
        {
            FormatoModel model = new FormatoModel();
            ConfigurarFormatoModeloOtro(model);
            model.Handson.ListaExcelData = matrizDatos;
            return model;
        }

        private void ConfigurarFormatoModelo(FormatoModel model)
        {
            model.Handson = new HandsonModel();
            model.Formato = new MeFormatoDTO();
            model.Handson.ListaMerge = GenerarMerges();
            model.Handson.ListaColWidth = new List<int>();
            model.Formato.Formatrows = 1;
            model.Formato.Formatcols = 6;
            model.FilasCabecera = model.Formato.Formatrows;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.Handson.ListaColWidth = new List<int> { 50,50, 150, 150, 150, 150, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80,80, 80, 80, 80, 80, 80,80, 80, 80, 80, 80, 80,80, 120 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true, true };
        }

        private void ConfigurarFormatoModeloOtro(FormatoModel model)
        {
            model.Handson = new HandsonModel();
            model.Formato = new MeFormatoDTO();
            model.Handson.ListaMerge = GenerarMergesOtro();
            model.Handson.ListaColWidth = new List<int>();
            model.Formato.Formatrows = 1;
            model.Formato.Formatcols = 5;
            model.FilasCabecera = model.Formato.Formatrows;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.Handson.ListaColWidth = new List<int> { 50, 50, 150, 150, 150, 150, 80, 80, 80, 80, 80, 100, 100, 100, 100 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true, true };
        }

        private List<CeldaMerge> GenerarMerges()
        {
            return new List<CeldaMerge>{
               new CeldaMerge{row=0, col=12, colspan=2, rowspan=1},
               new CeldaMerge{row=0, col=14, colspan=2, rowspan=1},
               new CeldaMerge{row=0, col=17, colspan=5, rowspan=1},
               new CeldaMerge{row=0, col=22, colspan=5, rowspan=1},
               new CeldaMerge{row=0, col=27, colspan=2, rowspan=1},
               new CeldaMerge{row=0, col=29, colspan=2, rowspan=1},
           };
        }

        private List<CeldaMerge> GenerarMergesOtro()
        {
            return new List<CeldaMerge>{
               new CeldaMerge{row=0, col=12, colspan=2, rowspan=1},              
           };
        }


        /// <summary>
        /// Carga archivo excel con un nombre temporal en la ruta configurada en el Archivo de Configuración
        /// </summary>
        /// <returns></returns>
        public ActionResult Subir()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var archivo = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.NombreArchivo = fileRandom + "." + NombreArchivoTipoUso.ExtensionFileUploadTipoUso;
                    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.NombreArchivo;
                    this.RutaCompletaArchivo = ruta;
                    archivo.SaveAs(ruta);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Subir", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ValidarDatosArchivo(string datos, int tipoUsoId)
        {
            Respuesta repuestaJson = null;

            switch (tipoUsoId)
            {
                case ConstantesProteccion.CodigoReleTipoUsoGeneral:
                    {
                        var listaRegistros = FormatearDatosArchivo(datos);

                        //Hacer las validaciones de cada registro
                        ValidarRegistrosReleUsoGeneral(listaRegistros.ToList());

                        var registroObservados = 0;
                        if (listaRegistros.Any(p => p.Error.Length > 0))
                        {
                            registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                        }

                        //luego devolver el handsontable
                        var matrizDatos = GenerarData(listaRegistros.ToList(), false);
                        FormatoModel model = FormatearModeloDesdeMatriz(matrizDatos);

                        repuestaJson = new Respuesta { Exito = true, Datos = model, RegistrosObservados = registroObservados };
                        break;
                    }
                case ConstantesProteccion.CodigoReleTipoUsoMandoSincronizado:
                    {
                        var listaRegistros = FormatearDatosArchivoMandoSincronizado(datos);

                        //Hacer las validaciones de cada registro
                        ValidarRegistrosReleMandoSincronizado(listaRegistros.ToList());

                        var registroObservados = 0;
                        if (listaRegistros.Any(p => p.Error.Length > 0))
                        {
                            registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                        }

                        //luego devolver el handsontable
                        var matrizDatos = GenerarDataMandoSincronizado(listaRegistros.ToList(), false);
                        FormatoModel model = FormatearModeloDesdeMatrizOtro(matrizDatos);

                        repuestaJson = new Respuesta { Exito = true, Datos = model, RegistrosObservados = registroObservados };
                        break;
                    }
                case ConstantesProteccion.CodigoReleTipoUsoTorsional:
                    {
                        var listaRegistros = FormatearDatosArchivoTorsional(datos);

                        //Hacer las validaciones de cada registro
                        ValidarRegistrosReleTorsional(listaRegistros.ToList());

                        var registroObservados = 0;
                        if (listaRegistros.Any(p => p.Error.Length > 0))
                        {
                            registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                        }

                        //luego devolver el handsontable
                        var matrizDatos = GenerarDataTorsional(listaRegistros.ToList(), false);
                        FormatoModel model = FormatearModeloDesdeMatrizOtro(matrizDatos);

                        repuestaJson = new Respuesta { Exito = true, Datos = model, RegistrosObservados = registroObservados };
                        break;
                    }
                case ConstantesProteccion.CodigoReleTipoUsoPmu:
                    {
                        var listaRegistros = FormatearDatosArchivoPmu(datos);

                        //Hacer las validaciones de cada registro
                        ValidarRegistrosRelePmu(listaRegistros.ToList());

                        var registroObservados = 0;
                        if (listaRegistros.Any(p => p.Error.Length > 0))
                        {
                            registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                        }

                        //luego devolver el handsontable
                        var matrizDatos = GenerarDataPmu(listaRegistros.ToList(), false);
                        FormatoModel model = FormatearModeloDesdeMatrizOtro(matrizDatos);

                        repuestaJson = new Respuesta { Exito = true, Datos = model, RegistrosObservados = registroObservados };
                        break;
                    }
            }
            
            
            return Json(repuestaJson);


           
        }

        private void ValidarRegistrosReleUsoGeneral(List<EprCargaMasivaDetalleDTO> listaRegistros)
        {
            var nombresRepetidos = listaRegistros
            .GroupBy(e => e.NombreRele + e.CodigoCelda) 
            .Where(g => g.Count() > 1)  
            .Select(g => g.Key) 
            .ToList();            
           
            foreach (var nombre in nombresRepetidos)
            {
                var registrosRepetidos = listaRegistros.Where(e => e.NombreRele + e.CodigoCelda == nombre).ToList();
                                
                foreach (var item in registrosRepetidos)
                {
                    if ((item.NombreRele != null && !item.NombreRele.Equals("")) &&
                        (item.CodigoCelda != null && !item.CodigoCelda.Equals("")))
                    {
                        item.Error = "Código de la Celda y Nombre Relé duplicado. Revisar.";
                    }
                }               
            }

            var codigosRepetidos = listaRegistros
            .GroupBy(e => e.CodigoRele)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

            foreach (var codigo in codigosRepetidos)
            {
                var registrosRepetidos = listaRegistros.Where(e => e.CodigoRele == codigo).ToList();

                foreach (var item in registrosRepetidos)
                {
                    if (item.CodigoRele != null && !item.CodigoRele.Equals(""))
                    {
                        if (item.Error != null && !item.Error.Equals(""))
                        {
                            item.Error = item.Error + "&nbsp;" + "\nCódigo del Relé duplicado. Revisar.";
                        }
                        else
                        {
                            item.Error = "Código del Relé duplicado. Revisar.";
                        }
                    }
                }
            }

            //Si no hay nombre de Rele duplicado se valida todo el registro
            if (nombresRepetidos.Count() == 0 && codigosRepetidos.Count() == 0)
            {
                foreach (var registro in listaRegistros)
                {
                    var res = servicioArea.ValidarProteccionesUsoGeneral(registro);

                    if (!string.IsNullOrEmpty(res)) registro.Error = res;
                }
            }
         
        }

        private void ValidarRegistrosReleMandoSincronizado(List<EprCargaMasivaDetalleDTO> listaRegistros)
        {
            var nombresRepetidos = listaRegistros
           .GroupBy(e => e.NombreRele + e.CodigoCelda)
           .Where(g => g.Count() > 1)
           .Select(g => g.Key)
           .ToList();

            foreach (var nombre in nombresRepetidos)
            {
                var registrosRepetidos = listaRegistros.Where(e => e.NombreRele + e.CodigoCelda == nombre).ToList();

                foreach (var item in registrosRepetidos)
                {
                    if ((item.NombreRele != null && !item.NombreRele.Equals("")) &&
                        (item.CodigoCelda != null && !item.CodigoCelda.Equals("")))
                    {
                        item.Error = "Código de la Celda y Nombre Relé duplicado. Revisar.";
                    }
                }
            }

            var codigosRepetidos = listaRegistros
            .GroupBy(e => e.CodigoRele)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

            foreach (var codigo in codigosRepetidos)
            {
                var registrosRepetidos = listaRegistros.Where(e => e.CodigoRele == codigo).ToList();

                foreach (var item in registrosRepetidos)
                {
                    if (item.CodigoRele != null && !item.CodigoRele.Equals(""))
                    {
                        if (item.Error != null && !item.Error.Equals(""))
                        {
                            item.Error = item.Error + "&nbsp;" + "\nCódigo del Relé duplicado. Revisar.";
                        }
                        else
                        {
                            item.Error = "Código del Relé duplicado. Revisar.";
                        }
                    }
                }
            }

            if (nombresRepetidos.Count() == 0)
            {
                foreach (var registro in listaRegistros)
                {
                    var res = servicioArea.ValidarProteccionesMandoSincronizado(registro);

                    if (!string.IsNullOrEmpty(res)) registro.Error = res;
                }
            }
                
        }

        private void ValidarRegistrosReleTorsional(List<EprCargaMasivaDetalleDTO> listaRegistros)
        {

            var nombresRepetidos = listaRegistros
           .GroupBy(e => e.NombreRele + e.CodigoCelda)
           .Where(g => g.Count() > 1)
           .Select(g => g.Key)
           .ToList();

            foreach (var nombre in nombresRepetidos)
            {
                var registrosRepetidos = listaRegistros.Where(e => e.NombreRele + e.CodigoCelda == nombre).ToList();

                foreach (var item in registrosRepetidos)
                {
                    if ((item.NombreRele != null && !item.NombreRele.Equals("")) &&
                        (item.CodigoCelda != null && !item.CodigoCelda.Equals("")))
                    {
                        item.Error = "Código de la Celda y Nombre Relé duplicado. Revisar.";
                    }
                }
            }

            var codigosRepetidos = listaRegistros
            .GroupBy(e => e.CodigoRele)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

            foreach (var codigo in codigosRepetidos)
            {
                var registrosRepetidos = listaRegistros.Where(e => e.CodigoRele == codigo).ToList();

                foreach (var item in registrosRepetidos)
                {
                    if (item.CodigoRele != null && !item.CodigoRele.Equals(""))
                    {
                        if (item.Error != null && !item.Error.Equals(""))
                        {
                            item.Error = item.Error + "&nbsp;" + "\nCódigo del Relé duplicado. Revisar.";
                        }
                        else
                        {
                            item.Error = "Código del Relé duplicado. Revisar.";
                        }
                    }
                }
            }

            if (nombresRepetidos.Count() == 0)
            {
                foreach (var registro in listaRegistros)
                {
                    var res = servicioArea.ValidarProteccionesTorsional(registro);

                    if (!string.IsNullOrEmpty(res)) registro.Error = res;
                }
            }
           
        }

        private void ValidarRegistrosRelePmu(List<EprCargaMasivaDetalleDTO> listaRegistros)
        {
            var nombresRepetidos = listaRegistros
           .GroupBy(e => e.NombreRele + e.CodigoCelda)
           .Where(g => g.Count() > 1)
           .Select(g => g.Key)
           .ToList();

            foreach (var nombre in nombresRepetidos)
            {
                var registrosRepetidos = listaRegistros.Where(e => e.NombreRele + e.CodigoCelda == nombre).ToList();

                foreach (var item in registrosRepetidos)
                {
                    if ((item.NombreRele != null && !item.NombreRele.Equals("")) &&
                        (item.CodigoCelda != null && !item.CodigoCelda.Equals("")))
                    {
                        item.Error = "Código de la Celda y Nombre Relé duplicado. Revisar.";
                    }
                }
            }

            var codigosRepetidos = listaRegistros
            .GroupBy(e => e.CodigoRele)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

            foreach (var codigo in codigosRepetidos)
            {
                var registrosRepetidos = listaRegistros.Where(e => e.CodigoRele == codigo).ToList();

                foreach (var item in registrosRepetidos)
                {
                    if (item.CodigoRele != null && !item.CodigoRele.Equals(""))
                    {
                        if (item.Error != null && !item.Error.Equals(""))
                        {
                            item.Error = item.Error + "&nbsp;" + "\nCódigo del Relé duplicado. Revisar.";
                        }
                        else
                        {
                            item.Error = "Código del Relé duplicado. Revisar.";
                        }
                    }   
                }
            }

            if (nombresRepetidos.Count() == 0)
            {
                foreach (var registro in listaRegistros)
                {
                    var res = servicioArea.ValidarProteccionesPmu(registro);

                    if (!string.IsNullOrEmpty(res)) registro.Error = res;
                }
            }
          
        }


        private void GrabarRegistrosRele(List<EprCargaMasivaDetalleDTO> listaRegistros, int tipoUsoId)
        {

            switch (tipoUsoId)
            {
                case ConstantesProteccion.CodigoReleTipoUsoGeneral:
                    {
                        foreach (var registro in listaRegistros)
                        {
                            registro.NombreUsuario = User.Identity.Name;

                            var res = servicioArea.SaveProteccionesUsoGeneral(registro);

                            if (!string.IsNullOrEmpty(res)) registro.Error = res;
                        }
                        break;
                    }
                case ConstantesProteccion.CodigoReleTipoUsoMandoSincronizado:
                    {
                        foreach (var registro in listaRegistros)
                        {
                            registro.NombreUsuario = User.Identity.Name;

                            var res = servicioArea.SaveProteccionesMandoSincronizado(registro);

                            if (!string.IsNullOrEmpty(res)) registro.Error = res;
                        }
                        break;
                    }
                case ConstantesProteccion.CodigoReleTipoUsoTorsional:
                    {
                        foreach (var registro in listaRegistros)
                        {
                            registro.NombreUsuario = User.Identity.Name;

                            var res = servicioArea.SaveProteccionesTorsional(registro);

                            if (!string.IsNullOrEmpty(res)) registro.Error = res;
                        }
                        break;
                    }
                case ConstantesProteccion.CodigoReleTipoUsoPmu:
                    {
                        foreach (var registro in listaRegistros)
                        {
                            registro.NombreUsuario = User.Identity.Name;

                            var res = servicioArea.SaveProteccionesPmu(registro);

                            if (!string.IsNullOrEmpty(res)) registro.Error = res;
                        }
                        break;
                    }
            }
                       
            //Grabar Registro Log

            var registroLog = new EprCargaMasivaDTO();

            registroLog.Epcamatipuso = tipoUsoId;
            registroLog.Epcamafeccarga = DateTime.Now;
            registroLog.Epcamatotalregistro = listaRegistros.Count;
            registroLog.Epcamausucreacion = User.Identity.Name;

            servicioArea.SaveCargaMasiva(registroLog);
        }

        public JsonResult GrabarDatosArchivo(string datos, int tipoUsoId)
        {
            base.ValidarSesionJsonResult();

            try
            {
                var listaRegistros = new List<EprCargaMasivaDetalleDTO>();
                var registroObservados = 0;
                string[][] matrizDatos = null;
                FormatoModel model = null;

                switch (tipoUsoId)
                {
                    case ConstantesProteccion.CodigoReleTipoUsoGeneral:
                        {
                            listaRegistros = FormatearDatosArchivo(datos).ToList();

                            //Hacer las validaciones de cada registro
                            ValidarRegistrosReleUsoGeneral(listaRegistros.ToList());
                           
                            if (listaRegistros.Any(p => p.Error.Length > 0))
                            {
                                registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                            }

                            //luego devolver el handsontable
                            matrizDatos = GenerarData(listaRegistros.ToList(), false);
                            model = FormatearModeloDesdeMatriz(matrizDatos);

                            
                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoMandoSincronizado:
                        {
                            listaRegistros = FormatearDatosArchivoMandoSincronizado(datos).ToList();

                            //Hacer las validaciones de cada registro
                            ValidarRegistrosReleMandoSincronizado(listaRegistros.ToList());
                           
                            if (listaRegistros.Any(p => p.Error.Length > 0))
                            {
                                registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                            }

                            //luego devolver el handsontable
                            matrizDatos = GenerarDataMandoSincronizado(listaRegistros.ToList(), false);
                            model = FormatearModeloDesdeMatrizOtro(matrizDatos);
                           
                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoTorsional:
                        {
                            listaRegistros = FormatearDatosArchivoTorsional(datos).ToList();

                            //Hacer las validaciones de cada registro
                            ValidarRegistrosReleTorsional(listaRegistros.ToList());
                           
                            if (listaRegistros.Any(p => p.Error.Length > 0))
                            {
                                registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                            }

                            //luego devolver el handsontable
                            matrizDatos = GenerarDataTorsional(listaRegistros.ToList(), false);
                            model = FormatearModeloDesdeMatrizOtro(matrizDatos);
                            
                            break;
                        }
                    case ConstantesProteccion.CodigoReleTipoUsoPmu:
                        {
                            listaRegistros = FormatearDatosArchivoPmu(datos).ToList();

                            //Hacer las validaciones de cada registro
                            ValidarRegistrosRelePmu(listaRegistros.ToList());
                           
                            if (listaRegistros.Any(p => p.Error.Length > 0))
                            {
                                registroObservados = listaRegistros.Count(p => p.Error.Length > 0);
                            }

                            //luego devolver el handsontable
                            matrizDatos = GenerarDataPmu(listaRegistros.ToList(), false);
                            model = FormatearModeloDesdeMatrizOtro(matrizDatos);
                            
                            break;
                        }
                }
                              
                var exito = registroObservados == 0;

                if (exito && listaRegistros.Count() > 0)
                {
                    GrabarRegistrosRele(listaRegistros, tipoUsoId);

                    return Json(new Respuesta { Exito = exito, Datos = null });
                }
                else
                {
                    var mensaje = listaRegistros.Count() == 0 ? "No hay registros para grabar" : "";

                    return Json(new Respuesta { Exito = false, Datos = model, Mensaje= mensaje });
                }
            }
            catch(Exception ex)
            {
                log.Error("GrabarDatosArchivo", ex);
                return Json(new Respuesta { Exito = false, Datos = null });
            }
        }

        private ICollection<EprCargaMasivaDetalleDTO> FormatearDatosArchivo(string datos)
        {
            var parametros = new List<EprCargaMasivaDetalleDTO>();
            if (datos != null && !datos.Equals(""))
            {
                var listaJSON = JsonConvert.DeserializeObject<List<List<string>>>(datos);
                for (int i = 2; i < listaJSON.Count; i++)
                {
                    List<string> lColumnas = listaJSON[i];

                    var parametro = new EprCargaMasivaDetalleDTO();
                    parametro.Error = "";
                    parametro.Item = lColumnas[1].Replace(@"\", "").Replace("\"", "").Replace("[", "");
                    parametro.CodigoEmpresa = lColumnas[2].Replace(@"\", "").Replace("\"", "");
                    parametro.CodigoCelda = lColumnas[3].Replace(@"\", "").Replace("\"", "");
                    parametro.CodigoRele = Convert.ToString(lColumnas[4].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));
                    parametro.NombreRele = lColumnas[5].Replace(@"\", "").Replace("\"", "");
                    parametro.Fecha = lColumnas[6].Replace(@"\", "").Replace("\"", "");
                    parametro.Estado = Convert.ToString(lColumnas[7].Replace(@"\", "").Replace("\"", ""));

                    parametro.NivelTension = Convert.ToString(lColumnas[8].Replace(@"\", "").Replace("\"", ""));
                    parametro.SistemaRele = Convert.ToString(lColumnas[9].Replace(@"\", "").Replace("\"", ""));
                    parametro.Fabricante = Convert.ToString(lColumnas[10].Replace(@"\", "").Replace("\"", ""));
                    parametro.Modelo = Convert.ToString(lColumnas[11].Replace(@"\", "").Replace("\"", ""));
                    parametro.RTC_Ip = Convert.ToString(lColumnas[12].Replace(@"\", "").Replace("\"", ""));
                    parametro.RTC_Is = Convert.ToString(lColumnas[13].Replace(@"\", "").Replace("\"", ""));
                    parametro.RTT_Vp = Convert.ToString(lColumnas[14].Replace(@"\", "").Replace("\"", ""));
                    parametro.RTT_Vs = Convert.ToString(lColumnas[15].Replace(@"\", "").Replace("\"", ""));
                    parametro.ProteccionesCoordinables = Convert.ToString(lColumnas[16].Replace(@"\", "").Replace("\"", ""));
                    parametro.SincActivo = Convert.ToString(lColumnas[17].Replace(@"\", "").Replace("\"", ""));
                    parametro.SincInterruptor = Convert.ToString(lColumnas[18].Replace(@"\", "").Replace("\"", ""));
                    parametro.SincTension = Convert.ToString(lColumnas[19].Replace(@"\", "").Replace("\"", ""));
                    parametro.SincAngulo = Convert.ToString(lColumnas[20].Replace(@"\", "").Replace("\"", ""));
                    parametro.SincFrecuencia = Convert.ToString(lColumnas[21].Replace(@"\", "").Replace("\"", ""));
                    parametro.SobretActivo = Convert.ToString(lColumnas[22].Replace(@"\", "").Replace("\"", ""));
                    parametro.SobretInterruptor = Convert.ToString(lColumnas[23].Replace(@"\", "").Replace("\"", ""));
                    parametro.SobretTension = Convert.ToString(lColumnas[24].Replace(@"\", "").Replace("\"", ""));
                    parametro.SobretAngulo = Convert.ToString(lColumnas[25].Replace(@"\", "").Replace("\"", ""));
                    parametro.SobretFrecuencia = Convert.ToString(lColumnas[26].Replace(@"\", "").Replace("\"", ""));
                    parametro.SobreCorrienteActivo = Convert.ToString(lColumnas[27].Replace(@"\", "").Replace("\"", ""));
                    parametro.SobreCorrienteActivoDelta = Convert.ToString(lColumnas[28].Replace(@"\", "").Replace("\"", ""));
                    parametro.PmuActivo = Convert.ToString(lColumnas[29].Replace(@"\", "").Replace("\"", ""));
                    parametro.PmuAccion = Convert.ToString(lColumnas[30].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                    parametro.Proyecto = Convert.ToString(lColumnas[31].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                    var filaVacia = false;

                    if (string.IsNullOrEmpty(parametro.Item) && string.IsNullOrEmpty(parametro.CodigoEmpresa) &&
                    string.IsNullOrEmpty(parametro.CodigoCelda) && string.IsNullOrEmpty(parametro.CodigoRele) &&
                    string.IsNullOrEmpty(parametro.NombreRele) && string.IsNullOrEmpty(parametro.Fecha) && 
                    string.IsNullOrEmpty(parametro.Estado) &&
                    string.IsNullOrEmpty(parametro.NivelTension) && string.IsNullOrEmpty(parametro.SistemaRele) &&
                    string.IsNullOrEmpty(parametro.Fabricante) && string.IsNullOrEmpty(parametro.Modelo) &&
                    string.IsNullOrEmpty(parametro.RTC_Ip) && string.IsNullOrEmpty(parametro.RTC_Is) &&
                    string.IsNullOrEmpty(parametro.RTT_Vp) && string.IsNullOrEmpty(parametro.RTT_Vs) &&
                    string.IsNullOrEmpty(parametro.ProteccionesCoordinables) && string.IsNullOrEmpty(parametro.SincActivo) &&
                    string.IsNullOrEmpty(parametro.SincInterruptor) && string.IsNullOrEmpty(parametro.SincTension) &&
                    string.IsNullOrEmpty(parametro.SincAngulo) && string.IsNullOrEmpty(parametro.SincFrecuencia) &&
                    string.IsNullOrEmpty(parametro.SobretActivo) && string.IsNullOrEmpty(parametro.SobretInterruptor) &&
                    string.IsNullOrEmpty(parametro.SobretTension) && string.IsNullOrEmpty(parametro.SobretAngulo) &&
                    string.IsNullOrEmpty(parametro.SobretFrecuencia) && string.IsNullOrEmpty(parametro.SobreCorrienteActivo) &&
                    string.IsNullOrEmpty(parametro.SobreCorrienteActivoDelta) && string.IsNullOrEmpty(parametro.PmuActivo) &&
                    string.IsNullOrEmpty(parametro.PmuAccion) && string.IsNullOrEmpty(parametro.Proyecto) ) {
                        filaVacia = true;
                    }

                    if (!filaVacia)
                    {
                        parametros.Add(parametro);
                    }
                }
            }
            
            return parametros;
        }

        private ICollection<EprCargaMasivaDetalleDTO> FormatearDatosArchivoMandoSincronizado(string datos)
        {
            var parametros = new List<EprCargaMasivaDetalleDTO>();
            if (datos != null && !datos.Equals(""))
            {
                var listaJSON = JsonConvert.DeserializeObject<List<List<string>>>(datos);
                for (int i = 2; i < listaJSON.Count; i++)
                {
                    List<string> lColumnas = listaJSON[i];

                    var parametro = new EprCargaMasivaDetalleDTO();

                    parametro.Error = "";
                    parametro.Item = lColumnas[1].Replace(@"\", "").Replace("\"", "").Replace("[", "");
                    parametro.CodigoEmpresa = lColumnas[2].Replace(@"\", "").Replace("\"", "");
                    parametro.CodigoCelda = lColumnas[3].Replace(@"\", "").Replace("\"", "");
                    parametro.CodigoRele = Convert.ToString(lColumnas[4].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));
                    parametro.NombreRele = lColumnas[5].Replace(@"\", "").Replace("\"", "");
                    parametro.Fecha = lColumnas[6].Replace(@"\", "").Replace("\"", "");
                    parametro.Estado = Convert.ToString(lColumnas[7].Replace(@"\", "").Replace("\"", ""));
                    parametro.NivelTension = Convert.ToString(lColumnas[8].Replace(@"\", "").Replace("\"", ""));
                    parametro.SistemaRele = Convert.ToString(lColumnas[9].Replace(@"\", "").Replace("\"", ""));
                    parametro.Fabricante = Convert.ToString(lColumnas[10].Replace(@"\", "").Replace("\"", ""));
                    parametro.Modelo = Convert.ToString(lColumnas[11].Replace(@"\", "").Replace("\"", ""));

                    parametro.InterruptorComanda = Convert.ToString(lColumnas[12].Replace(@"\", "").Replace("\"", ""));
                    parametro.Mando = Convert.ToString(lColumnas[13].Replace(@"\", "").Replace("\"", ""));

                    parametro.Proyecto = Convert.ToString(lColumnas[14].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                    var filaVacia = false;
                    if (string.IsNullOrEmpty(parametro.Item) && string.IsNullOrEmpty(parametro.CodigoEmpresa) &&
                    string.IsNullOrEmpty(parametro.CodigoCelda) && string.IsNullOrEmpty(parametro.CodigoRele) &&
                    string.IsNullOrEmpty(parametro.NombreRele) && string.IsNullOrEmpty(parametro.Fecha) &&
                    string.IsNullOrEmpty(parametro.Estado) && string.IsNullOrEmpty(parametro.NivelTension) &&
                    string.IsNullOrEmpty(parametro.SistemaRele) && string.IsNullOrEmpty(parametro.Fabricante) &&
                    string.IsNullOrEmpty(parametro.Modelo) && string.IsNullOrEmpty(parametro.InterruptorComanda) &&
                    string.IsNullOrEmpty(parametro.Mando) && string.IsNullOrEmpty(parametro.Proyecto)) {
                        filaVacia = true;
                    }

                    if (!filaVacia)
                    {
                        parametros.Add(parametro);
                    }
                }
            }

            return parametros;
        }

        private ICollection<EprCargaMasivaDetalleDTO> FormatearDatosArchivoTorsional(string datos)
        {
            var parametros = new List<EprCargaMasivaDetalleDTO>();
            if (datos != null && !datos.Equals(""))
            {
                var listaJSON = JsonConvert.DeserializeObject<List<List<string>>>(datos);
                for (int i = 2; i < listaJSON.Count; i++)
                {
                    List<string> lColumnas = listaJSON[i];

                    var parametro = new EprCargaMasivaDetalleDTO();

                    parametro.Error = "";
                    parametro.Item = lColumnas[1].Replace(@"\", "").Replace("\"", "").Replace("[", "");
                    parametro.CodigoEmpresa = lColumnas[2].Replace(@"\", "").Replace("\"", "");
                    parametro.CodigoCelda = lColumnas[3].Replace(@"\", "").Replace("\"", "");
                    parametro.CodigoRele = Convert.ToString(lColumnas[4].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));
                    parametro.NombreRele = lColumnas[5].Replace(@"\", "").Replace("\"", "");
                    parametro.Fecha = lColumnas[6].Replace(@"\", "").Replace("\"", "");
                    parametro.Estado = Convert.ToString(lColumnas[7].Replace(@"\", "").Replace("\"", ""));
                    parametro.NivelTension = Convert.ToString(lColumnas[8].Replace(@"\", "").Replace("\"", ""));
                    parametro.SistemaRele = Convert.ToString(lColumnas[9].Replace(@"\", "").Replace("\"", ""));
                    parametro.Fabricante = Convert.ToString(lColumnas[10].Replace(@"\", "").Replace("\"", ""));
                    parametro.Modelo = Convert.ToString(lColumnas[11].Replace(@"\", "").Replace("\"", ""));

                    parametro.MedidaMitigacion = Convert.ToString(lColumnas[12].Replace(@"\", "").Replace("\"", ""));
                    parametro.Implementado = Convert.ToString(lColumnas[13].Replace(@"\", "").Replace("\"", ""));

                    parametro.Proyecto = Convert.ToString(lColumnas[14].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                    var filaVacia = false;
                    if (string.IsNullOrEmpty(parametro.Item) && string.IsNullOrEmpty(parametro.CodigoEmpresa) &&
                    string.IsNullOrEmpty(parametro.CodigoCelda) && string.IsNullOrEmpty(parametro.CodigoRele) &&
                    string.IsNullOrEmpty(parametro.NombreRele) && string.IsNullOrEmpty(parametro.Fecha) &&
                    string.IsNullOrEmpty(parametro.Estado) && string.IsNullOrEmpty(parametro.NivelTension) &&
                    string.IsNullOrEmpty(parametro.SistemaRele) && string.IsNullOrEmpty(parametro.Fabricante) &&
                    string.IsNullOrEmpty(parametro.Modelo) && string.IsNullOrEmpty(parametro.MedidaMitigacion) &&
                    string.IsNullOrEmpty(parametro.Implementado) && string.IsNullOrEmpty(parametro.Proyecto)) {
                        filaVacia = true;
                    }

                    if (!filaVacia)
                    {
                        parametros.Add(parametro);
                    }
                }
            }

            return parametros;
        }


        private ICollection<EprCargaMasivaDetalleDTO> FormatearDatosArchivoPmu(string datos)
        {
            var parametros = new List<EprCargaMasivaDetalleDTO>();
            if (datos != null && !datos.Equals(""))
            {
                var listaJSON = JsonConvert.DeserializeObject<List<List<string>>>(datos);
                for (int i = 2; i < listaJSON.Count; i++)
                {
                    List<string> lColumnas = listaJSON[i];

                    var parametro = new EprCargaMasivaDetalleDTO();
                    parametro.Error = "";
                    parametro.Item = lColumnas[1].Replace(@"\", "").Replace("\"", "").Replace("[", "");
                    parametro.CodigoEmpresa = lColumnas[2].Replace(@"\", "").Replace("\"", "");
                    parametro.CodigoCelda = lColumnas[3].Replace(@"\", "").Replace("\"", "");
                    parametro.CodigoRele = Convert.ToString(lColumnas[4].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));
                    parametro.NombreRele = lColumnas[5].Replace(@"\", "").Replace("\"", "");
                    parametro.Fecha = lColumnas[6].Replace(@"\", "").Replace("\"", "");
                    parametro.Estado = Convert.ToString(lColumnas[7].Replace(@"\", "").Replace("\"", ""));
                    parametro.NivelTension = Convert.ToString(lColumnas[8].Replace(@"\", "").Replace("\"", ""));
                    parametro.SistemaRele = Convert.ToString(lColumnas[9].Replace(@"\", "").Replace("\"", ""));
                    parametro.Fabricante = Convert.ToString(lColumnas[10].Replace(@"\", "").Replace("\"", ""));
                    parametro.Modelo = Convert.ToString(lColumnas[11].Replace(@"\", "").Replace("\"", ""));

                    parametro.PmuAccion = Convert.ToString(lColumnas[12].Replace(@"\", "").Replace("\"", ""));
                    parametro.Implementado = Convert.ToString(lColumnas[13].Replace(@"\", "").Replace("\"", ""));

                    parametro.Proyecto = Convert.ToString(lColumnas[14].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                    var filaVacia = false;
                    if (string.IsNullOrEmpty(parametro.Item) &&  string.IsNullOrEmpty(parametro.CodigoEmpresa) &&
                    string.IsNullOrEmpty(parametro.CodigoCelda) && string.IsNullOrEmpty(parametro.CodigoRele) &&
                    string.IsNullOrEmpty(parametro.NombreRele) && string.IsNullOrEmpty(parametro.Fecha) &&
                    string.IsNullOrEmpty(parametro.Estado) && string.IsNullOrEmpty(parametro.NivelTension) &&
                    string.IsNullOrEmpty(parametro.SistemaRele) && string.IsNullOrEmpty(parametro.Fabricante) &&
                    string.IsNullOrEmpty(parametro.Modelo) && string.IsNullOrEmpty(parametro.PmuAccion) &&
                    string.IsNullOrEmpty(parametro.Implementado) && string.IsNullOrEmpty(parametro.Proyecto) ) {
                        filaVacia = true;
                    }

                    if (!filaVacia)
                    {
                        parametros.Add(parametro);
                    }
                }
            }

            return parametros;
        }
        /// <summary>
        /// Devuelve la lista de entidades en una matriz
        /// </summary>
        /// <param name="registros"></param>
        /// <returns></returns>
        private string[][] GenerarData(List<EprCargaMasivaDetalleDTO> registros, bool plantillaVacia)
        {
            var filas = registros.Count + 2;
            if (plantillaVacia)
            {
                filas += 10;
            }
            var cabecera = ObtenerTitulosColumnas();
            var columnas = cabecera.Count;
            string[][] matriz = new string[filas][];

            for (var i = 0; i < filas; i++)
            {
                matriz[i] = new string[columnas];
                if (i == 0)
                {
                    matriz[i][12] = "Relación de Transformación de corriente (RTC)";
                    matriz[i][14] = "Relación de Transformación de tensión (RTT)";
                    matriz[i][17] = "Ajuste de sincronismo";
                    matriz[i][22] = "Ajuste de sobretensión";
                    matriz[i][27] = "Umbral de sobrecorrientes";
                    matriz[i][29] = "Ajuste PMU";
                }              
                if (i == 1)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        matriz[i][j] = cabecera[j];
                    }
                }
                if (!plantillaVacia && i > 1)
                {
                    matriz[i][0] = registros[i - 2].Error ?? "";
                    matriz[i][1] = registros[i - 2].Item;
                    matriz[i][2] = registros[i - 2].CodigoEmpresa;
                    matriz[i][3] = registros[i - 2].CodigoCelda;
                    matriz[i][4] = registros[i - 2].CodigoRele.ToString();
                    matriz[i][5] = registros[i - 2].NombreRele;
                    matriz[i][6] = registros[i - 2].Fecha.ToString();
                    matriz[i][7] = registros[i - 2].Estado.ToString();
                    matriz[i][8] = registros[i - 2].NivelTension.ToString();
                    matriz[i][9] = registros[i - 2].SistemaRele.ToString();
                    matriz[i][10] = registros[i - 2].Fabricante.ToString();
                    matriz[i][11] = registros[i - 2].Modelo.ToString();

                    matriz[i][12] = registros[i - 2].RTC_Ip.ToString();
                    matriz[i][13] = registros[i - 2].RTC_Is;
                    matriz[i][14] = registros[i - 2].RTT_Vp.ToString();
                    matriz[i][15] = registros[i - 2].RTT_Vs.ToString();
                    matriz[i][16] = registros[i - 2].ProteccionesCoordinables.ToString();
                    matriz[i][17] = registros[i - 2].SincActivo.ToString();
                    matriz[i][18] = registros[i - 2].SincInterruptor.ToString();
                    matriz[i][19] = registros[i - 2].SincTension.ToString();
                    matriz[i][20] = registros[i - 2].SincAngulo.ToString();
                    matriz[i][21] = registros[i - 2].SincFrecuencia.ToString();

                    matriz[i][22] = registros[i - 2].SobretActivo.ToString();
                    matriz[i][23] = registros[i - 2].SobretInterruptor.ToString();
                    matriz[i][24] = registros[i - 2].SobretTension.ToString();
                    matriz[i][25] = registros[i - 2].SobretAngulo.ToString();
                    matriz[i][26] = registros[i - 2].SobretFrecuencia.ToString();
                    matriz[i][27] = registros[i - 2].SobreCorrienteActivo.ToString();
                    matriz[i][28] = registros[i - 2].SobreCorrienteActivoDelta.ToString();
                    matriz[i][29] = registros[i - 2].PmuActivo.ToString();
                    matriz[i][30] = registros[i - 2].PmuAccion.ToString();
                    matriz[i][31] = registros[i - 2].Proyecto.ToString();
                }
                else if(i > 1)
                {
                    matriz[i][0] = "";
                    matriz[i][1] = "";
                    matriz[i][2] = "";
                    matriz[i][3] = "";
                    matriz[i][4] = "";
                    matriz[i][5] = "";
                    matriz[i][6] = "";
                    matriz[i][7] = "";
                    matriz[i][8] = "";
                    matriz[i][9] = "";

                    matriz[i][10] = "";
                    matriz[i][11] = "";
                    matriz[i][12] = "";
                    matriz[i][13] = "";
                    matriz[i][14] = "";
                    matriz[i][15] = "";
                    matriz[i][16] = "";
                    matriz[i][17] = "";
                    matriz[i][18] = "";
                    matriz[i][19] = "";

                    matriz[i][20] = "";
                    matriz[i][21] = "";
                    matriz[i][22] = "";
                    matriz[i][23] = "";
                    matriz[i][24] = "";
                    matriz[i][25] = "";
                    matriz[i][26] = "";
                    matriz[i][27] = "";
                    matriz[i][28] = "";
                    matriz[i][29] = "";
                    matriz[i][30] = "";
                    matriz[i][31] = "";
                }
            }
            return matriz;
        }

        private string[][] GenerarDataMandoSincronizado(List<EprCargaMasivaDetalleDTO> registros, bool plantillaVacia)
        {
            var filas = registros.Count + 2;
            if (plantillaVacia)
            {
                filas += 10;
            }
            var cabecera = ObtenerTitulosColumnasMandoSincronizado();
            var columnas = cabecera.Count;
            string[][] matriz = new string[filas][];

            for (var i = 0; i < filas; i++)
            {
                matriz[i] = new string[columnas];
                if (i == 0)
                {
                    matriz[i][12] = "Mando Sincronizado";                  
                }
                if (i == 1)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        matriz[i][j] = cabecera[j];
                    }
                }
                if (!plantillaVacia && i > 1)
                {
                    matriz[i][0] = registros[i - 2].Error ?? "";
                    matriz[i][1] = registros[i - 2].Item;
                    matriz[i][2] = registros[i - 2].CodigoEmpresa;
                    matriz[i][3] = registros[i - 2].CodigoCelda;
                    matriz[i][4] = registros[i - 2].CodigoRele.ToString();
                    matriz[i][5] = registros[i - 2].NombreRele;
                    matriz[i][6] = registros[i - 2].Fecha.ToString();
                    matriz[i][7] = registros[i - 2].Estado.ToString();
                    matriz[i][8] = registros[i - 2].NivelTension.ToString();
                    matriz[i][9] = registros[i - 2].SistemaRele.ToString();
                    matriz[i][10] = registros[i - 2].Fabricante.ToString();
                    matriz[i][11] = registros[i - 2].Modelo.ToString();

                    matriz[i][12] = registros[i - 2].InterruptorComanda.ToString();
                    matriz[i][13] = registros[i - 2].Mando;                   
                    matriz[i][14] = registros[i - 2].Proyecto.ToString();
                }
                else if (i > 1)
                {
                    matriz[i][0] = "";
                    matriz[i][1] = "";
                    matriz[i][2] = "";
                    matriz[i][3] = "";
                    matriz[i][4] = "";
                    matriz[i][5] = "";
                    matriz[i][6] = "";
                    matriz[i][7] = "";
                    matriz[i][8] = "";
                    matriz[i][9] = "";

                    matriz[i][10] = "";
                    matriz[i][11] = "";
                    matriz[i][12] = "";
                    matriz[i][13] = "";
                    matriz[i][14] = "";                  
                }
            }
            return matriz;
        }

        private string[][] GenerarDataTorsional(List<EprCargaMasivaDetalleDTO> registros, bool plantillaVacia)
        {
            var filas = registros.Count + 2;
            if (plantillaVacia)
            {
                filas += 10;
            }
            var cabecera = ObtenerTitulosColumnasTorsional();
            var columnas = cabecera.Count;
            string[][] matriz = new string[filas][];

            for (var i = 0; i < filas; i++)
            {
                matriz[i] = new string[columnas];
                if (i == 0)
                {
                    matriz[i][12] = "Rele Torsional";
                }
                if (i == 1)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        matriz[i][j] = cabecera[j];
                    }
                }
                if (!plantillaVacia && i > 1)
                {
                    matriz[i][0] = registros[i - 2].Error ?? "";
                    matriz[i][1] = registros[i - 2].Item;
                    matriz[i][2] = registros[i - 2].CodigoEmpresa;
                    matriz[i][3] = registros[i - 2].CodigoCelda;
                    matriz[i][4] = registros[i - 2].CodigoRele.ToString();
                    matriz[i][5] = registros[i - 2].NombreRele;
                    matriz[i][6] = registros[i - 2].Fecha.ToString();
                    matriz[i][7] = registros[i - 2].Estado.ToString();
                    matriz[i][8] = registros[i - 2].NivelTension.ToString();
                    matriz[i][9] = registros[i - 2].SistemaRele.ToString();
                    matriz[i][10] = registros[i - 2].Fabricante.ToString();
                    matriz[i][11] = registros[i - 2].Modelo.ToString();

                    matriz[i][12] = registros[i - 2].MedidaMitigacion.ToString();
                    matriz[i][13] = registros[i - 2].Implementado;
                    matriz[i][14] = registros[i - 2].Proyecto.ToString();
                }
                else if (i > 1)
                {
                    matriz[i][0] = "";
                    matriz[i][1] = "";
                    matriz[i][2] = "";
                    matriz[i][3] = "";
                    matriz[i][4] = "";
                    matriz[i][5] = "";
                    matriz[i][6] = "";
                    matriz[i][7] = "";
                    matriz[i][8] = "";
                    matriz[i][9] = "";

                    matriz[i][10] = "";
                    matriz[i][11] = "";
                    matriz[i][12] = "";
                    matriz[i][13] = "";
                    matriz[i][14] = "";
                }
            }
            return matriz;
        }

        private string[][] GenerarDataPmu(List<EprCargaMasivaDetalleDTO> registros, bool plantillaVacia)
        {
            var filas = registros.Count + 2;
            if (plantillaVacia)
            {
                filas += 10;
            }
            var cabecera = ObtenerTitulosColumnasPmu();
            var columnas = cabecera.Count;
            string[][] matriz = new string[filas][];

            for (var i = 0; i < filas; i++)
            {
                matriz[i] = new string[columnas];
                if (i == 0)
                {
                    matriz[i][12] = "Rele PMU";
                }
                if (i == 1)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        matriz[i][j] = cabecera[j];
                    }
                }
                if (!plantillaVacia && i > 1)
                {
                    matriz[i][0] = registros[i - 2].Error ?? "";
                    matriz[i][1] = registros[i - 2].Item;
                    matriz[i][2] = registros[i - 2].CodigoEmpresa;
                    matriz[i][3] = registros[i - 2].CodigoCelda;
                    matriz[i][4] = registros[i - 2].CodigoRele.ToString();
                    matriz[i][5] = registros[i - 2].NombreRele;
                    matriz[i][6] = registros[i - 2].Fecha.ToString();
                    matriz[i][7] = registros[i - 2].Estado.ToString();
                    matriz[i][8] = registros[i - 2].NivelTension.ToString();
                    matriz[i][9] = registros[i - 2].SistemaRele.ToString();
                    matriz[i][10] = registros[i - 2].Fabricante.ToString();
                    matriz[i][11] = registros[i - 2].Modelo.ToString();

                    matriz[i][12] = registros[i - 2].PmuAccion.ToString();
                    matriz[i][13] = registros[i - 2].Implementado;
                    matriz[i][14] = registros[i - 2].Proyecto.ToString();
                }
                else if (i > 1)
                {
                    matriz[i][0] = "";
                    matriz[i][1] = "";
                    matriz[i][2] = "";
                    matriz[i][3] = "";
                    matriz[i][4] = "";
                    matriz[i][5] = "";
                    matriz[i][6] = "";
                    matriz[i][7] = "";
                    matriz[i][8] = "";
                    matriz[i][9] = "";

                    matriz[i][10] = "";
                    matriz[i][11] = "";
                    matriz[i][12] = "";
                    matriz[i][13] = "";
                    matriz[i][14] = "";
                }
            }
            return matriz;
        }

        #endregion

    }
}
