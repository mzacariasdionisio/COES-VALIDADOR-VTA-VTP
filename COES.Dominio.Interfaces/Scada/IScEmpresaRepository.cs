using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SC_EMPRESA
    /// </summary>
    public interface IScEmpresaRepository
    {
        int Save(ScEmpresaDTO entity);
        void Update(ScEmpresaDTO entity);
        void Delete(int emprcodi);
        ScEmpresaDTO GetById(int emprcodi);
        List<ScEmpresaDTO> List();
        List<ScEmpresaDTO> GetByCriteria();
        ScEmpresaDTO GetInfoScEmpresa(int emprcodi);
        List<ScEmpresaDTO> GetListaScEmpresa();

        #region FIT - SEÑALES NO DISPONIBLES - ASOCIACION EMPRESAS
        List<ScEmpresaDTO> ObtenerBusquedaAsociocionesEmpresa(string nombre);
        int GrabarAsociacionEmpresa(int emprcodi, int emprcodisp7, string emprusumodificacion);
        void NuevoAsociacionEmpresa(ScEmpresaDTO entity);
        void EliminarAsociacionEmpresa(int emprcodi);
        #endregion
    }
}
