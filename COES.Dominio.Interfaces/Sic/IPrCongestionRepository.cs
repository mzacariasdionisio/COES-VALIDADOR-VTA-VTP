using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_CONGESTION
    /// </summary>
    public interface IPrCongestionRepository
    {
        int Save(PrCongestionDTO entity);
        void Update(PrCongestionDTO entity);
        void Delete(int congescodi);
        PrCongestionDTO GetById(int congescodi);
        List<PrCongestionDTO> List();
        List<PrCongestionDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
        List<PrCongestionDTO> ObtenerCongestionSimple(DateTime fechaInicio, DateTime fechaFin);
        List<PrCongestionDTO> ObtenerCongestionConjunto(DateTime fechaInicio, DateTime fechaFin);
        List<PrCongestionDTO> ObtenerCongestionRegistro(DateTime fechaProceso);
        List<PrCongestionDTO> ObtenerCongestionConjuntoRegistro(DateTime fechaProceso, string tipo);
        List<PrCongestionDTO> ObtenerCongestion(DateTime fechaInicio,DateTime fechaFin);
        List<PrCongestionDTO> ListaCongestionConjunto(DateTime fechaInicio, DateTime fechaFin, string Indtipo);

        #region Regiones_seguridad
        List<PrCongestionDTO> ObtenerCongestionRegionSeguridad(DateTime fechaProceso);
        #endregion

        bool VerificarExistenciaCongestion(int configcodi, DateTime fecha);
    }
}
