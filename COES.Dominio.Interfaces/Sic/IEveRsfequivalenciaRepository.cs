using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_RSFEQUIVALENCIA
    /// </summary>
    public interface IEveRsfequivalenciaRepository
    {
        int Save(EveRsfequivalenciaDTO entity);
        void Update(EveRsfequivalenciaDTO entity);
        void Delete(int rsfequcodi);
        EveRsfequivalenciaDTO GetById(int rsfequcodi);
        List<EveRsfequivalenciaDTO> List();
        List<EveRsfequivalenciaDTO> GetByCriteria();
    }
}
