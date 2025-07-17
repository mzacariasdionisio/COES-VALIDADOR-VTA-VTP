using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea un objeto Central Única
    /// </summary>
    public class RerCentralUnicaDTO : EntityBase
    {
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public DateTime Rercenfechainicio { get; set; }
        public decimal Rercenenergadj { get; set; }
        public decimal Rercenprecbase { get; set; }
        public decimal Rerceninflabase { get; set; }
        public decimal FactorActualizacionAnterior { get; set; }
        public RerCalculoAnualDTO CalculoAnual { get; set; }
        public List<RerCalculoMensualDTO> ListCalculoMensual { get; set; }
    }
}