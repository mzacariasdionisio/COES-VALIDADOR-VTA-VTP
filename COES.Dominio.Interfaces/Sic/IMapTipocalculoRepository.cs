using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MAP_TIPOCALCULO
    /// </summary>
    public interface IMapTipocalculoRepository
    {
        int Save(MapTipocalculoDTO entity);
        void Update(MapTipocalculoDTO entity);
        void Delete(int tipoccodi);
        MapTipocalculoDTO GetById(int tipoccodi);
        List<MapTipocalculoDTO> List();
        List<MapTipocalculoDTO> GetByCriteria();
    }
}
