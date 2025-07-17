using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.Publicaciones.Models
{
    /// <summary>
    /// Clase para manejo de datos de subscripciones
    /// </summary>
    public class SubscripcionModel
    {
        public List<WbPublicacionDTO> ListaPublicacion { get; set; }
        public string Detalle { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Empresa { get; set; }
        public int Codigo { get; set; }     
    }
}