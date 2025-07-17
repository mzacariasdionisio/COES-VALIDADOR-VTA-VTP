using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Mediciones.Helper
{
    /// <summary>
    /// Constantes para los puntos de medición
    /// </summary>
    public class ConstantesMedicion
    {
        public const int OrigenLecturaGeneracionRER = 15;
        public const int LecturaProgDiaraRER = 61;
        public const int LecturaProgSemanalRER = 62;
        public const int TipoInformacionRER = 1;
        public const string TextoNoEnvio = "No envió";
        public const string ViaWebService = "Vía Web Service";
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string DentroDePlazo = "Dentro del plazo";
        public const string FueraDePlazo = "Fuera del plazo";
        public const string DatosProgramaDiario = "Se deben enviar datos de un día.";
        public const string DatosProgramaSemanal = "Se deben enviar datos de siete días.";
        public const string FechaDiarioGeneracionRER = "La fecha enviada y la fecha de los datos no coinciden.";
        public const string FechaSemanaGeneracionRER = "LaS fechaS de los datos no corresponden a las fechas de la semana.";
        public const string IndicadorMaximo = "H";
        public const string IndicadorMinimo = "L";
        public const string CaracterH = "H";

        public const string KeyTodos = "_TODOS";
        public const int TODOS = 0;
        public const int COES = -1;
        public const int NOCOES = 10;

        //// Constante entidad ENVIO
        public const byte ENVIO_ENVIADO = 1;
        public const byte ENVIO_PROCESADO = 2;
        public const byte ENVIO_APROBADO = 3;
        public const byte ENVIO_RECHAZADO = 4;
        public const byte ENVIO_FUERAPLAZO = 5;
        public const decimal DELTACAMBIO = 0.0003M;

        //Destino Potencia
        public const int GeneracionPeru = 0;
        public const int ExportacionEcuador = 1;
        public const int ImportacionEcuador = 2;

        //Maxima Demanda
        public const int IdOrigenLecturaMedidorGeneracion = 1;
        public const int IdLecturaMedidorGeneracion = 1;
        public const int IdTipoInfoPotenciaActiva = 1;
        public const int IdTipoInfoPotenciaReactiva = 2;
        public const int IdTipoInfoEnergiaActiva = 3;
        public const int IdTipoInfoEnergiaReactiva = 4;
        public const int IdTipoInfoFrecuencia = 6;
        public const int IdTipoPtomedicodiMedidaElectrica = 15;
        public const int IdTipoPtomedicodiCalorUtilGeneracion = 49;
        public const int IdTipoPtomedicodiCalorUtilRecibidoProceso = 50;
        public const int IdFamiliaSSAA = 40;
        public const int IdTipogrupoTodos = 0;
        public const int IdTipogrupoCOES = 1;
        public const int IdTipogrupoNoIntegrante = 10;
        public const int IdTipogrupoRER = 3;
        public const string GrupoNoIntegrante = "N";
        public const string TipoGenerrer = "S";
        public const int IdEmpresaTodos = -1;
        public const int IdTptomedicodiTodos = -1;
        public const string ListaGeneracionTodos = "1,2,3,4,5";
        public const int IdTipoGeneracionTodos = -1;
        public const int IdTipoGeneracionHidrolectrica = 1;
        public const int IdTipoGeneracionTermoelectrica = 2;
        public const int IdTipoGeneracionSolar = 3;
        public const int IdTipoGeneracionEolica = 4;
        public const int IdTipoGeneracionNuclear = 5;

        public const int IdTipoRecursoTodos = -1;

        //Parámetro Consulta Medidores de Generación
        public const int TipoPotenciaActiva = 1;
        public const int TipoPotenciaReactiva = 5;
        public const int TipoPotenciaReactivaCapacitiva = 2;
        public const int TipoPotenciaReactivaInductiva = 4;
        public const int TipoPotenciaActivaSSAA = 3;

        public const int TipoCuadroFuenteEnergia = 1;
        public const int TipoCuadroTipoGeneracion = 2;
        public const int TipoCuadroUnidades = 3;

        //Informes SGI
        public const int TipoDatoDespachoEjec = 1;
        public const int TipoDatoDespachoProg = 2;
        public const int TipoDatoDespachoEjecAnexoA = 3;
    }

    public class ConstantesRepMaxDemanda
    {
        //Constantes para cálculo
        public const int TipoMDNormativa = -1;
        public const short TipoPeriodoBloqueMaxima = 0; //Bloque HORA PUNTA - bloque maxima
        public const short TipoPeriodoBloqueMinima = 3; //Bloque mínima: despues de máxima y antes de media
        public const short TipoPeriodoBloqueMedia = 4; //Bloque media: despues de minima y antes de maxima
        public const int TipoMaximaTodoDia = 10;
        public const int TipoMinimaTodoDia = 11;
        public const int TipoHoraPunta = 12; //Bloque HORA PUNTA
        public const int TipoFueraHoraPunta = 13; //Bloque HORA FUERA PUNTA

        //constantes para Combo seleccionable de periodos
        public const int CodigoHorarioMD = 0;
        public const int CodigoHorarioMaxima = 1;
        public const int CodigoHorarioMedia = 2;
        public const int CodigoHorarioMinima = 3;
        public const int CodigoHorarioHP = 4;
        public const int CodigoHorarioHFP = 5;
    }

    public class ConstantesIndicador
    {
        public const string GpsOficial = "S";//"N,S";

        public const string VariacionSubita = "U";
        public const string VariacionSostenida = "O";

        public const string DatoFechaHora = "FECHAHORA";
        public const string DatoIndicValor = "INDICVALOR";

        public const string UnidMedPer = "%";
        public const string UnidMedHz = "Hz";

        public const string FrecuenciaMinima = "m";
        public const string FrecuenciaMaxima = "M";
    }
}
