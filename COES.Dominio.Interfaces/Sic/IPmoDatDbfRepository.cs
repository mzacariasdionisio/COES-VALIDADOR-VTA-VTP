using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPmoDatDbfRepository
    {
        //List<PmoDatDbfDTO> ListDbf(int codigoPeriodo);
        List<PmoDatDbfDTO> ListDbf(int codigoPeriodo, string nombarra);
        List<PmoDatDbfDTO> ListDatDbf(int codigoPeriodo);        
        int CountDatDbf(int periCodi);
        List<PrGrupoDTO> ListGrupoDbf(int catecodi);
        int Delete(int periCodi);
        int Save(PmoDatDbfDTO entity);
        PrGrupoDTO GetGrupoCodi(int grupoCodiSddp);

        #region NET 20190304
        List<PmoDatDbfDTO> GetDataBaseDbf(DateTime fechaInicio, DateTime fechaFin);
        int DeleteDataTmp(int periCodi);
        void SqlSaveTmp(PmoDatDbfDTO entity);
        List<PmoDatDbfDTO> GetDataTmpByFilter(int codigoPeriodo, int grupoCodi, DateTime fechaIni, int bloqCodi, string lcod);
        List<PmoDatDbfDTO> GetDataFinalProcesamiento(int codigoPeriodo);
        int Update(PmoDatDbfDTO entity);
        void CompletarBarrasModelo(int PeriCodi);

        #endregion
    }
}
