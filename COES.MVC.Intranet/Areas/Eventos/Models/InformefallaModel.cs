using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class EveInformefallaModel
    {
        public EveInformefallaDTO EveInformefalla { get; set; }
        public List<SiPersonaDTO> ListaProgramador { get; set; }

        public int EveninfCodi { get; set; }
        public int EvenCodi { get; set; }
        public int? EvenAnio { get; set; }
        public int? EvenCorr { get; set; }
        public string EveninfFechEmis { get; set; }
        public string EveninfElab { get; set; }
        public string EveninfRevs { get; set; }
        public string EveninfLastUser { get; set; }
        public string EveninfLastDate { get; set; }
        public string EveninfEmitido { get; set; }
        public string EveninfPFechEmis { get; set; }
        public string EveninfPElab { get; set; }
        public string EveninfPRevs { get; set; }
        public string EveninfPIFechEmis { get; set; }
        public string EveninfPIElab { get; set; }
        public string EveninfPIRevs { get; set; }
        public string EveninfPEmitido { get; set; }
        public string EveninfPIEmitido { get; set; }
        public string EveninfMem { get; set; }
        public string EveninfPIEmit { get; set; }
        public string EveninfPEmit { get; set; }
        public string EveninfEmit { get; set; }
        public int? EvenCorrmem { get; set; }
        public string EveninfMemFechEmis { get; set; }
        public string EveninfMemElab { get; set; }
        public string EveninfMemRevs { get; set; }
        public string EveninfMemEmit { get; set; }
        public string EveninfMemEmitido { get; set; }
        public int? EvenCorrSco { get; set; }
        public string EveninfActuacion { get; set; }
        public string EveninfActLlamado { get; set; }
        public string EveninfActElab { get; set; }
        public string EveninfActFecha { get; set; }
        public string EvenAsunto { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaEveInformefallaModel
    {
        public List<EveInformefallaDTO> ListaEveInformefalla { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public string EquiAbrev { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string InformeFalla { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
    }

}
