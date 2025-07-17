using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_UMBRAL_COMPARACION
    /// </summary>
    public interface ICmUmbralComparacionRepository
    {
        int Save(CmUmbralComparacionDTO entity);
        void Update(CmUmbralComparacionDTO entity);
        void Delete(int cmumcocodi);
        CmUmbralComparacionDTO GetById(int cmumcocodi);
        List<CmUmbralComparacionDTO> List();
        List<CmUmbralComparacionDTO> GetByCriteria();
    }
}
