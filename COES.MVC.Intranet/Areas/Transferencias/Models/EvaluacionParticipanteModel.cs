using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class EvaluacionParticipanteModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MeHojaptomedDTO> ListaHojaPto { get; set; }
        public string FechaVigencia { get; set; }
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public TrnConfiguracionPmmeDTO EntidadConfPtoMME { get; set; }
        public List<TrnConfiguracionPmmeDTO> ListaConfiguracionPtos { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public TrnPeriodoCnaDTO EntidadConfiguracionDias { get; set; }
        public string SemanaPeriodo { get; set; }
        public string AnioPeriodo { get; set; }
        public List<GenericoDTO> ListaGenSemanas { get; set; }
        public List<TrnLogCnaDTO> ListaLogCna { get; set; }
        public TrnConfiguracionPmmeDTO ConfiguracionPto { get; set; }
    }
    public class BusquedaEVALDPModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }       
        public int IdEmpresa { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
    }
}