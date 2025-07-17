using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_OFERTA
    /// </summary>
    public class VcrOfertaDTO : EntityBase
    {
        public int Vcrofecodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int Usercode { get; set; } 
        public string Vcrofecodigoenv { get; set; } 
        public DateTime? Vcrofefecha { get; set; } 
        public DateTime? Vcrofehorinicio { get; set; } 
        public DateTime? Vcrofehorfinal { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public string Vcrofemodoperacion { get; set; } 
        public decimal Vcrofepotofertada { get; set; } 
        public decimal Vcrofeprecio { get; set; } 
        public string Vcrofeusucreacion { get; set; } 
        public DateTime Vcrofefeccreacion { get; set; }
        //ASSETEC: 202010 - Nuevo atributo
        public int Vcrofetipocarga { get; set; }

        //VARIABLES QUE SE EMPLEAN EN LAS CONSULTAS
        public string Username { get; set; } 
    }
}
