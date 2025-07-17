using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_ENVIOLIQUIDACION
    /// </summary>
    public interface ICoEnvioliquidacionRepository
    {
        int Save(CoEnvioliquidacionDTO entity);
        void Update(CoEnvioliquidacionDTO entity);
        void Delete(int coenlicodi);
        CoEnvioliquidacionDTO GetById(int coenlicodi);
        List<CoEnvioliquidacionDTO> List();
        List<CoEnvioliquidacionDTO> GetByCriteria(int idVersion);
        List<CoEnvioliquidacionDTO> ObtenerEnviosPorPeriodo(int idPeriodo);
    }
}
