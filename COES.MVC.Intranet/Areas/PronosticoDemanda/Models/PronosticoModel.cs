using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class PronosticoModel
    {
        public string Fecha { get; set; }
        public string FechaFin { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<Tuple<int, int, string, bool>> ListArea { get; set; }
        public List<PrnVersionDTO> ListBarraCP { get; set; }
        public int idCentro { get; set; }
        public int idSein { get; set; }
        //Id de los filtros
        public List<int> SelArea { get; set; }
        public List<PrnMediciongrpDTO> ListBarrasOrigen { get; set; }
        public List<PrnMediciongrpDTO> ListBarrasDestino { get; set; }
        public List<PrnMediciongrpDTO> ListVersion { get; set; }
        public List<PrGrupoDTO> ListBarraCPActiva { get; set; }
        public List<PrnRelacionTnaDTO> ListRelacionTna { get; set; }
        public List<PrnVersiongrpDTO> ListVersionFecha { get; set; }
    }
}