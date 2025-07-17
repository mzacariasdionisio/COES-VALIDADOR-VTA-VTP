using System;
using System.Collections.Generic;
using COES.Base.Core;
using COES.Framework.Base.Tools;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_MEDICION1
    /// </summary>
    [Serializable]
    public class MeMedicion1DTO : EntityBase, IMeMedicion
    {
        public int Lectcodi { get; set; }
        public DateTime Medifecha { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Ptomedicodi { get; set; }
        public decimal? H1 { get; set; }
        public string Nota { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Emprnomb { get; set; }
        public string Gruponomb { get; set; }
        public string Tipoinfodesc { get; set; }
        public string IndInformo { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public int Emprcodi { get; set; }
        public string Tipoinfoabrev { get; set; }
        public int Tipoptomedicodi { get; set; }
        public string Tipoptomedinomb { get; set; }
        public string Ptomedibarranomb { get; set; }
        public int Famcodi { get; set; }
        public string Ptomedinomb { get; set; }
        public string Famabrev { get; set; }
        public int IdCuenca { get; set; }
        public string Cuenca { get; set; }
        public string Emprcoes { get; set; }
        public string Ptomedielenomb { get; set; }
        public decimal? PorcentajeEjec { get; set; }

        //- Campos adicionales para reporte hidrologia
        public int Reporcodi { get; set; }
        public int TipoRelacioncodi { get; set; }
        public decimal CalculadoFactor { get; set; }
        public int CalculadoPtomedicodi { get; set; }
        public string CalculadoPtomedidesc { get; set; }
        public int CalculadoOrden { get; set; }
        public int Ubicacioncodi { get; set; }
        public string Ubicaciondesc { get; set; }
        public int OrigenPtomedicodi { get; set; }
        public string OrigenPtomedidesc { get; set; }

        #region PR5
        public int Semana { get; set; }
        public int Anio { get; set; }
        #endregion

        #region SIOSEIN

        public int Tipoptomedicodi2 { get; set; }
        public DateTime Medifecha2 { get; set; }
        public int Grupocodi { get; set; }
        public int Fenergcodi { get; set; }
        public string Fenergnomb { get; set; }
        public int Grupopadre { get; set; }
        public string Concepabrev { get; set; }
        public string Formuladat { get; set; }
        public decimal? Tptomedicodi { get; set; }
        public string Modopercodi { get; set; }
        public decimal? RendimientoTerm { get; set; }
        public string Tipcombustible { get; set; }
        public decimal? Cc { get; set; }
        public decimal? Cvc { get; set; }
        public decimal? Cvnc { get; set; }
        public decimal? Cv { get; set; }
        public int Barrcodi { get; set; }
        public string Barrnombre { get; set; }
        public DateTime Barrfecins { get; set; }
        public int Periodo { get; set; }
        public string Codigoempresa { get; set; }
        public string Codigobarra { get; set; }
        public string Codentregaret { get; set; }
        public decimal? Energactentr { get; set; }
        public decimal? Energactret { get; set; }
        public decimal? EnergAct { get; set; }
        public decimal? EnergAnt { get; set; }
        public string Codiemprentr { get; set; }
        public string Codiemprrecb { get; set; }
        public int CodEmpr1 { get; set; }
        public int CodEmpr2 { get; set; }
        public string Nombemprentr { get; set; }
        public string Nombemprrecb { get; set; }
        public string Codclasiftrans { get; set; }
        public decimal? Recatransmisi { get; set; }
        public decimal? Recageneradic { get; set; }
        public decimal? Recasegsumrf { get; set; }
        public decimal? Recasegsumres { get; set; }
        public decimal? Recaprimarer { get; set; }
        public decimal? Recaprimafise { get; set; }
        public decimal? Recaprimacase { get; set; }
        public decimal? Recaconfiasum { get; set; }
        public decimal? Recaotroscarg { get; set; }
        public string Osicodi { get; set; }
        public string Osinergcodi { get; set; }
        public int Tgenercodi { get; set; }
        public string Tgenernomb { get; set; }
        #endregion

        #region SIOSEIN - CAMBIOS
        public int Codref { get; set; }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string Equiabrev { get; set; }
        public decimal? ValorNcp { get; set; }
        #endregion

        #region SIOSEIN2
        public decimal? H1Costo { get; set; }
        //Campos adicionales para reporte costos marginales- Boletin informativo
        //Costo marinal
        public List<decimal?> ListaH1 { get; set; }
        //T.Barras
        public List<decimal?> ListaH1Costo { get; set; }

        #endregion

        #region Numerales Datos Base
        public decimal? Valor { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Dia { get; set; }
        #endregion

        public string MensajeValidacion { get; set; }
        public string MedifechaPto { get; set; }
        public string LastdateDesc { get; set; }
    }

}
