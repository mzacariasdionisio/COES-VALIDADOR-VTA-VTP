using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AF_ERACMF_EVENTO
    /// </summary>
    public class AfEracmfEventoDTO : EntityBase
    {
        public int Evencodi { get; set; }
        public string Eracmfusumodificacion { get; set; }
        public string Eracmfusucreacion { get; set; }
        public DateTime? Eracmffecmodificacion { get; set; }
        public DateTime? Eracmffeccreacion { get; set; }
        public string Eracmfcodrele { get; set; }
        public string Eracmftiporegistro { get; set; }
        public string Eracmffechretiro { get; set; }
        public string Eracmffechingreso { get; set; }
        public string Eracmffechimplementacion { get; set; }
        public string Eracmfobservaciones { get; set; }
        public string Eracmfsuministrador { get; set; }
        public decimal? Eracmfdreferencia { get; set; }
        public decimal? Eracmfmindregistrada { get; set; }
        public decimal? Eracmfmediadregistrada { get; set; }
        public decimal? Eracmfmaxdregistrada { get; set; }
        public decimal? Eracmftiemporderivada { get; set; }
        public decimal? Eracmfdfdtrderivada { get; set; }
        public decimal? Eracmfarranqrderivada { get; set; }
        public decimal? Eracmftiemporumbral { get; set; }
        public decimal? Eracmfarranqrumbral { get; set; }
        public string Eracmfnumetapa { get; set; }
        public string Eracmfcodinterruptor { get; set; }
        public string Eracmfciralimentador { get; set; }
        public string Eracmfnivtension { get; set; }
        public string Eracmfsubestacion { get; set; }
        public string Eracmfnroserie { get; set; }
        public string Eracmfmodelo { get; set; }
        public string Eracmfmarca { get; set; }
        public string Eracmfzona { get; set; }
        public string Eracmfemprnomb { get; set; }
        public int Eracmfcodi { get; set; }

        //Campos de validación
        public int NumeroFilaExcel { get; set; }
        public string CampoExcel { get; set; }
        public string ValorCeldaExcel { get; set; }
        public string MensajeValidacion { get; set; }
        public string EVENCODI { get; set; }
        public DateTime EVENINI { get; set; }
    }
}
