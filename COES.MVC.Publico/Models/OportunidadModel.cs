using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Publico.Models
{

    public class OportunidadModel
    {
        public List<WbConvocatoriasDTO> Convocatoria { get; set; }
        public string ConvocatoriaDesc { get; set; }
        public string NumeroDeIdentificacion { get; set; }
        public string NombresCompletos { get; set; }
        public string Apellidos { get; set; }
        public string ciudad { get; set; }
        public string Correo { get; set; }
        public string Correoalterno{ get; set; }
        public string Telefonocontacto { get; set; }
        public string descripcion{ get; set; }
        public string nombrearchivo { get; set; }
    }
}
 
                       
                            
                           
                    
                           
                         

