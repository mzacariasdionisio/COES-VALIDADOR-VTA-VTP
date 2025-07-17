using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_PERFIL_RULE_AREA
    /// </summary>
    public class MePerfilRuleAreaDTO : EntityBase
    {
        public int Areacode { get; set; } 
        public int Prrucodi { get; set; } 
    }
}

