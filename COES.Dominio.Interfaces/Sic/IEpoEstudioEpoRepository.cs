using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_ESTUDIO_EPO
    /// </summary>
    public interface IEpoEstudioEpoRepository
    {
        int Save(EpoEstudioEpoDTO entity);
        void Update(EpoEstudioEpoDTO entity);
        void Delete(int estepocodi);
        EpoEstudioEpoDTO GetById(int estepocodi);
        List<EpoEstudioEpoDTO> List();
        List<EpoEstudioEpoDTO> GetByCriteria(EpoEstudioEpoDTO estudioepo);
        int ObtenerNroRegistroBusqueda(EpoEstudioEpoDTO estudioepo);

        #region Mejoras EO-EPO
        List<EpoEstudioEpoDTO> ListVigenciaAnioIngreso(string FechaAnioIngreso);
        List<EpoEstudioEpoDTO> ListVigencia36Meses();
        List<EpoEstudioEpoDTO> ListVigencia48Meses();
        #endregion
    }
}
