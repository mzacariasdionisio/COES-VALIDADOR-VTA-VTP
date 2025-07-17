using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_PERIODO
    /// </summary>
    public interface IEpoPeriodoRepository
    {
        int Save(EpoPeriodoDTO entity);
        void Update(EpoPeriodoDTO entity);
        void Delete(int percodi);
        EpoPeriodoDTO GetById(int percodi);
        List<EpoPeriodoDTO> List();
        List<EpoPeriodoDTO> GetByCriteria();
    }
}
