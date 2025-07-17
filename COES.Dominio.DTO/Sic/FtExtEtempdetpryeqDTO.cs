using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ETEMPDETPRYEQ
    /// </summary>
    public partial class FtExtEtempdetpryeqDTO : EntityBase
    {
        public int Feeprycodi { get; set; } 
        public int? Equicodi { get; set; } 
        public int Feepeqcodi { get; set; }  //Id
        public string Feepeqflagotraetapa { get; set; }
        public string Feepeqflagsistema { get; set; }
        
        public string Feepequsucreacion { get; set; } 
        public DateTime? Feepeqfeccreacion { get; set; } 
        public string Feepequsumodificacion { get; set; } 
        public DateTime? Feepeqfecmodificacion { get; set; } 
        public int? Grupocodi { get; set; }
        public string Feepeqestado{ get; set; }
    }

    public partial class FtExtEtempdetpryeqDTO
    {        
        public string FechaCreacionDesc { get; set; }
        public string FechaModificacionDesc { get; set; }

        public string Equinomb { get; set; }//
        public string Areanomb { get; set; }//
        public string Emprnomb { get; set; }//
        public string Famnomb { get; set; }//
        public string Catenomb { get; set; }//
        public int Emprcodi { get; set; }//

        public int? Famcodi { get; set; }//
        public int? Catecodi { get; set; }//
        public int Areacodi { get; set; }//
    }
}
