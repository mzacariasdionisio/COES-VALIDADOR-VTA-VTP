using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_REPORTE
    /// </summary>
    public interface ICmReporteRepository
    {
        int Save(CmReporteDTO entity);
        void Update(CmReporteDTO entity);
        void Delete(int cmrepcodi);
        CmReporteDTO GetById(int cmrepcodi);
        List<CmReporteDTO> List();
        List<CmReporteDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
        int ObtenerNroVersion(DateTime fecha);
    }
}
