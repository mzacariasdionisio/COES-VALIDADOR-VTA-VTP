using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_INDICADOR
    /// </summary>
    public interface IEpoIndicadorRepository
    {
        int Save(EpoIndicadorDTO entity);
        void Update(EpoIndicadorDTO entity);
        void Delete(int indcodi);
        EpoIndicadorDTO GetById(int indcodi);
        List<EpoIndicadorDTO> List();
        List<EpoIndicadorDTO> GetByCriteria();
    }
}
