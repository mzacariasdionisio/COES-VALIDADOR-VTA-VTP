using System;
using System.Collections.Generic;
using COES.Base.Core;
namespace COES.Dominio.DTO.Sic
{
    public class DpoTransformadorDTO
    {
        public int Dpotnfcodi { get; set; }
        public string Dpotnfcodiexcel { get; set; }
        public string Dposubnombre { get; set; }
        public string Emprnomb { get; set; }
        public string Dpotnfusucreacion { get; set; }
        public DateTime Dpotnffeccreacion { get; set; }

        //Adicional
        public string Dposubcodi { get; set; }
        public string Dposubcodiexcel { get; set; }
    }
}
