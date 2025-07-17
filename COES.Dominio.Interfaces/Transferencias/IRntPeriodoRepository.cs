using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RNT_PERIODO
    /// </summary>
    public interface IRntPeriodoRepository
    {
        int Save(RntPeriodoDTO entity);
        void Update(RntPeriodoDTO entity);
        void Delete(RntPeriodoDTO entity);
        RntPeriodoDTO GetById(int periodocodi);
        List<RntPeriodoDTO> List();
        List<RntPeriodoDTO> ListCombo();
        List<RntPeriodoDTO> GetByCriteria();
    }
}
