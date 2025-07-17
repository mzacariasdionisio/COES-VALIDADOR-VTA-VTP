using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_INTERRUPCION_SUMINISTRO
    /// </summary>
    public interface IReInterrupcionSuministroRepository
    {
        int Save(ReInterrupcionSuministroDTO entity);
        void Update(ReInterrupcionSuministroDTO entity);
        void Delete(int reintcodi);
        ReInterrupcionSuministroDTO GetById(int reintcodi);
        List<ReInterrupcionSuministroDTO> List();
        List<ReInterrupcionSuministroDTO> GetByCriteria();
        List<ReInterrupcionSuministroDTO> ObtenerPorEmpresaPeriodo(int idEmpresa, int idPeriodo);
        void AnularInterrupcion(int id, string comentario, string username);
        List<ReInterrupcionSuministroDTO> ObtenerInterrupcionesPorResponsable(int idEmpresa, int idPeriodo);
        List<ReInterrupcionSuministroDTO> ObtenerConsolidado(int idPeriodo, int suministrador, int idCausaInterrupcion,
            string estado, int ptoEntrega, string final, int responsable, string disposicion, string compensacion);
        void ActualizarDecisionControveria(int id, string decision, string comentario);
        List<ReInterrupcionSuministroDTO> ObtenerTrazabilidad(int idPeriodo, int idSuministrador);
        void ActualizarArchivo(int id, string extension);
        List<ReInterrupcionSuministroDTO> ObtenerNotificacionInterrupcion(List<int> ids);

        void ActualizarResarcimiento(int id, decimal ei, decimal resarcimiento);
    }
}
