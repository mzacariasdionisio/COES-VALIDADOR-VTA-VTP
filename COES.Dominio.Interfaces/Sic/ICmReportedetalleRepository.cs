using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_REPORTEDETALLE
    /// </summary>
    public interface ICmReportedetalleRepository
    {
        int Save(CmReportedetalleDTO entity);
        void Update(CmReportedetalleDTO entity);
        void Delete(int cmredecodi);
        CmReportedetalleDTO GetById(int cmredecodi);
        List<CmReportedetalleDTO> List();
        List<CmReportedetalleDTO> GetByCriteria(int idReporte, string tipo);
        void GrabarDatosBulkResult(List<CmReportedetalleDTO> entitys);
        int ObtenerMaxId();
        List<CmReportedetalleDTO> ObtenerReporte(DateTime fechaInicio, DateTime fechaFin);
    }
}
