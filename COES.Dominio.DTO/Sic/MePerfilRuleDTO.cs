using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_PERFIL_RULE
    /// </summary>
    public class MePerfilRuleDTO : EntityBase
    {
        public int Prrucodi { get; set; }
        public string Prrupref { get; set; }
        public string Prruabrev { get; set; }
        public string Prrudetalle { get; set; }
        public string Prruformula { get; set; }
        public string Prruactiva { get; set; }
        public string Prrulastuser { get; set; }
        public string Prrufirstuser { get; set; }
        public DateTime? Prrulastdate { get; set; }
        public DateTime? Prrufirstdate { get; set; }
        public string Prruind { get; set; }
        

        public string EmprNomb { get; set; }
        public string AreaNomb { get; set; }
        public string EquiNomb { get; set; }
        public int PtoMediCodi { get; set; }
        public string PtoNomb { get; set; }
        public string PtoDescripcion { get; set; }
        public int EmprCodi { get; set; }
        public int Areacodi { get; set; }
        public int Tipoemprcodi { get; set; }

        public List<int> IdRoles { get; set; }
        public List<string> Data { get; set; }
        // -----------------------------------------------------------------------------------------------------------------
        // ASSETEC 04-04-2022
        // -----------------------------------------------------------------------------------------------------------------
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public bool TomarManual { get; set; }
        // -----------------------------------------------------------------------------------------------------------------
    }
}
