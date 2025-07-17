using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_EVENEQUIPO
    /// </summary>
    public interface IEveEvenequipoRepository
    {
        void Save(EveEvenequipoDTO entity);
        void Update(EveEvenequipoDTO entity);
        void Delete(int evencodi, int emprcodi);
        EveEvenequipoDTO GetById(int evencodi, int emprcodi);
        List<EveEvenequipoDTO> List();
        List<EveEvenequipoDTO> GetByCriteria();
        List<EqEquipoDTO> GetEquiposPorEvento(string idEvento);
        void DeleteEquipos(int evencodi);
    }
}
