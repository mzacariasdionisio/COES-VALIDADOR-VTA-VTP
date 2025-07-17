using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_HORAOPERACION
    /// </summary>
    [Serializable]
    public partial class EveHoraoperacionDTO : EntityBase, ICloneable
    {
        public int Hopcodi { get; set; }
        public DateTime? Hophorini { get; set; }
        public int? Subcausacodi { get; set; }
        public DateTime? Hophorfin { get; set; }
        public int? Equicodi { get; set; }
        public string Hopdesc { get; set; }
        public DateTime? Hophorordarranq { get; set; }
        public DateTime? Hophorparada { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public int? Grupocodi { get; set; }
        public int? Hopsaislado { get; set; }
        public string Hoplimtrans { get; set; }
        public string Hopfalla { get; set; }
        public string Hopcompordarrq { get; set; }
        public string Hopcompordpard { get; set; }
        public int? Hopcausacodi { get; set; }
        public int? Hopcodipadre { get; set; }
        public string Hopestado { get; set; }
        public int Hopnotifuniesp { get; set; }
        public int? Evencodi { get; set; }
        public string Hopobs { get; set; }
        public int Emprcodi { get; set; }
        public string Hoparrqblackstart { get; set; }
        public string Hopensayope { get; set; }
        public string Hopensayopmin { get; set; }
        public int? HopPruebaExitosa { get; set; }
    }

    public partial class EveHoraoperacionDTO
    {
        public int Hopunicodi { get; set; }

        public string Tipogrupomodo { get; set; }
        public string Gruponomb { get; set; }
        public int Grupopadre { get; set; }
        public int Unidad { get; set; }
        public string Emprnomb { get; set; }
        public string Subcausadesc { get; set; }
        public string Subcausacolor { get; set; }
        public string EquipoNombre { get; set; }
        public string PadreNombre { get; set; }

        public int OpcionCrud { get; set; }
        public int CodiPadre { get; set; }
        public List<UnidadesExtra> UnidadesExtra { get; set; }

        public int Grupourspadre { get; set; }
        public int Fenergcodi { get; set; }
        public string Fenergnomb { get; set; }
        public string Fenercolor { get; set; }
        public string Fenergabrev { get; set; }

        public int HIni48 { get; set; }
        public int HFin48 { get; set; }
        public int HIni96 { get; set; }
        public int HFin96 { get; set; }
        public int TotalMinuto { get; set; }
        public double CIncremental { get; set; }

        #region PR5        
        public string Grupoabrev { get; set; }
        public int Famcodi { get; set; }
        public string Famnomb { get; set; }
        public DateTime? HoraIni48 { get; set; }
        public DateTime? HoraFin48 { get; set; }
        public DateTime? HoraIni96 { get; set; }
        public DateTime? HoraFin96 { get; set; }
        public DateTime? FechaProceso { get; set; }
        public bool Es15MinTieneHo { get; set; }
        public bool Es30MinTieneHo { get; set; }
        public int FlagCalificado { get; set; }
        #endregion

        #region SIOSEIN
        public string Grupotipo { get; set; }
        public string Grupotipomodo { get; set; }
        public string Osinergcodi { get; set; }
        public decimal? Duracion { get; set; }
        public decimal DuracionEntero { get; set; }
        public string HoplimtransTemporal { get; set; }
        public string SubcausaOsi { get; set; }
        #endregion

        #region Horas Operacion EMS
        public string HophoriniDesc { get; set; }
        public string HophorfinDesc { get; set; }
        public string HophorordarranqDesc { get; set; }
        public string HophorparadaDesc { get; set; }
        public string HopcompordarrqDesc { get; set; }
        public string HopcompordpardDesc { get; set; }

        public int Equipadre { get; set; }
        public string Central { get; set; }
        public int FlagTipoTemporal { get; set; }
        public int TieneAlertaEms { get; set; }
        public int TieneAlertaScada { get; set; }
        public int TieneAlertaIntervencion { get; set; }
        public int TieneAlertaCostoIncremental { get; set; }

        public string HopensayopeDesc { get; set; }
        public string HopensayopminDesc { get; set; }
        public string HopsaisladoDesc { get; set; }
        public string HoplimtransDesc { get; set; }

        public string LastdateDesc { get; set; }
        public string HopcausacodiDesc { get; set; }

        public DateTime? BitacoraHophorini { get; set; }
        public DateTime? BitacoraHophorfin { get; set; }
        public string BitacoraHophoriniFecha { get; set; }
        public string BitacoraHophoriniHora { get; set; }
        public string BitacoraHophorfinFecha { get; set; }
        public string BitacoraHophorfinHora { get; set; }
        public string BitacoraDescripcion { get; set; }
        public string BitacoraComentario { get; set; }
        public string BitacoraDetalle { get; set; }
        public int? BitacoraIdSubCausaEvento { get; set; }
        public int? BitacoraIdEvento { get; set; }
        public int? BitacoraIdTipoEvento { get; set; }
        public int? BitacoraIdEquipo { get; set; }
        public int? BitacoraIdEmpresa { get; set; }
        public int? BitacoraIdTipoOperacion { get; set; }

        public int FlagTipoHo { get; set; }

        public int EventoGenerado { get; set; }

        #endregion

        #region MigracionSGOCOES-GrupoB

        public int Areacodi { get; set; }
        public string Areanomb { get; set; }
        public string Equiabrev { get; set; }
        public int Ptomedicodi { get; set; }
        public int Ptomedicodicalculado { get; set; }
        public string Grupocomb { get; set; }
        public string Emprabrev { get; set; }
        public decimal? PotenciaPromedio { get; set; }
        #endregion

        public string FlagModoEspecial { get; set; }
        public decimal? Valor { get; set; }
        public List<EveHoEquiporelDTO> ListaDesglose { get; set; }

        #region Numerales Datos Base
        public string Equinomb { get; set; }
        public string Osicodi { get; set; }
        public string Dia { get; set; }
        public string Osigrupocodi { get; set; }

        public string Motivo { get; set; } /**/
        public int Area { get; set; } /**/
        #endregion

        public string PMin { get; set; }
        public string PEfe { get; set; }
        public string TMinO { get; set; }
        public string TMinA { get; set; }
        public string TParada { get; set; }
        public string TArranque { get; set; }
        public bool FlagCentralEspecial { get; set; }

        public string HorasProgIni { get; set; }
        public string HorasProgFin { get; set; }

        public DateTime? FechaProgIni { get; set; }
        public DateTime? FechaProgFin { get; set; }

        #region Mejoras Hop

        public List<EveHoUnidadDTO> ListaHoUnidad { get; set; }

        #endregion

        #region color termica
        public string ColorTermica { get; set; }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Clase que almacena las unidades especiales seleccionadas de una hora de operación
    /// </summary>
    public class UnidadesExtra
    {
        public int Equicodi { get; set; }
        public int Subcausacodi { get; set; }
    }

}
