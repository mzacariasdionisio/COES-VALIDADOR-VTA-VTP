using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_GRUPO
    /// </summary>
    [Serializable]
    public partial class PrGrupoDTO : EntityBase, ICloneable
    {
        public int? Fenergcodi { get; set; }
        public int Barracodi { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public decimal? Grupovmax { get; set; }
        public decimal? Grupovmin { get; set; }
        public int? Grupoorden { get; set; }
        public int? Emprcodi { get; set; }
        public string Grupotipo { get; set; }
        public int Catecodi { get; set; }
        public int? Grupotipoc { get; set; }
        public int? Grupopadre { get; set; }
        public string Grupoactivo { get; set; }
        public string Grupocomb { get; set; }
        public string Osicodi { get; set; }
        public int? Grupocodi2 { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Gruposub { get; set; }
        public int Barracodi2 { get; set; }
        public decimal Barramw1 { get; set; }
        public decimal Barramw2 { get; set; }
        public string Gruponombncp { get; set; }
        public int Tipogrupocodi { get; set; }
        public int TipoGrupoCodi2 { get; set; }
        public string TipoGenerRer { get; set; }
        public string Osinergcodi { get; set; }
        public decimal? Grupotension { get; set; }
        public string GrupoEstado { get; set; }
        public int Gruponodoenergetico { get; set; }
        public int Gruporeservafria { get; set; }
        public int? Curvcodi { get; set; }
        public int Areacodi { get; set; }

        public string Grupotipomodo { get; set; }
        public string Grupousucreacion { get; set; }
        public DateTime? Grupofeccreacion { get; set; }
        public string Grupousumodificacion { get; set; }
        public DateTime? Grupofecmodificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class PrGrupoDTO
    {
        public string ColorTermica { get; set; }
        public string DesTipoGrupo { get; set; }
        public string EmprNomb { get; set; }
        public int PtoMediCodi { get; set; }
        public string Fenergnomb { get; set; }
        public int GrupoCentral { get; set; }
        public string GruponombPadre { get; set; }
        public bool ConfiguradoReportePe { get; set; }
        public string Fueosinergcodi { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public int Idtv { get; set; }
        public int? Fenergpadre { get; set; }
        public string FlagModoEspecial { get; set; }
        public string FlagDividirModoEnEquipos { get; set; }
        public int? CurvGrupocodiPrincipal { get; set; }
        public int? Grupocodimodo { get; set; }
        public int Tgenercodi { get; set; }
        public string Tgenernomb { get; set; }
        public int? Emprorden { get; set; }
        public string Emprabrev { get; set; }
        public string AreaOperativa { get; set; }
        public List<string> Data { get; set; }
        public List<MePerfilRuleDTO> DtFormulas { get; set; }

        #region INDISPONIBILIDADES
        public int Grupocodicc { get; set; }
        public int Grupocodidet { get; set; }
        public int Equipadre { get; set; }
        public int TipoCombustible { get; set; }
        public int NumeroUnidades { get; set; }
        public bool TieneModoCicloCombinado { get; set; }
        public bool TieneModoCicloSimple { get; set; }
        public bool TieneModoEspecial { get; set; }
        public List<int> ListaGrupocodiDespacho { get; set; }
        public List<string> ListaGruponombDespacho { get; set; }
        public List<int> ListaEquicodi { get; set; }
        public List<string> ListaEquiabrev { get; set; }

        public DateTime? FechaVigencia { get; set; }
        public bool TieneNuevoIngresoOpComercial { get; set; }
        public bool TieneNuevoRetiroOpComercial { get; set; }
        public bool TieneModificacionPropiedad { get; set; }

        public DateTime? Fechiniopcom { get; set; }
        public DateTime? Fechfinopcom { get; set; }

        public string TipogenerrerDesc { get; set; }
        public string GrupotipocogenDesc { get; set; }

        public bool EsEditableOsinergcodi { get; set; }

        #endregion

        #region Horas Operacion EMS
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string CentralOsi { get; set; }
        public int TipoModOp { get; set; }
        public PrGrupoDTO()
        {
            TipoModOp = 1;
            ListaGrupocodiDespacho = new List<int>();
            ListaEquicodi = new List<int>();
            ListaEquiabrev = new List<string>();
            ListaGruponombDespacho = new List<string>();
        }
        public string Tipo { get; set; }
        public int FlagEncendido { get; set; }
        public decimal? Rendimiento { get; set; }
        public DateTime? FechaVigenciaRendimiento { get; set; }
        public bool TieneModificacionPropiedadRendimiento { get; set; }

        public int TminArranque { get; set; }
        public bool EsUnModoXCentral { get; set; }

        public decimal? RsfDefecto { get; set; }
        public DateTime? FechaVigenciaRsfDefecto { get; set; }

        public int Tminoperacion { get; set; }
        public decimal? Potencia { get; set; }
        public decimal PeIni { get; set; }
        public decimal PeFin { get; set; }
        public double CIncremental { get; set; }
        public double CVariable { get; set; }
        public double CVariable1 { get; set; }
        public string CVariableFormateado { get; set; }
        public string CVariable1Formateado { get; set; }
        public string PotenciaFormateado { get; set; }
        public string CIncrementalFormateado { get; set; }

        public double CIncremental1 { get; set; }
        public string CIncremental1Formateado { get; set; }
        public double CIncremental2 { get; set; }
        public string CIncremental2Formateado { get; set; }
        public string Tramo1 { get; set; }
        //public string Tramo1Formateado { get; set; }
        public string Tramo2 { get; set; }
        //public string Tramo2Formateado { get; set; }

        public int NumTramo { get; set; }
        public string Tramo { get; set; }
        public string TramoFormateado { get; set; }
        public string Comentario { get; set; }
        public int Hopcodi { get; set; }
        public string HoraMin { get; set; }

        public decimal? PotenciaMinima { get; set; }
        public DateTime? FechaVigenciaPmin { get; set; }

        public decimal? PotenciaFirme { get; set; }
        public DateTime? FechaVigenciaFirme { get; set; }


        public string PMin { get; set; }
        public string PEfe { get; set; }
        public string TMinO { get; set; }
        public string TMinA { get; set; }
        public string TParada { get; set; }
        public string TArranque { get; set; }

        #endregion

        #region Pronostico de la Demanda - PRODEM

        public string Areanomb { get; set; }
        public string Areadesc { get; set; }
        public string Barrnombre { get; set; }

        //20200128
        public int Areapadre { get; set; }
        public string Areapadreabrev { get; set; }
        public string Areapadrenomb { get; set; }
        public string Areaabrev { get; set; }

        #endregion

        #region MigracionSGOCOES-GrupoB

        public bool EsGrupogen { get; set; }
        public int Ptomedicodi { get; set; }
        public int Famcodi { get; set; }
        public string Equiabrev { get; set; }
        public string Digsilent { get; set; }
        public string Cateabrev { get; set; }
        public string Catenomb { get; set; }
        public string GrupoactivoDesc { get; set; }
        public string GrupoEstadoDesc { get; set; }
        public string GruponombWeb { get; set; }
        public string GruponombWebCompleto { get; set; }
        public int Tipoinfocodi { get; set; }
        public string Formuladat { get; set; }
        public string Grupotipocogen { get; set; }
        public decimal Factor { get; set; }
        public string ModoOperacion { get; set; }
        public string Concepabrev { get; set; }
        public double Cvnc_US { get; set; }
        public double Cmarrpar { get; set; }

        public string EstiloEstado { get; set; }

        public EqEquipoDTO ObjEqequipo = null;
        public SiEmpresaDTO ObjSiempresa = null;
        public List<PrGrupoDTO> ListaPrgrupoFunc = new List<PrGrupoDTO>();
        public List<PrGrupoDTO> ListaPrgrupoCC = new List<PrGrupoDTO>();
        public PrGrupoDTO ObjPrGrupodat = null;
        public int? ObjPrgrupopadre { get; set; }
        public List<MeMedicion48DTO> ListaReservaFria { get; set; }
        public List<PrGrupoDTO> ListaPrgrupoDespacho = new List<PrGrupoDTO>();
        public List<PrGrupodatDTO> ListaParametros = new List<PrGrupodatDTO>();

        #endregion
        ///Compensacion
        public decimal HorasOperacion { get; set; }
        public string Calificacion { get; set; }
        public string AccionCalculo { get; set; }
        public string Grupointegrante { get; set; }

        #region Titularidad-Instalaciones-Empresas

        public string EmprnombOrigen { get; set; }
        public string ColorEstado { get; set; }

        #endregion

        #region Numerales Datos Base
        public string Comb { get; set; }
        #endregion

        public int Sddpcodi { get; set; }
        public string CodAgc { get; set; }

        public string GrupofeccreacionDesc { get; set; }
        public string GrupofecmodificacionDesc { get; set; }
        public string FtverocultoPortal { get; set; } //Ocultar grupo Ficha técnica (Portal)
        public string FtverocultoExtranet { get; set; } //Ocultar grupo Ficha técnica (Extranet)
        public string FtverocultoIntranet { get; set; } //Ocultar grupo Ficha técnica (Intranet)

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

        public bool FlagActivo { get; set; }
    }
}
