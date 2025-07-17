using System;

namespace COES.Dominio.DTO.Transferencias
{
    public class CpaDocumentosDetalleDTO
    {
        public int Cpaddtcodi { get; set; }
        public int Cpadoccodi { get; set; }            
        public string Cpaddtruta { get; set; } 
        public string Cpaddtnombre { get; set; } 
        public string Cpaddttamano { get; set; }
        public DateTime Cpaddtfeccreacion { get; set; } 
        public string Cpaddtusucreacion { get; set; }

    }
}
