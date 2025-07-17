using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_INDICADOR_PERIODO
    /// </summary>
    public interface IReIndicadorPeriodoRepository
    {
        int Save(ReIndicadorPeriodoDTO entity);
        void Update(ReIndicadorPeriodoDTO entity);
        void Delete(int reindcodi);
        ReIndicadorPeriodoDTO GetById(int reindcodi);
        List<ReIndicadorPeriodoDTO> List();
        List<ReIndicadorPeriodoDTO> GetByCriteria();
        List<ReIndicadorPeriodoDTO> ObtenerParaImportar(int idPeriodo);
    }
}
