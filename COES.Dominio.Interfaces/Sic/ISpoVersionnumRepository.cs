using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_VERSIONNUM
    /// </summary>
    public interface ISpoVersionnumRepository
    {
        int Save(SpoVersionnumDTO entity);
        void Update(SpoVersionnumDTO entity);
        void UpdateEstado(SpoVersionnumDTO entity);
        void Delete(int verncodi);
        SpoVersionnumDTO GetById(int verncodi);
        List<SpoVersionnumDTO> List();
        List<SpoVersionnumDTO> GetByCriteria(DateTime fecha, int numeral);
    }
}
