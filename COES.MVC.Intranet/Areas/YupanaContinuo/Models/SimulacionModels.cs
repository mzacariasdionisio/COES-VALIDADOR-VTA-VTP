using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Models
{
    public class SimulacionModel
    {
        public string Fecha { get; set; }
        public List<GenericoDTO> ListaHora { get; set; }

        public List<CpTopologiaDTO> ListaTopologia { get; set; }
        public List<CpArbolContinuoDTO> ListaTag { get; set; }
        public int CodigoTopologiaMostrado { get; set; }
        public int CodigoArbolMostrado { get; set; }

        public List<EstructuraNodo> LstDatosNodos { get; set; }
        public string EsUltimoTag { get; set; }

        public decimal PorcentajeNodosSimulados { get; set; }
        public string IdentificadorArbolCreado { get; set; }
        public string MensajeProceso { get; set; }
        public string TagArbolCreado { get; set; }
        public string FechaArbolCreado { get; set; }

        public bool TienePermisoAdmin { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }


    }
}