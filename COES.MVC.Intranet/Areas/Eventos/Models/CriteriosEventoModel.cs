using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class CriteriosEventoModel
    {

        //filtro de la consulta
        public string EmpresaPropietaria { get; set; }
        public string EmpresaInvolucrada { get; set; }
        public string CriterioDecision { get; set; }
        public string CasosEspeciales { get; set; }
        public string Impugnacionc { get; set; }
        public string CriteriosImpugnacion { get; set; }
        public string DI { get; set; }
        public string DF { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }
        //------------------------------------------------
        public int Crevencodi { get; set; }
        public string CodigoEvento { get; set; }
        public string FechaEvento { get; set; }
        public string NombreEvento { get; set; }
        public int CodigoImpugnacion { get; set; }
        public string Impugnacion { get; set; }
        public int Crespecialcodi { get; set; }
        public int Afecodi { get; set; }
        public DateTime Afeitdecfechaelab { get; set; }
        public List<CrCasosEspecialesDTO> ListaCasosEspeciales { get; set; }
        public List<CrCriteriosDTO> ListaCrCriterios { get; set; }
        public List<CrEventoDTO> listaCriterios { get; set; }
        public CrEtapaEventoDTO EtapaDesicion { get; set; }
        public List<CrEmpresaResponsableDTO> ListaEmpresaRespDesicion { get; set; }
        public string ListaCriteriosDecision { get; set; }
        public CrEtapaEventoDTO EtapaReconsideracion { get; set; }
        public List<CrEmpresaResponsableDTO> ListaEmpresaRespReconsideracion { get; set; }
        public List<CrEmpresaSolicitanteDTO> ListaEmpresaSolReconsideracion { get; set; }
        public string ListaCriteriosReconsideracion { get; set; }
        public CrEtapaEventoDTO EtapaApelacion { get; set; }
        public List<CrEmpresaResponsableDTO> ListaEmpresaRespApelacion { get; set; }
        public List<CrEmpresaSolicitanteDTO> ListaEmpresaSolApelacion { get; set; }
        public string ListaCriteriosApelacion { get; set; }
        public CrEtapaEventoDTO EtapaArbitraje { get; set; }
        public List<CrEmpresaResponsableDTO> ListaEmpresaRespArbitraje { get; set; }
        public List<CrEmpresaSolicitanteDTO> ListaEmpresaSolArbitraje { get; set; }
        public string ListaCriteriosArbitraje { get; set; }
        public CrEmpresaSolicitanteDTO DetEmpresaSolicitante { get; set; }
        public int TipoSolicitante { get; set; }
    }

    /// <summary>
    /// Model para el CRUD de CR_CASOS_ESPECIALES
    /// </summary>
    public class CasosEspecialesModel
    {
        public int CRESPECIALCODI { get; set; }
        public string CREDESCRIPCION { get; set; }
        public string CREESTADO { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public List<CrCasosEspecialesDTO> ListaCasosEspeciales { get; set; }
        public CrCasosEspecialesDTO EntidadCasosEspeciales { get; set; }
    }

    public class CriteriosModel
    {
        public int CRCRITERIOCODI { get; set; }
        public string CREDESCRIPCION { get; set; }
        public string CREESTADO { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public List<CrCriteriosDTO> ListaCriterios { get; set; }
        public CrCriteriosDTO EntidadCriterios { get; set; }
    }
}