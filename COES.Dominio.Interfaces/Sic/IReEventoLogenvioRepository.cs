using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_EVENTO_LOGENVIO
    /// </summary>
    public interface IReEventoLogenvioRepository
    {
        int Save(ReEventoLogenvioDTO entity);
        void Update(ReEventoLogenvioDTO entity);
        void Delete(int reevlocodi);
        ReEventoLogenvioDTO GetById(int reevlocodi);
        List<ReEventoLogenvioDTO> List();
        List<ReEventoLogenvioDTO> GetByCriteria();
        List<ReEventoLogenvioDTO> ObtenerEnvios(int idEmpresa, int idEvento);
    }
}
