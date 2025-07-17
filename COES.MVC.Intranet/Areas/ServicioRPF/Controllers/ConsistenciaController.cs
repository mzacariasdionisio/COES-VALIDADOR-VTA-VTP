using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ServicioRPF.Helper;
using COES.MVC.Intranet.Areas.ServicioRPF.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.ServicioCloud;
using COES.Servicios.Aplicacion.ServicioRPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ServicioRPF.Controllers
{
    public class ConsistenciaController : Controller
    {
        ServicioCloudClient servicioCloud = new ServicioCloudClient();
        RpfAppServicio logic = new RpfAppServicio();

        /// <summary>
        /// Almacena los datos del reporte
        /// </summary>
        public List<ServicioRpfDTO> ListaReporte
        {
            get
            {
                return (Session[DatosSesion.ListaReporteConsistencia] != null) ?
                    (List<ServicioRpfDTO>)Session[DatosSesion.ListaReporteConsistencia] : new List<ServicioRpfDTO>();
            }
            set { Session[DatosSesion.ListaReporteConsistencia] = value; }
        }

        /// <summary>
        /// Almacena la fecha de consulta
        /// </summary>
        public DateTime FechaConsulta
        {
            get
            {
                return (Session[DatosSesion.FechaProceso] != null) ?
                    (DateTime)Session[DatosSesion.FechaProceso] : DateTime.Now;
            }
            set { Session[DatosSesion.FechaProceso] = value; }
        }

        /// <summary>
        /// Pagina de consulta de carga de datos
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ServicioModel model = new ServicioModel();
            model.FechaConsulta = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite verificar existencia de datos
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultarExistencia(string fecha)
        {
            int result = 0;
            try
            {
                using (ServicioCloudClient service = new ServicioCloudClient())
                {
                    DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    List<ReporteEnvio> resultado = service.ObtenerReporteEndio(fechaConsulta).ToList();

                    if (resultado.Count > 0)
                    {
                        result = 1;
                    }
                }
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Consulta el estado de envio
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PartialViewResult Consulta(string fecha, int indicador)
        {
            ServicioModel model = new ServicioModel();

            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<ServicioRpfDTO> list = this.logic.ObtenerUnidadesCarga(fechaConsulta);
            List<int> ids = (from puntos in list select puntos.PTOMEDICODI).Distinct().ToList();
            int error = 0;

            List<ReporteEnvio> resultado = null;

            if (indicador == 1)
            {
                List<decimal> frecuencias = this.logic.ObtenerFrecuenciasComparacion(fechaConsulta);

                if (frecuencias.Count == 86400)
                {
                    decimal potencia = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.PotenciaPercentil);
                    decimal percentil = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.ValorPercentil);
                    decimal porcentaje = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.PorcentajePercentil);

                    #region Obteniendo Resultado

                    using (ServicioCloudClient cliente = new ServicioCloudClient())
                    {
                        try
                        {
                            int resul = cliente.ConsultaEnvioDatos(fechaConsulta,
                                frecuencias.ToArray(), porcentaje, potencia, percentil);

                            if (resul == 1)
                            {
                                resultado = cliente.ObtenerReporteEndio(fechaConsulta).ToList();
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }

                    #endregion
                }
                else
                {
                    error = 4;
                }
            }
            else
            {
                using (ServicioCloudClient service = new ServicioCloudClient())
                {
                    resultado = service.ObtenerReporteEndio(fechaConsulta).ToList();
                    error = 5;
                }
            }

            if (resultado != null)
            {
                foreach (ServicioRpfDTO item in list)
                {
                    bool flag = false;

                    foreach (ReporteEnvio result in resultado)
                    {
                        if (item.PTOMEDICODI == result.PtoMediCodi)
                        {
                            item.FechaCarga = (result.FechaCarga != null) ? ((DateTime)result.FechaCarga).ToString(Constantes.FormatoFechaHora)
                                : string.Empty;
                            item.EstadoOperativo = result.EstadoOperativo;
                            item.EstadoInformacion = result.EstadoInformacion;
                            item.Consistencia = result.ValConsistencia;
                            item.IndicadorConsistencia = result.IndConsistencia;

                            if (item.EstadoOperativo == Constantes.NO)
                            {
                                if (item.FAMCODI == 2 || item.FAMCODI == 4)
                                {
                                    item.EstadoOperativo = Constantes.Indeterminado;
                                }
                                if (item.FAMCODI == 3 || item.FAMCODI == 5)
                                {
                                    int contador = this.logic.ObtenerEstadoOperativo(item.EQUICODI, fechaConsulta);
                                    item.EstadoOperativo = (contador > 0) ? Constantes.Opero : Constantes.NoOpero;
                                }
                            }

                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        if (item.FAMCODI == 2 || item.FAMCODI == 4)
                        {
                            item.EstadoOperativo = Constantes.Indeterminado;
                        }
                        if (item.FAMCODI == 3 || item.FAMCODI == 5)
                        {
                            int contador = this.logic.ObtenerEstadoOperativo(item.EQUICODI, fechaConsulta);
                            item.EstadoOperativo = (contador > 0) ? Constantes.Opero : Constantes.NoOpero;
                        }
                    }
                }
            }

            model.ListaConsulta = list;
            this.FechaConsulta = fechaConsulta;
            this.ListaReporte = list;
            model.IndicadorReporte = error;

            return PartialView(model);
        }

        /// Permite generar el archivo de reporte de datos cargados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fecha)
        {
            int indicador = 1;
            try
            {
                if (this.ListaReporte != null)
                {
                    if (this.ListaReporte.Count > 0)
                    {
                        ExcelDocument.GenerarReporteConsistencia(this.ListaReporte, this.FechaConsulta);
                        indicador = 1;
                    }
                    else
                    {
                        indicador = 0;
                    }
                }
                else
                {
                    indicador = 0;
                }
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + NombreArchivo.ReporteConsistenciaRPF;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteConsistenciaRPF);
        }

        /// <summary>
        /// Permite mostrar la pantala de configuracion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Configuracion()
        {
            ConfiguracionRpfModel model = new ConfiguracionRpfModel();

            model.ListaParametro = (new ConfiguracionRPF()).ListarParametros(ValoresRPF.Consultas);

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
        public JsonResult GrabarParametro(decimal porcentaje, decimal percentil, decimal potencia)
        {
            try
            {
                List<ParametroRpfDTO> entitys = new List<ParametroRpfDTO>();

                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.PorcentajePercentil, PARAMVALUE = porcentaje.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.ValorPercentil, PARAMVALUE = percentil.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.PotenciaPercentil, PARAMVALUE = potencia.ToString() });

                (new ConfiguracionRPF()).GrabarParametro(entitys, ValoresRPF.Consultas);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

    }
}
