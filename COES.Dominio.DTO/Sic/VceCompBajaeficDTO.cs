using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class VceCompBajaeficDTO
    {
        public string Crcbetipocalc { get; set; }
        public decimal? Crcbecompensacion { get; set; }
        public decimal? Crcbecvt { get; set; }
        public decimal? Crcbecvnc { get; set; }
        public decimal? Crcbecvc { get; set; }
        public decimal? Crcbeconsumo { get; set; }
        public decimal? Crcbepotencia { get; set; }
        public DateTime Crcbehorfin { get; set; }
        public DateTime Crcbehorini { get; set; }
        public int Subcausacodi { get; set; }
        public int Grupocodi { get; set; }
        public int PecaCodi { get; set; } 

        //Adicionales
        public string Emprnomb { get; set; }
        public string Subcausadesc { get; set; }
        public string Gruponomb { get; set; }

        // Inicio de Agregados 31/05/2017 - Sistema de Compensaciones
        public decimal Minimacarga { get; set; }
        public decimal Pruebapr25 { get; set; }
        public decimal Pagocuenta { get; set; }
        public decimal Totalmodo { get; set; }
        public decimal Seguridad { get; set; }
        public decimal Rsf { get; set; }
        public decimal Reservaesp { get; set; }
        public decimal Tension { get; set; }
        public decimal Totalemp { get; set; }
        // Fin de Aregado
    }
}
