using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_ESCENARIO
    /// </summary>
    public interface IPfrEscenarioRepository
    {
        int Save(PfrEscenarioDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfrEscenarioDTO entity);
        void Delete(int pfresccodi);
        PfrEscenarioDTO GetById(int pfresccodi);
        List<PfrEscenarioDTO> List();
        List<PfrEscenarioDTO> GetByCriteria();
        List<PfrEscenarioDTO> ListByReportecodi(int pfrrptcodi);
    }
}
