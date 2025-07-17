using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AGC_CONTROL
    /// </summary>
    public class EmpresaRecomendacionDTO : EntityBase
    {
        public int EMPRCODI { get; set; }
        public int AFRCORR { get; set; }
        public string EMPRNOMB { get; set; }
        public string AFRRECOMEND { get; set; }
        public string LASTUSER { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string LASTDATEstr
        {
            get
            {
                if (LASTDATE.HasValue)
                {
                    return LASTDATE.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
        public string LASTDATEReg
        {
            get
            {
                if (LASTDATE.HasValue)
                {
                    return LASTDATE.Value.ToString("dd/MM/yyyy HH:mm:ss");
                }
                return "";
            }
        }

        public int AFRREC { get; set; }
        public int AFECODI { get; set; }
        public string CODIGO { get; set; }
        public string RECOMENDACION { get; set; }
        public string ESTADO { get; set; }
        public string IMPORTANTE { get; set; }
        public string RESPUESTA { get; set; }
        public string OBSERVACION { get; set; }
        public string ACCIONFINAL { get; set; }
        public string INDIMPORTANTE { get; set; }
        public string NROREGRESPUESTA { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string EVENRCMCTAF { get; set; }
        public int EVENCODI { get; set; }
        public string EVENASUNTO { get; set; }
        public int IDEQUIPO { get; set; }
        public int IDSUBESTACION { get; set; }
        
    }
}
