using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EXT_ARCHIVO
    /// </summary>
    public class ExtArchivoDTO : EntityBase
    {
        public int? Emprcodi { get; set; } 
        public string Earcopiado { get; set; } 
        public int? Estenvcodi { get; set; } 
        public DateTime? Earfecha { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; } 
        public string Earip { get; set; } 
        public int? Usercode { get; set; } 
        public string Eararchruta { get; set; } 
        public string Eararchver { get; set; } 
        public decimal? Eararchtammb { get; set; } 
        public string Eararchnomb { get; set; } 
        public int? Etacodi { get; set; } 
        public int Earcodi { get; set; }

        public string Emprnomb { get; set; }
        public string Username { get; set; }
        public string Usertelf { get; set; }
        public string Estenvnomb { get; set; }
        public string Estado { get; set; }

    }
}
