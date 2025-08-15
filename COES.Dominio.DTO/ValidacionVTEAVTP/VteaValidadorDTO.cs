
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.ValidacionVTEAVTP
{

    public class VteaDTO
    { 
        public int Resultado { get; set; }
        public string Mensaje { get; set; }

        [JsonProperty("Table_E_H")]
        public List<TableEH> TableEH { get; set; }

        [JsonProperty("Table_H_E")]
        public List<TableHE> TableHE { get; set; }

        [JsonProperty("Table_F_C")]
        public List<TableFC> TableFC { get; set; }

        [JsonProperty("RETIROS_NEGATIVOS")]
        public List<RetirosNegativos> RetirosNegativos { get; set; }

    }

    public class TableEH {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("inicio_contrato")]
        public string InicioContrato { get; set; }

        [JsonProperty("fin_contrato")]
        public string FinContrato { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

    }

    public class TableHE
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("inicio_contrato")]
        public string InicioContrato { get; set; }

        [JsonProperty("fin_contrato")]
        public string FinContrato { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }

    public class TableFC
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("inicio_contrato")]
        public string InicioContrato { get; set; }

        [JsonProperty("fin_contrato")]
        public string FinContrato { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }

    public class RetirosNegativos
    {
       //No encontré data para retiros negativos
    }

}
