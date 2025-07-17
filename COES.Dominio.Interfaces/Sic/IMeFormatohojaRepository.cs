using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_FORMATOHOJA
    /// </summary>
    public interface IMeFormatohojaRepository
    {
        void Save(MeFormatohojaDTO entity);
        void Update(MeFormatohojaDTO entity);
        void Delete(int hojanumero, int formatcodi);
        MeFormatohojaDTO GetById(int hojanumero, int formatcodi);
        List<MeFormatohojaDTO> List();
        List<MeFormatohojaDTO> GetByCriteria(int formatcodi);
    }
}
