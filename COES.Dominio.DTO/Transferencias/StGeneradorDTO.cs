using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_GENERADOR
    /// </summary>
    public class StGeneradorDTO : EntityBase
    {
        public int Stgenrcodi { get; set; }
        public int Strecacodi { get; set; }
        public int Emprcodi { get; set; }
        public string Stgenrusucreacion { get; set; }
        public DateTime Stgenrfeccreacion { get; set; }
        //variables de consulta
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Barrnombre { get; set; }
    }
}
