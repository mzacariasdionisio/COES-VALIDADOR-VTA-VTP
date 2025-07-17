using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_TIPOEVENTO
    /// </summary>
    public interface IEveTipoeventoRepository
    {
        int Save(EveTipoeventoDTO entity);
        void Update(EveTipoeventoDTO entity);
        void Delete(int tipoevencodi);
        EveTipoeventoDTO GetById(int tipoevencodi);
        List<EveTipoeventoDTO> List();
        List<EveTipoeventoDTO> GetByCriteria();

        #region INTERVENCIONES
        List<EveTipoeventoDTO> ListarComboTiposIntervenciones(int iEscenario);
        #endregion
    }
}

