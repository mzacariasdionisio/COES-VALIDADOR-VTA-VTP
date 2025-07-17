using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_LOGCORREO
    /// </summary>
    public interface IReLogcorreoRepository
    {
        int Save(ReLogcorreoDTO entity);
        void Update(ReLogcorreoDTO entity);
        void Delete(int relcorcodi);
        ReLogcorreoDTO GetById(int relcorcodi);
        List<ReLogcorreoDTO> List();
        List<ReLogcorreoDTO> GetByCriteria();
        List<ReLogcorreoDTO> ObtenerPorFechaYTipo(DateTime fechaInicio, DateTime fechaFin, string idsPlantilla);
    }
}
