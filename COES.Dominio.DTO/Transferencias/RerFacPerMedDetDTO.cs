using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_FAC_PER_MED_DET
    /// </summary>
    public class RerFacPerMedDetDTO : EntityBase
    {
        public int Rerfpdcodi { get; set; }
        public int Rerfpmcodi { get; set; }
        public int Codentcodi { get; set; }
        public int Barrcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public decimal Rerfpdfactperdida { get; set; }
        public string Rerfpdusucreacion { get; set; }
        public DateTime Rerfpdfeccreacion { get; set; }
        public string Rerfpdusumodificacion { get; set; }
        public DateTime Rerfpdfecmodificacion { get; set; }

        //Atributos de consulta
        public string Codentcodigo { get; set; }
        public string Barrnombre { get; set; }
        public string Empresanombre { get; set; }
        public string Equiponombre { get; set; }
        public DateTime Rerfpmdesde { get; set; }
        public DateTime Rerfpmhasta { get; set; }

    }
}