using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_CONTACTO
    /// </summary>
    public class WbContactoDTO : EntityBase
    {
        public int Contaccodi { get; set; } 
        public string Contacnombre { get; set; } 
        public string Contacapellido { get; set; } 
        public string Contacemail { get; set; } 
        public string Contaccargo { get; set; } 
        public string Contacempresa { get; set; } 
        public string Contactelefono { get; set; } 
        public string Contacmovil { get; set; } 
        public string Contaccomentario { get; set; }
        public string Contacdoc { get; set;}
        public DateTime ContacFecRegistro { get; set; }
        public string Contacarea { get; set; } 
        public string Contacestado { get; set; }
        public int Emprcodi { get; set; }
        public int Tipoemprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Emprdire { get; set; }
        public string Fuente { get; set; }
        public string Userreplegal { get; set; }
        public string Usercontacto { get; set; }
        public string Tipoemprnomb { get; set; }
        public string Emprcoes { get; set; }
        public string Nombrecomercial { get; set; }
    }
}
