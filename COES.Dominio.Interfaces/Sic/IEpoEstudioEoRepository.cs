using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_ESTUDIO_EO
    /// </summary>
    public interface IEpoEstudioEoRepository
    {
        int Save(EpoEstudioEoDTO entity);
        void Update(EpoEstudioEoDTO entity);
        void Delete(int esteocodi);
        EpoEstudioEoDTO GetById(int esteocodi);
        List<EpoEstudioEoDTO> List();
        List<EpoEstudioEoDTO> GetByCriteria(EpoEstudioEoDTO estudioeo);
        int ObtenerNroRegistroBusqueda(EpoEstudioEoDTO estudioeo);
        List<EpoEstudioEoDTO> ListFwUser();

        #region Mejoras EO-EPO
        List<EpoEstudioEoDTO> ListVigencia12Meses();
        
        #endregion

        #region Ficha Tecnica
        List<EpoEstudioEoDTO> ListarPorEmpresa(int idEmpresa);
        #endregion
    }
}
