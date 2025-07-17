using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la ENTIDAD
    /// </summary>
    public interface IEntidadListadoRepository
    {
        List<EntidadListadoDTO> ListMaestroEmpresa(string nombre);
        List<EntidadListadoDTO> ListMaestroUsuarioLibre(string nombre);
        List<EntidadListadoDTO> ListMaestroSuministrador(string nombre);
        List<EntidadListadoDTO> ListMaestroBarra(string nombre);
        List<EntidadListadoDTO> ListMaestroCentralGeneracion(string nombre);
        List<EntidadListadoDTO> ListMaestroGrupoGeneracion(string nombre);
        List<EntidadListadoDTO> ListMaestroModoOperacion(string nombre);
        List<EntidadListadoDTO> ListMaestroCuenca(string nombre);
        List<EntidadListadoDTO> ListMaestroEmbalse(string nombre);
        List<EntidadListadoDTO> ListMaestroLago(string nombre);
        List<EntidadListadoDTO> ListResutado(string entidad, string pendiente);
        string GetFechaUltSincronizacion();
        string SaveHomologacion(string query, string mapencodi);
        string ObtenerIdPendiente(string entidad, string codigo);

        string DeleteHomologacion(string query);
    }
}
