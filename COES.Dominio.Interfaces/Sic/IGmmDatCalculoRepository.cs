using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IGmmDatCalculoRepository
    {
        void UpsertDatCalculo(GmmDatCalculoDTO entity); 
        List<GmmDatCalculoDTO> ListarValores1Originales(GmmDatCalculoDTO datDC);
        void Deletevalores(GmmEmpresaDTO agente);
        void DeletevaloresEntrega(GmmEmpresaDTO agente);
        void SetPPO(int anio, int mes, int pericodi, String user);
        void SetValoresAdicionales(int anio, int mes, int pericodi, String user,
                         decimal tipoCambio, decimal margenReserva, decimal totalInflex, decimal totalExceso);
        void SetValorTipoCambio(int anio, int mes, int pericodi, String user);
        void SetLVTEA(int anio, int mes, int pericodi, String user, int emprcodi); 
        void SetMEEN10(int anio, int mes, int pericodi, String user, int emprcodi);
        void SetPFR(int anio, int mes, int pericodi, String user, int emprcodi);
        void SetVTP(int anio, int mes, int pericodi, String user, int emprcodi);
        void SetPREG(int anio, int mes, int pericodi, String user, int emprcodi);
        void SetMPE(int anio, int mes, int pericodi, String user, int emprcodi);
        void SetENRG10(int anio, int mes, int pericodi, String user, int emprcodi);
        void SetENRG11(int anio, int mes, int pericodi, String user, int emprcodi);
        void SetEntPre(int anio, int mes, int pericodi, String user, int emprcodi);
        void SetDemandaCOES(int anio, int mes, int pericodi, String user);
        void SetVDIO10(int anio, int mes, int pericodi, string user, int Emprcodi);
        void SetVDER10(int anio, int mes, int pericodi, string user, int Emprcodi);
        void SetPerimsjpaso1(int pericodi, string user);
        void SetPerimsjpaso2(int pericodi, string user);
        void SetPerimsjpaso3(int pericodi, string user);
        void SetPeriEstado(int pericodi, string periestado, string user);
        void SetGarantiaEnergia(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user);
        void SetGarantiaPotenciaPeaje(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user);
        void SetGarantiaServiciosComp(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user);
        void SetGarantiaEnergiaReactiva(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user);
        void SetGarantiainflexibilidadOpe(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user);

        List<GmmDatCalculoDTO> listarRptEnergia(int anio, int mes);
        List<GmmDatCalculoDTO> listarRptInsumo(int anio, int mes);
        List<GmmDatCalculoDTO> listarRpt1(int anio, int mes);
        List<GmmDatCalculoDTO> listarRpt2(int anio, int mes);
        List<GmmDatCalculoDTO> listarRpt3(int anio, int mes);
        List<GmmDatCalculoDTO> listarRpt4(int anio, int mes);
        List<GmmDatCalculoDTO> listarRpt5(int anio, int mes);
        List<GmmDatCalculoDTO> listarRpt6(int anio, int mes);
       

    }
}
