using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerInsumoMesRepository
    {
        int Save(RerInsumoMesDTO entity);
        void Update(RerInsumoMesDTO entity);
        void Delete(int Rerinmmescodi);
        RerInsumoMesDTO GetById(int Rerinmmescodi);
        List<RerInsumoMesDTO> List();
        void DeleteByParametroPrimaAndTipo(int iRerpprcodi, int iRerpprmes, string sRerindtipresultado);
        List<RerInsumoMesDTO> GetByAnioTarifario(int reravcodi, string rerinmtipresultado);
        List<RerInsumoMesDTO> GetByTipoResultadoByPeriodo(string rerinmtipresultado, int reravcodi, string rerpprmes);
    }
}

