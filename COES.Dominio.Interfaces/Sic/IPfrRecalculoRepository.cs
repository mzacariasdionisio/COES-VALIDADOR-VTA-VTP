using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_RECALCULO
    /// </summary>
    public interface IPfrRecalculoRepository
    {
        int Save(PfrRecalculoDTO entity);
        void Update(PfrRecalculoDTO entity);
        void Delete(int pfrreccodi);
        PfrRecalculoDTO GetById(int pfrreccodi);
        List<PfrRecalculoDTO> List();
        List<PfrRecalculoDTO> GetByCriteria(int pfrPericodi);
    }
}
