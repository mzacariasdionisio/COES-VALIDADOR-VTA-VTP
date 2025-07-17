using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using EmpresaDTO = COES.MVC.Extranet.SeguridadServicio.EmpresaDTO;

namespace COES.MVC.Extranet.Areas.InformacionAgentes.Models
{
    public class InformacionAgenteViewModels
    {
    }
    public class FileUploadViewModel
    {
        public string FormAction { get; set; }
        public string FormMethod { get; set; }
        public string FormEnclosureType { get; set; }
        public string Bucket { get; set; }
        public string FileId { get; set; }
        public string AWSAccessKey { get; set; }
        public string RedirectUrl { get; set; }
        public string Acl { get; set; }
        public string Base64Policy { get; set; }
        public string Signature { get; set; }
        //Datos de Paginado y Vista
        public List<EmpresaDTO> ListaEmpresa { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public List<InfArchivoAgenteDTO> ListaResultados { get; set; }
        public int iEmpresa { get; set; }
    }
}