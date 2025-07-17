namespace COES.Servicios.Aplicacion.PotenciaFirme
{
    public static class ConstantesPotenciaFirmeRemunerable
    {
        public const string ConcepcodiIngresos = "5,643,541";
        public const int ConcepcodiCR = 5;
        public const int ConcepcodiCA = 643;
        public const int ConcepcodiMR = 541;
        public const bool UsarLayoutModulo = true;
        public const string EstadoPeriodoAbierto = "A";
        public const string EstadoPeriodoCerrado = "C";

        //Cuadro 
        public const int CuadroRelacion = 1;
        public const int CuadroC8 = 2;
        public const int CuadroAUX1 = 3;
        public const int CuadroDatos = 4;
        public const int CuadroPFirmeRemunerable = 7;

        //public const int CuadroTodo = 9;

        public const int EsVersionGenerado = 0;
        public const int EsVersionValidado = 1;

        //periodos        
        public const string Abierto = "A";
        public const string Cerrado = "C";
        public const string Preliminar = "P";

        public const string Equicodissaa = "40";

        public const int ConcepcodiMargeReserva = 541;
        public const int ConcepcodiCV_PFR = 647;
        public const int PBase = 100;

        public enum Accion
        {
            Nuevo = 1,
            Editar = 2
        }

        public const int ParametroDefecto = -1;
        public const string ParametroXDefecto = "-1";

        public const string Historico = "H";
        public const string Aplicativo = "A";

        public const string Activo = "A";
        public const string Baja = "B";

        public const int RelacionGamsEquipo = 16;
        public const int Congestion = 15;

        public const int CatecodiBarras = 10;
        public enum Estado
        {
            Baja = 0,
            Activo = 1,
            Defecto = -1
        }

        public enum TipoRelacionInd
        {
            FactorK = 1,
            CRHidro = 2,
            CRTermoFortuita = 3,
            CRTermoProgramada = 4,
        }

        public const int FamcodiGeneradorTermo = 3;
        public const int FamcodiCentralTermo = 5;
        public const int FamcodiGeneradorHidro = 2;
        public const int FamcodiCentralHidro = 4;
        public const int FamcodiGeneradorSolar = 36;
        public const int FamcodiCentralSolar = 37;
        public const int FamcodiGeneradorEolico = 38;
        public const int FamcodiCentralEolico = 39;



        public enum ExcelReporteLVTP
        {
            Relacion = 1,
            C8 = 2,
            Aux1 = 3,
            Datos = 4,
            Aux2 = 7,
            ReporteLvtp = 0

        }

        public enum ExcelLVTP_OPF
        {
            Barras = 0,
            Demanda = 1,
            Generacion = 2,
            CompDinamica = 3,
            Lineas = 4,
            Trafo2 = 5,
            Trafo3 = 6,
            DiagramaUnifilar = 7,
            Congestion = 8,
            Aux2 = 9,
            Carga = 10,
            LvtpOpf = 11
        }

        public enum Cuadro
        {
            C8 = 2,
            Aux1 = 3,
            Datos = 4,
            Aux2 = 7,
            C5 = 8
        }

        public const string CaracterCero = "0";
        public const string FolderEscenario = "Escenario_";
        public const string ArchivoEntradaGams = "OPS_ENTRADA.DAT";
        public const string ArchivoSalidaGams = "_opf.csv";
        public const string ArchivoInicializaGms = "inicializa12.gms";
        public const string ArchivoGdx = "sol.gdx";
        public const int ColumnaGmasSize = 20;
        public const string MargenIzquierdoDat = "        ";
        public const char CaracterSeparacionCSV = ',';
        public const string InicioResultadoGamsPotGenerada = "\"Potencia Generada\",\"\",\"MW\"";
        public const string InicioResultadoGamsCongCompuesta = "\"Congestion Compuesta\",\"\",\"MW\",\"Limite MW\",\"Envio\"";
        public const string InicioResultadoGamsCongSimple = "\"Congestion Simple\",\"\",\"Envio(1)\",\"MW\",\"Limite MW\",\"Envio\"";

        public const string PathPotRemunerable = "PathPotRemunerable";

        //para  en control de archivos  Archivos (upload y download)";
        public const string SModuloEventos = "PotenciaFirmeRemunerable";
        public const string SNombreCarpetaUnifilar = "Unifilar";
        public const string SNombreCarpetaSalidasGams = "SalidasGams";
        public const string SNombreCarpetaCargaGams = "FuenteGams";
        //public const string UnifilarFile = "PotenciaFirmeRemunerable\\AplicativoPFR\\";
        public const string PotenciaFirmeRemunerableFile = "PotenciaFirmeRemunerable\\AplicativoPFR\\";

        #region  Salidas Gams
        public const string ColorSalidasGams = "#FCFF33"; //color amarillo 
        public enum SalidasGams
        {
            V = 1,
            Pg = 2,
            Congestion = 3,
        }
        #endregion

        #region Mejoras PFR
        public const string FormatoFecha = "dd-MM-yyyy";
        public enum Tipo
        {
            Barra = 1,
            Linea = 2,
            Trafo2 = 3,
            Trafo3 = 4,
            CompDinamica = 5,
            GamsVtp = 6,
            GamsSsaa = 7,
            GamsEquipos = 8,
            Congestion = 9,
            Penalidad = 10
        }

        public enum Concepto
        {
            Vigencia = 1,
            Tension = 2,
            Vmax = 3,
            Vmin = 4,
            Compreactiva = 5,
            Resistencia = 6,
            Reactancia = 7,
            Conductancia = 8,
            Admitancia = 9,
            Potenciamaxima = 10,
            Tap1 = 11,
            Tap2 = 12,
            Qmax = 13,
            Qmin = 14,
            Numunidad = 15,
            Ref = 16,
            PMax = 17,
            PMin = 18,
            Linea1 = 19,
            Linea2 = 20,
            Linea3 = 21,
            Linea4 = 22,
            Linea5 = 23,
            Linea6 = 24,
            Linea7 = 25,
            Linea8 = 26,
            Linea9 = 27,
            Linea10 = 28,
            Linea11 = 29,
            Linea12 = 30,
            Penalidad = 31,
            Descripcion = 32
        }

        #endregion

        //RE-19799
        public const string MesCVBaseDatos = "04 2022"; //mes desde que se usará los costos variables de base de datos
    }

    public class BarraSuministro
    {
        public int CodigoGams { get; set; }
        public string Faltante { get; set; }
        public string IdGams { get; set; }
        public string NombreGams { get; set; }
        public string NombreBarraGams { get; set; }
        public decimal? Pload { get; set; }
        public decimal? Qload { get; set; }
        public decimal? Fp { get; set; }
    }

    public class BarraSSAA
    {
        public int Codigo { get; set; }
        public string IdBGams { get; set; }
        public decimal? Ssaa { get; set; }
    }

    public class PestaniaDemanda
    {
        public int CodiBarra { get; set; }
        public string IdBarra { get; set; }
        public string NombreBarra { get; set; }
        public decimal? TensionBarra { get; set; }
        public decimal? P { get; set; }
        public decimal? Q { get; set; }
        public decimal? CompReactiva { get; set; }

    }

    public class PestaniaDatos
    {

        public string Empresa { get; set; }
        public string Central { get; set; }
        public string UnidadNombre { get; set; }
        public decimal? PE { get; set; }
        public decimal? PF { get; set; }
        public decimal? CV { get; set; }
        public string CR { get; set; }
        public decimal? FK { get; set; }

        public int Emprcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Equicodi { get; set; }

    }

    public class PestaniaAux2
    {
        public int CodiBarra { get; set; }
        public int NUmunidad { get; set; }
        public string Empresa { get; set; }
        public string Central { get; set; }
        public string UnidadNombre { get; set; }
        public decimal? PD { get; set; }
        public decimal? PDD { get; set; }
        public decimal? PE { get; set; }
        public decimal? PF { get; set; }
        public decimal? CV { get; set; }
        public bool tieneFicticio { get; set; }

        public int Emprcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Equicodi { get; set; }
    }
}

