using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class LineasFichaBDetDTO
    {
        public int FichaBDetCodi { get; set; }
        public int FichaBCodi { get; set; }
        public int DataCatCodi { get; set; }
        public string Anio { get; set; }
        public decimal Trimestre { get; set; }
        public string Valor { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string NombreProyecto { get; set; }
        public string ProyCodi { get; set; }
        public string ProyNombre { get; set; }
        public DateTime? FecPuestaOpe { get; set; }
    }
}
