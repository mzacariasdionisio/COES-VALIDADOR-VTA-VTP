using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.Mediciones.Helper
{
    public class ConstantesMedidores
    {
        public const int IdOrigenMedidorGeneracion = 1;
        public const int IdLecturaMedidorGeneracion = 1;
        public const int IdFormatoMedidorGeneracion = 1;

        public const int IdHojaCargaCentralPotActiva = 18;
        public const int IdHojaCargaCentralPotReactiva = 19;
        public const int IdHojaCargaServAuxPotReactiva = 20;

        public const int IdFormatoCargaCentralPotActiva = 80;
        public const int IdFormatoCargaCentralPotReactiva = 81;
        public const int IdFormatoCargaServAuxPotActiva = 82;

        public const int IdTipoPtomedicionCaudalTurbinado = 1;
        public const int IdUsuarioCOES = 1;

        public const int IdFormatoTurbinadoEjecutadoDiario = 38;
        public const int IdFormatoDespachoEjecutadoDiario = 62;

        //Fuentes Informacion
        public const int IdFuenteMedidores = 1;
        public const string DescFuenteMedidores = "MEDIDORES";
        public const string DescNombreMedidores = "Medidores";
        public const string DescLeyendaMedidores = "MW";
        public const string DescTituloMedidores = "MW";

        public const int IdFuenteDatosScada = 2;
        public const string DescFuenteDatosScada = "SCADA";
        public const string DescNombreDatosScada = "Scada";
        public const string DescLeyendaDatosScada = "SC";
        public const string DescTituloDatosScada = "SC";

        public const int IdFuenteDespachoDiario = 3;
        public const string DescFuenteDespachoDiario = "DESPACHO DIARIO";
        public const string DescNombreDespachoDiario = "Despacho Diario";
        public const string DescLeyendaDespachoDiario = "MW despacho";
        public const string DescTituloDespachoDiario = "Despacho";

        public const int IdFuenteCaudalTurbinado = 4;
        public const string DescFuenteCaudalTurbinado = "CAUDAL TURBINADO";
        public const string DescNombreCaudalTurbinado = "Q Turbinados";
        public const string DescLeyendaCaudalTurbinado = "Qturb";
        public const string DescTituloCaudalTurbinado = "Q = m3/s";

        public const int IdFuenteRPF = 5;
        public const string DescFuenteRPF = "RPF";
        public const string DescNombreRPF = "RPF";
        public const string DescLeyendaRPF = "rpf";
        public const string DescTituloRPF = "rpf";

        //Tipo periodo
        public const int PeriodoTodos = 1;
        public const int PeriodoHp = 2;
        public const int PeriodoHfp = 3;
        public const string DescPeriodoTodos = "TODOS";
        public const string DescPeriodoHp = "HP";
        public const string DescPeriodoHfp = "HFP";

        //Tipo Dato
        public const int DatoPromedio = 1;
        public const int DatoHorario = 2;

        //Tipo Usuario
        public const int UsuarioCOES = 1;
        public const int UsuarioAgentes = 2;

        ///PR15
        public const int TptoMedicodiTodos = -2;
        public const int TptoMedicodiDefault = -1;
        public const int TptoMedicodiInductiva = 58;
        public const int TptoMedicodiCapacitiva = 59;
        public const int TptoMedicodiMedElectrica = 15;

        //Tipo de Centrales
        public const int CentralHidraulica = 4;
        public const int CentralTermica = 5;
        public const int CentralEolica = 39;
        public const int CentralSolar = 37;

        //Tipo Grafico
        public const int GraficoIgualMedida = 1;
        public const int GraficoDiferenteMedida = 2;
    }

    public class ConstantesTipoInformacion
    {
        //Analogicos
        public const int TipoinfoMW = 1;
        public const int TipoinfoMVar = 2;
        public const int TipoinfoKV = 5;
        public const int TipoinfoHz = 6;
        public const int TipoinfoAmperio = 9;
        public const int TipoinfoCaudal = 11;
        public const int TipoinfoGalon = 15;
        public const int TipoinfoHoras = 23;
        public const int TipoinfoVelocidad = 54;
        public const int TipoinfoRpm = 75;

        //Digital
        public const int TipoinfoAlg = 68;
        public const int TipoinfoInt = 69;
        public const int TipoinfoSec = 70;
        public const int TipoinfoAlg1 =71;
        public const int TipoinfoSec1 = 72;
        public const int TipoinfoSec2 = 73;
        public const int TipoinfoSect = 74;
        public const int TipoinfoAl2 = 76;
        public const int TipoinfoSect1 = 77;
        public const int TipoinfoSecba = 78;
        public const int TipoinfoSecbb = 79;
        public const int TipoinfoSecl = 81;
        public const int TipoinfoSect2 = 82;
        public const int TipoinfoSect3 = 83;
        public const int TipoinfoToma = 84;
        public const int TipoinfoInt1 = 85;
        public const int TipoinfoP1 = 86;
        public const int TipoinfoSecb1 = 87;
        public const int TipoinfoAlg2 = 88;
    }

    public class ConstantesTipoPuntoMedicion
    {
        public const int TptomedicodiMedidaElectrica = 15;
        public const int TptomedicionCalorUtilGeneracion = 49;
        public const int TptomedicodiCalorUtilRecibidoProceso = 50;
        public const int TptomedicodiInductiva = 58;
        public const int TptomedicodiCapacitiva = 59;
        public const int TptomedicodiPotActivaCogeneracion = 60;
    }
}
