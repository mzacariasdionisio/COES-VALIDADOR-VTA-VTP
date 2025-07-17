using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
 
    public class SiSolicitudAmpliacionDTO : EntityBase
    {
        public int SoliCodi { get; set; }
        public int MsgCodi { get; set; }
        public DateTime? AmpliFecha { get; set; }        
        public int EmprCodi { get; set; }
        public DateTime? AmpliFechaPlazo { get; set; }
        public string LastUser { get; set; }
        public DateTime? LastDate { get; set; }
        public int FormatCodi { get; set; }
        public int FDatCodi { get; set; }
        public int FlagTipo { get; set; }    
    }
}