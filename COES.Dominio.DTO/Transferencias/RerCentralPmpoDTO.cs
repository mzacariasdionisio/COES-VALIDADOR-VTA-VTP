using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_CENTRAL_PMPO
    /// </summary>
    public class RerCentralPmpoDTO : EntityBase
    {
        public int Rercpmcodi { get; set; }
        public int Rercencodi { get; set; }
        public int Ptomedicodi { get; set; }
        public string Rercpmusucreacion { get; set; }
        public DateTime Rercpmfeccreacion { get; set; }

        public string Ptomedidesc { get; set; }
        public int Fenergcodi { get; set; }
        public int Tgenercodi { get; set; }
        public string Grupotipocogen { get; set; }

    }
}