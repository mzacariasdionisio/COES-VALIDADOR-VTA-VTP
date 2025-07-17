using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_EMPRESA_CORREO
    /// </summary>
    public class SiEmpresaCorreoDTO //: EntityBase
    {
        public int Empcorcodi { get; set; }
        public int Emprcodi { get; set; }
        public int? Modcodi { get; set; }
        public string Empcornomb { get; set; }
        public string Empcordesc { get; set; }
        public string Empcoremail { get; set; }
        public string Empcorestado { get; set; }
        public string Emprnomb { get; set; }
        public string Tipoemprnomb { get; set; }
        public string Indnotificacion { get; set; }
        public string Username { get; set; }
        public string Useremail { get; set; }
        public string Userlogin { get; set; }
        public string Empcorcargo { get; set; } 
        public string Empcortelefono { get; set; }
        public string Empcormovil { get; set; }
        public string Emprruc { get; set; }
        public string Emprcortipo { get; set; }
        public string Empcorindnotic { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
    }
}
