using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_ROL_TURNO
    /// </summary>
    public class SiRolTurnoDTO : EntityBase
    {
        public DateTime Roltfecha { get; set; }
        public int Actcodi { get; set; }
        public string Lastuser { get; set; }
        public DateTime Lastdate { get; set; }
        public int Percodi { get; set; }
        public string Actabrev { get; set; }
        public string Pernomb { get; set; }
        public string Actnomb { get; set; }
        public string Roltestado { get; set; }
        public DateTime Roltfechaactualizacion { get; set; }

        public int Tipoproceso { get; set; }//1=Insert 2=Update
        public string RoltfechaactualizacionDesc { get; set; }
    }
}
