using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_EVENTOAGENDA
    /// </summary>
    public interface IWbEventoagendaRepository
    {
        int Save(WbEventoagendaDTO entity);
        void Update(WbEventoagendaDTO entity);
        void Delete(int eveagcodi);
        WbEventoagendaDTO GetById(int eveagcodi);
        List<WbEventoagendaDTO> List(int tipoEvento, string anio);
        List<WbEventoagendaDTO> GetByCriteria(int tipoEvento, DateTime fecha);
    }
}
