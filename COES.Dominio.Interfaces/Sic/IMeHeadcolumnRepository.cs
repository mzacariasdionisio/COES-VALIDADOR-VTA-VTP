using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_HEADCOLUMN
    /// </summary>
    public interface IMeHeadcolumnRepository
    {
        void Save(MeHeadcolumnDTO entity);
        void Update(MeHeadcolumnDTO entity);
        void Delete(int formato, int hoja, int empresa, int pos);
        MeHeadcolumnDTO GetById(int formato, int hoja, int empresa, int pos);
        List<MeHeadcolumnDTO> List();
        List<MeHeadcolumnDTO> GetByCriteria(int formato,int empresa);
    }
}
