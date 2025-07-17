using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_VERSIONPROGRAMA
    /// </summary>
    public interface ICmVersionprogramaRepository
    {
        int Save(CmVersionprogramaDTO entity);
        void Update(CmVersionprogramaDTO entity);
        void Delete(int cmvepr);
        CmVersionprogramaDTO GetById(int cmvepr);
        List<CmVersionprogramaDTO> List();
        List<CmVersionprogramaDTO> GetByCriteria();
    }
}
