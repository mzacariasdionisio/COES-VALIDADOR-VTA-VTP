using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Subastas
{
    /// <summary>
    /// Contiene constantes de subasta
    /// </summary>
    public class ConstantesSubasta
    {
        #region Encriptación

        //Encrypt Section 
        private const int Keysize = 256;
        private const int DerivationIterations = 1000;
        public static readonly string PasswordHash = "P@@Sw0rd";
        public static readonly string SaltKey = "S@LT&KEY";
        public static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        #endregion

        public const string Registrar = "Se Registro Correctamente";
        public const string Modificar = "Registro Modificado";
        public const string Eliminar = "Se Elimino Correctamente";

        public const string ErrorDeSistema = "Error en el Sistema...";
        public const string Duplicada = "Información se encuentra registrada. Ingrese otro valor.";

        public static readonly string[] ArrHoras = {"00:00", "00:30", "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30"
                               , "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30"
                               , "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30"
                               , "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30", "22:00", "22:30", "23:00", "23:30", "23:59"};

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_usuario_Ofertas_RSF.rar";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizOfertasRSFModuloManual = "Ofertas RSF\\";

        #region URS_CALIFICADAS
        //para las URS Calificadas
        public const int ConceptoURSFecInicio = 567;
        public const int ConceptoURSFecfIn = 568;
        public const int ConceptoURSActa = 569;
        public const int ConceptoURSBanda = 609;
        public const string ConceptoURSFull = "567,568,569,609";

        //>>>>>>>>>>>>>>>>>>>>>>>> para los modos >>>>>>>>>>>>>>>>>>>>>>>>>>
        // Catecodi 5 - Grupo Hidraúlico
        public const int ConceptoGrupHidraPotenciaMin = 570;
        public const int ConceptoGrupHidraPotenciaMax = 571;
        public const int ConceptoGrupHidraBandaCalificada = 572;
        public const int ConceptoGrupHidraComentario = 573;

        // Catecodi 9 - Modo Operación Hidro
        public const int ConceptoMOHidroPotenciaMin = 574;
        public const int ConceptoMOHidroPotenciaMax = 575;
        public const int ConceptoMOHidroBandaCalificada = 576;
        public const int ConceptoMOHidroComentario = 577;

        // Catecodi 5 - Modo de Operación Termo
        public const int ConceptoMOTermoPotenciaMin = 578;
        public const int ConceptoMOTermoPotenciaMax = 579;
        public const int ConceptoMOTermoBandaCalificada = 580;
        public const int ConceptoMOTermoComentario = 581;

        public const string ConcepcodiURSyModosCalificadas = "567,568,569,570,571,572,573,574,575,576,577,578,579,580,581,609";

        public const string NombreTabURSCalificadas = "URS Calificadas";
        public const string TituloReporteURSCalificada = "Reporte de URS Calificadas";

        public const string NombreTabURSBase = "URS Base Adjudicada";
        public const string TituloReporteURSBase = "Reporte de URS con Provisión Base Adjudicada";
        #endregion

        #region URS_BASE
        //para las URS Calificadas
        public const int ConceptoURSBaseFecInicio = 582;
        public const int ConceptoURSBaseFecfIn = 583;
        public const int ConceptoURSBaseActa = 584;
        public const string ConceptoURSBaseFull = "582,583,584";

        //>>>>>>>>>>>>>>>>>>>>>>>> para los modos >>>>>>>>>>>>>>>>>>>>>>>>>>
        // Catecodi 5 - Grupo Hidraúlico
        public const int ConceptoURSBaseGrupHidraPotenciaMin = 585;
        public const int ConceptoURSBaseGrupHidraPotenciaMax = 586;
        public const int ConceptoURSBaseGrupHidraBanda = 587;
        public const int ConceptoURSBaseGrupHidraPrecioMin = 588;
        public const int ConceptoURSBaseGrupHidraPrecioMax = 589;
        public const int ConceptoURSBaseGrupHidraFecInicio = 590;
        public const int ConceptoURSBaseGrupHidraFecfIn = 591;
        public const int ConceptoURSBaseGrupHidraComentario = 592;

        // Catecodi 9 - Modo Operación Hidro
        public const int ConceptoURSBaseMOHidroPotenciaMin = 593;
        public const int ConceptoURSBaseMOHidroPotenciaMax = 594;
        public const int ConceptoURSBaseMOHidroBanda = 595;
        public const int ConceptoURSBaseMOHidroPrecioMin = 596;
        public const int ConceptoURSBaseMOHidroPrecioMax = 597;
        public const int ConceptoURSBaseMOHidroFecInicio = 598;
        public const int ConceptoURSBaseMOHidroFecfIn = 599;
        public const int ConceptoURSBaseMOHidroComentario = 600;

        // Catecodi 5 - Modo de Operación Termo

        public const int ConceptoURSBaseMOTermoPotenciaMin = 601;
        public const int ConceptoURSBaseMOTermoPotenciaMax = 602;
        public const int ConceptoURSBaseMOTermoBanda = 603;
        public const int ConceptoURSBaseMOTermoPrecioMin = 604;
        public const int ConceptoURSBaseMOTermoPrecioMax = 605;
        public const int ConceptoURSBaseMOTermoFecInicio = 606;
        public const int ConceptoURSBaseMOTermoFecfIn = 607;
        public const int ConceptoURSBaseMOTermoComentario = 608;

        public const string ConcepcodiURSyModosBase = "582,583,584,585,586,587,588,589,590,591,592,593,594,595,596,597,598,599,600,601,602,603,604,605,606,607,608";

        public const string EstadoURSVigente = "1";
        public const string EstadoURSConProvBase = "2";
        #endregion

        //Tipo de carga
        public const int TipoCargaBanda = 0;
        public const int TipoCargaSubir = 1;
        public const int TipoCargaBajar = 2;
        public const string TabCargaSubir = "SUBIR";
        public const string TabCargaBajar = "BAJAR";
        public const string TituloReporteOfertaDefecto = "Reporte de Ofertas por Defecto";
        public const string TituloReporteOfertaDiaria = "Reporte de Ofertas por Día";

        //ConstantesValores
        public const int Todos = -1;
        public const int OfertipoDefecto = 0;
        public const int OfertipoDiaria = 1;
        public const string EstadoDefecto = "-1";
        public const string EstadoActivo = "A";
        public const string EstadoInactivo = "I";

        //ConstantesConfiguracion
        public const string ConcepcodiConfig = "562,563,564,565,566";
        public const int AccionVer = 1;
        public const int AccionEditar = 2;
        public const int AccionNuevo = 3;
        public const int GrupodatActivo = 0;
        public const int GrupodatInactivo = 1;

        public const int AccionEliminar = 4;

        public const int AccionExcelWebCrearNuevo = 1;
        public const int AccionExcelWebCopiar = 4;

        public const int AccionURSCalificada = 1;
        public const int AccionURSBase = 2;

        //Parámetros de configuración
        public const int PrecioMinimo = 562;
        public const int PotenciaNoMinimo = 563;
        public const int PotenciaURSMinimoAuto = 564;
        public const int PotenciaURSMinimoMan = 565;
        public const int PrecioMaximo = 566;

        //Archivos Actas
        public const string SModuloSubastas = "Subastas";
        public const string SUrsCalSubastasFile = "Subastas\\URS_Calificada\\";
        public const string SUrsProvSubastasFile = "Subastas\\URS_Provision_Base\\";
        public const string SNombreCarpetaTemporal = "Temporal";
        public const string SNombreCarpetaActa = "Acta";

        public const string KeyAmbientePrueba = "SubastasAmbientePrueba";
        public const string KeyUsercodePrueba = "SubastasUsercodePrueba";
        public const string KeyTamanioArchivoActa = "SubastasTamanioFileActa";
        //public const string KeyUrscodiProvisionBase = "SubastasUrscodiProvisionBase";
        public const string KeyFechaDespliegue = "SubastasFechaDespliegue";
        public const int MenuOpcionCodeSubastas = 527;

        #region RSF 2024
        public const string DatoDeficitRSF = "D";
        public const string DatoReduccionBanda = "R";
        public const string ReservaSubir = "S";
        public const string ReservaBajar = "B";

        public const string EstadoVigenteDesc = "Vigente";
        public const string EstadoNoVigenteDesc = "No Vigente";

        public const int ParametroPotenciaUrsMinAuto = 564;
        public const string Si = "S";
        public const string No = "N";
        public const string TipoTotal = "T";
        public const string TipoParcial = "P";

        public const string FuenteActivacion = "A";
        public const string FuenteExtranet = "E";
        public const string FuenteTodos = "T";

        public const string GraficoUrsSubir = "1";
        public const string GraficoUrsBajar = "2";
        public const string GraficoTotalSubir = "3";
        public const string GraficoTotalBajar = "4";

        public const string CaracterV = "V";

        public const int OfdetipoSubir = 1;
        public const int OfdetipoBajar = 2;

        public const int SrestcodiSubir = 47;
        public const int SrestcodiBajar = 48;

        public const int CasoAutogeneracionOfertasDefectoMesEnero = 1;
        public const int CasoActivacionOfertaDefecto = 2;



        #endregion

        #region Plantilla de correo y Proceso automático

        public const int PlantcodiNotifAgenteSinOfertaXDefecto = 310;
        public const int PlantcodiNotifAutogenerarOfertaXDefecto = 311;

        public const string VariableNombreEmpresa = "{NOMBRE_EMPRESA}";
        public const string VariableAnioSiguiente = "{ANIO_SIGUIENTE}";
        public const string VariableCorreosAgente = "{CORREOS_AGENTE}";
        public const string VariableTablaOfertaDefectoAutogenerado = "{TABLA_OFERTAS_POR_DEFECTO_AUTOGENERADO}";

        public const int UsercodeSistema = 1;

        //SI_PROCESO
        public const int PrcscodiDesencriptarOfertaDiariaSubasta = 4;
        public const int PrcscodiOfertaRSF_RecordatorioSinOfDefectoAnioSig = 110;
        public const int PrcscodiOfertaRSF_AutogeneracionOfDefectoAnioSig = 111;

        public const string MetodoDesencriptarOfertaDiariaSubasta = "DesencriptarOfertaDiariaSubasta";
        public const string MetodoOfertaRSF_RecordatorioSinOfDefectoAnioSig = "OfertaRSF_RecordatorioSinOfDefectoAnioSig";
        public const string MetodoOfertaRSF_AutogeneracionOfDefectoAnioSig = "OfertaRSF_AutogeneracionOfDefectoAnioSig";

        #endregion
    }

    public class ReporteRSModel
    {
        public string Urs { get; set; }
        public string Dia { get; set; }
        public string Hora { get; set; }
        public decimal? PotenciaOfertada { get; set; }
        public string ColorFondo { get; set; }
        public string ColorTexto { get; set; }
    }

    #region RSF 2024
    public class DatoActivacionOferta
    {
        public SmaActivacionDataDTO DatosDeficitSubir { get; set; }
        public SmaActivacionDataDTO DatosReduccionBandaSubir { get; set; }
        public List<int> IdsMotivosSubir { get; set; }
        public SmaActivacionDataDTO DatosDeficitBajar { get; set; }
        public SmaActivacionDataDTO DatosReduccionBandaBajar { get; set; }
        public List<int> IdsMotivosBajar { get; set; }
        public bool ExisteActivacionOferta { get; set; }
    }

    public class RangoActivacionOfertaDefecto
    {
        public int EscenarioIni { get; set; }
        public int EscenarioFin { get; set; }
        public string HoraIni { get; set; }
        public string HoraFin { get; set; }
    }

    public class RangoEscenario
    {
        public int Escenario { get; set; }
        public string HoraIni { get; set; }
        public string HoraFin { get; set; }
    }

    public class RangoActivacionPorUrs
    {
        public int Urscodi { get; set; }
        public string Ursnombre { get; set; }
        public int TipoReserva { get; set; }
        public List<RangoActivacionOfertaDefecto> ListaRangosActivacion { get; set; }
    }

    public class InsumosActivacionPorEscenario
    {
        public int Urscodi { get; set; }
        public string Ursnomb { get; set; }
        public int Escenario { get; set; }
        public decimal? ParamPotMinAuto { get; set; }
        public decimal? PotOfertaDefecto { get; set; }
        public decimal? PotOfertaDiaria { get; set; }
        public decimal? PotOfertaDiariaOtroTipoCarga { get; set; }
        public decimal? BandaDisponible { get; set; }
        public decimal? PotActivada { get; set; }

        public bool TieneOfertaDiariaParaTipoCarga { get; set; }
        public bool TieneOfertaDiariaParaOtroTipoCarga { get; set; }
        public decimal? PotenciaOfertada { get; set; }
        public bool DebeActivarseOferta { get; set; }

        public string Precio { get; set; }
    }

    public class Bitacora
    {
        public string FechaOferta { get; set; }
        public string Deficit { get; set; }
        public int EscenarioIni { get; set; }
        public int EscenarioFin { get; set; }
        public string HoraIni { get; set; }
        public string HoraFin { get; set; }
        public string Horario { get; set; }
        public decimal? Promedio { get; set; }
        public decimal? Minimo { get; set; }
        public decimal? Maximo { get; set; }
        public string Motivos { get; set; }
        public List<SmaMaestroMotivoDTO> ValoresMotivos { get; set; }
        public List<decimal> ValoresDeficit { get; set; }
        public decimal? Banda { get; set; }
    }

    public class Dato48
    {
        public bool ConInformacion { get; set; }
        public int Urscodi { get; set; }
        public string Etiqueta { get; set; }
        //public decimal? Total { get; set; }
        public decimal? V1 { get; set; }
        public decimal? V2 { get; set; }
        public decimal? V3 { get; set; }
        public decimal? V4 { get; set; }
        public decimal? V5 { get; set; }
        public decimal? V6 { get; set; }
        public decimal? V7 { get; set; }
        public decimal? V8 { get; set; }
        public decimal? V9 { get; set; }
        public decimal? V10 { get; set; }
        public decimal? V11 { get; set; }
        public decimal? V12 { get; set; }
        public decimal? V13 { get; set; }
        public decimal? V14 { get; set; }
        public decimal? V15 { get; set; }
        public decimal? V16 { get; set; }
        public decimal? V17 { get; set; }
        public decimal? V18 { get; set; }
        public decimal? V19 { get; set; }
        public decimal? V20 { get; set; }
        public decimal? V21 { get; set; }
        public decimal? V22 { get; set; }
        public decimal? V23 { get; set; }
        public decimal? V24 { get; set; }
        public decimal? V25 { get; set; }
        public decimal? V26 { get; set; }
        public decimal? V27 { get; set; }
        public decimal? V28 { get; set; }
        public decimal? V29 { get; set; }
        public decimal? V30 { get; set; }
        public decimal? V31 { get; set; }
        public decimal? V32 { get; set; }
        public decimal? V33 { get; set; }
        public decimal? V34 { get; set; }
        public decimal? V35 { get; set; }
        public decimal? V36 { get; set; }
        public decimal? V37 { get; set; }
        public decimal? V38 { get; set; }
        public decimal? V39 { get; set; }
        public decimal? V40 { get; set; }
        public decimal? V41 { get; set; }
        public decimal? V42 { get; set; }
        public decimal? V43 { get; set; }
        public decimal? V44 { get; set; }
        public decimal? V45 { get; set; }
        public decimal? V46 { get; set; }
        public decimal? V47 { get; set; }
        public decimal? V48 { get; set; }
    }
    #endregion

}