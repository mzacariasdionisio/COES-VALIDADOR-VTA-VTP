using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ITrnCostoMarginalAjusteRepository
    {
        void Save(TrnCostoMarginalAjusteDTO entity);
        void Delete(int trncmacodi);
        void Update(int idPeriodo, int idVersion, string suser);
        List<TrnCostoMarginalAjusteDTO> ListByPeriodoVersion(int idPeriodo, int idVersion);
        PeriodoDTO GetPeriodo(int idPeriodo);
        void CopiarAjustesCostosMarginales(int iPeriCodi, int iVersionNew, int iVersionOld);
        void DeleteListaAjusteCostoMarginal(int iPeriCodi, int iVersion);
    }
}
