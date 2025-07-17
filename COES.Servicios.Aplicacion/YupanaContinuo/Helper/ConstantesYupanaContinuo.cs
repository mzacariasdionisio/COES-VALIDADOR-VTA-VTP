using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.YupanaContinuo.Helper
{
    public class ConstantesYupanaContinuo
    {
        //Condiciones termicas
        public const string ParamEmpresaSeleccione = "-3";
        public const string ParamCentralSeleccione = "-3";
        public const int AlertaXSI = 1;
        public const int IdTipoTermica = 5;
        public const string ParamModoSeleccione = "-3";

        public const string CreadoModificadoUsuario = "U";
        public const string CreadoSistemaHoraOp = "S";
        public const string CreadoSistemaYupana = "Y";
        public const string SrestcodiUniForzada = "22";
        public const string CaracterH = "H";
        public const int ParametroDefecto = -1;
        public const int ParametroDefecto2 = -2;
        public const string SubcausacodiPotenciaOEnergia = "101";

        public const string FormatoFechaHora = "dd/MM/yyyy HH";

        // Constantes Compromiso hidráulico
        public const int Todos = -1;

        public const int FormatoSinCompromiso = 128;
        public const int FormatoConCompromiso = 127;
        public const int LecturaSinCompromiso = 243;
        public const int LecturaConCompromiso = 242;

        public const int PestaniaSinCompromiso = 0;
        public const int PestaniaConCompromiso = 1;
        public const int PestaniaAmbos = 2;

        // Constantes Arbol Simulacion
        public const string DirectorioGAMS = "DirectorioGAMS";
        public const string PathYupanaContinuo = "PathYupanaContinuo";
        public const string NumCPUYupanaContinuo = "NumCPUYupanaContinuo";
        public const string NumGamsParaleloYupanaContinuo = "NumGamsParaleloYupanaContinuo";
        public const string MinutoMaxGamsYupanaContinuo = "MinutoMaxGamsYupanaContinuo";
        public const string UsarWebServiceYupanaContinuo = "UsarWebServiceYupanaContinuo";
        public const string ActualizarInsumoYupanaContinuo = "ActualizarInsumoYupanaContinuo";

        public const int ConcepcodiCO = 1;
        public const int ConcepcodiCR = 2;
        public const int ConcepcodiCMgMinPerMin = 3;
        public const int ConcepcodiCMgMinPerMed = 4;
        public const int ConcepcodiCMgMinPerMax = 5;
        public const int ConcepcodiCMgMaxPerMin = 6;
        public const int ConcepcodiCMgMaxPerMed = 7;
        public const int ConcepcodiCMgMaxPerMax = 8;
        public const int ConcepcodiCMgPromPerMin = 9;
        public const int ConcepcodiCMgPromPerMed = 10;
        public const int ConcepcodiCMgPromPerMax = 11;

        public const int SimulacionAutomatica = 0;
        public const int SimulacionManual = 1;

        public const string EstadoArbolNoIniciado = "NI";
        public const string EstadoArbolEnEjecucion = "EE";

        public const int DefaultNumGamsParalelo = 2;
        public const int DefaultNumCPUSimulacion = 3;
        public const int DefaultMinutoMaxGams = 15;

        public const int SegundosEsperaAProcesar = 20;

        public const int TipoSegundo = 1;
        public const int TipoMinuto = 2;

        //Mejoras RER
        public const int TopologiaBase = 0;
        public const string MensajeNoExisteTopologia = "No se encontró Escenario yupana (PDO/RDO) para la fecha de simulación";

        public const string TipoConfiguracionBase = "B";
        public const string TipoConfiguracionDiario = "D";

        public const int TipoConfiguracionCaudal = 1;
        public const int TipoConfiguracionRer = 2;

        public const int ConfiguracionBaseCaudal = 1;
        public const int ConfiguracionBaseRer = 2;

        public const int OrigenExtranetHidrologia = 1;
        public const int OrigenExtranetIDCC = 2;
        public const int OrigenYupanaAporte = 3;
        public const int OrigenYupanaNoConvencional = 4;
        public const int OrigenEdicionManual = 5;

        //Proceso automatico
        public const string PrcsmetodoSimularArbolYupanaContinuo = "SimularArbolYupanaContinuo";
    }

    #region Clases de Compromiso hidráulico
    public class CompromisoHidraulico
    {
        public int Emprcodi { get; set; }
        public int PtoMedicion { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public DateTime Fecha { get; set; }
        public int Hx { get; set; }
        public int Flag { get; set; }
    }
    #endregion

    #region Clases Arbol de Simulación

    public class EstructuraNodo : ICloneable
    {
        public string Id { get; set; }
        public string NumeroNodo { get; set; }

        public int Identificador { get; set; }
        public string Estado { get; set; } //C: creado, E: ejecutado, N: no ejecutado
        public string Convergencia { get; set; }  //C: converge. D: diverge.     
        public string Offset { get; set; }
        public string Info { get; set; }

        public string MensajeError { get; set; }
        public string MensajeActualizacion { get; set; }
        public string ColorActualizacion { get; set; }

        public string BorderColor { get; set; }
        public int Height { get; set; }

        public string HtmlResultado { get; set; }
        public string HtmlGuardarEscenario { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    #endregion

    /// <summary>
    /// Clase que tiene los límites mínimos y máximos de los equipos RER
    /// </summary>
    public class RDOLimiteRer
    {
        //datos de empresa
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }

        //datos de punto de medición
        public int Ptomedicodi { get; set; }
        public string Ptomedidesc { get; set; }
        public int Tipoinfocodi { get; set; }

        //datos de equipo
        public int Famcodi { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public decimal? Pmin { get; set; }
        public decimal? Pmax { get; set; }
    }

    /// <summary>
    /// Clase que tiene los limites del punto de medición Caudal hidrologico
    /// </summary>
    public class RDOLimiteHidrologiaCaudal
    {
        //datos de empresa
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }

        //datos de punto de medición
        public int Ptomedicodi { get; set; }
        public string Ptomedidesc { get; set; }
        public int Tipoinfocodi { get; set; }
        public string Tipoptomedinomb { get; set; }

        //datos de equipo
        public int Famcodi { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }


        //Valores para alertas
        public decimal[] ArrayPromedio { get; set; } = new decimal[24];
        public decimal[] ArrayDesvEstandar { get; set; } = new decimal[24];
        public decimal[] ArrayMinimo { get; set; } = new decimal[24];
        public decimal[] ArrayMaximo { get; set; } = new decimal[24];
    }

    /// <summary>
    /// Insumos que actualizan los .dat de los nodos
    /// </summary>
    public class InsumoYupanaContinuo
    {
        public List<CpMedicion48DTO> ListaCondTermicas { get; set; }
        public List<CpMedicion48DTO> ListaAportesSC { get; set; }
        public List<CpMedicion48DTO> ListaAportesCC { get; set; }
        public List<CpMedicion48DTO> ListaAportesCCSC { get; set; }
        public List<CpMedicion48DTO> ListaProyRer { get; set; }
    }
}
