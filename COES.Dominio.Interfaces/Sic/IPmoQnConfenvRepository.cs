using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_QN_CONFENV
    /// </summary>
    public interface IPmoQnConfenvRepository
    {
        int Save(PmoQnConfenvDTO entity);
        void Update(PmoQnConfenvDTO entity);
        void Delete(int qncfgecodi);
        PmoQnConfenvDTO GetById(int qncfgecodi);
        List<PmoQnConfenvDTO> List();
        List<PmoQnConfenvDTO> GetByCriteria();
        int Save(PmoQnConfenvDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
