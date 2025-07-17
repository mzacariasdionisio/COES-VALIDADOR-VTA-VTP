using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.DemandaMaxima.Helper;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.DemandaMaxima;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Net;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.DemandaMaxima.Helper;
using log4net;
using System.Reflection;

namespace COES.MVC.Extranet.Areas.DemandaMaxima.Controllers
{
    public class ValidacionController : Controller 
    {
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
        public ValidacionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

         /// <summary>
         /// Instanciamiento de Log4net
         /// </summary>  
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        DemandaMaximaAppServicio servicio = new DemandaMaximaAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Carga principal de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            int empresa = (int)Session["sIdEmpresa"];
            string mes = (string)Session["sMes"];

            FormatoModel model = new FormatoModel();
            model.IdModulo = Modulos.AppMedidoresDistribucion;
                        
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasActivasPorUsuario(User.Identity.Name).Select(x => new SiEmpresaDTO
            {
                Emprcodi = x.EMPRCODI,
                Emprnomb = x.EMPRNOMB
            }).OrderBy(x => x.Emprnomb).ToList();
            
            if (empresa != null)
            {
                model.IdEmpresa = empresa;
            }
            else 
            {                
                if (model.ListaEmpresas.Count != 1)
                {
                    model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
                }
            }
            if (mes != null)
            {
                model.Mes = mes;
            }
            else
            {
                model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            }           
            
            return View(model);
        }

        /// <summary>
        /// Metodo que permite obtener el listado de observaciones en la vista
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <param name="variacion"></param>
        /// <param name="consumo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarObservaciones(int idEmpresa, string mes, int variacion, string consumo)
        {
            FormatoModel model = new FormatoModel();

            int imes = Int32.Parse(mes.Substring(0, 2));
            int ianho = Int32.Parse(mes.Substring(3, 4));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string text_fecha = fechaProceso.ToString(Constantes.FormatoFecha);

            List<MeEnvioDTO> listEnvio = this.servicio.ObtenerListaEnvioActual(idEmpresa, text_fecha);
            MeEnvioDTO envio = new MeEnvioDTO();
            envio = listEnvio[0];

            List<MeMedicion96DTO> listObservaciones = this.servicio.ObteneListaObservacionCoherencia(envio.EnvioCodiAct.Value, envio.EnvioCodiAnt.Value, 
                envio.EnvioFechaIniAct.Value.ToString(Constantes.FormatoFecha), envio.EnvioFechaFinAct.Value.ToString(Constantes.FormatoFecha), 
                envio.EnvioFechaIniAnt.Value.ToString(Constantes.FormatoFecha), envio.EnvioFechaFinAnt.Value.ToString(Constantes.FormatoFecha), variacion, consumo);
            List<String> list = new List<String>();
            if (listObservaciones.Count > 0)
            {
                foreach (var obs in listObservaciones)
                {
                    if (consumo == "M")
                    {
                        list.Add("El consumo de Pto. de Suministro " + obs.Ptomedielenomb + " tiene una variación del " + obs.VarMensual + 
                            "% con respecto al mes anterior.");
                    }
                    else
                    {
                        list.Add("El consumo de Pto. de Suministro " + obs.Ptomedielenomb + " tiene una variación del " + obs.VarPromDiaria + 
                            "% con respecto al mes anterior.");
                    }
                }
            }

            model.ListaObservaciones = list;
            return View(model);
        }

        /// <summary>
        /// Método para exportar en Excel las observaciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <param name="variacion"></param>
        /// <param name="consumo"></param>
        /// <returns></returns>
        public JsonResult Exportar(int idEmpresa, string mes, int variacion, string consumo)
        {
            try
            {
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + ConstantesDemandaMaxima.PathLogo;
                string file = this.servicio.GenerarFormatoValidacion(idEmpresa, mes, variacion, consumo, AppDomain.CurrentDomain.BaseDirectory +
                    ConstantesDemandaMaxima.RutaCarga, pathLogo);
                return Json(file);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Descarga el formato
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int formato, string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDemandaMaxima.RutaCarga + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, file);
        }

    }
}
