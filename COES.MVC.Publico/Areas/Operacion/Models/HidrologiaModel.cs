using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.Operacion.Models
{
    public class HidrologiaModel
    {

        public string Fecha { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Anho { get; set; }
        public string SemanaIni { get; set; }
        public string SemanaFin { get; set; }
        public string FechaPlazo { get; set; }
        public int HoraPlazo { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Detalle2 { get; set; }

        public MeFormatoDTO Formato { get; set; }
        public string Mes { get; set; }
        public string Semana { get; set; }
        public string Dia { get; set; }
        public int NroSemana { get; set; }

        // variables listas
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
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

        public string FechaRepInicio { get; set; }
        public string FechaRepFinal { get; set; }

        /// <summary>
        /// Reporte de pronostico
        /// </summary>
        public int TipoReportePronostico { get; set; }
        public string HtmlReportePronostico { get; set; }
        public string HtmlReporteHistorico { get; set; }
        public string NomTipoPeriodo { get; set; }
        public DateTime FechainiPronostico { get; set; }
        public DateTime FechafinPronostico { get; set; }
        public DateTime FechainiHistorico { get; set; }
        public DateTime FechafinHistorico { get; set; }
        public List<DateTime> ListaFechaPronostico { get; set; }
        public List<DateTime> ListaFechaHistorico { get; set; }
        public string TituloReportePronostico { get; set; }
        public string TituloReporteHistorico { get; set; }
        public string FiltroPronosticoDesc { get; set; }
        public string FiltroPronosticoValor { get; set; }
        public string FiltroHistoricoDesc { get; set; }
        public string FiltroHistoricoValor { get; set; }
        public string HojaPronostico { get; set; }
        public string HojaHistorico { get; set; }
        public string EmpresaDesc { get; set; }
        public string UbicacionDesc { get; set; }
        public string PuntoCalculadoDesc { get; set; }
        public string NombreArchivo { get; set; }
        //inicio agregado
        public int idReportePronostico { get; set; }
        //fin agregado

        /// <summary>
        /// Reporte de cumplimiento
        /// </summary>
        public List<FormatoCoes> ListaFormatoCOES { get; set; }

        /// <summary>
        /// Reporte App Powel
        /// </summary>
        public List<MeReporptomedDTO> Listacabecera { get; set; }
        public DateTime FechaIniPowel { get; set; }
        public DateTime FechaFinPowel { get; set; }

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
    }

    public class PtoPronostico
    {
        public int Ptocodi { get; set; }
        public bool EsCorrelacionSuma { get; set; }
        public int? NextPtomedicodi { get; set; }
        public bool EsSumado { get; set; }

        public PtoPronostico()
        {
        }

        public PtoPronostico(int Ptocodi, bool EsCorrelacionSuma, int? NextPtomedicodi)
        {
            this.Ptocodi = Ptocodi;
            this.EsCorrelacionSuma = EsCorrelacionSuma;
            this.NextPtomedicodi = NextPtomedicodi;
        }
    }

    public class FormatoCoes
    {
        public string Areaname { get; set; }
        public int Areacodi { get; set; }
        public string html { get; set; }
    }

}