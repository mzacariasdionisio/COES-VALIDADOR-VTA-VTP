using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_VERREPNUM
    /// </summary>
    public interface ISpoVerrepnumRepository
    {
        int Save(SpoVerrepnumDTO entity);
        void Update(SpoVerrepnumDTO entity);
        void Delete(int verrncodi);
        SpoVerrepnumDTO GetById(int verrncodi);
        List<SpoVerrepnumDTO> List();
        List<SpoVerrepnumDTO> GetByCriteria();
        List<SpoVerrepnumDTO> GetByVersionReporte(int verrcodi);
    }
}
