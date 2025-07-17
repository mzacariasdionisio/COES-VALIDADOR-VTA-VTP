using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Models
{
    public class BrowserModel
    {
        public string BaseDirectory { get; set; }
        public List<FileData> DocumentList { get; set; }
        public List<BreadCrumb> BreadList { get; set; }
        public List<WbColumnitemDTO> ListaItems { get; set; }
        public List<WbBlobconfigDTO> TypeLibraryList { get; set; }
        public WbBlobDTO Folder { get; set; }
        public WbBlobDTO Archivo { get; set; }
        public string IndicadorPrincipal { get; set; }
        public string IndicadorMetadato { get; set; }
        public List<WbBlobcolumnDTO> ListaColumnas { get; set; }
        public string Formulario { get; set; }
        public string Origen { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public string ContenidoIssuu { get; set; }
        public string PosicionIssuu { get; set; }
        public string IndicadorIssuu { get; set; }
        public string TipoVisor { get; set; }
        public string ArchivoDefecto { get; set; }
        public string IndicadorHeader { get; set; }
        public string BreadName { get; set; }
        public string OrderFolder { get; set; }
        public string BlobCodi { get; set; }
        public string RelativeDirectory { get; set; }
        public string Url { get; set; }
    }
}