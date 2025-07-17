using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_CONGESTION_CONFIG
    /// </summary>
    public interface IEqCongestionConfigRepository
    {
        int Save(EqCongestionConfigDTO entity);
        void Update(EqCongestionConfigDTO entity);
        void Delete(int configcodi);
        EqCongestionConfigDTO GetById(int configcodi);
        List<EqCongestionConfigDTO> List();
        List<EqCongestionConfigDTO> GetByCriteria(int idEmpresa, string estado, int idGrupo, int idFamilia);
        List<EqCongestionConfigDTO> ObtenerPorGrupo(int idGrupo);
        List<SiEmpresaDTO> ObtenerEmpresasFiltro();
        List<SiEmpresaDTO> ObtenerEmpresasLineas();
        List<EqEquipoDTO> ListarEquipoLineaPorEmpresa(int idEmpresa, int idFamilia);
        int ValidarExistencia(int idEquipo);
        List<EqCongestionConfigDTO> ObtenerLineasPorGrupo(int idGrupo);
        List<EqCongestionConfigDTO> ObtenerListadoEquipos();

    }
}
