using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_DISTELECTRICA_GENELE
    /// </summary>
    public class StDistelectricaGeneleDTO : EntityBase
    {
        public int Degelecodi { get; set; } 
        public int Strecacodi { get; set; }
        public int Stcntgcodi { get; set; } 
        public int Barrcodigw { get; set; }
        public int Stcompcodi { get; set; }
        public int Barrcodilm { get; set; }
        public int Barrcodiln { get; set; }
        public decimal Degeledistancia { get; set; }
        public string Degeleusucreacion { get; set; } 
        public DateTime Degelefeccreacion { get; set; } 
        //VARIABLES PARA REPORTE
        public string Equinomb { get; set; }
        public string Cmpmelcodelemento { get; set; } 
    }
}
