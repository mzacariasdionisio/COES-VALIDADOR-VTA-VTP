using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_BARRAGENERADOR
    /// </summary>
    public interface ICmBarrageneradorRepository
    {
        int Save(CmBarrageneradorDTO entity);
        void Update(CmBarrageneradorDTO entity);
        void Delete(int bargercodi);
        CmBarrageneradorDTO GetById(int bargercodi);
        List<CmBarrageneradorDTO> List();
        List<CmBarrageneradorDTO> GetByCriteria();
    }
}
