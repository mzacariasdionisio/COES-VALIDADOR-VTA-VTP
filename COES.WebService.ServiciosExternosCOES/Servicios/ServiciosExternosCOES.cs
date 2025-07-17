using COES.WebService.ServiciosExternosCOES.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Globalization;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using log4net;
using COES.WebService.ServiciosExternosCOES.Resultados;

namespace COES.WebService.ServiciosExternosCOES.Servicios
{
    /// <summary>
    /// Conjunto de servicios para consumo y envío de información para agentes
    /// </summary>
    public class ServiciosExternosCOES : IServiciosExternosCOES
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ServiciosExternosCOES));
        public ServiciosExternosCOES()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Método que devuelve los costos marginales programados según el día de cosnulta
        /// </summary>
        /// <param name="fecha">Fecha de Consulta</param>
        /// <returns>Listado de Costos marginales programados cada média hora del día de consulta</returns>
        public List<ObtenerCostoMarginalResult> ObtenerCostoMarginal(string fecha)
        {
            try
            {
                List<ObtenerCostoMarginalResult> costosMarginales = new List<ObtenerCostoMarginalResult>();
                DateTime dfecha = DateTime.Now;

                if (!string.IsNullOrEmpty(fecha))
                {
                    log.Info(fecha);
                    dfecha = DateTime.ParseExact(fecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                }

                List<CmCostomarginalDTO> result = (new CortoPlazoAppServicio()).ObtenerReporteCostosMarginalesWeb(dfecha, dfecha, "S");
                result = result.OrderByDescending(x => x.Indicador).ToList();
                foreach (var costo in result)
                {
                    costosMarginales.Add(new ObtenerCostoMarginalResult { CMGNFECHA = costo.Cmgnfecha, CMGNENERGIA = costo.Cmgnenergia, CMGNCONGESTION = costo.Cmgncongestion, CMGNTOTAL = costo.Cmgntotal, CNFBARNOMBRE = costo.Cnfbarnombre });
                }
                return costosMarginales;
            }
            catch (Exception ex)
            {
                log.Error("ObtenerCostoMarginal", ex);
                return null;
            }
        }

        /// <summary>
        /// Método que devuelve la demanda ejecutada según el día de consulta.
        /// </summary>
        /// <param name="fecha">Fecha de Consulta</param>
        /// <returns>Demanda Ejecutada Cada 30 minutos</returns>
        public ObtenerDemandaEjecutadaResult ObtenerDemandaEjecutada(string fecha)
        {
            try
            {
                DateTime dfecha = DateTime.Now;

                if (!string.IsNullOrEmpty(fecha))
                {
                    log.Info(fecha);
                    dfecha = DateTime.ParseExact(fecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                }

                PortalAppServicio servicio = new PortalAppServicio();

                var demandaEjecutada = servicio.ObtenerDatosDemandaPortal(dfecha, dfecha, 6);
                if (demandaEjecutada != null && demandaEjecutada[0] != null)
                {

                    return new ObtenerDemandaEjecutadaResult
                    {
                        Fecha = demandaEjecutada[0].FechaFila,
                        H1 = demandaEjecutada[0].H1,
                        H2 = demandaEjecutada[0].H2,
                        H3 = demandaEjecutada[0].H3,
                        H4 = demandaEjecutada[0].H4,
                        H5 = demandaEjecutada[0].H5,
                        H6 = demandaEjecutada[0].H6,
                        H7 = demandaEjecutada[0].H7,
                        H8 = demandaEjecutada[0].H8,
                        H9 = demandaEjecutada[0].H9,
                        H10 = demandaEjecutada[0].H10,
                        H11 = demandaEjecutada[0].H11,
                        H12 = demandaEjecutada[0].H12,
                        H13 = demandaEjecutada[0].H13,
                        H14 = demandaEjecutada[0].H14,
                        H15 = demandaEjecutada[0].H15,
                        H16 = demandaEjecutada[0].H16,
                        H17 = demandaEjecutada[0].H17,
                        H18 = demandaEjecutada[0].H18,
                        H19 = demandaEjecutada[0].H19,
                        H20 = demandaEjecutada[0].H20,
                        H21 = demandaEjecutada[0].H21,
                        H22 = demandaEjecutada[0].H22,
                        H23 = demandaEjecutada[0].H23,
                        H24 = demandaEjecutada[0].H24,
                        H25 = demandaEjecutada[0].H25,
                        H26 = demandaEjecutada[0].H26,
                        H27 = demandaEjecutada[0].H27,
                        H28 = demandaEjecutada[0].H28,
                        H29 = demandaEjecutada[0].H29,
                        H30 = demandaEjecutada[0].H30,
                        H31 = demandaEjecutada[0].H31,
                        H32 = demandaEjecutada[0].H32,
                        H33 = demandaEjecutada[0].H33,
                        H34 = demandaEjecutada[0].H34,
                        H35 = demandaEjecutada[0].H35,
                        H36 = demandaEjecutada[0].H36,
                        H37 = demandaEjecutada[0].H37,
                        H38 = demandaEjecutada[0].H38,
                        H39 = demandaEjecutada[0].H39,
                        H40 = demandaEjecutada[0].H40,
                        H41 = demandaEjecutada[0].H41,
                        H42 = demandaEjecutada[0].H42,
                        H43 = demandaEjecutada[0].H43,
                        H44 = demandaEjecutada[0].H44,
                        H45 = demandaEjecutada[0].H45,
                        H46 = demandaEjecutada[0].H46,
                        H47 = demandaEjecutada[0].H47,
                        H48 = demandaEjecutada[0].H48
                    };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                log.Error("ObtenerDemandaEjecutada", ex);
                return null;
            }


        }
    }
}