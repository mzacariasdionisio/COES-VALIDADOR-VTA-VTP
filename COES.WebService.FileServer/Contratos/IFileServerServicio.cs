using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace COES.WebService.FileServer.Contratos
{
    
    [ServiceContract]
    public interface IFileServerServicio
    {
        [OperationContract]
        List<FileDataRequest> ListarArhivosWS(string relativePath);

        [OperationContract]
        Stream ObtenerBytesArchivoADescargar(string carpeta, string nombreRecursoSeleccionado);

    }

    [DataContract]
    public class FileDataRequest
    {
        [DataMember]
        public string FileUrl { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string FileType { get; set; }
        [DataMember]
        public string FileSize { get; set; }
        [DataMember]
        public string Icono { get; set; }
        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public string FechaModificacion { get; set; }
        [DataMember]
        public string LastWriteTime { get; set; }
    }
}
