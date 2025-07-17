using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using COES.Servicios.Distribuidos.Contratos;
using COES.Dominio.DTO.Sic;
using log4net;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.ServiceModel.Web;
using System.Net;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Distribuidos.SeguridadServicio;
using COES.Servicios.Distribuidos.Resultados;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class EquipoServicio : IEquipoServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EquipoServicio));

        /// <summary>
        /// Constructor
        /// </summary>
        public EquipoServicio() {
            log4net.Config.XmlConfigurator.Configure();
        }
        List<EqEquipoDTO> IEquipoServicio.GetCentralesPorPuntosMedicion(List<MeHojaptomedDTO> listaPuntos)
        {
            if (listaPuntos == null || listaPuntos.Count == 0)
            {
                throw new FaultException(new FaultReason("La empresa no tiene puntos de medición de envío"), new FaultCode("1"));
            }
            List<EqEquipoDTO> centrales = null;
            FormatoMedicionAppServicio formatoMedicion = new FormatoMedicionAppServicio();
            try
            {
                centrales = formatoMedicion.ListCentralByEmpresaAndFormato(listaPuntos);

                if (centrales == null || centrales.Count == 0)
                {
                    throw new FaultException(new FaultReason("No existen centrales para los puntos de medición consultados"), new FaultCode("2"));
                }
            }
            catch (Exception ex)
            {
                log.Error("GetCentralesPorPuntosMedicion", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al consultar las centrales"), new FaultCode("3"));
            }

            return centrales;
        }
    }
}