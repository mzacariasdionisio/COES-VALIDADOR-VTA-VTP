using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_REVISION_EPO
    /// </summary>
    public interface IEpoRevisionEpoRepository
    {
        int Save(EpoRevisionEpoDTO entity);
        void Update(EpoRevisionEpoDTO entity);
        void Delete(int revepocodi);
        EpoRevisionEpoDTO GetById(int revepocodi);
        List<EpoRevisionEpoDTO> List();
        List<EpoRevisionEpoDTO> GetByCriteria(int esteocodi);
        List<EpoRevisionEpoDTO> GetByCriteriaRevisionEstudio(int diautil, int diautilvenc);
        List<EpoRevisionEpoDTO> GetByCriteriaEnvioTerceroInv(int diautil, int diautilvenc);
        #region Mejoras EO-EPO-II
        List<EpoRevisionEpoDTO> ListEposExcAbsObs();
        #endregion
        
    }
}
