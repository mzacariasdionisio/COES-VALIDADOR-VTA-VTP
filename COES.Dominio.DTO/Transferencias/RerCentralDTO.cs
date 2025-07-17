using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_CENTRAL
    /// </summary>
    public class RerCentralDTO : EntityBase
    {
        public int Rercencodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public int? Famcodi { get; set; }
        public string Rercenestado { get; set; }
        public DateTime Rercenfechainicio { get; set; }
        public DateTime Rercenfechafin { get; set; }
        public decimal Rercenenergadj { get; set; }
        public decimal Rercenprecbase { get; set; }
        public decimal Rerceninflabase { get; set; }
        public string Rercendesccontrato { get; set; }
        public int Codentcodi { get; set; }
        public string Pingnombre { get; set; }
        public string Barrbarratransferencia { get; set; }
        public int? Ptomedicodi { get; set; }
        public string Rercenusucreacion { get; set; }
        public DateTime Rercenfeccreacion { get; set; }
        public string Rercenusumodificacion { get; set; }
        public DateTime Rercenfecmodificacion { get; set; }

        //Atributos de consulta
        public string Equinomb { get; set; }
        public string Emprnomb { get; set; }
        public string Ptomedidesc { get; set; }
        public string Codentcodigo { get; set; }
        public decimal TotalMes { get; set; }
    }
}