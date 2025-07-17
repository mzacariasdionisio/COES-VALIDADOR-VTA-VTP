using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MAP_VERSION
    /// </summary>
    public interface IMapVersionRepository
    {
        int Save(MapVersionDTO entity);
        void Update(MapVersionDTO entity);
        void Delete(int vermcodi);
        MapVersionDTO GetById(int vermcodi);
        List<MapVersionDTO> List();
        List<MapVersionDTO> GetByCriteria();


        int GetMaxVermnumero(DateTime fecha);
    }
}
