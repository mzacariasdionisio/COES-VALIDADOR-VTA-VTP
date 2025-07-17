using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_MEDBORNE
    /// </summary>
    public class VcrMedborneDTO : EntityBase
    {
        public int Vcrmebcodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int Equicodicen { get; set; } 
        public int Equicodiuni { get; set; } 
        public DateTime Vcrmebfecha { get; set; } 
        public string Vcrmebptomed { get; set; } 
        public decimal Vcrmebpotenciamed { get; set; } 
        public string Vcrmebusucreacion { get; set; } 
        public DateTime Vcrmebfeccreacion { get; set; } 
        //atributos de consulta
        public string Emprnomb { get; set; }
        public string Equinombcen { get; set; }
        public string Equinombuni { get; set; }
        public string Vcmbciconsiderar { get; set; }
        //ASSETEC: 202012
        public decimal Vcrmebpotenciamedgrp { get; set; }
        public decimal Vcrmebpresencia { get; set; }
    }
}
