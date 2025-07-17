using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_BLOB
    /// </summary>
    public class WbBlobDTO : EntityBase
    {
        public int Blobcodi { get; set; }
        public int? Configcodi { get; set; }
        public string Bloburl { get; set; }
        public int? Padrecodi { get; set; }
        public string Blobname { get; set; }
        public string Blobsize { get; set; }
        public DateTime? Blobdatecreated { get; set; }
        public string Blobusercreate { get; set; }
        public DateTime? Blobdateupdate { get; set; }
        public string Blobuserupdate { get; set; }
        public string Blobstate { get; set; }
        public string Blobtype { get; set; }
        public string Blobfoldertype { get; set; }
        public string Blobissuu { get; set; }
        public string Blobissuulink { get; set; }
        public string Blobissuupos { get; set; }
        public string Blobissuulenx { get; set; }
        public string Blobissuuleny { get; set; }
        public string Blobhiddcol { get; set; }
        public string Blobbreadname { get; set; }
        public string Bloborderfolder { get; set; }
        public string Blobhide { get; set; }
        public string Indtree { get; set; }
        public int? Blobtreepadre { get; set; }
        public int? Blobfuente { get; set; }
        public int? Blofuecodi { get; set; }
        public int? Blobconfidencial { get; set; }
    }
}
