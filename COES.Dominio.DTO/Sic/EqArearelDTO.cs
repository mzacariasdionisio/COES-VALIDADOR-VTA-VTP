using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_AREAREL
    /// </summary>
    public class EqAreaRelDTO : EntityBase
    {
        public int AreaRlCodi { get; set; }
        public int? AreaPadre { get; set; }
        public int AreaCodi { get; set; }
        public DateTime? FechaDat { get; set; }
        public int? LastCodi { get; set; }
        public string Arearlusumodificacion { get; set; } 
        public DateTime? Arearlfecmodificacion { get; set; } 
        //Datos EQ_AREA
        public int? TAREACODI { get; set; }
        public string TAREANOMB { get; set; }
        public string AREAABREV { get; set; }
        public string AREANOMB { get; set; }
    }
}
