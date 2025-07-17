using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_CONFIGBARRA
    /// </summary>
    public interface ICmConfigbarraRepository
    {
        int Save(CmConfigbarraDTO entity);
        void Update(CmConfigbarraDTO entity);
        void Delete(int cnfbarcodi);
        CmConfigbarraDTO GetById(int cnfbarcodi);
        List<CmConfigbarraDTO> List();
        List<CmConfigbarraDTO> GetByCriteria(string estado, string publicacion);
        void UpdateCoordenada(CmConfigbarraDTO entity);

        #region Mejoras CMgN

        int ValidarRegistro(int recurcodi, int topcodi, int canalcodi, int cnfbarcodi);
        List<CmConfigbarraDTO> ObtenerBarrasYupana(int topcodi, int catcodi);
        int ValidarCodigoScada(int canalcodi);

        #endregion
    }
}
