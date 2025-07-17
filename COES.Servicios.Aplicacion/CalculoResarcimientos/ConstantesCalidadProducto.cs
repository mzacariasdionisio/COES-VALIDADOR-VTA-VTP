using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CalculoResarcimientos
{
    public class ConstantesCalidadProducto
    {

        public const string ValorPorDefecto = "-1";

        public const string IdPlantillaSolicitudEnvioInterrupcionesTrimestral = "174";
        public const string IdPlantillaSolicitudEnvioInterrupcionesSemestral = "175";
        public const string IdPlantillaSolicitudEnvioObservacionesAInterrupciones = "176";
        public const string IdPlantillaSolicitudEnvioRespuestasAInterrupciones = "177";
        public const string IdPlantillaSolicitudDecisionesControversia = "178";
        public const string IdPlantillaSolicitudEnvíoCompensacionesMalaCalidadProducto = "179";
        public const string IdPlantillaSolicitudEnvioInterrupcionesPendienteReportar = "320";
        public const string IdPlantillaTodosPlantilla = "174,175,176,177,178,179,320";
        public const string IdPlantillaTodosCorreo = "174,175,176,177,178";
        public const string IdTipoCorreos = "1,2,3,4,5,7";

        public readonly static string FolderRaizIntranetResarcimiento = (ConfigurationManager.AppSettings["PathIntranetAppResarcimiento"] ?? "").ToString();
        public const string RutaCarpetaTempResarcimiento = "Resarcimiento2023\\";
        public const string NombreCarpetaTempResarcimiento = "CorreosAutomaticos";

        public const string FormatoFechaFull3 = "yyyy/MM/dd HH:mm:ss";
        public const int ModuloCalculoResarcimiento = 48;
        public const int IdEmpresaNoExistente = -9;

        public const int VerCorreo = 1;
        public const int NuevoCorreo = 2;

        public const int VariableAsunto = 0;
        public const int VariableContenido = 1;
        public const int VariableCC = 2;
        public const int VariablePara = 3;

        //variables Solicitud de envío de interrupciones 
        public const string ValPeriodo = "{Periodo}"; //Segundo Semestre 2022
        public const string DscPeriodo = "Periodo";
        public const string ValMesesPeriodo = "{MesesPeriodo}"; //julio a setiembre
        public const string DscMesesPeriodo = "Meses Periodo";
        public const string ValAnioPeriodo = "{AnioPeriodo}"; //2022
        public const string DscAnioPeriodo = "Anio Periodo";
        public const string ValNumPeriodo = "{NumPeriodo}"; //02_SEGUNDO SEMESTRE
        public const string DscNumPeriodo = "Numero Periodo";
        public const string ValMesFinalPeriodo = "{MesFinalPeriodo}"; // Setiembre
        public const string DscMesFinalPeriodo = "Mes Final Periodo";
        public const string ValFecFinE1Periodo = "{FechaEtapa01}"; // X de octubre del 2022
        public const string DscFecFinE1Periodo = "Fecha Final Etapa 1";

        //variables Solicitud de envío de observaciones a las interrupciones
        public const string ValResponsable = "{NombreResponsable}"; //Aceros Arequipa
        public const string DscResponsable = "Nombre Responsable";
        public const string ValNombrePeriodo = "{NombrePeriodo}"; //2022-S2
        public const string DscNombrePeriodo = "Nombre Periodo";        
        public const string ValFecFinE2Periodo = "{FechaEtapa02}"; //X de octubre del 2022
        public const string DscFecFinE2Periodo = "Fecha Final Etapa 2";

        //variables Solicitud de envío de respuestas a las observaciones        
        public const string ValFecFinE3Periodo = "{FechaEtapa03}";
        public const string DscFecFinE3Periodo = "Fecha Final Etapa 3"; //X de octubre del 2022

        //variables Solicitud de envío de decisiones de controversias      
        public const string ValFecFinE6Periodo = "{FechaEtapa06}";
        public const string DscFecFinE6Periodo = "Fecha Final Etapa 6"; //X de octubre del 2022

        //variables Solicitud de envío de compensaciones por mala calidad de producto
        public const string ValMesAnioEvento = "{MesAnioEvento}"; //agosto del 2020
        public const string DscMesAnioEvento = "Mes Anio del Evento";
        public const string ValPuntoEntregaEvento = "{PuntoEntregaEvento}"; //Huallanca
        public const string DscPuntoEntregaEvento = "Punto Entrega del Evento";

        public const string ArchivoEventosProducto = "EventosCalidadProducto.xlsx";

        public const int IdParametroFCU = 37; 
    }

    public class CorreoPeriodo
    {
        public int CorreoId { get; set; }
        public int PeriodoId { get; set; }
        public int TipoCorreo { get; set; }
        public int ResponsableId { get; set; }
    }

    public class VariableCorreo
    {
        public string Valor { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string ValorConDato { get; set; } = string.Empty;
    }

    public class EmpresaCorreo
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; } = string.Empty;
        public string LstCorreos { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string FechaModificacionDesc { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class EmpresaCorreoErrorExcel
    {
        public int NumeroFilaExcel { get; set; }

        public string CampoExcel { get; set; }
        public string ValorCeldaExcel { get; set; }
        public string MensajeValidacion { get; set; }
    }

    public class ReporteCompensacionMalaCalidad
    {
        public Dictionary<int,string> ListaResponsables { get; set; }
        public Dictionary<int, string> ListaSuministradores { get; set; }
        public List<RegistroReporteCompensacionMalaCalidad> ListaDatos { get; set; } 
    }
    public class RegistroReporteCompensacionMalaCalidad
    {
        public int ResponsableId { get; set; }
        public int SuministradorId { get; set; }
        public decimal? ValorResarcimiento { get; set; }
    }
}
