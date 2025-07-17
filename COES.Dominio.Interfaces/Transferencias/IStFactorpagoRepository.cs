using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_FACTORPAGO
    /// </summary>
    public interface IStFactorpagoRepository
    {
        int Save(StFactorpagoDTO entity);
        void Update(StFactorpagoDTO entity);
        void Delete(int strecacodi);
        StFactorpagoDTO GetById(int facpagcodi);
        List<StFactorpagoDTO> List(int strecacodi, int stcompcodi);
        List<StFactorpagoDTO> GetByCriteria(int strecacodi, int stcompcodi);
        List<StFactorpagoDTO> GetByCriteriaInicialReporte(int strecacodi);
        List<StFactorpagoDTO> GetByCriteriaReporte(int strecacodi);
        List<StFactorpagoDTO> GetByCriteriaReporteFactorPago(int strecacodi);
        StFactorpagoDTO GetByIdFK(int strecacodi, int stcntgcodi, int stcompcodi);
        List<StFactorpagoDTO> ObtenerFactorPagoParticipacion(int strecacodi, int stcompcodi);
        List<StFactorpagoDTO> ObtenerCompensacionMensual(int strecacodi, int stcompcodi);
    }
}
