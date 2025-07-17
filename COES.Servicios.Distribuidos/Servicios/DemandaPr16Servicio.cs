using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using COES.Servicios.Distribuidos.Contratos;
using COES.Dominio.DTO.Sic;
using log4net;
using COES.Servicios.Aplicacion.DemandaMaxima;
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
    public class DemandaPr16Servicio : IDemandaPr16Servicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(DemandaPr16Servicio));
        private DemandaMaximaAppServicio demandaServicio = new DemandaMaximaAppServicio();
        /// <summary>
        /// 
        /// </summary>
        public DemandaPr16Servicio() {
            log4net.Config.XmlConfigurator.Configure();
            
        }
        /// <summary>
        /// Operación que retorna el listado de puntos de medición 
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<MeHojaptomedDTO> ConsultaPuntoMedicion(int emprcodi, string periodo)
        {
            List<MeHojaptomedDTO> puntosResult = new List<MeHojaptomedDTO>();
            FaultData fd = new FaultData();

            if (emprcodi <= 0)
            {

                fd.FaulCode = "1";
                fd.FaultString = "La empresa no existe, el código de empresa debe ser mayor a cero.";
                throw new FaultException(new FaultReason(fd.FaultString), new FaultCode(fd.FaulCode));
            }
            if (periodo.Trim().Length != 7)
            {
                throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("2"));
            }
            else
            {
                try
                {
                    int imes = Int32.Parse(periodo.Substring(0, 2));
                    int ianho = Int32.Parse(periodo.Substring(3, 4));
                }
                catch (Exception)
                {
                    throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("2"));
                }
            }

            puntosResult = demandaServicio.GetPuntosMedicionPR16(emprcodi, periodo);
            if (puntosResult.Count == 0)
            {
                throw new FaultException(new FaultReason("La empresa no tiene puntos de medición para el periodo consultado."), new FaultCode("3"));
            }
            return puntosResult;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="envioCodi"></param>
        /// <param name="emprCodi"></param>
        /// <param name="periodo"></param>
        public void EnvioNotificacion(int envioCodi, int emprCodi, string periodo,string usuario)
        {
            if (emprCodi <= 0)
            {
                throw new FaultException(new FaultReason("La empresa no existe, el código de empresa debe ser mayor a cero."), new FaultCode("1"));
            }
            if (periodo.Trim().Length != 7)
            {
                throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("2"));
            }
            else
            {
                try
                {
                    int imes = Int32.Parse(periodo.Substring(0, 2));
                    int ianho = Int32.Parse(periodo.Substring(3, 4));
                }
                catch (Exception)
                {
                    throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("2"));
                }
            }
            if (envioCodi <= 0)
            {
                throw new FaultException(new FaultReason("El código de envío no es válido."), new FaultCode("3"));
            }
            if (usuario.Trim().Length == 0)
            {
                throw new FaultException(new FaultReason("Identificador de usuario no válido."), new FaultCode("4"));
            }

            try
            {

                SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
                ModuloDTO modulo = seguridad.ObtenerModulo(14);
                List<string> emailsAdmin = modulo.ListaAdministradores.Select(x => x.UserEmail).ToList();
                var usuarioObj = seguridad.ObtenerUsuarioPorLogin(usuario);
                demandaServicio.EnviarNotificacionPR16(envioCodi, emprCodi, periodo, emailsAdmin, usuarioObj.UserEmail);
            }
            catch (Exception ex)
            {
                log.Error("EnvioNotificacion", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al enviar notificación"), new FaultCode("5"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="periodo"></param>
        /// <param name="usuario"></param>
        /// <param name="idEmpresa"></param>
        /// <returns>Código de envío generado</returns>
        public int GrabarValoresDemandaPr16(List<MeMedicion96DTO> entitys,string periodo, string usuario, int idEmpresa)
        {

            if (idEmpresa <= 0)
            {
                throw new FaultException(new FaultReason("La empresa no existe, el código de empresa debe ser mayor a cero."), new FaultCode("1"));
            }
            if (periodo.Trim().Length != 7)
            {
                throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("2"));
            }
            else
            {
                try
                {
                    int imes = Int32.Parse(periodo.Substring(0, 2));
                    int ianho = Int32.Parse(periodo.Substring(3, 4));
                }
                catch (Exception)
                {
                    throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("2"));
                }
            }
            if (usuario.Trim().Length == 0)
            {
                throw new FaultException(new FaultReason("Identificador de usuario no válido."), new FaultCode("3"));
            }
            try
            {
                return demandaServicio.GrabarValoresPR16(entitys, periodo, usuario, idEmpresa);
            }
            catch (Exception ex)
            {
                log.Error("GrabarValoresDemandaPr16", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al grabar datos"), new FaultCode("4"));
            }
                        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public MaximaDemandaDTO ObtenerDatosDemandaMaxima(string periodo)
        {
            ReporteMedidoresAppServicio servcio = new ReporteMedidoresAppServicio();
            MaximaDemandaDTO maxDemanda = new MaximaDemandaDTO();
            if (periodo.Trim().Length != 7)
            {
                throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("1"));
            }
            else
            {
                try
                {
                    int imes = Int32.Parse(periodo.Substring(0, 2));
                    int ianho = Int32.Parse(periodo.Substring(3, 4));
                }
                catch (Exception)
                {
                    throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("1"));
                }
            }
            try
            {
                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3, 4));
                DateTime fechaProceso = new DateTime(ianho, imes, 1);
                maxDemanda = servcio.GetDiaMaximaDemanda96XMes(fechaProceso,false);
                return maxDemanda;
            }
            catch (Exception ex)
            {
                log.Error("ObtenerDatosDemandaMaxima", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al obtener la demanda máxima"), new FaultCode("2"));
            }


            
        }
        /// <summary>
        /// Método que retorno los datos de demanda segpun el código de envío
        /// </summary>
        /// <param name="envioCodi">Código de envío</param>
        /// <returns>Listado de demanda por cada 15 min</returns>
        public List<MeMedicion96DTO> ObtenerRemision(int envioCodi)
        {
            List<MeMedicion96DTO> resultado = new List<MeMedicion96DTO>();
            if (envioCodi <= 0)
            {
                throw new FaultException(new FaultReason("El código de envío no es válido."), new FaultCode("1"));
            }
            //try
            //{
                resultado = demandaServicio.ObtenerDatosEnvio(envioCodi);

                if (resultado.Count == 0)
                {
                    throw new FaultException(new FaultReason("No existen datos para el envío consultado."), new FaultCode("3"));
                }

                return resultado;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("ObtenerRemision", ex);
            //    throw new FaultException(new FaultReason("Ocurrió un error al obtener datos de remisión"), new FaultCode("2"));
            //}
            
        }

        /// <summary>
        /// Método que determina si la empresa puede enviar información para determinado periodo
        /// </summary>
        /// <param name="emprCodi">Código de empresa</param>
        /// <param name="periodo">Periodo de envío</param>
        /// <returns></returns>
        public bool ValidarFechaPr16(int emprCodi, string periodo)
        {
            if (emprCodi <= 0)
            {
                throw new FaultException(new FaultReason("La empresa no existe, el código de empresa debe ser mayor a cero."), new FaultCode("1"));
            }
            if (periodo.Trim().Length != 7)
            {
                throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("2"));
            }
            else
            {
                try
                {
                    int imes = Int32.Parse(periodo.Substring(0, 2));
                    int ianho = Int32.Parse(periodo.Substring(3, 4));
                }
                catch (Exception)
                {
                    throw new FaultException(new FaultReason("El periodo debe estar en el formato 'MM YYYY'"), new FaultCode("2"));
                }
            }
            try
            {
                bool plazo = false;
                return demandaServicio.ValidarFechaPr16(emprCodi, periodo, out plazo);
            }
            catch (Exception ex)
            {
                log.Error("ValidarFechaPr16", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al validar fecha."), new FaultCode("3"));
            }
        }
        
    }
}