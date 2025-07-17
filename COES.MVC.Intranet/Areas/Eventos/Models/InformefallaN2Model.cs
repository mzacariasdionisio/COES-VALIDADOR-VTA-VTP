using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class EveInformefallaN2Model
    {
        public EveInformefallaN2DTO EveInformefallaN2 { get; set; }
        public List<SiPersonaDTO> ListaProgramador { get; set; }
        public int EvenInfN2Codi { get; set; }
        public int? EvenCodi { get; set; }
        public int? EvenAnio { get; set; }
        public int? EvenN2Corr { get; set; }
        public string EvenInfPIN2FechEmis { get; set; }
        public string EvenInfPIN2Emitido { get; set; }
        public string EvenInfPIN2Elab { get; set; }
        public string EvenInfFN2Emitido { get; set; }
        public string EvenInfFN2Elab { get; set; }
        public string EvenInfN2LastUser { get; set; }
        public string EvenInfN2LastDate { get; set; }
        public string EvenInfFN2FechEmis { get; set; }
        public string EvenIPIEN2Emitido { get; set; }
        public string EvenIPIEN2Elab { get; set; }
        public string EvenIPIEN2FechEm { get; set; }
        public string EvenIFEN2Emitido { get; set; }
        public string EvenIFEN2Elab { get; set; }
        public string EvenIFEN2FechEm { get; set; }
        public int Accion { get; set; }
        
        

    }

    public class BusquedaEveInformefallaN2Model
    {
        public List<EveInformefallaN2DTO> ListaEveInformefallaN2 { get; set; }
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
