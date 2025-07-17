using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{
    public class ZonaModel
    {
        public List<EqAreaNivelDTO> ListaNiveles { get; set; }
    }

    public class ListaZonasModel
    {
        public List<EqAreaDTO> ListaZonas { get; set; }
    }

    public class ListaZonasModelxNivel
    {
        public List<EqAreaDTO> ListaZonasxNivel { get; set; }
    }

    public class DetalleZonaModel
    {
        public EqAreaDTO DetalleZona { get; set; }
        public List<EqAreaRelDTO> ListaAreasDeZona { get; set; }
    }

    public class EditarZonaModel
    {
        public int ANivelCodi { get; set; }
        public EqAreaDTO ZonaEditar { get; set; }
        public List<EqAreaDTO> ListaAreasEditar { get; set; }
        public List<EqAreaNivelDTO> ListaNivelesEditar { get; set; }
    }

    public class NuevaZonaModel
    {
        public EqAreaDTO Zona { get; set; }
        public List<EqAreaDTO> ListaAreas { get; set; }
        public List<EqAreaNivelDTO> ListaNiveles { get; set; }
    }
    
}