using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_PERFIL_RULE_AREA
    /// </summary>
    public interface IMePerfilRuleAreaRepository
    {
        void Save(MePerfilRuleAreaDTO entity);
        void Update(MePerfilRuleAreaDTO entity);
        void Delete(int prrucodi);
        MePerfilRuleAreaDTO GetById(int areacode, int prrucodi);
        List<MePerfilRuleAreaDTO> List();
        List<MePerfilRuleAreaDTO> GetByCriteria(int id);
    }
}

