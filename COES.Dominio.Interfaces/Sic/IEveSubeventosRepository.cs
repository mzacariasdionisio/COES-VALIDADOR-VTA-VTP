using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_SUBEVENTOS
    /// </summary>
    public interface IEveSubeventosRepository
    {
        void Save(EveSubeventosDTO entity);
        void Update(EveSubeventosDTO entity);
        void Delete(int evencodi, int equicodi, DateTime subevenini);
        EveSubeventosDTO GetById(int evencodi, int equicodi, DateTime subevenini);
        List<EveSubeventosDTO> List();
        List<EveSubeventosDTO> GetByCriteria(int idEvento);
    }
}

