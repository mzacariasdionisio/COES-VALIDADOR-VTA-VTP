using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Proteccion.Models
{
    public class UbicacionCOESModel
    {
        public List<EqTipoareaDTO> ListaTipoArea { get; set; }
        public List<List<string>> listaUbicacion { get; set; }
        public string Fecha { get; set; }
        public int Count { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public List<string> ListaResultado { get; set; }
        public List<int> ListaCount { get; set; }
        public List<EqPropequiDTO> ListaPropiedad { get; set; }
        public HandsonModel Handson { get; set; }
        public List<int> ListaFlagVigenciaCorrecta { get; set; }

    }

    public class UbicacionCOESEditarModel
    {
        public List<EqAreaDTO> ListaZona { get; set; }
        public int IdZona { get; set; }
        public int Areacodi { get; set; }
        public int Epareacodi { get; set; }
        public string Nombre { get; set; }
        public string FlagCambioUbicacion { get; set; }
    }

    public class UbicacionCOESEliminarModel
    {
        public int Estado{ get; set; }
        public string Mensaje { get; set; }
    }

    public class ListadoUbicacionesModel
    {
        public List<EqAreaDTO> ListaUbicaciones { get; set; }
    }
}