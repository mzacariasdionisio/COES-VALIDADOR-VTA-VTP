using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MMM_INDICADOR
    /// </summary>
    public interface IMmmIndicadorRepository
    {
        int Save(MmmIndicadorDTO entity);
        void Update(MmmIndicadorDTO entity);
        void Delete(int immecodi);
        MmmIndicadorDTO GetById(int immecodi);
        List<MmmIndicadorDTO> List();
        List<MmmIndicadorDTO> GetByCriteria();
    }
}
