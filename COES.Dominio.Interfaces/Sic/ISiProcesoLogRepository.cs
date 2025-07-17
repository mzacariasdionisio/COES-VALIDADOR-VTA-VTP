using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_PROCESO_LOG
    /// </summary>
    public interface ISiProcesoLogRepository
    {
        int Save(SiProcesoLogDTO entity);
        void Update(SiProcesoLogDTO entity);
        void Delete(int prcslgcodi);
        SiProcesoLogDTO GetById(int prcslgcodi);
        List<SiProcesoLogDTO> List();
        List<SiProcesoLogDTO> GetByCriteria();
    }
}
