using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_REP_PROPIEDAD
    /// </summary>
    public interface ICbRepPropiedadRepository
    {
        int Save(CbRepPropiedadDTO entity);
        void Update(CbRepPropiedadDTO entity);
        void Delete(int cbrprocodi);
        CbRepPropiedadDTO GetById(int cbrprocodi);
        List<CbRepPropiedadDTO> List();
        List<CbRepPropiedadDTO> GetByCriteria();
    }
}
