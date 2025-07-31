
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.ValidacionVTEAVTP
{

    public class VtpVteaDTO { 
        public int Resultado { get; set; }

        [JsonProperty("Table_X_Y")]
        public List<TableXY> TablesXY { get; set; }
        [JsonProperty("Table_Y_X")]
        public List<TableYX> TablesYX { get; set; }
        [JsonProperty("Tabla_D")]
        public List<TableD> TablesD { get; set; }

    }

    public class TableXY {
        [JsonProperty("codigo_vtea")]
        public string CodigoVtea { get; set; }

        [JsonProperty("codigo_vtp")]
        public string CodigoVtp { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("potencia_vtea")]
        public double? PotenciaVtea { get; set; }

        [JsonProperty("potencia_vtp")]
        public double? PotenciaVtp { get; set; }

    }

    public class TableYX
    {
        [JsonProperty("codigo_vtea")]
        public string CodigoVtea { get; set; }

        [JsonProperty("codigo_vtp")]
        public string CodigoVtp { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("potencia_vtea")]
        public double? PotenciaVtea { get; set; }

        [JsonProperty("potencia_vtp")]
        public double? PotenciaVtp { get; set; }
    }

    public class TableD
    {
        [JsonProperty("codigo_vtea")]
        public string CodigoVtea { get; set; }

        [JsonProperty("codigo_vtp")]
        public string CodigoVtp { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("potencia_vtea")]
        public double? PotenciaVtea { get; set; }

        [JsonProperty("potencia_vtp")]
        public double? PotenciaVtp { get; set; }

        [JsonProperty("diferencia")]
        public double? Diferencia { get; set; }

        [JsonProperty("error_vtea")]
        public double? ErrorVtea { get; set; }

        [JsonProperty("error_vtp")]
        public double? ErrorVtp { get; set; }
    }



}
