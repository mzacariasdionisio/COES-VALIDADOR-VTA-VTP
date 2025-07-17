using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class PlanTransmisionDTO
    {

        public int Plancodi { get; set; }
        public int Pericodi { get; set; }

        public String PeriNombre { get; set; }
        public string Codempresa { get; set; }
        public string Nomempresa { get; set; }
        public DateTime? Fecenvio { get; set; }
        public int Numreg { get; set; }
        public int Planversion { get; set; }
        public string Planestado { get; set; }
        public string Plancumplimiento { get; set; }
        public string Vigente { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
        public string Proycodi { get; set; }
        public string Proynombre { get; set; }
        public string Proyestado { get; set; }
        public DateTime? FechaenvObs { get; set; }
        public string Tiponombre { get; set; }
        public string Tipofinombre { get; set; }
        public string CorreoUsu { get; set; }

        public int ObservPendiente { get; set; }
    }
}
