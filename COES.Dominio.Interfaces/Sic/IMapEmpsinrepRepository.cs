using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MAP_EMPSINREP
    /// </summary>
    public interface IMapEmpsinrepRepository
    {
        int Save(MapEmpsinrepDTO entity);
        void Update(MapEmpsinrepDTO entity);
        void Delete(int empsrcodi);
        MapEmpsinrepDTO GetById(int empsrcodi);
        List<MapEmpsinrepDTO> List();
        List<MapEmpsinrepDTO> GetByCriteria();
    }
}
