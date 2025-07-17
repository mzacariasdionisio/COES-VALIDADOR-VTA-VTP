using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_PERSONA
    /// </summary>
    public class SiPersonaDTO : EntityBase
    {
        public int Percodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public int? Tipopercodi { get; set; } 
        public string Pernomb { get; set; } 
        public string Perapellido { get; set; } 
        public string Perdni { get; set; }
        public string Pertelefono { get; set; } 
        public string Perfax { get; set; } 
        public string Percargo { get; set; } 
        public string Pertitulo { get; set; } 
        public string Peremail { get; set; } 
        public string Percelular { get; set; } 
        public string Perg1 { get; set; } 
        public string Perasunto { get; set; } 
        public string Perg2 { get; set; } 
        public string Perg3 { get; set; } 
        public string Perg4 { get; set; } 
        public string Perg5 { get; set; } 
        public string Perg6 { get; set; } 
        public string Perg7 { get; set; } 
        public int? Usercode { get; set; } 
        public string Perclientelibre { get; set; } 
        public string Percomision { get; set; } 
        public int? Areacodi { get; set; } 
        public string Perestado { get; set; } 
        public int? Perorden { get; set; } 
        public string Peradminrolturno { get; set; } 
        public string Perg8 { get; set; } 
        public string Perg9 { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
