using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_PARAMETRO
    /// </summary>
    public interface ICmParametroRepository
    {
        int Save(CmParametroDTO entity);
        void Update(CmParametroDTO entity);
        void Delete(int cmparcodi);
        CmParametroDTO GetById(int cmparcodi);
        List<CmParametroDTO> List();
        List<CmParametroDTO> GetByCriteria();
    }
}
