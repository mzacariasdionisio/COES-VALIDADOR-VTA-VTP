using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_CONGESTION_CALCULO
    /// </summary>
    public interface ICmCongestionCalculoRepository
    {
        int Save(CmCongestionCalculoDTO entity);
        void Update(CmCongestionCalculoDTO entity);
        void Delete(int periodo, DateTime fecha);
        CmCongestionCalculoDTO GetById(int cmcongcodi);
        List<CmCongestionCalculoDTO> List();
        List<CmCongestionCalculoDTO> GetByCriteria();
        List<CmCongestionCalculoDTO> ObtenerRegistroCongestion(DateTime fecha);
        List<CmCongestionCalculoDTO> ObtenerCongestionProceso(DateTime fecha);
        List<CmCongestionCalculoDTO> ObtenerCongestionPorLinea(DateTime fecha, string linea);
    }
}
