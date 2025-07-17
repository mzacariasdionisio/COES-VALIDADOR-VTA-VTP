using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_PERIODO
    /// </summary>
    public class StPeriodoDTO : EntityBase
    {
        public int Stpercodi { get; set; } 
        public int Stperanio { get; set; } 
        public int Stpermes { get; set; } 
        public int Stperaniomes { get; set; } 
        public string Stpernombre { get; set; } 
        public string Stperusucreacion { get; set; } 
        public DateTime Stperfeccreacion { get; set; } 
        public string Stperusumodificacion { get; set; } 
        public DateTime Stperfecmodificacion { get; set; } 
        //variable de consulta
        public string NombreMes { get; set; }
    }
}
