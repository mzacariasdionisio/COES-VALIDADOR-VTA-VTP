using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_EMPRESA_SP7
    /// </summary>
    public interface ITrEmpresaSp7Repository
    {
        void Update(TrEmpresaSp7DTO entity);
        void Delete();
        TrEmpresaSp7DTO GetById(int emprcodi);
        List<TrEmpresaSp7DTO> List();
        List<TrEmpresaSp7DTO> GetByCriteria();
        int Save(TrEmpresaSp7DTO trEmpresaSp7DTO);
        void ActualizarNombreEmpresa(int emprcodi, string emprnomb);

        #region Mejoras IEOD
        List<TrEmpresaSp7DTO> ListarEmpresaCanal();
        List<TrEmpresaSp7DTO> ListarEmpresaCanalBdTreal();
        #endregion
    }
}
