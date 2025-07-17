using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_EVENCLASE
    /// </summary>
    public interface IEveEvenclaseRepository
    {
        void Save(EveEvenclaseDTO entity);
        void Update(EveEvenclaseDTO entity);
        void Delete(int evenclasecodi);
        EveEvenclaseDTO GetById(int evenclasecodi);
        List<EveEvenclaseDTO> List();
        List<EveEvenclaseDTO> GetByCriteria();

        #region PROGRAMACIONES
        List<EveEvenclaseDTO> ListarComboTiposProgramaciones(int iEscenario);
        #endregion
    }
}

