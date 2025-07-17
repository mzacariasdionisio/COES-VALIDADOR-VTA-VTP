using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Scada;
using COES.Servicios.Aplicacion.Eventos.Helper;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class AnalisisFallasModel
    {
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
        public string EveSinDatosReportados { get; set; }
        public string Afecodi { get; set; }
        public string FechEvento { get; set; }
        public string Emprcodi { get; set; }
        public string Tipoemprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string TipoEmpresa { get; set; }
        public string CodEracmf { get; set; }
        public string CodOsinergmin { get; set; }
        public string Resultado { get; set; }
        public string Resultado2 { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public string NombreArchivo { get; set; }
        public AnalisisFallaDTO oAnalisisFallaDTO { get; set; }
        public EventoDTO oEventoDTO { get; set; }
        public EquipoDTO oEquipoDTO { get; set; }
        public List<EmpresaInvolucradaDTO> lsEmpresaInvolucrada { get; set; }
        public List<EmpresaInvolucradaDTO> lsEmpresaInvolucradaReunion { get; set; }
        public List<EmpresaRecomendacionDTO> lsEmpresaRecomendacionInformeTecnico { get; set; }
        public List<EmpresaObservacionDTO> lsEmpresaObservacionInformeTecnico { get; set; }
        public List<EmpresaFuerzaMayorDTO> lsEmpresaFuerzaMayorInformeTecnico { get; set; }
        public List<EmpresaResponsableDTO> lsEmpresaResponsableCompensacion { get; set; }
        public List<EmpresaResponsableDTO> lsEmpresaCompensadaCompensacion { get; set; }

        public List<ReunionResponsableDTO> lsReunionResponsable { get; set; }
        public List<EmpresaReporte> lsEmpresaConfigurable { get; set; }
        public List<ReclamoDTO> lsReclamoReconsideracion { get; set; }
        public List<ReclamoDTO> lsReclamoApelacion { get; set; }
        public List<ReclamoDTO> lsReclamoArbitraje { get; set; }
        public bool grabar { get; set; }
        public List<EventoDTO> LstEvento { get; set; }
        public List<EventoModel> LstEventos { get; set; }
        public List<InformeCTAFDTO> CTAFINFORMEREPORTE { get; set; }
        public List<SecuenciaCTAFDTO> SecuenciaEventoREPORTE { get; set; }
        public List<SenalizacionCTAFDTO> SenalizacionREPORTE { get; set; }
        public List<SiSenializacionDTO> ListaSenializacionProteccionInforme { get; set; }
        public List<SiSenializacionDTO> ListaSenializacionProteccion { get; set; }
        public List<SuministroCTAFDTO> SuministroREPORTE { get; set; }
        public AfEracmfEventoDTO EracmfEventoDTO { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public List<EmpresaReporte> ListaReporte { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EmpresasTipo> LstEmpresasT { get; set; }
        public List<SiFuentedatosDTO> ListaTipoInformacion { get; internal set; }
        public List<string> ListaReporte1Html { get; set; }
        public List<string> ListaReporte2Html { get; set; }
        public List<string> ListaReporte3Html { get; set; }
        public List<AfSolicitudRespDTO> ListSolicitudes { get; set; }
        public AfSolicitudRespDTO oSolicitudDTO { get; set; }
        public string EstadoSoli { get; set; }
        public List<string> ListaArchivos { get; set; }
        public string ArchivoFinal { get; set; }
        public string Modulo { get; set; }
        public string ValorEnPlazo { get; set; }
        public string ValorFinPlazo { get; set; }
        public string ValorEjecucion { get; set; }
        public List<AfHoraCoordDTO> ListaHandsonHorasCoord { get; internal set; }
        public List<AfHoraCoordDTO> ListaHandsonHorasSuministradora { get; internal set; }
        public List<AfCondicionesDTO> ListaHandsonEtapasFunc { get; internal set; }
        public List<AfHoraCoordDTO> ListaHandsonAgentesDemoras { get; internal set; }
        #region Mejoras CTAF
        public List<EventoDTO> ListaEventosSco { get; set; }
        public List<EveInformesScoDTO> LstInformes { get; set; }
        public List<ReunionResponsableDTO> LstAsistenteResponsable { get; set; }
        public List<SecuenciaEventoDTO> LstSecuenciaEvento { get; set; }
        public string Afeanio { get; set; }
        public string Afecorr { get; set; }
        public string Evenini { get; set; }       
        public string Afefechainter { get; set; }
        public string Afeeracmf { get; set; }
        public string url { get; set; }
        public string Afalerta { get; set; }
        #endregion
        public HandsonModel Handson { get; set; }
        public List<CeldaCambios> ListaCambios { get; set; }
        public List<TrZonaSp7DTO> ListaTrZonaSp7 { get; set; }
        public List<TrCanalSp7DTO> ListaTrCanalSp7 { get; set; }
        public string FechasEventos { get; set; }
        public List<EveintdescargaDTO> lstInterruptoresDescargadores { get; set; }
        public List<DatosSP7DTO> listaTrCircularSp7GraficaFiltrada { get; set; }
        public List<TrCanalSp7DTO> listCanalesSp7 { get; set; }
        public List<InformeCtafModel> ListaInformeCtaf { get; set; }
        public string FechaInicioAnalisis { get; set; }
        public string FechaHastaAnalisis { get; set; }
        public string HoraIniAnalisis { get; set; }
        public string HoraFinAnalisis { get; set; }
        public List<EveRecomobservDTO> ListaEveRecomobserv { get; set; }
        public List<EveRecomobservDTO> ListaObservaciones { get; set; }
        public List<EveTiposNumeralDTO> ListaTiposNumerales { get; set; }
        public int Evencodi { get; set; }
    }

    public class HandsonModel
    {
        public string[][] ListaExcelData { get; set; }
        public string ListaExcelData2 { get; set; }
        public string Esquema { get; set; }
        public string[][] ListaExcelDescripcion { get; set; }
        public string[][] ListaExcelFormatoHtml { get; set; }
        public List<CeldaMerge> ListaMerge { get; set; }
        public List<int> ListaColWidth { get; set; }
        public List<Boolean> ListaFilaReadOnly { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Boolean ReadOnly { get; set; }
        public string[][] ListaSourceDropDown { get; set; }
        public short[][] MatrizTipoEstado { get; set; }
        public int[][] MatrizDigitoDecimal { get; set; }
        public string[][] MatrizCeldaExcel { get; set; }
        public string[][] ListaExcelComment { get; set; }
        public int TieneData { get; set; }
        public bool[][] MatrizEstado { get; set; }
        public List<string> ListaDropDown { get; set; }
        public int HMaximoData48Enviado { get; set; }
        public int HMaximoDataScadaDisponible { get; set; }
        public string Resultado { get; set; }
        public object[] Columnas { get; set; }
        public string[] Headers { get; set; }
        public List<CeldaCambios> ListaCambios { get; set; }
        public int ColCabecera { get; set; }
        public int FilasCabecera { get; set; }
        public int MaxCols { get; set; }
        public int MaxRows { get; set; }
    }
    public class CeldaMerge
    {
        public int col { get; set; }
        public int row { get; set; }
        public int colspan { get; set; }
        public int rowspan { get; set; }
        public int rowAbsoluto { get; set; }
    }
    public class CeldaCambios
    {
        public int Col { get; set; }
        public int Row { get; set; }
    }
    /// <summary>
    /// Constantes para el manejo de handson
    /// </summary>
    public class HandsonConstantes
    {
        public const int ColWidth = 145;
        public const int ColPorHoja = 7;
    }

    public class InformeCtafModel
    {
        public string CabNombreEvento { get; set; }
        public string CabDescripcionEvento { get; set; }
        public DateTime? EVENINI { get; set; }
        public List<EveCondPreviaDTO> listaCondPreviaLinea { get; set; }
        public List<EveCondPreviaDTO> listaCondPreviaCentral { get; set; }
        public List<EveCondPreviaDTO> listaCondPreviaTransformadores { get; set; }
        public List<SecuenciaCTAFDTO> listaSecuenciaEvento { get; set; }
        public List<SecuenciaEventoEmpresaDTO> listaSecuenciaEventoEmpresaFooter { get; set; }
        public List<EveAnalisisEventoDTO> listaAnalisisEvento { get; set; }
    }

    public class IndicadorCtafModel
    {
        public int Anio { get; set; }
        public List<AfIndicadoresDTO> listaAfIndicadores { get; set; }
    }
}