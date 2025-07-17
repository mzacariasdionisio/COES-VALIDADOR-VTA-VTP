using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class TipoFichaProyectoDTO
    {
        public int TipoFiCodigo { get; set; }

        public string TipoFiNombre { get; set; }

        public int TipoCodigo { get; set; }

        public int SubTipoCodigo { get; set; }

        public string ContrHtml { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string IndDel { get; set; }

        public int Orden { get; set; }
        public List<HojaFichaDTO> ListaHojaFichas { get; set; }
    }
}
