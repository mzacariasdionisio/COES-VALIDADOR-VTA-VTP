using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_REVISION_EO
    /// </summary>
    public interface IEpoRevisionEoRepository
    {
        int Save(EpoRevisionEoDTO entity);
        void Update(EpoRevisionEoDTO entity);
        void Delete(int reveocodi);
        EpoRevisionEoDTO GetById(int reveocodi);
        List<EpoRevisionEoDTO> List();
        List<EpoRevisionEoDTO> GetByCriteria(int esteocodi);
        List<EpoRevisionEoDTO> GetByCriteriaRevisionEstudio(int diautil, int diautilvenc);
        List<EpoRevisionEoDTO> GetByCriteriaEnvioTerceroInv(int diautil, int diautilvenc);
        #region Mejoras EO-EPO-II
        List<EpoRevisionEoDTO> ListEosExcAbsObs();
        #endregion
    }
}
