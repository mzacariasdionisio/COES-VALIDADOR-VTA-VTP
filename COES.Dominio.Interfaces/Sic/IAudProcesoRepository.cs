using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_PROCESO
    /// </summary>
    public interface IAudProcesoRepository
    {
        int Save(AudProcesoDTO entity);
        void Update(AudProcesoDTO entity);
        void Delete(AudProcesoDTO proceso);
        AudProcesoDTO GetById(int proccodi);
        AudProcesoDTO GetByCodigo(string proccodigo);
        List<AudProcesoDTO> List();
        List<AudProcesoDTO> GetByCriteria(AudProcesoDTO proceso);
        List<AudProcesoDTO> ListProcesoSuperior(int proccodi, int areacodi);
        List<AudProcesoDTO> GetByProcesoPorEstado(string estado);
        int ObtenerNroRegistrosBusqueda(AudProcesoDTO proceso);
        List<AudProcesoDTO> GetByProcesoPorArea(string areacodi);
        string GetByProcesoValidacion(int proccodi, string procedescripcion);
    }
}
