using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_MEDBORNECARGOINCP
    /// </summary>
    public class VcrMedbornecargoincpDTO : EntityBase
    {
        public int Vcmbcicodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int Equicodicen { get; set; } 
        public int Equicodiuni { get; set; } 
        public string Vcmbciconsiderar { get; set; } 
        public string Vcmbciusucreacion { get; set; } 
        public DateTime Vcmbcifeccreacion { get; set; } 

        //Atributos de consultas
        public string Emprnomb { get; set; }
        public string Equinombcen { get; set; }
        public string Equinombuni { get; set; } 
        
    }
}
