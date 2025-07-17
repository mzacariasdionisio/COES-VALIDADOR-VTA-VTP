using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_CENTRAL_CODRETIRO
    /// </summary>
    public class RerCentralCodRetiroDTO : EntityBase
    {
        public int Rerccrcodi { get; set; }
        public int Rerpprcodi { get; set; }
        public int Rercencodi { get; set; }
        public int Coresocodi { get; set; }
        public string Rerccrusucreacion { get; set; }
        public DateTime Rerccrfeccreacion { get; set; }


        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public int Cantidad { get; set; }
        public string Equinomb { get; set; }
    }
}