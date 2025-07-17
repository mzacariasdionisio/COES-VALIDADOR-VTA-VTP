using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SEG_REGION
    /// </summary>
    public interface ISegRegionRepository
    {
        int Save(SegRegionDTO entity);
        void Update(SegRegionDTO entity);
        void Delete(int regcodi);
        SegRegionDTO GetById(int regcodi);
        List<SegRegionDTO> List();
        List<SegRegionDTO> GetByCriteria();
        void ActualizarCongestion(int regcodi, string usuario, string estado);
    }
}
