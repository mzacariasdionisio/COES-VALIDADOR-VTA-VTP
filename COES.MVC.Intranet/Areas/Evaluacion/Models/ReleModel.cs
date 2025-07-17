using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class ReleModel
    {
        public List<EprAreaDTO> ListaSubestacion { get; set; }
        public List<EprEquipoDTO> ListaCelda { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqAreaDTO> ListaArea { get; set; }
        public HandsonModel Handson { get; set; }

    }

    public class ListadoReleModel
    {
        public List<EqEquipoDTO> ListaEquipoCOES { get; set; }
    }

    public class ReleEditarModel
    {
        public int Id { get; set; }
        public int IdEpe { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioNombre { get; set; }

    }

    public class ReleEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }
}