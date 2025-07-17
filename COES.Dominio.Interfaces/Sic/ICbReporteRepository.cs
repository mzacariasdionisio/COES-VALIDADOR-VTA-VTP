using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_REPORTE
    /// </summary>
    public interface ICbReporteRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(CbReporteDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(CbReporteDTO entity);
        void Update(CbReporteDTO entity);
        void Delete(int cbrepcodi);
        CbReporteDTO GetById(int cbrepcodi);
        List<CbReporteDTO> List();
        List<CbReporteDTO> GetByCriteria();
        List<CbReporteDTO> GetByTipoYMesVigencia(int tipoReporte, string mesVigencia);
    }
}
