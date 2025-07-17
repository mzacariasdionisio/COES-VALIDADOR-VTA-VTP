using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AF_EMPRESA
    /// </summary>
    public class AfEmpresaDTO : EntityBase
    {
        public int Afemprestado { get; set; }
        public string Afemprosinergmin { get; set; }
        public int Emprcodi { get; set; }
        public string Afemprusumodificacion { get; set; }
        public string Afemprusucreacion { get; set; }
        public DateTime? Afemprfecmodificacion { get; set; }
        public DateTime? Afemprfeccreacion { get; set; }
        public string Afemprnomb { get; set; }
        public int Afemprcodi { get; set; }
        public string Afalerta { get; set; }
    }
}
