using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using System;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Migraciones.Models
{
    public class MigracionesModel
    {
        public List<SiAreaDTO> ListaAreas { get; set; }
        public string Resultado { get; set; }

        public int nRegistros { get; set; }
        public string Fecha { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string Actabrev { get; set; }
        public string Actnomb { get; set; }
        public string Readonly { get; set; }
        public string Disabled { get; set; }
        public List<MeLecturaDTO> TipoProgramacion { get; set; }
        public List<string> ListaString { get; set; }
        public List<string> ListaStringNoRepet { get; set; }
        public string Comentario { get; set; }
        public string Mes { get; set; }
        public string Anio { get; set; }
        public List<UserModel> ListaSelect { get; set; }
        public string MesAnio { get; set; }
        public HandsonModel Handson { get; set; }
        public int Administrador { get; set; }
        public List<SiTipoinformacionDTO> ListaTipoInfo { get; set; }
        public List<TrEmpresaSp7DTO> ListaTrEmpresa { get; set; }
        public List<ScEmpresaDTO> GetListaEmpresaRis7 { get; set; }
        public List<TrZonaSp7DTO> ListaTrzonas { get; set; }
        public WbComunicadosDTO Wbcomunicados { get; set; }
        public string Mesanio { get; set; }
        public List<TipoInformacion> ListaSemanas { get; set; }
        public List<MeLecturaDTO> TipoProgramacion3 { get; set; }
        public List<MeLecturaDTO> TipoProgramacion4 { get; set; }

        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }

        public int Areacodi { get; set; }
        public int Actcodi { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

        public List<DateTime> FechasPaginado { get; set; }

        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public List<string> ListaResultado { get; set; }
        public string ResultadoObservaciones { get; set; }
        public string CostoTotalOperacion { get; set; }
        public string CostoTotalOperacionNuevo { get; set; }
        public bool HayDiferenciaCosto { get; set; }
        public bool TienePermisoRecalculo { get; set; }
        public string MensajeError { get; set; }
        public string MensajeOK { get; set; }

        public bool TienePermisoGrabar { get; set; }

        public string Imagen { get; set; }

        public string Resultado2 { get; set; }
        public string Resultado3 { get; set; }

        public int IdReporte { get; set; }

        public string FechaIniSemana { get; set; }
        public string FechaFinSemana { get; set; }
        public int IdReporteHidrologiaSemanal { get; set; }
        public List<RegistroRepHidrologia> ListadoHidrologia { get; set; }
        public ReporteProduccion DatosRepGeneracionCoes { get; set; }
        public ReporteProduccion DatosRepGeneracionNoCoes { get; set; }
        public List<MeGpsDTO> ListaGPS { get; set; }
    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionMigraciones
    {
        public const string SesionListaEquirel = "SesionFormato";
        public const string SesionListaGeneracionOpera = "SesionListaGeneracionOpera";
        public const string SesionListaGeneracionNoOpera = "SesionListaGeneracionNoOpera";
        public const string SesionListaLineas = "SesionListaLineas";
        public const string SesionListaManttos = "SesionListaManttos";
        public const string SesionListaSvc = "SesionListaSvc";
        public const string SesionListaTrafo2d = "SesionListaTrafo2d";
        public const string SesionListaTrafo3d = "SesionListaTrafo3d";
        public const string SesionListaDemanda = "SesionListaDemanda";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionSfecha = "SesionSfecha";
        public const string SesionScanalcodi = "SesionScanalcodi";
        public const string SesionCDespacho = "SesionCDespacho";
        public const string SesionListaMe48 = "SesionListaMe48";
        public const string SesionListaMe1 = "SesionListaMe1";
        public const string SesionListaCostos = "SesionListaCostos";
        public const string SesionListaPotencia = "SesionListaPotencia";
        public const string SesionCostoTotalOperacion = "SesionCostoTotalOperacion";
    }
}