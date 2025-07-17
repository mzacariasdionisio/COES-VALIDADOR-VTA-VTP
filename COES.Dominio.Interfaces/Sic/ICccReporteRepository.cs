using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CCC_REPORTE
    /// </summary>
    public interface ICccReporteRepository
    {
        int GetMaxId();
        int Save(CccReporteDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CccReporteDTO entity);
        void Delete(int cccrptcodi);
        CccReporteDTO GetById(int cccrptcodi);
        List<CccReporteDTO> List();
        List<CccReporteDTO> GetByCriteria(string cccvercodi);
    }
}
