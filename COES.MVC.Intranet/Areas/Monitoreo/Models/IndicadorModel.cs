using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Monitoreo.Models
{
    public class IndicadorModel
    {
        public int Id { get; set; }
        public string FechaInicio { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string Resultado { get; set; }
        public List<string> ListaResultado { get; set; }
        public string Resultado2 { get; set; }
        public string Resultado3 { get; set; }
        public int Resultado4 { get; set; }
        public bool Estado { get; set; }
        public string Periodo { get; set; }
        public string NombreArchivo { get; set; }
        public int DiaMes { get; set; }
        public string Mensaje { get; set; }
        public List<EveIeodcuadroDTO> ListaIeodcuadro { get; set; }
        public string Motivo { get; set; }
        public MmmVersionDTO Generador { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroSemana { get; set; }
        public int Total { get; set; }
        public string MesPeriodo { get; set; }
        public List<MmmVersionDTO> ListaVersion { get; set; }
        public int Indicador { get; set; }
        public List<MmmIndicadorDTO> ListaIndicador { get; set; }
        public List<MmmJustificacionDTO> ListaJustif { get; set; }
    }
}