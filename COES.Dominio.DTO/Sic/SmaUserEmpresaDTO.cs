using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_USER_EMPRESA
    /// </summary>
    public class SmaUserEmpresaDTO : EntityBase
    {
        public int Usercode { get; set; } 

        public int Emprcodi { get; set; } 
        
        // INNER JOIN
        public string Username { get; set; }

        public string Emprnomb { get; set; } 

        public SmaOfertaDTO OfertaAutogenerada { get; set; }
        public List<string> ListaCorreo { get; set; }
    }
}
