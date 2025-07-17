using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_PROPIEDAD
    /// </summary>
    public interface IMpPropiedadRepository
    {
        int Save(MpPropiedadDTO entity);
        void Update(MpPropiedadDTO entity);
        void Delete(int mpropcodi);
        MpPropiedadDTO GetById(int mpropcodi);
        List<MpPropiedadDTO> List();
        List<MpPropiedadDTO> GetByCriteria();
    }
}
