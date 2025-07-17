using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_REPORTE
    /// </summary>
    public interface IPfrReporteRepository
    {
        //int Save(PfrReporteDTO entity);
        int Save(PfrReporteDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfrReporteDTO entity);
        void Delete(int pfrrptcodi);
        PfrReporteDTO GetById(int pfrrptcodi);
        List<PfrReporteDTO> List();        
        List<PfrReporteDTO> GetByCriteria(int pfrreccodi, int pfrcuacodi);
    }
}
