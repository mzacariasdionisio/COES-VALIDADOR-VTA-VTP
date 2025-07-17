namespace COES.WebAPI.HTrabajo.Models
{
    public class ServidorFtp
    {        
        public string Carpeta { get; set; }
        public string TipoServer { get; set; }
        public string FTPServidor { get; set; }
        public string FTPUsuario { get; set; }
        public string FTPClave { get; set; }
        public string FTPCarpetaCSV { get; set; }
        public string SFTPServidor { get; set; }
        public string SFTPUsuario { get; set; }
        public string SFTPClave { get; set; }
        public string SFTPRutaClavePrivada { get; set; }
        public string SFTPPassPhraseClavePrivada { get; set; }
        public string SFTPFingerprintClavePrivada { get; set; }
        public string SFTPCarpetaCSV { get; set; }
    }
}
