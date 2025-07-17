using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.StockCombustibles.Helper;
using COES.MVC.Intranet.Areas.StockCombustibles.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.StockCombustibles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.StockCombustibles.Controllers
{
    public class CumplimientoController : BaseController
    {
        //
        // GET: /StockCombustibles/Cumplimiento/

        /// <summary>
        /// Index de inicio de controller Cumplimiento
        /// </summary>
        /// <returns></returns>
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        StockCombustiblesAppServicio logic = new StockCombustiblesAppServicio();
        public ActionResult Index()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<int> idsFormatos = this.logic.ListaFormatos().Select(x => x.Formatcodi).ToList();
            string formatos = string.Join(",", idsFormatos.Select(n => n.ToString()).ToArray());
            model.ListaEmpresas = servFormato.GetListaEmpresaFormatoMultiple(formatos);//logicGeneral.ObtenerEmpresasHidro();
            model.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaCombo = logic.ListaFormatos().OrderBy(x => x.Formatnombre).ToList();
            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de cumplimiento
        /// </summary>
        /// <param name="sEmpresas"></param>
        /// <param name="idFormato"></param>
        /// <param name="fIni"></param>
        /// <param name="fFin"></param>
        /// <param name="mes1"></param>
        /// <param name="mes2"></param>
        /// <param name="semana1"></param>
        /// <param name="semana2"></param>
        /// <returns></returns>
        public PartialViewResult Lista(string sEmpresas, int idFormato, string fIni, string fFin, string envio, string estado)
        {
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            var formato = servFormato.GetByIdMeFormato(idFormato);
            fechaIni = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, string.Empty, fIni, Constantes.FormatoFecha);
            fechaFin = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, string.Empty, fFin, Constantes.FormatoFecha);
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.Resultado = logic.GeneraViewCumplimiento(sEmpresas, fechaIni, fechaFin, idFormato, (int)formato.Formatperiodo, envio, estado);
            model.NombreFortmato = formato.Formatnombre;
            return PartialView(model);
        }

        // exporta el reporte general consultado a archivo excel
        [HttpPost]
        public JsonResult GenerarReporteCumplimiento(string sEmpresas, int idFormato, string fIni, string fFin, string envio, string estado)
        {
            int indicador = 1;
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            //var formato = servFormato.GetByIdMeFormato(idFormato);
            fechaIni = DateTime.ParseExact(fIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = DateTime.ParseExact(fFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            StockCombustiblesModel model = new StockCombustiblesModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;

            try
            {
                logic.GeneraExcelCumplimiento(sEmpresas, fechaIni, fechaFin, idFormato, ruta + ConstantesIntranet.NombreArchivoCumplimiento,
                    ruta + Constantes.NombreLogoCoes, envio, estado );
                indicador = 1;

            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesIntranet.NombreArchivoCumplimiento;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            string file = string.Format("ReporteCumplimientoIEOD_{0}.xlsx", DateTime.Now.ToString("ddMMyyyy"));
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Listar las empresas segun formato IEOD
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult CargarListaEmpresa(int idFormato)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(idFormato);
            return Json(model);
        }

        #region Configuracion

        public ActionResult Configuracion()
        {
            NotificacionModel model = new NotificacionModel();
            model.IdPlantillaCorreo = ConstantesStockCombustibles.IdPlantillaNotificacion;
            model.ConfiguracionCorreo = this.logic.ObtenerConfiguracionCorreo();
            model.ListaEmpresa = this.logic.ObtenerEmpresasConfiguracion(-1);
            model.ListaFormato = this.logic.ListaFormatos().OrderBy(x => x.Formatnombre).ToList();
            return View(model);
        }
       
        /// <summary>
        /// Devuelve los detalles de la plantilla del correo
        /// </summary>
        /// <param name="plantillacodi"></param>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDetalleCorreo(int plantillacodi)
        {
            NotificacionModel model = new NotificacionModel();

            try
            {
                //Obtengo el registro de la plantilla
                SiPlantillacorreoDTO plantillaCompleta = new SiPlantillacorreoDTO();
                SiPlantillacorreoDTO plantillaConHoraEjecucion = new SiPlantillacorreoDTO();
                SiPlantillacorreoDTO plantillaBD = (new CorreoAppServicio()).GetByIdSiPlantillacorreo(plantillacodi);

                plantillaCompleta = plantillaBD;
                model.PlantillaCorreo = plantillaCompleta;
                //obtengo las variables del Contenido
                model.ListaVariables = logic.ObtenerListadoVariables(plantillacodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {                
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// devuelve el listado de variables 
        /// </summary>
        /// <param name="idBoton"></param>
        /// <param name="idPlantilla"></param>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarVariables(int idPlantilla, int campo)
        {
            NotificacionModel model = new NotificacionModel();

            try
            {         
                model.ListaVariables = logic.ObtenerListadoVariables(idPlantilla);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Guarda la informacion de una plantilla de correo
        /// </summary>
        /// <param name="plantillaCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarPlantillaCorreo(SiPlantillacorreoDTO plantillaCorreo)
        {
            NotificacionModel model = new NotificacionModel();

            try
            {               
                this.logic.ActualizarDatosPlantillaCorreo(plantillaCorreo, base.UserName);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {               
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite grabar la configuracion del correo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarConfiguracionCorreo(NotificacionModel model)
        {
            try
            {
                MeEnvcorreoConfDTO entity = new MeEnvcorreoConfDTO();
                entity.Ecconfanexo = model.Anexo;
                entity.Ecconfcargo = model.Cargo;
                entity.Ecconfestadonot = model.EstadoEjecucion;
                entity.Ecconfhoraenvio = model.HoraEjecucion;
                entity.Ecconfnombre = model.Firmante;
                entity.Ecconffeccreacion = DateTime.Now;
                entity.Ecconffecmodificacion = DateTime.Now;
                entity.Ecconfusucreacion = base.UserName;
                entity.Ecconfusumodificacion = base.UserName;

                this.logic.GrabarConfuguracionCorreo(entity);


                

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener las empresas
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEmpresaFormato(int idTipoEmpresa)
        {
            return Json(this.logic.ObtenerEmpresasConfiguracion(idTipoEmpresa));
        }

        /// <summary>
        /// Permite generar la consulta de la configuracion
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetConsuntaFormato(int idTipoEmpresa, int idEmpresa, int idFormato)
        {
            return Json(this.logic.ObtenerConsultaFormato(idTipoEmpresa, idEmpresa, idFormato));
        }

        /// <summary>
        /// Permite grabar la configuracion del formato
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarConfiguracionFormato(string[][] data)
        {
            return Json(this.logic.GrabarConfiguracionFormato(data, base.UserName));
        }

        #endregion

        #region Envio de Correos

        [HttpPost]
        public JsonResult EnviarCorreoNotificacion(string fecha)
        {
            DateTime fechaEnvio = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            int result = this.logic.EnviarCorreoNotificacion(fechaEnvio);
            return Json(result);
        }

        #endregion
    }
}
