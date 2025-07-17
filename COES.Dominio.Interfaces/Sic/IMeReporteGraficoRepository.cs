using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_REPORTE_GRAFICO
    /// </summary>
    public interface IMeReporteGraficoRepository
    {
        int Save(MeReporteGraficoDTO entity);
        void Update(MeReporteGraficoDTO entity);
        void Delete(int repgrcodi);
        MeReporteGraficoDTO GetById(int repgrcodi);
        List<MeReporteGraficoDTO> List();
        List<MeReporteGraficoDTO> GetByCriteria(int reporcodi);
    }
}
