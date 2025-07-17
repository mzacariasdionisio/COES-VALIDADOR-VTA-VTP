using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Infraestructura.Datos.Repositorio.Transferencias;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using System.Data;
using log4net;

namespace COES.Servicios.Aplicacion.ReportesFrecuencia
{
    public class InformacionFrecuenciaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InformacionFrecuenciaAppServicio));

        /// <summary>
        ///  Ontener Reporte de Frecuencia de Desviacion
        /// </summary>
        /// <returns></returns>
        public List<InformacionFrecuenciaDTO> GetReporteFrecuenciaDesviacion()
        {
            var resultado = FactoryReportesFrecuencia.GetInformacionFrecuenciaRepository().GetReporteFrecuenciaDesviacion();
            return resultado;
        }

        /// <summary>
        ///  Ontener Reporte de Eventos de Frecuencia
        /// </summary>
        /// <returns></returns>
        public List<InformacionFrecuenciaDTO> GetReporteEventosFrecuencia()
        {
            var resultado = FactoryReportesFrecuencia.GetInformacionFrecuenciaRepository().GetReporteEventosFrecuencia();
            return resultado;
        }

        /// <summary>
        /// Permite ejecutar el envio de correos
        /// </summary>
        public void EnvioCorreoReporteFrecuenciaDesviacion()
        {
            try
            {
                List<InformacionFrecuenciaDTO> entitys = new List<InformacionFrecuenciaDTO>();
                entitys = this.GetReporteFrecuenciaDesviacion();


                //UtilCortoPlazo.EnviarCorreoEjecucionReprocesoMasivo(entitys, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }


    }
}
