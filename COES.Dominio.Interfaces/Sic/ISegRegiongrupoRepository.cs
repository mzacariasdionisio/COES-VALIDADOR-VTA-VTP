using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SEG_REGIONGRUPO
    /// </summary>
    public interface ISegRegiongrupoRepository
    {
        int Save(SegRegiongrupoDTO entity);
        void Update(SegRegiongrupoDTO entity);
        void Delete(int regcodi, int grupocodi, int segcotipo);
        SegRegiongrupoDTO GetById();
        List<SegRegiongrupoDTO> List();
        List<SegRegiongrupoDTO> GetByCriteria(int regcodi, int tipo);
    }
}
