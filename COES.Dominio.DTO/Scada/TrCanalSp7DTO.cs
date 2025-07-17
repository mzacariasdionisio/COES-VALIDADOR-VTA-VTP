using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_CANAL_SP7
    /// </summary>
    public class TrCanalSp7DTO : EntityBase
    {
        public int? Canalmseg { get; set; } 
        public int Canalcodi { get; set; } 
        public decimal? Canalvalor { get; set; } 
        public int? Alarmcodi { get; set; } 
        public int? Canalcalidad { get; set; } 
        public DateTime? Canalfhora { get; set; } 
        public string Canalnomb { get; set; } 
        public int? Emprcodi { get; set; } 
        public string Canaliccp { get; set; } 
        public int? Canaltdato { get; set; } 
        public string Canalunidad { get; set; }
        public string CanalPointType { get; set; }
        public int? Zonacodi { get; set; } 
        public string Canaltipo { get; set; } 
        public string Canalabrev { get; set; } 
        public DateTime? Canalfhora2 { get; set; } 
        public string Canalcodscada { get; set; } 
        public int? Canalflags { get; set; } 
        public int? Canalcalidadforzada { get; set; } 
        public decimal? Canalvalor2 { get; set; } 
        public string Canalestado { get; set; } 
        public DateTime? Canalfhestado { get; set; } 
        public decimal? Alarmmin1 { get; set; } 
        public decimal? Alarmmax1 { get; set; } 
        public decimal? Alarmmin2 { get; set; } 
        public decimal? Alarmmax2 { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Canaldescripcionestado { get; set; } 
        public int? Canalprior { get; set; } 
        public int? Canaldec { get; set; } 
        public decimal? Canalntension { get; set; } 
        public string Canalinvert { get; set; } 
        public int? Canaldispo { get; set; } 
        public string Canalcritico { get; set; } 
        public string Canaliccpreenvio { get; set; } 
        public string Canalcelda { get; set; } 
        public string Canaldescrip2 { get; set; } 
        public string Rdfid { get; set; } 
        public int? Gisid { get; set; } 
        public string Pathb { get; set; } 
        public string PointType { get; set; } 
        public DateTime? Lastdatesp7 { get; set; } 
        public int? Gpscodi { get; set; }
        public string Emprnomb { get; set; }
        public string Zonanomb { get; set; }
        public string Zonaabrev { get; set; }
        public int TrEmprcodi { get; set; }
        public string TrEmprnomb { get; set; }
        public string TrEmprabrev { get; set; }
        public DateTime Canalfeccreacion { get; set; }
        public String Canalusucreacion { get; set; }

        #region FIT - Señales no Disponibles
        public string Motivo { get; set; }
        public string Tiempo { get; set; }
        public string Calidad { get; set; }
        public string Caida { get; set; }
        #endregion

        #region Mejoras IEOD
        
        public int? Tipoinfocodi { get; set; }
        public string Tipoinfoabrev { get; set; }
        public string CanalRemota { get; set; }
        public string CanalContenedor { get; set; }
        public string CanalEnlace { get; set; }

        #endregion

    }
}
