using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_CONFIGURA
    /// </summary>
    public interface IEpoConfiguraRepository
    {
        int Save(EpoConfiguraDTO entity);
        void Update(EpoConfiguraDTO entity);
        void Delete(int confcodi);
        EpoConfiguraDTO GetById(int confcodi);
        List<EpoConfiguraDTO> List();
        List<EpoConfiguraDTO> GetByCriteria();
    }
}
