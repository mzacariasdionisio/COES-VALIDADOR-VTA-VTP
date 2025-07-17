using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.GMM
{
    public class GarantiaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GarantiaAppServicio));

        /// <summary>
        /// Inserta un registro de la tabla ...
        /// </summary>
        public int Save(GmmGarantiaDTO entity)
        {
            try
            {
                return FactorySic.GetGmmGarantiaRepository().Save(entity);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla garantia ...
        /// </summary>
        public int Update(GmmGarantiaDTO entity)
        {
            try
            {
                return FactorySic.GetGmmGarantiaRepository().Update(entity);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla garantia ...
        /// </summary>
        public bool Delete(GmmGarantiaDTO entity)
        {
            try
            {
                return FactorySic.GetGmmGarantiaRepository().Delete(entity);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtiene la lista de resultados del procesamiento del periodo 
        /// </summary>
        public GmmGarantiaDTO mensajeProcesamiento_x(int anio, string mes)
        {
            return FactorySic.GetGmmGarantiaRepository().mensajeProcesamiento(anio, mes);
        }

        /// <summary>
        /// Obtiene la lista de resultados del procesamiento del periodo por Participante 
        /// </summary>
        public List<GmmGarantiaDTO> mensajeProcesamientoParticipante(int anio, string mes)
        {
            return FactorySic.GetGmmGarantiaRepository().mensajeProcesamientoParticipante(anio, mes);
        }

        /// <summary>
        /// Leer información de energia para el periodo 
        /// </summary>
        public List<GmmValEnergiaDTO> ListarValores96Originales(GmmValEnergiaDTO valEnergiaDTO)
        {
            return FactorySic.GetGmmValEnergiaRepository().ListarValores96Originales(valEnergiaDTO);
        }

        /// <summary>
        /// Leer información de energia entregas para el periodo 
        /// </summary>
        public List<GmmValEnergiaEntregaDTO> ListarValores96OriginalesEntrega(GmmValEnergiaEntregaDTO valEnergiaDTO)
        {
            return FactorySic.GetGmmValEnergiaEntregaRepository().ListarValores96Originales(valEnergiaDTO);
        }
        /// <summary>
        /// Leer información de Costos Marginales
        /// </summary>
        public List<GmmValEnergiaDTO> ListarValoresCostoMarginal(GmmValEnergiaDTO valEnergiaDTO, int anio, int mes)
        {
            return FactorySic.GetGmmValEnergiaRepository().ListarValoresCostoMarginal(valEnergiaDTO, anio, mes);
        }
        /// <summary>
        /// Leer información de Costos Marginales para Entrega
        /// </summary>
        public List<GmmValEnergiaEntregaDTO> ListarValoresEntregaCostoMarginal(GmmValEnergiaEntregaDTO valEnergiaDTO, int anio, int mes)
        {
            return FactorySic.GetGmmValEnergiaEntregaRepository().ListarValoresCostoMarginal(valEnergiaDTO, anio, mes);
        }
        public void SaveValEnergiaOrigen(GmmValEnergiaDTO entity)
        {
            try
            {
                FactorySic.GetGmmValEnergiaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        public void SaveValEnergiaEntregaOrigen(GmmValEnergiaEntregaDTO entity)
        {
            try
            {
                FactorySic.GetGmmValEnergiaEntregaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpsertDatCalculo(GmmDatCalculoDTO entity)
        {
            try
            {
                FactorySic.GetGmmDatCalculoRepository().UpsertDatCalculo(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void Deletevalores(GmmEmpresaDTO agente)
        {
            FactorySic.GetGmmDatCalculoRepository().Deletevalores(agente);
        }

        public void DeletevaloresEntrega(GmmEmpresaDTO agente)
        {
            FactorySic.GetGmmDatCalculoRepository().DeletevaloresEntrega(agente);
        }

        public List<GmmDatCalculoDTO> ListarValores1Originales(GmmDatCalculoDTO datDC)
        {
            return FactorySic.GetGmmDatCalculoRepository().ListarValores1Originales(datDC);
        }

        public void SetPPO(int anio, int mes, int pericodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetPPO(anio, mes, pericodi, user);
        }

        public void SetValoresAdicionales(int anio, int mes, int pericodi, String user,
        
        decimal tipoCambio, decimal margenReserva, decimal totalInflex, decimal totalExceso)
        {
            FactorySic.GetGmmDatCalculoRepository().SetValoresAdicionales(anio, mes, pericodi, user,
                tipoCambio, margenReserva, totalInflex, totalExceso);
        }

        public void SetValorTipoCambio(int anio, int mes, int pericodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetValorTipoCambio(anio, mes, pericodi, user);
        }


        public void SetPerimsjpaso1(int pericodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetPerimsjpaso1(pericodi, user);
        }

        public void SetPerimsjpaso2(int pericodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetPerimsjpaso2(pericodi, user);
        }

        public void SetPeriEstado(int pericodi, String periestado, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetPeriEstado(pericodi, periestado, user);
        }

        public void SetDemandaCOES(int anio, int mes, int pericodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetDemandaCOES(anio, mes, pericodi, user);
        }

        public void SetPFR(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetPFR(anio, mes, pericodi, user, emprcodi);
        }
        public void SetLVTEA(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetLVTEA(anio, mes, pericodi, user, emprcodi);
        }
        public void SetMEEN10(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetMEEN10(anio, mes, pericodi, user, emprcodi);
        }
        public void SetVTP(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetVTP(anio, mes, pericodi, user, emprcodi);
        }

        public void SetPREG(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetPREG(anio, mes, pericodi, user, emprcodi);
        }
        public void SetMPE(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetMPE(anio, mes, pericodi, user, emprcodi);
        }
        public void SetENRG10(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetENRG10(anio, mes, pericodi, user, emprcodi);
        }
        public void SetENRG11(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetENRG11(anio, mes, pericodi, user, emprcodi);
        }
        public void SetENTPRE(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetEntPre(anio, mes, pericodi, user, emprcodi);
        }
        public void SetVDIO10(int anio, int mes, int pericodi, string user, int Emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetVDIO10(anio, mes, pericodi, user, Emprcodi);
        }
        public void SetVDER10(int anio, int mes, int pericodi, string user, int Emprcodi)
        {
            FactorySic.GetGmmDatCalculoRepository().SetVDER10(anio, mes, pericodi, user, Emprcodi);
        }

        public void SetGarantiaEnergia(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetGarantiaEnergia(anio, mes, pericodi, primerMes, Emprcodi, Empgcodi, user);
        }
        public void SetGarantiaPotenciaPeaje(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetGarantiaPotenciaPeaje(anio, mes, pericodi, primerMes, Emprcodi, Empgcodi, user);
        }
        public void SetGarantiaServiciosComp(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetGarantiaServiciosComp(anio, mes, pericodi, primerMes, Emprcodi, Empgcodi, user);
        }
        public void SetGarantiaEnergiaReactiva(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetGarantiaEnergiaReactiva(anio, mes, pericodi, primerMes, Emprcodi, Empgcodi, user);
        }
        public void SetGarantiainflexibilidadOpe(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {
            FactorySic.GetGmmDatCalculoRepository().SetGarantiainflexibilidadOpe(anio, mes, pericodi, primerMes, Emprcodi, Empgcodi, user);
        }

        public List<GmmDatCalculoDTO> listarRptEnergia(int anio, int mes)
        {
            return FactorySic.GetGmmDatCalculoRepository().listarRptEnergia(anio, mes);

        }
        public List<GmmDatCalculoDTO> listarRptInsumo(int anio, int mes)
        {
            return FactorySic.GetGmmDatCalculoRepository().listarRptInsumo(anio, mes);

        }

        public List<GmmDatCalculoDTO> listarRpt1(int anio, int mes)
        {
            return FactorySic.GetGmmDatCalculoRepository().listarRpt1(anio, mes);

        }

        public List<GmmDatCalculoDTO> listarRpt2(int anio, int mes)
        {
            return FactorySic.GetGmmDatCalculoRepository().listarRpt2(anio, mes);

        }
        public List<GmmDatCalculoDTO> listarRpt3(int anio, int mes)
        {
            return FactorySic.GetGmmDatCalculoRepository().listarRpt3(anio, mes);

        }
        public List<GmmDatCalculoDTO> listarRpt4(int anio, int mes)
        {
            return FactorySic.GetGmmDatCalculoRepository().listarRpt4(anio, mes);

        }
        public List<GmmDatCalculoDTO> listarRpt5(int anio, int mes)
        {
            return FactorySic.GetGmmDatCalculoRepository().listarRpt5(anio, mes);

        }
        public List<GmmDatCalculoDTO> listarRpt6(int anio, int mes)
        {
            return FactorySic.GetGmmDatCalculoRepository().listarRpt6(anio, mes);

        }


    }
}
