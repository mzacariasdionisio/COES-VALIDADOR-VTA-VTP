using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class ExogenasModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        //public PrnExogenamedicionDTO Exomedicion { get; set; }
        //public List<PrnExogenamedicionDTO> ListaExomedicion { get; set; }
        public List<PrnAreamedicionDTO> ListaCiudad { get; set; }
        //public DateTime ParseFecini { get; set; }
        //public DateTime ParseFecfin { get; set; }
        //public DateTime FecAux { get; set; }
        //public string[][] ListaDetalle { get; set; }
        //public string[] Intervalos { get; set; }
        public string[] Ciudades { get; set; }
        //public int CantidadColumnas { get; set; }
        //public int CantidadFilas { get; set; }
        public bool Valido { get; set; }
        public object Datos { get; set; }
    }
}