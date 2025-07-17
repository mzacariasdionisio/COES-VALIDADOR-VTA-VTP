using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Campanias
{
    public class ConstantesCampania
    {

        public const string RutaBaseCampania = "LocalDirectory";
        public const string FolderCampanias = "Campanias/";
        public const string Agregar = "agregar";
        public const string Editar = "editar";
        public const string Consultar = "consultar";
        public const string VAR_CORREO_USUARIO_ENVIO = "{CORREO_USUARIO_SOLICITUD}";
        public const string VAR_CODIGO_TRANSMISION = "{CODIGO_TRANSMISION}";
        public const string VAR_NOMBRE_TRANSMISION = "{NOMBRE_TRANSMISION}";
        public const string VAR_NOMBRE_EMPRESA = "{NOMBRE_EMPRESA}";
        public const string VAR_NOMBRE_PROYECTO = "{NOMBRE_PROYECTO}";
        public const string VAR_FECHA_ENVIO = "{FECHA_ENVIO}";
        public const string VAR_COMENTARIOS = "{COMENTARIOS}";
        public const string VAR_NOMBRE_PERIODO = "{NOMBRE_PERIODO}";
        public const string VAR_LISTA_PROYECTOS = "{LISTA_PROYECTOS}";
        public const string CaracterComa = ",";
    }
}

public class TipoProyecto
{

    public const int Generacion = 1;
    public const int Transmision = 2;
    public const int ITC = 3;
    public const int Demanda = 4;
    public const int GeneracionDistribuida = 5;
    public const int HidrogenoVerde = 6;
}

public class SubTipoProyecto
{

    public const int CentralHidroeléctrica = 10;
    public const int CentralTermoeléctrica = 11;
    public const int CentralEólica = 12;
    public const int CentralSolar = 13;
    public const int CentralBiomasa = 14;
    public const int Subestaciones = 1;
    public const int Lineas = 2;
}

public class Estado
{
    public const string Registrado = "Registrado";
    public const string Enviado = "Enviado";
    public const string Observado = "Observado";
}

public class Vigencia
{
    public const string Activado = "1";
    public const string Desactivado = "0";
}

public class CategoriaRequisito
{
    public const int CentralHidroHojaC = 7;
    public const int CentralTermoHojaC = 7;
    public const int CentralEolHojaC = 7;
    public const int CentralSolarHojaC = 7;
    public const int CentralBioHojaC = 7;
    public const int LineasHojaB = 7;
    public const int DemandaHojaA = 47;
    public const int DemandaHojaD = 32;
    public const int H2VHojaA = 41;
    public const int H2VHojaG = 42;
    public const int CcGdHojaF = 7;
    public const int SubesTrasnformPot = 33;
    public const int SubesTrasnformPotPrueba = 39;
    public const int SubesCompenReactPrueba = 40;
    public const int TransmisionCrono = 43;
}

public class CatalogoValor
{
    public const int catPropietario = 2;
    public const int catConcesionTemporal = 3;
    public const int catConcesionActual = 4;
    public const int catTipoPPL = 5;
    public const int catSE = 6;
    public const int catSerieVelocidad = 9;
    public const int catEstudioGeol = 10;
    public const int catEstudioTopo = 11;
    public const int catTipTurbina = 12;
    public const int catTipParqueEol = 13;
    public const int catTipGenerador = 14;
    public const int catNomSubEsacionSein = 15;
    public const int catPerfil = 16;
    public const int catPreFactibilidad = 17;
    public const int catFactibilidad = 18;
    public const int catEstudDef = 19;
    public const int catEia = 20;
    public const int catBacterias = 24;
    public const int catElectrolizador = 25;
    public const int catObjProyec = 26;
    public const int catHidrogeno = 27;
    public const int catTransporteH2 = 28;
    public const int catSuministro = 29;
    public const int catRecursoUsado = 30;
    public const int catTecnologia = 31;
    public const int catCombustible = 34;
    public const int catRadSolar = 35;
    public const int catSegSolar = 36;
    public const int catSistBarras = 37;
    public const int catTipoCarga = 44;
    public const int catSubEmpElectro = 46;
    public const int catObjProyecto = 52;
    public const int catPodCalInf = 54;
    public const int catPodCalSup = 55;
    public const int catCComb = 56;
    public const int catCTratComb = 57;
    public const int catCTranspComb = 58;
    public const int catCVarNComb = 59;
    public const int catCInvIni = 60;
    public const int catRendPl = 61;
    public const int catConsEspCon = 62;
    public const int catEstadoOperacion = 63;
    public const int catTituloHab = 64;
}

public class TipoReporte
{
    public const int ReporteItcDemanda = 1;
    public const int ReporteItcSistema = 2;
    public const int ReporteEmpresas = 3;
    public const int ReporteCentralesTermicas = 4;
    public const int ReporteCentralesHidroelectricas = 5;
    public const int ReporteCentralesSolares = 6;
    public const int ReporteCentralesEolicas = 7;
    public const int ReporteCentralesBiomasa = 8;
    public const int ReporteLineasTransmision = 9;
    public const int ReporteTransformadores = 10;
    public const int ReporteGeneracionDistribuida = 11;
    public const int ReporteHidrogenoVerde = 12;
    public const int ReporteCronogramaEjecucion = 13;
    public const int ReporteProyeccionDemanda = 14;
    public const string DeEmpresas = "DeEmpresas";
    public const string DeProyectos = "DeProyectos";
    public const string DePronosticoDemanda = "DePronosticoDemanda";
    public const string DePronosticos = "DePronosticos";
}







