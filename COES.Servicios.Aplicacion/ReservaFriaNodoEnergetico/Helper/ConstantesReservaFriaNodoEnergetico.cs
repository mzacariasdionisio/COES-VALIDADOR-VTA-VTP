using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico.Helper
{
    public class ConstantesReservaFriaNodoEnergetico
    {
        public const int ModuloReservaFria = 1;
        public const int ModuloNodoEnergetico = 2;

        //constante genéricas
        public const int Todos = 0;
        public const string Vigente = "N"; //(eliminado:N);

        //tipo de proceso
        public const string ProcesoManual = "M";

        //Conceptos de Reserva Fria
        public const int RfDemoraEnElArranque = 1;
        //public const int RFHorasMantenimientoProgramadoCorrectivoEjecutado = 2;
        public const int RfHorasMantenimientoProgramadoEjecutado = 2;
        public const int RfHorasMantenimientoCorrectivoEjecutado = 3;
        public const int RfHorasEnergiaDejadaDeEntregar = 4;
        public const int RfEnergiaDejadaDeEntregar = 5;

        //Conceptos de Nodo Energético
        public const int NeHorasIndispTotalFortuita = 10;
        public const int NeHorasIndispTotalProgramada = 11;

        public const int NeHorasIndispParcialFortuita = 12;
        public const int NeHorasIndispParcialProgramada = 13;
        public const int NeEdeIndispTotalFortuita = 14;
        public const int NeEdeIndispTotalProgramada = 15;
        public const int NeEdeIndispParcialFortuita = 16;
        public const int NeEdeIndispParcialProgramada = 17;
        public const int NeSobrecostoIndispTotalFortuita = 18;
        public const int NeSobrecostoIndispParcialFortuita = 19;

        //mediciones
        //public const int OrigenLecturaMedidores = 1;
        public const int TipoinfocodiPotenciaActiva = 1;
        public const int TipoinfocodiEnergiaActiva = 3;
        //tiempo de arranque
        public const int ConcepcodiTiempoDeArranque = 121;
        //velocidad de toma de carga
        public const int ConcepcodiVelocidadTomaCarga = 115;
        //velocidad de reducción de carga
        public const int ConcepcodiVelocidadReduccionCarga = 120;
        //codigo de potencia efectiva
        public const int ConcepcodiPotenciaEfectiva = 14;

        //tipo de evento Falla
        public const int CausaevencodiFallaEquipo = 5;

        //código Subcausacodi Demora en el arranque
        public const int SubcausacodiDemoraArranque = 119; //NUEVO

        //código Subcausacodi Declaro disponible
        public const int SubcausacodiDeclaroDisponible = 120; //NUEVO

        //codigo de mantenimiento programado y ejecutado (de reserva fría) - HMPE
        public const int ConceptoMantoProgEjecutado = 2;

        //codigo de mantenimiento correctivo y ejecutado (de reserva fría) - HMCE
        public const int ConceptoMantoCorrEjecutado = 3;

        //codigo de mantenimiento ejecutado
        public const int EvenclasecodiEjecutado = 1;

        //codigo de mantenimiento programado
        public const int EvenclasecodiProgramadoDiario = 2;

        //Parámetros
        //codigo de Tolerancia
        public const int Rpf = 1;
        public const int ToleranciaReservaFria = 2;
        public const int ToleranciaReservaNodoEnergetico = 3;

        //código de ratio Potencia Calculada Adjudicada
        public const int RatioReservaFriaPcalculadaAdjudicada = 4;

        //código de Origen Lectura
        public const int OrigenLecturaMedidores = 1;
        //código de Origen Lectura 
        public const int OrigenLecturaDespacho = 2;
        //código de Origen Lectura Stock
        public const int OrigenLecturaStock = 21;
        //código de Origen Lectura PDO simulado
        public const int OrigenLecturaNodoEnergetico = 25; //OrigenLecturaPdoSimulado = 24;
        //código de Origen Lectura RDO simulado
        public const int OrigenLecturaReservaFria = 26; //OrigenLecturaRdoSimulado = 25;
        //código de Origen Nodo Energetico - Energia dejada de entregar
        //public const int OrigenLecturaNeEde96 = 26;

        public const int OrigenLecturaCombProgramado = 13;

        //Tipo punto medición stock
        public const int TptomedicodiStock = 25;

        public const int TptomedicodiMedidaElectrica = 15;

        //Código de lectura de stock
        public const int LectcodiStock = 77;

        public const int LectcodiEjecutado = 6;
        public const int LectcodiProgramado = 4;
        public const int LectcodiReprograma = 5;
        public const int LectcodiStockProgramado = 50; //galones

        //Código de lectura de PDO Simulado
        public const int Lectcodi48PdoSimulado = 86;
        //Código de lectura de RDO Simulado
        public const int Lectcodi48RdoSimulado = 87;
        //Código de lectura de EDE Indisponibilidad Total Fortuita
        public const int Lectcodi96EdeNodoIndispTotalFort = 88;
        //Código de lectura de EDE Indisponibilidad Total Programada
        public const int Lectcodi96EdeNodoIndispTotalProg = 89;
        //Código de lectura de EDE Indisponibilidad Parcial Fortuita
        public const int Lectcodi96EdeNodoIndispParcFort = 90;
        //Código de lectura de EDE Indisponibilidad Parcial Programada
        public const int Lectcodi96EdeNodoIndispParcProg = 91;

        //Código de lectura de EDE Reserva Fria
        public const int Lectcodi96EdeReservaFria = 92;

        //Código de tipo de información de volumen
        public const int TipoinfocodiVolumen = 12;

        //código de potencia adjudicada
        public const int PotenciaAdjudicada = 278;

        //código de Rendimiento
        public const int ConcepcodiRendimiento = 190;

        //código de tipo información: Biodiesel (galones)
        public const int TipoinfocodiBiodieselGalon = 43;

        //código de Subcausa de Restricciones Operativas
        public const int SubcausacodiRestriccOperat = 205;

        //código de combustible B5
        public const int CombcodiB5 = 14;
        public const int TipoinfocodiGalon = 15;

        //otros parametros de configuracion de modulo
        public const int MinutoSincronizacion = 30;


        // Nota automatica
        public const string NotaRpfCero = "RPF es cero";
        public const string NotaHippcaso1 = "HIPP Caso1";
        public const string NotaHippcaso2 = "HIPP Caso2";
        public const string NotaHipfcaso1 = "HIPF Caso1";
        public const string NotaHipfcaso2 = "HIPF Caso2";
        public const string NotaHoraParalelo = "Error. No hay potencia de medidores desde Hora de Paralelo a bloque 15 min. siguiente";
        public const string NotaNoHayDeclaracDispo = "No hay declaración de disponibilidad";
        public const string NotaNoHayHoraOperacion = "No existe hora operación.";

        public const string NotaNoHayVolumenFinalDiarioCombustible = "No se registro volumen final diario de combustible.";
        public const string NotaNoHayCombustibleProgramado = "No se registro combustible programado.";


        //otras constantes
        public const string ParametroTodos = "0";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaYMD = "yyyy-MM-dd";
        public const string FormatoFecha = "dd/MM/yyyy";

        //constante para conocer si es granularidad 15 o 30 minutos
        public const int Factor15 = 15;
        public const int Factor30 = 30;

        //Factor de conversion galón a m3
        public const decimal RatioM3aGalon = (decimal)264.17;

    }
}
