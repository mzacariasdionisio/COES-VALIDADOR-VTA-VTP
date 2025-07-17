using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_INFORME
    /// </summary>
    public interface IEveInformeRepository
    {
        int Save(EveInformeDTO entity);
        void Update(EveInformeDTO entity);
        void Delete(int eveninfcodi);
        EveInformeDTO GetById(int eveninfcodi);
        List<EveInformeDTO> List();
        List<EveInformeDTO> GetByCriteria();
        List<EveInformeDTO> ObtenerInformesEmpresa(int idEvento, int idEmpresa);
        List<EqEquipoDTO> ObtenerEquiposSeleccion(int idEmpresa);
        void FinalizarInforme(int idInforme, string indPlazo, string estado, string username);
        List<EveInformeDTO> ObtenerReporteEmpresa(int idEvento, string empresas);
        List<EveInformeDTO> ObtenerReporteEmpresaGeneral(int idEvento);
        List<EveInformeDTO> ObtenerEstadoReporte(int idEvento, string empresas);
        List<EveInformeDTO> ObtenerEmpresaInforme(string empresas);
        EveInformeDTO ObtenerInformePorTipo(int idEvento, int idEmpresa, string tipo);
        void RevisarInforme(int idInforme, string estado, string username);
    }
}
