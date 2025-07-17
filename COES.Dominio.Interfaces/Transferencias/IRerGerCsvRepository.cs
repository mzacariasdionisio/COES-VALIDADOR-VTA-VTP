using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerGerCsvRepository
    {
        int Save(RerGerCsvDTO entity);
        void Update(RerGerCsvDTO entity);
        void Delete(int rerGerCsvId);
        RerGerCsvDTO GetById(int rerGerCsvId);
        List<RerGerCsvDTO> List();

        
        void BulkInsertTablaTemporal(List<RerLecCsvTemp> entitys, string nombreTabla);
        void TruncateTablaTemporal(string nombreTabla);
        void InsertTablaTemporal(RerLecCsvTemp entity);
        List<RerLecCsvTemp> ListTablaTemporal(string nomCentralSddp);
        List<RerCentralDTO> ListEquiposEmpresasCentralesRer();
        List<RerCentralPmpoDTO> ListPtosMedicionCentralesPmpo(int rercencodi);
        PmoSddpCodigoDTO GetByCentralesSddp(int ptoMediCodi);
        List<RerCentralDTO> ListPtoMedicionCentralesRer();
        List<RerInsumoTemporalDTO> ListTablaCMTemporal(int ptoMediCodi);
        List<RerInsumoDiaTemporalDTO> ListTablaCMTemporalDia(int ptoMediCodi, DateTime fechaInicio, DateTime fechaFin, decimal dTipoCambio);
    }
}

