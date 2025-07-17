using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase de mapeo de la vista WV_SI_EMPRESA
    /// </summary>
    public class EmpresaDTO
    {
        public System.Int32 EmprCodi { get; set; }
        public System.String EmprNombre { get; set; }
        public System.String EmprAbrev { get; set; }
        public System.Int32 TipoEmprCodi { get; set; }
        public System.String Mensaje { get; set; }
        public System.String EmprAbrevCodi { get; set; }
        
    }
}
