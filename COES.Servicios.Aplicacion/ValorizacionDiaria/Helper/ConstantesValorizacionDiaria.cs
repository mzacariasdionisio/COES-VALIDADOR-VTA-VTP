using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ValorizacionDiaria.Helper
{
    public class ConstantesValorizacionDiaria
    {
        public const string PlantillaExcelMontoPorEnergia = "PlantillaReporteMontoPorEnergia.xlsx";
        public const string NombreReporteMontoPorEnergia = "RptMontoPorEnergia.xlsx";
        public const string PlantillaExcelMontoPoCapacidad = "PlantillaReporteMontoPorCapacidad.xlsx";
        public const string NombreReporteMontoPorCapacidad = "RptMontoPorCapacidad.xlsx";
        public const string PlantillaExcelMontoPorPeaje = "PlantillaReporteMontoPorPeaje.xlsx";
        public const string NombreReporteMontoPorPeaje = "RptMontoPorPeaje.xlsx";
        public const string PlantillaExcelMontoSCeIO = "PlantillaReporteMontoSCeIO.xlsx";
        public const string NombreReporteMontoSCeIO = "RptMontoSCeIO.xlsx";
        public const string PlantillaExcelMontoPorExceso = "PlantillaReporteMontoPorExceso.xlsx";
        public const string NombreReporteMontoPorExceso = "RptMontoPorExceso.xlsx";
        public const string PlantillaExcelValorizacionDiaria = "PlantillaReporteValorizacionDiaria.xlsx";
        public const string NombreReporteValorizacionDiaria = "RptValorizacionDiaria.xlsx";
        public const string PlantillaExcelReporteInformacionPrevista = "PlantillaExcelReporteInformacionPrevista.xlsx";
        public const string NombreReporteInformacionPrevista = "RptInformacionPrevista.xlsx";
        public const string FolderReporte = "Areas\\ValorizacionDiaria\\Reporte\\";
        public const int FilaExcelData = 16;
        public const int ResolucionCuartoHora = 15;
        public const int EstacionHidrologica = 43;

        public const int GRUPOCODI = 2500; // YA NO SE USA
        public const int FRECTOTAL = 962;
        public const int COSTOSOTROSEQUIPOS = 961;
        public const int COSTOFUERADEBANDA = 960;
        public const char EXITO = 'E';
        public const char FALLIDO = 'F';
        public const char MANUAL = 'M';
        public const char AUTOMATICO = 'A';
        public const string USUARIO_PROCESO = "AUTOMATICO";
        public const string PROCESO_FALLIDO = "Proceso de Valorización Diaria Fallida, revisar log de eventos del aplicativo.";
        public const char ACTIVO = 'A';
        public const char INACTIVO = 'I';

        public const int GrupoCodiParametro = 0;

        // Calificaciones
        public const int CalificacionRSF = 320;
        public const int CalificacionPotenciaEnergia = 101;
        public const int CalificacionPruebasAleatrorias = 114;
        public const int CalificacionMinimaCarga = 102;
        public const int CalificacionPorManiobra = 121;
        public const int CalificacionPorSeguridad = 105;
        public const int CalificacionPorRT = 103;

        // Tipos Energia Prevista a retirar por Formato
        public const int EPRFORMATODIARIO = 101;
        public const int EPRFORMATOTRIMESTRAL = 102;

        // Tipo Energia Prevista a retirar por lectcodi
        public const int EPRLECTCODIDIARIO = 223;
        public const int EPRLECTCODITRIMESTRAL = 224;


        #region Fit Valorizacion-Parametro
        //5.2_Monto por Capacidad
        //margen de reserva 

        public const int ConceptoCodiMargenReserva = 541;

        //5.4_Monto por Servicio
        //factores de reparto

        public const int ConceptoCodiFactorRepartoLunes = 542;
        public const int ConceptoCodiFactorRepartoMartes = 543;
        public const int ConceptoCodiFactorRepartoMiercoles = 544;
        public const int ConceptoCodiFactorRepartoJueves = 545;
        public const int ConceptoCodiFactorRepartoViernes = 546;
        public const int ConceptoCodiFactorRepartoSabado = 547;
        public const int ConceptoCodiFactorRepartoDomingo = 548;

        //5.4_Monto por Servicio
        //porcentaje de perdidas 
        public const int ConceptoCodiPorcentajePerdida = 549;

        //5.4_Monto por Servicio
        //Costo Oportunidad
        public const int ConceptoCodiCostoOportunidad = 550;

        //5.5_Monto por Exceso
        //cargos por consumo
        public const int ConceptoCodiCargosConsumo = 551;

        //5.5_Monto por Exceso
        //AporteAd
        public const int ConceptoCodiCostoFueraBanda = 552;
        public const int ConceptoCodiCostoOtrosEquipos = 553;
        public const int ConceptoCodiFRECTotal = 554;
        #endregion

        public const int CodigoProcesoValorizacion = 20;
        
    }
}
