using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerInsumoDiaRepository
    {
        int Save(RerInsumoDiaDTO entity);
        void Update(RerInsumoDiaDTO entity);
        void Delete(int Rerinddiacodi);
        RerInsumoDiaDTO GetById(int Rerinddiacodi);
        List<RerInsumoDiaDTO> GetByMesByEmpresaByCentral(int rerinmmescodi, int emprcodi, int equicodi);
        List<RerInsumoDiaDTO> List();

        #region CUS21

        RerInsumoDiaDTO GetByCriteria(int rerInmMesCodi, int emprcodi, int equicodi, string rerIndDiaFecha);
        void DeleteByParametroPrimaAndTipo(int iRerpprcodi, int iRerpprmes, string iRerindtipresultado);
        List<RerInsumoDiaDTO> GetByTipoResultadoByPeriodo(string rerindtipresultado, int reravcodi, string rerpprmes);
        #endregion


        void TruncateTablaTemporal(string nombreTabla);
        void SaveTablaTemporal(RerInsumoDiaTemporalDTO entity);
        void BulkInsertRerInsumoDiaTemporal(List<RerInsumoDiaTemporalDTO> entitys, string nombreTabla);

        int GetMaxId();
        void BulkInsertRerInsumoDia(List<RerInsumoDiaDTO> entitys);
    }
}

