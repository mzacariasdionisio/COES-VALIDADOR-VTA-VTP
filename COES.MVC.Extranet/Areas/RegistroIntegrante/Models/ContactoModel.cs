using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Models
{
    public class ContactoModel
    {
        public List<SiRepresentanteDTO> ListaContacto { get; set; }
        public int EmpresaId { get; set; }
        public int RpteCodi { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }       
        public string CargoEmpresa { get; set; }
        public string Telefono { get; set; }
        public string TelefonoMovil { get; set; }
        public string CorreoElectronico { get; set; }
    }
}