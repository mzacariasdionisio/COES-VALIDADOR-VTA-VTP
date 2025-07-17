using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_RECHAZO_CARGA
    /// </summary>
    public interface IReRechazoCargaRepository
    {
        int Save(ReRechazoCargaDTO entity);
        void Update(ReRechazoCargaDTO entity);
        void Delete(int rerccodi);
        ReRechazoCargaDTO GetById(int rerccodi);
        List<ReRechazoCargaDTO> List();
        List<ReRechazoCargaDTO> GetByCriteria();
        List<ReRechazoCargaDTO> ObtenerPorEmpresaPeriodo(int idEmpresa, int idPeriodo);
        void AnularRechazoCarga(int idRechazo, string comentario, string usuario);
        List<ReRechazoCargaDTO> ObtenerConsolidado(int periodo, int suministrador, int barra, string estado, int evento, string alimentadorSED, string final, int responsable, string disposicion);
        List<ReRechazoCargaDTO> ObtenerTrazabilidad(int periodo, int suministrador);
        List<ReRechazoCargaDTO> ObtenerNotificacionInterrupcion(List<int> ids);
        void ActualizarPorcentajes(ReRechazoCargaDTO entity);
        void ActualizarResarcimiento(int id, decimal resarcimiento);
    }
}
