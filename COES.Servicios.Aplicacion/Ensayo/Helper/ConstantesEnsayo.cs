namespace COES.Servicios.Aplicacion.Ensayo.Helper
{
    /// <summary>
    /// Constantes para los datos de ensayo
    /// </summary>
    public class SesionEnsayo
    {
        public const string SesionIdEnsayo = "SesionIdEnsayo";
        public const string SesionIdFormAnt = "SesionIdForm";
        public const string SesionNroOrdenEnsayo = "SesionNroOrdenEnsayo";
        public const string SesionIdOpcion = "SesionIdOpcion";
    }

    public class ConstantesEnsayo
    {
        public const string CodFamilias = "5";
        public const string FolderReporte = "Areas\\Ensayo\\Reporte\\";
        public const string FolderRepositorio = "Areas\\Ensayo\\Repositorio\\";
        public const string FolderFormato = "Formatos";
    }

    /// <summary>
    /// Contiene los nombres de los archivos
    /// </summary>
    public class FormatoEnsayo
    {
        public const string RepositorioEnsayo = "RepositorioEnsayo";
        public const string NombreReporte = "RptEnsayos.xlsx";
        public const string PlantillaExcelEnsayos = "PlantillaReporteEnsayos.xlsx";
        public const int NroFormatos = 10;

    }

    public class EnsayoEnvio
    {
        public const byte EnsayoEnviado = 1;
        public const byte EnsayoObservaciones = 2;
        public const byte EnsayoAprobado = 3;
        public const byte EnsayoRechazado = 4;
        public const string FromEmail = "webapp@coes.org.pe";
        public const string FromName = "Declaración Jurada de Ensayos de Potencia";
        public const string AdminEmail = "admEnsayo";
        public const string SubjetcEmail = "Notificacion de Solicitud de Ensayo de Potencia";
    }

    /// <summary>
    /// Estado de los ensayos
    /// </summary>
    public class EstadoEnsayo
    {
        public const int Solicitado = 1;
        public const int Autorizado = 2;
        public const int AutorizadoObservacion = 3;
        public const int Aprobado = 4;
        public const int Archivado = 5;
    }

    /// <summary>
    /// Estado de los formatos
    /// </summary>
    public class EstadoFormato
    {
        public const int Enviado = 5;
        public const int Observado = 6;
        public const int Corregido = 7;
        public const int Aprobado = 4;
    }

    /// <summary>
    /// Contiene las rutas de los directorios utilizados
    /// </summary>
    public class RutaDirectorioEnsayo
    {
        public const string Repositorio = "RepositorioEnsayo";
    }
}
