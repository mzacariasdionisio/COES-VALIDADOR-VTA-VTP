using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.Servicios.Aplicacion.CalculoPorcentajes.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class GeneracionEjecutadaController : BaseController
    {
        // GET: /AporteIntegrantes/GeneracionEjecutada/

        public GeneracionEjecutadaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }
        
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        CalculoPorcentajesAppServicio servicioCalculoPorcentajes = new CalculoPorcentajesAppServicio();
        ConsultaMedidoresAppServicio servicioConsultaMedidores = new ConsultaMedidoresAppServicio();

        public ActionResult Index(int caiprscodi = 0, int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            BaseModel model = new BaseModel();
            Log.Info("Lista Presupuesto - ListCaiPresupuestos");
            model.ListaPresupuesto = this.servicioCalculoPorcentajes.ListCaiPresupuestos();
            if (model.ListaPresupuesto.Count > 0 && caiprscodi == 0)
            {
                caiprscodi = model.ListaPresupuesto[0].Caiprscodi;

            }
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);
            Log.Info("Lista Ajuste - ListCaiAjustes");
            model.ListaAjuste = this.servicioCalculoPorcentajes.ListCaiAjustes(caiprscodi); //Ordenado en descendente
            if (model.ListaAjuste.Count > 0 && caiajcodi == 0)
            {
                caiajcodi = (int)model.ListaAjuste[0].Caiajcodi;
            }

            if (caiprscodi > 0 && caiajcodi > 0)
            {
                Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
                model.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(caiajcodi);
            }
            else
            {
                model.EntidadAjuste = new CaiAjusteDTO();
            }
            model.Caiprscodi = caiprscodi;
            model.Caiajcodi = caiajcodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        /// <summary>
        /// Permite Exportar la energía inyectada en bornes de generación - SGOCOES
        /// </summary>
        /// <param name="caiprscodi">Código de la Versión de Presupuesto</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarGE(int caiprscodi = 0)
        {
            base.ValidarSesionUsuario();
            string result = "-1";
            try
            {
                BaseModel model = new BaseModel();
                if (caiprscodi > 0)
                {
                    Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
                    model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);
                    string sMes = model.EntidadPresupuesto.Caiprsmesinicio.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;

                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPresupuesto.Caiprsanio;
                    var sFechaFin = System.DateTime.DaysInMonth(model.EntidadPresupuesto.Caiprsanio, model.EntidadPresupuesto.Caiprsmesinicio) + "/" + sMes + "/" + model.EntidadPresupuesto.Caiprsanio;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = DateTime.ParseExact(sFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    //Solo estamos exportando el primer mes
                    string tiposEmpresa = "3";
                    string empresas = Constantes.ParametroDefecto;
                    string tiposGeneracion = "4,1,3,2";
                    int central = 1;
                    string parametros = "1";
                    int tipo = 1;
                    string nombreArchivo = NombreArchivo.ReporteMedidoresHorizontal;
                    string pathFile = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    bool flag = (User.Identity.Name == Constantes.UsuarioAnonimo) ? false : true;
                    Log.Info("Exportar información - GenerarArchivoExportacion");
                    this.servicioConsultaMedidores.GenerarArchivoExportacion(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, central, parametros, pathFile, nombreArchivo, tipo, flag);

                    result = "1";
                }
            }
            catch (Exception e)
            {
                string sMensaje = e.Message;
                result = "-1";
            }
            return Json(result);
        }

        /// <summary>
        /// Abrir el archivode de la Reserva Asignada
        /// </summary>
        public virtual ActionResult AbrirArchivo()
        {
            string nombreArchivo = NombreArchivo.ReporteMedidoresHorizontal;
            string fullPath = ConfigurationManager.AppSettings[ConstantesCalculoPorcentajes.ReporteDirectorio].ToString() + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Permite copiar la energía inyectada en bornes de generación - SGOCOES
        /// </summary>
        /// <param name="caiprscodi">Código del Presupuesto</param>
        /// <param name="caiajcodi">Código de la Versión de Presupuesto</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarGE(int caiprscodi = 0, int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            BaseModel model = new BaseModel();
            model.sError = "";
            model.iNumReg = 0;
            string sCagdcmFuenteDatos = "EG"; //Ejecutados Generación
            try
            {
                if (caiprscodi > 0 && caiajcodi > 0)
                {
                    //Eliminando Todo el proceso de calculo
                    this.servicioCalculoPorcentajes.EliminarCalculo(caiajcodi);
                    //Eliminando la información del periodo
                    Log.Info("Elimina registro - DeleteCaiGenerdeman");
                    this.servicioCalculoPorcentajes.DeleteCaiGenerdeman(caiajcodi, sCagdcmFuenteDatos);
                    //Calculamos la fecha de inicio del Presupuesto
                    Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
                    model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);
                    string sMes = model.EntidadPresupuesto.Caiprsmesinicio.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes; 
                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPresupuesto.Caiprsanio;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    //Calculamos la fecha final Ejecutado:
                    Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
                    model.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(caiajcodi);
                    sMes = model.EntidadAjuste.Caiajmes.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicioAjuste = "01/" + sMes + "/" + model.EntidadAjuste.Caiajanio;
                    //La fecha de ajuste, es el inicio para la data Proyectada
                    DateTime dFecInicioAjuste = DateTime.ParseExact(sFechaInicioAjuste, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = dFecInicioAjuste.AddDays(-1); // al quitarle un dia, le estamos colocando al final del dia del mes anterior
                    
                    //Parametros de consulta
                    string tiposEmpresa = "3";
                    string tiposGeneracion = "4,1,3,2";
                    int central = 1;

                    Log.Info("Lista Empresa - ObteneEmpresasPorTipo");
                    List<int> idsEmpresas = this.servicioConsultaMedidores.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                    string empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);

                    string sUser = User.Identity.Name;
                    Log.Info("Insertar la generación ejecutada - CopiarSGOCOESCaiGenerdemans");
                    model.iNumReg = this.servicioCalculoPorcentajes.CopiarSGOCOESCaiGenerdemans(caiajcodi, empresas, central, tiposGeneracion, fecInicio, fecFin, sUser);
                }
                else
                    model.sError = "Debe seleccionar un periodo y versión correcto";
            }
            catch (Exception e)
            {
                model.sError = e.Message;
            }
            return Json(model);
        }

    }
}
