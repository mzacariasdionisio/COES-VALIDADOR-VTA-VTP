using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_FACTOR_PERDIDA_MEDIA
    /// </summary>
    public class TrnFactorPerdidaMediaDTO : EntityBase
    {
        public int Trnfpmcodi { get; set; } 
        public int Pericodi { get; set; } 
        public int Barrcodi { get; set; } 
        public int Codentcodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int Equicodi { get; set; } 
        public int Trnfpmversion { get; set; } 
        public decimal Trnfpmvalor { get; set; } 
        public string Trnfpmobserv { get; set; } 
        public string Trnfpmusername { get; set; } 
        public DateTime Trnfpmfecins { get; set; }

        //Atributos adicionales para mostrar en consulta
        public string Codentcodigo { get; set; }
        public string Barrnombre { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        //ASSETEC 20190104
        public decimal EntregaMes { get; set; }
        public decimal MedidoresMes { get; set; }
    }
}
