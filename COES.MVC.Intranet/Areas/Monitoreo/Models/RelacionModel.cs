using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Monitoreo.Models
{
    public class RelacionModel
    {
        public int Id { get; set; }
        public string FechaInicio { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<PrCategoriaDTO> ListaCategoria { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Motivo { get; set; }
        public MmmVersionDTO Generador { get; set; }
        public List<CmConfigbarraDTO> LisConfigBarr { get; set; }
        public PrGrupoDTO ObjBarr { get; set; }
        public List<PrGrupoxcnfbarDTO> LisRelacion { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
    }
}