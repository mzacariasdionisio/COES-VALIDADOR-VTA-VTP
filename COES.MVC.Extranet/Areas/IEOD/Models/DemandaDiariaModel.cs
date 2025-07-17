using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.IEOD.Models
{
    public class DemandaDiariaModel : FormatoModel
    {
        public Boolean EnabledTension { get; set; }
        public List<decimal> ListaDemandaDiaria { get; set; }
        public List<Congelado> ListaCongeladoByEnvio { get; set; }
        public List<GenericoDTO> ListaGenSemanas { get; set; }
    }

    public class Congelado
    {
        public int Ptomedicodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public string Rango { get; set; }
        public string Periodo { get; set; }
        public string Empresa { get; set; }
        public string Subestacion { get; set; }
        public string Equipo { get; set; }
        public string Unidad { get; set; }
        public int Justificacion { get; set; }
        public string SubcausacodiDesc { get; set; }
        public string Texto { get; set; }
        public int Valor { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string FechaInicioTexto { get; set; }
        public string FechaFinTexto { get; set; }
        //ASSETEC 201909: Se agrego un nuevo atributo para distinguir el codigo de lectura
        public int Lectcodi { get; internal set; }
    }
}