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
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Framework.Base.Tools;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Servicio que expone funcionalidad de puntos de medición
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class PuntoMedicionServicio : IPuntoMedicionServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PuntoMedicionServicio));
        /// <summary>
        /// Constructor
        /// </summary>
        public PuntoMedicionServicio()
        {
            log4net.Config.XmlConfigurator.Configure();

        }

        List<MeHojaptomedDTO> IPuntoMedicionServicio.GetPuntosMedicion(int emprcodi, int formatcodi)
        {
            if (formatcodi < 1)
            {
                throw new FaultException(new FaultReason("El código de formato no es válido"), new FaultCode("1"));
            }
            if (emprcodi < 1)
            {
                throw new FaultException(new FaultReason("El código de empresa no es válido"), new FaultCode("2"));
            }
            var idCfgFormato = 0;
            List<MeHojaptomedDTO> listaPto = null;

            try
            {
                FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
                MeFormatoDTO formato = servFormato.GetByIdMeFormato(formatcodi);
                var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();

                string mes = DateTime.Today.Month < 10 ? "0" + DateTime.Today.Month.ToString() + " " + DateTime.Today.Year: DateTime.Today.Month.ToString() + " " + DateTime.Today.Year;

                DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
                DateTime fechaIni = fechaProceso;
                DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
                formato.FechaFin = fechaFin;
                listaPto = servFormato.GetListaPtos(formato.FechaFin, idCfgFormato, emprcodi, formatcodi, cabecera.Cabquery);

                if (listaPto == null || listaPto.Count == 0)
                {
                    throw new FaultException(new FaultReason("La empresa no tiene puntos de medición para este formato"), new FaultCode("3"));
                }
            }
            catch (Exception ex)
            {
                log.Error("GetPuntosMedicion", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al consultar puntos de medición."), new FaultCode("4"));
            }
            return listaPto;
        }

        EqEquipoDTO IPuntoMedicionServicio.GetEquipoPorPuntoMedicion(int ptomedicodi)
        {
            if (ptomedicodi < 1)
            {
                throw new FaultException(new FaultReason("El código de punto de medición no es válido"), new FaultCode("1"));
            }
            EqEquipoDTO equipo = null;

            try
            {
                EquipamientoAppServicio equipoServ = new EquipamientoAppServicio();
                var equipos = equipoServ.ListarEquiposPorPuntoMedicion(ptomedicodi);
                if (equipos == null || equipos.Count == 0)
                {
                    throw new FaultException(new FaultReason("El punto de medición no tiene equipos asociados"), new FaultCode("2"));
                }
                equipo = equipos.Where(t => t.Famcodi == 2 || t.Famcodi == 3 || t.Famcodi == 4 || t.Famcodi == 5 || t.Famcodi == 36 || t.Famcodi == 37 || t.Famcodi == 38 || t.Famcodi == 39).FirstOrDefault();
                if (equipo == null)
                {
                    throw new FaultException(new FaultReason("El punto de medición no tiene equipos asociados"), new FaultCode("2"));
                }
            }
            catch (Exception ex)
            {
                log.Error("GetEquipoPorPuntoMedicion", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al consultar equipo."), new FaultCode("3"));
            }

            return equipo;
        }

        List<MeTipopuntomedicionDTO> IPuntoMedicionServicio.GetTipoPuntoMedicionPorTipoInformacion(int tipoinfocodi)
        {
            FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
            List<MeTipopuntomedicionDTO> tipos = null;
            try
            {
                tipos = servFormato.ListarTiposPuntoMedicionPorTipoInformacion(tipoinfocodi);
                if (tipos == null)
                {
                    throw new FaultException(new FaultReason("No existen tipos de punto de medición para el tipo de información consultado."), new FaultCode("1"));
                }
            }
            catch (Exception ex)
            {
                log.Error("GetTipoPuntoMedicionPorTipoInformacion", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al consultar tipos de punto de medición."), new FaultCode("2"));
            }
            return tipos;
        }
    }
}