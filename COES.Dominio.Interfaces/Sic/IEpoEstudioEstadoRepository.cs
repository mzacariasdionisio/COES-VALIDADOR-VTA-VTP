using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_ESTUDIO_ESTADO
    /// </summary>
    public interface IEpoEstudioEstadoRepository
    {
        int Save(EpoEstudioEstadoDTO entity);
        void Update(EpoEstudioEstadoDTO entity);
        void Delete(int estacodi);
        EpoEstudioEstadoDTO GetById(int estacodi);
        List<EpoEstudioEstadoDTO> List();
        List<EpoEstudioEstadoDTO> GetByCriteria();
        #region Mejoras EO-EPO
        List<EpoEstudioEstadoDTO> GetByCriteriaEstadosVigencia();
        #endregion
    }
}
