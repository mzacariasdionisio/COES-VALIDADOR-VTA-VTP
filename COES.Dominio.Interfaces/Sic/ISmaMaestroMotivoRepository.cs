using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_MAESTRO_MOTIVO
    /// </summary>
    public interface ISmaMaestroMotivoRepository
    {
        int Save(SmaMaestroMotivoDTO entity);
        void Update(SmaMaestroMotivoDTO entity);
        void Delete(int smammcodi);
        SmaMaestroMotivoDTO GetById(int smammcodi);
        List<SmaMaestroMotivoDTO> List();
        List<SmaMaestroMotivoDTO> GetByCriteria(string smammcodis);
    }
}
