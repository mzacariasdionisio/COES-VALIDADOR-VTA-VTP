using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_ALERTAVALOR
    /// </summary>
    public interface ICmAlertavalorRepository
    {
        int Save(CmAlertavalorDTO entity);
        void Update(string indicador, decimal maxCM, decimal maxCMCongestion, decimal maxCMSinCongestion, decimal maxCICongestion, decimal maxCISinCongestion, DateTime fechaProceso);
        void Delete(int alevalcodi);
        CmAlertavalorDTO GetById();
        List<CmAlertavalorDTO> List();
        List<CmAlertavalorDTO> GetByCriteria();
    }
}
