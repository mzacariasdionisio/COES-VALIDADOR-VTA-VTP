using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_EVENTO_PERIODO
    /// </summary>
    public interface IReEventoPeriodoRepository
    {
        int Save(ReEventoPeriodoDTO entity);
        void Update(ReEventoPeriodoDTO entity);
        void Delete(int reevecodi);
        ReEventoPeriodoDTO GetById(int reevecodi);
        List<ReEventoPeriodoDTO> List();
        List<ReEventoPeriodoDTO> GetByCriteria(int periodo);
        List<ReEventoPeriodoDTO> ObtenerEventosUtilizadosPorPeriodo(int periodo, int idEmpresa);
    }
}
