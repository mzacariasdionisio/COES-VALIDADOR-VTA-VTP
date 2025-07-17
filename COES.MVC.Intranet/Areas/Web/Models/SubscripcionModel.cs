using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Web.Models
{
    /// <summary>
    /// Model para el tratamiento de equivalencia de codigos
    /// </summary>
    public class SubscripcionModel
    {
        public List<WbPublicacionDTO> ListaPublicacion { get; set; }
        public List<WbSubscripcionitemDTO> ListaItems { get; set; }
        public List<WbSubscripcionDTO> ListaSubscripciones { get; set; }
        public WbSubscripcionDTO Entidad { get; set; }
        public int Codigo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Empresa { get; set; }
        public string Publicaciones { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }

    /// <summary>
    /// Model para las publicaciones
    /// </summary>
    public class PublicacionModel
    {
        public List<WbPublicacionDTO> ListaPublicacion { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Correo1 { get; set; }
        public string Correo2 { get; set; }
        public int? IdArea { get; set; }
        public string Estado { get; set; }
        public WbPublicacionDTO Entidad { get; set; }
        public List<FwAreaDTO> ListaAreas { get; set; }
    }     
}