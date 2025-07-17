using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_TOLERANCIA_PERIODO
    /// </summary>
    public interface IReToleranciaPeriodoRepository
    {
        int Save(ReToleranciaPeriodoDTO entity);
        void Update(ReToleranciaPeriodoDTO entity);
        void Delete(int retolcodi);
        ReToleranciaPeriodoDTO GetById(int retolcodi);
        List<ReToleranciaPeriodoDTO> List();
        List<ReToleranciaPeriodoDTO> GetByCriteria(int periodo);
        List<ReToleranciaPeriodoDTO> ObtenerParaImportar(int idPeriodo);
    }
}
