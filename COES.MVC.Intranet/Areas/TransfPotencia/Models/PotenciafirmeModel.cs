using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class PotenciafirmeModel : BaseModel
    {
        public HandsonModel Handson { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public Boolean EnPlazo { get; set; }
        public string Mes { get; set; }
        public List<MeEnvioDTO> ListaEnvios { get; set; }
        public int IdEnvioLast { get; set; }
        public int IdEnvio { get; set; }
        public string FechaEnvio { get; set; }
        public string Resultado { get; set; }
        public int Nregistros { get; set; }
        public string Fecha { get; set; }
        public List<EqCatpropiedadDTO> ListaEqCatpropiedad { get; set; }
        public List<EqCategoriaDTO> ListaEqCategoria { get; set; }
        public EqCatpropiedadDTO ObjEqcatpropiedad { get; set; }
        public EqCategoriaDTO ObjEqCategoria { get; set; }
        public List<EqCategoriaDetDTO> ListaEqCategoriaDet { get; set; }
        public EqCategoriaDetDTO ObjEqCategoriaDet { get; set; }
    }
}
