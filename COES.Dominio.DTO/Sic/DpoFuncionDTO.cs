using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DPO_FUNCION
    /// </summary>
    [Serializable]
    public class DpoFuncionDTO : EntityBase
    {
        public int Dpofnccodi { get; set; }
        public string Dpofncnombre { get; set; }
        public string Dpofnctipo { get; set; }
        public string Dpofncdescripcion { get; set; }
        public string Dpofncusucreacion { get; set; }
        public DateTime Dpofncfeccreacion { get; set; }
        public string Dpofncusumodificacion { get; set; }
        public DateTime Dpofncfecmodificacion { get; set; }
    }
}
