using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AGC_CONTROL
    /// </summary>
    public class EmpresaObservacionDTO : EntityBase
    {
        public int EMPRCODI { get; set; }
        public int AFOCORR { get; set; }
        public string EMPRNOMB { get; set; }
        public string AFOOBSERVAC { get; set; }
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
        public int AFECODI { get; set; }
        public int AFOOBS { get; set; }
        public string EVENTO { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Estado { get; set; }
        public string Importante { get; set; }
    }
}
