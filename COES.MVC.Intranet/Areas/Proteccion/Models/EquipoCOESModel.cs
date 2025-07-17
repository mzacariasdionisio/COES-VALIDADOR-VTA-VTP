using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Proteccion.Models
{
    public class EquipoCOESModel
    {
        public List<EprAreaDTO> ListaUbicacion { get; set; }
        public List<EqFamiliaDTO> ListaTipoArea { get; set; }
        public List<List<string>> listaEquipos { get; set; }
        public HandsonModel Handson { get; set; }

    }

    public class ListadoEquipoCOESModel
    {
        public List<EqEquipoDTO> ListaEquipoCOES { get; set; }
    }

    public class EquipoCOESEditarModel
    {
        public int Id { get; set; }
        public int IdEpe { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioNombre { get; set; }

    }

    public class EquipoCOESEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }
}