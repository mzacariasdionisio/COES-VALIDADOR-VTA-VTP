using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;


namespace COES.MVC.Extranet.Areas.CPPA.Models
{
    public class ApiResponse<T>
    {
        public string mensaje { get; set; }
        public int resultado { get; set; }
        public List<T> response { get; set; }
    }

    // Definir la clase equivalente a ConsultarEnvioResponse
    public class ConsultarEnvioResponse
    {
        public int Resultado { get; set; }
        public int Cpadoccodi { get; set; }
        public DateTime Cpadocfeccreacion { get; set; }
        public string Cpadocusucreacion { get; set; }
        public int Cpaapanio { get; set; }
        public string Cpaapajuste { get; set; }
        public int Cparcodi { get; set; }
        public string Cparrevision { get; set; }
        public int Cpadoccodenvio { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
    }

    public class ConsultarDetalleEnvioResponse
    {
        public int Resultado { get; set; }
        public int Cpaddtcodi { get; set; }
        public int Cpadoccodi { get; set; }
        public string Cpaddtruta { get; set; }
        public string Cpaddtnombre { get; set; }
        public string Cpaddttamano { get; set; }
        public DateTime Cpaddtfeccreacion { get; set; }
        public string Cpaddtusucreacion { get; set; }
    }

    public class ConsultarRevisionResponse
    {
        public int Resultado { get; set; }
        public int Cpaapanio { get; set; }
        public string Cpaapajuste { get; set; }
        public int Cpaapanioejercicio { get; set; }
        public string Cparestado { get; set; }
        public int Cparcodi { get; set; }
        public string Cparrevision { get; set; }
    }

    public class CargaArchivosIntegrantesModel
    {
        public int Resultado { get; set; }
        public string sError { get; set; }
        public string sMensaje { get; set; }
        public string sTipo { get; set; }
        public int sStatus { get; set; }
        public List<GenericoDTO> ListaAnio { get; set; }
        public List<GenericoDTO> ListaPopAnio { get; set; }
        public List<CpaDocumentosDTO> ListaDocumentosGrilla { get; set; }
        public List<CpaDocumentosDetalleDTO> ListaDocumentosDetalleGrilla { get; set; }
        public COES.Dominio.DTO.Transferencias.EmpresaDTO EntidadEmpresa { get; set; }
        public List<COES.Dominio.DTO.Transferencias.EmpresaDTO> ListaEmpresas { get; set; }
    }

    public class ArchivoJsonResponse {
        public string FileName { get; set; }
        public string FileBytes { get; set; }
    }
}