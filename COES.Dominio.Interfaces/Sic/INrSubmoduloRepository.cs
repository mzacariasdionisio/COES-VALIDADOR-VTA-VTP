using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla NR_SUBMODULO
    /// </summary>
    public interface INrSubmoduloRepository
    {
        int Save(NrSubmoduloDTO entity);
        void Update(NrSubmoduloDTO entity);
        void Delete(int nrsmodcodi);
        NrSubmoduloDTO GetById(int nrsmodcodi);
        List<NrSubmoduloDTO> List();
        List<NrSubmoduloDTO> GetByCriteria();
        

    }
}
