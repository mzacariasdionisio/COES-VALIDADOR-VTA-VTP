using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_FACTOR_UTILIZACION
    /// </summary>
    public interface ICoFactorUtilizacionRepository
    {
        int Save(CoFactorUtilizacionDTO entity);
        void Update(CoFactorUtilizacionDTO entity);
        void Delete(int facuticodi);
        CoFactorUtilizacionDTO GetById(int facuticodi);
        List<CoFactorUtilizacionDTO> List();
        List<CoFactorUtilizacionDTO> GetByCriteria(int prodiacodi);
        List<CoFactorUtilizacionDTO> ObtenerReporte(int idPeriodo, int idVersion, DateTime fechaInicio, DateTime fechaFin);
        List<CoFactorUtilizacionDTO> ObtenerReporteDiario(DateTime fechaInicio, DateTime fechaFin);
        void EliminarFactoresUtilizacion(string strProdiacodis);
        List<CoFactorUtilizacionDTO> ObtenerReporteResultados(int prodiacodi);
        CoFactorUtilizacionDTO GetByProdiacodiYPeriodo(int prodiacodi, int periodo);
    }
}
