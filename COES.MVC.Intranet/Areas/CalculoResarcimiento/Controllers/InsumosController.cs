using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Helper;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class InsumosController : BaseController
    {
        /// <summary>
        /// Instancia de clase de servicios
        /// </summary>
        CalculoResarcimientoAppServicio servicio = new CalculoResarcimientoAppServicio();

        /// <summary>
        /// Muestra la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            InsumosModel model = new InsumosModel();
            model.Anio = DateTime.Now.Year;
            model.ListaPeriodo = this.servicio.ObtenerPeriodosSemestralesSinRevision(model.Anio);
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return View(model);
        }

        /// <summary>
        /// Permite obtener los periodos semestrales por anio
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPeriodos(int anio)
        {
            return Json(this.servicio.ObtenerPeriodosSemestralesSinRevision(anio));
        }

        /// <summary>
        /// Permite obtener el listado de puntos de entrega
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoPtoEntrega(int periodo)
        {
            InsumosModel model = new InsumosModel();
            model.ListaPtoEntrega = this.servicio.ObtenerPtoEntregaPorPeriodo(periodo);
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }


        /// <summary>
        /// Permite exportar los ingresos por transmisión a Excel
        /// </summary>      
        [HttpPost]
        public JsonResult ExportarPuntoEntrega(int periodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoPuntoEntrega;
                this.servicio.ExportarPuntoEntregaPeriodo(periodo, path, file);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarPuntoEntrega()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                ConstantesCalculoResarcimiento.ArchivoPuntoEntrega;
            return File(fullPath, Constantes.AppExcel, ConstantesCalculoResarcimiento.ArchivoPuntoEntrega);
        }


        /// <summary>
        /// Permite exportar los ingresos por transmisión a Excel
        /// </summary>      
        [HttpPost]
        public JsonResult ExportarSuministradores(int periodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoPuntoEntregaSuministrador;
                this.servicio.ExportarSuministradorPorPuntoEntregaPeriodo(periodo, path, file);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarSuministradores()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                ConstantesCalculoResarcimiento.ArchivoPuntoEntregaSuministrador;
            return File(fullPath, Constantes.AppExcel, ConstantesCalculoResarcimiento.ArchivoPuntoEntregaSuministrador);
        }


        /// <summary>
        /// Permite mostrar la ventand e edicion de pto de entrega
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EditarPtoEntrega(int id)
        {
            InsumosModel model = new InsumosModel();
            model.ListaMaestroPtoEntrega = this.servicio.ListRePuntoEntregas().Where
                (x=>x.Repentestado == Constantes.EstadoActivo).ToList();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        /// <summary>
        /// Permite quitar el punto de entrega
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarPtoEntrega(int id, int periodo)
        {
            return Json(this.servicio.DeleteRePtoentregaPeriodo(id, periodo));
        }

        /// <summary>
        /// Permite grabar el punto de entrega del periodo
        /// </summary>
        /// <param name="ptoEntrega"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarPtoEntrega(int ptoEntrega, int periodo)
        {
            return Json(this.servicio.GrabarPtoEntregaPeriodo(periodo, ptoEntrega, base.UserName));
        }

        /// <summary>
        /// Permite cargar el archivo de puntos de entrega
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadPtoEntrega()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesCalculoResarcimiento.ArchivoPtoEntrega;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite cargar el archivo de puntos de entrega
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadSuministrador()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesCalculoResarcimiento.ArchivoImportacionSuministrador;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite realizar la importación de los puntos de entrega
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarPtoEntrega(int periodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos + ConstantesCalculoResarcimiento.ArchivoPtoEntrega;
                List<RePtoentregaPeriodoDTO> entitys = LecturaExcel.ObtenerPuntoEntrega(path);
                List<string> validaciones = new List<string>();
                this.servicio.CargarMasivoPtoEntrega(entitys, periodo, base.UserName, out validaciones);

                if(validaciones.Count == 0)                
                    return Json(new { Result = 1, Errores = new List<string>() });                
                else                
                    return Json(new { Result = 2, Errores = validaciones});                
            }
            catch
            {
                return Json(new { Result = -1, Errores = new List<string>() });
            }
        }

        /// <summary>
        /// Permite realizar la importación de los puntos de entrega
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarSuministradores(int periodo)
        {
            try
            {
                List<string> validaciones = new List<string>();
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos + 
                    ConstantesCalculoResarcimiento.ArchivoImportacionSuministrador;
                List<RePtoentregaSuministradorDTO> entitys = LecturaExcel.ObtenerSuministradoresPorPuntoEntrega(path, out validaciones);

                if (validaciones.Count == 0)
                {
                    this.servicio.CargarMasivoSuministradoresPorPuntoEntrega(entitys, periodo, base.UserName, out validaciones);

                    if (validaciones.Count == 0)
                        return Json(new { Result = 1, Errores = new List<string>() });
                    else
                        return Json(new { Result = 2, Errores = validaciones });
                }
                else
                {
                    return Json(new { Result = 2, Errores = validaciones });
                }
            }
            catch
            {
                return Json(new { Result = -1, Errores = new List<string>() });
            }
        }

        /// <summary>
        /// Permite visualizar los suministradores por punto de entrega
        /// </summary>
        /// <param name="idPtoEntrega"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EditarSuministrador(int idPtoEntrega, int periodo)
        {
            InsumosModel model = new InsumosModel();
            model.ListSuministradores = this.servicio.ObtenerEmpresasSuministradorasTotal();
            model.ListadoSuministradorPeriodo = this.servicio.ObtenerSuministradoresPorPuntoEntrega(idPtoEntrega, periodo);

            return PartialView(model);
        }

        /// <summary>
        /// Permite almacenar la relación de suministrador por punto de entrega
        /// </summary>
        /// <param name="idPtoEntrega"></param>
        /// <param name="periodo"></param>
        /// <param name="suministradores"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarSuministrador(int idPtoEntrega, int periodo, string suministradores)
        {
            return Json(this.servicio.GrabarPtoEntregaSuministrador(idPtoEntrega, periodo, suministradores, base.UserName));
        }

        /// <summary>
        /// Permite obtener la estructura de indicadores por periodos
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerIndicadores(int periodo)
        {
            int[] rowspan = null;
            string[][] result = this.servicio.ObtenerEstructuraIndicadores(periodo, out rowspan);

            return Json(new { Data = result, RowSpans = rowspan });
        }

        /// <summary>
        /// Permite grabar los indicadores por periodo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarIndicadores(string[][] data, int periodo)
        {
            return Json(this.servicio.GrabarIndicadoresPorPeriodo(data, periodo, base.UserName));
        }

        /// <summary>
        /// Permite obtener la estructura de indicadores por periodos
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerTolerancias(int periodo)
        {            
            string[][] result = this.servicio.ObtenerEstructuraTolerancia(periodo);
            return Json(new { Data = result});
        }

        /// <summary>
        /// Permite grabar los indicadores por periodo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarTolerancias(string[][] data, int periodo)
        {
            return Json(this.servicio.GrabarToleranciaPorPeriodo(data, periodo, base.UserName));
        }

        /// <summary>
        /// Permite mostrar el listado de ingresos por transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoIngreso(int periodo) 
        {
            InsumosModel model = new InsumosModel();
            model.ListaIngresos = this.servicio.ListIngresosPorTransmision(periodo);
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos del ingreso por transmision
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EditarIngreso(int id)
        {
            InsumosModel model = new InsumosModel();
            model.ListaEmpresa = this.servicio.ObtenerEmpresas();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            if (id == 0)
            {
                model.EntidadIngreso = new ReIngresoTransmisionDTO();
                model.EntidadIngreso.Reingmoneda = ConstantesCalculoResarcimiento.MonedaSoles;
            }
            else 
            {
                model.EntidadIngreso = this.servicio.ObtenerIngresoPorId(id);
            }

            return PartialView(model);
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
        public JsonResult GrabarIngreso(int codigo, int empresa, string moneda, decimal ingreso, int periodo)
        {
            return Json(this.servicio.GrabarIngreso(codigo, empresa, moneda, ingreso, periodo, base.UserName));
        }

        /// <summary>
        /// Permite eliminar un ingreso
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarIngreso(int codigo)
        {
            return Json(this.servicio.EliminarIngreso(codigo));
        }

        /// <summary>
        /// Permite exportar los ingresos por transmisión a Excel
        /// </summary>      
        [HttpPost]
        public JsonResult ExportarIngresos(int periodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoIngresoTransmision;                
                this.servicio.ExportarIngresos(periodo, path, file);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarIngresos()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos + 
                ConstantesCalculoResarcimiento.ArchivoIngresoTransmision;
            return File(fullPath, Constantes.AppExcel, ConstantesCalculoResarcimiento.ArchivoIngresoTransmision);
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
            string fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoIngreso, id.ToString(), extension);          
            Stream stream = FileServer.DownloadToStream(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
              ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

            return File(stream, extension, fileName);
        }

        /// <summary>
        /// Permite mostrar el listado de eventos
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoEvento(int periodo) 
        {
            InsumosModel model = new InsumosModel();
            model.ListaEventos = this.servicio.ObtenerEventosPorPeriodo(periodo);
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el formulario de eventos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EditarEvento(int id, int idPeriodo)
        {
            InsumosModel model = new InsumosModel();
            model.ListaEmpresa = this.servicio.ObtenerEmpresas();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            if (id == 0)
            {
                model.EntidadEvento = new ReEventoPeriodoDTO();
                model.EntidadEvento.Repercodi = idPeriodo;
            }
            else
            {
                model.EntidadEvento = this.servicio.ObtenerEvento(id);
                model.EntidadEvento.FechaEvento = (model.EntidadEvento.Reevefecha != null) ?
                    ((DateTime)model.EntidadEvento.Reevefecha).ToString(Constantes.FormatoFecha) : string.Empty;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del evento
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEvento(InsumosModel model)
        {
            ReEventoPeriodoDTO entity = new ReEventoPeriodoDTO();
            entity.Reevecodi = model.CodigoEvento;
            entity.Repercodi = model.CodigoPeriodo;
            entity.Reevedescripcion = model.Evento;
            entity.Reevefecha = DateTime.ParseExact(model.FechaEvento, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            entity.Reeveempr1 = model.Empresa1;
            entity.Reeveempr2 = model.Empresa2;
            entity.Reeveempr3 = model.Empresa3;
            entity.Reeveempr4 = model.Empresa4;
            entity.Reeveempr5 = model.Empresa5;
            entity.Reeveporc1 = model.Porcentaje1;
            entity.Reeveporc2 = model.Porcentaje2;
            entity.Reeveporc3 = model.Porcentaje3;
            entity.Reeveporc4 = model.Porcentaje4;
            entity.Reeveporc5 = model.Porcentaje5;
            entity.Reevecomentario = model.Comentario;
            entity.Reeveestado = Constantes.EstadoActivo;

            return Json(this.servicio.GrabarEvento(entity, base.UserName));            
        }

        /// <summary>
        /// Permite eliminar un evento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEvento(int id)
        {
            return Json(this.servicio.EliminarEvento(id));
        }

        /// <summary>
        /// Permite exportar los eventos a Excel
        /// </summary>      
        [HttpPost]
        public JsonResult ExportarEventos(int periodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoEventosCOES;
                this.servicio.ExportarEventos(periodo, path, file);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarEventos()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                ConstantesCalculoResarcimiento.ArchivoEventosCOES;
            return File(fullPath, Constantes.AppExcel, ConstantesCalculoResarcimiento.ArchivoEventosCOES);
        }

        /// <summary>
        /// Permite cargar el archivo de puntos de entrega
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadEvento()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesCalculoResarcimiento.ArchivoEventosCOES;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite realizar la importación de los puntos de entrega
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarEvento(int periodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos + 
                    ConstantesCalculoResarcimiento.ArchivoEventosCOES;
                List<ReEventoPeriodoDTO> entitys = LecturaExcel.ObtenerEventos(path);
                List<string> validaciones = new List<string>();
                this.servicio.CargarMasivoEvento(entitys, periodo, base.UserName, out validaciones);

                if (validaciones.Count == 0)
                    return Json(new { Result = 1, Errores = new List<string>() });
                else
                    return Json(new { Result = 2, Errores = validaciones });
            }
            catch
            {
                return Json(new { Result = -1, Errores = new List<string>() });
            }
        }


        /// <summary>
        /// Permite realizar la consulta de interrupciones
        /// </summary>        
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConsultarInterrupciones(int periodo)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            EstructuraInterrupcionInsumo estructura = this.servicio.ObtenerEstructuraInsumoInterrupciones(periodo, false);
            estructura.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            result.Content = serializer.Serialize(estructura);
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormatoInterrupcion(int periodo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
            string template = ConstantesCalculoResarcimiento.PlantillaCargaInterrupcionInsumo;
            string file = ConstantesCalculoResarcimiento.FormatoCargaInterrupcionInsumo;

            return Json(this.servicio.GenerarFormatoInterrupcionesInsumo(path, template, file,  periodo));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormatoInterrupcion()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                ConstantesCalculoResarcimiento.FormatoCargaInterrupcionInsumo;
            return File(fullPath, Constantes.AppExcel, ConstantesCalculoResarcimiento.FormatoCargaInterrupcionInsumo);
        }

        /// <summary>
        /// Permite cargar el archivo de puntos de entrega
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadInterrupcion()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesCalculoResarcimiento.ArchivoImportacionInterrupcionInsumo;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite realizar la importacion de suministros
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarInterrupciones(int periodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoImportacionInterrupcionInsumo;

                List<string> validaciones = new List<string>();
                string[][] data = this.servicio.CargarInterrupcionesInsumosExcel(path, file, periodo, out validaciones);

                if (validaciones.Count == 0)
                    return Json(new { Result = 1, Data = data, Errores = new List<string>() });
                else
                    return Json(new { Result = 2, Data = new string[1][], Errores = validaciones });
            }
            catch
            {
                return Json(new { Result = -1, Data = new string[1][], Errores = new List<string>() });
            }
        }

        /// <summary>
        /// Permite grabar los datos de interrupciones
        /// </summary>
        /// <param name="data"></param>       
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarInterrupciones(string[][] data, int periodo)
        {            
            return Json(this.servicio.GrabarInterrupcionesInsumo(data, periodo, base.UserName));
        }



    }
}
