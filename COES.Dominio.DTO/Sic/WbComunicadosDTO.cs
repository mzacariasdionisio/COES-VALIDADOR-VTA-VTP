using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_COMUNICADOS
    /// </summary>
    public class WbComunicadosDTO : EntityBase
    {
        public int Comcodi { get; set; } 
        public DateTime? Comfecha { get; set; } 
        public string Comtitulo { get; set; }
        public string Comresumen { get; set; }
        public string Comdesc { get; set; } 
        public string Comlink { get; set; } 
        public string Comestado { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Comfechaini { get; set; } 
        public DateTime? Comfechafin { get; set; } 
        public int? Modcodi { get; set; } 
        //Campo agregado para sala de prensa
        public string Comtipo { get; set; }

        public string ComImagen { get; set; }

        #region MigracionSGOCOES-GrupoB
        public int? Comorden { get; set; }
        public int? Composition { get; set; }
        public string ComfechaDesc { get; set; }
        public string ComfechainiDesc { get; set; }
        public string ComfechafinDesc { get; set; }
        #endregion
    }
}
