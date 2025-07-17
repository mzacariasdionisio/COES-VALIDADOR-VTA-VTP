using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_MAESTRO_ETAPA
    /// </summary>
    public interface IReMaestroEtapaRepository
    {
        int Save(ReMaestroEtapaDTO entity);
        void Update(ReMaestroEtapaDTO entity);
        void Delete(int reetacodi);
        ReMaestroEtapaDTO GetById(int reetacodi);
        List<ReMaestroEtapaDTO> List();
        List<ReMaestroEtapaDTO> GetByCriteria(int idPeriodo);
    }
}
