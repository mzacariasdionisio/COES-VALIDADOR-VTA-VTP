using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Interconexiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Interconexiones.Models
{
    public class InterconexionesModel
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Resultado { get; set; }
        public string SheetName { get; set; }
        public string TituloCapExc { get; set; }
        public int IdPtomedicion { get; set; }
        public List<MeLecturaDTO> ListaHorizonte { get; set; }
        public List<InInterconexionDTO> ListaInterconexion { get; set; }
        public GraficoWeb Grafico { get; set; }
        public List<MeMedicion24DTO> ListaEnerPot { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public int IdParametro { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int IdMedidor { get; set; }
        public List<MeMedidorDTO> ListaMedidor { get; set; }
        public List<LogErrorInterconexion> ListaErrores { get; set; }
        public List<EstructuraEvolucionEnergia> ListaReporteEvolucion { get; set; }
    }

}