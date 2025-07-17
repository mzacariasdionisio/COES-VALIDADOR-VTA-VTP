using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_GENERACION
    /// </summary>
    public interface ICmGeneracionRepository
    {
        int Save(CmGeneracionDTO entity);
        void Update(CmGeneracionDTO entity);
        void Delete(int genecodi);
        CmGeneracionDTO GetById(int genecodi);
        List<CmGeneracionDTO> List();
        List<CmGeneracionDTO> GetByCriteria();
        void DeleteByCriteria(int intervalo, DateTime fecha);

        List<CmGeneracionDTO> ListByEmpresa(int emprcodi, DateTime fecha);
        #region FIT - VALORIZACION DIARIA
        decimal ProducionEnergiaByDate(DateTime fecha);
        #endregion
    }
}
