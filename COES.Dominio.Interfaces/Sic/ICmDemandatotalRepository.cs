using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_DEMANDATOTAL
    /// </summary>
    public interface ICmDemandatotalRepository
    {
        int Save(CmDemandatotalDTO entity);
        void Update(CmDemandatotalDTO entity);
        void Delete(int demacodi);
        CmDemandatotalDTO GetById(int demacodi);
        List<CmDemandatotalDTO> List();
        List<CmDemandatotalDTO> GetByCriteria();

        void DeleteByCriteria(int intervalo, DateTime fecha);
        CmDemandatotalDTO GetByDate(DateTime demafecha);

        #region Fit - VALORIZACION DIARIA
        CmDemandatotalDTO GetDemandaTotal(DateTime demafecha);
        #endregion
    }
}
