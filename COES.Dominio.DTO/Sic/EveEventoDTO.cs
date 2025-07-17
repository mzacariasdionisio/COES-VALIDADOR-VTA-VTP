using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_EVENTO
    /// </summary>    
    public class EveEventoDTO : EntityBase
    {
        public string Evencomentarios { get; set; } 
        public string Evenperturbacion { get; set; } 
        public string Twitterenviado { get; set; } 
        public int Evencodi { get; set; } 
        public int? Emprcodirespon { get; set; } 
        public int? Equicodi { get; set; } 
        public int? Evenclasecodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public int? Tipoevencodi { get; set; } 
        public DateTime? Evenini { get; set; } 
        public decimal? Evenmwindisp { get; set; } 
        public DateTime? Evenfin { get; set; } 
        public int? Subcausacodi { get; set; } 
        public string Evenasunto { get; set; } 
        public int? Evenpadre { get; set; } 
        public string Eveninterrup { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public DateTime? Evenpreini { get; set; } 
        public DateTime? Evenpostfin { get; set; } 
        public string Evendesc { get; set; } 
        public decimal? Eventension { get; set; } 
        public string Evenaopera { get; set; } 
        public string Evenpreliminar { get; set; } 
        public int? Evenrelevante { get; set; } 
        public string Evenctaf { get; set; } 
        public string Eveninffalla { get; set; } 
        public string Eveninffallan2 { get; set; } 
        public string Deleted { get; set; } 
        public string Eventipofalla { get; set; } 
        public string Eventipofallafase { get; set; } 
        public string Smsenviado { get; set; } 
        public string Smsenviar { get; set; } 
        public string Evenactuacion { get; set; }
        public string Equiabrev { get; set; }
        public string Tipoevenabrev { get; set; }
        public string Emprnomb { get; set; }
        public string Tareaabrev { get; set; }
        public string Areanomb { get; set; }
        public string Indinforme { get; set; }
        public string Tiporegistro { get; set; }
        public string Valtiporegistro { get; set; }
        public string CodEve { get; set; }
        public int? Subcausacodiop { get; set; }
        public string Equinomb { get; set; }
        public string Famnomb { get; set; }
        public string Evengendescon { get; set; }
        public decimal? Evenmwgendescon { get; set; }
        public string EveAdjunto { get; set; }

        #region CTAF

        public int? Eveninfplazodiasipi { get; set; }
        public int? Eveninfplazodiasif { get; set; }
        public int? Eveninfplazohoraipi { get; set; }
        public int? Eveninfplazohoraif { get; set; }
        public string PlazoEnvioIPI { get; set; }
        public string PlazoEnvioIF { get; set; }
        public string ColorPlazoIPI { get; set; }
        public string ColorTextoPlazoIPI { get; set; }
        public string ColorPlazoIF { get; set; }
        public string ColorTextoPlazoIF { get; set; }
        public bool EnPlazoIPI { get; set; }
        public bool EnPlazoIF { get; set; }
        public int? Eveninfplazominipi { get; set; }
        public int? Eveninfplazominif { get; set; }
        public int? Eveninfplazodiasipi_N2 { get; set; }
        public int? Eveninfplazodiasif_N2 { get; set; }
        public int? Eveninfplazohoraipi_N2 { get; set; }
        public int? Eveninfplazohoraif_N2 { get; set; }
        public int? Eveninfplazominipi_N2 { get; set; }
        public int? Eveninfplazominif_N2 { get; set; }

        public int Eveninfcodi { get; set; }
        public int Eveninfn2codi { get; set; }

        #endregion
        #region PR5        
        public int Famcodi { get; set; }
        public int Causaevencodi { get; set; }
        public string Causaevendesc { get; set; }
        public decimal Evenenergia { get; set; }
        public decimal Eveninterrupmw { get; set; }
        public string Causaevenabrev { get; set; }
        #endregion

        #region SIOSEIN

        public string TipoEquipo { get; set; }
        public decimal? Generacion { get; set; }
        public decimal? Transmision { get; set; }
        public decimal? Distribucion { get; set; }
        public decimal? UsuarioLibre { get; set; }
        public decimal Interrupcion { get; set; }
        public DateTime EvenIni { get; set; }
        public int CountFenAmbien { get; set; }
        public int CountFallEquip { get; set; }
        public int CountFallExter { get; set; }
        public int CountOtros { get; set; }
        public int CountNoIdentif { get; set; }
        public int CountFallSisPro { get; set; }
        public int CountFallHumana { get; set; }
        public int CountFallTotal { get; set; }
        public decimal EnergiaInterrum { get; set; }
        public int CountLineaTrans { get; set; }
        public int CountTransform { get; set; }
        public int CountBarras { get; set; }
        public int CountUnidGener { get; set; }
        public decimal? IntUnidadesGeneracion { get; set; }

        public decimal? Total
        {
            get { return Generacion + Transmision + Distribucion + UsuarioLibre; }
        }

        public decimal? Interrminu { get; set; }
        public decimal? Interrmw { get; set; }
        public int? Tipoemprcodi { get; set; }
        public string Tipoemprdesc { get; set; }
        public decimal? EnergiaNoSuministrada { get; set; }
        public decimal? Horas { get; set; }
        public string Osinergcodi { get; set; }
        public string Subcausadesc { get; set; }

        #endregion

        #region MigracionSGOCOES-GrupoB

        public string Emprabrev { get; set; }
        public decimal Evenbajomw { get; set; }

        #endregion

        #region AseguramientoOperacionSev

        public string Evenasegoperacion { get; set; }

        #endregion

        #region SIOSEIN-PRIE-2021
        public string Emprsein { get; set; }
        #endregion

        public string Evenrcmctaf { get; set; }

    }
}
