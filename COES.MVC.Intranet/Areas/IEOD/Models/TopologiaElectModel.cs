using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IEOD.Models
{
    public class TopologiaElectModel : FormatoModel
    {
        public List<TipoDatoTopologia> ListaTipoequipopadre { get; set; }
        public List<EqEquipoDTO> ListaTipoEquipos { get; set; }
        public List<EqEquipoDTO> ListaEquiposPadres { get; set; }
        public List<EqEquipoDTO> ListaEquipos { get; set; }
        public EqEquirelDTO EquipoTopologia { get; set; }
        public List<EqEquirelDTO> ListaEquiposTopologia { get; set; }
        public List<TipoDatoTopologia> ListaTipoExcepcion { get; set; }
        public List<TipoDatoTopologia> ListaAgrupacion { get; set; }

        public int IdTipoExcepcion { get; set; }
        public int IdTipoEquipo { get; set; }
        public int IdEquipo { get; set; }
        public int IdEquipopadre { get; set; }
        public int IdAgrupacion { get; set; }
        public string ActFiltroEquipoPadre { get; set; }
        public string StrListaDatos { get; set; }

        public string Mensaje { get; set; }
        public string Detalle { get; set; }
    }
}