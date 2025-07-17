using System;

namespace COES.Dominio.DTO.Busqueda
{
    // Único DTO para las tablas Resultados_relacionados, Documentos_abiertos, Resultados_busquedas y BCD_Resultados_recomendados
    public class BCDResultadosRecomendadosDTO
    {
        public int Id_document { get; set; }
        public int Id_search_rr { get; set; }
        public string Index_rowkey { get; set; }
        public string Document_path { get; set; }
        public string Document_name { get; set; }
        public Nullable<decimal> Confidence { get; set; }
        public string Recommend_by { get; set; }
    }
}
