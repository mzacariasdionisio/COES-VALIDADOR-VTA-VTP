using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_GENERADOR_BARRAEMS
    /// </summary>
    public interface ICmGeneradorBarraemsRepository
    {
        int Save(CmGeneradorBarraemsDTO entity);
        void Update(CmGeneradorBarraemsDTO entity);
        void Delete(int genbarcodi);
        CmGeneradorBarraemsDTO GetById(int genbarcodi);
        List<CmGeneradorBarraemsDTO> List();
        List<CmGeneradorBarraemsDTO> GetByCriteria(int relacionCodi);
    }
}
