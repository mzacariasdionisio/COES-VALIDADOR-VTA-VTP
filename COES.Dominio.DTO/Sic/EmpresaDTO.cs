using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class EmpresaDTO
    {
        public short EMPRCODI { get; set; }
        public string EMPRNOMB { get; set; }
        public short TIPOEMPRCODI { get; set; }
        public string EMPRDIRE { get; set; }
        public string EMPRTELE { get; set; }
        public string EMPRNUMEDOCU { get; set; }
        public string TIPODOCUCODI { get; set; }
        public string EMPRRUC { get; set; }
        public string EMPRABREV { get; set; }
        public Nullable<int> EMPRORDEN { get; set; }
        public string EMPRDOM { get; set; }
        public string EMPRSEIN { get; set; }
        public string EMPRRAZSOCIAL { get; set; }
        public string EMPRCOES { get; set; }
        public string LASTUSER { get; set; }
        public string EMPRESTADO { get; set; }
        public int SCADACODI { get; set; }
        public string EMPRINDPROVEEDOR { get; set; }
    }  
  
}
