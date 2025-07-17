using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.IEOD.Models
{
    public class EquiposSEINModel
    {

        public int Eeqcodi { get; set; }
        public int EquiCodi { get; set; }
        public int SubcausaCodi { get; set; }
        public DateTime? EeqFechaIni { get; set; }
        public int EeqEstado { get; set; }
        public string EeqDescripcion { get; set; }
        public DateTime? EeqFechaFin { get; set; }


        public string Fecha { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Anho { get; set; }
        public string SemanaIni { get; set; }
        public string SemanaFin { get; set; }
        public string FechaPlazo { get; set; }
        public int HoraPlazo { get; set; }
        public string Resultado { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public string Mes { get; set; }
        public string Semana { get; set; }
        public string Dia { get; set; }
        public int NroSemana { get; set; }

        // variables listas
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        // public List<EveEvenequipoDTO> ListaEquipoSEIN { get; set; }
        public List<EveEventoEquipoDTO> ListaEquipoSEIN { get; set; }



        public List<MeHojaptomedDTO> ListaHojaPto { get; set; }
        public List<MeHeadcolumnDTO> ListaHeadColumn { get; set; }
        public List<MePtomedicionDTO> ListaMedicion { get; set; }
        public List<MeEnvioDTO> ListaEnvio { get; set; }
        public List<MeEstadoenvioDTO> ListaEstadoEnvio { get; set; }
        public List<PtoMedida> ListaPtoMedida { get; set; }
        public List<EqEquipoDTO> ListaCuenca { get; set; }
        public List<EqEquipoDTO> ListaRecursosCuenca { get; set; }
        public List<EqFamiliaDTO> ListaTipoRecursos { get; set; }
        public List<MeTipopuntomedicionDTO> ListaTipoPtoMedicion { get; set; }
        public List<MeLecturaDTO> ListaTipoLectura { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<MeLecturaDTO> LstTipoInformacion { get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<TipoInformacion> ListaTipoInformacionEnvio { get; set; }
        public List<TipoInformacion> ListaTipoFormato { get; set; }
        public List<TipoInformacion> ListaFrecuencia { get; set; }
        public List<TipoInformacion> ListaSemanas { get; set; }
        public List<MeMedicion1DTO> ListaMedicion1 { get; set; }
        public List<MeMedicion24DTO> ListaMedicion24 { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }
        public List<MeMedicion24DTO> ListaMedicion24horas { get; set; }
        public List<MeMedicion24DTO> ListaMedicion24Cabecera { get; set; }
        public List<MeMedicion24DTO> ListaMedicion24dias { get; set; }
        public List<MeMedicion24DTO> ListaMedicion24semanas { get; set; }
        public List<MeMedicion24DTO> ListaMedicion24meses { get; set; }
        public List<SiTipoinformacionDTO> ListaUnidades { get; set; }
        public List<TipoInformacion> LstTipoReporte { get; set; }
        public List<MeMedicionxintervaloDTO> ListaDescargaVert { get; set; }
        public string TituloReporteXLS { get; set; }
        public string NombreFortmato { get; set; }
        public string SheetName { get; set; }
        public int TipoReporte { get; set; }

        public string CadenaLectCodi { get; set; }
        public string CadenaLectPeriodo { get; set; }

        public int IdModulo { get; set; }
        public int IdLectura { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public string StrFormatCodi { get; set; }
        public string StrFormatPeriodo { get; set; }
        public GraficoWeb Grafico { get; set; }
        public int RbTipoReporte { get; set; }

        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
    }

    public class CambioEnvioModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<MeCambioenvioDTO> ListaCambioEnvio { get; set; }
        public List<TipoInformacion> ListaSemanas { get; set; }
        public int IdModulo { get; set; }
        public string StrFormatCodi { get; set; }
        public string StrFormatPeriodo { get; set; }
        public string StrFormatDescrip { get; set; }
        public int Columnas { get; set; }
        public int Resolucion { get; set; }

        public string Mes { get; set; }
        public string Semana { get; set; }
        public string Dia { get; set; }
        public string Fecha { get; set; }
        public string Anho { get; set; }
        public int NroSemana { get; set; }
    }

    public class PtoMedida
    {
        public int IdMedida { get; set; }
        public string NombreMedida { get; set; }
    }

    public class BusquedaModel
    {
        public List<TipoInformacion> ListaSemanas { get; set; }
        public int SemanaDestino { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }

        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EqAreaDTO> ListaAreas { get; set; }

        public List<EveSubcausaeventoDTO> ListaMotivo { get; set; }

        public List<MeLecturaDTO> ListaLectura { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<MeCambioenvioDTO> ListaCambioEnvio { get; set; }
        public int IdModulo { get; set; }
        public string StrFormatCodi { get; set; }
        public string StrFormatPeriodo { get; set; }
        public string StrFormatDescrip { get; set; }
        public int Columnas { get; set; }
        public int Resolucion { get; set; }
        public string Mes { get; set; }
        public string Semana { get; set; }
        public string Dia { get; set; }
        public string Fecha { get; set; }
        public string Anho { get; set; }
        public string AnhoMes { get; set; }
        public int NroSemana { get; set; }
        public string FechaPlazo { get; set; }
        public int HoraPlazo { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }
    }
}