using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_GENFORZADA
    /// </summary>
    public class PrGenforzadaDTO : EntityBase
    {
        public int? Relacioncodi { get; set; }
        public int Genforcodi { get; set; }
        public DateTime? Genforinicio { get; set; }
        public DateTime? Genforfin { get; set; }
        public string Genforsimbolo { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Lastuser { get; set; }
        public string Equinomb { get; set; }
        public string Nombarra { get; set; }
        public string Idgener { get; set; }
        public int? Subcausacodi { get; set; }
        public string Subcausadesc { get; set; }
        public int Grupocodi { get; set; }
        public int Equicodi { get; set; }
        public string Subcausacmg { get; set; }
        public string Codbarra { get; set; }
        public string Tension { get; set; }
        public decimal? Valor { get; set; }
        public decimal Suma { get; set; }
        public int Cantidad { get; set; }
        public string Nombretna { get; set; }
        private List<PrgenforzadaitemDTO> listaItems = new List<PrgenforzadaitemDTO>();
        public List<PrgenforzadaitemDTO> ListaItems
        {
            get { return listaItems; }
            set { listaItems = value; }
        }
    }


    public class PrgenforzadaitemDTO
    {
        public string Codbarra { get; set; }
        public string Idgenerador { get; set; }
        public string Nombarra { get; set; }
        public string Tension { get; set; }
        public string Subcausacmg { get; set; }
        public string Nombretna { get; set; }
    }
}
