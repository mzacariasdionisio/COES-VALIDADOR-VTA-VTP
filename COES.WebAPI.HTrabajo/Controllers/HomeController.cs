using Azure.Identity;
using COES.WebAPI.HTrabajo.Helper;
using COES.WebAPI.HTrabajo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Renci.SshNet;
using System.Net;
using System.Text;

namespace COES.WebAPI.HTrabajo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public ActionResult Index()
        {

            return View();
        }

        [HttpGet("1/{nombreArchivo}")]
        public int DescargarArchivo(string nombreArchivo)
        {
            var clientId = _configuration.GetValue<string>("AzureAd:ClientId");
            var tenantId = _configuration.GetValue<string>("AzureAd:TenantId");
            var clientSecret = _configuration.GetValue<string>("AzureAd:ClientSecret");
            var driverIdSCO = _configuration.GetValue<string>("AzureAd:DriverIdSCO");

            var keyCarpetaTemporal = _configuration.GetValue<string>("HtrabajoRER:Carpeta");

            //eliminar archivos temporales
            UtilArchivo.EliminarArchivoTemporal(keyCarpetaTemporal, "Htrabajo_");
            UtilArchivo.EliminarArchivoTemporal(keyCarpetaTemporal, "Measurement_");

            //Graph
            var clienteSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            GraphServiceClient graphServiceClient = new GraphServiceClient(clienteSecretCredential);

            //https://developer.microsoft.com/es-ES/graph/graph-explorer
            //https://graph.microsoft.com/v1.0/users
            //
            var listaArchivo = graphServiceClient.Drives[driverIdSCO].Root.Children["IDCOS"].Children.Request().OrderBy("name desc").GetAsync().Result; //Obtener los primeros 200 archivos ordenados descendentemente

            bool existeArchivo = false;
            foreach (var file in listaArchivo)
            {
                if (file.Name.ToLower() == nombreArchivo.ToLower())
                {
                    var requestFile = graphServiceClient.Drives[driverIdSCO].Items[file.Id].Content.Request();
                    var stream = requestFile.GetAsync().Result;

                    var driveItemPath = Path.Combine(keyCarpetaTemporal, nombreArchivo);
                    var driveItemFile = System.IO.File.Create(driveItemPath);
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(driveItemFile);
                    driveItemFile.Close();

                    existeArchivo = true;
                }
            }

            return existeArchivo ? 1 : 0;
        }

        [HttpGet("2/{archivoCsv}")]
        public int SubirArchivo(string archivoCsv)
        {
            _logger.LogInformation("INI.............");
            List<ServidorFtp> listadoServidores = new List<ServidorFtp>()
            {
                new ServidorFtp()
                {
                    Carpeta = _configuration.GetValue<string>("HtrabajoRER:Carpeta"),
                    TipoServer = _configuration.GetValue<string>("HtrabajoRER:TipoServer"),
                    FTPServidor = _configuration.GetValue<string>("HtrabajoRER:FTPServidor"),
                    FTPUsuario = _configuration.GetValue<string>("HtrabajoRER:FTPUsuario"),
                    FTPClave = _configuration.GetValue<string>("HtrabajoRER:FTPClave"),
                    FTPCarpetaCSV = _configuration.GetValue<string>("HtrabajoRER:FTPCarpetaCSV"),
                    SFTPServidor = _configuration.GetValue<string>("HtrabajoRER:SFTPServidor"),
                    SFTPUsuario = _configuration.GetValue<string>("HtrabajoRER:SFTPUsuario"),
                    SFTPClave = _configuration.GetValue<string>("HtrabajoRER:SFTPClave"),
                    SFTPRutaClavePrivada = _configuration.GetValue<string>("HtrabajoRER:SFTPRutaClavePrivada"),
                    SFTPPassPhraseClavePrivada = _configuration.GetValue<string>("HtrabajoRER:SFTPPassPhraseClavePrivada"),
                    SFTPFingerprintClavePrivada = _configuration.GetValue<string>("HtrabajoRER:SFTPFingerprintClavePrivada"),
                    SFTPCarpetaCSV = _configuration.GetValue<string>("HtrabajoRER:SFTPCarpetaCSV"),
                }
                /*,
                new ServidorFtp()
                {
                    Carpeta = _configuration.GetValue<string>("HtrabajoREREnorChile:Carpeta"),
                    TipoServer = _configuration.GetValue<string>("HtrabajoREREnorChile:TipoServer"),
                    FTPServidor = _configuration.GetValue<string>("HtrabajoREREnorChile:FTPServidor"),
                    FTPUsuario = _configuration.GetValue<string>("HtrabajoREREnorChile:FTPUsuario"),
                    FTPClave = _configuration.GetValue<string>("HtrabajoREREnorChile:FTPClave"),
                    FTPCarpetaCSV = _configuration.GetValue<string>("HtrabajoREREnorChile:FTPCarpetaCSV"),
                    SFTPServidor = _configuration.GetValue<string>("HtrabajoREREnorChile:SFTPServidor"),
                    SFTPUsuario = _configuration.GetValue<string>("HtrabajoREREnorChile:SFTPUsuario"),
                    SFTPClave = _configuration.GetValue<string>("HtrabajoREREnorChile:SFTPClave"),
                    SFTPRutaClavePrivada = _configuration.GetValue<string>("HtrabajoREREnorChile:SFTPRutaClavePrivada"),
                    SFTPPassPhraseClavePrivada = _configuration.GetValue<string>("HtrabajoREREnorChile:SFTPPassPhraseClavePrivada"),
                    SFTPFingerprintClavePrivada = _configuration.GetValue<string>("HtrabajoREREnorChile:SFTPFingerprintClavePrivada"),
                    SFTPCarpetaCSV = _configuration.GetValue<string>("HtrabajoREREnorChile:SFTPCarpetaCSV"),
                },
                new ServidorFtp()
                {
                    Carpeta = _configuration.GetValue<string>("HtrabajoRESuncast:Carpeta"),
                    TipoServer = _configuration.GetValue<string>("HtrabajoRESuncast:TipoServer"),
                    FTPServidor = _configuration.GetValue<string>("HtrabajoRESuncast:FTPServidor"),
                    FTPUsuario = _configuration.GetValue<string>("HtrabajoRESuncast:FTPUsuario"),
                    FTPClave = _configuration.GetValue<string>("HtrabajoRESuncast:FTPClave"),
                    FTPCarpetaCSV = _configuration.GetValue<string>("HtrabajoRESuncast:FTPCarpetaCSV"),
                    SFTPServidor = _configuration.GetValue<string>("HtrabajoRESuncast:SFTPServidor"),
                    SFTPUsuario = _configuration.GetValue<string>("HtrabajoRESuncast:SFTPUsuario"),
                    SFTPClave = _configuration.GetValue<string>("HtrabajoRESuncast:SFTPClave"),
                    SFTPRutaClavePrivada = _configuration.GetValue<string>("HtrabajoRESuncast:SFTPRutaClavePrivada"),
                    SFTPPassPhraseClavePrivada = _configuration.GetValue<string>("HtrabajoRESuncast:SFTPPassPhraseClavePrivada"),
                    SFTPFingerprintClavePrivada = _configuration.GetValue<string>("HtrabajoRESuncast:SFTPFingerprintClavePrivada"),
                    SFTPCarpetaCSV = _configuration.GetValue<string>("HtrabajoRESuncast:SFTPCarpetaCSV"),
                }*/
            };

            int resultado = SubirArchivoFtps(archivoCsv, listadoServidores);
            return resultado;
        }

        [HttpGet("3/{nombreArchivo}")]
        public int DescargarArchivoCDispatch(string nombreArchivo)
        {
            var clientId = _configuration.GetValue<string>("AzureAd:ClientId");
            var tenantId = _configuration.GetValue<string>("AzureAd:TenantId");
            var clientSecret = _configuration.GetValue<string>("AzureAd:ClientSecret");
            var driverIdSCO = _configuration.GetValue<string>("AzureAd:DriverIdSCO");

            var keyCarpetaTemporal = _configuration.GetValue<string>("HtrabajoRER:Carpeta");

            //eliminar archivos temporales
            UtilArchivo.EliminarArchivoTemporal(keyCarpetaTemporal, "CDispatch_");

            //Graph
            var clienteSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            GraphServiceClient graphServiceClient = new GraphServiceClient(clienteSecretCredential);

            //https://developer.microsoft.com/es-ES/graph/graph-explorer
            //https://graph.microsoft.com/v1.0/users
            //
            var listaArchivo = graphServiceClient.Drives[driverIdSCO].Root.Children["IDCOS"].Children.Request().OrderBy("name desc").GetAsync().Result; //Obtener los primeros 200 archivos ordenados descendentemente

            //buscar nombre de archivo en Onedrive
            bool existeArchivo = false;
            foreach (var file in listaArchivo)
            {
                if (file.Name.ToLower() == nombreArchivo.ToLower())
                {
                    var requestFile = graphServiceClient.Drives[driverIdSCO].Items[file.Id].Content.Request();
                    var stream = requestFile.GetAsync().Result;

                    //cambiar nombre archivo
                    nombreArchivo = nombreArchivo.Replace("Htrabajo", "CDispatch");
                    var driveItemPath = Path.Combine(keyCarpetaTemporal, nombreArchivo);
                    var driveItemFile = System.IO.File.Create(driveItemPath);
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(driveItemFile);
                    driveItemFile.Close();

                    existeArchivo = true;
                }
            }

            return existeArchivo ? 1 : 0;
        }

        public int SubirArchivoFtps(string archivoCsv, List<ServidorFtp> listadoServidores)
        {
            int resultado = 0;
            _logger.LogInformation("INICIANDO ENVÍO CSV A LOS AGENTES");

            foreach (ServidorFtp servidor in listadoServidores)
            {
                try
                {
                    _logger.LogInformation(String.Format("PROCESANDO: {0} - {1} - {2}", servidor.SFTPServidor, servidor.SFTPCarpetaCSV, archivoCsv));
                    resultado = 0;

                    var keyCarpetaTemporal = servidor.Carpeta;
                    var keyTipoServer = servidor.TipoServer;

                    if (keyTipoServer == "FTP")
                    {
                        _logger.LogInformation("FTP");
                        var keyServidor = servidor.FTPServidor;
                        var keyUsuario = servidor.FTPUsuario;
                        var keyClave = servidor.FTPClave;
                        var keyCarpetaCSV = servidor.FTPCarpetaCSV;

                        String uploadUrl = String.Format("ftp://{0}/{1}/{2}", keyServidor, keyCarpetaCSV, archivoCsv);
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        // This example assumes the FTP site uses anonymous logon.  
                        request.Credentials = new NetworkCredential(keyUsuario, keyClave);
                        request.Proxy = null;
                        request.KeepAlive = true;
                        request.UseBinary = true;
                        request.Method = WebRequestMethods.Ftp.UploadFile;

                        // Copy the contents of the file to the request stream.  
                        string uploadfile = string.Format("{0}\\{1}", keyCarpetaTemporal, archivoCsv);
                        StreamReader sourceStream = new StreamReader(uploadfile);
                        byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                        sourceStream.Close();
                        request.ContentLength = fileContents.Length;
                        Stream requestStream = request.GetRequestStream();
                        requestStream.Write(fileContents, 0, fileContents.Length);
                        requestStream.Close();

                        Console.WriteLine("Uploading {0} ({1:N0} bytes)", uploadfile, fileContents.Length);
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                        Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                    }
                    else
                    {
                        _logger.LogInformation("SFTP");
                        //SFTP
                        var keyServidor = servidor.SFTPServidor;
                        var keyUsuario = servidor.SFTPUsuario;
                        var keyClave = servidor.SFTPClave;

                        var keyRutaClavePrivada = servidor.SFTPRutaClavePrivada;
                        var keyPassPhraseClavePrivada = servidor.SFTPPassPhraseClavePrivada;
                        var keyFingerprintClavePrivada = servidor.SFTPFingerprintClavePrivada;

                        var keyCarpetaCSV = servidor.SFTPCarpetaCSV;

                        if (!string.IsNullOrEmpty(keyRutaClavePrivada))
                        {
                            _logger.LogInformation("SFTP - 1");
                            WinSCP.SessionOptions sessionOptions = new WinSCP.SessionOptions
                            {
                                Protocol = WinSCP.Protocol.Sftp,
                                PortNumber = 22,
                                HostName = keyServidor,
                                UserName = keyUsuario,
                                SshPrivateKeyPath = keyRutaClavePrivada,
                                GiveUpSecurityAndAcceptAnySshHostKey = true //autenticar sin solicitar passphare ni fingerprint
                            };

                            string uploadfile = string.Format("{0}\\{1}", keyCarpetaTemporal, archivoCsv);
                            _logger.LogInformation("SFTP - 1 - uploadfile " + uploadfile);
                            string directorioRemoto = string.Format("/{0}/", keyCarpetaCSV);
                            _logger.LogInformation("SFTP - 1 - directorioRemoto - " + directorioRemoto);

                            using (WinSCP.Session session = new WinSCP.Session())
                            {
                                session.Open(sessionOptions);

                                // Upload files
                                WinSCP.TransferOptions transferOptions = new WinSCP.TransferOptions();
                                transferOptions.TransferMode = WinSCP.TransferMode.Binary;

                                WinSCP.TransferOperationResult transferResult;
                                transferResult = session.PutFiles(uploadfile, directorioRemoto, false, transferOptions);

                                // Throw on any error
                                transferResult.Check();

                                resultado = 1;
                            }

                        }
                        else
                        {
                            _logger.LogInformation("SFTP - 2");
                            using (SftpClient sftpClient = new SftpClient(keyServidor, 22, keyUsuario, keyClave))
                            {
                                try
                                {
                                    sftpClient.Connect();

                                    sftpClient.ChangeDirectory(string.Format("/{0}/", keyCarpetaCSV));

                                    string uploadfile = string.Format("{0}\\{1}", keyCarpetaTemporal, archivoCsv);
                                    using (var fileStream = new FileStream(uploadfile, System.IO.FileMode.Open))
                                    {
                                        Console.WriteLine("Uploading {0} ({1:N0} bytes)", uploadfile, fileStream.Length);
                                        sftpClient.BufferSize = 4 * 1024; // bypass Payload error large files
                                        sftpClient.UploadFile(fileStream, Path.GetFileName(uploadfile));
                                    }

                                    sftpClient.Disconnect();
                                    Console.WriteLine("Upload File Complete");

                                    resultado = 1;
                                }
                                catch (Exception er)
                                {
                                    Console.WriteLine("An exception has been caught " + er.ToString());
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    resultado = 0;
                    _logger.LogError("SubirArchivoFtps: " + ex.Message, ex.Message);
                }

            }

            return resultado;
        }
    }
}
