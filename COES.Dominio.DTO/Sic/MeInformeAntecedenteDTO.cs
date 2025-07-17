using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_INFORME_ANTECEDENTE
    /// </summary>
    public class MeInformeAntecedenteDTO : EntityBase
    {
        public int Infantcodi { get; set; } 
        public int? Infantorden { get; set; } 
        public string Intantcontenido { get; set; } 
        public string Intantestado { get; set; } 
        public string Intantusucreacion { get; set; } 
        public DateTime? Intantfeccreacion { get; set; } 
        public string Intantusumodificacion { get; set; } 
        public DateTime? Intantfecmodificacion { get; set; } 
    }
}
