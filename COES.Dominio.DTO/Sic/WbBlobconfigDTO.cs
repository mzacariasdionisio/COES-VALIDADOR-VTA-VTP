using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_BLOBCONFIG
    /// </summary>
    public class WbBlobconfigDTO : EntityBase
    {
        public int Configcodi { get; set; }
        public string Usercreate { get; set; }
        public DateTime? Datecreate { get; set; }
        public string Userupdate { get; set; }
        public DateTime? Dateupdate { get; set; }
        public string Configname { get; set; }
        public string Configstate { get; set; }
        public string Configdefault { get; set; }
        public string Configorder { get; set; }
        public string Configespecial { get; set; }
        public int? Columncodi { get; set; }
        public int? Blofuecodi { get; set; }
    }
}
