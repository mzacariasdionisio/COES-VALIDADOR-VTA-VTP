using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_CONGESGDESPACHO
    /// </summary>
    public interface IEveCongesgdespachoRepository
    {
        int Save(EveCongesgdespachoDTO entity);
        void Update(EveCongesgdespachoDTO entity);
        void UpdateEstado(EveCongesgdespachoDTO entity);
        void Delete(int congdecodi);
        List<EveCongesgdespachoDTO> GetById(int congdecodi);
        List<EveCongesgdespachoDTO> List();
        List<EveCongesgdespachoDTO> GetByCriteria();
        List<EveCongesgdespachoDTO> BuscarOperacionesCongestion(DateTime fechaInicio, DateTime fechaFinal);
    }
}
