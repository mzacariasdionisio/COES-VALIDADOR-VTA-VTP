using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ServicioRPFNuevo.Helper;
using COES.MVC.Intranet.Areas.ServicioRPFNuevo.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.ServicioRPF;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Controllers
{
    public class AnalisisAlternativoController : Controller
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AnalisisAlternativoController));

        /// <summary>
        /// Instancia de objeto para acceso a datos
        /// </summary>
        RpfAppServicio logic = new RpfAppServicio();

        /// <summary>
        /// Configuracion del analisis
        /// </summary>
        ConfiguracionRPF configuracion = new ConfiguracionRPF();

        /// <summary>
        /// Almacena los datos a procesar
        /// </summary>
        public List<RegistrorpfDTO> ListaDatos
        {
            get
            {
                return (Session[DatosSesion.ListaEvaluacion] != null) ?
                    (List<RegistrorpfDTO>)Session[DatosSesion.ListaEvaluacion] : new List<RegistrorpfDTO>();
            }
            set { Session[DatosSesion.ListaEvaluacion] = value; }
        }

        /// <summary>
        /// Almacena el resultado del proceso
        /// </summary>
        public List<ServicioRpfDTO> ListaConfiguracion
        {
            get
            {
                return (Session[DatosSesion.ListaConfiguracion] != null) ?
                    (List<ServicioRpfDTO>)Session[DatosSesion.ListaConfiguracion] : new List<ServicioRpfDTO>();
            }
            set { Session[DatosSesion.ListaConfiguracion] = value; }
        }

        /// <summary>
        /// Pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ServicioModel model = new ServicioModel();
            model.FechaConsulta = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            return View(model);
        }


        /// <summary>
        /// Permite mostrar la pantala de configuracion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Configuracion()
        {
            ConfiguracionRpfModel model = new ConfiguracionRpfModel();

            model.ListaParametro = (new ConfiguracionRPF()).ListarParametros(ValoresRPF.AnalisisAlternativo);

            return PartialView(model);
        }

        /// <summary>
        /// Obtencion del rango a evaluar
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult SeleccionRango(string fecha, decimal varFmax)
        {
            AnalisisModel model = new AnalisisModel();

            DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<ServicioRpfDTO> list = this.logic.ObtenerUnidadesCarga(fechaProceso);
            List<int> ids = (from puntos in list select puntos.PTOMEDICODI).Distinct().ToList();

            List<ParametroRpfDTO> parametros = (new ConfiguracionRPF()).ListarParametros(ValoresRPF.AnalisisAlternativo);

            //Obtenemos los parámetros a utilizar
            decimal ajuste = decimal.Parse(parametros.Where(x => x.PARAMRPFCODI == ValoresRPF.VariacionPotenciaAlt).First().PARAMVALUE);
            decimal constanteD = decimal.Parse(parametros.Where(x => x.PARAMRPFCODI == ValoresRPF.ConstanteDAlt).First().PARAMVALUE);
            decimal frecuenciaNominal = decimal.Parse(parametros.Where(x => x.PARAMRPFCODI == ValoresRPF.FrecuenciaNomimalAlt).First().PARAMVALUE);
            decimal balance = decimal.Parse(parametros.Where(x => x.PARAMRPFCODI == ValoresRPF.PorcentajeBalanceAlt).First().PARAMVALUE);
            decimal frecuenciaLimMax = decimal.Parse(parametros.Where(x => x.PARAMRPFCODI == ValoresRPF.FrecuenciaLimiteMaxAlt).First().PARAMVALUE);
            decimal frecuenciaLimMin = decimal.Parse(parametros.Where(x => x.PARAMRPFCODI == ValoresRPF.FrecuenciaLimiteMinAlt).First().PARAMVALUE);
            int intentos = int.Parse(parametros.Where(x => x.PARAMRPFCODI == ValoresRPF.NumeroIntentosAlt).First().PARAMVALUE);
            int minimo = int.Parse(parametros.Where(x => x.PARAMRPFCODI == ValoresRPF.CantidadMinSinRangoAlt).First().PARAMVALUE);
            int nroDatos = int.Parse(parametros.Where(x => x.PARAMRPFCODI == ValoresRPF.NroDatosAlt).First().PARAMVALUE);

            //Obtenemos los valorss de fmaxgen y fmingen
            decimal fmaxgen = frecuenciaNominal + constanteD * varFmax;
            decimal fmingen = frecuenciaNominal - constanteD * varFmax;

            List<int> idsCargaron = null;
            List<int> idsNoEncontrados = null;
            List<int> idsPotenciaCero = null;
            List<int> idsFrecuenciaCero = null;

            //Obtenemos el resultado desde el web service
            List<RegistrorpfDTO> resultado = this.logic.ObtenerRangoSegundos(fechaProceso, ajuste, intentos, minimo, nroDatos,
                fmaxgen, fmingen, frecuenciaLimMax, frecuenciaLimMin, balance, out idsCargaron,
                out idsNoEncontrados, out idsPotenciaCero, out idsFrecuenciaCero);

            //Lista que no cargaron
            List<ServicioRpfDTO> listNoCargaron = list.Where(x => !idsCargaron.Any(y => x.PTOMEDICODI == y)).ToList();
            model.ListaNoCargaron = listNoCargaron;

            //Lista no encontrados rango
            List<ServicioRpfDTO> listNoEncontrado = list.Where(x => idsNoEncontrados.Any(y => x.PTOMEDICODI == y)).ToList();
            model.ListaNoEncontrados = listNoEncontrado;

            //potencia o frecuencia en cero
            List<int> idsPotenciaError = new List<int>(idsPotenciaCero);
            List<int> idsIncorrecto = idsPotenciaCero.Union(idsFrecuenciaCero).ToList();
            List<ServicioRpfDTO> listIncorrecto = list.Where(x => idsIncorrecto.Any(y => x.PTOMEDICODI == y)).ToList();

            foreach (ServicioRpfDTO item in listIncorrecto)
            {
                if (idsPotenciaError.Contains(item.PTOMEDICODI))
                {
                    item.INDICADORPOTENCIA = Constantes.CaracterCero;
                }
                if (idsFrecuenciaCero.Contains(item.PTOMEDICODI))
                {
                    item.INDICADORFRECUENCIA = Constantes.CaracterCero;
                }
            }

            model.ListIncorrecto = listIncorrecto;

            List<int> idsOk = (from item in resultado select item.PTOMEDICODI).Distinct().ToList();
            List<ServicioRpfDTO> listOk = list.Where(x => idsOk.Any(y => x.PTOMEDICODI == y)).ToList();

            foreach (ServicioRpfDTO item in listOk)
            {
                RegistrorpfDTO entity = resultado.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).First();
                item.HORAINICIO = entity.HORAINICIO.ToString(Constantes.FormatoHora);
                item.HORAFIN = entity.HORAFIN.ToString(Constantes.FormatoHora);
                item.BALANCE = entity.BALANCE;
                item.NRODATOS = entity.NRODATOS;
            }
            //aca debemos asegurarnos que la lista esté ordenada en base al segundo
            model.ListaOK = listOk.OrderBy(x => x.HORAINICIO).ToList();

            this.ListaDatos = resultado;
            this.ListaConfiguracion = listOk;

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los parametros de configuración del análisis
        /// </summary>
        /// <param name="estatismo"></param>
        /// <param name="rPrimaria"></param>
        /// <param name="frecNominal"></param>
        /// <param name="porCumplimiento"></param>
        /// <param name="varPotencia"></param>
        /// <param name="varFrecuencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarParametro(decimal variacionPotencia, decimal constanteD, decimal frecuenciaNominal, decimal balance,
            decimal frecuenciaLimMax, decimal frecuenciaLimMin, decimal nroIteraciones, decimal cantidadSinRango, decimal nroDatos)
        {
            try
            {
                List<ParametroRpfDTO> entitys = new List<ParametroRpfDTO>();

                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.VariacionPotenciaAlt, PARAMVALUE = variacionPotencia.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.ConstanteDAlt, PARAMVALUE = constanteD.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.FrecuenciaNomimalAlt, PARAMVALUE = frecuenciaNominal.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.PorcentajeBalanceAlt, PARAMVALUE = balance.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.FrecuenciaLimiteMaxAlt, PARAMVALUE = frecuenciaLimMax.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.FrecuenciaLimiteMinAlt, PARAMVALUE = frecuenciaLimMin.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.NumeroIntentosAlt, PARAMVALUE = nroIteraciones.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.CantidadMinSinRangoAlt, PARAMVALUE = cantidadSinRango.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.NroDatosAlt, PARAMVALUE = nroDatos.ToString() });

                (new ConfiguracionRPF()).GrabarParametro(entitys, ValoresRPF.AnalisisAlternativo);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite generar el reporte de cumplimiento
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar()
        {
            try
            {
                ExcelDocument.GenerarReporteRangoHoras(this.ListaConfiguracion, this.ListaDatos);
                return Json(1);
            }
            catch (Exception ex)
            {
                Logger.Error(Constantes.LogError, ex);
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite abrir el archivo del reporte de rango de segundos generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + NombreArchivo.RangoHorasRPF;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.RangoHorasRPF);
        }

    }
}
