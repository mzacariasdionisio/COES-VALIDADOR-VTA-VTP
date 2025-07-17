using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AGC_CONTROL
    /// </summary>
    public class ReclamoDTO : EntityBase
    {
        public int AFECODI { get; set; }
        public int TIPCODI { get; set; }
        public int RECLAMOCODI { get; set; }
        public int RPTACODI { get; set; }
        public int EMPRCODI { get; set; }
        public string EMPRNOMB { get; set; }
        public string FECHA { get; set; }
        public string LASTUSER { get; set; }
        public string LASTDATE { get; set; }
        public string FECHA_COES { get; set; }
        public string LASTUSER_COES { get; set; }
        public string LASTDATE_COES { get; set; }
        public string AFREMPFECHA { get; set; }
        public string AFREMPPUBLICALASTUSER { get; set; }
        public string AFREMPPUBLICALASTDATE { get; set; }
        public string AFRCOESFECHA { get; set; }
        public string AFRCOESPUBLICALASTUSER { get; set; }
        public string AFRCOESPUBLICALASTDATE { get; set; }
        public string ANIO { get; set; }

    }
}
