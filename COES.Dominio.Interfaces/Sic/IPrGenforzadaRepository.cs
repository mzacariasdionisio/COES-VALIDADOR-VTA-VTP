using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_GENFORZADA
    /// </summary>
    public interface IPrGenforzadaRepository
    {
        int Save(PrGenforzadaDTO entity);
        void Update(PrGenforzadaDTO entity);
        void Delete(int genforcodi);
        PrGenforzadaDTO GetById(int genforcodi);
        List<PrGenforzadaDTO> List();
        List<PrGenforzadaDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
        List<PrGenforzadaDTO> ObtenerGeneracionForzadaProceso(DateTime fechaProceso);
        List<PrGenforzadaDTO> ObtenerGeneracionForzadaProcesoV2(DateTime fechaProceso);
        List<PrGenforzadaDTO> ObtenerUnidadesPasada();
    }
}
