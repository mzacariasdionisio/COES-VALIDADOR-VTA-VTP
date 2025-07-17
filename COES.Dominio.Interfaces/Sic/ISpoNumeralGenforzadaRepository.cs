using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_NUMERAL_GENFORZADA
    /// </summary>
    public interface ISpoNumeralGenforzadaRepository
    {
        int Save(SpoNumeralGenforzadaDTO entity);
        void Update(SpoNumeralGenforzadaDTO entity);
        void Delete(int genforcodi);
        SpoNumeralGenforzadaDTO GetById(int genforcodi);
        List<SpoNumeralGenforzadaDTO> List();
        List<SpoNumeralGenforzadaDTO> GetByCriteria(int verncodi);
    }
}
