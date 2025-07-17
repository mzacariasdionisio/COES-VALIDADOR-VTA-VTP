using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_FACTOR_PERDIDA
    /// </summary>
    public interface IFactorPerdidaRepository
    {
        int Save(FactorPerdidaDTO entity);
        void Delete(System.Int32 PeriCodi, System.Int32 FactPerdVersion);
        List<FactorPerdidaDTO> ListByPeriodoVersion(System.Int32 PeriCodi, System.Int32 FactPerdVersion);
        void BulkInsert(List<TrnFactorPerdidaBullkDTO> entitys);
        int GetCodigoGenerado();
        int GetCodigoGeneradoDec();
        void CopiarFactorPerdidaCostoMarginal(int iPeriCodi, int iVersionOld, int iVersionNew, int iFacPerCodi, int iCostMargCodi, System.Data.IDbConnection conn, System.Data.Common.DbTransaction tran);
        void CopiarSGOCOES(int pericodi, int version, int facPerCodi, int costMargCodi, string suser, string sAnioMes, System.Data.IDbConnection conn, System.Data.Common.DbTransaction tran);
        //ASSETEC 202002
        void DeleteCMTMP();
        List<FactorPerdidaDTO> ListBarrasSiCostMarg(string sAnioMes);
        void SaveCostMargTmp(int barrcodi, int iDiasMes, string sAnioMes);
        List<string> ListFechaXBarraSiCostMarg(string sAnioMes, int barrcodi);
    }
}

