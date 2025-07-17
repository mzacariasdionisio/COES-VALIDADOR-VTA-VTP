using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_VERIFICACION_FORMATO
    /// </summary>
    public class MeVerificacionFormatoDTO : EntityBase
    {
        public int Formatcodi { get; set; }
        public int Verifcodi { get; set; }
        public string Fmtverifestado { get; set; }
        public string Fmtverifusucreacion { get; set; }
        public DateTime? Fmtveriffeccreacion { get; set; }
        public string Fmtverifusumodificacion { get; set; }
        public DateTime? Fmtveriffecmodificacion { get; set; }
        public string Verifnomb { get; set; }
        public string FmtverifestadoDescripcion { get; set; }
        public string Formatnomb { get; set; }
    }
}
