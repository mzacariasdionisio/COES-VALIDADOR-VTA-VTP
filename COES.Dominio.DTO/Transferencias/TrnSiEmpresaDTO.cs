using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    public class TrnSiEmpresaDTO : EntityBase
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
        public DateTime EmprestadoFecha { get; set; }
        public string TiposEmpresas { get; set; }
        public string Emprusucreacion { get; set; }
        public DateTime? Emprfeccreacion { get; set; }
        public string Emprusumodificacion { get; set; }
        public DateTime? Emprfecmodificacion { get; set; }
        public string Emprindusutramite { get; set; }
        public DateTime? Emprfecusutramite { get; set; }



        #region Titularidad-Instalaciones-Empresas

        public string EmprestadoDesc { get; set; }
        public string EmprcoesDesc { get; set; }
        public string EmprseinDesc { get; set; }
        public string EmpragenteDesc { get; set; }

        #endregion
    }


}

