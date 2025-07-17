using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
namespace COES.MVC.Intranet.Areas.Proteccion.Models
{
    public class HistorialCambioSEModel
    {
        public List<EqAreaDTO> ListaArea { get; set; }
        public List<EprAreaDTO> listaSubestacion { get; set; }
        public string fechaCreacion { get; set; }
        public string fechaActualizacion { get; set; }
        public int idZona { get; set; }
        public int idSubestacion { get; set; }
    }

    public class ListadoHistorialCambioSEModel
    {
        public List<EprSubestacionDTO> ListaHistorialCambioSE { get; set; }
    }


    public class HistorialCambioSEEditarModel
    {
        public List<EprAreaDTO> listaSubestacion { get; set; }
        public List<EprProyectoActEqpDTO> listaProyecto { get; set; }
        public int epsubecodi { get; set; }
        public int? areacodi { get; set; }
        public int? epproycodi { get; set; }
        public string epsubemotivo { get; set; }
        public string epsubefecha { get; set; }
        public string epsubememoriacalculo { get; set; }
        public string epsubememoriacalculoTexto { get; set; }
        public string accion { get; set; }
    }

    public class HistorialCambioSEFileModel
    {
        public string nombreArchivo { get; set; }
        public string nombreArchivoTexto { get; set; }
        
        public int estado { get; set; }
    }
}