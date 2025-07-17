using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Models
{
    public class RepresentanteModel
    {
        public List<SiRepresentanteDTO> ListaRepresentantes { get; set; }

        public int Id { get; set; }
        public int RpteCodi { get; set; }
        public string TipoDocumento { get; set; }
        public string TipoRepresentanteLegal { get; set; }        
        public string Documento { get; set; }

        public string DocumentoAdjunto { get; set; }
        public string DocumentoAdjuntoFileName { get; set; }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        public string VigenciaPoderAdjunto { get; set; }
        public string VigenciaPoderAdjuntoFileName { get; set; }

        public string CargoEmpresa { get; set; }
        public string Telefono { get; set; }
        public string TelefonoMovil { get; set; }
        public string CorreoElectronico { get; set; }

        public int Emprcodi { get; set; }
        public HttpPostedFileBase DNI { get; set; }
        public HttpPostedFileBase VigenciaPoder { get; set; }


    }

    public class TipoDocumentoModel
    {
        public string TipoDocumentoCodigo { get; set; }
        public string TipoDocumentoDescripcion { get; set; }
        public TipoDocumentoModel() { }
        public TipoDocumentoModel(string pTipoDocumentoCodigo, string pTipoDocumentoDescripcion)
        {
            TipoDocumentoCodigo = pTipoDocumentoCodigo;
            TipoDocumentoDescripcion = pTipoDocumentoDescripcion;
        }
    }
}