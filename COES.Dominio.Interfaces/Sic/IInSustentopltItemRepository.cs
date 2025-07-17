using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_SUSTENTOPLT_ITEM
    /// </summary>
    public interface IInSustentopltItemRepository
    {
        int Save(InSustentopltItemDTO entity);
        void Update(InSustentopltItemDTO entity);
        void Delete(int inpsticodi);
        InSustentopltItemDTO GetById(int inpsticodi);
        List<InSustentopltItemDTO> List();
        List<InSustentopltItemDTO> GetByCriteria(int inpstcodi);
        int Save(InSustentopltItemDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateOrdenRequisito(int orden, int inpstcodi);
    }
}
