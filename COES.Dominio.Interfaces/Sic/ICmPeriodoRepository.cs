using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_PERIODO
    /// </summary>
    public interface ICmPeriodoRepository
    {
        int Save(CmPeriodoDTO entity);
        void Update(CmPeriodoDTO entity);
        void Delete(int cmpercodi);
        CmPeriodoDTO GetById(int cmpercodi);
        List<CmPeriodoDTO> List();
        List<CmPeriodoDTO> GetByCriteria(DateTime fecha);
        List<CmPeriodoDTO> ObtenerHistoricoPeriodo();
    }
}
