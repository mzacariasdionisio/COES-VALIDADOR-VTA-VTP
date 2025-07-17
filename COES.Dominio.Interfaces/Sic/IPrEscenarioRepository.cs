using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_ESCENARIO
    /// </summary>
    public interface IPrEscenarioRepository
    {
        int Save(PrEscenarioDTO entity);
        void Update(PrEscenarioDTO entity);
        void Delete(int escecodi);
        PrEscenarioDTO GetById(int escecodi);
        List<PrEscenarioDTO> List();
        List<PrEscenarioDTO> GetByCriteria();
        List<PrEscenarioDTO> GetEscenariosPorFechaRepCv(DateTime fecha);
    }
}
