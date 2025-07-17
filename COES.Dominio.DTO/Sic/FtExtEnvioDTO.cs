using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO
    /// </summary>
    public partial class FtExtEnvioDTO : EntityBase
    {
        public int Ftenvcodi { get; set; }
        public int Ftetcodi { get; set; }
        public int Emprcodi { get; set; }
        public int? Ftprycodi { get; set; }
        public DateTime Ftenvfecsolicitud { get; set; }
        public string Ftenvususolicitud { get; set; }
        public DateTime? Ftenvfecaprobacion { get; set; }
        public string Ftenvusuaprobacion { get; set; }
        public DateTime? Ftenvfecfinrptasolicitud { get; set; }
        public DateTime? Ftenvfecfinsubsanarobs { get; set; }
        public int Ftenvtipoenvio { get; set; }
        public int? Ftevcodi { get; set; }
        public int Estenvcodi { get; set; }
        public DateTime? Ftenvfecmodificacion { get; set; }
        public string Ftenvusumodificacion { get; set; }
        public int Ftenvtipoformato { get; set; }
        public string Ftenvobs { get; set; }
        public DateTime? Ftenvfecvigencia { get; set; }
        public DateTime? Ftenvfecsistema { get; set; }
        public DateTime? Ftenvfecampliacion { get; set; }
        public DateTime? Ftenvfecobservacion { get; set; }
        public bool EsDenegacionAprobados { get; set; }
        public bool EsAmpliacionPlazo { get; set; }
        public string Ftenvenlacesint { get; set; }
        public string Ftenvenlacecarta { get; set; }
        public string Ftenvenlaceotro { get; set; }
        public DateTime? Ftenvfecinirev1 { get; set; }
        public DateTime? Ftenvfecinirev2 { get; set; }
        public int Ftenvtipocasoesp { get; set; }
        public string Ftenvflaghabeq { get; set; }
    }

    public partial class FtExtEnvioDTO
    {
        public bool FlagUpdateEnvio { get; set; }

        public int FtevercodiActual { get; set; }
        public FtExtEnvioVersionDTO VersionActual { get; set; }

        public int FtevercodiOficial { get; set; }
        public FtExtEnvioVersionDTO VersionOficialActual { get; set; }
        public FtExtEnvioVersionDTO VersionAnterior { get; set; }

        public int FtevercodiTemporalExtranet { get; set; }
        public FtExtEnvioVersionDTO VersionTemporalExtranet { get; set; }
        public int FtevercodiTemporalIntranet { get; set; }
        public FtExtEnvioVersionDTO VersionTemporalIntranet { get; set; }

        public int FtevercodiTemporalIntranetSolicitud { get; set; }
        public int FtevercodiTemporalIntranetSubsanacion { get; set; }

        public int FtevercodiTemporalFTVigente { get; set; }

        public FtExtEnvioLogDTO LogEnvioActual { get; set; }
        public List<FtExtEnvioLogDTO> ListaLog { get; set; }
        public List<string> CodigoEquipos { get; set; }

        public List<FtExtEnvioArchivoDTO> ListaArchivo { get; set; }
        public List<FtExtEnvioRevisionDTO> ListaRevision { get; set; }

        public string Emprnomb { get; set; }
        public string Ftetnombre { get; set; }
        public string Ftprynombre { get; set; }
        public string EquiposProyecto { get; set; }
        public string EquiposProyectoUnico { get; set; }
        public string NombreEquipos { get; set; }
        public string NombreEquiposUnico { get; set; }
        public string Faremnombre { get; set; }
        public int? Faremcodi { get; set; }
        public int? Ftevercodi { get; set; }
        public int Estenvcodiversion { get; set; }
        public int NumeroAmpliaciones { get; set; }
        public string Estenvnomb { get; set; }
        public string Estenvcolor { get; set; }

        public string FtenvfecsolicitudDesc { get; set; }
        public string FtenvfecaprobacionDesc { get; set; }
        public string FtenvfecfinrptasolicitudDesc { get; set; }
        public string FtenvfecfinsubsanarobsDesc { get; set; }
        public string FechaDesaprobacionDesc { get; set; }
        public string FtenvfecmodificacionDesc { get; set; }
        public string FechaVigenciaDesc { get; set; }
        public string FechaAprobacionParcialDesc { get; set; }
        public string FechaMaxRptaDerivacionDesc { get; set; }
        public string FtenvfecsistemaDesc { get; set; }
        public string FtenvfecobservacionDesc { get; set; }
        public string FtenvfecampliacionDesc { get; set; }

        public bool EsEditableExtranet { get; set; }
        public bool EsEditableIntranet { get; set; }

        public string UsuariosAgentesTotales { get; set; }
        public string AgenteUltimoEvento { get; set; }
        public string OtrosAgentesEmpresaUsuarioSolicitud { get; set; }
        public string OtrosAgentesEmpresaUsuarioUltEvento { get; set; }

        //equipo aprobado
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public string Tipoelemento { get; set; }
        public int Idelemento { get; set; }
        public string Nombreelemento { get; set; }
        public string Abrevelemento { get; set; }
        public string Estadoelemento { get; set; }
        public string Areaelemento { get; set; }
        public int? Famcodi { get; set; }
        public int? Catecodi { get; set; }
        public int Fteeqcodi { get; set; }

        public string CorreosCCAgentes { get; set; }
        public string CorreosCCAgentesCopropietarios { get; set; }
        public string MensajeAlAgente { get; set; }

        public int TipoAccionIntranet { get; set; }
        public int TipoAccionExtranet { get; set; }
        public List<FtExtEnvioDatoDTO> ListaParametrosVacios { get; set; }
        public int TieneParametrosVacios { get; set; }
        public string OpcionReemplazo { get; set; }
        public string HtmlTablaDenegados { get; set; }
        public string HtmlTablaAprobados { get; set; }
        public string DiasRecepcionSolicitud { get; set; }
        public string DiasRecepcionSubsanacion { get; set; }
        public string Envarestado { get; set; }
        public bool EsEditableParaAreas { get; set; }
        public string Estenvnombversion { get; set; }
        
        public DateTime Envarfecmaxrpta { get; set; }

        public string CorreosAreasAsignadoPendienteRevision { get; set; }
        public string NombreAreaPendienteRevision { get; set; }
        public string DiasPlazoFinRevisionAreas { get; set; }
        public string CorreosDelAreaQuienEstaRevisando { get; set; }
        public string NombreAreaQuienEstaRevisando { get; set; }
        public string CorreosDelAreaQuienesDebieronRevisar { get; set; }
        public string NombreAreaQuienDebioRevisar { get; set; }

        //Validacion nuevo envio
        public string MensajeEqSinFicha { get; set; }

        //reporte
        public bool EsEqVigente { get; set; }
        public string EstadoelementoDesc { get; set; }

        public bool UsuarioPerteneceAArea { get; set; }
        public string Ftenvfecinirev1Desc { get; set; }
        public string Ftenvfecinirev2Desc { get; set; }
    }
}
