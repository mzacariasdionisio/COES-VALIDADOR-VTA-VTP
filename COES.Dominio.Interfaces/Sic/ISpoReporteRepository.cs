using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_REPORTE
    /// </summary>
    public interface ISpoReporteRepository
    {
        int Save(SpoReporteDTO entity);
        void Update(SpoReporteDTO entity);
        void Delete(int repcodi);
        SpoReporteDTO GetById(int repcodi);
        List<SpoReporteDTO> List();
        List<SpoReporteDTO> GetByCriteria();
    }
}
