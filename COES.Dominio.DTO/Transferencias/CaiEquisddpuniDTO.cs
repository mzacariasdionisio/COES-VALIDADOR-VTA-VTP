using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_EQUISDDPUNI
    /// </summary>
    public class CaiEquisddpuniDTO : EntityBase
    {
        public int Casdducodi { get; set; } 
        public int Equicodi { get; set; } 
        public string Casdduunidad { get; set; }
        public DateTime? Casddufecvigencia { get; set; } 
        public string Casdduusucreacion { get; set; } 
        public DateTime Casddufeccreacion { get; set; } 
        public string Casdduusumodificacion { get; set; } 
        public DateTime Casddufecmodificacion { get; set; }
        //Atributos de consulta
        public string Equinomb { get; set; }
    }
}
