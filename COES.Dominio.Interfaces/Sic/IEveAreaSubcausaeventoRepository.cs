using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_AREA_SUBCAUSAEVENTO
    /// </summary>
    public interface IEveAreaSubcausaeventoRepository
    {
        int Save(EveAreaSubcausaeventoDTO entity);
        void Update(EveAreaSubcausaeventoDTO entity);
        void Delete(int arscaucodi);
        EveAreaSubcausaeventoDTO GetById(int arscaucodi);
        List<EveAreaSubcausaeventoDTO> List();
        List<EveAreaSubcausaeventoDTO> GetByCriteria();

        List<int> ListarSubcausacodiRegistrados();
        List<EveAreaSubcausaeventoDTO> ListBySubcausacodi(int subcausacodi, string estado);
    }
}
