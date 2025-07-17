using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_TIPOPLANTILLACORREO
    /// </summary>
    public interface ISiTipoplantillacorreoRepository
    {
        void Update(SiTipoplantillacorreoDTO entity);
        void Delete(int tpcorrcodi);
        SiTipoplantillacorreoDTO GetById(int tpcorrcodi);
        List<SiTipoplantillacorreoDTO> List();
        List<SiTipoplantillacorreoDTO> GetByCriteria();        
    }
}
