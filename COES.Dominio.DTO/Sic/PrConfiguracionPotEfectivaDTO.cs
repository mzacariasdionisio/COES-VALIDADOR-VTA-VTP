using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_CONFIGURACION_POT_EFECTIVA
    /// </summary>
    public class PrConfiguracionPotEfectivaDTO : EntityBase
    {
        public int Grupocodi { get; set; } 
        public string Confpeusuariocreacion { get; set; } 
        public DateTime Confpefechacreacion { get; set; } 
    }
}
