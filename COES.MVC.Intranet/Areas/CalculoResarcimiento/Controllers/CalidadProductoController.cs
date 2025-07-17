using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
   /// <summary>
   /// 
   /// </summary>
    public class CalidadProductoController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        CalidadProductoAppServicio servicio = new CalidadProductoAppServicio();
        CalculoResarcimientoAppServicio servicioResarcimiento = new CalculoResarcimientoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(PuntoEntregaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Muestra la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CalidadProductoModel model = new CalidadProductoModel();
            model.Anio = DateTime.Now.Year;
            model.Mes = DateTime.Now.Month;
            model.ListaAnio = this.servicio.ListarAnios();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return View(model);
        }

        /// <summary>
        /// Permite obtener la lista de eventos
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EventoLista(int anio, int mes)
        {            
            CalidadProductoModel model = new CalidadProductoModel();
            model.ListaEventos = this.servicio.ConsultarEventosProducto(anio, mes);
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos del evento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EventoEditar(int id)
        {
            CalidadProductoModel model = new CalidadProductoModel();
            model.ListaEmpresa = this.servicioResarcimiento.ObtenerEmpresas();
            model.ListaSuministradores = this.servicioResarcimiento.ObtenerEmpresasSuministradoras();
            model.ListaAnio = this.servicio.ListarAnios();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            if (id == 0)
            {
                model.Entidad = new ReEventoProductoDTO();
                model.ListaSuministrador = new List<ReEmpresaDTO>();
            }
            else
            {
                model.Entidad = this.servicio.GetByIdReEventoProducto(id);
                model.FechaInicial = (((DateTime)model.Entidad.Reevprfecinicio)).ToString(ConstantesAppServicio.FormatoFechaFull);
                model.FechaFinal = (((DateTime)model.Entidad.Reevprfecfin)).ToString(ConstantesAppServicio.FormatoFechaFull);
                model.ListaSuministrador = this.servicio.ObtenerSuministradorPorEvento(id);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del evento
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEvento(CalidadProductoModel model)
        {
            ReEventoProductoDTO entity = new ReEventoProductoDTO();
            entity.Reevprcodi = model.CodigoEvento;
            entity.Reevpranio = model.AnioEvento;
            entity.Reevprmes = model.MesEvento;
            entity.Reevprfecinicio = DateTime.ParseExact(model.FechaInicial, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
            entity.Reevprfecfin = DateTime.ParseExact(model.FechaFinal, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
            entity.Reevprptoentrega = model.PuntoEntrega;
            entity.Reevprtension = model.Tension;
            entity.Reevprempr1 = model.Empresa1;
            entity.Reevprempr2 = model.Empresa2;
            entity.Reevprempr3 = model.Empresa3;            
            entity.Reevprporc1 = model.Porcentaje1;
            entity.Reevprporc2 = model.Porcentaje2;
            entity.Reevprporc3 = model.Porcentaje3;            
            entity.Reevprcomentario = model.Comentario;
            entity.Reevpracceso = model.Acceso;
            entity.Reevprestado = Constantes.EstadoActivo;
            entity.Empresas = model.Empresas;

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
        public JsonResult ExportarEventos(int anio, int mes)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalidadProducto.ArchivoEventosProducto;
                this.servicio.ExportarEventos(anio, mes, path, file);
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
                ConstantesCalidadProducto.ArchivoEventosProducto;
            return File(fullPath, Constantes.AppExcel, ConstantesCalidadProducto.ArchivoEventosProducto);
        }

       
        #region Reporte Word

        /// <summary>
        /// Genera el reporte Word de informe de compensacion de mala calidad
        /// </summary>
        /// <param name="reevprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteInformeCompensacionMalaCalidad(int reevprcodi)
        {
            CalidadProductoModel model = new CalidadProductoModel();

            try
            {
                base.ValidarSesionJsonResult();
                
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;             
                string nameFile = "InformeCompensacionPorMalaCalidad.docx";

                servicio.GenerarReporteCompensacionPorMalaCalidadWord(reevprcodi, ruta, nameFile);
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

        #endregion

        #region Notificacion por correo

        /// <summary>
        /// Devuelve los datos del correo a enviar
        /// </summary>
        /// <param name="reevprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult cargarDatosCorreo(int reevprcodi)
        {
            CalidadProductoModel model = new CalidadProductoModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                ReEventoProductoDTO evento = servicio.GetByIdReEventoProducto(reevprcodi);
                SiCorreoDTO correo;
                if (reevprcodi == null)//Ver detalles de un correo
                {
                    throw new Exception("Código de evento no reconocido.");
                }
                else
                {
                    correo = servicio.ObtenerDatosCorreoCompensacionMalaCalidad(evento);
                }

                model.Entidad = evento;
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
        /// Envia los correos de notificacion de compensacion de mala calidad
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="repercodi"></param>
        /// <param name="tipoCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarNotificacionMalaCalidad(SiCorreoDTO correo, int reevprcodi)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                base.ValidarSesionJsonResult();
                                                

                //Actualizo la plantilla
                servicio.EnviarMensajeCompensacionMalaCalidad(correo, reevprcodi, base.UserName);
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
        /// valida que las empresas suministradoras tengan correo registrados
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="reevprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarCorreoEmpresas(int reevprcodi)
        {
            CorreoModel model = new CorreoModel();

            try
            {
                base.ValidarSesionJsonResult();
              

                List<ReEmpresaDTO> listaSuministradores = servicio.ObtenerEmpresasSuministradoras(reevprcodi);
                string msgValidacion = servicio.ValidarExistenciaCorreoEmpresas(listaSuministradores, out bool ningunaEmpresaConCorreo);
                if (msgValidacion != "")
                {
                    if (ningunaEmpresaConCorreo)
                        model.Resultado = "3";
                    else 
                        model.Resultado = "2";

                    model.Mensaje = msgValidacion;
                }
                else
                {
                    model.Resultado = "1";
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
        #endregion

    }
}