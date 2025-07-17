using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_LOG
    /// </summary>
    public interface IPmoLogRepository
    {
        int Save(PmoLogDTO entity);
        void Update(PmoLogDTO entity);
        void Delete(int pmologcodi);
        PmoLogDTO GetById(int pmologcodi);
        List<PmoLogDTO> List();
        List<PmoLogDTO> GetByCriteria(int enviocodi);
    }
}
