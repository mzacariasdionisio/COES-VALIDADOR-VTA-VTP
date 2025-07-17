using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    //[Serializable]
    public class EventoDTO
    {
        public string EQUIABREV { get; set; }
        public int? EVENCODI { get; set; }
        public int? EMPRCODI { get; set; }
        public int? EQUICODI { get; set; }
        public int? TIPOEVENCODI { get; set; }
        public DateTime? EVENINI { get; set; }
        public DateTime? EVENFIN { get; set; }
        public int? EVENPADRE { get; set; }
        public int? FAMCODI { get; set; }
        public int? AREACODI { get; set; }
        public string SUBCAUSAABREV { get; set; }
        public int? SUBCAUSACODI { get; set; }
        public string TIPOEVENABREV { get; set; }
        public string AREANOMB { get; set; }
        public string AREADESC { get; set; }
        public string EVENINTERRUP { get; set; }
        public string FAMABREV { get; set; }
        public string FAMNOMB { get; set; }
        public string TAREAABREV { get; set; }
        public string EMPRNOMB { get; set; }
        public string RAZSOCIAL { get; set; }
        public string EMPRABREV { get; set; }
        public int? TAREACODI { get; set; }
        public string LASTUSER { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string EVENASUNTO { get; set; }
        public string EVENPRELIMINAR { get; set; }
        public string CAUSAEVENABREV { get; set; }
        public int? EVENRELEVANTE { get; set; }
        public Nullable<short> EMPRCODIRESPON { get; set; }
        public Nullable<short> EVENCLASECODI { get; set; }
        public Nullable<decimal> EVENMWINDISP { get; set; }
        public Nullable<System.DateTime> EVENPREINI { get; set; }
        public Nullable<System.DateTime> EVENPOSTFIN { get; set; }
        public string EVENDESC { get; set; }
        public Nullable<decimal> EVENTENSION { get; set; }
        public string EVENAOPERA { get; set; }
        public string EVENCTAF { get; set; }
        public string EVENINFFALLA { get; set; }
        public string EVENINFFALLAN2 { get; set; }
        public string DELETED { get; set; }
        public string EVENTIPOFALLA { get; set; }
        public string EVENTIPOFALLAFASE { get; set; }
        public string SMSENVIADO { get; set; }
        public string SMSENVIAR { get; set; }
        public string EVENACTUACION { get; set; }
        public string EVENCOMENTARIOS { get; set; }
        public string EVENPERTURBACION { get; set; }
        public string TIPOEMPRDESC { get; set; }
        public Nullable<decimal> EQUITENSION { get; set; }
        public Nullable<decimal> ENERGIAINTERRUMPIDA { get; set; }
        public Nullable<decimal> INTERRUPCIONMW { get; set; }
        public Nullable<decimal> DISMINUCIONMW { get; set; }
        public double DURACION { get; set; }
        public string TIPOREGISTRO { get; set; }
        public string VALTIPOREGISTRO { get; set; }
        public string TIPOEMPRNOMB { get; set; }
        public Nullable<decimal> EVENMWGENDESCON { get; set; }
        public string EVENGENDESCON { get; set; }
        public decimal MWINTERRUMPIDOS { get; set; }
        //-  Agregados para el reporte extenido de eventos
        public string Eventipofalla { get; set; }
        public string Eventipofallafase { get; set; }
        public decimal Interrmwde { get; set; }
        public decimal Interrmwa { get; set; }
        public decimal Interrminu { get; set; }
        public decimal Interrmw { get; set; }
        public string Interrdesc { get; set; }
        public string Interrnivel { get; set; }
        public string Interrracmf { get; set; }
        public string Interrmfetapadesc { get; set; }
        public decimal Interrmfetapa { get; set; }
        public string Interrmanualr { get; set; }
        public string Ptointerrupnomb { get; set; }
        public string Ptoentrenomb { get; set; }
        public string Clientenomb { get; set; }

        #region "SGOCOES func A"
        public string EVENINIstr
        {
            get
            {
                if (EVENINI.HasValue)
                {
                    return EVENINI.Value.ToString("dd/MM/yyyy HH:mm");
                }
                return "";
            }
        }
        public string EVENFINstr
        {
            get
            {
                if (EVENFIN.HasValue)
                {
                    return EVENFIN.Value.ToString("dd/MM/yyyy HH:mm");
                }
                return "";
            }
        }
        public string LASTDATEstr
        {
            get
            {
                if (LASTDATE.HasValue)
                {
                    return LASTDATE.Value.ToString("dd/MM/yyyy HH:mm");
                }
                return "";
            }
        }


        // Filtros de consulta
        public string EmpresaPropietaria { get; set; }
        public string EmpresaInvolucrada { get; set; }
        public string TipoEquipo { get; set; }
        public string Estado { get; set; }
        public string Impugnacion { get; set; }
        public string TipoReunion { get; set; }
        public string RNC { get; set; }
        public string ERACMF { get; set; }
        public string ERACMT { get; set; }
        public string EDAGSF { get; set; }
        public string DI { get; set; }
        public string DF { get; set; }
        public string FuerzaMayor { get; set; }
        public string Anulado { get; set; }

        // Resultado de Consulta 

        public int AFECODI { get; set; }
        public string CODIGO { get; set; }
        public string NOMBRE_EVENTO { get; set; }
        public string INTERRUMPIDO { get; set; }
        public string FECHA_EVENTO { get; set; }
        public string FECHA_REUNION { get; set; }
        public string FECHA_INFORME { get; set; }
        public string REVISADO_DJR { get; set; }
        public string REVISADO_DO { get; set; }
        public string PUBLICADO { get; set; }
        public string ESTADO { get; set; }
        public string IMPUG { get; set; }
        public string RESPONSABLE { get; set; }
        public string INF_TECNICO { get; set; }
        public string AFEFZAMAYOR { get; set; }
        #endregion

        #region SIOSEIN2
        public int Causaevencodi { get; set; }
        public string Causaevendesc { get; set; }
        public decimal? SumEnerAreaFami { get; set; }
        #endregion

        public int EquicodiInvolucrado { get; set; }
        public string Ieventtipoindisp { get; set; }
        public decimal? Ieventpr { get; set; }

        #region Aplicativo Extranet CTAF

        //Para listado eventos con interrupción suministros
        public DateTime FechaEvento { get; set; }
        public string FechaEventoDesc { get; set; }

        public DateTime? Afefechainterr { get; set; }
        public DateTime? Afeplazofecha { get; set; }
        public DateTime? Afeplazofechaampl { get; set; }
        public DateTime? Afeplazofecmodificacion { get; set; }
        public string Afeplazousumodificacion { get; set; }

        //filtro consulta adicional
        public string EveSinDatosReportados { get; set; }
        public string FechaInterrupcion { get; set; }
        public int NumMaxSegundosInicio { get; set; }
        public string FechaPlazoEnvio { get; set; }
        public string PlazoEnvio { get; set; }

        public string ListaEmprcodi { get; set; }

        public string Eveninidesc { get; set; }
        public string Evenfindesc { get; set; }

        public string Reportado { get; set; }

        //Para ayuda en validacones
        public int ValidFecha { get; set; }
        public string ColorPlazo { get; set; }
        public bool EnPlazo { get; set; }
        public bool Deshabilidado { get; set; }
        public bool EmpresaActivo { get; set; }

        #endregion
        #region Mejoras CTAF
        public string CodigoCtaf { get; set; }
        public string Afeanio { get; set; }
        public string Afecorr { get; set; }
        public string Afeeracmf { get; set; }
        public bool ConInterrupcion { get; set; }
        #endregion

        #region Informes SGI
        public decimal? INTERRMW { get; set; }
        public decimal? BAJOMW { get; set; }


        #endregion
        public string FechasEventosSco { get; set; }
        public string EVENDESCCTAF { get; set; }
        public int DIASINFCTAF { get; set; }
        public int DIASINFTEC { get; set; }
        public DateTime AFEREUFECHAPROG { get; set; }
        public DateTime AFEITDECFECHAELAB { get; set; }
    }
}
