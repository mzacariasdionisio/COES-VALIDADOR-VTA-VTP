using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_ESCENARIO
    /// </summary>
    public interface IPfEscenarioRepository
    {
        int Save(PfEscenarioDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfEscenarioDTO entity);
        void Delete(int pfescecodi);
        PfEscenarioDTO GetById(int pfescecodi);
        List<PfEscenarioDTO> List();
        List<PfEscenarioDTO> GetByCriteria(int pfrptcodi);
    }
}
