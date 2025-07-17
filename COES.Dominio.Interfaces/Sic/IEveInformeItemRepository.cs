using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_INFORME_ITEM
    /// </summary>
    public interface IEveInformeItemRepository
    {
        int Save(EveInformeItemDTO entity);
        void Update(EveInformeItemDTO entity);
        void Delete(int infitemcodi);
        void DeletePorInforme(int idInforme);
        EveInformeItemDTO GetById(int infitemcodi);
        List<EveInformeItemDTO> List();
        List<EveInformeItemDTO> GetByCriteria(int idInforme);
        List<EveInformeItemDTO> ObtenerItemInformeEvento(int idInforme);
        List<EveInformeItemDTO> ObtenerItemInformeEvento(int idEvento, int idEmpresa);
        EveInformeItemDTO ObtenerItemInformePorId(int idItemCodi);
        bool VerificarExistencia(int idInforme, int itemNumber);
        void ActualizarTextoInforme(int idItemInforme, string comentario);
        void SaveConsolidado(int idEvento, int idInforme, string version);
        void DeleteConsolidado(int idEvento);
        List<EveInformeItemDTO> ObtenerInformeInterrupcion(int idEvento);
        void ActualizarInformeItem(int idItemInforme, int idPtoInterrupcion);
    }
}
