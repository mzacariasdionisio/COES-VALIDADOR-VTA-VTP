using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class ValorizacionDiariaDTO
    {
        //Valorizacion
        public int Valocodi { get; set; }
        public DateTime Valofecha { get; set; }
        public decimal Valomr { get; set; }
        public decimal Valopreciopotencia { get; set; }
        public decimal Valodemandacoes { get; set; }
        public decimal Valofactorreparto { get; set; }
        public decimal Valoporcentajeperdida { get; set; }
        public decimal Valofrectotal { get; set; }
        public decimal Valootrosequipos { get; set; }
        public decimal Valocostofuerabanda { get; set; }
        public decimal Valoco { get; set; }
        public decimal Valora { get; set; }
        public decimal Valoofmax { get; set; }
        public decimal Valocompcostosoper { get; set; }
        public decimal Valocomptermrt { get; set; }
        public char Valoestado { get; set; }
        public string Valousucreacion { get; set; }
        public DateTime? Valofeccreacion { get; set; }
        public string Valousumodificacion { get; set; }
        public DateTime? Valofecmodificacion { get; set; }

        //ValorizacionDetalle
        public int Valdcodi { get; set; }
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
