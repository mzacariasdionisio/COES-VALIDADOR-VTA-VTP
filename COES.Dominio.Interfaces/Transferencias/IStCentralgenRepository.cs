using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_CENTRALGEN
    /// </summary>
    public interface IStCentralgenRepository
    {
        int Save(StCentralgenDTO entity);
        void Update(StCentralgenDTO entity);
        void Delete(int stcntgcodi);
        void DeleteVersion(int strecacodi);
        StCentralgenDTO GetById(int stcntgcodi);
        List<StCentralgenDTO> List(int id);
        List<StCentralgenDTO> GetByCriteria(int strecacodi);
        List<StCentralgenDTO> GetByCriteriaReporte(int strecacodi);
        
        StCentralgenDTO GetByCentNombre(string equinomb, int strecacodi);
        List<StCentralgenDTO> ObtenerGeneradoresCompensacion(int strecacodi, int stcompcodi);
    }
}
