using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla VCE_HORA_OPERACION
    /// </summary>
    public class VceHoraOperacionDTO : EntityBase
    {
        public DateTime? Crhophorfinajust { get; set; }
        public DateTime? Crhophoriniajust { get; set; }
        public string Crhopcompordpard { get; set; }
        public string Crhopcompordarrq { get; set; }
        public string Crhopdesc { get; set; }
        public int? Crhopcausacodi { get; set; }
        public string Crhoplimtrans { get; set; }
        public int? Crhopsaislado { get; set; }
        public int? Subcausacodi { get; set; }
        public DateTime? Crhophorparada { get; set; }
        public DateTime? Crhophorarranq { get; set; }
        public DateTime Crhophorfin { get; set; }
        public DateTime Crhophorini { get; set; }
        public int Grupocodi { get; set; }
        public int Hopcodi { get; set; }
        public int PecaCodi { get; set; }

        //Adicionales       
        public String Empresa { get; set; }
        public String Central { get; set; }
        public String Grupo { get; set; }
        public String ModoOperacion { get; set; }
        public String TipoOperacion { get; set; }

        // DSH 10-08-2017 : Se agrego por requerimiento
        public int Hopcodi2 { get; set; }
        public DateTime Crhophorfin2 { get; set; }
        public DateTime Crhophorini2 { get; set; }
        public String TipoOperacion2 { get; set; }
    }
}
