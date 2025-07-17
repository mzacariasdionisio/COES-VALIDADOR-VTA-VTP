using System;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using COES.WebService.Tramite.Contratos;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.General.Helper;
using System.Threading.Tasks;
using System.ServiceModel.Activation;
using System.Linq;
using COES.Servicios.Aplicacion.Equipamiento;
using System.Globalization;
using log4net;

namespace COES.WebService.Tramite.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class TramiteServicio : ITramiteServicio
    {
        //- DEMANDA

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(TramiteServicio));
   
        /// <summary>
        /// 
        /// </summary>
        public TramiteServicio()
        {
            log4net.Config.XmlConfigurator.Configure();

        }
        public int EnviarNotificacionTramiteVirtual(string expediente, string tipo)
        {
            try
            {
                log.Info("Servicio EnviarNotificacionTramiteVirtual " + expediente + "  - " + tipo);
                int result;
                result = (new TramiteVirtualAppServicio()).EnviarNotificacionPortalTramite(int.Parse(expediente), int.Parse(tipo));
                log.Info("Result EnviarNotificacionTramiteVirtual " + result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Servicio EnviarNotificacionTramiteVirtual", ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<SiEmpresaCorreoDTO> ObtenerCorreosNotificacion(string ruc)
        {
            try
            {
                log.Info("Servicio ObtenerCorreosNotificacion " + ruc);
                List<SiEmpresaCorreoDTO> result = new List<SiEmpresaCorreoDTO>();
                result = (new TramiteVirtualAppServicio()).ObtenerCorreosNotificacion(ruc);
                log.Info("Result ObtenerCorreosNotificacion " + result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Servicio ObtenerCorreosNotificacion", ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<String> ObtenerListaCorreosPorTipo(string ruc, string tipo)
        {            
            try
            {
                log.Info("Servicio ObtenerListaCorreosPorTipo " + ruc + " - " + tipo);
                List<String> result = new List<String>();
                result = (new TramiteVirtualAppServicio()).ObtenerListaCorreosNotificacion(ruc, tipo);
                log.Info("Result ObtenerListaCorreosPorTipo " + result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Servicio ObtenerListaCorreosPorTipo", ex);
                throw new Exception(ex.Message, ex);
            }

        }
    }
}