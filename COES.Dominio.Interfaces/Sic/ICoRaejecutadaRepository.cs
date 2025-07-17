using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_RAEJECUTADA
    /// </summary>
    public interface ICoRaejecutadaRepository
    {
        int Save(CoRaejecutadaDTO entity);
        void Update(CoRaejecutadaDTO entity);
        void Delete(int coraejcodi);
        CoRaejecutadaDTO GetById(int coraejcodi);
        List<CoRaejecutadaDTO> List();
        List<CoRaejecutadaDTO> GetByCriteria();
    }
}
