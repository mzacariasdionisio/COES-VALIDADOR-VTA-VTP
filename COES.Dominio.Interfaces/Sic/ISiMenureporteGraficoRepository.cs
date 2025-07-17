using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MENUREPORTE_GRAFICO
    /// </summary>
    public interface ISiMenureporteGraficoRepository
    {
        int Save(SiMenureporteGraficoDTO entity);
        void Update(SiMenureporteGraficoDTO entity);
        void Delete(int mrgrcodi);
        SiMenureporteGraficoDTO GetById(int mrgrcodi);
        List<SiMenureporteGraficoDTO> List();
        List<SiMenureporteGraficoDTO> GetByCriteria(int tmrepcodi);
    }
}
