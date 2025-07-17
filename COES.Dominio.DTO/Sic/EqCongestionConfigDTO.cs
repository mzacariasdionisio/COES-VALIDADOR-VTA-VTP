using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_CONGESTION_CONFIG
    /// </summary>
    public class EqCongestionConfigDTO : EntityBase
    {               
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public int? Grulincodi { get; set; } 
        public int Configcodi { get; set; } 
        public int Equicodi { get; set; }         
        public string Estado { get; set; }
        public string Nombrencp { get; set; }
        public int? Codincp;        
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public int Emprcodi { get; set; }
        public string Grulinnomb { get; set; }  
        public string Nodobarra1 { get; set; }
        public string Nodobarra2 { get; set; }
        public string Nodobarra3 { get; set; }       
        public string Idems { get; set; }
        public int? Famcodi { get; set; }

        //- Campos EMS TNA
        public string Nombretna1 { get; set; }
        public string Nombretna2 { get; set; }
        public string Nombretna3 { get; set; }
        //- Fin campos EMS TNA

        #region Mejoras CMgN
        public int? Canalcodi { get; set; }        
        #endregion
    }
}
