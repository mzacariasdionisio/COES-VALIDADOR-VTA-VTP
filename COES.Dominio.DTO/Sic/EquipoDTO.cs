using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class EquipoDTO
    {
        public short EQUICODI { get; set; }
        public Nullable<short> EMPRCODI { get; set; }
        public Nullable<short> GRUPOCODI { get; set; }
        public Nullable<short> ELECODI { get; set; }
        public Nullable<short> FAMCODI { get; set; }
        public Nullable<short> AREACODI { get; set; }
        public string EQUIABREV { get; set; }
        public string EQUINOMB { get; set; }
        public string EQUIABREV2 { get; set; }        
        public Nullable<decimal> EQUITENSION { get; set; }
        public Nullable<short> EQUIPADRE { get; set; }
        public Nullable<decimal> EQUIPOT { get; set; }
        public string LASTUSER { get; set; }
        public Nullable<System.DateTime> LASTDATE { get; set; }
        public string ECODIGO { get; set; }
        public string EQUIESTADO { get; set; }
        public string OSIGRUPOCODI { get; set; }
        public Nullable<short> LASTCODI { get; set; }
        public Nullable<System.DateTime> EQUIFECHINIOPCOM { get; set; }
        public Nullable<System.DateTime> EQUIFECHFINOPCOM { get; set; }
       
        public string TAREAABREV { get; set; }
        public string AREANOMB { get; set; }
        public string FAMABREV { get; set; }
        public string EMPRENOMB { get; set; }
        public string DESCENTRAL { get; set; }
        public string Grupotipocogen { get; set; }

        #region SGOCOES func A
        public decimal DISMINUIDO { get; set; }
        public decimal INTERRUMPIDO { get; set; }
        #endregion
    }

}
