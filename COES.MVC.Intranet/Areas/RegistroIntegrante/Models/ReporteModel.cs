using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Models
{

    public class ReporteModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public int IdTipoEmpresa { get; set; }
        public int IdEmpresa { get; set; } 

        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaIntegrantes;
        public List<TipoTitularAlternoModel> ListaTipoRepresentante;
        public List<EstadoModel> ListaEstado;
        public List<TipoRpteLegalContactoModel> ListaRepresentanteContacto;
        public List<ModalidadVoluntarioObligatorioModel> ListaModalidadVoluntarioObligatorio;
        public List<RiTiposolicitudDTO> ListaTipoSolicitudes;
        public List<TipoRevisionModel> ListaTipoRevision;

        public ReporteModel()
        {
            ListaIntegrantes = new List<SiEmpresaDTO>();
            ListaTipoRepresentante = new List<TipoTitularAlternoModel>();
            ListaEstado = new List<EstadoModel>();            
            ListaRepresentanteContacto = new List<TipoRpteLegalContactoModel>();
            ListaModalidadVoluntarioObligatorio = new List<ModalidadVoluntarioObligatorioModel>();
            ListaTipoSolicitudes = new List<RiTiposolicitudDTO>();
            ListaTipoRevision = new List<TipoRevisionModel>();
        }
    }

    public class TipoRevisionModel
    {
        public int TipoRevisionCodigo { get; set; }
        public string TipoRevisionDescripcion { get; set; }
        public TipoRevisionModel() { }
        public TipoRevisionModel(int pTipoRevisionCodigo, string pTipoRevisionDescripcion)
        {
            TipoRevisionCodigo = pTipoRevisionCodigo;
            TipoRevisionDescripcion = pTipoRevisionDescripcion;
        }
    }
    public class TipoTitularAlternoModel
    {
        public string TipoTitularAlternoCodigo { get; set; }
        public string TipoTitularAlternoDescripcion { get; set; }
        public TipoTitularAlternoModel() { }
        public TipoTitularAlternoModel(string pTipoTitularAlternoCodigo, string pTipoTitularAlternoDescripcion)
        {
            TipoTitularAlternoCodigo = pTipoTitularAlternoCodigo;
            TipoTitularAlternoDescripcion = pTipoTitularAlternoDescripcion;
        }
    }

    public class TipoRpteLegalContactoModel
    {
        public string TipoRpteLegalContactoCodigo { get; set; }
        public string TipoRpteLegalContactoDescripcion { get; set; }
        public TipoRpteLegalContactoModel() { }
        public TipoRpteLegalContactoModel(string pTipoRpteLegalContactoCodigo, string pTipoRpteLegalContactoDescripcion)
        {
            TipoRpteLegalContactoCodigo = pTipoRpteLegalContactoCodigo;
            TipoRpteLegalContactoDescripcion = pTipoRpteLegalContactoDescripcion;
        }
    }

    public class ModalidadVoluntarioObligatorioModel
    {
        public string ModalidadVoluntarioObligatorioCodigo { get; set; }
        public string ModalidadVoluntarioObligatorioDescripcion { get; set; }
        public ModalidadVoluntarioObligatorioModel() { }
        public ModalidadVoluntarioObligatorioModel(string pModalidadVoluntarioObligatorioCodigo, string pModalidadVoluntarioObligatorioDescripcion)
        {
            ModalidadVoluntarioObligatorioCodigo = pModalidadVoluntarioObligatorioCodigo;
            ModalidadVoluntarioObligatorioDescripcion = pModalidadVoluntarioObligatorioDescripcion;
        }
    }

}