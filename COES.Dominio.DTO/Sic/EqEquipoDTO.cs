using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_EQUIPO
    /// </summary>
    [DataContract]
    [Serializable]
    public class EqEquipoDTO : EntityBase
    {
        [DataMember]
        public int Equicodi { get; set; }
        [DataMember]
        public int? Emprcodi { get; set; }
        [DataMember]
        public int? Grupocodi { get; set; }
        [DataMember]
        public int? Elecodi { get; set; }
        [DataMember]
        public int? Areacodi { get; set; }
        [DataMember]
        public int? Famcodi { get; set; }
        [DataMember]
        public string Equiabrev { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev2 { get; set; }
        public decimal? Equitension { get; set; }
        public int? Equipadre { get; set; }
        public decimal? Equipot { get; set; } //DEPRECATED
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Ecodigo { get; set; }
        public string Equiestado { get; set; }
        public string Osigrupocodi { get; set; }
        public int? Lastcodi { get; set; }
        public DateTime? Equifechiniopcom { get; set; }
        public DateTime? Equifechfinopcom { get; set; }
        public string UsuarioUpdate { get; set; }
        public DateTime? FechaUpdate { get; set; }
        public string EquiManiobra { get; set; }
        //DATOS EMPRESA
        public string EMPRNOMB { get; set; }
        public string DESCENTRAL { get; set; }
        public string TipoEmpresa { get; set; }
        //DATOS AREA
        public string AREANOMB { get; set; }
        public string TAREAABREV { get; set; }
        //DATOS FAMILIA
        public string Famnomb { get; set; }
        public string FAMABREV { get; set; }
        public string Urlmaniobra { get; set; }
        public string Emprnomb { get; set; }
        public string Emprabrev { get; set; }
        public string Areanomb { get; set; }
        public string Tareaabrev { get; set; }
        public string Famabrev { get; set; }
        public string Cuencanomb { get; set; }
        public string EstadoDesc { get; set; }
        public string Osinergcodi { get; set; }
        public string OsinergcodiGen { get; set; }              // ticket-6068
        public string OsinergcodiDespacho { get; set; }
        public string B2 { get; set; }
        public string B3 { get; set; }
        public int Propcodi { get; set; }
        public int Famcodipadre { get; set; }
        public string Nombrecentral { get; set; }
        public int Codipadre { get; set; }
        public int EquiCodiSelect { get; set; }
        public int TipoOpId { get; set; }
        public string EmpresaOrigen { get; set; }
        public string Emprcoes { get; set; }
        public string Formuladat { get; set; }
        public string Fecha { get; set; }
        public string Condicion { get; set; }
        public string Codigotipoempresa { get; set; }
        public string Codigogrupo { get; set; }
        public int Correlativo { get; set; }
        public string Capacidadanterior { get; set; }
        public string Capacidadnueva { get; set; }
        public string Observaciones { get; set; }

        public int Ctgcodi { get; set; }
        public int Ctgdetcodi { get; set; }

        public string Equinombpadre { get; set; }
        public int Operadoremprcodi { get; set; }
        public string Operadornomb { get; set; }

        public int Evencodi { get; set; }
        public DateTime? Equirelfecmodificacion { get; set; }
        public string Equirelusumodificacion { get; set; }
        public string EquirelfecmodificacionDesc { get; set; }

        public int Equicodiactual { get; set; }
        public DateTime Heqdatfecha { get; set; }
        public string Heqdatestado { get; set; }
        public string Gruponomb { get; set; }
        public string Grupointegrante { get; set; }
        public string Grupoabrev { get; set; }
        public string Areadesc { get; set; }

        public List<EqPropequiDTO> PropiedadesEquipo { get; set; }

        #region Indisponibilidades

        public string UnidadnombPR25 { get; set; }
        public string UnidadnombCCC { get; set; }
        public int Grupocodicentral { get; set; }
        public int Grupocodidespacho { get; set; }
        public int Grupocodimodo { get; set; }
        public int Grupopadre { get; set; }
        public string Grupocomb { get; set; }
        public List<EqEquipoDTO> ListaCentral = new List<EqEquipoDTO>();
        public int Indcuaadieje { get; set; }
        public string Gaseoducto { get; set; }

        public decimal? Potenciagarantizada { get; set; }
        public DateTime? FechaVigenciaGarantizada { get; set; }
        public string ComentarioGarantizada { get; set; }
        public bool TieneModificacionPropiedadGarantizada { get; set; }

        public DateTime? FechaVigenciaPmin { get; set; }

        public decimal? Rendimiento { get; set; }
        public DateTime? FechaVigenciaRendimiento { get; set; }
        public string ComentarioRendimiento { get; set; }
        public bool TieneModificacionPropiedadRendimiento { get; set; }

        public decimal? Potenciafirme { get; set; }
        public decimal? Factorindisponibilidad { get; set; }
        public decimal? Factorpresencia { get; set; }

        public decimal? Potenciaefectiva { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public string Comentario { get; set; }
        public bool TieneNuevoIngresoOpComercial { get; set; }
        public bool TieneNuevoRetiroOpComercial { get; set; }
        public bool TieneModificacionPropiedad { get; set; }

        public bool EsUnaUnidadXCentral { get; set; }
        public int NumeroGen { get; set; }
        public decimal? ConsumoCombAlt { get; set; }
        public DateTime? FechaVigenciaCombAlt { get; set; }
        public int FenergcodiCombAlt { get; set; }
        public bool TieneCalculoCS { get; set; }
        public int EquicodiTVCicloComb { get; set; }
        public string GruponombCS { get; set; }

        public int? Estcomcodi { get; set; }
        public decimal? Fcc { get; set; }
        public decimal? Fce { get; set; }
        public decimal? PotenciaAsegurada { get; set; }
        public List<int> ListaEquicodi { get; set; }

        public string TipogenerrerDesc { get; set; }
        public string GrupotipocogenDesc { get; set; }

        #endregion

        #region CCC

        public decimal? RendBagazo { get; set; }

        #endregion

        #region SIOSEIN
        public List<MeHojaptomedDTO> ListaCaudales { get; set; }
        public string ColorEstado { get; set; }
        public int TipoOperacion { get; set; }

        public EqEquipoDTO()
        {
            this.Equitension = 0.000m;
            this.Equipot = 0.000m;
            this.ValorEolico = 0.000m;
            this.ValorHidroelectrico = 0.000m;
            this.ValorSolar = 0.000m;
            this.ValorTermoelectrico = 0.000m;
            this.ValorRenovable = 0.000m;
            this.Promedio = ValorEolico + ValorHidroelectrico + ValorSolar + ValorTermoelectrico;
            ListaPrgrupo = new List<PrGrupoDTO>();
            ListaFuenteEnergia = new List<SiFuenteenergiaDTO>();
            ListaEquicodi = new List<int>();
        }

        public int Tgenercodi { get; set; }
        public int Mes { get; set; }
        public decimal? Promedio { get; set; }
        public decimal? ValorHidroelectrico { get; set; }
        public decimal? ValorTermoelectrico { get; set; }
        public decimal? ValorRenovable { get; set; }
        public decimal? ValorSolar { get; set; }
        public decimal? ValorEolico { get; set; }
        public int Fenergcodi { get; set; }
        public string NombreTipoGeneracion { get; set; }
        public string NombreFuenteEnergia { get; set; }
        public decimal? Agua { get; set; }
        public decimal? Bagazo { get; set; }
        public decimal? Biogas { get; set; }
        public decimal? Carbon { get; set; }
        public decimal? DieselB5 { get; set; }
        public decimal? Eolica { get; set; }
        public decimal? Gas { get; set; }
        public decimal? Residual { get; set; }
        public decimal? ResidualR500 { get; set; }
        public decimal? ResidualR6 { get; set; }
        public decimal? Solar { get; set; }
        public decimal? Total { get; set; }
        public int CodCentral { get; set; }
        public decimal? Participacion { get; set; }
        public string Tgenernomb { get; set; }
        public string Fenergnomb { get; set; }
        public string Central { get; set; }
        public string Ctgdetnomb { get; set; }
        public string Grupotipocogen { get; set; }
        public int Gruporeservafria { get; set; }
        public int Grupoemergencia { get; set; }
        public int Gruponodoenergetico { get; set; }
        public int Grupoincremental { get; set; }
        public decimal? PotenciaEfectiva { get; set; }
        public decimal? PotenciaInstalada { get; set; }
        public decimal? PotenciaNominal { get; set; }
        public string Tipogenerrer { get; set; }
        #endregion

        #region MigracionSGOCOES-GrupoB

        public string Valor { get; set; }
        public List<PrGrupoDTO> ListaPrgrupo { get; set; }
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia { get; set; }
        public decimal? Pmin { get; set; }
        public decimal? Pmax { get; set; }
        public decimal? Pe { get; set; }

        public decimal? Pf { get; set; }
        public DateTime? FechaVigenciaFirme { get; set; }
        public bool TieneModificacionPropiedadPfirme { get; set; }

        public decimal? Rsf { get; set; }
        public DateTime? FechaVigenciaRsf { get; set; }

        public int Tminoperacion { get; set; }
        public int Tminarranque { get; set; }
        public string Tminarranquedesc { get; set; }
        public string Tminparadadesc { get; set; }
        public string Tminoperadesc { get; set; }
        public bool EsUnidadModoEspecial { get; set; }
        public bool EsUnidadRepartible { get; set; }
        public bool TieneModoOpDefecto { get; set; }
        public decimal? PotenciaMinUtilizado { get; set; }
        #endregion

        #region SIOSEIN2

        public string Tipoemprdesc { get; set; }
        public int Tipoemprcodi { get; set; }
        public bool TieneCicloComb { get; set; }
        public int Evenclasecodi { get; set; }

        #endregion

        #region Informe_Ejecutivo_Semanal
        public string Recurso { get; set; }
        public string Tecnologia { get; set; }
        public string Unidades { get; set; }
        #endregion

        #region Intervenciones
        public int Circodi { get; set; }
        public string Circnomb { get; set; }
        #endregion

        #region vtp Ingreso potencia
        public int? Pfrtotficticio { get; set; }
        public string Pfrtotunidadnomb { get; set; }
        #endregion

        #region Ficha Técnica
        public int NroItem { get; set; } // Indice para la importación de equipos
        public bool ExisteCambio { get; set; }

        //Ocultar equipo
        public string FtverocultoPortal { get; set; }
        public string FtverocultoExtranet { get; set; }
        public string FtverocultoIntranet { get; set; }
        #endregion

        #region Ficha tecnica 2023
        public int Idelemento { get; set; }
        public int Idempresaelemento { get; set; }
        public string Nombempresaelemento { get; set; }
        public int Idempresacopelemento { get; set; }
        public string Nombempresacopelemento { get; set; }
        public string Nombreelemento { get; set; }
        public string Tipoelemento { get; set; }
        public string Areaelemento { get; set; }
        public string Estadoelemento { get; set; }
        #endregion

        public class TipoSerie : EntityBase
        {
            public int Tiposeriecodi { get; set; }

            public string Tiposerienomb { get; set; }
        }

        public class TipoPuntoMedicion : EntityBase
        {
            public int TipoPtoMediCodi { get; set; }

            public string TipoPtoMediNomb { get; set; }

            public string TipoInfoDesc { get; set; }
        }

        #region CPPA.ASSETEC.2024
        public string EquinombConcatenado { get; set; }
        #endregion
        #region GESPROTEC-20241031
        public string Flaggenprotec { get; set; }
        public string Epequinombenprotec { get; set; }
        public int Epequicodi { get; set; }
        public string Subestacion { get; set; }
        public string Celda { get; set; }
        public string EquiestadoDesc { get; set; }

        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}