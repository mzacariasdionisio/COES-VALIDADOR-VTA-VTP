using System;

namespace COES.Dominio.DTO.Transferencias
{
    public class CpaDocumentosDTO
    {
        public int Cpadoccodi { get; set; }            
        public int Emprcodi { get; set; }              
        public int Cparcodi { get; set; }
        public int Cpadoccodenvio { get; set; }
        public DateTime Cpadocfeccreacion { get; set; } 
        public string Cpadocusucreacion { get; set; }

        //ADICIONALES
        public int Cpaapanio { get; set; }
        public string Cpaapajuste { get; set; }
        public string Cparrevision { get; set; }
        public string Emprnomb { get; set; }
    }
}
