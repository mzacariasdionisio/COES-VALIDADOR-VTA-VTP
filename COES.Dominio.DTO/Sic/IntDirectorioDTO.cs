using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla INT_DIRECTORIO
    /// </summary>
    public class IntDirectorioDTO : EntityBase
    {
        public int Dircodi { get; set; } 
        public string Dirnombre { get; set; } 
        public string Dirapellido { get; set; } 
        public string Dircorreo { get; set; } 
        public string Diranexo { get; set; } 
        public string Dirtelefono { get; set; } 
        public string Dirfuncion { get; set; } 
        public int? Areacodi { get; set; } 
        public DateTime? Dircumpleanios { get; set; } 
        public string Dirfoto { get; set; } 
        public string Direstado { get; set; } 
        public int? Dirpadre { get; set; } 
        public int? Usercode { get; set; } 
        public string Dirusucreacion { get; set; } 
        public DateTime? Dirfeccreacion { get; set; } 
        public string Dirusumodificacion { get; set; } 
        public DateTime? Dirfecmodificacion { get; set; } 
        public string Dircargo { get; set; } 
        public string Dirapoyo { get; set; } 
        public string Dirorganigrama { get; set; } 
        public string Dirsecretaria { get; set; } 
        public string Dirsuperiores { get; set; } 
        public string Dirindsuperior { get; set; } 
        public int? Dirnivel { get; set; } 
    }
}
