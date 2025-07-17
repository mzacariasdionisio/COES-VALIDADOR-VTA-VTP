using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_NUMHISTORIA
    /// </summary>
    public class SpoNumhistoriaDTO : EntityBase
    {
        public int Numhiscodi { get; set; } 
        public int? Numecodi { get; set; } 
        public string Numhisdescripcion { get; set; } 
        public string Numhisabrev { get; set; } 
        public DateTime? Numhisfecha { get; set; } 
        public string Numhisusuario { get; set; } 
    }
}
