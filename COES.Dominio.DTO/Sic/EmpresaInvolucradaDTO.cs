using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Web;
namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AGC_CONTROL
    /// </summary>
    public class EmpresaInvolucradaDTO : EntityBase
    {
        public int AFECODI { get; set; }
        public int EMPRCODI { get; set; }
        public string AFIVERSION { get; set; }
        public string EMPRNOMB { get; set; }
        public string VERSION { get; set; }
        public string CUMPLIMIENTO { get; set; }
        public string AFIEXTENSION { get; set; }
        public string AFIMENSAJE { get; set; }
        public string AFIPUBLICA { get; set; }
        public DateTime? AFIFECHAINF { get; set; }
        public string AFIFECHAINFEVE { get; set; }
        public string FileName { get; set; }
        public string AFIFECHAINFstr
        {
            get {
                if (AFIFECHAINF.HasValue)
                {
                    return AFIFECHAINF.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
            
        }
        public string LASTUSER { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string LASTDATEstr
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
               
        public string ANIO { get; set; }

    }
}
