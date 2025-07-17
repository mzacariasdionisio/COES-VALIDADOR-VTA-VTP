using COES.MVC.Intranet.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Helper;
using System.Configuration;
using COES.Servicios.Aplicacion.CalculoPorcentajes.Helper;
using COES.MVC.Intranet.Helper;
using log4net;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class CalculoPorctAporteController : BaseController
    {

        public CalculoPorctAporteController()
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

        CalculoPorcentajesAppServicio servicioCalculoPorcentajes = new CalculoPorcentajesAppServicio();
        //
        // GET: /AporteIntegrantes/CalculoPorctAporte/

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
        /// Permite exportar a un archivo excel los resultados del mes de calculo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de calculo</param>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int caiprscodi = 0, int caiajcodi = 0, string reporte = "", int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesCalculoPorcentajes.ReporteDirectorio].ToString();
                string file = "-1";
                if (reporte.Equals("Energia"))
                {
                    Log.Info("Genera Archivo Exportación - GenerarReporteEnergia");
                    file = this.servicioCalculoPorcentajes.GenerarReporteEnergia(caiprscodi, caiajcodi, formato, pathFile, pathLogo);
                }
                if (reporte.Equals("Potencia"))
                {
                    Log.Info("Genera Archivo Exportación - GenerarReportePotencia");
                    file = this.servicioCalculoPorcentajes.GenerarReportePotencia(caiprscodi, caiajcodi, formato, pathFile, pathLogo);
                }
                if (reporte.Equals("Transmision"))
                {
                    Log.Info("Genera Archivo Exportación - GenerarReporteTransmision");
                    file = this.servicioCalculoPorcentajes.GenerarReporteTransmision(caiprscodi, caiajcodi, formato, pathFile, pathLogo);
                }
                if (reporte.Equals("Porcentaje"))
                {
                    Log.Info("Genera Archivo Exportación - GenerarReportePorcentajes");
                    file = this.servicioCalculoPorcentajes.GenerarReportePorcentajes(caiprscodi, caiajcodi, formato, pathFile, pathLogo);
                }
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Abrir el archivo
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesCalculoPorcentajes.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }

    }
}
