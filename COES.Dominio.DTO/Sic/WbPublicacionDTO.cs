using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_PUBLICACION
    /// </summary>
    public class WbPublicacionDTO : EntityBase
    {
        public int Publiccodi { get; set; }
        public string Publicnombre { get; set; }
        public string Publicestado { get; set; }
        public string Publicplantilla { get; set; }
        public string Publicasunto { get; set; }
        public string Publicemail { get; set; }
        public string Publicemail1 { get; set; }
        public string Publicemail2 { get; set; } 
        public int? Areacode { get; set; }
        public string Areaname { get; set; }
    }
}
