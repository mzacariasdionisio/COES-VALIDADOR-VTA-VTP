using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class RegHojaASubestDTO
    {
        public int Centralcodi { get; set; }
        public int Proycodi { get; set; }
        public string Nombresubestacion { get; set; }
        public string Tipoproyecto { get; set; }
        public DateTime? Fechapuestaservicio { get; set; }
        public string Empresapropietaria { get; set; }
        public string Sistemabarras { get; set; }
        public int Numtrafos { get; set; }
        public int Numequipos { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
    }
}
