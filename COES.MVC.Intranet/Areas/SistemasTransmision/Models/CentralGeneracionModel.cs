using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class CentralGeneracionModel : BaseModel
    {
        public CentralGeneracionDTO EntidadCentralGeneracion { get; set; }
        public List<CentralGeneracionDTO> ListaCentralGeneracion { get; set; }

        public List<StCentralgenDTO> ListaSTCentralGen { get; set; }
        public StCentralgenDTO Entidad { get; set; }

        public List<StGeneradorDTO> ListaEmpresaGenerador { get; set; }
        public StGeneradorDTO EntidadGenerador { get; set; }

        public List<BarraDTO> ListaBarra { get; set; }
        public BarraDTO EntidadBarra { get; set; }

        public List<StRecalculoDTO> ListaRecalculo { get; set; }
        public StRecalculoDTO EntidadRecalculo { get; set; }

    }
}