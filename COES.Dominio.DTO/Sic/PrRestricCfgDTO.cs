using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_RESTRIC_CFG
    /// </summary>
    public partial class PrRestricCfgDTO : EntityBase
    {
        public int Rescfgcodi { get; set; } 
        public string Rescfgnombre { get; set; } 
        public string Rescfgusucreacion { get; set; } 
        public DateTime? Rescfgfeccreacion { get; set; } 
        public string Rescfgusumodificacion { get; set; } 
        public string Rescfgestado { get; set; } 
        public int Restipcodi { get; set; } 
        public DateTime? Rescfgfecmodificacion { get; set; } 
        public string Rescfgtipoecuacion { get; set; } 
        public decimal? Rescfges { get; set; } 
        public decimal? Rescfgfs { get; set; } 
    }

    public partial class PrRestricCfgDTO : EntityBase
    {
        public string Resdatdato { get; set; }
        public string Resdatflag { get; set; }
        public List<PrRestricCfgdetDTO> ListaRelaciones { get; set; }
        public string RescfgfeccreacionDesc { get; set; }
        public string RescfgfecmodificacionDesc { get; set; }
        public string RescfgesDesc { get; set; }
        public string RescfgfsDesc { get; set; }
        public string Restipnombre { get; set; }

        //Datos restricciones
        public int Item { get; set; }
        public string Fecha { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string H3 { get; set; }
        public string H4 { get; set; }
        public string H5 { get; set; }
        public string H6 { get; set; }
        public string H7 { get; set; }
        public string H8 { get; set; }
        public string H9 { get; set; }
        public string H10 { get; set; }
        public string H11 { get; set; }
        public string H12 { get; set; }
        public string H13 { get; set; }
        public string H14 { get; set; }
        public string H15 { get; set; }
        public string H16 { get; set; }
        public string H17 { get; set; }
        public string H18 { get; set; }
        public string H19 { get; set; }
        public string H20 { get; set; }
        public string H21 { get; set; }
        public string H22 { get; set; }
        public string H23 { get; set; }
        public string H24 { get; set; }
        public string H25 { get; set; }
        public string H26 { get; set; }
        public string H27 { get; set; }
        public string H28 { get; set; }
        public string H29 { get; set; }
        public string H30 { get; set; }
        public string H31 { get; set; }
        public string H32 { get; set; }
        public string H33 { get; set; }
        public string H34 { get; set; }
        public string H35 { get; set; }
        public string H36 { get; set; }
        public string H37 { get; set; }
        public string H38 { get; set; }
        public string H39 { get; set; }
        public string H40 { get; set; }
        public string H41 { get; set; }
        public string H42 { get; set; }
        public string H43 { get; set; }
        public string H44 { get; set; }
        public string H45 { get; set; }
        public string H46 { get; set; }
        public string H47 { get; set; }
        public string H48 { get; set; }
    }
}
