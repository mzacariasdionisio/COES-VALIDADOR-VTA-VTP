using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.Titularidad
{
    public class ConstantesTitularidad
    {
        public const int TipoMigrDuplicidad = 1;
        public const int TipoMigrInstalacionNoCorresponden = 2;
        public const int TipoMigrCambioRazonSocial = 3;
        public const int TipoMigrFusion = 4;
        public const int TipoMigrTransferenciaEquipos = 5;

        public const int FlagAnulacion = 1;
        public const int EliminadoLogicoSi = 1;
        public const int EliminadoLogicoNo = 0;

        public const int TamanioSubLista = 500;

        public const int MaxIteracionRecursivo = 7;

        //Estados de la relación entre el Equipo, Punto o grupo y la empresa en una fecha especifica
        public const string EstadoRelEmpFechaInicio = "I";
        public const string EstadoRelEmpFechaFin = "F";

        public const int ModcodiTitularidad = 24;

        //Tipo de Evento del log, 1: Inicio, 2: fin, 3: Correcto, 4: Error
        public const int TipoEventoLogInicio = 1;
        public const int TipoEventoLogFin = 2;
        public const int TipoEventoLogCorrecto = 3;
        public const int TipoEventoLogError = 4;

        public const string TipoEventoLogInicioDesc = "INICIO";
        public const string TipoEventoLogFinDesc = "FIN";
        public const string TipoEventoLogCorrectoDesc = "CORRECTO";
        public const string TipoEventoLogErrorDesc = "ERROR";

        public const string NombreReporte = "ReporteTTIE.xlsx";
        public const string FolderReporte = "Areas\\Titularidad\\Reporte\\";
        public const string NombreHoja = "Reporte";
        public const string TituloHoja = "Transferencia de Instalaciones de las Empresas";

        public const string AplicativosSRT = "351,352,353,354,355,358,359,360,361,362,363,364,365,366,367,368,369,370,371,372,373,374,375,376,377,378,379,380,381,382,383,384,385,386,387,388,389,390,391,392,393,394,395,396,397,398,399,400,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,418,419,420,421,422,423,424,425,426,427,428,429,430,431,432,433,434,435,436,437,438,439,440,441,442,443,444,445,446,453,454,455,456,457,458,459,460,461";
        public const int FlagProcesoStrSi = 1;
        public const int FlagProcesoStrNo = 0;
        public const int FlagProcesoStr = 2;

        public const int FlagActivo = 1;
        public const string TipoQueryCodigoUnico = "C";
        public const string TipoQueryCodigoRowid = "R";

        //Permiso para usuario DTI
        public const int AreacodiDTI = 1;
    }

    public class CodigosParametro
    {
        public const string SesionFileName = "SesionFileName";
        public const string ExtensionFileUploadTransferencia = "xlsx";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string RutaCargaFile = "RutaCargaFile";

        public const int Migracodi = 1;
        public const int EmpresaOrigen = 2;
        public const int EmpresaDestino = 3;
        public const int Equicodi = 4;
        public const int Mqxtopcodi = 5;
        public const int Ptomedicodi = 6;
        public const int Grupocodi = 7;
        public const int Fechacorte = 8;
        public const int Tmopercodi = 9;
        public const int Usuariocreacion = 10;
        public const int Anulacion = 11;
        public const int Migradescripcion = 24;
        public const int Fechainicial = 25;
    }

    public class TTIEDetalleAdicional
    {
        public string Titulo { get; set; } = string.Empty;
        public string CampoDesc1 { get; set; } = string.Empty;
        public string CampoDesc2 { get; set; } = string.Empty;
        public string CampoEstado { get; set; } = string.Empty;
        public List<SiHisempentidadDetDTO> ListaDetalle { get; set; } = new List<SiHisempentidadDetDTO>();
    }

    public class TTIEParametro
    {
        public int MigraCodi { get; set; }
        public int TipoOperacion { get; set; }
        public int IdEmpresaOrigen { get; set; }
        public int IdEmpresaDestino { get; set; }
        public int? Migraflagstr { get; set; }
        public string User { get; set; }
        public string Migradescripcion { get; set; }
        public List<int> Ptos { get; set; }
        public List<int> Equips { get; set; }
        public List<int> Grups { get; set; }
        public DateTime Feccorte { get; set; }
        public DateTime FeccorteSTR { get; set; }
        public int FlagAnulacion { get; set; } = 0;

        public List<TTIEEntidad> ListaEqFechaInicial { get; set; } = new List<TTIEEntidad>();
        public List<TTIEEntidad> ListaPtoFechaInicial { get; set; } = new List<TTIEEntidad>();
        public List<TTIEEntidad> ListaGrupoFechaInicial { get; set; } = new List<TTIEEntidad>();
    }

    public class TTIEEntidad
    {
        public int Codigo { get; set; }
        public DateTime FechaCorte { get; set; }
        public DateTime FechaInicial { get; set; }
    }
}