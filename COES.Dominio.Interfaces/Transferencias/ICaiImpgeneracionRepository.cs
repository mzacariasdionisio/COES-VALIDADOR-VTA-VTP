using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_IMPGENERACION
    /// </summary>
    public interface ICaiImpgeneracionRepository
    {
        int Save(CaiImpgeneracionDTO entity);
        void Update(CaiImpgeneracionDTO entity);
        void Delete(int caiajcodi);
        CaiImpgeneracionDTO GetById(int caimpgcodi);
        List<CaiImpgeneracionDTO> List();
        List<CaiImpgeneracionDTO> GetByCriteria();
    }
}
