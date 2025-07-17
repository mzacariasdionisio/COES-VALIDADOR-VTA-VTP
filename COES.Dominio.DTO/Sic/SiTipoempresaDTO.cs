using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_TIPOEMPRESA
    /// </summary>
    public class SiTipoempresaDTO : EntityBase
    {
        public int Tipoemprcodi { get; set; } 
        public string Tipoemprdesc { get; set; } 
        public string Tipoemprabrev { get; set; } 
    }
}

