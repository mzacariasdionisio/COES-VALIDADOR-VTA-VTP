
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.ValidacionVTEAVTP
{

    public class VtpValidacionDTO { 
        public ValorizacionDTO Valorizacion { get; set; }
        public PeajeDTO Peaje { get; set; } 
    }

    public class PeajeDTO {

        [JsonProperty("Table_ANA")]
        public List<TableAnaPeajeDTO> TableAnas { get; set; }
        [JsonProperty("Table_EVA")]
        public List<TablaEvaPeajeDTO> TableEvas { get; set; }
        [JsonProperty("Table_HIS")]
        public List<TablaHisPeajeDTO> TableHiss { get; set; }
    }

    public class TablaHisPeajeDTO {
        [JsonProperty("PERICODI")]
        public int Pericodi { get; set; }

        [JsonProperty("TIME")]
        public DateTime Time { get; set; }

        [JsonProperty("EMPRESA")]
        public string Empresa { get; set; }

        [JsonProperty("POTENCIADECLARADA")]
        public double PotenciaDeclarada { get; set; }

        [JsonProperty("RETIRONODECLARADO")]
        public double RetiroNoDeclarado { get; set; }

        [JsonProperty("POTENCIA")]
        public double Potencia { get; set; }

        [JsonProperty("POTENCIATOTAL")]
        public double PotenciaTotal { get; set; }

        [JsonProperty("PEAJETOTAL")]
        public double PeajeTotal { get; set; }

        [JsonProperty("COMP. PEAJE APROX.")]
        public double CompPeajeAprox { get; set; }

        [JsonProperty("COMP. PEAJE REAL")]
        public double CompPeajeReal { get; set; }

    }

    public class TablaEvaPeajeDTO {
        [JsonProperty("PERICODI")]
        public int Pericodi { get; set; }

        [JsonProperty("TIME")]
        public DateTime Time { get; set; }

        [JsonProperty("EMPRESA")]
        public string Empresa { get; set; }

        [JsonProperty("POTENCIADECLARADA")]
        public double PotenciaDeclarada { get; set; }

        [JsonProperty("RETIRONODECLARADO")]
        public double RetiroNoDeclarado { get; set; }

        [JsonProperty("POTENCIA")]
        public double Potencia { get; set; }

        [JsonProperty("POTENCIATOTAL")]
        public double PotenciaTotal { get; set; }

        [JsonProperty("PEAJETOTAL")]
        public double PeajeTotal { get; set; }

        [JsonProperty("COMP. PEAJE APROX.")]
        public double CompPeajeAprox { get; set; }

        [JsonProperty("COMP. PEAJE REAL")]
        public double CompPeajeReal { get; set; }
    }

    public class TableAnaPeajeDTO {
        [JsonProperty("EMPRESA")]
        public string Empresa { get; set; }

        [JsonProperty("COMP. PEAJE APROX.")]
        public double CompPeajeAprox { get; set; }

        [JsonProperty("COMP. PEAJE REAL")]
        public double CompPeajeReal { get; set; }

        [JsonProperty("PREDICCIÓN")]
        public double Prediccion { get; set; }

        [JsonProperty("ERROR")]
        public double Error { get; set; }

        [JsonProperty("ERROR(%)")]
        public double ErrorPorcentaje { get; set; }

        [JsonProperty("CALIDAD")]
        public string Calidad { get; set; }

        [JsonProperty("m")]
        public double M { get; set; }

        [JsonProperty("b")]
        public double B { get; set; }

        [JsonProperty("MEAN")]
        public double Mean { get; set; }

        [JsonProperty("STD")]
        public double Std { get; set; }
    }
    public class ValorizacionDTO
    {
        public int RecPortCodi { get; set; }

        [JsonProperty("Table_ANA")]
        public List<TableAnaValDTO> TableAnas { get; set; }

        [JsonProperty("Table_EVA")]
        public List<TableEvaValDTO> TableEvas { get; set; }

        [JsonProperty("Table_HIS")]
        public List<TableHisValDTO> TableHiss { get; set; }

    }
    public class TableHisValDTO {
        [JsonProperty("PERICODI")]
        public int Pericodi { get; set; }

        [JsonProperty("TIME")]
        public DateTime Time { get; set; }

        [JsonProperty("EMPRESA")]
        public string Empresa { get; set; }

        [JsonProperty("POTENCIA CONSUMIDA")]
        public double? PotenciaConsumida { get; set; }

        [JsonProperty("VALORIZACION")]
        public double? Valorizacion { get; set; }
    }
    
    public class TableEvaValDTO
    {
        [JsonProperty("PERICODI")]
        public int Pericodi { get; set; }

        [JsonProperty("TIME")]
        public DateTime Time { get; set; }

        [JsonProperty("EMPRESA")]
        public string Empresa { get; set; }

        [JsonProperty("POTENCIA CONSUMIDA")]
        public double? PotenciaConsumida { get; set; }

        [JsonProperty("VALORIZACION")]
        public double? Valorizacion { get; set; }
    }
    public class TableAnaValDTO {

        [JsonProperty("EMPRESA")]
        public string Empresa { get; set; }

        [JsonProperty("POTENCIA CONSUMIDA")]
        public double? PotenciaConsumida { get; set; }

        [JsonProperty("VALORIZACION")]
        public double? Valorizacion { get; set; }

        [JsonProperty("PREDICCIÓN")]
        public double? Prediccion { get; set; }

        [JsonProperty("ERROR")]
        public double? Error { get; set; }

        [JsonProperty("ERROR(%)")]
        public double? ErrorPorcentaje { get; set; }

        [JsonProperty("CALIDAD")]
        public string Calidad { get; set; }

        [JsonProperty("m")]
        public double? M { get; set; }

        [JsonProperty("b")]
        public double? B { get; set; }

        [JsonProperty("MEAN")]
        public double? Mean { get; set; }

        [JsonProperty("STD")]
        public double? Std { get; set; }
    }
}
