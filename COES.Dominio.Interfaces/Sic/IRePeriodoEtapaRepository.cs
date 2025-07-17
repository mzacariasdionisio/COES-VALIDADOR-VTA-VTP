using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_PERIODO_ETAPA
    /// </summary>
    public interface IRePeriodoEtapaRepository
    {
        int Save(RePeriodoEtapaDTO entity);
        void Update(RePeriodoEtapaDTO entity);
        void Delete(int repeetcodi);
        RePeriodoEtapaDTO GetById(int repeetcodi);
        List<RePeriodoEtapaDTO> List();
        List<RePeriodoEtapaDTO> GetByCriteria();
        List<RePeriodoEtapaDTO> GetByPeriodo(int repercodi);
    }
}
