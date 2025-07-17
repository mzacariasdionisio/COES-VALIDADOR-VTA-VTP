using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ISiLogRepository
    {
        #region INTERVENCIONES - CAMBIOS
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        #endregion

        List<SiLogDTO> Listar(string usuario, string fecha, int ModCodi);
        int Save(int ModCodi, string evento, string fecha, string usuario);        
        List<SiLogDTO> ListLogByMigracion(int migracodi);
        //int SaveTransferencia(int ModCodi, string evento, string fecha, string usuario, IDbConnection conn, DbTransaction tran,int corrSiLog);
        int SaveTransferencia(SiLogDTO entidad, IDbConnection conn, DbTransaction tran);
        int GetMaxId();

        #region INTERVENCIONES
        void Save(SiLogDTO entity, IDbConnection conn, DbTransaction tran);
        List<SiLogDTO> ReporteHistorial(DateTime? fechaInicio, DateTime? fechaFin, string actividad);
        #endregion

        int Save(SiLogDTO entity);
    }
}
