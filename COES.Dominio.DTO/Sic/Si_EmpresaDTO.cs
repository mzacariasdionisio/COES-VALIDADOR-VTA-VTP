using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_EMPRESA
    /// </summary>
    [Serializable]
    public class Si_EmpresaDTO : EntityBase
    {
        public int Emprcodi { get; set; } 
        public string Emprnomb { get; set; } 
        public int Tipoemprcodi { get; set; } 
        public string Emprdire { get; set; } 
        public string Emprtele { get; set; } 
        public string Emprnumedocu { get; set; } 
        public string Tipodocucodi { get; set; } 
        public string Emprruc { get; set; } 
        public string Emprabrev { get; set; } 
        public int? Emprorden { get; set; } 
        public string Emprdom { get; set; } 
        public string Emprsein { get; set; } 
        public string Emprrazsocial { get; set; } 
        public string Emprcoes { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public int? Compcode { get; set; } 
        public string Inddemanda { get; set; }
        public string UserEmail { get; set; }
        public string Emprestado { get; set; }
        public string Tipoemprdesc { get; set; }
        public int Scadacodi { get; set; }
        public string Etiqueta { get; set; }        
        public string EmprCodOsinergmin { get; set; }  
        public string Emprdomiciliada { get; set; }
        public string Emprambito { get; set; }
        public int Emprrubro { get; set; }
        public string Empragente { get; set; }
        public string EmprnombAnidado { get; set; }
        public List<PrGrupoDTO> ListaPrgrupo = new List<PrGrupoDTO>();
        public string Emprnombcomercial { get; set; }
        public string Emprdomiciliolegal { get; set; }
        public string Emprsigla { get; set; }
        public string Emprnumpartidareg { get; set; }
        public string Emprtelefono { get; set; }
        public string Emprfax { get; set; }
        public string Emprpagweb { get; set; }
        public string Emprcartadjunto { get; set; }
        public string Emprestadoregistro { get; set; }
        public DateTime? Emprfechacreacion { get; set; }
        public string Emprcartadjuntofilename { get; set; }
        public string Emprcondicion { get; set; }
        public int Emprnroconstancia { get; set; }
    }
}
