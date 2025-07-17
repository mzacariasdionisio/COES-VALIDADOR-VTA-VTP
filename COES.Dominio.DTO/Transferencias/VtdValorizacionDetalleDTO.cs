using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class ValorizacionDTO
    {
        public VtdValorizacionDTO cabecera;
        public List<VtdValorizacionDetalleDTO> detalle;
    }
    


    public class VtdValorizacionDetalleDTO
    {
      

        public int Valdcodi { get; set; }
        public int Valocodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal Valdretiro { get; set; }
        public decimal Valdentrega { get; set; }
        public decimal Valdpfirremun { get; set; }
        public decimal Valddemandacoincidente { get; set; }
        public decimal Valdmoncapacidad { get; set; }
        public decimal Valdpeajeuni { get; set; }
        public decimal Valdfactorp { get; set; }
        public decimal Valdpagoio { get; set; }
        public decimal Valdpagosc { get; set; }
        public decimal Valdfpgm { get; set; }
        public decimal Valdmcio { get; set; }
        public decimal Valdpdsc { get; set; }
        public decimal Valdcargoconsumo { get; set; }
        public decimal Valdaportesadicional { get; set; }
        public string Valdusucreacion { get; set; }
        public DateTime? Valdfeccreacion { get; set; }
        public string Valdusumodificacion { get; set; }
        public DateTime? Valdfecmodificacion { get; set; }

        //Si_empresa
        public string Emprnomb { get; set; }


    }
}
