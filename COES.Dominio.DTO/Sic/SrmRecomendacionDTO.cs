using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SRM_RECOMENDACION
    /// </summary>
    public class SrmRecomendacionDTO : EntityBase
    {
        public int Srmreccodi { get; set; } 
        public int? Evencodi { get; set; } 
        public int? Equicodi { get; set; } 
        public int? Srmcrtcodi { get; set; } 
        public int? Srmstdcodi { get; set; } 
        public int? Usercode { get; set; } 
        public DateTime? Srmrecfecharecomend { get; set; } 
        public DateTime? Srmrecfechavencim { get; set; } 
        public int? Srmrecdianotifplazo { get; set; } 
        public string Srmrectitulo { get; set; } 
        public string Srmrecrecomendacion { get; set; } 
        public string Srmrecactivo { get; set; } 
        public string Srmrecusucreacion { get; set; } 
        public DateTime? Srmrecfeccreacion { get; set; } 
        public string Srmrecusumodificacion { get; set; } 
        public DateTime? Srmrecfecmodificacion { get; set; }
        public DateTime? Evenini { get; set; }
        public string Equiabrev { get; set; }
        public string Srmcrtdescrip { get; set; }
        public string Srmstddescrip { get; set; }
        public string Username { get; set; }

        public string Tipo { get; set; }
        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public DateTime? Evenfin { get; set; }
        public string EvenAsunto { get; set; }
        public string Equinomb { get; set; }
        public string Srmstdcolor { get; set; }        
        public string Srmcrtcolor { get; set; }
        public string Comentario { get; set; }
        public int Emprcodi { get; set; }
        public int Famcodi { get; set; }
        public string Famnomb { get; set; }

        public int Registros { get; set; }
        public decimal Porcentaje { get; set; }
        public string ConComentario { get; set; }



        public int Avencer { get; set; }
        public int Vencido { get; set; }
        public int Ciclico { get; set; }

        public string Fecharecomendacion { get; set; }
        public string Fechavencimiento { get; set; }    
        public string Evenrcmctaf { get; set; }
        public int Afrrec { get; set; }

    }
}
