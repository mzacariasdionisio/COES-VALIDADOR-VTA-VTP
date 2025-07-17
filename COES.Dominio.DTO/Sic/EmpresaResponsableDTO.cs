using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AGC_CONTROL
    /// </summary>
    public class EmpresaResponsableDTO : EntityBase
    {
        public int AFECODI { get; set; }
        public int EMPRCODI { get; set; }
        public string EMPRNOMB { get; set; }
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
        public decimal AFIPORCENTAJE { get; set; }
        public int AFIDECIDE { get; set; }
        public string AFIMENSAJE { get; set; }
        public string AFIVERSION { get; set; }
        public string AFIFECHAINF { get; set; }
        public string AFIPUBLICA { get; set; }
    }
}
