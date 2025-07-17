using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.FileManager.ViewModels
{
    public class InformacionAgentesViewModel
    {
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public int iEmpresa { get; set; }
        public List<InfArchivoAgenteDTO> ListaResultados { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
    }
}