using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla vtdValorizacion y vtdValorizaciondetalle
    /// </summary>
    public class VtdMontoSCeIODTO
    {      
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public decimal Valdfpgm { get; set; }
        public decimal Valoporcentajeperdida { get; set; }
        public decimal Valdfactorp { get; set; }
        public decimal Valdmcio { get; set; }
        public decimal Valdpdsc { get; set; }
        public decimal Valoco { get; set; }
        public decimal Valora { get; set; }
        public decimal ValoraSub { get; set; }
        public decimal ValoraBaj { get; set; }
        public decimal Valodemandacoes { get; set; }
        public decimal Valofactorreparto { get; set; }
        public decimal Valocompcostosoper { get; set; }
        public decimal Valoofmax { get; set; }
        public decimal ValoofmaxBaj { get; set; }
        public decimal Valdpagoio { get; set; }
        public decimal Valdpagosc { get; set; }
        public DateTime Valofecha { get; set; }
    }
}
