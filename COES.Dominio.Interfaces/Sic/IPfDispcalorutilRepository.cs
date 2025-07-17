using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_DISPCALORUTIL
    /// </summary>
    public interface IPfDispcalorutilRepository
    {
        int GetMaxId();
        int Save(PfDispcalorutilDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfDispcalorutilDTO entity);
        void Delete(int pfcucodi);
        PfDispcalorutilDTO GetById(int pfcucodi);
        List<PfDispcalorutilDTO> List();
        List<PfDispcalorutilDTO> GetByCriteria(int irptcodi);
    }
}
