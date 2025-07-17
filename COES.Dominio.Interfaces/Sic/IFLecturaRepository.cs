using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla F_LECTURA
    /// </summary>
    public interface IFLecturaRepository
    {
        void Update(FLecturaDTO entity);
        void Delete();
        FLecturaDTO GetById(DateTime FechaHora, Int32 GpsCodi);
        List<FLecturaDTO> List();
        List<FLecturaDTO> GetByCriteria(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFi);
        DataTable GetByCriteriaDatatable(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin);
        DataTable GetByCriteriaDatatable2(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin);
        DataTable GetFechaDesvNumPorGpsFecha(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin);
        List<FLecturaDTO> ObtenerFrecuenciaSein(int iGpscodi);
        List<FLecturaDTO> ListByFechaDesvNumPorGpsFecha(int gpsCodi, DateTime fecha);
        int ContarRegistrosRepetidos(Int32 GpsCodi, DateTime FechaInicio, DateTime FechaFin);
    }
}
