using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla VCE_PTOMED_MODOPE
    /// </summary>
    public class VcePtomedModopeDTO
    {
        public DateTime? Pmemopfecmodificacion { get; set; }
        public string Pmemopusumodificacion { get; set; }
        public DateTime Pmemopfeccreacion { get; set; }
        public string Pmemopusucreacion { get; set; }
        public string Pmemopestregistro { get; set; }
        public int? Pmemoporden { get; set; }
        public int Grupocodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Pecacodi { get; set; }

        //Adicional
        public string Gruponomb { get; set; }

    }
}
