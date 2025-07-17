using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Models
{
    public class EmpresaModel
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
        public string Etiqueta { get; set; }

        public string EmprCodOsinergmin { get; set; }

        // Cambios realizados por Erick Alfaro - 06/06/2017
        public string Emprnombrecomercial { get; set; }
        public string Emprdomiciliolegal { get; set; }
        public string Emprsigla { get; set; }
        public string Emprnumpartidareg { get; set; }
        public string Emprtelefono { get; set; }
        public string Emprfax { get; set; }
        public string Emprpagweb { get; set; } 
        public int Emprnroregistro { get; set; }
    }
}