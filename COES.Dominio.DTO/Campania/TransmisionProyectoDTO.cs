using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class TransmisionProyectoDTO
    {
       
        public int Proycodi { get; set; }
        public int Pericodi { get; set; }

        public int Tipocodi { get; set; } 

        public int Plancodi { get; set; }

        public string NomPeri { get; set; }

        public string TipoNombre { get; set; }  
        
        public string Proynombre { get; set; }
        public string Proydescripcion { get; set; }
        public string Proyconfidencial { get; set; }

        public string EmpresaCodi { get; set; }

        public string EmpresaNom { get; set; }

        public string Planestado { get; set; }

        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }

        public int? Tipoficodi { get; set; }
        public string TipofiNom { get; set; }
        public PeriodoDTO Periodo { get; set; }
        
        public int? Areademanda { get; set; }
        public List<DataCatalogoDTO> DataCatalogoDTOs { get; set; }
        public string CorreoUsu { get; set; }

        public string Proyestado { get; set; }

        public string TipoProyecto { get; set; }
        public string TipoSubProyecto { get; set; }
    }
}
