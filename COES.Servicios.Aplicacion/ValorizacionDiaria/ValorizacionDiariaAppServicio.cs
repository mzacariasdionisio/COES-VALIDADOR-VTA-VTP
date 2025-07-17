using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
using COES.Servicios.Aplicacion.ValorizacionDiaria.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace COES.Servicios.Aplicacion.ValorizacionDiaria
{
    /// <summary>
    /// Valrizaciones Diarias
    /// </summary>
    public class ValorizacionDiariaAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ValorizacionDiariaAppServicio));

        private DespachoAppServicio appDespacho = new DespachoAppServicio();
        private SubastasAppServicio appServicioSubastas = new SubastasAppServicio();
        private RsfAppServicio appServicioRSF = new RsfAppServicio();
        private CompensacionRSFAppServicio servicioCompensacionRsf = new CompensacionRSFAppServicio();
        List<VcrProvisionbaseDTO> ListaProvisionbase = new List<VcrProvisionbaseDTO>();
        List<SmaOfertaDetalleDTO> ListamayorOfertaSubida = new List<SmaOfertaDetalleDTO>();
        List<SmaOfertaDetalleDTO> ListamayorOfertaBajada = new List<SmaOfertaDetalleDTO>();
        List<EveRsfdetalleDTO> configuracion = new List<EveRsfdetalleDTO>();
        List<EveRsfhoraDTO> horas = new List<EveRsfhoraDTO>();
        List<EveRsfdetalleDTO> detalle = new List<EveRsfdetalleDTO>();
        public List<SiEmpresaDTO> ListarEmpresaTodo()
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerAgentesParticipantesMME();
        }

        #region FIT - 5.1 Monto Por Energia
        /// <summary>
        /// FIT - Obtener Energia Prevista
        /// </summary>
        /// <returns></returns>
        public List<VtdValorizacionDetalleDTO> ObtenerEnergiaPrevista(DateTime date)
        {
            return FactoryTransferencia.GetVtdValorizacionDetalle().ObtenerEnergiaPrevista(date);
        }
        /// <summary>
        /// FIT - Obtener Energia Prevista a Retirar del Participante
        /// </summary>
        /// <param name="date"></param>
        /// <param name="emprcodi"></param>
        /// <param name="tpEPR"></param>
        /// <returns></returns>
        public decimal GetEPR(DateTime date, int emprcodi, int tpEPR)
        {
            return FactoryTransferencia.GetVtdValorizacionDetalle().ObtenerEnergiaPrevistaRetirar(date, emprcodi, tpEPR);
        }

        /// <summary>
        /// Permite obtener los retiros previstros de energia totales por dia
        /// </summary>
        /// <param name="date"></param>
        /// <param name="tpEPR"></param>
        /// <returns></returns>
        public decimal GetEPRTotal(DateTime date, int tpEPR)
        {
            return FactoryTransferencia.GetVtdValorizacionDetalle().ObtenerEnergiaPrevistaRetirarTotal(date, tpEPR);
        }

        /// <summary>
        /// FIT - Obtener Entregas del Participante
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<VtdValorizacionDetalleDTO> ObtenerEntregasParticipante(DateTime date)
        {
            return FactoryTransferencia.GetVtdValorizacionDetalle().ObtenerEntregaParticipante(date);
        }
        /// <summary>
        /// FIT - Obtener Entregas del Participante por empresas
        /// </summary>
        /// <param name="date"></param>
        /// <param name="participantes"></param>
        /// <returns></returns>
        public List<VtdValorizacionDetalleDTO> ObtenerEnergiaEntregadaByEmpresas(DateTime date, string participantes)
        {
            return FactoryTransferencia.GetVtdValorizacionDetalle().ObtenerEnergiaEntregadaByEmpresas(date, participantes);
        }
        /// <summary>
        /// FIT - Procesa los Montos por Energia
        /// </summary>
        /// <param name="date"></param>
        /// <param name="valorizacion"></param>
        /// <returns></returns>
        public ValorizacionDTO ProcesarMontosPorEnergia(DateTime date, ValorizacionDTO valorizacion, List<SiEmpresaDTO> participantes = null)
        {
            try
            {
                List<VtdValorizacionDetalleDTO> empresasPorEntrega = new List<VtdValorizacionDetalleDTO>();
                if (participantes == null)
                {
                    empresasPorEntrega = ObtenerEntregasParticipante(date);
                }
                else
                {
                    string participantesCodis = ListSieEmpresaDTOtoEMPRCODIS(participantes);
                    empresasPorEntrega = ObtenerEnergiaEntregadaByEmpresas(date, participantesCodis);
                }

                List<VtdValorizacionDetalleDTO> empresasPorRetiro = ObtenerEnergiaPrevista(date);

                foreach (VtdValorizacionDetalleDTO participante in valorizacion.detalle)
                {
                    if (empresasPorEntrega.Find(x => x.Emprcodi == participante.Emprcodi) != null)
                    {
                        participante.Valdentrega = empresasPorEntrega.Find(x => x.Emprcodi == participante.Emprcodi).Valdentrega;
                    }

                    // Regla de negocio Plazo Energía prevista a retirar diaria. 
                    //- En caso de que el Agente no comunique dicha información en el plazo 
                    //    indicado (plazo a ser configurable por el Usuario STR), previo al día de la valorización, 
                    //    se considerará que el Participante, para el cálculo de garantías, no efectuará 
                    //    Retiro alguno del MME.
                    if (empresasPorRetiro.Find(x => x.Emprcodi == participante.Emprcodi) != null)
                    {
                        participante.Valdretiro = empresasPorRetiro.Find(x => x.Emprcodi == participante.Emprcodi).Valdretiro;
                    }
                    else
                    {
                        participante.Valdretiro = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return valorizacion;
        }
        #endregion

        #region FIT - 5.2 Monto Por Capacidad
        /// <summary>
        /// FIT - Obtener Potencia FirmeRemunerable
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public List<VtdValorizacionDetalleDTO> ObtenerPotenciaFirmeRemunerable(int pericodi)
        {
            return FactoryTransferencia.GetVtdValorizacionDetalle().ObtenerPotenciaFirmeRemunerable(pericodi);
        }
        /// <summary>
        /// FIT - Obtener Margen Reserva
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerMargenReserva(DateTime fecha)
        {
            decimal MargenReserva = -1;
            var parametro = FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(fecha, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiMargenReserva);

            if (parametro.Formuladat != null) //CAMBIOS
                MargenReserva = decimal.Parse(parametro.Formuladat);

            return MargenReserva;
        }
        /// <summary>
        /// FIT - Obtener Demanda Coincidente
        /// </summary>
        /// <returns></returns>
        public List<VtdValorizacionDetalleDTO> ObtenerDemandaCoincidente(MeMedicion48DTO m48)
        {
            List<VtdValorizacionDetalleDTO> demandaCoincidente = new List<VtdValorizacionDetalleDTO>();

            decimal max = int.MinValue;
            int index = int.MinValue;
            DateTime fechaIntervalo = DateTime.Now;

            decimal[] data = new decimal[48];
            data[0] = Convert.ToDecimal(m48.H1);
            data[1] = Convert.ToDecimal(m48.H2);
            data[2] = Convert.ToDecimal(m48.H3);
            data[3] = Convert.ToDecimal(m48.H4);
            data[4] = Convert.ToDecimal(m48.H5);
            data[5] = Convert.ToDecimal(m48.H6);
            data[6] = Convert.ToDecimal(m48.H7);
            data[7] = Convert.ToDecimal(m48.H8);
            data[8] = Convert.ToDecimal(m48.H9);
            data[9] = Convert.ToDecimal(m48.H10);
            data[10] = Convert.ToDecimal(m48.H11);
            data[11] = Convert.ToDecimal(m48.H12);
            data[12] = Convert.ToDecimal(m48.H13);
            data[13] = Convert.ToDecimal(m48.H14);
            data[14] = Convert.ToDecimal(m48.H15);
            data[15] = Convert.ToDecimal(m48.H16);
            data[16] = Convert.ToDecimal(m48.H17);
            data[17] = Convert.ToDecimal(m48.H18);
            data[18] = Convert.ToDecimal(m48.H19);
            data[19] = Convert.ToDecimal(m48.H20);
            data[20] = Convert.ToDecimal(m48.H21);
            data[21] = Convert.ToDecimal(m48.H22);
            data[22] = Convert.ToDecimal(m48.H23);
            data[23] = Convert.ToDecimal(m48.H24);
            data[24] = Convert.ToDecimal(m48.H25);
            data[25] = Convert.ToDecimal(m48.H26);
            data[26] = Convert.ToDecimal(m48.H27);
            data[27] = Convert.ToDecimal(m48.H28);
            data[28] = Convert.ToDecimal(m48.H29);
            data[29] = Convert.ToDecimal(m48.H30);
            data[30] = Convert.ToDecimal(m48.H31);
            data[31] = Convert.ToDecimal(m48.H32);
            data[32] = Convert.ToDecimal(m48.H33);
            data[33] = Convert.ToDecimal(m48.H34);
            data[34] = Convert.ToDecimal(m48.H35);
            data[35] = Convert.ToDecimal(m48.H36);
            data[36] = Convert.ToDecimal(m48.H37);
            data[37] = Convert.ToDecimal(m48.H38);
            data[38] = Convert.ToDecimal(m48.H39);
            data[39] = Convert.ToDecimal(m48.H40);
            data[40] = Convert.ToDecimal(m48.H41);
            data[41] = Convert.ToDecimal(m48.H42);
            data[42] = Convert.ToDecimal(m48.H43);
            data[43] = Convert.ToDecimal(m48.H44);
            data[44] = Convert.ToDecimal(m48.H45);
            data[45] = Convert.ToDecimal(m48.H46);
            data[46] = Convert.ToDecimal(m48.H47);
            data[47] = Convert.ToDecimal(m48.H48);

            int loop = 0;
            foreach (decimal mmax in data)
            {
                loop++;
                if (mmax > max)
                {
                    max = mmax;
                    index = loop;
                }
            }

            index *= 2;

            return FactoryTransferencia.GetVtdValorizacionDetalle().ObtenerDemandaCoincidente(m48.Medifecha, index);
        }
        /// <summary>
        /// FIT - Obtener Precio Potencia
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public VtdValorizacionDTO ObtenerPrecioPotencia(int pericodi)
        {
            return FactoryTransferencia.GetVtdValorizacion().ObtenerPrecioPotencia(pericodi);
        }
        /// <summary>
        /// FIT - Procesa los Montos por Capacidad
        /// </summary>
        /// <param name="date"></param>
        /// <param name="valorizacion"></param>
        /// <returns></returns>
        public ValorizacionDTO ProcesarMontosPorCapacidad(DateTime date, ValorizacionDTO valorizacion)
        {
            int perianiomes = int.Parse(date.AddMonths(-1).ToString("yyyyMM"));
            PeriodoDTO periodo = FactorySic.GetTrnPeriodoRepository().GetByAnioMes(perianiomes);

            if (periodo == null)
            {
                throw new Exception("No existe el periodo", new Exception());
            }

            try
            {
                List<VtdValorizacionDetalleDTO> empresasPotenciaFirmeRemunerable = ObtenerPotenciaFirmeRemunerable(periodo.PeriCodi);
                decimal margenReserva = ObtenerMargenReserva(date);

                valorizacion.cabecera.Valomr = margenReserva;

                DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);


                MeMedicion48DTO intervaloPuntaMes = FactorySic.GetMeMedicion48Repository().ObtenerIntervaloPuntaMes(firstDayOfMonth, date);

                List<VtdValorizacionDetalleDTO> empresasdemandaCoincidente = ObtenerDemandaCoincidente(intervaloPuntaMes);

                decimal precioPotencia = Convert.ToDecimal(ObtenerPrecioPotencia(periodo.PeriCodi).Valopreciopotencia);
                valorizacion.cabecera.Valopreciopotencia = precioPotencia;

                if (empresasPotenciaFirmeRemunerable.Count > 0 || empresasdemandaCoincidente.Count > 0)
                {
                    foreach (VtdValorizacionDetalleDTO participante in valorizacion.detalle)
                    {
                        if (empresasPotenciaFirmeRemunerable.Find(x => x.Emprcodi == participante.Emprcodi) != null)
                        {
                            participante.Valdpfirremun = empresasPotenciaFirmeRemunerable.Find(x => x.Emprcodi == participante.Emprcodi).Valdpfirremun;
                        }

                        if (empresasdemandaCoincidente.Find(x => x.Emprcodi == participante.Emprcodi) != null)
                        {
                            participante.Valddemandacoincidente = empresasdemandaCoincidente.Find(x => x.Emprcodi == participante.Emprcodi).Valddemandacoincidente;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return valorizacion;
        }
        #endregion

        #region FIT - 5.3 Monto Por Peaje
        /// <summary>
        /// FIT - Obtener Precio Peaje Unitario
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public decimal ObtenerPrecioPeajeUnitario(int pericodi)
        {
            return FactoryTransferencia.GetVtdValorizacionDetalle().ObtenerPrecioPeaje(pericodi);
        }
        /// <summary>
        /// FIT - Procesa los Montos por Peaje
        /// </summary>
        /// <param name="date"></param>
        /// <param name="detalle"></param>
        /// <param name="valorizacion"></param>
        /// <returns></returns>
        public ValorizacionDTO ProcesarMontosPorPeaje(DateTime date, ValorizacionDTO valorizacion)
        {
            try
            {
                int perianiomes = int.Parse(date.AddMonths(-1).ToString("yyyyMM"));
                PeriodoDTO periodo = FactorySic.GetTrnPeriodoRepository().GetByAnioMes(perianiomes);
                decimal precioPeajeUnitario = ObtenerPrecioPeajeUnitario(periodo.PeriCodi);

                if (precioPeajeUnitario > 0)
                {
                    foreach (VtdValorizacionDetalleDTO participante in valorizacion.detalle)
                    {
                        participante.Valdpeajeuni = precioPeajeUnitario;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return valorizacion;
        }

        #endregion

        #region FIT - 5.4 Monto por Servicios Complementarios e Inflexibilidades Operativas
        /// <summary>
        /// FIT - Obtener Factor Reparto De Energia Semanal
        /// </summary>
        /// <returns></returns>
        public decimal[] ObtenerFactorReparto(DateTime date)
        {
            decimal[] factorReparto = new decimal[7];
            factorReparto[0] = decimal.Parse(FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoSabado).Formuladat);
            factorReparto[1] = decimal.Parse(FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoDomingo).Formuladat);
            factorReparto[2] = decimal.Parse(FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoLunes).Formuladat);
            factorReparto[3] = decimal.Parse(FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoMartes).Formuladat);
            factorReparto[4] = decimal.Parse(FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoMiercoles).Formuladat);
            factorReparto[5] = decimal.Parse(FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoJueves).Formuladat);
            factorReparto[6] = decimal.Parse(FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFactorRepartoViernes).Formuladat);

            return factorReparto;
        }
        /// <summary>
        /// FIT - Obtener Porcentaje Perdida
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerPorcentajePerdida(DateTime date)
        {
            return decimal.Parse(FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiPorcentajePerdida).Formuladat); ;
        }
        /// <summary>
        /// FIT - Obtener Retiros Mensuales
        /// </summary>
        /// <returns></returns>
        public List<VtdRetiroMensual> ObtenerRetirosMensuales(DateTime date)
        {
            decimal[] factorReparto = ObtenerFactorReparto(date);

            decimal porcentajePerdida = ObtenerPorcentajePerdida(date);

            List<VtdRetiroMensual> retirosMensuales = new List<VtdRetiroMensual>();


            for (int i = 0; i < (DateTime.DaysInMonth(date.Year, date.Month)); i++)
            {
                VtdRetiroMensual retiroMensual = new VtdRetiroMensual();

                retiroMensual.ReMeNDia = i + 1;
                DateTime dateInBucle = new DateTime(date.Year, date.Month, retiroMensual.ReMeNDia);
                retiroMensual.ReMeNDSe = NrDay(dateInBucle.DayOfWeek);
                retiroMensual.ReMeSem = dateInBucle;
                int nWeek = EPDate.f_numerosemana(dateInBucle);
                decimal demandaCOES = FactorySic.GetMeMedicionxintervaloRepository().GetDemandaMedianoPlazoCOES(nWeek, dateInBucle);
                retiroMensual.ReMeProD = demandaCOES * factorReparto[retiroMensual.ReMeNDSe - 1];
                retiroMensual.ReMeReDP = retiroMensual.ReMeProD * (1 - (porcentajePerdida / 100));
                retiroMensual.ReMeReDE = FactorySic.GetCmDemandatotalRepository().GetDemandaTotal(dateInBucle).Dematotal;
                retirosMensuales.Add(retiroMensual);
            }

            return retirosMensuales;
        }
        /// <summary>
        /// FIT - Obtener Calculo Fraccion Pago Mensual
        /// </summary>
        /// <param name="date"></param>
        /// <param name="retiroMensual"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public decimal ObtenerCalculoFraccionPagoMensual(DateTime date, List<VtdRetiroMensual> retiroMensual, int emprcodi)
        {
            decimal RMMEpi = ObtenerRmmepi(date, emprcodi);
            decimal RMMEpj = ObtenerRmmepj(date, emprcodi);

            decimal RSEINi = ObtenerRSEINi(date, retiroMensual);
            decimal RSEINj = ObtenerRSEINj(date, retiroMensual);

            decimal fpgm = 0;

            //Validar division 0
            if ((RSEINi + RSEINj) > 0)
            {
                fpgm = (RMMEpi + RMMEpj) / (RSEINi + RSEINj);
            }

            return fpgm;
        }

        /// <summary>
        /// FIT - suma de la Energia Prevista a Retirar del participante P desde el 1 del mes hasta el día de la valorización(Rmmepi)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public decimal ObtenerRmmepi(DateTime date, int emprcodi)
        {
            decimal RMMEpi = 0;

            for (int i = 1; i <= date.Day; i++)
            {
                RMMEpi += GetEPR(new DateTime(date.Year, date.Month, i), emprcodi, ConstantesValorizacionDiaria.EPRLECTCODIDIARIO);
            }

            return RMMEpi;
        }
        /// <summary>
        /// FIT - suma de la Energía Prevista a Retirar para los dias que restan del mes(Rmmepj)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public decimal ObtenerRmmepj(DateTime date, int emprcodi)
        {
            int daysofmonth = DateTime.DaysInMonth(date.Year, date.Month);
            decimal RMMEpj = 0;

            for (int i = date.AddDays(1).Day; i <= daysofmonth; i++)
            {
                RMMEpj += GetEPR(new DateTime(date.Year, date.Month, i), emprcodi, ConstantesValorizacionDiaria.EPRLECTCODITRIMESTRAL);
            }

            return RMMEpj;
        }
        /// <summary>
        /// FIT -Obtener RSEINi:suma de los retiros del día ejecutados desde el día 1 del mes hasta el día de la valorización
        /// </summary>
        /// <param name="date"></param>
        /// <param name="retiroMensual"></param>
        /// <returns></returns>
        public decimal ObtenerRSEINi(DateTime date, List<VtdRetiroMensual> retiroMensual)
        {
            decimal RSEINi = 0;
            for (int i = 1; i <= date.Day; i++)
            {
                RSEINi += retiroMensual[i - 1].ReMeReDE;
            }

            return RSEINi;
        }
        /// <summary>
        /// FIT -Obtener RSEINj:suma de los retiros del día previsto desde el día siguiente al día de la valorización hasta el último día del mes
        /// </summary>
        /// <param name="date"></param>
        /// <param name="retiroMensual"></param>
        /// <returns></returns>
        public decimal ObtenerRSEINj(DateTime date, List<VtdRetiroMensual> retiroMensual)
        {
            decimal RSEINj = 0;
            for (int i = date.AddDays(1).Day; i <= retiroMensual.Count; i++)
            {
                RSEINj += retiroMensual[i - 1].ReMeReDP;
            }

            return RSEINj;
        }

        /// <summary>
        /// FIT - Obtener los Calculos Monto por Servicios Complementarios e Inflexibilidades Operativas
        /// </summary>
        /// <param name="date"></param>
        /// <param name="detalle"></param>
        /// <returns></returns>      
        public List<VtdValorizacionDetalleDTO> ObtenerCalculoMontosSCeIO(DateTime date, List<VtdValorizacionDetalleDTO> detalle, List<VcrProvisionbaseDTO> ListaProvisionbase)
        {
            detalle = ObtenerPDsc(date, detalle, ListaProvisionbase);
            decimal MCio = ObtenerMCio(date);
            decimal PDiop = ObtenerPDiop();
            decimal MCsc = ObtenerMCsc();

            foreach (var item in detalle)
            {

                item.Valdpagoio = (item.Valdfpgm * MCio) + PDiop;
                item.Valdpagosc = (item.Valdfpgm * MCsc) + item.Valdpdsc;
            }

            return detalle;
        }

        /// <summary>
        /// FIT - Obtener MCio = suma de compensaciones térmicas
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal ObtenerMCio(DateTime date)
        {
            decimal sumaCompensacionesTermicas = 0;

            //Se calcula como la suma de compensaciones térmicas obtenidas del aplicativo que calcula los Costos Marginales 
            //que tengan los siguientes tipos de calificaciones “Potencia y Energía”, “Pruebas aleatorias”, “Mínima carga”, “Por maniobra” y “Por seguridad”.

            List<CmCompensacionDTO> compensasionesPotenciaEnergia = FactorySic.GetCmCompensacionRepository().GetCompensacionporCalificacion(date, ConstantesValorizacionDiaria.CalificacionPotenciaEnergia);

            foreach (var compensasion in compensasionesPotenciaEnergia)
            {
                sumaCompensacionesTermicas += compensasion.Compvalor;
            }

            List<CmCompensacionDTO> compensasionesPruebasAleatorias = FactorySic.GetCmCompensacionRepository().GetCompensacionporCalificacion(date, ConstantesValorizacionDiaria.CalificacionPruebasAleatrorias);

            foreach (var compensasion in compensasionesPruebasAleatorias)
            {
                sumaCompensacionesTermicas += compensasion.Compvalor;
            }

            List<CmCompensacionDTO> compensasionesMinimaCarga = FactorySic.GetCmCompensacionRepository().GetCompensacionporCalificacion(date, ConstantesValorizacionDiaria.CalificacionMinimaCarga);

            foreach (var compensasion in compensasionesMinimaCarga)
            {
                sumaCompensacionesTermicas += compensasion.Compvalor;
            }

            List<CmCompensacionDTO> compensasionesPorManiobra = FactorySic.GetCmCompensacionRepository().GetCompensacionporCalificacion(date, ConstantesValorizacionDiaria.CalificacionPorManiobra);

            foreach (var compensasion in compensasionesPorManiobra)
            {
                sumaCompensacionesTermicas += compensasion.Compvalor;
            }

            List<CmCompensacionDTO> compensasionesPorSeguridad = FactorySic.GetCmCompensacionRepository().GetCompensacionporCalificacion(date, ConstantesValorizacionDiaria.CalificacionPorSeguridad);

            foreach (var compensasion in compensasionesPorSeguridad)
            {
                sumaCompensacionesTermicas += compensasion.Compvalor;
            }

            return sumaCompensacionesTermicas;
        }

        /// <summary>
        /// FIT - ObtenerPDiop:0
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerPDiop()
        {
            return 0;
        }

        /// <summary>
        /// FIT - ObtenerMCsc:0
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerMCsc()
        {
            return 0;
        }

        /// <summary>
        /// FIT - ObtenerPDsc = (𝐶. 𝑂. +𝑅. 𝐴. +𝐶𝑜𝑚𝑝. 𝐶𝑜𝑠𝑡𝑜𝑠 𝑂𝑝𝑒𝑟. ) 𝑥 𝐹𝑎𝑐𝑡𝑜𝑟 p
        /// <param name="date"></param>
        /// <param name="detalle"></param>
        /// </summary>
        /// <returns></returns>
        public List<VtdValorizacionDetalleDTO> ObtenerPDsc(DateTime date, List<VtdValorizacionDetalleDTO> detalleVtd, List<VcrProvisionbaseDTO> ListaProvisionbase)
        {

            detalleVtd = ObtenerFactorP(date, detalleVtd);

            //𝑃𝐷𝑠𝑐 𝑝=(𝐶.𝑂.+𝑅.𝐴.+𝐶𝑜𝑚𝑝.𝐶𝑜𝑠𝑡𝑜𝑠 𝑂𝑝𝑒𝑟.) 𝑥 𝐹𝑎𝑐𝑡𝑜𝑟 𝑝
            var ValorSub = ObtenerRASub(date, ListaProvisionbase);

            //Obtiene Valor Reserva Asignada de Bajada * Of Max Bajada
            var ValorBaj = ObtenerRABaj(date, ListaProvisionbase);
            //Obtiene Mayor Oferta Subida
            #region Obtener Mayor Oferta                  
            decimal mayorOfertaSubida = CalcularMayorOfertaSubida(configuracion, horas, detalle, ListamayorOfertaSubida);
            decimal mayorOfertaBajada = CalcularMayorOfertaBajada(configuracion, horas, detalle, ListamayorOfertaBajada);
            #endregion
            //Caculo Reserva Asignada Diaria
            decimal RA = (ValorSub * mayorOfertaSubida) + (ValorBaj * mayorOfertaBajada);
            decimal CO = ObtenerCostoDeOportunidad(date);
            decimal CompCostosOper = ObtenerCompCostosOper(date); // viene de cm_compensacion
            foreach (var item in detalleVtd)
            {
                item.Valdpdsc = (CO + RA + CompCostosOper) * item.Valdfactorp;
            }

            return detalleVtd;
        }

        /// <summary>
        /// FIT - Obtener Costo de Oportunidad
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerCostoDeOportunidad(DateTime date)
        {
            //Costo de Oportunidad
            //Valor a ser ingresado por el usuario STR previo al inicio del mes del día de valorización(S /./ dia)

            return decimal.Parse(FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiCostoOportunidad).Formuladat); // CAMBIO
        }
      
        /// <summary>
        /// FIT - Obtener CompCostpsOper = suma de las compensacionestérmicas del día en valorización
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerCompCostosOper(DateTime date)
        {
            //Se calcula como la suma de las compensaciones térmicas del día en valorización, 
            //obtenidas del aplicativo que calcula los Costos Marginales con tipo de calificación “RSF”.

            List<CmCompensacionDTO> compensasiones = FactorySic.GetCmCompensacionRepository().GetCompensacionporCalificacion(date, ConstantesValorizacionDiaria.CalificacionRSF);
            decimal costoOper = 0;

            foreach (var compensasion in compensasiones)
            {
                costoOper += compensasion.Compvalor;
            }

            return costoOper;
        }

        /// <summary>
        /// FIT - Obtener mayor oferta de subida del dia del aplicativo de subastas
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<SmaOfertaDetalleDTO> ListaMayorOfertaSubida(DateTime date)
        {
            List<SmaOfertaDetalleDTO> ofertaDetalleDTOs = FactorySic.GetSmaOfertaDetalleRepository().ListByDateTipo(date, date, ConstantesSubasta.OfertipoDiaria, ConstantesSubasta.EstadoActivo); // Helper errado corrección en URSCODI

            //Tipo 1 hace referencia a las ofertas de subida
            ofertaDetalleDTOs = ofertaDetalleDTOs.Where(a => a.Ofdetipo == 1).ToList();

            return ofertaDetalleDTOs;
        }

        /// <summary>
        /// FIT - Obtener mayor oferta de bajada del dia del aplicativo de subastas
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<SmaOfertaDetalleDTO> ListaMayorOfertaBajada(DateTime date)
        {
            List<SmaOfertaDetalleDTO> ofertaDetalleDTOs = FactorySic.GetSmaOfertaDetalleRepository().ListByDateTipo(date, date, ConstantesSubasta.OfertipoDiaria, ConstantesSubasta.EstadoActivo); // Helper errado corrección en URSCODI

            //Tipo 2 hace referencia a las ofertas de bajada
            ofertaDetalleDTOs = ofertaDetalleDTOs.Where(a => a.Ofdetipo == 2).ToList();

            return ofertaDetalleDTOs;
        }
        
        /// <summary>
        /// FIT - Obtener𝑅𝐴 = ∑ 𝑅𝐴𝑖 𝑥 𝑂𝑓𝑚𝑎x
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerRA(DateTime date)
        {
            decimal RA = 0;
            decimal mayorOferta = 0;//ObtenerMayorOfertaBajada(date);

            List<EveRsfdetalleDTO> configuracion = appServicioRSF.ObtenerConfiguracion(date);
            List<List<string>> urss = new List<List<string>>();

            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(date);
            List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(date);

            foreach (EveRsfhoraDTO hora in horas)
            {
                List<EveRsfdetalleDTO> items = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi).ToList();

                List<string> lista = new List<string>();
                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    //- Modificación Movisoft 12022021
                    if (item.Grupotipo != ConstantesAppServicio.SI)
                    {
                        EveRsfdetalleDTO registro = items.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();
                        string valor = string.Empty;

                        if (registro != null)
                        {
                            if (registro.Rsfdetsub != null || registro.Rsfdetbaj != null)
                            {
                                double difHoras = ((DateTime)hora.Rsfhorfin - (DateTime)hora.Rsfhorinicio).TotalHours;

                                valor = ((
                                    ((registro.Rsfdetsub != null) ? (decimal)registro.Rsfdetsub : 0) +
                                    ((registro.Rsfdetbaj != null) ? (decimal)registro.Rsfdetbaj : 0)
                                    ) * (decimal)difHoras).ToString();

                                lista.Add(valor);
                            }
                        }
                    }
                    //- Fin modificación Movisoft 1202022021
                }
                if (lista != null)
                    urss.Add(lista);

            }
            decimal s = 0;
            foreach (var urs in urss)
            {
                decimal sumMagnitud = 0;
                foreach (var magnitud in urs)
                {
                    sumMagnitud += decimal.Parse(magnitud);
                }

                s = sumMagnitud;
                sumMagnitud = (sumMagnitud) / 24;

                RA += sumMagnitud * mayorOferta;

            }

            return RA;
        }

        /// <summary>
        /// FIT - Obtener𝑅𝐴Sub = ∑ RA Subida 𝑥 𝑂𝑓𝑚𝑎x Subida
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerRASub(DateTime date, List<VcrProvisionbaseDTO> ListaProvisionbase)
        {
            decimal RA = 0;
            List<List<string>> urss = new List<List<string>>();
         
            decimal mayorOferta = CalcularMayorOfertaSubida(configuracion, horas, detalle, ListamayorOfertaSubida);
            foreach (EveRsfhoraDTO hora in horas)
            {
                List<EveRsfdetalleDTO> items = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi).ToList();

                List<string> lista = new List<string>();
                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    //- Modificación Movisoft 12022021
                    if (item.Grupotipo != ConstantesAppServicio.SI)
                    {
                        EveRsfdetalleDTO registro = items.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();
                        string valor = string.Empty;

                        if (registro != null)
                        {
                            if (registro.Rsfdetsub != null)
                            {
                                double difHoras = ((DateTime)hora.Rsfhorfin - (DateTime)hora.Rsfhorinicio).TotalHours;
                                //ASSETEC 2022 - Incorporamos al calculo la Provisión base si corresponde al mismo GrupoCodi y si esta en vigencia
                                decimal dRSFSub = (decimal)registro.Rsfdetsub;
                                if (difHoras > 0)
                                {
                                    int iGrupoCodi = (int)registro.Grupocodi;
                                    VcrProvisionbaseDTO dtoPB = ListaProvisionbase.Where(x => x.Grupocodi == iGrupoCodi && x.Vcrpbperiodoini <= date && x.Vcrpbperiodofin >= date).FirstOrDefault();
                                    if (dtoPB != null)
                                    {
                                        dRSFSub = dRSFSub - dtoPB.Vcrpbpotenciabf;
                                    }
                                }
                                //valor = ((registro.Rsfdetsub != null ? (decimal)registro.Rsfdetsub : 0) * (decimal)difHoras).ToString();
                                valor = ((dRSFSub > 0) ? (dRSFSub * (decimal)difHoras) : 0).ToString();
                                //FIN
                                lista.Add(valor);
                            }
                        }
                    }
                    //- Fin modificación Movisoft 1202022021
                }
                if (lista != null)
                    urss.Add(lista);

            }
            decimal s = 0;
            foreach (var urs in urss)
            {
                decimal sumMagnitud = 0;
                foreach (var magnitud in urs)
                {
                    sumMagnitud += decimal.Parse(magnitud);
                }

                s = sumMagnitud;
                sumMagnitud = (sumMagnitud) / 24;

                RA += sumMagnitud;//* mayorOferta;

            }

            return RA;
        }

        /// <summary>
        /// FIT - Obtener𝑅𝐴Baj = ∑ RA Bajada 𝑥 𝑂𝑓𝑚𝑎x Bajada
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerRABaj(DateTime date, List<VcrProvisionbaseDTO> ListaProvisionbase)
        {
            decimal RA = 0;
            List<List<string>> urss = new List<List<string>>();
           
            decimal mayorOferta = CalcularMayorOfertaBajada(configuracion, horas, detalle, ListamayorOfertaBajada);
            foreach (EveRsfhoraDTO hora in horas)
            {
                List<EveRsfdetalleDTO> items = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi).ToList();

                List<string> lista = new List<string>();
                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    //- Modificación Movisoft 12022021
                    if (item.Grupotipo != ConstantesAppServicio.SI)
                    {
                        EveRsfdetalleDTO registro = items.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();
                        string valor = string.Empty;

                        if (registro != null)
                        {
                            if (registro.Rsfdetbaj != null)
                            {

                                double difHoras = ((DateTime)hora.Rsfhorfin - (DateTime)hora.Rsfhorinicio).TotalHours;

                                //ASSETEC 2022 - Incorporamos al calculo la Provisión base si corresponde al mismo GrupoCodi y si esta en vigencia
                                decimal dRSFBaj = (decimal)registro.Rsfdetbaj;
                                if (difHoras > 0)
                                {
                                    int iGrupoCodi = (int)registro.Grupocodi;
                                    VcrProvisionbaseDTO dtoPB = ListaProvisionbase.Where(x => x.Grupocodi == iGrupoCodi && x.Vcrpbperiodoini <= date && x.Vcrpbperiodofin >= date).FirstOrDefault();
                                    if (dtoPB != null)
                                    {
                                        dRSFBaj = dRSFBaj - dtoPB.Vcrpbpotenciabfb;
                                    }
                                }
                                //valor = ((registro.Rsfdetbaj != null ? (decimal)registro.Rsfdetbaj : 0) * (decimal)difHoras).ToString();
                                valor = ((dRSFBaj > 0) ? (dRSFBaj * (decimal)difHoras) : 0).ToString();
                                //FIN
                                lista.Add(valor);
                                //Calculo de horas rangos horarios

                            }
                        }
                    }
                    //- Fin modificación Movisoft 1202022021
                }
                if (lista != null)
                    urss.Add(lista);

            }
            decimal s = 0;
            foreach (var urs in urss)
            {
                decimal sumMagnitud = 0;
                foreach (var magnitud in urs)
                {
                    sumMagnitud += decimal.Parse(magnitud);
                }

                s = sumMagnitud;
                sumMagnitud = (sumMagnitud) / 24;

                RA += sumMagnitud;//* mayorOferta;

            }

            return RA;
        }

        /// <summary>
        /// Calcula la Mayor Oferta de Subida
        /// </summary>
        /// <param name="configuracion"></param>
        /// <param name="horas"></param>
        /// <param name="detalle"></param>
        /// <param name="ListamayorOferta"></param>
        /// <returns></returns>
        public decimal CalcularMayorOfertaSubida(List<EveRsfdetalleDTO> configuracion, List<EveRsfhoraDTO> horas, List<EveRsfdetalleDTO> detalle, List<SmaOfertaDetalleDTO> ListamayorOferta)
        {
            decimal mayorOferta = 0;
            horas = horas.Where(x => x.Indicador != 1).ToList();
            foreach (EveRsfhoraDTO hora in horas)
            {
                List<EveRsfdetalleDTO> items = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi).ToList();

                List<string> lista = new List<string>();
                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    if (item.Grupotipo != ConstantesAppServicio.SI)
                    {
                        EveRsfdetalleDTO registro = items.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();
                        string valor = string.Empty;

                        if (registro != null && ListaProvisionbase.Count > 0)
                        {
                            VcrProvisionbaseDTO provision = ListaProvisionbase.Where(x => x.Grupocodi == item.Grupocodi
                            && x.Vcrpbperiodoini <= hora.Rsfhorfecha && hora.Rsfhorfecha <= x.Vcrpbperiodofin).FirstOrDefault();
                            //VALIDACION PENDIENTE CONFIRMAR SI MAYOR A SU PROVISION BASE POR RANGO MEDIO HORARIO O LA SUMA DEL DIA
                            if (registro.Rsfdetsub != null && registro.Rsfdetsub > 0 && ((provision != null && registro.Rsfdetsub > provision.Vcrpbpotenciabf) || provision == null))
                            {
                                var listado = ListamayorOferta.Where(a => a.Urscodi == item.Grupocodi && a.Ofdeprecio != "0").ToList();
                                if (listado.Count > 0)
                                {

                                    bool validado = false;
                                    decimal aux = 0;
                                    foreach (var mayor in listado)
                                    {
                                        DateTime dateIni = DateTime.Parse(mayor.Ofdehorainicio, System.Globalization.CultureInfo.CurrentCulture);
                                        DateTime dateFin = DateTime.Parse(mayor.Ofdehorafin, System.Globalization.CultureInfo.CurrentCulture);
                                        DateTime dateInival = DateTime.Parse(((DateTime)hora.Rsfhorinicio).ToString("HH:mm"), System.Globalization.CultureInfo.CurrentCulture);
                                        DateTime dateFinval = DateTime.Parse(((DateTime)hora.Rsfhorfin).ToString("HH:mm"), System.Globalization.CultureInfo.CurrentCulture);

                                        if (mayor.Ofdehorainicio == ((DateTime)hora.Rsfhorinicio).ToString("HH:mm") && mayor.Ofdehorafin == ((DateTime)hora.Rsfhorfin).ToString("HH:mm"))
                                        {
                                            validado = true;
                                        }


                                        else if (dateIni <= dateInival && dateFinval <= dateFin && dateInival <= dateFin)
                                        {
                                            validado = true;
                                        }


                                        if (validado)
                                        {
                                            //var listaSumar = detalle.Where(x => x.Grupocodi == item.Grupocodi && x.Rsfdetsub > 0).ToList();
                                            //if ((listaSumar.Sum(a => a.Rsfdetsub) / 2) > provision.Vcrpbpotenciabf)
                                            aux = decimal.Parse(mayor.Ofdeprecio);
                                        }
                                        if (mayorOferta < aux)
                                        {
                                            mayorOferta = aux;
                                        }
                                    }

                                }

                            }
                        }
                    }
                }

            }
            if (mayorOferta > 0)
            {
                mayorOferta /= 30;
            }
            return mayorOferta;
        }

        /// <summary>
        /// Calcula la Mayor Oferta de bajada
        /// </summary>
        /// <param name="configuracion"></param>
        /// <param name="horas"></param>
        /// <param name="detalle"></param>
        /// <param name="ListamayorOferta"></param>
        /// <returns></returns>
        public decimal CalcularMayorOfertaBajada(List<EveRsfdetalleDTO> configuracion, List<EveRsfhoraDTO> horas, List<EveRsfdetalleDTO> detalle, List<SmaOfertaDetalleDTO> ListamayorOferta)
        {
            decimal mayorOferta = 0;
            horas = horas.Where(x => x.Indicador != 1).ToList();
            foreach (EveRsfhoraDTO hora in horas)
            {
                List<EveRsfdetalleDTO> items = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi).ToList();

                List<string> lista = new List<string>();
                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    if (item.Grupotipo != ConstantesAppServicio.SI)
                    {
                        EveRsfdetalleDTO registro = items.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();
                        string valor = string.Empty;

                        if (registro != null)
                        {
                            VcrProvisionbaseDTO provision = ListaProvisionbase.Where(x => x.Grupocodi == item.Grupocodi
                           && x.Vcrpbperiodoini <= hora.Rsfhorfecha && hora.Rsfhorfecha <= x.Vcrpbperiodofin).FirstOrDefault();
                            //VALIDACION PENDIENTE CONFIRMAR SI MAYOR A SU PROVISION BASE POR RANGO MEDIO HORARIO O LA SUMA DEL DIA
                            if (registro.Rsfdetbaj != null && registro.Rsfdetbaj > 0 && ((provision != null && registro.Rsfdetbaj > provision.Vcrpbpotenciabfb) || provision == null))
                            {
                                var listado = ListamayorOferta.Where(a => a.Urscodi == item.Grupocodi && a.Ofdeprecio != "0").ToList();
                                if (listado.Count > 0)
                                {
                                    bool validado = false;
                                    decimal aux = 0;
                                    foreach (var mayor in listado)
                                    {
                                        DateTime dateIni = DateTime.Parse(mayor.Ofdehorainicio, System.Globalization.CultureInfo.CurrentCulture);
                                        DateTime dateFin = DateTime.Parse(mayor.Ofdehorafin, System.Globalization.CultureInfo.CurrentCulture);
                                        DateTime dateInival = DateTime.Parse(((DateTime)hora.Rsfhorinicio).ToString("HH:mm"), System.Globalization.CultureInfo.CurrentCulture);
                                        DateTime dateFinval = DateTime.Parse(((DateTime)hora.Rsfhorfin).ToString("HH:mm"), System.Globalization.CultureInfo.CurrentCulture);

                                        if (mayor.Ofdehorainicio == ((DateTime)hora.Rsfhorinicio).ToString("HH:mm") && mayor.Ofdehorafin == ((DateTime)hora.Rsfhorfin).ToString("HH:mm"))
                                        {
                                            validado = true;
                                        }


                                        else if (dateIni <= dateInival && dateFinval <= dateFin && dateInival <= dateFin)
                                        {
                                            validado = true;
                                        }


                                        if (validado)
                                        {
                                            //var listaSumar = detalle.Where(x => x.Grupocodi == item.Grupocodi && x.Rsfdetbaj > 0).ToList();
                                            //if ((listaSumar.Sum(a => a.Rsfdetbaj) / 2) > provision.Vcrpbpotenciabf)
                                            aux = decimal.Parse(mayor.Ofdeprecio);
                                        }
                                        if (mayorOferta < aux)
                                        {
                                            mayorOferta = aux;
                                        }
                                    }

                                }

                            }
                        }
                    }
                }

            }
            if (mayorOferta > 0)
            {
                mayorOferta /= 30;
            }
            return mayorOferta;
        }

        /// <summary>
        /// FIT - Obtener FactorP
        /// </summary>
        /// <returns></returns>
        public List<VtdValorizacionDetalleDTO> ObtenerFactorP(DateTime date, List<VtdValorizacionDetalleDTO> detalle)
        {
            //Corresponde a la producción de energía del Participante p dividido entre la producción de energía del SEIN, evaluado para el día de valorización

            //La información de producción de energía diaria resulta de la sumatorias de la energía producida de todos los intervalos de 30 minutos del día
            //En la figura 12 se muestra la producción de energía de cada central Térmica e Hidraulica en la columna “Pg MW” y “Ph MW”, 

            //la cual deberá identificarse sólo aquellas que es titular el Participante p.

            //La producción de energía del SEIN se puede visualizar en la Figura 12, item “Resumen”, sumando los valores que corresponden a “Generación Termica” y “Generación Hidraulica”.

            CmDemandatotalDTO cmDemandatotal = FactorySic.GetCmDemandatotalRepository().GetDemandaTotal(date);

            decimal produccionSEIN = cmDemandatotal.Dematermica + cmDemandatotal.Demahidraulica;

            decimal prodGeneral = ObtenerProducionEnergia(date) / 2;
            decimal retiroParticipantes = GetEPRTotal(date, ConstantesValorizacionDiaria.EPRLECTCODIDIARIO);


            foreach (var item in detalle)
            {
                if (prodGeneral > 0)
                {
                    decimal retiroParticipante = GetEPR(date, item.Emprcodi, ConstantesValorizacionDiaria.EPRLECTCODIDIARIO);

                    item.Valdfactorp = (ObtenerProduccionEnergia(item.Emprcodi) + retiroParticipante) / (prodGeneral + retiroParticipantes);
                }
            }

            decimal ObtenerProduccionEnergia(int emprcodi)
            {
                List<CmGeneracionDTO> energiasProducida = FactorySic.GetCmGeneracionRepository().ListByEmpresa(emprcodi, date);
                decimal produccionEnergia = 0;

                foreach (var energiaProducida in energiasProducida)
                {
                    produccionEnergia += energiaProducida.Genevalor;
                }

                return produccionEnergia / 2;
            }

            return detalle;
        }


        decimal ObtenerProducionEnergia(DateTime date)
        {
            return FactorySic.GetCmGeneracionRepository().ProducionEnergiaByDate(date);
        }

        /// <summary>
        /// FIT - Procesa los Montos por Servicios Complementarios e Inflexibilidades Operativas
        /// </summary>
        /// <param name="date"></param>
        /// <param name="valorizacion"></param>
        /// <returns></returns>
        public ValorizacionDTO ProcesarMontosPorServiciosComplementariosInflexibilidadOperativa(DateTime date, ValorizacionDTO valorizacion, List<VcrProvisionbaseDTO> ListaProvisionbase)
        {
            List<VtdRetiroMensual> retiroMensual = ObtenerRetirosMensuales(date);

            foreach (var item in valorizacion.detalle)
            {
                item.Valdfpgm = ObtenerCalculoFraccionPagoMensual(date, retiroMensual, item.Emprcodi);
            }

            valorizacion.detalle = ObtenerCalculoMontosSCeIO(date, valorizacion.detalle, ListaProvisionbase);


            return valorizacion;
        }

        #endregion

        #region FIT - 5.5 Monto por Exceso de Consumo de Energia Reactiva
        /// <summary>
        /// FIT - Obtener Monto Exceso Consumo Energia Reactiva
        /// </summary>
        /// <param name="date"></param>
        /// <param name="detalle"></param>
        /// <returns></returns>
        public ValorizacionDTO ObtenerMontoExcesoConsumoEnergiaReactiva(DateTime date, ValorizacionDTO valorizacion)
        {

            valorizacion.detalle = ObtenerCargosPorConsumoExcesoEnergiaReactiva(date, valorizacion.detalle);
            valorizacion.detalle = ObtenerAporteAdicional(date, valorizacion.detalle);

            return valorizacion;
        }

        /// <summary>
        /// FIT - Obtener Cargos Por Consumo Exceso Energia Reactiva
        /// </summary>e
        /// <param name="date"></param>
        /// <param name="detalle"></param>
        /// <returns></returns>
        public List<VtdValorizacionDetalleDTO> ObtenerCargosPorConsumoExcesoEnergiaReactiva(DateTime date, List<VtdValorizacionDetalleDTO> detalle)
        {
            //Cargos por consumo en exceso de energía reactiva del Participante

            foreach (var item in detalle)
            {
                //FIT: tabla VTD_CARGOSENEREAC buscar por participante

                VtdCargoEneReacDTO energiaReactiva = FactoryTransferencia.GetVtdCargosEneReacRepository().GetMontoByEmpresa(item.Emprcodi, date);

                decimal cargoConsumo = 0;

                if (energiaReactiva != null)
                    cargoConsumo = energiaReactiva.Caermonto;

                item.Valdcargoconsumo = cargoConsumo;
            }
            return detalle;
        }

        /// <summary>
        /// FIT - Obtener Aporte Adicional = fpgm × AporteAd - ∑ (Comp.RT × fpgm)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="detalle"></param>
        /// <returns></returns>
        public List<VtdValorizacionDetalleDTO> ObtenerAporteAdicional(DateTime date, List<VtdValorizacionDetalleDTO> detalle)
        {
            decimal aporteAD = ObtenerAporteAD(date);

            foreach (var item in detalle)
            {
                //recorrer del día 1 hasta día -1
                decimal resultado = 0;
                for (int i = 1; i < date.Day; i++)
                {
                    DateTime fechaCompensacion = new DateTime(date.Year, date.Month, i);
                    decimal fpgm = ObtenerFpgm(fechaCompensacion, item.Emprcodi);

                    resultado += ObtenerCompensacionCVporRT(fechaCompensacion, item.Emprcodi) * fpgm;
                }

                item.Valdaportesadicional = (item.Valdfpgm * aporteAD) - resultado;

                if (item.Valdaportesadicional < 0)
                {
                    item.Valdaportesadicional = 0;
                }


            }
            return detalle;
        }

        /// <summary>
        /// FIT - Obtener Fpgm
        /// </summary>
        /// <param name="date"></param>
        /// <param name="participante"></param>
        /// <returns></returns>
        public decimal ObtenerFpgm(DateTime date, int participante)
        {
            decimal fpgm = 0;
            var valorizacionDetalle = FactoryTransferencia.GetVtdValorizacionDetalle().GetValorizacionDetalleporFechaParticipante(date, participante);

            if (valorizacionDetalle != null)
                fpgm = valorizacionDetalle.Valdfpgm;

            return fpgm;
        }

        /// <summary>
        ///  FIT - Obtener CompensacionCVporRT
        /// </summary>
        /// <param name="date"></param>
        /// <param name="participante"></param>
        /// <returns></returns>
        public decimal ObtenerCompensacionCVporRT(DateTime date, int participante)
        {
            //compensaciones térmicas con calificación del tipo “RT” del participante
            //para el día de la valorización, obtenida del aplicativo de Cálculo de los Costos Marginales

            decimal resultado = 0;

            List<CmCompensacionDTO> compensacion = FactorySic.GetCmCompensacionRepository().GetCompensacionporCalificacionParticipante(date, ConstantesValorizacionDiaria.CalificacionPorRT, participante);

            foreach (var item in compensacion)
            {
                resultado += item.Compvalor;
            }

            return resultado;
        }


        /// <summary>
        /// FIT - Obtener AporteAD
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal ObtenerAporteAD(DateTime date)
        {
            decimal aporteAD = 0;

            aporteAD = ObtenerCostoFueraDeBanda(date) +
                        ObtenerCostoOtrosEquipos(date) +
                        ObtenerCompensacionAcumuladaCVporRT(date) -
                        ObtenerFRECTotal(date);

            if (aporteAD < 0)
                return 0;
            else
                return aporteAD;
        }

        /// <summary>
        /// FIT - Obtener CompensacionAcumuladaCVporRT
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal ObtenerCompensacionAcumuladaCVporRT(DateTime date)
        {
            //se calculará como la suma de las compensaciones térmicas con calificación del tipo “RT”
            //para el día de la valorización, obtenida del aplicativo de Cálculo de los Costos Marginales

            decimal resultado = 0;
            for (int i = 1; i <= date.Day; i++)
            {
                DateTime fechaCompensacion = new DateTime(date.Year, date.Month, i);

                var listaCompensacion = FactorySic.GetCmCompensacionRepository().GetCompensacionporCalificacion(fechaCompensacion, ConstantesValorizacionDiaria.CalificacionPorRT);

                if (listaCompensacion != null)
                {
                    foreach (var item in listaCompensacion)
                    {
                        resultado += item.Compvalor;
                    }
                }

            }

            return resultado;
        }

        /// <summary>
        /// FIT - Obtener Costo Fuera De Banda
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal ObtenerCostoFueraDeBanda(DateTime date)
        {
            decimal costoFueraDeBanda = -1;

            var parametro = FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiCostoFueraBanda); // CAMBIO
            if (parametro != null)
                costoFueraDeBanda = decimal.Parse(parametro.Formuladat);

            return costoFueraDeBanda;
        }

        /// <summary>
        /// FIT - Obtener Costo Otros Equipos
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal ObtenerCostoOtrosEquipos(DateTime date)
        {
            decimal costoOtrosEquipos = -1;

            var parametro = FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiCostoOtrosEquipos); // CAMBIO
            if (parametro != null)
                costoOtrosEquipos = decimal.Parse(parametro.Formuladat);

            return costoOtrosEquipos;
        }

        /// <summary>
        /// FIT - Obtener FRECTotal
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal ObtenerFRECTotal(DateTime date)
        {
            decimal frecuenciaTotal = -1;

            var parametro = FactorySic.GetPrGrupodatRepository().ObtenerParametroValorizacion(date, ConstantesValorizacionDiaria.GrupoCodiParametro, ConstantesValorizacionDiaria.ConceptoCodiFRECTotal);
            if (parametro != null)
                frecuenciaTotal = decimal.Parse(parametro.Formuladat);

            return frecuenciaTotal;
        }
        #endregion

        #region FIT - Validaciones
        /// <summary>
        /// FIT - Guardar en LogProceso
        /// </summary>
        /// <param name="Valocodi"></param>
        /// <param name="Logpsucreacion"></param>
        /// <param name="Logplog"></param>
        /// <param name="Logptipo"></param>
        /// <param name="Logpestado"></param>
        /// cambios con el tipo de dato que se va entregar de void a int
        public void SaveLogProceso(int Valocodi, string Logpusucreacion, string Logplog, char Logptipo, char Logpestado, DateTime horaInicio)
        {
            try
            {
                VtdLogProcesoDTO entity = new VtdLogProcesoDTO
                {
                    Valocodi = Valocodi,
                    Logpfecha = DateTime.Now,
                    Logphorainicio = horaInicio,
                    Logphorafin = DateTime.Now,
                    Logpusucreacion = Logpusucreacion,
                    Logplog = Logplog,
                    Logptipo = Logptipo,
                    Logpestado = Logpestado,
                    Logpfeccreacion = DateTime.Now,
                    Logpusumodificacion = null,
                    Logpfecmodificacion = DateTime.Now
                };
                if (SaveLogProceso(entity) == -1)
                {
                    throw new Exception("Error al guardar en VTDLOGPROCESO");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// FIT - Validaciones = valida el proceso de Valorizaciones Diarias
        /// </summary>
        /// <param name="valocodi"></param>
        /// <param name="date"></param>
        /// <param name="tipo"></param>
        /// <param name="usuario"></param>
        /// <param name="participantes"></param>
        /// /// <param name="ListaProvisionbase"></param>
        private bool Validaciones(int valocodi, DateTime date, char tipo, string usuario, List<SiEmpresaDTO> participantes, List<VcrProvisionbaseDTO> ListaProvisionbase)
        {
            DateTime horaValidacion = DateTime.Now;
            bool resultado = true;
            #region Valida que Existan Los siguientes Parametros
            // Margen de reserva
            if (ObtenerMargenReserva(date) > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Margen de Reserva", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No Se obtuvo Margen de Reserva", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }

            //Factor de Reparto
            horaValidacion = DateTime.Now;
            decimal[] factorReparto = ObtenerFactorReparto(date);
            foreach (decimal factor in factorReparto)
            {
                if (factor > 0)
                {
                    SaveLogProceso(valocodi, usuario, "Se obtuvo Factor de Reparto ", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
                }
                else
                {
                    resultado = false;
                    SaveLogProceso(valocodi, usuario, "No Se obtuvo Factor de Reparto", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
                }
            }

            //Porcentaje de Perdida
            horaValidacion = DateTime.Now;
            if (ObtenerPorcentajePerdida(date) > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Porcentaje de Perdida", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No Se obtuvo Porcentaje de Perdida", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }

            //Costo de Oportunidad
            horaValidacion = DateTime.Now;
            if (ObtenerCostoDeOportunidad(date) > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Costo de Oportunidad", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No se obtuvo Costo de Oportunidad", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }

            //Cargos por Energia reactiva
            horaValidacion = DateTime.Now;
            foreach (var participante in participantes)
            {
                if (ObtenerMontoEnergiaReactivaPorParticipante(participante.Emprcodi, date).Caermonto > 0)
                {
                    SaveLogProceso(valocodi, usuario, "Se obtuvo Cargo por Energia Reactiva para " + participante.Emprnomb, tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
                }
                else
                {
                    SaveLogProceso(valocodi, usuario, "No se obtuvo Cargo por Energia Reactiva para " + participante.Emprnomb, tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
                }
            }

            //Costo Fuera de Banda
            horaValidacion = DateTime.Now;
            if (ObtenerCostoFueraDeBanda(date) > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Costo Fuera de Banda", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No Se obtuvo Costo Fuera de Banda", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }

            //Costo Otros Equipos
            horaValidacion = DateTime.Now;
            if (ObtenerCostoOtrosEquipos(date) >= 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Costo Otros Equipos", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No Se obtuvo Costo Otros Equipos", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }

            //FREC Total
            horaValidacion = DateTime.Now;
            if (ObtenerFRECTotal(date) > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo FRECTotal", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No Se obtuvo FRECTotal", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }
            #endregion

            #region Valida Informacion de Base de Datos

            //Entrega Participante
            horaValidacion = DateTime.Now;
            //if (tipo == ConstantesValorizacionDiaria.MANUAL)
            //{
            string participantesCodis = ListSieEmpresaDTOtoEMPRCODIS(participantes);

            if (ObtenerEnergiaEntregadaByEmpresas(date, participantesCodis).Count > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Entrega Participante", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                SaveLogProceso(valocodi, usuario, "No Se obtuvo Entrega Participante", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }
            //}
            //else if (tipo == ConstantesValorizacionDiaria.AUTOMATICO)
            //{
            //    if (ObtenerEntregasParticipante(date).Count > 0)
            //    {
            //        SaveLogProceso(valocodi, usuario, "Se obtuvo Entrega Participante", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            //    }
            //    else
            //    {
            //        SaveLogProceso(valocodi, usuario, "No Se obtuvo Entrega Participante", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            //    }
            //}


            //Precio Peaje Unitario
            horaValidacion = DateTime.Now;
            int perianiomes = int.Parse((date.AddMonths(-1)).ToString("yyyyMM"));
            PeriodoDTO periodo = new PeriodoDTO();
            periodo = FactorySic.GetTrnPeriodoRepository().GetByAnioMes(perianiomes);
            if (ObtenerPrecioPeajeUnitario(periodo.PeriCodi) > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Precio Peaje Unitario", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No Se obtuvo Precio Peaje Unitario", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }


            //Datos de la demanda total del SEIN obtenidos del PMPO
            horaValidacion = DateTime.Now;
            CmDemandatotalDTO cmDemandatotal = FactorySic.GetCmDemandatotalRepository().GetDemandaTotal(date);
            if (cmDemandatotal.Dematotal > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Demanda Total", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No Se obtuvo Demanda Total", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }

            //Datos de Reserva Asignada
            horaValidacion = DateTime.Now;
            decimal RASub = ObtenerRASub(date, ListaProvisionbase);
            if (RASub > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Reserva Asignada Subida", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No Se obtuvo Reserva Asignada Subida", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }
            decimal RAbaj = ObtenerRABaj(date, ListaProvisionbase);
            if (RAbaj > 0)
            {
                SaveLogProceso(valocodi, usuario, "Se obtuvo Reserva Asignada Bajada", tipo, ConstantesValorizacionDiaria.EXITO, horaValidacion);
            }
            else
            {
                resultado = false;
                SaveLogProceso(valocodi, usuario, "No Se obtuvo Reserva Asignada Bajada", tipo, ConstantesValorizacionDiaria.FALLIDO, horaValidacion);
            }
            #endregion

            return resultado;
        }
        #endregion

        #region FIT - Consultas

        /// <summary>
        /// FIT - Obtener la Lista para Monto por Energia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<VtdMontoPorEnergiaDTO> GetListFullByDateRangeME(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            return FactoryTransferencia.GetVtdMontoPorEnergiaRepository().GetListFullByDateRange(emprcodi, fechaInicio, fechaFinal);
        }
        /// <summary>
        /// FIT - Obtener la Lista paginada para Monto por Energia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<VtdMontoPorEnergiaDTO> GetListPageByDateRangeME(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactoryTransferencia.GetVtdMontoPorEnergiaRepository().GetListPageByDateRange(emprcodi, fechaInicio, fechaFinal, nroPage, pageSize);
        }


        /// <summary>
        /// FIT - Obtener la Lista para Monto por Capacidad
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public VtdMontoPorCapacidadDTO GetListFullByDateMC(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            return FactoryTransferencia.GetVtdMontoPorCapacidadRepository().GetListFullByDateRange(emprcodi, fechaInicio, fechaFinal);
        }
        /// <summary>
        /// FIT - Obtener la Lista paginada para Monto por Capacidad
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>        
        public List<VtdMontoPorCapacidadDTO> GetListPageByDateRangeMC(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactoryTransferencia.GetVtdMontoPorCapacidadRepository().GetListPageByDateRange(emprcodi, fechaInicio, fechaFinal, nroPage, pageSize);
        }


        /// <summary>
        /// FIT - Obtener la Lista para Monto por Peaje
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public VtdMontoPorPeajeDTO GetListFullByDateRangeMP(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            return FactoryTransferencia.GetVtdMontoPorPeajeRepository().GetListFullByDateRange(emprcodi, fechaInicio, fechaFinal);
        }
        /// <summary>
        /// FIT - Obtener la Lista paginada para Monto por Peaje
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<VtdMontoPorPeajeDTO> GetListPageByDateRangeMP(DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactoryTransferencia.GetVtdMontoPorPeajeRepository().GetListPageByDateRange(fechaInicio, fechaFinal, nroPage, pageSize);
        }


        /// <summary>
        /// FIT - Obtener la Lista paginada para Monto SCeIO
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<VtdMontoSCeIODTO> GetListFullByDateRangeSCeIO(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            var listado = FactoryTransferencia.GetVtdMontoSCeIORepository().GetListFullByDateRange(emprcodi, fechaInicio, fechaFinal);
            foreach (var item in listado)
            {
                item.Valoofmax = item.Valoofmax * 30;
                item.ValoofmaxBaj = item.ValoofmaxBaj * 30;
            }
            return listado;
        }
        /// <summary>
        /// FIT - Obtener la Lista paginada para Monto SCeIO
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<VtdMontoSCeIODTO> GetListPageByDateRangeSCeIO(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            var listado = FactoryTransferencia.GetVtdMontoSCeIORepository().GetListPageByDateRange(emprcodi, fechaInicio, fechaFinal, nroPage, pageSize);

            //Resultado Of Max Subir y Of Max Bajada multiplicado * 30 para visualizarlo por Soles MW/Mes
            foreach (var item in listado)
            {
                item.Valoofmax = item.Valoofmax * 30;
                item.ValoofmaxBaj = item.ValoofmaxBaj * 30;
            }

            return listado;
        }


        /// <summary>
        /// FIT - Obtener la Lista paginada para Monto por Exceso
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<VtdMontoPorExcesoDTO> GetListFullByDateRangeMEx(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            return FactoryTransferencia.GetVtdMontoPorExcesoRepository().GetListFullByDateRange(emprcodi, fechaInicio, fechaFinal);
        }
        /// <summary>
        /// FIT - Obtener la Lista paginada para Monto por Exceso
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<VtdMontoPorExcesoDTO> GetListPageByDateRangeMEx(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactoryTransferencia.GetVtdMontoPorExcesoRepository().GetListPageByDateRange(emprcodi, fechaInicio, fechaFinal, nroPage, pageSize);
        }


        /// <summary>
        ///  FIT - Obtener la Lista paginada de Log Proceso
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<VtdLogProcesoDTO> GetListFullByDateLogProceso(DateTime date)
        {
            return FactoryTransferencia.GetVtdLogProceso().GetListByDate(date);
        }
        /// <summary>
        /// FIT - Obtener Lista paginada de Log de Proceso
        /// </summary>
        /// <param name="date"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<VtdLogProcesoDTO> GetListPagedByDateLogProceso(DateTime date, int nroPage, int pageSize)
        {
            return FactoryTransferencia.GetVtdLogProceso().GetListPageByDate(date, nroPage, pageSize);
        }
        /// <summary>
        ///FIT - Guardar LogProceso
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SaveLogProceso(VtdLogProcesoDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetVtdLogProceso().Save(entity);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        ///FIT - Actualizar LogProceso
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateLogProceso(VtdLogProcesoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtdLogProceso().Update(entity);
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        ///FIT- Eliminar LogProceso
        /// </summary>
        /// <param name="Logpcodi"></param>
        /// <returns></returns>
        public int DeletelogProceso(int Logpcodi)
        {
            try
            {
                FactoryTransferencia.GetVtdLogProceso().Delete(Logpcodi);
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        ///FIT - Obtener LogProceso
        /// </summary>
        /// <param name="Logpcodi"></param>
        /// <returns></returns>
        public VtdLogProcesoDTO GetByIdLogProceso(int Logpcodi)
        {
            return FactoryTransferencia.GetVtdLogProceso().GetById(Logpcodi);
        }

        /// <summary>
        /// FIT - Obtener Lista de Cargos Consumo Exceso Energia Reactiva
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<VtdCargoEneReacDTO> GetListDateCER(int emprcodi)
        {
            return FactoryTransferencia.GetVtdCargosEneReacRepository().ListByParticipant(emprcodi);
        }
        /// <summary>
        /// FIT - Guardar Cargos Consumo Exceso Energia Reactiva
        /// </summary>
        /// <param name="entity"></param>
        public void GuardarVTDMER(VtdCargoEneReacDTO entity)
        {
            //return FactorySic.GetVtdCargosEneReacRepository().Save(entity);
            try
            {
                FactoryTransferencia.GetVtdCargosEneReacRepository().Delete(entity.Emprcodi, entity.Caerfecha);
                FactoryTransferencia.GetVtdCargosEneReacRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// FIT - Eliminar Cargos Consumo Exceso Energia Reactiva
        /// </summary>
        /// <param name="caercodi"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int DeleteVTDMER(int emprcodi, DateTime date)
        {
            try
            {
                FactoryTransferencia.GetVtdCargosEneReacRepository().Delete(emprcodi, date);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public int DeleteVTDMERByEmpresa(int emprcodi)
        {
            try
            {
                FactoryTransferencia.GetVtdCargosEneReacRepository().DeleteByEmpresa(emprcodi);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public List<VtdCargoEneReacDTO> ListVTDMER(DateTime date)
        {
            try
            {
                return FactoryTransferencia.GetVtdCargosEneReacRepository().List(date); ;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// FIT - Actualizar Cargos Consumo Exceso Energia Reactiva
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateVTDMER(VtdCargoEneReacDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtdCargosEneReacRepository().Update(entity);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// FIT - Actualizar ...
        /// </summary>
        /// <param name="delete"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public int UpdateVTDMERMONFEC(int delete, DateTime fecha)
        {
            try
            {
                FactoryTransferencia.GetVtdCargosEneReacRepository().UpdateByResultDate(delete, fecha);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// FIT - Obtener Energia Reactiva Por Participante
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="caerfecha"></param>
        /// <returns></returns>
        public VtdCargoEneReacDTO ObtenerMontoEnergiaReactivaPorParticipante(int emprcodi, DateTime caerfecha)
        {
            try
            {
                return FactoryTransferencia.GetVtdCargosEneReacRepository().GetMontoByEmpresa(emprcodi, caerfecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region FIT - Reportes

        /// <summary>
        ///FIT - Obtener Lista para Valorizaciones Diarias
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<ValorizacionDiariaDTO> GetListFullValorizacionDiariaByFilter(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            return FactoryTransferencia.GetValorizacionDiaria().GetListFullByFilter(emprcodi, fechaInicio, fechaFinal);
        }
        /// <summary>
        ///FIT - Obtener Lista paginada para Valorizaciones Diarias
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<ValorizacionDiariaDTO> GetListPagedValorizacionDiariaByFilter(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactoryTransferencia.GetValorizacionDiaria().GetListPageByFilter(emprcodi, fechaInicio, fechaFinal, nroPage, pageSize);
        }
        /// <summary>
        /// FIT - Obtener Montos Calculados por mes por cada Participante
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public ValorizacionDiariaDTO GetMontoCalculadoPorMesPorParticipante(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {

            return FactoryTransferencia.GetValorizacionDiaria().GetMontoCalculadoPorMesPorParticipante(emprcodi, fechaInicio, fechaFinal);

        }
        /// <summary>
        /// FIT - Obtener Lista Paginada para Reporte Informacion Prevista Remitida por el Participante
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="emprcodi"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> GetListPagedByFilterInformacionPRP(DateTime fechaInicio, DateTime fechaFinal, int emprcodi, int nroPage, int pageSize)
        {
            return FactorySic.GetMeMedicion96Repository().GetListPageByFilterInformacionPrevistaRPorParticipante(fechaInicio, fechaFinal, emprcodi, nroPage, pageSize);
        }
        /// <summary>
        /// FIT - Obtener Lista para Reporte Informacion Prevista Remitida por el Participante
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> GetListFullFilterInformacionPRP(DateTime fechaInicio, DateTime fechaFinal, int emprcodi)
        {
            return FactorySic.GetMeMedicion96Repository().GetListFullInformacionPrevistaRPorParticipante(fechaInicio, fechaFinal, emprcodi);
        }
        #endregion

        #region FIT - Valorizacion Diaria

        /// <summary>
        /// FIT - Valorizacion Diaria ...
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="empresas"></param>
        /// <param name="usuario"></param>
        /// <param name="email"></param>
        public bool ValorizacionDiaria(DateTime fecha, string usuario, string email, List<SiEmpresaDTO> empresas = null)
        {
            DateTime horaProceso = DateTime.Now;
            bool rtn = false;
            char tipoProceso = ConstantesValorizacionDiaria.MANUAL;

            ValorizacionDTO valorizacionPrincipal = new ValorizacionDTO()
            {
                cabecera = new VtdValorizacionDTO()
                {
                    Valoestado = ConstantesValorizacionDiaria.INACTIVO,
                    Valousucreacion = usuario,
                    Valofeccreacion = DateTime.Now,
                    Valofecha = DateTime.Now,
                    Valofecmodificacion = DateTime.Now,
                    Valousumodificacion = usuario
                },
                detalle = new List<VtdValorizacionDetalleDTO>()
            };

            if (empresas == null)
            {
                empresas = this.ObtenerEmpresasMME();
                tipoProceso = ConstantesValorizacionDiaria.AUTOMATICO;
            }

            foreach (SiEmpresaDTO empresa in empresas)
            {
                VtdValorizacionDetalleDTO vDetalleEmpresa = new VtdValorizacionDetalleDTO()
                {
                    Emprcodi = empresa.Emprcodi,
                    Emprnomb = empresa.Emprnomb
                };

                valorizacionPrincipal.detalle.Add(vDetalleEmpresa);
            }

            try
            {
                #region Cargar Configuraciones
                ListaProvisionbase = this.servicioCompensacionRsf.ListVcrProvisionbasesIndex();
                horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
                detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha);
                ListamayorOfertaSubida = ListaMayorOfertaSubida(fecha);
                ListamayorOfertaBajada = ListaMayorOfertaBajada(fecha);
                configuracion = appServicioRSF.ObtenerConfiguracion(fecha);
                #endregion


                int valoCodi = FactoryTransferencia.GetVtdValorizacion().Save(valorizacionPrincipal.cabecera);


                if (Validaciones(valoCodi, fecha, tipoProceso, usuario, empresas, ListaProvisionbase))
                {
                    //5.1
                    //if (tipoProceso == ConstantesValorizacionDiaria.MANUAL)
                    //{
                    valorizacionPrincipal = ProcesarMontosPorEnergia(fecha, valorizacionPrincipal, empresas);
                    //}
                    //else
                    //{
                    //    valorizacionPrincipal = ProcesarMontosPorEnergia(fecha, valorizacionPrincipal);
                    //}

                    //5.2
                    valorizacionPrincipal = ProcesarMontosPorCapacidad(fecha, valorizacionPrincipal);

                    //5.3
                    valorizacionPrincipal = ProcesarMontosPorPeaje(fecha, valorizacionPrincipal);

                    //5.4 
                    valorizacionPrincipal = ProcesarMontosPorServiciosComplementariosInflexibilidadOperativa(fecha, valorizacionPrincipal, ListaProvisionbase);

                    //5.5
                    valorizacionPrincipal = ObtenerMontoExcesoConsumoEnergiaReactiva(fecha, valorizacionPrincipal);

                    //registro de parametros
                    int nWeek = EPDate.f_numerosemana(fecha);
                    valorizacionPrincipal.cabecera.Valodemandacoes = FactorySic.GetMeMedicionxintervaloRepository().GetDemandaMedianoPlazoCOES(nWeek, fecha);

                    decimal[] factorReparto = ObtenerFactorReparto(fecha);

                    valorizacionPrincipal.cabecera.Valofactorreparto = Math.Round(factorReparto[NrDay(fecha.DayOfWeek) - 1], 5);

                    decimal PorcentajePerdida = ObtenerPorcentajePerdida(fecha);
                    valorizacionPrincipal.cabecera.Valoporcentajeperdida = Math.Round(PorcentajePerdida, 5);

                    valorizacionPrincipal.cabecera.Valofrectotal = Math.Round(ObtenerFRECTotal(fecha), 5);
                    valorizacionPrincipal.cabecera.Valootrosequipos = Math.Round(ObtenerCostoOtrosEquipos(fecha), 5);
                    valorizacionPrincipal.cabecera.Valocostofuerabanda = Math.Round(ObtenerCostoFueraDeBanda(fecha), 5);
                    valorizacionPrincipal.cabecera.Valocomptermrt = Math.Round(ObtenerCompensacionAcumuladaCVporRT(fecha), 5);

                    decimal CompCostosOper = Math.Round(ObtenerCompCostosOper(fecha), 5);
                    valorizacionPrincipal.cabecera.Valocompcostosoper = Math.Round(CompCostosOper, 5);

                    decimal CO = ObtenerCostoDeOportunidad(fecha);
                    valorizacionPrincipal.cabecera.Valoco = Math.Round(CO, 5);

                    //Obtiene Valor Reserva Asignada de Subida * Of Max Subida
                    var ValorSub = ObtenerRASub(fecha, ListaProvisionbase); //ASSETEC 202208 - Incorporando al calculo la Provisión Base

                    //Obtiene Valor Reserva Asignada de Bajada * Of Max Bajada
                    var ValorBaj = ObtenerRABaj(fecha, ListaProvisionbase);  //ASSETEC 202208 - Incorporando al calculo la Provisión Base
                    //Obtiene Mayor Oferta Subida
                    #region Obtener Mayor Oferta                  
                    decimal mayorOfertaSubida = CalcularMayorOfertaSubida(configuracion, horas, detalle, ListamayorOfertaSubida);
                    decimal mayorOfertaBajada = CalcularMayorOfertaBajada(configuracion, horas, detalle, ListamayorOfertaBajada);
                    #endregion
                    //Caculo Reserva Asignada Diaria
                    decimal RA = (ValorSub * mayorOfertaSubida) + (ValorBaj * mayorOfertaBajada);
                    valorizacionPrincipal.cabecera.Valora = Math.Round(RA, 5);

                    valorizacionPrincipal.cabecera.ValoraSub = Math.Round(ValorSub, 5);

                    valorizacionPrincipal.cabecera.ValoraBaj = Math.Round(ValorBaj, 5);


                    //decimal mayorOfertaSub = ObtenerMayorOfertaSubida(fecha);
                    valorizacionPrincipal.cabecera.Valoofmax = Math.Round(mayorOfertaSubida, 5);

                    //Obtiene Mayor Oferta Bajada
                    //decimal mayorOfertaBaj = ObtenerMayorOfertaBajada(fecha);
                    valorizacionPrincipal.cabecera.ValoofmaxBaj = Math.Round(mayorOfertaBajada, 5);

                    valorizacionPrincipal.cabecera.Valoestado = ConstantesValorizacionDiaria.ACTIVO;
                    valorizacionPrincipal.cabecera.Valofecha = fecha;
                    valorizacionPrincipal.cabecera.Valocodi = valoCodi;
                    valorizacionPrincipal.cabecera.Valousumodificacion = usuario;
                    valorizacionPrincipal.cabecera.Valofecmodificacion = DateTime.Now;


                    //Actualizacion
                    if (saveValorizacion(valorizacionPrincipal) > 0)
                    {
                        bool estado = true;
                        ValorizacionDiariaCorreoHelper.EnviarCorreo(fecha, email, estado);
                        SaveLogProceso(valorizacionPrincipal.cabecera.Valocodi, usuario, "Valorizacion Exitosa", tipoProceso, ConstantesValorizacionDiaria.EXITO, horaProceso);

                        rtn = true;
                    }
                    else
                    {
                        bool estado = false;
                        ValorizacionDiariaCorreoHelper.EnviarCorreo(fecha, email, estado);
                        SaveLogProceso(valorizacionPrincipal.cabecera.Valocodi, usuario, "Valorizacion Fallida", tipoProceso, ConstantesValorizacionDiaria.FALLIDO, horaProceso);
                    }
                }
                else
                {
                    bool estado = false;
                    ValorizacionDiariaCorreoHelper.EnviarCorreo(fecha, email, estado);
                    Logger.Error(ConstantesAppServicio.LogError, new Exception(ConstantesValorizacionDiaria.PROCESO_FALLIDO));
                    SaveLogProceso(valoCodi, usuario, "Valorizacion Fallida, No se cumplieron los requisitos: " + fecha.ToString(ConstantesBase.FormatoFecha), tipoProceso, ConstantesValorizacionDiaria.FALLIDO, horaProceso);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return rtn;
        }



        // CAMBIOS!!!!

        /// <summary>
        /// FIT - Valorizacion Diaria ...
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="participantes"></param>
        /// <param name="usuario"></param>
        /// <param name="email"></param>
        public bool ValorizacionDiaria(DateTime fecha, string usuario, string email, string participantes)
        {
            bool rtn = false;
            try
            {
                List<SiEmpresaDTO> empresas = FactorySic.GetSiEmpresaRepository().ListarEmpresasPorIds(participantes);

                rtn = ValorizacionDiaria(fecha, usuario, email, empresas);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return rtn;
        }
        /// <summary>
        ///FIT - Valorizacion Diaria Automatica
        /// </summary>
        public void ValorizacionDiariaAutomatica()
        {
            DateTime date = DateTime.Now.AddDays(-1);
            try
            {
                List<SiEmpresaDTO> empresas = this.ObtenerEmpresasMME();
                string usuarioProceso = ConstantesValorizacionDiaria.USUARIO_PROCESO;
                string emailProceso = "sme@coes.org.pe";

                foreach(SiEmpresaDTO empresa in empresas)
                {
                    List<SiEmpresaDTO> empresaList = new List<SiEmpresaDTO>();
                    empresaList.Add(empresa);
                    ValorizacionDiaria(date, usuarioProceso, emailProceso, empresaList);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region FIT - Proceso
        public void SaveHoraEjecucion(SiProcesoDTO entity)
        {
            FactorySic.GetSiProcesoRepository().Save(entity);
        }

        public void UpdateHoraEjecucion(SiProcesoDTO entity)
        {
            SiProcesoDTO entidad = FactorySic.GetSiProcesoRepository().GetById(ConstantesValorizacionDiaria.CodigoProcesoValorizacion);
            entidad.Prschorainicio = entity.Prschorainicio;
            entidad.Prscminutoinicio = entity.Prscminutoinicio;
            FactorySic.GetSiProcesoRepository().Update(entidad);
        }

        public string ObtenerHoraEjecucion()
        {
            SiProcesoDTO entidad = FactorySic.GetSiProcesoRepository().GetById(ConstantesValorizacionDiaria.CodigoProcesoValorizacion);

            return entidad.Prschorainicio.ToString().PadLeft(2, '0') + ":" + entidad.Prscminutoinicio.ToString().PadLeft(2, '0');
        }

        #endregion

        /// <summary>
        /// Inserta un registro de la tabla VTD_VALORIZACION y VTD_VALORIZACIONDETALLE
        /// </summary>
        /// <param name="valorizacion"></param>
        /// <returns></returns>
        public int saveValorizacion(ValorizacionDTO valorizacion)
        {
            int rtn = -1;
            IDbConnection conn = null;
            DbTransaction tran = null;
            bool resultVal = false;
            int resultValde = -1;

            try
            {
                conn = FactoryTransferencia.GetVtdValorizacion().BeginConnection();
                tran = FactoryTransferencia.GetVtdValorizacion().StartTransaction(conn);
                try
                {

                    FactoryTransferencia.GetVtdValorizacion().UpdateEstado(valorizacion.cabecera, conn, tran, valorizacion.detalle[0].Emprcodi.ToString());
                }
                catch
                {

                    FactoryTransferencia.GetVtdValorizacion().UpdateEstado(valorizacion.cabecera, conn, tran, "");
                }
                resultVal = FactoryTransferencia.GetVtdValorizacion().Update(valorizacion.cabecera, conn, tran);

                if (resultVal)
                {
                    int id = FactoryTransferencia.GetVtdValorizacionDetalle().GetMaxId();
                    foreach (var detalle in valorizacion.detalle)
                    {
                        detalle.Valdcodi = id;
                        detalle.Valocodi = valorizacion.cabecera.Valocodi;
                        detalle.Valdaportesadicional = Math.Round(detalle.Valdaportesadicional, 5);
                        detalle.Valdcargoconsumo = Math.Round(detalle.Valdcargoconsumo, 5);
                        detalle.Valddemandacoincidente = Math.Round(detalle.Valddemandacoincidente, 5);
                        detalle.Valdentrega = Math.Round(detalle.Valdentrega, 5);
                        detalle.Valdfactorp = Math.Round(detalle.Valdfactorp, 5);
                        detalle.Valdfpgm = Math.Round(detalle.Valdfpgm, 5);
                        detalle.Valdmcio = Math.Round(detalle.Valdmcio, 5);
                        detalle.Valdmoncapacidad = Math.Round(detalle.Valdmoncapacidad, 5);
                        detalle.Valdpagoio = Math.Round(detalle.Valdpagoio, 5);
                        detalle.Valdpagosc = Math.Round(detalle.Valdpagosc, 5);
                        detalle.Valdpdsc = Math.Round(detalle.Valdpdsc, 5);
                        detalle.Valdpeajeuni = Math.Round(detalle.Valdpeajeuni, 5);
                        detalle.Valdpfirremun = Math.Round(detalle.Valdpfirremun, 5);
                        detalle.Valdretiro = Math.Round(detalle.Valdretiro, 5);
                        detalle.Valdusucreacion = valorizacion.cabecera.Valousucreacion;
                        detalle.Valdfeccreacion = valorizacion.cabecera.Valofeccreacion;
                        detalle.Valdusumodificacion = valorizacion.cabecera.Valousumodificacion;
                        detalle.Valdfecmodificacion = valorizacion.cabecera.Valofecmodificacion;

                        resultValde = FactoryTransferencia.GetVtdValorizacionDetalle().Save(detalle, conn, tran);
                        id += 1;
                        if (resultValde == -1)
                        {
                            tran.Rollback();
                            Logger.Error("Error insertando registro en Tabla VtdValorizacionDetalle!...");
                            throw new Exception("Error insertando registro en Tabla VtdValorizacionDetalle...");
                        }
                    }

                    tran.Commit();
                    rtn = 1;
                }
                else
                {
                    tran.Rollback();
                    Logger.Error("Error insertando registro en Tabla VtdValorizacion!...");
                    throw new Exception("Error insertando registro en Tabla VtdValorizacion!...");
                }

            }
            catch (Exception ex)
            {
                if (tran != null) tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);

            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            return rtn;
        }

        /// <summary>
        /// Convierte una lista de empresas en un string de sus codigos separados por ','
        /// </summary>
        /// <param name="empresas"></param>
        /// <returns></returns>
        public string ListSieEmpresaDTOtoEMPRCODIS(List<SiEmpresaDTO> empresas)
        {
            string empresasCodis = string.Empty;

            for (int i = 0; i < empresas.Count; i++)
            {
                empresasCodis += empresas[i].Emprcodi;

                if (i + 1 != empresas.Count)
                {
                    empresasCodis += ",";
                }
            }

            return empresasCodis;
        }


        public int NrDay(DayOfWeek dayOfWeek)
        {
            int nDay = 0;
            switch (dayOfWeek)
            {
                case DayOfWeek.Saturday:
                    nDay = 1;
                    break;
                case DayOfWeek.Sunday:
                    nDay = 2;
                    break;
                case DayOfWeek.Monday:
                    nDay = 3;
                    break;
                case DayOfWeek.Tuesday:
                    nDay = 4;
                    break;
                case DayOfWeek.Wednesday:
                    nDay = 5;
                    break;
                case DayOfWeek.Thursday:
                    nDay = 6;
                    break;
                case DayOfWeek.Friday:
                    nDay = 7;
                    break;
            }
            return nDay;
        }

        /// <summary>
        /// Permite obtener las empreas por formatos
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasMME()
        {
            return FactoryTransferencia.GetVtdValorizacion().ObtenerEmpresas();
        }
    }
}
