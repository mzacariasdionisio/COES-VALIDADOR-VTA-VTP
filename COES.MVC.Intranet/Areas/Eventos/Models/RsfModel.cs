using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    /// <summary>
    /// Clase model para el manejo de la matriz rsf
    /// </summary>
    public class RsfModel
    {
        public string Fecha { get; set; }
        public string[][] Datos { get; set; }
        public string[][] DatosBkp { get; set; }
        public int Oper { get; set; }
        public int Longitud { get; set; }
        public decimal RA { get; set; }
        public decimal RaUp { get; set; }
        public decimal RaDown { get; set; }
        public int[] Indices { get; set; }
        public int CountAdicional { get; set; }
        public int Columnas { get; set; }

        public List<EveRsfhoraDTO> ListaHoras { get; set; }
        public List<RsfLimite> ListaLimite { get; set; }
    }

    public class RsfGraficoModel
    {
        public List<RsfGrafico> ListaGrafico { get; set; }
    }

    public class TipoReserva
    {
        public decimal ValorRaUp { get; set; }
        public decimal ValorRaDown { get; set; }
    }
}