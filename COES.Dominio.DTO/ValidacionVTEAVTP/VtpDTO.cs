
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.ValidacionVTEAVTP
{

    public class VtpDTO { 
        public int Resultado { get; set; }

        [JsonProperty("Table_ANA")]
        public List<TablaAnaResultDTO> TableAnas { get; set; }
        [JsonProperty("Table_VTP_SIN_ANALIZAR")]
        public List<TablaVtpSinAnalizarResultDTO> TableVtpSinAnalizar { get; set; }
        [JsonProperty("Table_VTP_BRG")]
        public List<TableVtpBrgResultDTO> TableVtpBrg { get; set; }
        [JsonProperty("Table_VTP_NO_BRG")]
        public List<TablaVtpNoBrgResultDTO> TableVtpNoBrg { get; set; }
    }

    public class TablaAnaResultDTO {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("potenciacoincidente")]
        public double? PotenciaCoincidente { get; set; }

        [JsonProperty("potenciadeclarada")]
        public double? PotenciaDeclarada { get; set; }

        [JsonProperty("diferencia")]
        public double? Diferencia { get; set; }

    }

    public class TablaVtpSinAnalizarResultDTO
    {
       
    }

    public class TableVtpBrgResultDTO
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("tipousuario")]
        public string TipoUsuario { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("potenciacoincidente")]
        public double? PotenciaCoincidente { get; set; }

        [JsonProperty("potenciadeclarada")]
        public double? PotenciaDeclarada { get; set; }

        [JsonProperty("PPM")]
        public double? Ppm { get; set; }

        [JsonProperty("VTP PPM")]
        public double? VtpPpm { get; set; }

        [JsonProperty("Error PPM")]
        public double? ErrorPpm { get; set; }

        [JsonProperty("Peaje")]
        public double? Peaje { get; set; }

        [JsonProperty("VTP Peaje")]
        public double? VtpPeaje { get; set; }

        [JsonProperty("Error Peaje")]
        public double? ErrorPeaje { get; set; }
    }

    public class TablaVtpNoBrgResultDTO
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("tipousuario")]
        public string TipoUsuario { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("potenciacoincidente")]
        public double? PotenciaCoincidente { get; set; }

        [JsonProperty("potenciadeclarada")]
        public double? PotenciaDeclarada { get; set; }

        [JsonProperty("PPM")]
        public double? Ppm { get; set; }

        [JsonProperty("VTP PPM")]
        public double? VtpPpm { get; set; }

        [JsonProperty("Error PPM")]
        public double? ErrorPpm { get; set; }

        [JsonProperty("Peaje")]
        public double? Peaje { get; set; }

        [JsonProperty("VTP Peaje")]
        public double? VtpPeaje { get; set; }

        [JsonProperty("Error Peaje")]
        public double? ErrorPeaje { get; set; }
    }

}
