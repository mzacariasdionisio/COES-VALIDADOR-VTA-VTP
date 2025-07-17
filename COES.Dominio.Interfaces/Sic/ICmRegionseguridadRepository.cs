using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_REGIONSEGURIDAD
    /// </summary>
    public interface ICmRegionseguridadRepository
    {
        int Save(CmRegionseguridadDTO entity);
        void Update(CmRegionseguridadDTO entity);
        void Delete(int regsegcodi);
        CmRegionseguridadDTO GetById(int regsegcodi);
        List<CmRegionseguridadDTO> List();
        List<CmRegionseguridadDTO> GetByCriteria();
        List<CmRegionseguridadDTO> GetByCriteriaCoordenada(int regcodi);
    }
}
