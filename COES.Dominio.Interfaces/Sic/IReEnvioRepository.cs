using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_ENVIO
    /// </summary>
    public interface IReEnvioRepository
    {
        int Save(ReEnvioDTO entity);
        void Update(ReEnvioDTO entity);
        void Delete(int reenvcodi);
        ReEnvioDTO GetById(int reenvcodi);
        List<ReEnvioDTO> List();
        List<ReEnvioDTO> GetByCriteria(int idEmpresa, int idPeriodo, string tipo);
        List<ReEnvioDTO> GetByPeriodoYEmpresa(int emprcodi, int idperiodo, string tipo);
    }
}
