using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_EVENTO_SUMINISTRADOR
    /// </summary>
    public interface IReEventoSuministradorRepository
    {
        int Save(ReEventoSuministradorDTO entity);
        void Update(ReEventoSuministradorDTO entity);
        void Delete(int reevsucodi);
        ReEventoSuministradorDTO GetById(int reevsucodi);
        List<ReEventoSuministradorDTO> List();
        List<ReEventoSuministradorDTO> GetByCriteria();
        List<ReEmpresaDTO> ObtenerSuministradoresPorEvento(int idEvento);
        ReEventoSuministradorDTO ObtenerSuministrador(int idEvento, int idEmpresa);
        List<ReEventoSuministradorDTO> ListarPorEvento(int idEvento);
    }
}
