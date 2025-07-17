using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_VERSIONREP
    /// </summary>
    public interface ISpoVersionrepRepository
    {
        int Save(SpoVersionrepDTO entity);
        void Update(SpoVersionrepDTO entity);
        void Delete(int verrcodi);
        SpoVersionrepDTO GetById(int verrcodi);
        List<SpoVersionrepDTO> List();
        List<SpoVersionrepDTO> GetByCriteria(DateTime fecha);
        int GetMaxIdVersion(DateTime periodo);
        void UpdateEstado(SpoVersionrepDTO entity);
    }
}
